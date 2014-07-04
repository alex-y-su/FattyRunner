using System;
using System.Collections.Generic;

using FattyRunner.Engine;
using System.Reflection;
using Microsoft.FSharp.Collections;

namespace FattyRunner.VisualClient.Controllers {
    public static class FSharpListExtensions {
        public static IEnumerable<T> ToEnumerable<T>(this FSharpList<T> list) {
            for (var x = 0; x < list.Length; x++) yield return list[x];
        }
    }

    public class ShellController {
        private readonly Func<EnvironmentConfiguration> _envConfigProvider;

        public ShellController() {
        }

        public IEnumerable<Tuple<Assembly, IEnumerable<TestDefenition>>> LoadTests(string fileOrDir) {
            var asm = ReflectionUtils.LoadAssemblyFromFile(fileOrDir);

            if (null == asm) {
                throw new NotImplementedException("No UI yet");
            }

            var tests = TestLoader.findTestDefenitions(asm).ToEnumerable();
            return new[] { Tuple.Create(asm, tests) };
        }
    }
}