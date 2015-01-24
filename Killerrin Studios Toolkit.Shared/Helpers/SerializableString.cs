using System;
using System.Collections.Generic;
using System.Text;
using KillerrinStudiosToolkit.Interfaces;
using System.Diagnostics;

namespace KillerrinStudiosToolkit.Helpers
{
    public class SerializableString : ISerializable
    {
        string m_content;
        public string Content { get { return m_content; } protected set { m_content = value; } }

        private byte[] m_data;
        public byte[] Data { get { return m_data; } protected set { m_data = value; } }

        public SerializableString(string content)
        {
            Content = content;
            Data = Serialize();
        }

        public SerializableString(byte[] data)
        {
            Data = data;
            Content = Deserialize();
        }

        public SerializableString(SerializableByte saveableByte)
        {
            Data = saveableByte.Data;
            try { Content = Encoding.UTF8.GetString(Data, 0, Data.Length); }
            catch (Exception) { Content = ""; }
        }

        #region Operator Overloads
        public static implicit operator SerializableString(SerializableByte saveableByte)
        {
            return new SerializableString(saveableByte);
        }
        #endregion

        byte[] ISerializable.Serialize() { return Serialize(); }
        public Byte[] Serialize()
        {
            Debug.WriteLine("Content Serialized");
            return Encoding.UTF8.GetBytes(m_content);
        }

        object ISerializable.Deserialize() { return Deserialize(); }
        public string Deserialize()
        {
            try {
                string deserial = Encoding.UTF8.GetString(Data, 0, Data.Length);

                Debug.WriteLine("Content Deserialized");
                return deserial;
            }
            catch (Exception) 
            {
                Debug.WriteLine("Content Deserializing Failed");
                return ""; 
            }
        }
    }
}
