using System;

using Caliburn.Micro;

using FattyRunner.Engine;
using FattyRunner.VisualClient.Controllers;

namespace FattyRunner.VisualClient.ViewModel {
    public class TestItemViewModel : Screen {
        private readonly TestItemController _controller;
        private readonly Func<TestResult, TestResultsViewModel> _testResultVmFactory;

        public TestItemViewModel(
            Test test, 
            TestItemController controller, 
            Func<TestResult,TestResultsViewModel> testResultVmFactory) {

            this._controller = controller;
            this._testResultVmFactory = testResultVmFactory;
            this.Test = test;
            this.CanRunTest = true;
        }

        public Test Test { get; private set; }

        public TestResultsViewModel TestResults { get; private set; }

        public string ClassName {
            get { return this.Test.Reference.Type.Name; }
        }

        public string MethodName {
            get { return this.Test.Reference.Run.Name; }
        }

        public bool CanRunTest { get; set; }

        public async void RunTest() {
            this.CanRunTest = false;
            this.TestResults = new InProgressTestResultsViewModel();
            var res = await this._controller.RunTest(this.Test);
            OnRunComplete(res);
        }

        private void OnRunComplete(TestResult results) {
            this.CanRunTest = true;
            this.TestResults = this._testResultVmFactory(results);
        }
    }
}