using System;
using System.Threading;

using FattyRunner.Interfaces;

namespace AssemblyLoadTests {
    public class InitDisposeFatTestsContainer: IDisposable {
        public InitDisposeFatTestsContainer(IFatLogger fatLogger) {
            
        }

        [FatTest]
        public void Test1() {
        }

        public void Dispose() {
        }
    }
}