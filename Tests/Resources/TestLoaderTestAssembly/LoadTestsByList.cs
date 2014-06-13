using FattyRunner.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestLoaderTestAssembly
{
    public class LoadTestsByList
    {
        [FatTestAttribute]
        public void ShouldNotBeLoaded() {

        }

        [FatTestAttribute]
        public void ShouldNotBeLoaded2() {

        }

        [FatTestAttribute]
        public void ShouldBeLoaded() {

        }
    }
}
