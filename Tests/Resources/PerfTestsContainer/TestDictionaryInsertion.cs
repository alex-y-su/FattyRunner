using System;
using System.Collections.Generic;

using FattyRunner.Interfaces;

namespace PerfTestsContainer {
    public class TestDictionaryInsertion : IDisposable {
        private readonly IDictionary<string, object> _dictionary;

        public TestDictionaryInsertion() {
            this._dictionary = new Dictionary<string, object>();
        }

        [FatTest]
        public void Insert() {
            this._dictionary.Add(Guid.NewGuid().ToString(), "123");
        }

        public void Dispose() {

        }
    }
}