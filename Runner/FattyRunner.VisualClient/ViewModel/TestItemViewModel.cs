using System.ComponentModel;

using Caliburn.Micro;

using FattyRunner.Engine;

namespace FattyRunner.VisualClient.ViewModel {

    public class InProgressTestResultsViewModel : TestResultsViewModel {
        
    }

    public class TestResultsViewModel : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class TestItemViewModel : Screen {
        public TestItemViewModel(Test test) {
            this.Test = test;
            this.TestResults = 
        }

        public Test Test { get; private set; }

        public TestResultsViewModel TestResults { get; private set; }

        public string ClassName {
            get { return this.Test.Reference.Type.Name; }
        }

        public string MethodName {
            get { return this.Test.Reference.Run.Name; }
        }

        public void RunTest() {
            //this._controller.RunTest(this.ActiveItem.Test, this.OnRunComplete);
        }


    }
}