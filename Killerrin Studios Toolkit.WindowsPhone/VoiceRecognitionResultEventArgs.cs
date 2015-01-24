using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.Media.SpeechRecognition;
using KillerrinStudiosToolkit.Enumerators;

namespace KillerrinStudiosToolkit
{
    public delegate void VoiceRecognitionResultEventHandler(object sender, VoiceRecognitionResultEventArgs e);

    public class VoiceRecognitionResultEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {
        public APIResponse Result { get; private set; }
        public SpeechRecognitionResult SpeechResult { get; private set; }

        public VoiceRecognitionResultEventArgs()
            : base(new Exception(), false, null)
        {
            Result = APIResponse.None;
            SpeechResult = null;
        }
        public VoiceRecognitionResultEventArgs(APIResponse _result)
            : base(new Exception(), false, null)
        {
            Result = _result;
            SpeechResult = null;
        }
        public VoiceRecognitionResultEventArgs(APIResponse _result, SpeechRecognitionResult _resultObject)
            : base(new Exception(), false, null)
        {
            Result = _result;
            SpeechResult = _resultObject;
        }
        public VoiceRecognitionResultEventArgs(APIResponse _result, SpeechRecognitionResult _resultObject, Exception e, bool canceled, Object state)
            : base(e, canceled, state)
        {
            Result = _result;
            SpeechResult = _resultObject;
        }
    }
}
