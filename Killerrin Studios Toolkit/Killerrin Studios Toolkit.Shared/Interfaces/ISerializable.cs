using System;
using System.Collections.Generic;
using System.Text;

namespace KillerrinStudiosToolkit.Interfaces
{
    
    public interface ISerializable
    {
        byte[] Serialize();
        object Deserialize();
    }
}
