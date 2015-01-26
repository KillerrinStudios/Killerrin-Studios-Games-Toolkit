using System;
using System.Collections.Generic;
using System.Text;

namespace KillerrinStudiosToolkit.Data_Models
{
    public class NameImageDescription
    {
        public string Name { get; set; }
        public Uri Image { get; set; }
        public string Description { get; set; }

        public NameImageDescription()
        {
            Name = "";
            Image = new Uri("http://www.killerrin.com");
            Description = "";
        }

        public NameImageDescription(string name, Uri image, string description)
        {
            Name = name;
            Image = image;
            Description = description;
        }
    }
}
