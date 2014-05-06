using System.Windows;

using FattyRunner.Interfaces;

namespace FattyRunner.VisualClient.Components {
    public class FattyLoggerAdapter : IFatLogger {
        public void Write(string format, params object[] prms) {
            MessageBox.Show(string.Format(format, prms));
        }
    }
}