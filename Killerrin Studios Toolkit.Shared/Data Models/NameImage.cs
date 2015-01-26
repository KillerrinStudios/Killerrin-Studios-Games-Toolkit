using System;
using System.Collections.Generic;
using System.Text;

namespace KillerrinStudiosToolkit.Data_Models
{
    public class NameImage
    {
        public string Name { get; set; }
        public Uri Image { get; set; }

        public NameImage()
        {
            Name = "";
            Image = new Uri("http://www.killerrin.com");
        }

        public NameImage(string name, Uri image)
        {
            Name = name;
            Image = image;
        }
    }
}
