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
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(501, 201);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(154, 31);
            this.button1.TabIndex = 0;
            this.button1.Text = "Preprocess Scans";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // EmailBox
            // 
            this.EmailBox.Location = new System.Drawing.Point(228, 23);
            this.EmailBox.Name = "EmailBox";
            this.EmailBox.Size = new System.Drawing.Size(181, 22);
            this.EmailBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "E-mail:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 201);
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
            this.label2.Location = new System.Drawing.Point(20, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "SCENE Project:";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 105);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 17);
            this.label3.TabIndex = 6;
            this.label3.Text = "Raw Scans:";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // btn_proj_dir
            // 
            this.btn_proj_dir.Location = new System.Drawing.Point(501, 66);
            this.btn_proj_dir.Name = "btn_proj_dir";
            this.btn_proj_dir.Size = new System.Drawing.Size(154, 23);
            this.btn_proj_dir.TabIndex = 9;
            this.btn_proj_dir.Text = "SCENE Project";
            this.btn_proj_dir.UseVisualStyleBackColor = true;
            this.btn_proj_dir.Click += new System.EventHandler(this.btn_proj_dir_Click);
            // 
            // btn_scan_dir
            // 
            this.btn_scan_dir.Location = new System.Drawing.Point(501, 99);
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
            this.check_trial.Location = new System.Drawing.Point(28, 153);
            this.check_trial.Name = "check_trial";
            this.check_trial.Size = new System.Drawing.Size(118, 21);
            this.check_trial.TabIndex = 12;
            this.check_trial.Text = "Trial Version?";
            this.check_trial.UseVisualStyleBackColor = true;
            // 
            // txt_proj_dir
            // 
            this.txt_proj_dir.Enabled = false;
            this.txt_proj_dir.Location = new System.Drawing.Point(228, 66);
            this.txt_proj_dir.Name = "txt_proj_dir";
            this.txt_proj_dir.Size = new System.Drawing.Size(181, 22);
            this.txt_proj_dir.TabIndex = 13;
            // 
            // txt_scan_dir
            // 
            this.txt_scan_dir.Enabled = false;
            this.txt_scan_dir.Location = new System.Drawing.Point(228, 105);
            this.txt_scan_dir.Name = "txt_scan_dir";
            this.txt_scan_dir.Size = new System.Drawing.Size(181, 22);
            this.txt_scan_dir.TabIndex = 14;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(678, 257);
            this.Controls.Add(this.txt_scan_dir);
            this.Controls.Add(this.txt_proj_dir);
            this.Controls.Add(this.check_trial);
            this.Controls.Add(this.btn_scan_dir);
            this.Controls.Add(this.btn_proj_dir);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.EmailBox);
            this.Controls.Add(this.button1);
            this.Name = "MainForm";
            this.Text = "SCENE Automation";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
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
    }
}

