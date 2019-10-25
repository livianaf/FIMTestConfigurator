//_______________________________________________________________________________________________________________________
namespace FIMTestConfigurator {
    //_______________________________________________________________________________________________________________________
    public class ManagementAgentInfo : TestObjectBase {
        //_______________________________________________________________________________________________________________________
        public Source Source { get; set; }
        //_______________________________________________________________________________________________________________________
        public ManagementAgentInfo(long id, string name, Source source) {
            Table = "managementagent";
            Alias = "MA";
            Id = id;
            Name = name;
            Source = source;
            }
        //_______________________________________________________________________________________________________________________
        public override string AdditionalInfo() {
            AdditionalLinkedInfo.Clear();
            string sMenu = (Source == null ? " (set)" : " (remove)"), sMenuLnk = (Source == null ? $"#set:{Link}:source" : $"#remove:{Link}:{Source.Link}");
            string additionalInfo = AddInfoItem("Source", Source?.Name, true, sMenu: sMenu);
            AddLinkInfoItem(additionalInfo, Source, sMenu: sMenu, sMenuLnk: sMenuLnk);
            return additionalInfo;
            }
        //_______________________________________________________________________________________________________________________
        public override string CheckResult() {
            ResetErrors();
            if (string.IsNullOrWhiteSpace(Name)) { ErrorsMessages += "- " + Alias + " Name is not valid\r\n"; ContainsErrors = true; }
            if (Source == null) { ErrorsMessages += "- Source is empty\r\n"; ContainsErrors = true; }
            return checkResultContent;
            }
        }
    }
