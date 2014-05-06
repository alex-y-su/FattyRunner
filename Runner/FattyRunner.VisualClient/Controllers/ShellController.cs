using System;
using System.Collections.Generic;

using FattyRunner.Engine;

namespace FattyRunner.VisualClient.Controllers {
    public class ShellController {
        private readonly Func<EnvironmentConfiguration> _envConfigProvider;

        public ShellController(Func<EnvironmentConfiguration> envConfigProvider) {
            this._envConfigProvider = envConfigProvider;
        }

        public IEnumerable<Test> LoadTests(string fileOrDir) {
            var envConfig = this._envConfigProvider();
            var asm = ReflectionUtils.LoadAssemblyFromFile(fileOrDir);
            
            if (null == asm) {
                throw new NotImplementedException("No UI yet");
            }

            return TestLoader.loadTests(envConfig, asm);
        }
    }
}