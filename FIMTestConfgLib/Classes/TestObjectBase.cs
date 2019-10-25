using System;
using System.Collections.Generic;

//_______________________________________________________________________________________________________________________
namespace FIMTestConfigurator {
    //_______________________________________________________________________________________________________________________
    public class TestObjectBase {
        //_______________________________________________________________________________________________________________________
        public const long NO_ID = -1;
        public long Id { get; set; }
        public string Name { get; set; }
        public string Alias { get; internal set; }
        public virtual string AdditionalInfo() { return ""; }
        public List<Tuple<int, int, string>> AdditionalLinkedInfo { get; internal set; } = new List<Tuple<int, int, string>>();
        public virtual string CheckResult() { return ""; }
        public bool ContainsErrors { get; internal set; }
        public string ErrorsMessages { get; internal set; }
        internal void ResetErrors() { ErrorsMessages = ""; ContainsErrors = false; }
        internal string checkResultContent => "\r\nChecking " + Alias + " " + Name + "\r\n" + (ContainsErrors ? ErrorsMessages : "- Correct!\r\n"); 
        public string Table { get; internal set; }
        public string Link => Table + "|" + Id; 
        //_______________________________________________________________________________________________________________________
        public static Type TypeOfLink(object oLink) {
            if (oLink == null) return null;
            if (oLink.GetType().Equals(typeof(string))) return TypeOfLink((string)oLink);
            else return null;
            }
        //_______________________________________________________________________________________________________________________
        private static Type TypeOfLink(string sLink) {
            if (!IsValidLink(sLink, out string Name, out long id)) return null;
            switch (Name) {
                case "batch": return typeof(Batch);
                case "groups": return typeof(Group);
                case "test": return typeof(Test);
                case "inputset": return typeof(InputSet);
                case "managementagent": return typeof(ManagementAgentInfo);
                case "outputset": return typeof(OutputSet);
                case "config": return typeof(Config);
                case "source": return typeof(Source);
                case "script": return typeof(Script);
                default: return null;
                }
            }
        //_______________________________________________________________________________________________________________________
        public static bool IsValidLink(string sLink) { if (!IsValidLink(sLink, out string Name, out long id)) return false; return true; }
        //_______________________________________________________________________________________________________________________
        private static bool IsValidLink(string sLink, out string Name, out long Id) {
            Name = null; Id = NO_ID;
            if (string.IsNullOrWhiteSpace(sLink) || !sLink.Contains("|")) return false;
            string[] lValores = sLink.Split('|');
            if (lValores.Length > 2) return false;
            if (!long.TryParse(lValores[1], out long id)) return false;
            Name = lValores[0].ToLower(); Id = id;
            return true;
            }
        //_______________________________________________________________________________________________________________________
        public static TestObjectBase LinkedObject(object oLink) {
            if (oLink == null) return null;
            if (oLink.GetType().Equals(typeof(string))) return LinkedObject((string)oLink);
            else return null;
            }
        //_______________________________________________________________________________________________________________________
        public static TestObjectBase LinkedObject (string sLink){
            if (!IsValidLink(sLink, out string Name, out long id)) return null;
            switch (Name) {
                case "batch": return TestsHelper.getBatchByID(id);
                case "groups": return TestsHelper.getGroupByID(id);
                case "test": return TestsHelper.getTestByID(id);
                case "inputset": return TestsHelper.getInputSetByID(id);
                case "managementagent": return TestsHelper.getMAByID(id);
                case "outputset": return TestsHelper.getOutputSetByID(id);
                case "config": return TestsHelper.getConfigByID(id);
                case "source": return TestsHelper.getSourceByID(id);
                case "script": return TestsHelper.getScriptByID(id);
                case "location": return TestsHelper.getLocationByID(id);
                case "variable": return TestsHelper.getVariableByID(id);
                default: return null;
                }
            }
        //_______________________________________________________________________________________________________________________
        public static string ActionLinkHeader = "%";
        public static string RemoveActionLinkHeader = "%remove:";
        public static string AddActionLinkHeader = "%add:";
        public static string SetActionLinkHeader = "%set:";
        public static bool IsActionLink( string sLink ) { return sLink.iStartsWith(ActionLinkHeader); }
        public static bool IsRemoveActionLink( string sLink ) { return sLink.iStartsWith(RemoveActionLinkHeader); }
        public static bool IsAddActionLink( string sLink ) { return sLink.iStartsWith(AddActionLinkHeader); }
        public static bool IsSetActionLink( string sLink ) { return sLink.iStartsWith(SetActionLinkHeader); }
        //_______________________________________________________________________________________________________________________
        internal string AddInfoItem(string sName, string sValue, bool Header = false, bool SubItem = false, string sMenu = "") {
            return (Header ? "Additional Info:\n" : "") + new string(' ',10)  + (SubItem ? "- " : "") + sName + ": " + sValue + sMenu + "\n";//(SubItem ? " (remove)" :(string.IsNullOrEmpty(sValue)?" (set)": " (add)")) + 
            }
        //_______________________________________________________________________________________________________________________
        internal void AddLinkInfoItem(string additionalInfo, TestObjectBase o, string sMenu = "", string sMenuLnk = "" ) {
            if (o != null) AdditionalLinkedInfo.Add(new Tuple<int, int, string>(additionalInfo.Length - (sMenu.Length) - o.Name.Length - 1, o.Name.Length, o.Link));
            if (sMenu != "" && sMenuLnk != "") AdditionalLinkedInfo.Add(new Tuple<int, int, string>(additionalInfo.Length - (sMenu.Length) + 1, sMenu.Length - 3, sMenuLnk));
            }
        //_______________________________________________________________________________________________________________________
        internal bool SearchErrorsInDetail(string Detail) {
            if (string.IsNullOrWhiteSpace(Detail)) { ErrorsMessages += "- Detail is empty\r\n"; return true; }

            string[] attributes = Detail.Replace("\r\n", "\n").Replace("\r", "\n").Split('\n');
            long count = 0, totalcount = 0;
            foreach (string s in attributes) {
                totalcount++;
                if (s.StartsWith("/")) continue;
                if (string.IsNullOrWhiteSpace(s)) continue;
                count++;
                if (!s.Contains("|")) { ErrorsMessages += "- Detail: line " + totalcount + " is not correct ('|' not found in line "+ totalcount + ").\r\n"; return true; }
                string[] attribute = s.Split('|');
                if (string.IsNullOrWhiteSpace(attribute[0]) || attribute[0].Contains(" ")) { ErrorsMessages += "- Detail: line " + totalcount + " is not correct (whitespace found in attribute name in line " + totalcount + ").\r\n"; return true; }
                }
            if (count == 0) { ErrorsMessages += "- Detail: all lines are commented.\r\n"; return false; }
            return false;
            }
        }
    }
