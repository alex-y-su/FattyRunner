using System;

using FattyRunner.Interfaces;

namespace PerfTestsContainer {
    public class WerboseConsoleTets {
        private int _n;

        [FatTest]
        public void SomeTest() {
            Console.WriteLine(++this._n);
        }
    }
}