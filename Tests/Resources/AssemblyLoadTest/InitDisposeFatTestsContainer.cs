using System;
using System.Threading;

using FattyRunner.Interfaces;

namespace AssemblyLoadTests {
    public class InitDisposeFatTestsContainer: IDisposable {
        public InitDisposeFatTestsContainer(IFatLogger fatLogger) {
            
        }

        [FatTest]
        public void Test1() {
            Thread.Sleep(1);
        }

        public void Dispose() {
        }
    }
}