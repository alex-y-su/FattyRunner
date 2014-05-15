using System;
using System.Threading.Tasks;

using FattyRunner.Engine;

namespace FattyRunner.VisualClient.Controllers {
    public class TestItemController {
        private readonly Func<EnvironmentConfiguration> _configurationProvdier;

        public TestItemController(Func<EnvironmentConfiguration> configurationProvdier) {
            this._configurationProvdier = configurationProvdier;
        }

        public async Task<TestResult> RunTest(Test test) {
            return await Task.Run(() => this.ExecuteTests(test));
        }

        private TestResult ExecuteTests(Test test) {
            var cfg = this._configurationProvdier();
            return TestRunnerEngine.runTest(cfg, test);
        }
    }
}