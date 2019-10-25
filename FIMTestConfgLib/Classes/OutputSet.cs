using System;

//_______________________________________________________________________________________________________________________
namespace FIMTestConfigurator {
    //_______________________________________________________________________________________________________________________
    public class OutputSet : TestObjectBase {
        //_______________________________________________________________________________________________________________________
        public string Detail { get; set; }
        public long DetailLines {
            get {
                if (string.IsNullOrWhiteSpace(Detail)) return 0;
                return Detail.Split('\n').Length;
                }
            }
        public string Type { get; set; }
        public bool MARequired {
            get {
                Enum.TryParse(Type, out AllowedTypes curtype);
                return (((int)curtype & (int)TypeRequires.MA) == (int)TypeRequires.MA);
                }
            }
        public bool DetailRequired {
            get {
                Enum.TryParse(Type, out AllowedTypes curtype);
                return (((int)curtype & (int)TypeRequires.Detail) == (int)TypeRequires.Detail);
                }
            }
        [Flags]
        public enum TypeRequires {
            Nothing = 0,
            MA = 1,
            Detail = 2
            }
        public enum AllowedTypes {
            filter = 0,
            join = 0,
            importflow = 2,
            project = 0,
            provisionadd = 1,
            provisionrename = 1,
            deprovisiondisconnect = 1,
            deprovisionexport = 3,
            deprovisiondelete = 1,
            exportflow = 3,
            noaction = 0,
            mvdeletion = 0
            }
        public ManagementAgentInfo DestinationManagementAgent { get; set; }
        //_______________________________________________________________________________________________________________________
        public OutputSet(long id, string name, string detail, string type, ManagementAgentInfo destinationManagementAgent) {
            Table = "outputset";
            Alias = "OutputSet";
            Id = id;
            Name = name;
            Detail = detail;
            Type = type;
            DestinationManagementAgent = destinationManagementAgent;
            }
        //_______________________________________________________________________________________________________________________
        public override string AdditionalInfo() {
            AdditionalLinkedInfo.Clear();
            Enum.TryParse(Type, out AllowedTypes curtype);

            string sMenu = (DestinationManagementAgent == null ? " (set)" : " (remove)"), sMenuLnk = (DestinationManagementAgent == null ? $"{TestObjectBase.SetActionLinkHeader}{Link}:managementagent" : $"{TestObjectBase.RemoveActionLinkHeader}{Link}:{DestinationManagementAgent.Link}");
            // Si el MA no es necesario quita la opción de establecerlo
            if (DestinationManagementAgent == null && !MARequired) { sMenu = sMenuLnk = ""; }

            string additionalInfo = AddInfoItem("MA " + (MARequired ? "(Required)" : "(Not Required)"), DestinationManagementAgent?.Name, true, sMenu: sMenu);
            AddLinkInfoItem(additionalInfo, DestinationManagementAgent, sMenu: sMenu, sMenuLnk: sMenuLnk);
            additionalInfo += AddInfoItem("Detail " + (DetailRequired ? "(Required)" : "(Not Required)"), DetailLines + " lines");
            return additionalInfo;
            }
        //_______________________________________________________________________________________________________________________
        public override string CheckResult() {
            ResetErrors();
            if (string.IsNullOrWhiteSpace(Name)) { ErrorsMessages += "- " + Alias + " Name is not valid\r\n"; ContainsErrors = true; }
            if (string.IsNullOrWhiteSpace(Type)) { ErrorsMessages += "- Type is empty\r\n"; ContainsErrors = true; return checkResultContent; }
            else if (!Enum.TryParse(Type, out OutputSet.AllowedTypes curtype)) { ErrorsMessages += "- Type is not valid\r\n"; ContainsErrors = true; return checkResultContent; }
            else {
                foreach (string name in TestsHelper.getOutputSetTypes()) {
                    if (Enum.TryParse(name, out OutputSet.AllowedTypes type) && Type.iEquals(name)) {

                        // Si no requiere nada
                        if (((OutputSet.TypeRequires)type | OutputSet.TypeRequires.Nothing) == OutputSet.TypeRequires.Nothing) {
                            if (DestinationManagementAgent != null) { ErrorsMessages += "- MA is not empty\r\n"; ContainsErrors = true; }
                            if (!string.IsNullOrWhiteSpace(Detail)) { ErrorsMessages += "- Detail is not empty\r\n"; ContainsErrors = true; }
                            }
                        // Si requiere MA
                        if (((OutputSet.TypeRequires)type & OutputSet.TypeRequires.MA) == OutputSet.TypeRequires.MA) {
                            if (DestinationManagementAgent == null) { ErrorsMessages += "- MA is empty\r\n"; ContainsErrors = true; }
                            // Si sólo requiere MA
                            if (((int)type & (int)OutputSet.TypeRequires.Detail) != (int)OutputSet.TypeRequires.Detail) {
                                if (!string.IsNullOrWhiteSpace(Detail)) { ErrorsMessages += "- Detail is not empty\r\n"; ContainsErrors = true; }
                                }
                            }
                        // Si requiere Detail
                        if (((OutputSet.TypeRequires)type & OutputSet.TypeRequires.Detail) == OutputSet.TypeRequires.Detail) {
                            if (SearchErrorsInDetail(Detail)) ContainsErrors = true;
                            if (((OutputSet.TypeRequires)type & OutputSet.TypeRequires.MA) != OutputSet.TypeRequires.MA) {
                                if (DestinationManagementAgent != null) { ErrorsMessages += "- MA is not empty\r\n"; ContainsErrors = true; }
                                }
                            }
                        }
                    }
                }
            return checkResultContent;
            }
        }
    }
