using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using KillerrinStudiosToolkit.Managers;
using KillerrinStudiosToolkit.Interfaces;
using KillerrinStudiosToolkit.Helpers;

namespace KillerrinStudiosToolkit.Datastructures
{
    public class Packet
    {
        public uint PacketID { get; protected set; }
        public DateTime Timestamp { get; protected set; }
        public bool RequiresAck { get; protected set; }

        //public Packet() { }
        public Packet(bool requiresAck)
        {
            PacketID = PacketIDManager.GetNewID();
            RequiresAck = requiresAck;
            Timestamp = DateTime.UtcNow;
        }
        public Packet(byte[] data)
        {
            Deserialize(data);
        }

        protected virtual void SetFromOtherPacket(Packet o)
        {
            PacketID = o.PacketID;
            Timestamp = o.Timestamp;
            RequiresAck = o.RequiresAck;
        }

        #region JSON Tools
        public virtual string ThisToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public virtual void JsonToThis(string json)
        {
            JObject jObject = JObject.Parse(json);
            Packet packet = JsonConvert.DeserializeObject<Packet>(jObject.ToString());

            SetFromOtherPacket(packet);
        }
        #endregion

        #region Serialization
        public byte[] Serialize() { return (new SerializableString(ThisToJson())).Serialize(); }
        public void Deserialize(byte[] data)
        {
            SerializableString ss = new SerializableString(data);
            string json = ss.Content;

            JsonToThis(json);
        }
        #endregion

        public override string ToString()
        {
            return ThisToJson();
        }
    }
}
