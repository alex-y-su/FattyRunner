using System;

using FattyRunner.Interfaces;

namespace PerfTestsContainer {
    public class WerboseConsoleTets : IDisposable {
        private readonly ExternalContext _ctx;
        private int _n;
        public WerboseConsoleTets(ExternalContext ctx) {
            this._ctx = ctx;
            this._ctx.Logger
                     .Write("WerboseConsoleTets started with {0} iterations",
                            this._ctx.IterationsCount);
        }

        [FatTest]
        public void SomeTest() {
            this._n++;
        }

        public void Dispose() {
            this._ctx.Logger
                     .Write("WerboseConsoleTets completed with {0} real iterations", this._n);
        }
    }
}