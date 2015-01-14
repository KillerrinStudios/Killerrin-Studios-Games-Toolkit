using System;
using System.Collections.Generic;
using System.Text;

namespace KillerrinStudiosToolkit.Events
{
    public delegate void ReceivedMessageEventHandler(object sender, ReceivedMessageEventArgs e);

    public class ReceivedMessageEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {
        public string Message { get; private set; }

        public ReceivedMessageEventArgs()
            : base(new Exception(), false, null)
        {
            Message = "";
        }

        public ReceivedMessageEventArgs(string message)
            : base(new Exception(), false, null)
        {
            Message = message;
        }

        public ReceivedMessageEventArgs(string message, Exception e, bool canceled, Object state)
            : base(e, canceled, state)
        {
            Message = message;
        }
    }
}
