using System;
using System.Collections.Generic;
using System.Text;
using KillerrinStudiosToolkit.Interfaces;

namespace KillerrinStudiosToolkit.Helpers
{
    public class SaveableByte : ISaveable
    {
        public Byte[] Content { get; set; }

        public SaveableByte(string content)
        {
            Content = Encoding.UTF8.GetBytes(content);
        }
        public SaveableByte(byte[] content)
        {
            Content = content;
        }

        byte[] ISaveable.Save() { return Save(); }
        public Byte[] Save()
        {
            return Content;
        }
    }
}
