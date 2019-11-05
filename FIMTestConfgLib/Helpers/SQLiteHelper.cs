using System;
using System.Data;
//using Finisar.SQLite;
using System.Data.SQLite;
using System.IO;

//_______________________________________________________________________________________________________________________
namespace FIMTestConfigurator {
    //_______________________________________________________________________________________________________________________
    class SQLiteHelper {
        //_______________________________________________________________________________________________________________________
        private SQLiteConnection oSqlCon;
        private string cNomBD = "";
        //_______________________________________________________________________________________________________________________
        private string _LastErrorMessages = "";
        public string LastErrorMessages { get { string s = _LastErrorMessages; _LastErrorMessages = ""; return s; } }

        //_______________________________________________________________________________________________________________________
        public SQLiteHelper(string sDBFile) { OpenDB(sDBFile); }
        //_______________________________________________________________________________________________________________________
        public bool IsNew { get; private set; }
        //_______________________________________________________________________________________________________________________
        public void ProcFile(string sSQLFile) {
            int counter = 0;
            string line;

            // Read the file and display it line by line.
            StreamReader file = new StreamReader(sSQLFile);
            while ((line = file.ReadLine()) != null) {
                Console.WriteLine("[" + counter + "] " + line);
                if (line.Trim() != "") ExecuteNonQuery(line);
                counter++;
                }

            file.Close();
            }
        //_______________________________________________________________________________________________________________________
        public void OpenDB(string sDBFile) {
            cNomBD = sDBFile;
            // This code segment connects/creates the DB and creates a table in it called names
            IsNew = !File.Exists(sDBFile);
            if (IsNew) Console.WriteLine(".DB file does not exist => it will be created");
            else
                try { File.Copy(sDBFile, Path.ChangeExtension(sDBFile, ".bak"), true); }
                catch (Exception ex) {
                    string msg = $"Error in creation of BAK copy of Database {sDBFile}. Message: {ex.Message}\r\n{ex.StackTrace}";
                    Console.WriteLine(msg);
                    _LastErrorMessages += (string.IsNullOrWhiteSpace(_LastErrorMessages) ? "" : "\r\n") + msg;
                    }
            oSqlCon = new SQLiteConnection($"Data Source={cNomBD};Version=3;New={(File.Exists(sDBFile) ? "False" : "True")};Compress=True;");
            oSqlCon.Open();
            }
        //_______________________________________________________________________________________________________________________
        public void CloseDB() { oSqlCon.Close(); }
        //_______________________________________________________________________________________________________________________
        public DataTable executeQuery(string sSql, SQLiteParameter[] parameters = null) {
            try {
                SQLiteCommand oSqlCmd = new SQLiteCommand(sSql, oSqlCon);
                if (parameters != null && parameters.Length > 0)
                    oSqlCmd.Parameters.AddRange(parameters);
                SQLiteDataAdapter oSqlAdapter = new SQLiteDataAdapter(oSqlCmd);
                DataSet dataSet = new DataSet();
                oSqlAdapter.Fill(dataSet);
                return dataSet.Tables[0];
                }
            catch (Exception ex) {
                string msg = $"Error in executeQuery. Query: {(sSql ?? "N/A")}. Message:{ex.Message}\r\n{ex.StackTrace}";
                Console.WriteLine(msg);
                _LastErrorMessages += (string.IsNullOrWhiteSpace(_LastErrorMessages) ? "" : "\r\n") + msg;
                return new DataTable();
                }
            }
        //_______________________________________________________________________________________________________________________
        public int ExecuteNonQuery(string sSql, SQLiteParameter[] parameters = null) {
            try {
                SQLiteCommand oSqlCmd = new SQLiteCommand(sSql, oSqlCon);
                if (parameters != null && parameters.Length > 0)
                    oSqlCmd.Parameters.AddRange(parameters);
                return oSqlCmd.ExecuteNonQuery();
                }
            catch (Exception ex) {
                string msg = $"Error in ExecuteNonQuery. Query: {(sSql ?? "N/A")}. Message:{ex.Message}\r\n{ex.StackTrace}";
                Console.WriteLine(msg);
                _LastErrorMessages += (string.IsNullOrWhiteSpace(_LastErrorMessages)?"":"\r\n") + msg;
                return -1;
                }
            }
        //_______________________________________________________________________________________________________________________
        public object ExecuteScalar(string sSql, SQLiteParameter[] parameters = null) {
            try {
                SQLiteCommand oSqlCmd = new SQLiteCommand(sSql, oSqlCon);
                if (parameters != null && parameters.Length > 0)
                    oSqlCmd.Parameters.AddRange(parameters);
                return oSqlCmd.ExecuteScalar();
                }
            catch (Exception ex) {
                string msg = $"Error in ExecuteScalar. Query: {(sSql ?? "N/A")}. Message:{ex.Message}\r\n{ex.StackTrace}";
                Console.WriteLine(msg);
                _LastErrorMessages += (string.IsNullOrWhiteSpace(_LastErrorMessages) ? "" : "\r\n") + msg;
                return null;
                }
            }
        }
    }
