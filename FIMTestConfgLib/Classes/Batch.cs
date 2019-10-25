using System;
using System.Collections.Generic;

//_______________________________________________________________________________________________________________________
namespace FIMTestConfigurator {
    //_______________________________________________________________________________________________________________________
    public class Batch : TestObjectBase {
        //_______________________________________________________________________________________________________________________
        public List<Group> Groups { get; set; }
        //_______________________________________________________________________________________________________________________
        public Batch( long id, string name, List<Group> groups ) {
            Table = "batch";
            Alias = "Batch";
            Id = id;
            Name = name;
            Groups = groups;
            }
        //_______________________________________________________________________________________________________________________
        public override string AdditionalInfo() {
            AdditionalLinkedInfo.Clear();
            string sMenu = " (add)", sMenuLnk = $"#add:{Link}:groups";
            string additionalInfo = AddInfoItem("Total groups", Groups.Count.ToString(), true, sMenu: sMenu);
            AddLinkInfoItem(additionalInfo, null, sMenu: sMenu, sMenuLnk: sMenuLnk);
            foreach (Group t in Groups) {
                sMenu = " (remove)"; sMenuLnk = $"#remove:{Link}:{t.Link}";
                additionalInfo += AddInfoItem("Group", t.Name, false, true, sMenu: sMenu);
                AddLinkInfoItem(additionalInfo, t, sMenu: sMenu, sMenuLnk: sMenuLnk);
                }
            return additionalInfo;
            }
        //_______________________________________________________________________________________________________________________
        public override string CheckResult() {
            ResetErrors();
            if (string.IsNullOrWhiteSpace(Name)) { ErrorsMessages += "- " + Alias + " Name is not valid\r\n"; ContainsErrors = true; }
            if (Groups.Count == 0) { ErrorsMessages += "- No groups in this batch!\r\n"; ContainsErrors = true; }
            return checkResultContent;
            }
        }
    }
