using System;
using System.Threading;
using System.Threading.Tasks;

using FattyRunner.Engine;

namespace FattyRunner.VisualClient.Controllers {
    public class TestItemController {
        public async Task<TestResult> RunTest(Test test) {
            return await Task.Run(() => this.ExecuteTests(test));
        }

        private TestResult ExecuteTests(Test test) {
            Thread.Sleep(TimeSpan.FromSeconds(20));
            return null;
        }
    }
}