using System;
using System.Collections.Generic;
using System.Text;
using KillerrinStudiosToolkit.Interfaces;

namespace KillerrinStudiosToolkit.Helpers
{
    public class SerializableString : ISerializable
    {
        string m_content;
        public string Content { get { return m_content; } set { m_content = value; Data = SerializeString(m_content); } }

        private byte[] m_data;
        public byte[] Data { get { return m_data; } set { m_data = value; Content = DeserializeString(m_data); } }

        public SerializableString(string content)
        {
            Content = content;
        }

        public SerializableString(byte[] data)
        {
            Data = data;
        }

        public SerializableString(SerializableByte saveableByte)
        {
            Data = saveableByte.Data;
        }

        #region Static Methods
        public static byte[] SerializeString(string str) { return Encoding.UTF8.GetBytes(str); }
        public static string DeserializeString(byte[] data) { return Encoding.UTF8.GetString(data, 0, data.Length); }
        #endregion

        #region Operator Overloads
        public static implicit operator SerializableString(SerializableByte saveableByte)
        {
            return new SerializableString(saveableByte);
        }
        #endregion

        byte[] ISerializable.Serialize() { return Serialize(); }
        public Byte[] Serialize()
        {
            return Data;
        }

        object ISerializable.Deserialize() { return Deserialize(); }
        public object Deserialize()
        {
            return Content;
        }
    }
}
