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
            this.TestInProgress = false;
        }

        public Test Test { get; private set; }

        public TestResultsViewModel TestResults { get; private set; }

        public string ClassName {
            get { return this.Test.Reference.Type.Name; }
        }

        public string MethodName {
            get { return this.Test.Reference.Run.Name; }
        }

        public bool TestInProgress { get; set; }

        public async void RunTest() {
            if(this.TestInProgress) return;

            this.TestInProgress = true;
            try {
                this.TestResults = new InProgressTestResultsViewModel();
                var res = await this._controller.RunTest(this.Test);
                OnRunComplete(res);
            }
            finally {
                this.TestInProgress = false;
            }
        }

        private void OnRunComplete(TestResult results) {
            this.TestResults = this._testResultVmFactory(results);
        }
    }
}