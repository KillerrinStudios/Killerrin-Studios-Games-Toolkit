using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace KillerrinStudiosToolkit
{
    public partial class KTKDebugTools
    {
        // Testing Code
        public static bool DebugMode = Debugger.IsAttached;

        public static string PrintOutException(string headerMessage, Exception ex)
        {
            string str = headerMessage;
            try {
                Debug.WriteLine(headerMessage);
                str += "\nSource: " + ex.Source +
                        "\n Help Link: " + ex.HelpLink +
                        "\n HResult: " + ex.HResult +
                        "\n Message: " + ex.Message +
                        "\n StackTrace: " + ex.StackTrace;
                Debug.WriteLine(str);

                foreach (var key in ex.Data.Keys) {
                    Debug.WriteLine(key.ToString() + " | " + ex.Data[key].ToString());
                }

                str += "\n";

                if (ex.InnerException != null)
                    str += PrintOutException("Entering Inner Exception", ex.InnerException);
            }
            catch (Exception) { str = headerMessage; }

            return str;
        }

    }
}
