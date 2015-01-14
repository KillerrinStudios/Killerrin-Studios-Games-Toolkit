using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

using Windows.Storage;
using Windows.Media.SpeechRecognition;
using Windows.Media.SpeechSynthesis;
using Windows.Media.Playback;
using Windows.UI.Xaml.Controls;

using KillerrinStudiosToolkit.Enumerators;

namespace KillerrinStudiosToolkit
{
    public partial class CortanaHelper
    {
        public static event VoiceRecognitionResultEventHandler CortanaVoiceRecognitionResult;

        public static CortanaRecognizerState cortanaRecognizerState = CortanaRecognizerState.NotListening;

        public static SpeechRecognizerState speechRecognizerState;
        public static SpeechRecognitionAudioProblem speechRecognitionAudioProblem;

        private static SpeechRecognizer speechRecognizer;
        private static Windows.Foundation.IAsyncOperation<SpeechRecognitionResult> speechResultTask;

        /// <summary>
        /// Installs the Voice Command Definition (VCD) file associated with the application.
        /// Based on OS version, installs a separate document based on version 1.0 of the schema or version 1.1.
        /// </summary>
        public static async void InstallVoiceCommands(string voiceCommandDefinitionPath = "ms-appx:///VoiceCommandDefinition_8.1.xml")
        {
            try {
                Uri vcdUri = new Uri(voiceCommandDefinitionPath);

                StorageFile voiceCommandDef = await StorageFile.GetFileFromApplicationUriAsync(vcdUri);
                await VoiceCommandManager.InstallCommandSetsFromStorageFileAsync(voiceCommandDef);
            }
            catch (Exception vcdEx) {
                Debug.WriteLine(vcdEx.Message);
            }
        }

        public static async void CortanaFeedback(string textToRead, MediaElement player)
        {
            using (var speech = new SpeechSynthesizer()) {
                var voiceStream = await speech.SynthesizeTextToStreamAsync(textToRead);
                player.SetSource(voiceStream, voiceStream.ContentType);
                player.Play();
            }
        }

        public static async void StartListening(string exampleText = "", bool readBackEnabled = false, bool showConfirmation = false)
        {
            if (cortanaRecognizerState == CortanaRecognizerState.Listening) return;

            Debug.WriteLine("Entering: StartListening()");

            speechRecognizer = new SpeechRecognizer();
            speechRecognizer.StateChanged += speechRecognizer_StateChanged;
            speechRecognizer.RecognitionQualityDegrading += speechRecognizer_RecognitionQualityDegrading;

            // Set special commands
            speechRecognizer.UIOptions.ExampleText = exampleText;
            speechRecognizer.UIOptions.IsReadBackEnabled = readBackEnabled;
            speechRecognizer.UIOptions.ShowConfirmation = showConfirmation;

            Debug.WriteLine("Speech Recognizer Set");

            cortanaRecognizerState = CortanaRecognizerState.Listening;
            SpeechRecognitionResult speechRecognitionResult = null;

            Debug.WriteLine("Setting States");
            try {
                await speechRecognizer.CompileConstraintsAsync();
                speechResultTask = speechRecognizer.RecognizeWithUIAsync();// RecognizeAsync();

                Debug.WriteLine("Beginning Recognition");

                //Continuously loop until we are completed
                while (speechResultTask.Status == Windows.Foundation.AsyncStatus.Started) { }
                if (speechResultTask.Status == Windows.Foundation.AsyncStatus.Completed) {
                    speechRecognitionResult = speechResultTask.GetResults();
                }
            }
            catch (Exception) { }

            Debug.WriteLine("Recognition Recieved");

            cortanaRecognizerState = CortanaRecognizerState.NotListening;
            speechRecognizer = null;
            speechResultTask = null;

            if (speechRecognitionResult != null) {
                if (CortanaVoiceRecognitionResult != null) {
                    CortanaVoiceRecognitionResult(null, new VoiceRecognitionResultEventArgs(APIResponse.Successful, speechRecognitionResult));
                }
            }
            else {
                if (CortanaVoiceRecognitionResult != null) {
                    CortanaVoiceRecognitionResult(null, new VoiceRecognitionResultEventArgs(APIResponse.Failed));
                }
            }

            Debug.WriteLine("Exiting StartRecognition()");
            return;
        }

        public static void StopListening()
        {
            if (cortanaRecognizerState == CortanaRecognizerState.NotListening) return;

            speechResultTask.Cancel();
        }


        public static void PlayCortanaListeningEarcon(MediaElement player, string listeningEarconPath = "ms-appx:///Assets/ListeningEarcon.wav")
        {
            player.Source = new Uri(listeningEarconPath);
            player.Play();
        }
        public static void PlayCortanaCancelledEarcon(MediaElement player, string cancelledEarconPath = "ms-appx:///Assets/CancelledEarcon.wav")
        {
            player.Source = new Uri(cancelledEarconPath);
            player.Play();
        }


        private static void speechRecognizer_RecognitionQualityDegrading(SpeechRecognizer sender, SpeechRecognitionQualityDegradingEventArgs args)
        {
            speechRecognitionAudioProblem = args.Problem;
        }
        private static void speechRecognizer_StateChanged(SpeechRecognizer sender, SpeechRecognizerStateChangedEventArgs args)
        {
            speechRecognizerState = args.State;
        }

    }
}
