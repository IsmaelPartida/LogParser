#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8604 // Possible null reference argument.
using System.Configuration;
using System.Collections.Specialized;
using Microsoft.VisualBasic.FileIO;
using LogReader;
using SearchOption = System.IO.SearchOption;

namespace Constants
{
    public static class TimeUtils
    {
        public static DateTime ZeroDate()
        {
            return new DateTime(1970, 1, 1, 0, 0, 0);
        }
        public static DateTime ToMillisecondPrecision(DateTime d)
        {
            return new DateTime(d.Year, d.Month, d.Day, d.Hour, d.Minute, d.Second, d.Millisecond, d.Kind);
        }
        public static DateTime Nowtime()
        {
            return ToMillisecondPrecision(DateTime.Now);
        }
        public static DateTime StrToDT(string date)
        {
            if (date == null || date.Length == 0)
            {
                return ZeroDate();
            }
            var a = DateTime.Parse(date);
            return ToMillisecondPrecision(a);
        }
    }
    public class Cons
    {
        
        public static void UpdateAppSetting(string key, string value)
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configuration.AppSettings.Settings[key].Value = value;
            configuration.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
        private static NameValueCollection AppSettings => ConfigurationManager.AppSettings;
        public static string? ReadPath { get; set; } = AppSettings[nameof(ReadPath)];
        public static string? ExportFolder { get; set; } = AppSettings[nameof(ExportFolder)];
        public static string? SaveFolder { get; set; }
        public static string? BackupFolder { get; set; } = AppSettings[nameof(BackupFolder)];
        public static string? LogFile { get; set; } = AppSettings[nameof(LogFile)];
        public static string? DatetimeSec { get; set; } = AppSettings[nameof(DatetimeSec)];

        public static string[]? header;
        public static string[]? fields;
        public static bool PassFiles;
        public static bool FailFiles;
        public static bool continu;

        //DateTime
        public static DateTime LastInFile;
        public static DateTime lastRead;

        public static void CleanUp()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
        public static void ProcessUnprocessedFiles(string template, string exportFolder, string ReadPath)
        {
            DateTime lastOpen = TimeUtils.StrToDT(DatetimeSec);
            foreach (string file in Directory.GetFiles(ReadPath, "*.csv", System.IO.SearchOption.TopDirectoryOnly))
            {
                try
                {
                    if (!file.Contains("Abort"))
                    {
                        DateTime lastModified = File.GetLastWriteTimeUtc(file);
                        if (lastModified >= lastOpen)
                        {
                            using (TextFieldParser reader = FileReader.Reader(file))
                            {
                                OpenFile(reader, template, exportFolder);
                            }
                        }
                    }
                }
                catch (Exception ex) { AppendLogFile(ex.ToString() + "\n"); }
            }
            UpdateAppSetting("DatetimeSec", TimeUtils.Nowtime().ToString());
        }
        public static void AppendLogFile(string message) => File.AppendAllText(path: LogFile, contents: message);
        public static void OpenFile(TextFieldParser Reader, string template, string ExportFolder)
        {
            DateTime lastopen = TimeUtils.StrToDT(date: ConfigurationManager.AppSettings.Get("DatetimeSec"));
            header = Reader.ReadFields();
            while (!Reader.EndOfData)
            {
                if (template == "EOL_Old" || template == "EOL")
                {
                    Dictionary<string, string> test = new Dictionary<string, string>();
                    foreach (string x in header) test[x] = "";
                    fields = Reader.ReadFields();
                    int place = 0;
                    foreach (string x in fields)
                    {
                        test[header[place]] = x;
                        place++;
                    }

                    bool continu = !string.IsNullOrEmpty(test["Failure Message"]) ? FailFiles : PassFiles;
                    if (continu)
                    {
                        if (test["Date"].Contains(" AM") || test["Date"].Contains(" PM") && (test["Motor Barcode"].Contains("N2Y") || test["Motor Barcode"].Contains("U8X")))
                        {
                            DateTime testdate = TimeUtils.StrToDT(test["Date"]);
                            if (testdate >= lastopen) { ThreadPool.QueueUserWorkItem(new WaitCallback(CreateHTMLWorker), new object[] { test, ExportFolder, template }); }
                        }
                    }
                }
                if (template is "DDC")
                {
                    Dictionary<string, string> test = new Dictionary<string, string>();
                    fields = Reader.ReadFields();
                    int place = fields.Length;
                    for (int i = 0; i < place; i++)
                    {
                        string key = header[i];
                        if (key.Contains("["))
                        {
                            key = key.Replace("][", "--").Replace("]", "").Replace("[", "");
                            string[] data = key.Split("--");
                            key = data[1];
                            string type = data[2];
                            if (type == "EQ")
                            {
                                string value = data[0] == "6.51" ? "PASS" : data[3];
                                test[key] = fields[i];
                                test[key + " expected"] = value;
                            }
                            if (type == "NC")
                            {
                                test[key] = fields[i];
                                test[key + " expected"] = "N/A";
                            }
                            if (type == "GTLT" || type == "GELE")
                            {
                                string lower = data[3];
                                string upper = data[4];
                                test[key] = fields[i];
                                test[key + " Lower Limit"] = lower;
                                test[key + " Upper Limit"] = upper;
                            }
                        }
                        else
                        {
                            test[key] = fields[i];
                        }
                    }
                    string status = test["Overall Result"];
                    continu = false;
                    if (!string.IsNullOrEmpty(status)) { continu = (status == "PASS" && PassFiles) || (status == "FAIL" && FailFiles); }
                    if (test["Serial No"].Contains("N2Y") && continu == true)
                    {
                        DateTime testdate = TimeUtils.StrToDT($"{test["Date"]} { test["Time"]}");
                        if (testdate >= lastopen)
                        {
                            ThreadPool.QueueUserWorkItem(new WaitCallback(CreateHTMLWorker), new object[] { test, ExportFolder, template });
                        }
                    }
                }
            }
        }
        private static void CreateHTMLWorker(object state)
        {
            object[] parameters = (object[])state;
            Dictionary<string, string> test = (Dictionary<string, string>)parameters[0];
            string exportFolder = (string)parameters[1];
            string template = (string)parameters[2];
            if (template == "EOL" || template == "EOL_Old") { EOL.CreateHTML(test, exportFolder, template);}
            else if(template == "DDC") { DDC.CreateHTML(test, ExportFolder, template); }
        }
    }
    public class FileReader
    {
        public static TextFieldParser Reader(string path)
        {
            TextFieldParser file = new(path)
            {
                TextFieldType = FieldType.Delimited,
                HasFieldsEnclosedInQuotes = true
            };
            file.SetDelimiters(new string[] { "," });
            return file;
        }
    }
}
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8604 // Possible null reference argument.