using System.Collections.Generic;

//_______________________________________________________________________________________________________________________
namespace FIMTestConfigurator {
    //_______________________________________________________________________________________________________________________
    public class Location : TestObjectBase {
        //_______________________________________________________________________________________________________________________
        public string Desc { get; set; }
        public List<Variable> Variables { get; set; }
        //_______________________________________________________________________________________________________________________
        public Location(long id, string name, string desc, List<Variable> variables) {
            Table = "location";
            Alias = "Location";
            Id = id;
            Name = name;
            Desc = desc;
            Variables = variables;
            }
        }
    }
