using System.Threading;

using FattyRunner.Interfaces;

namespace AssemblyLoadTests {
    public class PrimitiveFatTestsContainer {
        [FatTest(3000u,150u,300u)]
        public void Test1() {
        }
    }
}