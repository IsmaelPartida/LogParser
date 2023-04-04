using System.Globalization;
using System.Text;

namespace LogReader
{
    public class EOL
    {
        public static void CreateHTML(Dictionary<string, string> test, string ExportFolder, string template)
        {
            string serialnumber = test["Motor Barcode"];
            string date = test["Date"];
            DateTime fecha;
            bool v = DateTime.TryParse(date, out fecha);
            date = fecha.ToString("yyyyMMdd_hhmmss");
            string fechaCarpeta = fecha.ToString("yyyy-MM-dd");
            string status = string.IsNullOrEmpty(test["Failure Message"]) ? "PASS" : "FAIL";
            string filename = $"EOL_{status}_{serialnumber}_{date}";
            string folderpath = Path.Combine(ExportFolder, fechaCarpeta);
            string filepath = Path.Combine(folderpath, $"{filename}.html");

            Directory.CreateDirectory(folderpath);
            if (!File.Exists(filepath))
            {
                var myfile = File.Create(filepath);
                myfile.Close();
                if (template == "EOL_Old")
                {
                    CreateHeader(filename, filepath, test["Date"], test["Model"], test["Motor Barcode"], test["Hardware Serial Number"], test["Motor Version"], test["Elapse Time(sec)"], test["Failure Message"], test["Failure Code"]);
                    CreateTable(filepath, test, "P1 (Low Mode)", template);
                    CreateTable(filepath, test, "P1 (Medium Mode)", template);
                    CreateTable(filepath, test, "P1 (Hi Mode)", template);
                    CreateTable(filepath, test, "P3 (Hi Mode)", template);
                }
                if (template == "EOL")
                {
                    CreateHeader(filename, filepath, test["Date"], test["Model"], test["Motor Barcode"], test["Hardware Serial Number"], test["Motor Version"], test["Elapse Time (sec)"], test["Failure Message"], test["Failure Code"]);
                    CreateTable(filepath, test, "P1 Low Mode", template);
                    CreateTable(filepath, test, "P1 Medium Mode", template);
                    CreateTable(filepath, test, "P1 Hi Mode", template);
                    CreateTable(filepath, test, "P3 Hi Mode", template);
                }
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine("\t</body>\n" +
                        "</html>");
                }
            }
        }
        private static void CreateHeader(string fn, string fp, string date, string model, string serial, string EquipmentSN, string MV, string TE, string FM, string FC)
        {
            using var sw = new StreamWriter(fp);
            sw.WriteLine($@"
        <!DOCTYPE html>
        <html>
            <head>
                <style>
                    table, th, td {{
                        text-align: center;
                        border: 1px solid black;
                        border-collapse: collapse;
                    }}
                </style>
                <title>{fn}</title>
            </head>
            <body>
                <h2>Serial: {serial}</h2>
                <p>Date: {date}</p>
                <p>Time Elapsed: {TE} seconds</p>
                <p>Model: {model}</p>
                <p>Equipment: {System.Environment.MachineName}</p>
                <p>Equipment Serial: {EquipmentSN}</p>
                <p>Motor Version: {MV}</p>
                <h3>Failure Message: {FM}</h3>
                <h3>Failure Code: {FC}</h3>
            ");
        }
        private static void CreateTable(string filepath, Dictionary<string, string> test, string speed, string template)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"\t\t<h3>{speed} Mode</h3>");
            if (template == "EOL_Old")
            {
                if (speed != "P3 (Hi Mode)")
                {
                    stringBuilder.AppendLine("\t\t<table>");
                    stringBuilder.AppendLine($"\t\t\t<tr>");
                    stringBuilder.AppendLine($"\t\t\t\t<th>{speed} Humidity (Rh)</th>");
                    stringBuilder.AppendLine($"\t\t\t\t<th>{speed} Amb. Air (mbar)</th>");
                    stringBuilder.AppendLine($"\t\t\t\t<th>{speed} Amb. Temperature(C)</th>");
                    stringBuilder.AppendLine($"\t\t\t</tr>");
                    stringBuilder.AppendLine($"\t\t\t<tr>");
                    stringBuilder.AppendLine($"\t\t\t\t<td>{test[$"{speed} Humidity (Rh)"]}</td>");
                    stringBuilder.AppendLine($"\t\t\t\t<td>{test[$"{speed} Amb. Air (mbar)"]}</td>");
                    stringBuilder.AppendLine($"\t\t\t\t<td>{test[$"{speed} Amb. Temperature(C)"]}</td>");
                    stringBuilder.AppendLine($"\t\t\t</tr>");
                    stringBuilder.AppendLine("\t\t</table>");
                }
            }
            else
            {
                if (speed != "P3 Hi Mode")
                {
                    stringBuilder.AppendLine("\t\t<table>");
                    stringBuilder.AppendLine($"\t\t\t<tr>");
                    stringBuilder.AppendLine($"\t\t\t\t<th>{speed} Humidity (Rh)</th>");
                    stringBuilder.AppendLine($"\t\t\t\t<th>{speed} Amb. Air (mbar)</th>");
                    stringBuilder.AppendLine($"\t\t\t\t<th>{speed} Amb. Temperature(C)</th>");
                    stringBuilder.AppendLine($"\t\t\t</tr>");
                    stringBuilder.AppendLine($"\t\t\t<tr>");
                    stringBuilder.AppendLine($"\t\t\t\t<td>{test[$"{speed} Humidity (Rh)"]}</td>");
                    stringBuilder.AppendLine($"\t\t\t\t<td>{test[$"{speed} Amb. Air (mbar)"]}</td>");
                    stringBuilder.AppendLine($"\t\t\t\t<td>{test[$"{speed} Amb. Temperature (C)"]}</td>");
                    stringBuilder.AppendLine($"\t\t\t</tr>");
                    stringBuilder.AppendLine("\t\t</table>");
                }
                
            }
            
            stringBuilder.AppendLine("\t\t<p></p>");
            stringBuilder.AppendLine("\t\t<table>\n");
            stringBuilder.Append("\t\t\t<tr>\n");
            stringBuilder.Append($"\t\t\t\t<th>Step</th>\n");
            stringBuilder.Append($"\t\t\t\t<th>Lower Limit</th>\n");
            stringBuilder.Append($"\t\t\t\t<th>Measurement</th>\n");
            stringBuilder.Append($"\t\t\t\t<th>Upper Limit</th>\n");
            stringBuilder.Append($"\t\t\t\t<th>Status</th>\n");
            stringBuilder.Append("\t\t\t</tr>\n");

            string[,] measurements = template == "EOL_Old" ? new string[,] {
                {$"{speed} Suction Lower Limit", $"{speed} Suction Pressure (mbar)", $"{speed} Suction Upper Limit", $"{speed} Suction Pressure Status"},
                {$"{speed} Airwatt Lower Limit", $"{speed} Airwatt(AW)", $"{speed} Airwatt Upper Limit", $"{speed} Status Airwatt"},
                {$"{speed} Voltage Lower Limit", $"{speed} Voltage(V)", $"{speed} Voltage Upper Limit", $"{speed} Status Voltage"},
                {$"{speed} Power Drawn Lower Limit", $"{speed} Power(W)", $"{speed} Power DrawnUpper Limit", $"{speed} Status Power"},
                {$"{speed} Motor Power Lower Limit", $"{speed} Motor Power(W)", $"{speed} Motor Power Upper Limit", $"{speed} Status Motor Power"},
                {$"{speed} Motor Speed Lower Limit", $"{speed} Motor Speed(rpm)", $"{speed} Motor Speed Upper Limit", $"{speed} Status Motor Speed"},
                {$"{speed} Init. Motor Temp Lower Limit", $"{speed} Initial Motor Temp(C)", $"{speed} Init. Motor Temp Upper Limit", $"{speed} Status Initial  Temp"},
                {$"{speed} Init. Barometer Lower Limit", $"{speed} Initial Barometer (kPa)", $"{speed} Init.  Barometer Upper Limit", $"{speed} Status Init. Baro"},
                {$"{speed} Fin. Motor Temp Lower Limit", $"{speed} Final  Motor Temp(C)", $"{speed} Fin. Motor Temp Upper Limit", $"{speed} Status Final Temp"},
                {$"{speed} Final Barometer Lower Limit", $"{speed} Final Barometer (kPa)", $"{speed} Final Barometer Upper Limit", $"{speed} Status Fin. Baro"},
                {$"{speed} Calibrated Speed Lower Limit", $"{speed} Calibration Speed(rpm)", $"{speed} Calibrated Speed Upper Limit", $"{speed} Status Cal. Speed"},
                {$"", $"{speed} Filter Wash", $"", $"{speed} Status Filter Wash"},
                {$"", $"{speed} Init. Raw Baro(kPa)", $"", $"{speed} Init. Raw Baro(kPa)"},
                {$"", $"{speed} Init. Amb.Temp(C)", $"", $"{speed} Init. Amb.Temp(C)"},
                {$"", $"{speed} Init. Amb. Pressure(mbar)", $"", $"{speed} Init. Amb. Pressure(mbar)"},
                {$"", $"{speed} Fin. Raw Baro(kPa)", $"", $"{speed} Fin. Raw Baro(kPa)"},
                {$"", $"{speed} Fin.Amb Pressure(kPa)", $"", $"{speed} Fin.Amb Pressure(kPa)"},
                {$"", $"{speed} Voltage Stable", $"", $"{speed} Voltage Stable"},
                {$"", $"{speed} Suction Stable", $"", $"{speed} Suction Stable"},
                {$"", $"{speed} Pass/Fail", $"", $"{speed} Pass/Fail"}
            } : new string[,] {
                {$"{speed} Suction Lower Limit", $"{speed} Suction Pressure (mbar)", $"{speed} Suction Upper Limit", $"{speed} Suction Pressure Status"},
                {$"{speed} Airwatt Lower Limit", $"{speed} Airwatt (AW)", $"{speed} Airwatt Upper Limit", $"{speed} Airwatt Status"},
                {$"{speed} Voltage Lower Limit", $"{speed} Voltage (V)", $"{speed} Voltage Upper Limit", $"{speed} Voltage Status"},
                {$"{speed} Power Lower Limit", $"{speed} Power (W)", $"{speed} Power Upper Limit", $"{speed} Power Status"},
                {$"{speed} Motor Power Lower Limit", $"{speed} Motor Power (W)", $"{speed} Motor Power Upper Limit", $"{speed} Motor Power Status"},
                {$"{speed} Motor Speed Lower Limit", $"{speed} Motor Speed (rpm)", $"{speed} Motor Speed Upper Limit", $"{speed} Motor Speed Status"},
                {$"{speed} Initial Motor Temp Lower Limit", $"{speed} Initial Motor Temp (C)", $"{speed} Initial Motor Temp Upper Limit", $"{speed} Initial Motor Temp Status"},
                {$"{speed} Initial Barometer Lower Limit", $"{speed} Initial Barometer (kPa)", $"{speed} Initial Barometer Upper Limit", $"{speed} Initial Barometer Status"},
                {$"{speed} Final Motor Temp Lower Limit", $"{speed} Final Motor Temp (C)", $"{speed} Final Motor Temp Upper Limit", $"{speed} Final Motor Temp Status"},
                {$"{speed} Final Barometer Lower Limit", $"{speed} Final Barometer (kPa)", $"{speed} Final Barometer Upper Limit", $"{speed} Final Barometer Status"},
                {$"{speed} Calibrated Speed Lower Limit", $"{speed} Calibrated Speed (rpm)", $"{speed} Calibrated Speed Upper Limit", $"{speed} Calibrated Speed Status"},
                {$"", $"{speed} Filter Wash", $"", $"{speed} Filter Wash"},
                {$"", $"{speed} Init. Raw Baro (kPa)", $"", $"{speed} Init. Raw Baro (kPa)"},
                {$"", $"{speed} Init. Amb.Temp (C)", $"", $"{speed} Init. Amb.Temp (C)"},
                {$"", $"{speed} Init. Amb. Pressure (mbar)", $"", $"{speed} Init. Amb. Pressure (mbar)"},
                {$"", $"{speed} Fin. Raw Baro (kPa)", $"", $"{speed} Fin. Raw Baro (kPa)"},
                {$"", $"{speed} Fin. Amb. Pressure (kPa)", $"", $"{speed} Fin. Amb. Pressure (kPa)"},
                {$"", $"{speed} Voltage Stable", $"", $"{speed} Voltage Stable"},
                {$"", $"{speed} Suction Stable", $"", $"{speed} Suction Stable"},
                {$"", $"{speed} Pass/Fail", $"", $"{speed} Pass/Fail"}
            };
            for (int i = 0; i < measurements.GetLength(0); i++)
            {
                string label = measurements[i, 1];
                string lowerLimit = test[measurements[i, 0]];
                string value = test[measurements[i, 1]]; 
                string upperLimit = test[measurements[i, 2]]; 
                string status = test[measurements[i, 3]]; 

                if (lowerLimit != "" && upperLimit != "")
                {
                    string style = status == "PASS" ? " bgcolor = \"green\"" : (status == "FAILED" ? " bgcolor = \"red\"" : "");
                    stringBuilder.Append("\t\t\t<tr>\n");
                    stringBuilder.Append($"\t\t\t\t<td>{label}</td>\n");
                    stringBuilder.Append($"\t\t\t\t<td>{lowerLimit}</td>\n");
                    stringBuilder.Append($"\t\t\t\t<td>{value}</td>\n");
                    stringBuilder.Append($"\t\t\t\t<td>{upperLimit}</td>\n");
                    stringBuilder.Append($"\t\t\t\t<td {style}>{status}</td>\n");
                    stringBuilder.Append("\t\t\t</tr>\n");
                }
                else
                {
                    string style = status == "Pass" || status == "YES" || status == "PASS" ? " bgcolor = \"green\"" : (status == "NO" || status == "FAIL" || status == "Fail" ? " bgcolor = \"red\"" : "");
                    stringBuilder.Append("\t\t\t<tr>\n");
                    stringBuilder.Append($"\t\t\t\t<td colspan=\"4\">{label}</td>\n");
                    stringBuilder.Append($"\t\t\t\t<td {style}>{status}</td>\n");
                    stringBuilder.Append($"\t\t\t</tr>\n");

                }
            }
            stringBuilder.Append("\t\t</table>\n");
            using (StreamWriter sw = new StreamWriter(filepath, true))
            {
                sw.Write(stringBuilder.ToString());
            }
        }
    }
    public class DDC
    {
        public static void CreateHTML(Dictionary<string, string> test, string ExportFolder, string template)
        {
            string serialnumber = test["Serial No"];
            string fulldate = $"{test["Date"]} {test["Time"]}";
            string date = fulldate.Replace(" ", "_").Replace("/", "-").Replace(":", "-");
            string status = test["Overall Result"];
            string filename = $"DDC_{status}_{serialnumber}_{date}.html";
            DateTime fecha;
            bool v = DateTime.TryParse(test["Date"], out fecha);
            string fechaCarpeta = fecha.ToString("yyyy-MM-dd");
            string folderpath = Path.Combine(ExportFolder, fechaCarpeta);
            string filepath = Path.Combine(folderpath, filename);

            Directory.CreateDirectory(folderpath);
            if (!File.Exists(filepath))
            {
                var myfile = File.Create(filepath);
                myfile.Close();
                CreateHeader(filename, filepath, fulldate, test);
                CreateTable(filename, filepath, test);
                using StreamWriter sw = File.AppendText(filepath);
                sw.WriteLine("\t</body>\n" +
                    "</html>");
                sw.Close();
            }
        }
        private static void CreateTable(string fn, string fp, Dictionary<string, string> test)
        {
            Createstep(fp, "Index 1");
            CreateFullRow(fp, "Step", "Measure", "Expected", "Status");
            CreateFullRow(fp, "Check PSU CN1 Output", test);
            CreateFullRow(fp, "Check PSU CN2 Output", test);
            CreateFullRow(fp, "Power On UUT", test);
            CreateFullRow(fp, "Disable UI Polling", test);
            CloseStep(fp);
            
            Createstep(fp, "Index 2");
            CreateFullRow(fp, "Step", "Measure", "Expected", "Status");
            CreateFullRow(fp, "Disable DDM-BMS Polling", test);
            CreateFullRow(fp, "Disable DDM-PMSP Polling", test);
            CloseStep(fp);

            Createstep(fp, "Index 3");
            CreateFullRow(fp, "Step", "Measure", "Expected", "Status");
            CreateFullRow(fp, "Set ADC filter constants 0", test);
            CreateFullRow(fp, "Reset DIPC", test);
            CreateFullRow(fp, "Monitor for Responses", test);
            CreateFullRow(fp, "PMSP Calib - Hardware SN", test);
            CreateFullRow(fp, "PMSP Calib - Hardware SN - Byte Length", test);
            CreateFullRow(fp, "PMSP Calib - Software Version", test);
            CreateFullRow(fp, "PMSP Calib - Software Version - Byte Length", test);
            CreateFullRow(fp, "Activate Calib Mode", test);
            CloseStep(fp);

            Createstep(fp, "Index 4");
            CreateRangeRow(fp);
            CreateRangeRow(fp, "BG Noise - ADC offset", test);
            CreateRangeRow(fp, "BG Noise - RMS Mean", test);
            CreateRangeRow(fp, "BG Noise - RMS Standard Deviation", test);
            CloseStep(fp);

            Createstep(fp, "Index 5");
            CreateRangeRow(fp);
            CreateRangeRow(fp, "Force Measurement", test);
            CloseStep(fp);

            Createstep(fp, "Index 6");
            CreateFullRow(fp, "Step", "Measure", "Expected", "Status");
            CreateFullRow(fp, "Write Tester ID", test);
            CreateFullRow(fp, "Write Actuator ID", test);
            CreateFullRow(fp, "Load Reference Energies", test);
            CreateFullRow(fp, "Ultra Fine - Impact Threshold Value", test);
            CreateFullRow(fp, "Ultra Fine - Write Impact Threshold", test);
            CreateFullRow(fp, "Ultra Fine - Waveform Amplitude", test);
            CreateFullRow(fp, "Ultra Fine - All Impact Energy", test);
            CreateFullRow(fp, "Ultra Fine - Written Impact Energy", test);
            CreateFullRow(fp, "Ultra Fine - All Impact Duration", test);
            CreateFullRow(fp, "Ultra Fine - All Interim Sens Ratio", test);
            CreateFullRow(fp, "Fine - Impact Threshold Value", test);
            CreateFullRow(fp, "Fine - Write Impact Threshold", test);
            CreateFullRow(fp, "Fine - Waveform Amplitude", test);
            CreateFullRow(fp, "Fine - All Impact Energy", test);
            CreateFullRow(fp, "Fine - Written Impact Energy", test);
            CreateFullRow(fp, "Fine - All Impact Duration", test);
            CreateFullRow(fp, "Fine - All Interim Sens Ratio", test);
            CreateFullRow(fp, "Medium - Impact Threshold Value", test);
            CreateFullRow(fp, "Medium - Write Impact Threshold", test);
            CreateFullRow(fp, "Medium - Waveform Amplitude", test);
            CreateFullRow(fp, "Medium - All Impact Energy", test);
            CreateFullRow(fp, "Medium - Written Impact Energy", test);
            CreateFullRow(fp, "Medium - All Impact Duration", test);
            CreateFullRow(fp, "Medium - All Interim Sens Ratio", test);
            CreateFullRow(fp, "Large - Impact Threshold Value", test);
            CreateFullRow(fp, "Large - Write Impact Threshold", test);
            CreateFullRow(fp, "Large - Waveform Amplitude", test);
            CreateFullRow(fp, "Large - All Impact Energy", test);
            CreateFullRow(fp, "Large - Written Impact Energy", test);
            CreateFullRow(fp, "Large - All Impact Duration", test);
            CreateFullRow(fp, "Large - All Interim Sens Ratio", test);
            CreateFullRow(fp, "Ultra Fine - UUT Energies", test);
            CreateFullRow(fp, "Fine - UUT Energies", test);
            CreateFullRow(fp, "Medium - UUT Energies", test);
            CreateFullRow(fp, "Large - UUT Energies", test);
            CreateFullRow(fp, "Ultra Fine - Avg Impact Duration", test);
            CreateFullRow(fp, "Fine - Avg Impact Duration", test);
            CreateFullRow(fp, "Medium - Avg Impact Duration", test);
            CreateFullRow(fp, "Large - Avg Impact Duration", test);
            CreateFullRow(fp, "Sensitivity Ratios Verification", test);
            CloseStep(fp);
            Createstep(fp, "Index 6");
            CreateRangeRow(fp);
            CreateRangeRow(fp, "Ultra Fine - Written Impact Duration", test);
            CreateRangeRow(fp, "Ultra Fine - Written Interim Sens Ratio", test);
            CreateRangeRow(fp, "Fine - Written Impact Duration", test);
            CreateRangeRow(fp, "Fine - Written Interim Sens Ratio", test);
            CreateRangeRow(fp, "Medium - Written Impact Duration", test);
            CreateRangeRow(fp, "Medium - Written Interim Sens Ratio", test);
            CreateRangeRow(fp, "Large - Written Impact Duration", test);
            CreateRangeRow(fp, "Large - Written Interim Sens Ratio", test);
            CreateRangeRow(fp, "Ultra Fine - Sensitivity Ratios", test);
            CreateRangeRow(fp, "Fine - Sensitivity Ratios", test);
            CreateRangeRow(fp, "Medium - Sensitivity Ratios", test);
            CreateRangeRow(fp, "Large - Sensitivity Ratios", test);
            CloseStep(fp);
        }
        private static string getStyle(string status)
        {
            string style = "td";
            if (status is "N/A" or "Status" or "NA")
            { style = "td bgcolor = \"white\""; }
            else
            {
                if (status == "PASS") { style = "td bgcolor = \"green\""; }
                else { style = "td bgcolor = \"red\""; }
            }
            return style;
        }
        private static void CreateFullRow(string fp, string step, Dictionary<string, string> test)
        {
            string meas = test[step];
            string expected = test[step + " expected"];
            string status = test[step + "_Status"];
            string style = getStyle(status);

            using StreamWriter sw = File.AppendText(fp);
            sw.WriteLine($"\t\t\t<tr>\n" +
                $"\t\t\t\t<td>{step}</td>\n" +
                $"\t\t\t\t<td>{meas}</td>\n" +
                $"\t\t\t\t<td>{expected}</td>\n" +
                $"\t\t\t\t<{style}>{status}</td>\n" +
                $"\t\t\t</tr>\n");
            sw.Close();
        }
        private static void CreateFullRow(string fp, string step, string meas, string expected, string status)
        {
            using StreamWriter sw = File.AppendText(fp);
            sw.WriteLine($"\t\t\t<tr>\n" +
                $"\t\t\t\t<th>{step}</th>\n" +
                $"\t\t\t\t<th>{meas}</th>\n" +
                $"\t\t\t\t<th>{expected}</th>\n" +
                $"\t\t\t\t<th>{status}</th>\n" +
                $"\t\t\t</tr>\n");
            sw.Close();
        }
        private static void Createstep(string fp, string step)
        {
            using (StreamWriter sw = File.AppendText(fp))
            {
                sw.WriteLine($"\t\t<h3>{step}</h3>\n" +
                    $"\t\t<p></p>\n" +
                    $"\t\t<table>\n");
                sw.Close();
            }
        }
        private static void CreateRangeRow(string fp, string step, Dictionary<string, string> test)
        {
            string low = test[step + " Lower Limit"];
            string meas = test[step];
            string high = test[step + " Upper Limit"];
            string status = test[step + "_Status"];
            string style = getStyle(status);

            using StreamWriter sw = File.AppendText(fp);
            sw.WriteLine($"\t\t\t<tr>\n" +
                $"\t\t\t\t<td>{step}</td>\n" +
                $"\t\t\t\t<td>{low}</td>\n" +
                $"\t\t\t\t<td>{meas}</td>\n" +
                $"\t\t\t\t<td>{high}</td>\n" +
                $"\t\t\t\t<{style}>{status}</td>\n" +
                $"\t\t\t</tr>\n");
            sw.Close();


        }
        private static void CreateRangeRow(string fp)
        {
            using StreamWriter sw = File.AppendText(fp);
            sw.WriteLine($"\t\t\t<tr>\n" +
                $"\t\t\t\t<th>Step</th>\n" +
                $"\t\t\t\t<th>Low Limit</th>\n" +
                $"\t\t\t\t<th>Measure</th>\n" +
                $"\t\t\t\t<th>Upper Limit</th>\n" +
                $"\t\t\t\t<th>Result</th>\n" +
                $"\t\t\t</tr>\n");
            sw.Close();
        }
        private static void CloseStep(string fp)
        {
            using StreamWriter sw = File.AppendText(fp);
            sw.WriteLine($"\t\t</table>\n\n");
            sw.Close();
        }
        private static void CreateHeader(string fn, string fp, string date, Dictionary<string, string> test)
        {
            using StreamWriter sw = File.AppendText(fp);
            sw.WriteLine("<!DOCTYPE html>\n" +
                "<html>\n" +
                "\t<head>\n" +
                "\t\t<style>\n" +
                "\t\t\ttable, th, td {\n" +
                "\t\t\t\ttext-align: center;" +
                "\t\t\t\tborder: 1px solid black;\n" +
                "\t\t\t\tborder-collapse: collapse;\n" +
                "\t\t\t}\n" +
                "\t\t</style>\n" +
                $"\t\t<title>{fn}</title>\n" +
                $"\t</head>\n" +
                $"\t<body>\n" +
                $"\t\t<h2>Serial: {test["Serial No"]}</h2>\n" +
                $"\t\t<h2>Result: {test["Overall Result"]}</h2>\n" +
                $"\t\t<h3>Failure Message: {test["Failure Message"]}</h3>\n" +
                $"\t\t<p>Date: {date}</p>\n" +
                $"\t\t<p>Station Name: {test["Station Name"]}</p>\n" +
                $"\t\t<p>Station ID: {test["Station ID"]}</p>\n" +
                $"\t\t<p>Time Elapsed: {test["Overall Test Time"]} seconds</p>\n" +
                $"\t\t<p>Error Code: {test["Error Code"]}</p>\n" +
                $"\t\t<p>Equipment: {System.Environment.MachineName}</p>" +
                $"\t\t<p>Variant:  {test["Variant"]}</p>\n" +
                $"\t\t<p>ETS Revision:  {test["ETS Revision"]}</p>\n" +
                $"\t\t<p>Software Revision:  {test["Software Revision"]}</p>\n" +
                $"\t\t<p>CPC Network Status:  {test["CPC Network Status"]}</p>\n");
            sw.Close();

        }
    }
}