using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using KillerrinStudiosToolkit.Managers;

namespace KillerrinStudiosToolkit.Datastructures
{
    public class Packet
    {
        public uint PacketID { get; protected set; }
        public bool RequiresAck { get; protected set; }
        public DateTime Timestamp { get; protected set; }

        public Packet(bool requiresAck)
        {
            PacketID = PacketIDManager.GetNewID();
            RequiresAck = requiresAck;
            Timestamp = DateTime.UtcNow;
        }

        public virtual void SetFromOtherPacket(Packet o)
        {
            PacketID = o.PacketID;
            RequiresAck = o.RequiresAck;
            Timestamp = o.Timestamp;
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
    }
}
