using System;
using System.Collections.Generic;
using System.Text;

using KillerrinStudiosToolkit.Enumerators;
using System.Threading.Tasks;

namespace KillerrinStudiosToolkit
{
    public partial class KillerrinApplicationData
    {
        public string PackageID;
        public string PublisherID;

        public string Name;
        public string Version;
        public string Description;

        public string Developer = "Killerrin Studios";
        public string Website = "http://www.killerrin.com";
        public string Twitter = "https://www.twitter.com/killerrin";
        public string Facebook = "https://www.facebook.com/KillerrinStudios";
        
        public string FeedbackUrl = "support@killerrin.com";
        public string SupportUrl = "support@killerrin.com";
        public string FeedbackSubject = "feedback - ";
        public string SupportSubject = "support - ";
        
        public string OtherWebsite;

#if WINDOWS_PHONE_APP
        public ClientOSType OS = ClientOSType.WindowsPhone81;
#elif WINDOWS_APP
        public ClientOSType OS = ClientOSType.Windows81;
#endif

        public KillerrinApplicationData(string packageID,
                                        string publisherID,
                                        string name,
                                        string version,
                                        string description,
                                        string otherWebsite = "")
        {
            PackageID = packageID;
            PublisherID = publisherID;

            Name = name;
            Version = version;
            Description = description;
            OtherWebsite = otherWebsite;

            FeedbackSubject += name + ": ";
            SupportSubject += name + ": ";
        }

        public async Task<bool> LaunchReview()
        {
#if WINDOWS_PHONE_APP
            bool result = await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-windows-store:reviewapp?appid=" + PackageID));
#else
            bool result = await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-windows-store:review?PFN=" + PackageID));
#endif
            return result;
        }
    }
}
