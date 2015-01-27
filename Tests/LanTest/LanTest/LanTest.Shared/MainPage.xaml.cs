using KillerrinStudiosToolkit;
using KillerrinStudiosToolkit.Datastructures;
using KillerrinStudiosToolkit.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace LanTest
{

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        LANHelper lanHelper;

        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            lanHelper = new LANHelper();
            lanHelper.UDPMessageRecieved += lanHelper_UDPMessageRecieved;

            base.OnNavigatedTo(e);
        }

        void lanHelper_UDPMessageRecieved(object sender, ReceivedMessageEventArgs e)
        {
            chatTextBlock.Text = e.Message;
            Debug.WriteLine("Message Recieved: " + e.Message);
        }

        private void initButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            lanHelper.InitUDP();
            Debug.WriteLine("UDP Initialized");
        }

        private void connectButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(ipAddressPortTextBox.Text)) return;
            string ipAddress = null;
            string port = null;

            string[] textSplit = ipAddressPortTextBox.Text.Split(new char[] {':'});
            ipAddress = textSplit[0];
            port = (textSplit.Length >= 2 ? textSplit[1] : null);

            NetworkConnectionEndpoint endpoint = new NetworkConnectionEndpoint(ipAddress, (!string.IsNullOrEmpty(port) ? port : "11321"));

            lanHelper.ConnectUDP(endpoint);
            Debug.WriteLine("UDP Connected: " + lanHelper.UDPNetworkConnectionEndpoint.ToString());
        }

        private void sendButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(chatTextBox.Text)) return;

            lanHelper.SendUDPMessage(chatTextBox.Text);
            Debug.WriteLine("Sending Message: " + chatTextBox.Text);
        }

        private void resetButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            lanHelper.ResetUDP();
            Debug.WriteLine("Resetting UDP");
        }
    }
}
