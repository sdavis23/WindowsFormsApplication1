namespace WindowsFormsApplication
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.button1 = new System.Windows.Forms.Button();
            this.EmailBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btn_proj_dir = new System.Windows.Forms.Button();
            this.btn_scan_dir = new System.Windows.Forms.Button();
            this.check_trial = new System.Windows.Forms.CheckBox();
            this.txt_proj_dir = new System.Windows.Forms.TextBox();
            this.txt_scan_dir = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.mean = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.avg_subsample = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.num_iterations = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.max_search = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(434, 400);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(154, 31);
            this.button1.TabIndex = 0;
            this.button1.Text = "Preprocess Scans";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // EmailBox
            // 
            this.EmailBox.Location = new System.Drawing.Point(124, 21);
            this.EmailBox.Name = "EmailBox";
            this.EmailBox.Size = new System.Drawing.Size(181, 22);
            this.EmailBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "E-mail:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(30, 398);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(153, 31);
            this.button2.TabIndex = 4;
            this.button2.Text = "Apply/Filter/Export";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 94);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "SCENE Project:";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 159);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 17);
            this.label3.TabIndex = 6;
            this.label3.Text = "Raw Scans:";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // btn_proj_dir
            // 
            this.btn_proj_dir.Location = new System.Drawing.Point(329, 88);
            this.btn_proj_dir.Name = "btn_proj_dir";
            this.btn_proj_dir.Size = new System.Drawing.Size(154, 23);
            this.btn_proj_dir.TabIndex = 9;
            this.btn_proj_dir.Text = "SCENE Project";
            this.btn_proj_dir.UseVisualStyleBackColor = true;
            this.btn_proj_dir.Click += new System.EventHandler(this.btn_proj_dir_Click);
            // 
            // btn_scan_dir
            // 
            this.btn_scan_dir.Location = new System.Drawing.Point(329, 159);
            this.btn_scan_dir.Name = "btn_scan_dir";
            this.btn_scan_dir.Size = new System.Drawing.Size(154, 23);
            this.btn_scan_dir.TabIndex = 10;
            this.btn_scan_dir.Text = "Pick Raw Scan Folder";
            this.btn_scan_dir.UseVisualStyleBackColor = true;
            this.btn_scan_dir.Click += new System.EventHandler(this.btn_scan_dir_Click);
            // 
            // check_trial
            // 
            this.check_trial.AutoSize = true;
            this.check_trial.Location = new System.Drawing.Point(30, 341);
            this.check_trial.Name = "check_trial";
            this.check_trial.Size = new System.Drawing.Size(118, 21);
            this.check_trial.TabIndex = 12;
            this.check_trial.Text = "Trial Version?";
            this.check_trial.UseVisualStyleBackColor = true;
            // 
            // txt_proj_dir
            // 
            this.txt_proj_dir.Enabled = false;
            this.txt_proj_dir.Location = new System.Drawing.Point(124, 89);
            this.txt_proj_dir.Name = "txt_proj_dir";
            this.txt_proj_dir.Size = new System.Drawing.Size(181, 22);
            this.txt_proj_dir.TabIndex = 13;
            // 
            // txt_scan_dir
            // 
            this.txt_scan_dir.Enabled = false;
            this.txt_scan_dir.Location = new System.Drawing.Point(124, 159);
            this.txt_scan_dir.Name = "txt_scan_dir";
            this.txt_scan_dir.Size = new System.Drawing.Size(181, 22);
            this.txt_scan_dir.TabIndex = 14;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(236, 398);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(143, 33);
            this.button3.TabIndex = 15;
            this.button3.Text = "Registration";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(26, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(562, 283);
            this.tabControl1.TabIndex = 16;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btn_proj_dir);
            this.tabPage1.Controls.Add(this.btn_scan_dir);
            this.tabPage1.Controls.Add(this.txt_proj_dir);
            this.tabPage1.Controls.Add(this.txt_scan_dir);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.EmailBox);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(554, 254);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "General";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dataGridView1);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(554, 254);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Top View Config";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.mean,
            this.avg_subsample,
            this.num_iterations,
            this.max_search});
            this.dataGridView1.Location = new System.Drawing.Point(0, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(545, 245);
            this.dataGridView1.TabIndex = 0;
            // 
            // mean
            // 
            this.mean.HeaderText = "Mean";
            this.mean.Name = "mean";
            // 
            // avg_subsample
            // 
            this.avg_subsample.HeaderText = "Avg Sub Sample";
            this.avg_subsample.Name = "avg_subsample";
            // 
            // num_iterations
            // 
            this.num_iterations.HeaderText = "# of Iterations";
            this.num_iterations.Name = "num_iterations";
            // 
            // max_search
            // 
            this.max_search.HeaderText = "Max Search Distance";
            this.max_search.Name = "max_search";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(648, 448);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.check_trial);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "SCENE Automation";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox EmailBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_proj_dir;
        private System.Windows.Forms.Button btn_scan_dir;
        private System.Windows.Forms.CheckBox check_trial;
        private System.Windows.Forms.TextBox txt_proj_dir;
        private System.Windows.Forms.TextBox txt_scan_dir;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn mean;
        private System.Windows.Forms.DataGridViewTextBoxColumn avg_subsample;
        private System.Windows.Forms.DataGridViewTextBoxColumn num_iterations;
        private System.Windows.Forms.DataGridViewTextBoxColumn max_search;
    }
}

