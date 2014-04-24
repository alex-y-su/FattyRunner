namespace FattyRunner.VisualClient.ViewModel {
    using System.Windows.Controls;

    using Caliburn.Micro;

    public class ShellViewModel : Conductor<IScreen> {
        public ContentControl TestsList { get; set; }
    }
}