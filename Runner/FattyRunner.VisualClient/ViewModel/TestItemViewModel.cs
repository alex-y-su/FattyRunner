using System.ComponentModel;

using FattyRunner.Engine;

namespace FattyRunner.VisualClient.ViewModel {
    public class TestItemViewModel : INotifyPropertyChanged {
        public TestItemViewModel(Test test) {
            this.Test = test;
        }

        public Test Test { get; private set; }
        public string ClassName { get { return this.Test.Reference.Type.Name; } }
        public string MethodName { get { return this.Test.Reference.Run.Name; } }

        public void RunTest() {
            //this._controller.RunTest(this.ActiveItem.Test, this.OnRunComplete);
        }

        public void OnRunComplete(TestResult data) {

        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}