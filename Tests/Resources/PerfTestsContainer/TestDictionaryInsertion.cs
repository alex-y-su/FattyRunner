using System;
using System.Collections.Generic;
using System.Diagnostics;

using FattyRunner.Interfaces;

namespace PerfTestsContainer {
    public class FilterByNameRabbit {
        [FatTest]
        public void Some() {
            
        }
    }

    public class WerboseConsoleTets {
        private int _n;

        [FatTest]
        public void SomeTest() {
            Console.WriteLine(++this._n);
        }
    }

    public class TestDictionaryInsertion {
        private IDictionary<string, object> _dictionary;

        [FatInit]
        public void Init() {
            this._dictionary = new Dictionary<string, object>();
        }

        [FatCleanup]
        public void Cleanup() {
            this._dictionary = null;
        }

        [FatTest]
        public void Insert() {
            this._dictionary.Add(Guid.NewGuid().ToString(), "123");
        }
    }
}