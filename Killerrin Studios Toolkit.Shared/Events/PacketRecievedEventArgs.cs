using KillerrinStudiosToolkit.Datastructures;
using System;
using System.Collections.Generic;
using System.Text;

namespace KillerrinStudiosToolkit.Events
{
    public delegate void PacketRecievedEventHandler(object sender, PacketRecievedEventArgs e);

    public class PacketRecievedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {
        public Packet Packet { get; protected set; }
        public PacketRecievedEventArgs(Packet packet)
            : base(new Exception(), false, null)
        {
            Packet = packet;
        }
        public PacketRecievedEventArgs(Packet packet, Exception e, bool canceled, Object state)
            : base(e, canceled, state)
        {
            Packet = packet;
        }
    }
}
