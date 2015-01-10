using System;
using System.Collections.Generic;
using System.Text;
using KillerrinStudiosToolkit.Interfaces;

namespace KillerrinStudiosToolkit.Helpers
{
    public class SaveableString : ISaveable
    {
        public string Content { get; set; }

        public SaveableString(string content)
        {
            Content = content;
        }

        byte[] ISaveable.Save() { return Save(); }
        public Byte[] Save()
        {
            return Encoding.UTF8.GetBytes(Content);
        }
    }
}
