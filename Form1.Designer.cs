using System.Configuration;

namespace LogReader
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.ExportFolder = new System.Windows.Forms.TextBox();
            this.SelectFolder1 = new System.Windows.Forms.FolderBrowserDialog();
            this.ExportFolderButton = new System.Windows.Forms.Button();
            this.ReadPathButton = new System.Windows.Forms.Button();
            this.ReadPath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.BackupButton = new System.Windows.Forms.Button();
            this.BackupFolder = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TemplateBox = new System.Windows.Forms.ComboBox();
            this.Logfile = new System.Windows.Forms.Label();
            this.PasswordBox = new System.Windows.Forms.TextBox();
            this.Lockbutton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.StartButton = new System.Windows.Forms.Button();
            this.Watcher1 = new System.IO.FileSystemWatcher();
            this.ResetButton = new System.Windows.Forms.Button();
            this.LastTimeLabel = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.PassBox = new System.Windows.Forms.CheckBox();
            this.FailBox = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Watcher1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 84);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Save Folder";
            // 
            // ExportFolder
            // 
            this.ExportFolder.Enabled = false;
            this.ExportFolder.Location = new System.Drawing.Point(125, 81);
            this.ExportFolder.Name = "ExportFolder";
            this.ExportFolder.Size = new System.Drawing.Size(275, 23);
            this.ExportFolder.TabIndex = 1;
            // 
            // ExportFolderButton
            // 
            this.ExportFolderButton.Enabled = false;
            this.ExportFolderButton.Location = new System.Drawing.Point(406, 81);
            this.ExportFolderButton.Name = "ExportFolderButton";
            this.ExportFolderButton.Size = new System.Drawing.Size(25, 23);
            this.ExportFolderButton.TabIndex = 2;
            this.ExportFolderButton.Text = "...";
            this.ExportFolderButton.UseVisualStyleBackColor = true;
            this.ExportFolderButton.Click += new System.EventHandler(this.ExportFolderButton_Click);
            // 
            // ReadPathButton
            // 
            this.ReadPathButton.Enabled = false;
            this.ReadPathButton.Location = new System.Drawing.Point(406, 52);
            this.ReadPathButton.Name = "ReadPathButton";
            this.ReadPathButton.Size = new System.Drawing.Size(25, 23);
            this.ReadPathButton.TabIndex = 5;
            this.ReadPathButton.Text = "...";
            this.ReadPathButton.UseVisualStyleBackColor = true;
            this.ReadPathButton.Click += new System.EventHandler(this.ReadPath_Click);
            // 
            // ReadPath
            // 
            this.ReadPath.Enabled = false;
            this.ReadPath.Location = new System.Drawing.Point(125, 52);
            this.ReadPath.Name = "ReadPath";
            this.ReadPath.Size = new System.Drawing.Size(275, 23);
            this.ReadPath.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "Read Folder";
            // 
            // BackupButton
            // 
            this.BackupButton.Enabled = false;
            this.BackupButton.Location = new System.Drawing.Point(406, 110);
            this.BackupButton.Name = "BackupButton";
            this.BackupButton.Size = new System.Drawing.Size(25, 23);
            this.BackupButton.TabIndex = 8;
            this.BackupButton.Text = "...";
            this.BackupButton.UseVisualStyleBackColor = true;
            this.BackupButton.Click += new System.EventHandler(this.BackupButton_Click);
            // 
            // BackupFolder
            // 
            this.BackupFolder.Enabled = false;
            this.BackupFolder.Location = new System.Drawing.Point(125, 110);
            this.BackupFolder.Name = "BackupFolder";
            this.BackupFolder.Size = new System.Drawing.Size(275, 23);
            this.BackupFolder.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 114);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 15);
            this.label3.TabIndex = 6;
            this.label3.Text = "Extract Backup";
            // 
            // TemplateBox
            // 
            this.TemplateBox.Enabled = false;
            this.TemplateBox.FormattingEnabled = true;
            this.TemplateBox.Location = new System.Drawing.Point(125, 23);
            this.TemplateBox.Name = "TemplateBox";
            this.TemplateBox.Size = new System.Drawing.Size(121, 23);
            this.TemplateBox.TabIndex = 9;
            this.TemplateBox.SelectedIndexChanged += new System.EventHandler(this.TemplateBox_SelectedIndexChanged);
            // 
            // Logfile
            // 
            this.Logfile.AutoSize = true;
            this.Logfile.Location = new System.Drawing.Point(14, 141);
            this.Logfile.Name = "Logfile";
            this.Logfile.Size = new System.Drawing.Size(12, 15);
            this.Logfile.TabIndex = 11;
            this.Logfile.Text = "-";
            // 
            // PasswordBox
            // 
            this.PasswordBox.Location = new System.Drawing.Point(458, 23);
            this.PasswordBox.Name = "PasswordBox";
            this.PasswordBox.PasswordChar = '*';
            this.PasswordBox.Size = new System.Drawing.Size(121, 23);
            this.PasswordBox.TabIndex = 12;
            this.PasswordBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PasswordBox_KeyDown);
            // 
            // Lockbutton
            // 
            this.Lockbutton.Location = new System.Drawing.Point(585, 23);
            this.Lockbutton.Name = "Lockbutton";
            this.Lockbutton.Size = new System.Drawing.Size(75, 23);
            this.Lockbutton.TabIndex = 13;
            this.Lockbutton.Text = "Unlock";
            this.Lockbutton.UseVisualStyleBackColor = true;
            this.Lockbutton.Click += new System.EventHandler(this.Lockbutton_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(479, 6);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(86, 15);
            this.label5.TabIndex = 14;
            this.label5.Text = "Unlock configs";
            // 
            // StartButton
            // 
            this.StartButton.Enabled = false;
            this.StartButton.Location = new System.Drawing.Point(479, 111);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(75, 23);
            this.StartButton.TabIndex = 15;
            this.StartButton.Text = "Start";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // Watcher1
            // 
            this.Watcher1.EnableRaisingEvents = true;
            this.Watcher1.SynchronizingObject = this;
            this.Watcher1.Changed += new System.IO.FileSystemEventHandler(this.OnChanged);
            this.Watcher1.Created += new System.IO.FileSystemEventHandler(this.OnCreated);
            this.Watcher1.Deleted += new System.IO.FileSystemEventHandler(this.OnDeleted);
            this.Watcher1.Error += new System.IO.ErrorEventHandler(this.OnError);
            this.Watcher1.Renamed += new System.IO.RenamedEventHandler(this.OnRenamed);
            // 
            // ResetButton
            // 
            this.ResetButton.Enabled = false;
            this.ResetButton.Location = new System.Drawing.Point(585, 114);
            this.ResetButton.Name = "ResetButton";
            this.ResetButton.Size = new System.Drawing.Size(75, 23);
            this.ResetButton.TabIndex = 16;
            this.ResetButton.Text = "Reset";
            this.ResetButton.UseVisualStyleBackColor = true;
            // 
            // LastTimeLabel
            // 
            this.LastTimeLabel.AutoSize = true;
            this.LastTimeLabel.Location = new System.Drawing.Point(14, 165);
            this.LastTimeLabel.Name = "LastTimeLabel";
            this.LastTimeLabel.Size = new System.Drawing.Size(12, 15);
            this.LastTimeLabel.TabIndex = 17;
            this.LastTimeLabel.Text = "-";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(620, 174);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 15);
            this.label6.TabIndex = 18;
            this.label6.Text = "Ver 0.5";
            // 
            // PassBox
            // 
            this.PassBox.AutoSize = true;
            this.PassBox.Checked = true;
            this.PassBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.PassBox.Location = new System.Drawing.Point(458, 71);
            this.PassBox.Name = "PassBox";
            this.PassBox.Size = new System.Drawing.Size(49, 19);
            this.PassBox.TabIndex = 19;
            this.PassBox.Text = "Pass";
            this.PassBox.UseVisualStyleBackColor = true;
            this.PassBox.CheckedChanged += new System.EventHandler(this.PassBox_CheckedChanged);
            // 
            // FailBox
            // 
            this.FailBox.AutoSize = true;
            this.FailBox.Checked = true;
            this.FailBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.FailBox.Location = new System.Drawing.Point(577, 71);
            this.FailBox.Name = "FailBox";
            this.FailBox.Size = new System.Drawing.Size(44, 19);
            this.FailBox.TabIndex = 20;
            this.FailBox.Text = "Fail";
            this.FailBox.UseVisualStyleBackColor = true;
            this.FailBox.CheckedChanged += new System.EventHandler(this.FailBox_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 15);
            this.label4.TabIndex = 10;
            this.label4.Text = "Template";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(673, 198);
            this.Controls.Add(this.FailBox);
            this.Controls.Add(this.PassBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.LastTimeLabel);
            this.Controls.Add(this.ResetButton);
            this.Controls.Add(this.StartButton);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.Lockbutton);
            this.Controls.Add(this.PasswordBox);
            this.Controls.Add(this.Logfile);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.TemplateBox);
            this.Controls.Add(this.BackupButton);
            this.Controls.Add(this.BackupFolder);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ReadPathButton);
            this.Controls.Add(this.ReadPath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ExportFolderButton);
            this.Controls.Add(this.ExportFolder);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Log Parser";
            ((System.ComponentModel.ISupportInitialize)(this.Watcher1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label1;
        private TextBox ExportFolder;
        private FolderBrowserDialog SelectFolder1;
        private Button ExportFolderButton;
        private Button ReadPathButton;
        private TextBox ReadPath;
        private Label label2;
        private Button BackupButton;
        private TextBox BackupFolder;
        private Label label3;
        private ComboBox TemplateBox;
        private Label Logfile;
        private TextBox PasswordBox;
        private Button Lockbutton;
        private Label label5;
        private Button StartButton;
        private FileSystemWatcher Watcher1;
        private Label LastTimeLabel;
        private Button ResetButton;
        private Label label6;
        private CheckBox FailBox;
        private CheckBox PassBox;
        private Label label4;
    }
}