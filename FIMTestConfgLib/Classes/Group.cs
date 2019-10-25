using System;
using System.Collections.Generic;

//_______________________________________________________________________________________________________________________
namespace FIMTestConfigurator {
    //_______________________________________________________________________________________________________________________
    public class Group : TestObjectBase {
        //_______________________________________________________________________________________________________________________
        public List<Test> Tests { get; set; }
        //_______________________________________________________________________________________________________________________
        public Group(long id, string name, List<Test> tests) {
            Table = "groups";
            Alias = "Group";
            Id = id;
            Name = name;
            Tests = tests;
            }
        //_______________________________________________________________________________________________________________________
        public override string AdditionalInfo() {
            AdditionalLinkedInfo.Clear();
            string sMenu = " (add)", sMenuLnk = $"#add:{Link}:test";
            string additionalInfo = AddInfoItem("Total tests", Tests.Count.ToString(), sMenu: sMenu);
            AddLinkInfoItem(additionalInfo, null, sMenu: sMenu, sMenuLnk: sMenuLnk);
            foreach (Test t in Tests) {
                sMenu = " (remove)"; sMenuLnk = $"#remove:{Link}:{t.Link}";
                additionalInfo += AddInfoItem("Test", t.Name, false, true, sMenu: sMenu);
                AddLinkInfoItem(additionalInfo, t, sMenu: sMenu, sMenuLnk: sMenuLnk);
                }
            return additionalInfo;
            }
        //_______________________________________________________________________________________________________________________
        public override string CheckResult() {
            ResetErrors();
            if (string.IsNullOrWhiteSpace(Name)) { ErrorsMessages += "- " + Alias + " Name is not valid\r\n"; ContainsErrors = true; }
            if (Tests.Count == 0) { ErrorsMessages += "- No tests in this group!\r\n"; ContainsErrors = true; }
            return checkResultContent;
            }
        }
    }
