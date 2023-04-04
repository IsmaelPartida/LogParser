#pragma warning disable CS8601 // Possible null reference assignment.
using Constants;
using Microsoft.VisualBasic.FileIO;

namespace LogReader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            if (string.IsNullOrWhiteSpace(Cons.DatetimeSec))
			{
				Cons.UpdateAppSetting("DatetimeSec", TimeUtils.ZeroDate().ToString());
				UpdateLastTimeLabel();
			}
            ReadPath.Text = Cons.ReadPath;
            ExportFolder.Text = Cons.ExportFolder;
            BackupFolder.Text = Cons.BackupFolder;
            Logfile.Text = $"Logfile de eventos guardado en {Cons.LogFile}";
            //Add here templates
            var templates = new List<string> { "EOL", "EOL_Old", "DDC" };
			TemplateBox.Items.AddRange(templates.ToArray());
            Watcher1.EnableRaisingEvents = false;
            UpdateLastTimeLabel();
        }
        public void UpdateLastTimeLabel()
        {
            LastTimeLabel.Text = $"La ultima vez que se ejecuto el programa fue el dia {Cons.DatetimeSec}";
        }
        private void ReadPath_Click(object sender, EventArgs e)
        {
            DialogResult result = SelectFolder1.ShowDialog();
            if (result is DialogResult.OK)
            {
                ReadPath.Text = SelectFolder1.SelectedPath;
                Cons.UpdateAppSetting("ReadPath", ReadPath.Text);
            }
        }
        private void ExportFolderButton_Click(object sender, EventArgs e)
        {
            DialogResult result = SelectFolder1.ShowDialog();
            if (result is DialogResult.OK)
            {
                ExportFolder.Text = SelectFolder1.SelectedPath;
                Cons.UpdateAppSetting("ExportFolder", ExportFolder.Text);
            }
        }
        private void BackupButton_Click(object sender, EventArgs e)
        {
            DialogResult result = SelectFolder1.ShowDialog();
            if (result is DialogResult.OK)
            {
                BackupFolder.Text = SelectFolder1.SelectedPath;
                Cons.UpdateAppSetting("BackupFolder", BackupFolder.Text);
            }
        }

		private void Lockbutton_Click(object sender, EventArgs e)
		{
			if (Lockbutton.Text == "Unlock")
			{
				if (PasswordBox.Text == "Dyson") { Unlock(); }
				else { MessageBox.Show("Contraseña incorrecta, vuelva a intentar...", "Contraseña Incorrecta", MessageBoxButtons.OK); }
			}
			else { Lock(); }
		}
		private void Unlock()
		{
    		StartButton.Text = "Start";
    		Watcher1.EnableRaisingEvents = false;
    		Lockbutton.Text = "Lock";
    		TemplateBox.Enabled = true;
    		ResetButton.Enabled = true;
    		ReadPath.Enabled = true;
    		ReadPathButton.Enabled = true;
    		ExportFolderButton.Enabled = true;
		    ExportFolder.Enabled = true;
		    BackupButton.Enabled = true;
    		BackupFolder.Enabled = true;
    		PasswordBox.Text = "";
		    if (TemplateBox.SelectedIndex != -1) { StartButton.Enabled = true; }
		}
		private void Lock()
		{
    		Watcher1.EnableRaisingEvents = false;
    		Lockbutton.Text = "Unlock";
    		StartButton.Enabled = false;
    		ResetButton.Enabled = false;
    		TemplateBox.Enabled = false;
    		ReadPath.Enabled = false;
    		ReadPathButton.Enabled = false;
    		ExportFolderButton.Enabled = false;
    		ExportFolder.Enabled = false;
		    BackupButton.Enabled = false;
    		BackupFolder.Enabled = false;
		}

        private void StartButton_Click(object sender, EventArgs e)
		{
			try
			{
				if (StartButton.Text == "Stop")
				{
					Lockbutton.Text = "Unlock";
					PasswordBox.Text = "Dyson";
					Lockbutton_Click(sender, e);
					StartButton.Text = "Start";
					Watcher1.EnableRaisingEvents = false;
				}
				else
				{
					string template = TemplateBox.Text;
                    Cons.FailFiles = FailBox.Checked;
                    Cons.PassFiles = PassBox.Checked;
                    if (string.IsNullOrEmpty(template))
                    {
                        MessageBox.Show("Template esta vacio, por favor selecciona un template...", "Template vacio", MessageBoxButtons.OK);
                        Cons.CleanUp();
                    }
                    if (!string.IsNullOrEmpty(template) && StartButton.Text == "Start" && (template == "EOL_Old" || template == "EOL" || template == "DDC"))
					{
						StartButton.Text = "Stop";
						Lockbutton.Text = "Lock";
						Lockbutton_Click(sender, e);
						StartButton.Enabled = true;
						Cons.SaveFolder = Directory.Exists(ExportFolder.Text) ? ExportFolder.Text : BackupFolder.Text;
						Cons.ProcessUnprocessedFiles(template, Cons.SaveFolder, ReadPath.Text);
						Cons.UpdateAppSetting("DatetimeSec", TimeUtils.Nowtime().ToString());
						Cons.CleanUp();
						Cons.AppendLogFile($"{TimeUtils.Nowtime()}: Se termino de revisar los archivos previos a abrir este programa! :D\n");
						Watcher1.Path = Cons.ReadPath;
						Watcher1.EnableRaisingEvents = true;
					}
				}
			}
			catch (Exception ex) { Cons.AppendLogFile(ex.ToString() + "\n"); }
			UpdateLastTimeLabel();
		}
        private void TemplateBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TemplateBox.SelectedIndex.ToString() != "-1") { StartButton.Enabled = true; } else { StartButton.Enabled = false; }
        }
        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            Thread.Sleep(2000);
            Cons.AppendLogFile($"{TimeUtils.Nowtime()}: El archivo {e.Name} fue modificado.");
            string template = TemplateBox.Text;
            if  (template == "EOL_Old" || template == "EOL" || template == "DDC")
            {
                if (!Directory.Exists(ExportFolder.Text)) { Cons.SaveFolder = BackupFolder.Text; }
                else { Cons.SaveFolder = ExportFolder.Text; }
                TextFieldParser Reader = FileReader.Reader(e.FullPath);
                Cons.OpenFile(Reader, template, Cons.SaveFolder);
                Reader.Close();
                Cons.UpdateAppSetting("DatetimeSec", TimeUtils.Nowtime().ToString());
                UpdateLastTimeLabel();
            }
            Cons.CleanUp();
        }
        private void OnCreated(object sender, FileSystemEventArgs e) => Cons.AppendLogFile($"{TimeUtils.Nowtime()}: El archivo {e.Name} fue creado.\n");
        private void OnDeleted(object sender, FileSystemEventArgs e) => Cons.AppendLogFile($"{TimeUtils.Nowtime()}: El archivo {e.Name} fue eliminado.\n");
        private void OnRenamed(object sender, RenamedEventArgs e) => Cons.AppendLogFile($"{TimeUtils.Nowtime()}: El archivo {e.OldName} fue renombrado a {e.Name}.\n");
        private void OnError(object sender, ErrorEventArgs e) => PrintException(e.GetException());
        private static void PrintException(Exception? ex)
        {
            if (ex != null)
            {
                Console.WriteLine($"Message: {ex.Message}");
                Console.WriteLine("Stacktrace:");
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine();
                PrintException(ex.InnerException);
            }
        }
        private void PasswordBox_KeyDown(object sender, KeyEventArgs e) { if(e.KeyValue == 13) { Lockbutton_Click(sender, e); } }
        private void PassBox_CheckedChanged(object sender, EventArgs e) =>  _ = PassBox.Checked ? true : false;
        private void FailBox_CheckedChanged(object sender, EventArgs e) => _ = FailBox.Checked ? true : false;
    }
}
#pragma warning restore CS8601 // Possible null reference assignment.