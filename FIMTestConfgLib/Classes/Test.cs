using System;
using System.Collections.Generic;

//_______________________________________________________________________________________________________________________
namespace FIMTestConfigurator {
    //_______________________________________________________________________________________________________________________
    public class Test : TestObjectBase {
        //_______________________________________________________________________________________________________________________
        public string Detail { get; set; }
        public Config Config { get; set; }
        public Script Script { get; set; }
        public InputSet InputSet { get; set; }
        public List<OutputSet> OutputSets { get; set; }
        public ManagementAgentInfo SourceManagementAgent { get; set; }
        public bool Delta { get; set; }
        public bool Commit { get; set; }
        //_______________________________________________________________________________________________________________________
        public Test(long id, string name, string detail, Config config, Script script, InputSet inputSet, List<OutputSet> outputSets, ManagementAgentInfo sourceManagementAgent, bool delta, bool commit) {
            Table = "test";
            Alias = "Test";
            Id = id;
            Name = name;
            Detail = detail;
            Config = config;
            Script = script;
            InputSet = inputSet;
            OutputSets = outputSets;
            SourceManagementAgent = sourceManagementAgent;
            Delta = delta;
            Commit = commit;
            }
        //_______________________________________________________________________________________________________________________
        public override string AdditionalInfo() {
            string sMenu, sMenuLnk, additionalInfo;
            AdditionalLinkedInfo.Clear();

            sMenu = (Config == null ? " (set)" : " (remove)"); sMenuLnk = (Config == null ? $"#set:{Link}:config" : $"#remove:{Link}:{Config.Link}");
            additionalInfo = AddInfoItem("Config", Config?.Name, true, sMenu: sMenu); AddLinkInfoItem(additionalInfo, Config, sMenu: sMenu, sMenuLnk: sMenuLnk);

            sMenu = (Script == null ? " (set)" : " (remove)"); sMenuLnk = (Script == null ? $"#set:{Link}:script" : $"#remove:{Link}:{Script.Link}");
            additionalInfo += AddInfoItem("Script", Script?.Name, sMenu: sMenu); AddLinkInfoItem(additionalInfo, Script, sMenu: sMenu, sMenuLnk: sMenuLnk);

            sMenu = (SourceManagementAgent == null ? " (set)" : " (remove)"); sMenuLnk = (SourceManagementAgent == null ? $"#set:{Link}:managementagent" : $"#remove:{Link}:{SourceManagementAgent.Link}");
            additionalInfo += AddInfoItem("Source MA", SourceManagementAgent?.Name, sMenu: sMenu); AddLinkInfoItem(additionalInfo, SourceManagementAgent, sMenu: sMenu, sMenuLnk: sMenuLnk);

            sMenu = (InputSet == null ? " (set)" : " (remove)"); sMenuLnk = (InputSet == null ? $"#set:{Link}:inputset" : $"#remove:{Link}:{InputSet.Link}");
            additionalInfo += AddInfoItem("Input Set", InputSet?.Name, sMenu: sMenu); AddLinkInfoItem(additionalInfo, InputSet, sMenu: sMenu, sMenuLnk: sMenuLnk);

            additionalInfo += AddInfoItem("DN", InputSet?.DistinguishedName);

            sMenu = " (add)"; sMenuLnk = $"#add:{Link}:outputset";
            additionalInfo += AddInfoItem("Output Sets", OutputSets?.Count.ToString(), sMenu: sMenu);
            AddLinkInfoItem(additionalInfo, null, sMenu: sMenu, sMenuLnk: sMenuLnk);
            foreach (OutputSet t in OutputSets) {
                sMenu = " (remove)"; sMenuLnk = $"#remove:{Link}:{t.Link}";
                additionalInfo += AddInfoItem("Output Set", t.Name, SubItem: true, sMenu: sMenu);
                AddLinkInfoItem(additionalInfo, t, sMenu: sMenu, sMenuLnk: sMenuLnk);
                }
            return additionalInfo;
            }
        //_______________________________________________________________________________________________________________________
        public override string CheckResult() {
            ResetErrors();
            if (string.IsNullOrWhiteSpace(Name)) { ErrorsMessages += "- " + Alias + " Name is not valid\r\n"; ContainsErrors = true; }
            if (InputSet == null) { ErrorsMessages += "- InputSet is empty\r\n"; ContainsErrors = true; }
            if (OutputSets == null || OutputSets.Count == 0) { ErrorsMessages += "- OutputSets is empty\r\n"; ContainsErrors = true; }
            if (SourceManagementAgent == null) { ErrorsMessages += "- Source MA is empty\r\n"; ContainsErrors = true; }
            return checkResultContent;
            }
        }
    }
