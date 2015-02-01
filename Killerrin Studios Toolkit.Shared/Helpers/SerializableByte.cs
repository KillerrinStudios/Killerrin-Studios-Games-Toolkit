using System;
using System.Collections.Generic;
using System.Text;
using KillerrinStudiosToolkit.Interfaces;

namespace KillerrinStudiosToolkit.Helpers
{
    public class SerializableByte : ISerializable
    {
        public Byte[] Data { get; set; }

        public SerializableByte(byte[] content)
        {
            Data = content;
        }

        public static implicit operator SerializableByte(SerializableString serializableString)
        {
            return new SerializableByte(serializableString.Serialize());
        }

        byte[] ISerializable.Serialize() { return Serialize(); }
        public Byte[] Serialize()
        {
            return Data;
        }

        object ISerializable.Deserialize(byte[] data) { return Deserialize(); }
        public object Deserialize()
        {
            return Data;
        }
    }
}
