using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace FIMTestConfigurator {
    public class Utilities {
        //_______________________________________________________________________________________________________________________
        public static void LaunchApp( string CfgEntry, string SelectText, string Param = null ) {
            string exe = ConfigurationHelper.GetSetting(CfgEntry);
            if (!File.Exists(exe)) {
                exe = SelectFile("Select " + SelectText + " Application", "exe files (*.exe)|*.exe", Path.GetFileName(exe), checkFileExists: true);
                if (!File.Exists(exe)) return;
                ConfigurationHelper.SetSetting(CfgEntry, exe);
                }
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = exe;
            if (!string.IsNullOrWhiteSpace(Param)) startInfo.Arguments = Param;
            Process.Start(startInfo);
            }
        //_______________________________________________________________________________________________________________________
        public static string SelectFile( string title, string filter, string filename = null, bool checkFileExists = false) {
            using (OpenFileDialog oDlg = new OpenFileDialog {
                InitialDirectory = @"D:\",
                Title = title,
                CheckFileExists = checkFileExists,
                CheckPathExists = true,
                Filter = filter,
                ShowReadOnly = true,
                RestoreDirectory = true
                }
                ) {
                if (filename != null) oDlg.FileName = filename;
                if (oDlg.ShowDialog() == DialogResult.OK) return oDlg.FileName; else return null;
                }
            }
        //_______________________________________________________________________________________________________________________
        public static string SelectFolder() {
            using (var fbd = new FolderBrowserDialog() { SelectedPath = @"D:\", ShowNewFolderButton = false }) {
                DialogResult result = fbd.ShowDialog();
                if (result == DialogResult.OK) return fbd.SelectedPath; else return null;
                }
            }
        }
    }
