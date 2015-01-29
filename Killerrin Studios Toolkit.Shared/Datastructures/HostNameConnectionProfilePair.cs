using System;
using System.Collections.Generic;
using System.Text;
using Windows.Networking;
using Windows.Networking.Connectivity;

namespace KillerrinStudiosToolkit.Datastructures
{
    public class HostNameConnectionProfilePair
    {
        public HostName HostName { get; set; }
        public ConnectionProfile ConnectionProfile { get; set; }

        public HostNameConnectionProfilePair(HostName hostName, ConnectionProfile connectionProfile)
        {
            HostName = hostName;
            ConnectionProfile = connectionProfile;
        }
    }
}
