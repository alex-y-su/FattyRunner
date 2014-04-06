using System.Threading;

using FattyRunner.Interfaces;

namespace AssemblyLoadTests {
    public class InitDisposeFatTestsContainer {
        [FatInit]
        public void Init() {}

        [FatCleanup]
        public void Cleanup() {}

        [FatTest]
        public void Test1() {
            Thread.Sleep(1);
        }
    }
}