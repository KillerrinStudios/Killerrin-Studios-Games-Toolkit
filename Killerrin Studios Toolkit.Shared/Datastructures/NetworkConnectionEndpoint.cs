﻿using System;
using System.Collections.Generic;
using System.Text;
using Windows.Networking;

namespace KillerrinStudiosToolkit.Datastructures
{
    public struct NetworkConnectionEndpoint
    {
        public HostName HostName;
        public string Port;

        public NetworkConnectionEndpoint(string hostName = "localhost", string portOrServiceID = "11321")
        {
            HostName = new HostName(hostName);
            Port = portOrServiceID;
        }

        public NetworkConnectionEndpoint(HostName hostName, string portOrServiceID)
        {
            HostName = hostName;
            Port = portOrServiceID;
        }

        public override string ToString()
        {
            return "IP: " + HostName.RawName + " | " + "Port: " + Port;
        }
    }
}
