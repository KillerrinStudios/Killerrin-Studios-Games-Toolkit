using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Windows.UI.Xaml.Controls;

namespace KillerrinStudiosToolkit
{
    public static class XamlControlHelper
    {
        private static bool displayDebugText = false;

        public static void ChangeProgressIndicator(object progressBar, bool isEnabled)
        {
            if (progressBar == null) return;

            if (progressBar is ProgressRing) {
                (progressBar as ProgressRing).IsActive = isEnabled;
                return;
            }

            else if (progressBar is ProgressBar) {
                (progressBar as ProgressBar).IsEnabled = isEnabled;
                return;
            }
        }

        public static void SetDebugString(object textBlock, string str, bool forceDisplay = false)
        {
            if (string.IsNullOrEmpty(str)) return;
            Debug.WriteLine(str);

            if (textBlock == null) return;

            if (displayDebugText || forceDisplay) {
                (textBlock as TextBlock).Text = str;
            }
        }

        public static void LoseFocusOnTextBox(object sender)
        {
            if (sender == null) return;

            var control = (sender as Control);
            var isTabStop = control.IsTabStop;
            control.IsTabStop = false;
            control.IsEnabled = false;
            control.IsEnabled = true;
            control.IsTabStop = isTabStop;
        }
    }
}
