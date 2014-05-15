using System;
using System.ComponentModel;
using System.Threading.Tasks;

using FattyRunner.Engine;

namespace FattyRunner.VisualClient.Controllers {
    public class TestItemController {
        public TestItemController() {

        }

        public async Task<TestResult> RunTest(Test test) {
            return await Task.Run(() => this.ExecuteTests(test));
        }

        private TestResult ExecuteTests(Test test) {
            int i = 0;
            while (i < int.MaxValue) {
                i++;
            }
            return null;
        }
    }
}