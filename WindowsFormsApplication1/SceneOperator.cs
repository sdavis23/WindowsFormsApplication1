﻿using System;
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

            initProcess(System.Diagnostics.Process.Start(proj_path));
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

                startPreprocess();
                save();

            }

            catch(Exception e)
            {
                throw e;
            }
        }

        public void applyFilterExport()
        {
            filterScans();
            applyPictures();
            
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
            clickPopUpMenuPath("Operations", "Import/Export", "Export Scans - Ordered");
            waitForSingleWindow();
        }

        public void clean_exit()
        {
            app.Kill();
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
                    if (app.GetWindows().Where(item => item.Title.Equals("SCENE")).Count() > 0)
                    {
                        scene_window.Keyboard.PressSpecialKey(TestStack.White.WindowsAPI.KeyboardInput.SpecialKeys.RETURN);
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
                    

                    if (app.GetWindows().Count == 1)
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