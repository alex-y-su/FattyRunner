using System.Linq;

using Caliburn.Micro;

using FattyRunner.VisualClient.Controllers;

using Microsoft.Win32;

namespace FattyRunner.VisualClient.ViewModel {
    public class ShellViewModel : Conductor<TestItemViewModel>.Collection.OneActive {
        private readonly ShellController _controller;

        public ShellViewModel(ShellController controller) {
            this._controller = controller;
        }

        public void SelectAssemblyOrFolder() {
            var dlg = new OpenFileDialog();
            if (dlg.ShowDialog().GetValueOrDefault(false)) {
                var tests = this._controller.LoadTests(dlg.FileName)
                                            .Select(x => new TestItemViewModel(x));

                this.Items.AddRange(tests);
            }
        }
    }
}