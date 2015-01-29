using KillerrinStudiosToolkit;
using KillerrinStudiosToolkit.Datastructures;
using KillerrinStudiosToolkit.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking;
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
    public sealed partial class TCPTest : Page
    {
        LANHelper lanHelper;

        public TCPTest()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
#if WINDOWS_PHONE_APP
            // Subscribe to the back button event as to not close the page
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
#endif

            lanHelper = new LANHelper(Consts.APPGuid);
            lanHelper.TCPMessageRecieved += lanHelper_TCPMessageRecieved;

            // Set the current IP Address
            string currentIP = LANHelper.CurrentIPAddressAsString();
            myIPAddresses.Text = "My IP: " + currentIP;

            base.OnNavigatedTo(e);
        }

#if WINDOWS_PHONE_APP
        private void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            resetButton_Tapped(null, null);
            e.Handled = true;
            
            // Describe from event
            Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtons_BackPressed;

            Frame.GoBack();
        }
#endif

        void lanHelper_TCPMessageRecieved(object sender, ReceivedMessageEventArgs e)
        {
            Debug.WriteLine("InAppDataEvent");
            CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                chatTextBlock.Text = e.Message;
            });
        }

        private void initButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            lanHelper.InitTCP();
        }

        private void connectButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(ipAddressPortTextBox.Text)) return;

            lanHelper.ConnectTCP(NetworkConnectionEndpoint.Parse(ipAddressPortTextBox.Text));
        }

        private void startServerButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(ipAddressPortTextBox.Text)) return;
            
            lanHelper.StartTCPServer(NetworkConnectionEndpoint.Parse(ipAddressPortTextBox.Text));
        }

        private void sendButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(chatTextBox.Text)) return;

            lanHelper.SendTCPMessage(chatTextBox.Text);
        }

        private void resetButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            lanHelper.SendTCPCloseMessage();
            lanHelper.ResetTCP();
        }
    }
}
