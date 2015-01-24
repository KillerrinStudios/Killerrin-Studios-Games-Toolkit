using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Net.NetworkInformation;

namespace KillerrinStudiosToolkit
{
    public partial class Consts
    {
        public static bool isApplicationClosing = false;
        public static Random random = new Random();

        public static bool IsConnectedToInternet() { return NetworkInterface.GetIsNetworkAvailable(); }
    }
}
