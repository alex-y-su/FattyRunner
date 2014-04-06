using System.Threading;

using FattyRunner.Interfaces;

namespace AssemblyLoadTests {
    public class PrimitiveFatTestsContainer {
        [FatTest]
        public void Test1() {
            Thread.Sleep(1);
        }
    }
}