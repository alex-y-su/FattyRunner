using System;
using System.Linq;

using Caliburn.Micro;

using FattyRunner.Engine;
using FattyRunner.VisualClient.Controllers;

using Microsoft.Win32;
using System.Reflection;
using System.Collections.Generic;

namespace FattyRunner.VisualClient.ViewModel {
    public class ShellViewModel : Conductor<TestItemViewModel>.Collection.OneActive {
        private readonly ShellController _controller;
        private readonly Func<Assembly, TestDefenition, TestItemViewModel> _itemsFactory;

        public ShellViewModel(ShellController controller, Func<Assembly, TestDefenition, TestItemViewModel> itemsFactory) {
            this.DisplayName = "Fatty runner";

            this._controller = controller;
            this._itemsFactory = itemsFactory;
        }

        public void AddTests() {
            var dlg = new OpenFileDialog();
            if (dlg.ShowDialog().GetValueOrDefault(false)) {
                var loaded = this._controller.LoadTests(dlg.FileName);

                var tests = new List<TestItemViewModel>();
                foreach (var x in loaded) {
                    foreach (var t in x.Item2) {
                        tests.Add(this._itemsFactory(x.Item1, t));
                    }
                }
                
                this.Items.AddRange(tests);

                if (null == this.ActiveItem) this.ActiveItem = tests.First();
            }
        }

        public void RunSelected() {
            if (null == this.ActiveItem) return;
            this.ActiveItem.RunTest();
        }

        public void RunAll() {
            foreach (var x in this.Items) {
                x.RunTest();
            }
        }
    }
}