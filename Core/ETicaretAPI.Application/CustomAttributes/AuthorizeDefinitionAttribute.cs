using ETicaretAPI.Application.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETicaretAPI.Application.CustomAttributes
{
    public class AuthorizeDefinitionAttribute : Attribute
    {
        public string Menu { get; set; }
        public string Definition { get; set; }
        public ActionType ActionType { get; set; }
    }
}
