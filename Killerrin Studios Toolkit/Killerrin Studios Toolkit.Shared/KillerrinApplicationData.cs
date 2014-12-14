using System;
using System.Collections.Generic;
using System.Text;

using KillerrinStudiosToolkit.Enumerators;

namespace KillerrinStudiosToolkit
{
    public class KillerrinApplicationData
    {
        public string Name;
        public string Version;
        public string Description;

        public const string Developer = "Killerrin Studios";
        public const string Website = "http://www.killerrin.com";
        public const string Twitter = "https://www.twitter.com/killerrin";
        public const string Facebook = "https://www.facebook.com/KillerrinStudios";
        
        public const string FeedbackUrl = "support@killerrin.com";
        public const string SupportUrl = "support@killerrin.com";
        public string FeedbackSubject = "feedback - ";
        public string SupportSubject = "support - ";
        
        public string OtherWebsite;

#if WINDOWS_PHONE_APP
        public ClientOSType OS = ClientOSType.WindowsPhone81;
#elif WINDOWS_APP
        public ClientOSType OS = ClientOSType.Windows81;
#endif

        public KillerrinApplicationData(string name,
                                        string version,
                                        string description,
                                        string otherWebsite = "")
        {

            Name = name;
            Version = version;
            Description = description;
            OtherWebsite = otherWebsite;

            FeedbackSubject += name + ": ";
            SupportSubject += name + ": ";
        }
    }
}
