using System;
using System.Threading.Tasks;

using FattyRunner.Engine;
using System.Reflection;

namespace FattyRunner.VisualClient.Controllers {
    public class TestItemController {
        private readonly Func<EnvironmentConfiguration> _configurationProvdier;

        public TestItemController(Func<EnvironmentConfiguration> configurationProvdier) {
            this._configurationProvdier = configurationProvdier;
        }

        public async Task<TestResult> RunTest(TestDefenition test, Assembly asm) {
            return await Task.Run(() => this.ExecuteTests(test, asm));
        }

        private TestResult ExecuteTests(TestDefenition testDef, Assembly asm) {
            var cfg = this._configurationProvdier();
            var testRef = TestLoader.loadMultistepTestRef(asm, testDef);
            var test = TestLoader.loadMultistepTest(cfg, testRef);
            return TestRunnerEngine.runTest(cfg, test);
        }
    }
}