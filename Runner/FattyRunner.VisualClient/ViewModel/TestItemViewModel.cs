using System;

using Caliburn.Micro;

using FattyRunner.Engine;
using FattyRunner.VisualClient.Controllers;

namespace FattyRunner.VisualClient.ViewModel {
    public class TestItemViewModel : Screen {
        private readonly System.Reflection.Assembly _asm;
        private readonly TestItemController _controller;
        private readonly Func<TestResult, TestResultsViewModel> _testResultVmFactory;

        public TestItemViewModel(
            System.Reflection.Assembly asm,
            TestDefenition test,
            TestItemController controller,
            Func<TestResult, TestResultsViewModel> testResultVmFactory) {

            this._controller = controller;
            this._testResultVmFactory = testResultVmFactory;
            this.Test = test;
            this.TestInProgress = false;
            this._asm = asm;
        }

        public TestDefenition Test { get; private set; }

        public Screen TestResults { get; private set; }

        public string ClassName {
            get { return this.Test.TypeName; }
        }

        public string MethodName {
            get { return this.Test.TestName; }
        }

        public bool TestInProgress { get; set; }

        public async void RunTest() {
            if (this.TestInProgress) return;
            
            this.TestInProgress = true;
            try {
                this.TestResults = new InProgressTestResultsViewModel();
                var res = await this._controller.RunTest(this.Test, this._asm);
                this.TestResults = this._testResultVmFactory(res);
            }
            finally {
                this.TestInProgress = false;
            }
        }
    }
}