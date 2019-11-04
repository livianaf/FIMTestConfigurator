using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace FIMTestConfigurator {
    public class Utilities {
        //_______________________________________________________________________________________________________________________
        public static void LaunchApp( string CfgEntry, string Param = null ) {
            string sToolInfo = ConfigurationHelper.GetSetting(CfgEntry);
            string alias = sToolInfo.Split('|')[0];
            string exe = sToolInfo.Split('|')[1];
            string args = sToolInfo.Split('|')[2];
            if (!File.Exists(exe)) {
                exe = SelectFile($"Select {alias} Application", "exe files (*.exe)|*.exe", Path.GetFileName(exe), checkFileExists: true);
                if (!File.Exists(exe)) return;
                ConfigurationHelper.SetSetting(CfgEntry, $"{alias}|{exe}|{args}");
                }
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = exe;
            if (!string.IsNullOrWhiteSpace(args)) startInfo.Arguments = args.Replace("#DB#", Param);
            
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
