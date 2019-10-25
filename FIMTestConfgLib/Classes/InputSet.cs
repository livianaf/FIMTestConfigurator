//_______________________________________________________________________________________________________________________
namespace FIMTestConfigurator {
    //_______________________________________________________________________________________________________________________
    public class InputSet : TestObjectBase {
        //_______________________________________________________________________________________________________________________
        public string Detail { get; set; }
        public long DetailLines {
            get {
                if (string.IsNullOrWhiteSpace(Detail)) return 0;
                return Detail.Split('\n').Length;
                }
            }
        public string DistinguishedName { get; set; }
        //_______________________________________________________________________________________________________________________
        public InputSet(long id, string name, string detail, string distinguishedName) {
            Table = "inputset";
            Alias = "InputSet";
            Id = id;
            Name = name;
            Detail = detail;
            DistinguishedName = distinguishedName;
            }
        //_______________________________________________________________________________________________________________________
        public override string AdditionalInfo() { return AddInfoItem("Detail", DetailLines + " lines", true); }
        //_______________________________________________________________________________________________________________________
        public override string CheckResult() {
            ResetErrors();
            if (string.IsNullOrWhiteSpace(Name)) { ErrorsMessages += "- " + Alias + " Name is not valid\r\n"; ContainsErrors = true; }
            if (string.IsNullOrWhiteSpace(DistinguishedName)) { ErrorsMessages += "- DN is empty\r\n"; ContainsErrors = true; }
            if (SearchErrorsInDetail(Detail)) ContainsErrors = true;
            return checkResultContent;
            }
        }
    }
