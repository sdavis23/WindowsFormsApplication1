using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;
using WindowsFormsApplication1;

namespace WindowsFormsApplication
{
    public partial class MainForm : Form
    {

        private Communicator email_comm;
        private XmlSerializer serializer;
        private SceneOperator op;

        public MainForm()
        {  
            InitializeComponent();
            initObjects();
            loadControlDefaults();
        }

        private String getXMLFilePath()
        {
            return Application.StartupPath + "AutomationConfig.xml";
        }

        private void initObjects()
        {
            email_comm = new Communicator();
            serializer = new XmlSerializer(email_comm.GetType());
            op = null;
        }

        private void loadControlDefaults()
        {
            String config_path = getXMLFilePath();
            configNullDefaults();

            if (System.IO.File.Exists(config_path) )
            {
                using (StreamReader reader = new StreamReader(config_path))
                {
                    System.Xml.XmlReader xml_reader = System.Xml.XmlReader.Create(new StreamReader(config_path));

                    if (serializer.CanDeserialize(xml_reader))
                    {
                        email_comm = (Communicator)serializer.Deserialize(xml_reader);

                        if (email_comm.Project_Path != null)
                        {
                            txt_proj_dir.Text = email_comm.Project_Path;
                        }

                        if (email_comm.scan_dir != null)
                        {
                            txt_scan_dir.Text = email_comm.scan_dir;
                        }

                        if (email_comm.EmailAddress != null)
                        {
                            EmailBox.Text = email_comm.EmailAddress;
                        }

                        xml_reader.Close();
                        xml_reader.Dispose();

                        reader.Close();
                        reader.Dispose();

                    }

                }
            }
           
            
           
        }

        private void configNullDefaults()
        {
            txt_proj_dir.Text = "D:\\";
            txt_scan_dir.Text = "D:\\";
            op = null;
        }

        /// <summary>
        /// Ensures the project file directory entered is in fact vald.
        /// Rules:
        ///    Must end with .lsproj
        /// </summary>
        /// <param name="attempted_file_path">The file path the user is trying to use.</param>
        /// <returns></returns>
        private bool checkProjectFile(String attempted_file_path)
        {
            string[] split_list = attempted_file_path.Split('.');
            return split_list[split_list.Length - 1].Equals("lsproj");
        }
     
        private void button1_Click(object sender, EventArgs e)
        {

            startOperatorProcess(OPPROC.PREPROCESS_SCANS);

            
        }

        private void startOperatorProcess(OPPROC proc)
        {
            // make sure the email is correct
            if (email_comm == null)
            {

                try
                {
                    email_comm = new Communicator(EmailBox.Text.Trim());
                }

                catch(Exception e)
                {
                    MessageBox.Show(e.Message);
                    return;
                }

              
            }

            email_comm.EmailAddress = EmailBox.Text;

            if (checkProjectFile(txt_proj_dir.Text))
            {
                try
                {

                    if (op == null)
                    {
                        op = new SceneOperator(check_trial.Checked, txt_scan_dir.Text, txt_proj_dir.Text);
                    }

                    switch(proc)
                    {
                        case OPPROC.APPLY_FILTER_EXPORT:
                            op.applyFilterExport();
                            break;

                        case OPPROC.PREPROCESS_SCANS:
                            op.preProcessScans();
                            break;
                    }

                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                // it's made it to here, so we can assume the process is done and we're ready to send an email
                email_comm.sendMessage(proc);
                

                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(getXMLFilePath()))
                {
                    serializer.Serialize(writer, email_comm);
                    writer.Dispose();
                    writer.Close();
                }

            }

            else
            {

                MessageBox.Show("Invalid SCENE project selected");
            }

        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        { 
            email_comm.EmailAddress = EmailBox.Text.Trim();
          
            if (op != null)
            {
                op.clean_exit();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void btn_proj_dir_Click(object sender, EventArgs e)
        {
            OpenFileDialog proj_dir_dialog = new OpenFileDialog();
            proj_dir_dialog.Title = "Choose SCENE Project";
            proj_dir_dialog.ShowDialog();
            

            if (proj_dir_dialog.FileName.Length > 0)
            {
                txt_proj_dir.Text = proj_dir_dialog.FileName;
                
            }

            email_comm.Project_Path = proj_dir_dialog.FileName;

        }

        private void btn_scan_dir_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog proj_dir_dialog = new FolderBrowserDialog();
            proj_dir_dialog.ShowDialog();  

            if(proj_dir_dialog.SelectedPath.Length > 0)
            {
                txt_scan_dir.Text = proj_dir_dialog.SelectedPath;
                email_comm.scan_dir = proj_dir_dialog.SelectedPath;
            } 
        }

        private void button2_Click(object sender, EventArgs e)
        {

            startOperatorProcess(OPPROC.APPLY_FILTER_EXPORT);
        }
    }
}
