﻿//_______________________________________________________________________________________________________________________
namespace FIMTestConfigurator {
    //_______________________________________________________________________________________________________________________
    public class Variable : TestObjectBase {
        //_______________________________________________________________________________________________________________________
        public string Value { get; set; }
        public string Desc { get; set; }
        //_______________________________________________________________________________________________________________________
        public Variable(long id, string name, string value, string desc) {
            Table = "variable";
            Alias = "Variable";
            Id = id;
            Name = name;
            Value = value;
            Desc = desc;
            }
        }
    }
