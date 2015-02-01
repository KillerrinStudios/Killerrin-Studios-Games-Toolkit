using KillerrinStudiosToolkit.Datastructures;
using KillerrinStudiosToolkit.Enumerators;
using System;
using System.Collections.Generic;
using System.Text;

namespace KillerrinStudiosToolkit.Events
{
    public delegate void OnConnectedEventHandler(object sender, OnConnectedEventArgs e);

    public class OnConnectedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {
        public NetworkConnectionEndpoint? NetworkConnectionEndpoint { get; private set; }
        public NetworkType NetworkType { get; private set; }

        public OnConnectedEventArgs()
            : base(new Exception(), false, null)
        {
            NetworkConnectionEndpoint = null;
        }

        public OnConnectedEventArgs(NetworkConnectionEndpoint? networkConnectionEndpoint, NetworkType networkType)
            : base(new Exception(), false, null)
        {
            NetworkConnectionEndpoint = networkConnectionEndpoint;
            NetworkType = networkType;
        }

        public OnConnectedEventArgs(NetworkConnectionEndpoint? networkConnectionEndpoint, NetworkType networkType, Exception e, bool canceled, Object state)
            : base(e, canceled, state)
        {
            NetworkConnectionEndpoint = networkConnectionEndpoint;
            NetworkType = networkType;
        }
    }
}
