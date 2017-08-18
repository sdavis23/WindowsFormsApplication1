using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UIA;
using UIAutomationClient;
using System.Diagnostics;
using Microsoft.Win32;
using TestStack.White;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.MenuItems;
using TestStack.White.UIItems.TreeItems;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems.WindowStripControls;
using TestStack.White.UIItems.ListBoxItems;
using System.Windows;
using System.Windows.Input;
using System.Runtime.InteropServices;

/*
 * Responsible for operating the scene
 * program and for monitoring processes.
 * 
 * Another program should be responsible for opening the Operator.
 * 
 */
namespace WindowsFormsApplication
{

    public enum OPPROC
    {
        APPLY_FILTER_EXPORT,
        PREPROCESS_SCANS,
        REGISTER
    }

    public class RegistrationControl
    {
        public int min_mean;
        public int max_mean;
        public double avg_subsample;
        public int num_iterations;
        public double max_searchdist;
    }
   

    class SceneOperator
    {


        private Window scene_window;
        private TestStack.White.Application app;
        private const String TREE_TITLE = "Scans";
        private const String FILE_MODAL_TITLE = "Shell Folder View";
        private const String FILE_DIALOG_TITLE = "Import Scan Data";
        private const String PRE_PROC_MODAL_TITLE = "/Scans";
        private String RAW_SCAN_PATH = null;
        private String project_path  = null;
        private bool is_saving = false;
        private bool trial = false;

        [DllImport("kernel32.dll")]
        public static extern void GetSystemTime(ref SYSTEMTIME lpSystemTime);
        [DllImport("kernel32.dll")]
        public static extern bool SetSystemTime(ref SYSTEMTIME lpSystemTime);

        // the indices for the point cloud selections in each of the boxes.
        int TARGET_INDEX = 0;
        int TOP_VIEW_INDEX = 1;
        int CLOUD_TO_CLOUD_INDEX = 2;

        private enum PLACEMENT_TYPE
        {
            TARGET,
            TOP_VIEW,
            CLOUD_TO_CLOUD
        }

        // defines the signature for a function that traces a menu and \
        // opens a modal at the end of the apth
        // path: is the path to trace the menu
        // Returns: the window itself
        private delegate void MenuTrace(params string[] path);

        private Process Mainprocess;

        public SceneOperator()
        {
        }

        /// <summary>
        /// Constructs the object responsible for operating scene.
        /// 
        /// </summary>
        /// <param name="is_trial">Tells uus whether or not the scene you are using is a trial</param>
        /// <param name="proj_path">The path for the scanning project</param>
        /// <param name="scan_path">The path for the raw scans</param>
        public SceneOperator(bool is_trial, String scan_path, String proj_path)
        {
            this.trial = is_trial;
            RAW_SCAN_PATH = scan_path;
            project_path = proj_path;
            openScene(proj_path);
            
        }

        private void openScene(String proj_path)
        {
            // save the system time
            SYSTEMTIME current_time = new SYSTEMTIME();
            GetSystemTime(ref current_time);
           
            current_time.wMonth = (ushort)(current_time.wMonth - 1);
            //set the system time to one month ago
            SetSystemTime(ref current_time);

            // start the process
            initProcess(System.Diagnostics.Process.Start(proj_path));

            current_time.wMonth = (ushort)(current_time.wMonth + 1);
            //reset the system time
            SetSystemTime(ref current_time);

        }

        public void setScanDir(String scan_path)
        {
            RAW_SCAN_PATH = scan_path;
        }

        public void setProjDir(String proj_path)
        {

            project_path = proj_path;
        }

        private void initProcess(Process p)
        {
            Mainprocess = p;
            app = TestStack.White.Application.Attach(p.Id);
            // start the initial form
            scene_window = app.GetWindows().ToArray()[0];

            if(trial)
            {
                scene_window.Keyboard.PressSpecialKey(TestStack.White.WindowsAPI.KeyboardInput.SpecialKeys.RETURN);
            }

            
           
        }

        public void assignProcess(Process p)
        {
            initProcess(p);
        }

      /// <summary>
      /// Opens the file selector for opening a project or Workspace in SCENE
      /// </summary>
      /// <returns></returns>
        private Window openProjectSelector()
        {
            
            return openMainModal("Open Workspace", "File", "Open...");

        }
        /// <summary>
        /// Does the Preprocessing of the scans including:
        ///     importing from the file path given by the constant RAW_SCAN_PATH
        ///     the drawing and the initial save of the scans and the 
        ///     second save to ensure the changes actually have an effect
        /// </summary>
        public void preProcessScans()
       {
            if(RAW_SCAN_PATH == null)
            {
                throw new Exception("Raw Scan File Path is not given");

            }

            else if(project_path == null)
            {
                throw new Exception("SCENE Project File Path is not given");
            }
            else
            {

                try
                {
                    commitPreProcessScans();
                }
                catch(Exception e)
                {
                    throw e;
                }
            }
           
        }


        /// <summary>
        /// Does the actual operations surrounding the preprocessing of scans.
        /// Including: Saving and finding the targets, and the user of the preprocessing button
        /// in the scans.
        /// </summary>
        private void commitPreProcessScans()
        {
            try
            {
                Window file_dialog = openImport();

                changeFilePath(RAW_SCAN_PATH, file_dialog);

                Image folder = clickFirstFolder(getShellPanel(file_dialog));
                selectAll();

                file_dialog.Mouse.DragAndDrop(folder, scene_window.Get(SearchCriteria.ByText("Structure")));
                file_dialog.Close();

                save();
                findSpheres();
                startPreprocess();
                save(); 

            }

            catch(Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Registers the scan, using an array of objects that control
        /// what the numbers become if the mean falls within
        /// 
        /// At this point:
        ///     assumes the intervals in the array to be mutually exclusive.
        ///     and the intervals are closed on both ends.
        /// </summary>
        /// <param name="control_array"></param>
        public void registerScans(RegistrationControl[] control_array)
        {

           

            placeScanConfig();
            scene_window.Keyboard.PressSpecialKey(TestStack.White.WindowsAPI.KeyboardInput.SpecialKeys.RETURN);
            double meanVal = getScanMeanVal(); 

            int current_index = 0;
            bool set_reg_values = false;

            while(current_index < control_array.Length && !set_reg_values )
            {
                RegistrationControl current_control = control_array[current_index];

                if (meanVal > current_control.min_mean && meanVal < current_control.max_mean)
                {
                  setRegistrationValues(current_control.avg_subsample, current_control.num_iterations, current_control.max_searchdist);
                    waitForSingleWindow();
                  set_reg_values = true;
                }

                current_index++;
            }

            save();

            int test = 934;

        }

        private void pressKeyNTimes(Window w, int n, TestStack.White.WindowsAPI.KeyboardInput.SpecialKeys s)
        {
            int current_val = 1;
            while(current_val <= n)
            {
                w.Keyboard.PressSpecialKey(s);
                current_val++;
            }
        }

        private double getScanMeanVal()
        {

         
            while (!app.GetWindows().Exists(w => w.Name.Trim().Equals("/Scans/ScanManager")))
            {
                System.Threading.Thread.Sleep(200);
            }

            Window control_window = app.GetWindows().ToArray()[1];
            control_window.Get(SearchCriteria.ByText("Scan Point Tensions")).Click();
             
            String mean_text = control_window.Get<TextBox>("Mean:").Text;
            control_window.Close();

            return double.Parse(mean_text);

        }

        private void placeScanConfig()
        {

            scanRightClick();
            Window place_scans = openPlaceScanWindow();
            ComboBox c = place_scans.Get<ComboBox>();
            c.Items[TOP_VIEW_INDEX].Click();

            place_scans.Get(SearchCriteria.ByText("General")).Click();
            place_scans.Get<CheckBox>("GPS").Checked = false;

        }

        private void setRegistrationValues(double avg_subsample, int max_iterations, double max_dist)
        {

            scanRightClick();
            Window place_scans = openPlaceScanWindow();
            ComboBox c = place_scans.Get<ComboBox>();
            c.Items[CLOUD_TO_CLOUD_INDEX].Click();
            double current_value = double.Parse(place_scans.Get<TextBox>("Average subsampling point distance:").Text);
            double diff = Math.Round(current_value - avg_subsample, 3);

            string text_box_name = "Average subsampling point distance:";

            if(diff > 0)
            {
                pressKeyUntil(place_scans, text_box_name,  avg_subsample.ToString(), 
                                                           TestStack.White.WindowsAPI.KeyboardInput.SpecialKeys.LEFT);
            }
            else
            {
              pressKeyUntil(place_scans, text_box_name,   avg_subsample.ToString(),
                                                          TestStack.White.WindowsAPI.KeyboardInput.SpecialKeys.RIGHT);
            }

            place_scans.Get<TextBox>("Maximum number of iterations:").Text = max_iterations.ToString();
            place_scans.Get<TextBox>("Maximum search distance:").Text = max_dist.ToString();

            place_scans.Keyboard.PressSpecialKey(TestStack.White.WindowsAPI.KeyboardInput.SpecialKeys.RETURN);
           
        }

        private void pressKeyUntil(Window w, string textBoxName,   string stopping_string, 
                                                                    TestStack.White.WindowsAPI.KeyboardInput.SpecialKeys key)
        {
            string subsample = w.Get<TextBox>(textBoxName).Text;

            while (!subsample.Equals(stopping_string))
            {
                w.Keyboard.PressSpecialKey(key);
                subsample = w.Get<TextBox>(textBoxName).Text;
            }
        }

        private Window openPlaceScanWindow()
        {

            clickPopUpMenuPath("Operations", "Registration", "Place Scans");
            Window place_scans = null;

            while (app.GetWindows().ToArray().Length < 2)
            {
                System.Threading.Thread.Sleep(100);
            }

            place_scans = app.GetWindows().ToArray()[1];

            return place_scans;
        }

        /// <summary>
        /// Finds the targets: spheres in each scan shown in any given tree.
        /// </summary>
        private void findSpheres()
        {
            TestStack.White.UIItems.TreeItems.TreeNode tNode = getScanTreeViewer();
            TreeNode[] nodes = tNode.Nodes.ToArray();
            
            foreach(TreeNode child_node in nodes)
            {
                child_node.RightClick();
                clickPopUpMenuPath("Operations", "Find Objects", "Spheres");
                System.Threading.Thread.Sleep(500);
                waitForSingleWindow();
            }

        }

        public void applyFilterExport()
        {
            filterScans();
            applyPictures();
            exportScans();
        }

        public void applyPictures()
        {
            scanRightClick();
            clickPopUpMenuPath("Operations", "Color/Pictures", "Apply Pictures");
            waitForSingleWindow();
        }

        public void exportScans()
        {
            scanRightClick();
            clickPopUpMenuPath("Import/Export", "Export Scans - Ordered");
            hitEnterOnWindow("Export Scan Points");
            System.Threading.Thread.Sleep(500);
            waitForSingleWindow();
        }

        private void hitEnterOnWindow(String windowTitle)
        {
            app.GetWindows().Where(item => item.Name.Equals(windowTitle)).ToArray()[0]
                            .Keyboard.PressSpecialKey(TestStack.White.WindowsAPI.KeyboardInput.SpecialKeys.RETURN);
        }

        public void clean_exit()
        {
            app.Close();
        }

        /// <summary>
        /// Runs the colour contrast filter on all of the currently loaded scans on
        /// the project
        /// </summary>
        public void filterScans()
        {
            scanRightClick();
            clickPopUpMenuPath("Operations", "Color/Pictures", "Color Contrast Filter");
        }

        /// <summary>
        /// Does a complete save of the current SCENE project
        /// including waiting for the save and waiting
        /// for the save confirmation box.
        /// </summary>
        private void save()
        {
            waitForSave();
            waitForSaveConfirm();

        }

        /// <summary>
        /// To be used after waitForSave()
        /// Waits for the confirmation Modal Window with the title SCENE to come online
        /// then hits enter to close out of the window.
        /// Needed: because SCENE is waiting for input and IDLE even though we are still waiting for a save
        /// </summary>
        private void waitForSaveConfirm()
        {
            while (true)
            {
                try
                {
                    var dialog_windows = app.GetWindows().Where(item => item.Title.Equals("SCENE"));


                    if (dialog_windows.Count() > 0)
                    {
                        dialog_windows.ToArray()[0].Keyboard.PressSpecialKey(TestStack.White.WindowsAPI.KeyboardInput.SpecialKeys.RETURN);
                        break;
                    }
                    continue;
                }
                catch
                {
                    continue;
                }

            }

        }

        /// <summary>
        /// Waits until there is only one window on the screen.
        /// </summary>
        private void waitForSingleWindow()
        {
            while (true)
            {
                try
                {
                    

                    if (singleWindow())
                    {
                        break;
                    }

                    continue;
                }
                catch
                {
                    continue;
                }

            }


        }

        private bool singleWindow()
        {
            return app.GetWindows().Count == 1 || scene_window.ModalWindows().Count == 0;
        }

        /// <summary>
        /// Waits for the save,
        /// Essentially this just waits until the first time
        /// SCENE comes back with a wait for Input Idle signal.
        /// </summary>
        private void waitForSave()
        { 
            while (true)
            {
                try
                {
                    sceneSave();
                    break;
                }
                catch
                {
                    continue;
                }

            }

            is_saving = false;
        }

        /// <summary>
        /// Starts the preprocessing of the scans; from a right click, and Preprocess Scans...
        /// </summary>
        private void startPreprocess()
        {
            scanRightClick();
            clickPopUpMenuPath("Operations", "Preprocessing", "Preprocess Scans...");
            scene_window.ModalWindows().ToArray()[1].Keyboard.PressSpecialKey(TestStack.White.WindowsAPI.KeyboardInput.SpecialKeys.RETURN);
            waitForSaveConfirm();
        }

        // presses enter on the main window.
        private void hitEnter()
        {
            scene_window.Keyboard.PressSpecialKey(TestStack.White.WindowsAPI.KeyboardInput.SpecialKeys.RETURN);
        }

    /// <summary>
    /// Given a filePath and a file_dialog, changes the currently filePath on that file_dialog.
    /// </summary>
    /// <param name="filePath">the file path we are chaning to </param>
    /// <param name="file_dialog">the dialog we are operating.</param>
       private void changeFilePath(String filePath, Window file_dialog)
       {

            TestStack.White.UIItems.TextBox fileSearchBox =
                         (TestStack.White.UIItems.TextBox)file_dialog.Items.Where(item => item.Name.Equals("File name:") &&  
                                                                                   item is TestStack.White.UIItems.TextBox).ToArray()[0];

            TestStack.White.UIItems.Button openButton =
                (TestStack.White.UIItems.Button)getFirstInstance("Open", file_dialog);

         
            fileSearchBox.SetValue(filePath);
            openButton.Click();

       }

       /// <summary>
       /// Given a gile dialog retrieves the panel where the clickable folders actually reside
       /// </summary>
       /// <param name="file_dialog"></param>
       /// <returns></returns>
       private TestStack.White.UIItems.Panel getShellPanel(Window file_dialog)
       {
            return (TestStack.White.UIItems.Panel)getFirstInstance(FILE_MODAL_TITLE, file_dialog);
       }


        /// <summary>
        /// Given a shell view of the file dialog blindly clicks the first folder.
        /// </summary>
        /// <param name="shell_view"></param>
        /// <returns></returns>
        private Image clickFirstFolder(TestStack.White.UIItems.Panel shell_view)
        {
            IUIItem[] folder_list =
                   shell_view.Items.Where(item => item is Image).ToArray();

            Image folder = null ;

            int i = 0;
            while(folder == null)
            { 
                if(!folder_list[i].IsOffScreen)
                {
                    folder = (Image)folder_list[i];
                }

                i++;
            }

           
            folder.Click();
            return folder;
        }

        /// <summary>
        /// Selects all of the files in the file dialog. Actually Just presses CTRL + A
        /// </summary>
        private void selectAll()
        {
            // press ctrl-a to select all files
            scene_window.Keyboard.HoldKey(TestStack.White.WindowsAPI.KeyboardInput.SpecialKeys.CONTROL);
            scene_window.Keyboard.Enter("a");
            scene_window.Keyboard.LeaveKey(TestStack.White.WindowsAPI.KeyboardInput.SpecialKeys.CONTROL);
        }

        /// <summary>
        /// Presses the scene button and presses enter
        /// </summary>
       private void sceneSave()
        {

            if (!is_saving)
            {
                is_saving = true;
                Window saveModal = openMainModal("Share Changes", "File", "Save");
                scene_window.Keyboard.PressSpecialKey(TestStack.White.WindowsAPI.KeyboardInput.SpecialKeys.RETURN);
            }

        }

        /// <summary>
        /// Opens the import dialog
        /// </summary>
        /// <returns></returns>
       private Window openImport()
       {
            // click the import button
            return openMainModal(FILE_DIALOG_TITLE, "File", "Import...");
       }

        /// <summary>
        /// Opens a modal with modalTitle from the path in the main windpw
        /// </summary>
        /// <param name="modalTitle">The title of the modal we're opening</param>
        /// <param name="path">the path of the menubar to get to the modal.</param>
        /// <returns></returns>
        private Window openMainModal(String modalTitle, params string[] path)
        {
            return openModal(modalTitle, clickMainMenuPath, path);
        }

        /// <summary>
        /// Opens a modal with title modalTitle on the a popup menu
        /// </summary>
        /// <param name="modalTitle">the title of the modal we're interested in</param>
        /// <param name="path">the path on the popup menu</param>
        /// <returns></returns>
        private Window openPopUpMenuModal(String modalTitle, params string[] path)
        {
            return openModal(modalTitle, clickPopUpMenuPath, path);
        }

        private Window openFirstPopUpMenuModal(params string[] path)
        {
            return openFirstModal(clickPopUpMenuPath, path);
        }

        private void clickPopUpMenuPath(params string[] path)
        {
            scene_window.PopupMenu(path).Click();
        }

        private void clickMainMenuPath(params string[] path)
        {
            MenuBar menu = scene_window.MenuBar;

            menu.MenuItem(path).Click();
        }

        private Window openModal(String modalTitle, MenuTrace openModalFromMenu, params string[] path)
        {
            openModalFromMenu(path);
        
            return scene_window.ModalWindow(modalTitle);
        }
        
        private Window openFirstModal(MenuTrace openModalFromMenu, params string[] path)
        {

            openModalFromMenu(path);
            return scene_window.ModalWindows().ToArray()[0];
        }

        private TestStack.White.UIItems.TreeItems.TreeNode getScanTreeViewer()
        {
          return  scene_window.Get<TestStack.White.UIItems.TreeItems.TreeNode>(TREE_TITLE);
        }

        /*
         * Performs a right click on the Scene tree node title Scans
         * in the main window.
         */
        private TestStack.White.UIItems.TreeItems.TreeNode scanRightClick()
        {
           TestStack.White.UIItems.TreeItems.TreeNode tNode = getScanTreeViewer();                             
           tNode.RightClick();

            return tNode;
        }

        private IUIItem getFirstInstance(String itemName, Window w)
        {
            UIItemCollection c = w.Items;
            return w.Items.Where(item => item.Name.Equals(itemName)).ToArray()[0];
        }

    }
}
