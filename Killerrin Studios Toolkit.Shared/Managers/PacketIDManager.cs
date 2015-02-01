using System;
using System.Collections.Generic;
using System.Text;

namespace KillerrinStudiosToolkit.Managers
{
    public static class PacketIDManager
    {
        public static uint CurrentID { get; set; }

        public static uint GetNewID()
        {
            uint IDToUse = CurrentID;
            IncrimentID();

            return IDToUse;
        }

        private static void IncrimentID()
        {
            if (CurrentID < uint.MaxValue)
                CurrentID++;
            else
                CurrentID = uint.MinValue;
        }
    }
}
