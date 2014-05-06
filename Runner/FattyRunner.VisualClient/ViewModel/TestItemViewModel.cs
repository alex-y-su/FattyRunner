using System.ComponentModel;

using FattyRunner.Engine;

namespace FattyRunner.VisualClient.ViewModel {
    public class TestItemViewModel : INotifyPropertyChanged {
        private readonly Test _test;

        public TestItemViewModel(Test test) {
            this._test = test;
        }

        public string ClassName { get { return this._test.Reference.Type.Name; } }
        public string MethodName { get { return this._test.Reference.Run.Name; } }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}