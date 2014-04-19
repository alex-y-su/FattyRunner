using System.ComponentModel;
using System.Windows.Controls;

using Caliburn.Micro;

namespace FattyRunner.VisualClient.ViewModel {
    public class ShellViewModel : Screen {
        public UserControl TestsList { get; set; }
        public UserControl Details { get; set; }
    }
}