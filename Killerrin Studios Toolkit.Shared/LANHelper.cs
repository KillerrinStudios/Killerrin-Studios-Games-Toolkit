
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using System.Diagnostics;

using KillerrinStudiosToolkit.Datastructures;
using KillerrinStudiosToolkit.Enumerators;
using KillerrinStudiosToolkit.Events;

namespace KillerrinStudiosToolkit
{
    public class LANHelper
    {
        public static bool IsConnectedToInternet() { return System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable(); }

        #region Fields/Properties
        HostType m_hostType;

        /// <summary>
        /// Sets the mode the LANHelper API will operate under
        /// </summary>
        public HostType HostType {
            get { return m_hostType; }
            set { m_hostType = value; }
        }

        #region TCP
        public NetworkConnectionEndpoint TCPNetworkConnectionEndpoint { get; protected set; }
        
        public bool IsTCPSetup { get; protected set; }
        public bool IsTCPConnected { get; protected set; }
        
        private StreamSocket m_streamSocket;
        private DataReader m_tcpDataReader;
        private DataWriter m_tcpDataWriter;
        #endregion

        #region UDP
        public event ReceivedMessageEventHandler UDPMessageRecieved;

        public NetworkConnectionEndpoint UDPNetworkConnectionEndpoint { get; protected set; }
        
        public bool IsUDPSetup { get; protected set; }
        public bool IsUDPConnected { get; protected set; }

        private DatagramSocket m_datagramSocket;
        private DataWriter m_udpDataWriter;
        #endregion
        #endregion

        public LANHelper()
        {
            Reset();
        }

        #region Resets
        public void Reset()
        {
            HostType = HostType.Client;
            ResetTCP();
            ResetUDP();
        }

        public void ResetTCP()
        {
            TCPNetworkConnectionEndpoint = new NetworkConnectionEndpoint();

            IsTCPSetup = false;
            IsTCPConnected = false;

            m_streamSocket = null;
            m_tcpDataReader = null;
            m_tcpDataWriter = null;
        }

        public void ResetUDP()
        {
            UDPNetworkConnectionEndpoint = new NetworkConnectionEndpoint();

            IsUDPSetup = false;
            IsUDPConnected = false;

            m_datagramSocket = null;
            m_udpDataWriter = null;
        }
        #endregion

        #region TCP
        public void InitTCP()
        {
            IsTCPConnected = false;

            m_streamSocket = new StreamSocket();

            IsTCPSetup = true;
        }

        public async void ConnectTCP(NetworkConnectionEndpoint remoteNetworkConnectionEndpoint)
        {
            if (!IsTCPSetup) InitTCP();

            try
            {

                TCPNetworkConnectionEndpoint = remoteNetworkConnectionEndpoint;
                IsTCPConnected = true;
            }
            catch (Exception) { Debug.WriteLine("Error connecting to TDP Endpoint"); }
        }

        public async void SendTCPMessage(string message)
        {
            if (!IsTCPConnected) return;

            m_tcpDataWriter.WriteString(message);
            await m_tcpDataWriter.StoreAsync();
        }
        #endregion

        #region UDP
        public void InitUDP()
        {
            IsUDPConnected = false;

            m_datagramSocket = new DatagramSocket();
            m_datagramSocket.MessageReceived += OnUDPMessageReceived;

            IsUDPSetup = true;
        }

        public async void ConnectUDP(NetworkConnectionEndpoint remoteNetworkConnectionEndpoint)
        {
            if (!IsUDPSetup) InitUDP();

            try
            {
                await m_datagramSocket.ConnectAsync(remoteNetworkConnectionEndpoint.HostName, remoteNetworkConnectionEndpoint.Port);
                m_udpDataWriter = new DataWriter(m_datagramSocket.OutputStream);

                UDPNetworkConnectionEndpoint = remoteNetworkConnectionEndpoint;
                IsUDPConnected = true;
            }
            catch (Exception) { Debug.WriteLine("Error connecting to UDP Endpoint"); }
        }

        public async void SendUDPMessage(string message)
        {
            if (!IsUDPConnected) return;

            m_udpDataWriter.WriteString(message);
            await m_udpDataWriter.StoreAsync();
        }

        void OnUDPMessageReceived(DatagramSocket sender, DatagramSocketMessageReceivedEventArgs args)
        {
            var reader = args.GetDataReader();
            var count = reader.UnconsumedBufferLength;
            var data = reader.ReadString(count);

            if (UDPMessageRecieved != null)
            {
                var outputArgs = new ReceivedMessageEventArgs(data, new NetworkConnectionEndpoint(args.RemoteAddress, args.RemotePort));
                UDPMessageRecieved(this, outputArgs);
            }
        }
        #endregion

    }
}
