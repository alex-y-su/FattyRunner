using System;
using System.Linq;

using Caliburn.Micro;

using FattyRunner.Engine;
using FattyRunner.VisualClient.Controllers;

using Microsoft.Win32;

namespace FattyRunner.VisualClient.ViewModel {
    public class ShellViewModel : Conductor<TestItemViewModel>.Collection.OneActive {
        private readonly ShellController _controller;
        private readonly Func<Test, TestItemViewModel> _itemsFactory;

        public ShellViewModel(ShellController controller, Func<Test, TestItemViewModel> itemsFactory) {
            this._controller = controller;
            this._itemsFactory = itemsFactory;
        }

        public void AddTests() {
            var dlg = new OpenFileDialog();
            if (dlg.ShowDialog().GetValueOrDefault(false)) {
                var tests = this._controller.LoadTests(dlg.FileName)
                                            .Select(this._itemsFactory)
                                            .ToArray();
                
                this.Items.AddRange(tests);

                if (null == this.ActiveItem) this.ActiveItem = tests.First();
            }
        }

        public void RunSelected() {
            if(null == this.ActiveItem) return;
            this.ActiveItem.RunTest();
        }

        public void RunAll() {
            foreach (var x in this.Items) {
                x.RunTest();
            }
        }
    }
}