using System;
using System.Collections.Generic;
using System.Text;
using Windows.Networking;

namespace KillerrinStudiosToolkit.Datastructures
{
    public struct NetworkConnectionEndpoint
    {
        public HostName HostName;
        public string Port;

        public string HostNameAsString { get { return HostName.RawName; } }

        public NetworkConnectionEndpoint(string hostName = "localhost", string portOrServiceID = "11321")
        {
            HostName = new HostName(hostName);

            if (string.IsNullOrEmpty(portOrServiceID)) Port = "11321";
            else Port = portOrServiceID;
        }

        public NetworkConnectionEndpoint(HostName hostName, string portOrServiceID)
        {
            HostName = hostName;

            if (string.IsNullOrEmpty(portOrServiceID)) Port = "11321";
            else Port = portOrServiceID;
        }

        /// <summary>
        /// Parses a string into a NetworkConnectionEndpoint
        /// </summary>
        /// <param name="stringToParse">A Colon Delimited IPAddress/Port Pair</param>
        /// <returns>The parsed NetworkConnectionEndpoint</returns>
        public static NetworkConnectionEndpoint Parse(string stringToParse)
        {
            string[] textSplit = stringToParse.Split(new char[] { ':' });
            string ipAddress = textSplit[0];
            string port = (textSplit.Length >= 2 ? textSplit[1] : "");

            return new NetworkConnectionEndpoint(ipAddress, port);

        }

        /// <summary>
        /// Parses a string into a NetworkConnectionEndpoint
        /// </summary>
        /// <param name="stringToParse">A Colon Delimited IPAddress/Port Pair</param>
        /// <returns>The parsed NetworkConnectionEndpoint</returns>
        public static bool TryParse(string stringToParse, NetworkConnectionEndpoint? networkConnectionEndpoint)
        {
            try
            {
                networkConnectionEndpoint = Parse(stringToParse);
                return true;
            }
            catch (Exception) { return false; }
        }

        public override string ToString()
        {
            return HostName.RawName + ":" + Port;
        }
    }
}
