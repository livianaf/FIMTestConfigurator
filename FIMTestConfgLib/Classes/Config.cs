using System;
using System.IO;
using System.Xml;

//_______________________________________________________________________________________________________________________
namespace FIMTestConfigurator {
    //_______________________________________________________________________________________________________________________
    public class Config : TestObjectBase {
        //_______________________________________________________________________________________________________________________
        public string Desc { get; set; }
        public string FileContent { get; set; }
        //_______________________________________________________________________________________________________________________
        public Config(long id, string name, string desc, string fileContent) {
            Table = "config";
            Alias = "Config";
            Id = id;
            Name = name;
            Desc = desc;
            FileContent = fileContent;
            }
        //_______________________________________________________________________________________________________________________
        public override string CheckResult() {
            ResetErrors();
            if (string.IsNullOrWhiteSpace(Name) || 
               Name.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0 || 
               !Path.GetExtension(Name).iEquals(".xml")) {
                ErrorsMessages += "- File Name is not valid\r\n"; ContainsErrors = true; return checkResultContent;
                }
            if (string.IsNullOrWhiteSpace(FileContent)) { ErrorsMessages += "- File Content is empty\r\n"; ContainsErrors = true; return checkResultContent; }
            try {
                new XmlDocument().LoadXml(FileContent);
                }
            catch (Exception ex) {
                ErrorsMessages += "- File Content is not correct: " + ex.Message + "\r\n";
                ContainsErrors = true;
                }
            return checkResultContent;
            }
        }
    }
