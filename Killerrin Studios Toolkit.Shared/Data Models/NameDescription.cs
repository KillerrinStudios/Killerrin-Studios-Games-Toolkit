using System;
using System.Collections.Generic;
using System.Text;

namespace KillerrinStudiosToolkit.Data_Models
{
    public class NameDescription
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public NameDescription()
        {
            Name = "";
            Description = "";
        }

        public NameDescription(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
