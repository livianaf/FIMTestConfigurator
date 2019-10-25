namespace FIMTestConfigurator {
    public class Script : TestObjectBase {
        //_______________________________________________________________________________________________________________________
        public string Desc { get; set; }
        public string FileContent { get; set; }
        //_______________________________________________________________________________________________________________________
        public Script( long id, string name, string desc, string fileContent ) {
            Table = "script";
            Alias = "Script";
            Id = id;
            Name = name;
            Desc = desc;
            FileContent = fileContent;
            }
        //_______________________________________________________________________________________________________________________
        public override string CheckResult() {
            ResetErrors();
            if (string.IsNullOrWhiteSpace(Name)) { ErrorsMessages += "- " + Alias + " Name is not valid\r\n"; ContainsErrors = true; }
            if (string.IsNullOrWhiteSpace(FileContent)) { ErrorsMessages += "- File Content is empty\r\n"; ContainsErrors = true; }
            return checkResultContent;
            }
        }
    }
