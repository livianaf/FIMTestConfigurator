//_______________________________________________________________________________________________________________________
namespace FIMTestConfigurator {
    //_______________________________________________________________________________________________________________________
    public class Source : TestObjectBase {
        //_______________________________________________________________________________________________________________________
        public string Server { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string AuthType { get; set; }
        //_______________________________________________________________________________________________________________________
        public Source(long id, string name, string server, string user, string password, string authType) {
            Table = "source";
            Alias = "Source";
            Id = id;
            Name = name;
            Server = server;
            User = user;
            Password = password;
            AuthType = authType;
            }
        //_______________________________________________________________________________________________________________________
        public override string CheckResult() {
            ResetErrors();
            if (string.IsNullOrWhiteSpace(Name)) { ErrorsMessages += "- " + Alias + " Name is not valid\r\n"; ContainsErrors = true; }
            if (string.IsNullOrWhiteSpace(Server)) { ErrorsMessages += "- Server is empty\r\n"; ContainsErrors = true; }
            if (string.IsNullOrWhiteSpace(User)) { ErrorsMessages += "- User is empty\r\n"; ContainsErrors = true; }
            if (string.IsNullOrWhiteSpace(Password)) { ErrorsMessages += "- Password is empty\r\n"; ContainsErrors = true; }
            return checkResultContent;
            }
        }
    }
