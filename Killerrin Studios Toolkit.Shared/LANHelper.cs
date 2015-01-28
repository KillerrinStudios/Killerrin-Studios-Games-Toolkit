
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
        public event ReceivedMessageEventHandler TCPMessageRecieved;
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

            // Dispose of potentially used assets
            if (m_tcpDataReader != null)
                m_tcpDataReader.Dispose();

            if (m_tcpDataWriter != null)
                m_tcpDataWriter.Dispose();

            if (m_streamSocket != null)
                m_streamSocket.Dispose();

            // Set them to null now
            m_streamSocket = null;
            m_tcpDataReader = null;
            m_tcpDataWriter = null;

            Debug.WriteLine("Resetting TCP");
        }

        public void ResetUDP()
        {
            UDPNetworkConnectionEndpoint = new NetworkConnectionEndpoint();

            IsUDPSetup = false;
            IsUDPConnected = false;

            // Dispose of potentially used assets
            if (m_udpDataWriter != null)
                m_udpDataWriter.Dispose();

            if (m_datagramSocket != null)
                m_datagramSocket.Dispose();

            // Set them to null for reuse
            m_datagramSocket = null;
            m_udpDataWriter = null;

            Debug.WriteLine("Resetting UDP");
        }
        #endregion

        #region TCP
        public void InitTCP()
        {
            IsTCPConnected = false;

            m_streamSocket = new StreamSocket();

            IsTCPSetup = true;
            Debug.WriteLine("UDP Initialized");
        }

        public async void ConnectTCP(NetworkConnectionEndpoint remoteNetworkConnectionEndpoint)
        {
            if (!IsTCPSetup) InitTCP();
            if (IsTCPConnected) { ResetTCP(); InitTCP(); }

            try
            {

                Debug.WriteLine("UDP Connected: " + UDPNetworkConnectionEndpoint.ToString());
                TCPNetworkConnectionEndpoint = remoteNetworkConnectionEndpoint;
                IsTCPConnected = true;

                Debug.WriteLine("UDP Connected: " + UDPNetworkConnectionEndpoint.ToString());
            }
            catch (Exception) { Debug.WriteLine("Error connecting to TDP Endpoint"); }
        }

        public async void SendTCPMessage(string message)
        {
            if (!IsTCPConnected) return;

            m_tcpDataWriter.WriteString(message);
            await m_tcpDataWriter.StoreAsync();

            Debug.WriteLine("TCPMessage Sent: " + message);
        }
        #endregion

        #region UDP
        public void InitUDP()
        {
            IsUDPConnected = false;

            m_datagramSocket = new DatagramSocket();
            m_datagramSocket.MessageReceived += OnUDPMessageReceived;

            IsUDPSetup = true;
            Debug.WriteLine("UDP Initialized");
        }

        public async void ConnectUDP(NetworkConnectionEndpoint remoteNetworkConnectionEndpoint)
        {
            if (!IsUDPSetup) InitUDP();
            if (IsUDPConnected) { ResetUDP(); InitUDP(); }

            try
            {
                Debug.WriteLine("Binding UDPPort");
                await m_datagramSocket.BindServiceNameAsync(remoteNetworkConnectionEndpoint.Port);

                Debug.WriteLine("Connecting UDPSocket");
                await m_datagramSocket.ConnectAsync(remoteNetworkConnectionEndpoint.HostName, remoteNetworkConnectionEndpoint.Port);

                Debug.WriteLine("Creating UDPDataWriter");
                m_udpDataWriter = new DataWriter(m_datagramSocket.OutputStream);

                Debug.WriteLine("Completed UDP");
                UDPNetworkConnectionEndpoint = remoteNetworkConnectionEndpoint;
                IsUDPConnected = true;

                Debug.WriteLine("UDP Connected: " + UDPNetworkConnectionEndpoint.ToString());
            }
            catch (Exception) { Debug.WriteLine("Error connecting to UDP Endpoint"); }
        }

        public async void SendUDPMessage(string message)
        {
            if (!IsUDPConnected) return;

            try
            {
                m_udpDataWriter.WriteString(message);
                await m_udpDataWriter.StoreAsync();

                Debug.WriteLine("UDPMessage Sent: " + message);
            }
            catch (Exception ex) { Debug.WriteLine(DebugTools.PrintOutException("SendUDPMessage", ex)); }
        }

        void OnUDPMessageReceived(DatagramSocket sender, DatagramSocketMessageReceivedEventArgs args)
        {
            try
            {
                var reader = args.GetDataReader();
                var count = reader.UnconsumedBufferLength;
                
                var data = reader.ReadString(count);
                Debug.WriteLine("UDPMessage Recieved: " + data);

                if (UDPMessageRecieved != null)
                {
                    var outputArgs = new ReceivedMessageEventArgs(data, new NetworkConnectionEndpoint(args.RemoteAddress, args.RemotePort));
                    UDPMessageRecieved(this, outputArgs);
                }
            }
            catch (Exception ex) { Debug.WriteLine(DebugTools.PrintOutException("OnUDPMessageRecieved", ex)); }
        }
        #endregion

    }
}
