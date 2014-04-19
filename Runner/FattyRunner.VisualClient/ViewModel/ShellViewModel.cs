using System.ComponentModel;

using Caliburn.Micro;

namespace FattyRunner.VisualClient.ViewModel {
    public class ShellViewModel : Caliburn.Micro.Screen {
        public IScreen Tests { get; set; }
        public IScreen Details { get; set; }
    }
}