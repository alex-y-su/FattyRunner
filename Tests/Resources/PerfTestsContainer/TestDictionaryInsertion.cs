using FattyRunner.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerfTestsContainer
{
    public class TestDictionaryInsertion
    {
        IDictionary<string, object> _dictionary;

        [FatInit]
        public void Init()
        {
            _dictionary = new Dictionary<string, object>();
        }

        [FatCleanup]
        public void Cleanup()
        {
            _dictionary = null;
        }

        [FatTest]
        public void Insert()
        {
            _dictionary.Add("", "");
        }
    }
}
