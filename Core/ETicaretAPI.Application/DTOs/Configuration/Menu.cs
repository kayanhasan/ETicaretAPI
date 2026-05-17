using System;
using System.Collections.Generic;
using System.Text;

namespace ETicaretAPI.Application.DTOs.Configuration
{
    public class Menu
    {
        public string Name { get; set; }
        public List<Action> Actions { get; set; } = new();
    }
}
