FattyRunner
===========

Load test runner. Supposed to be used for fast and general check of algorithms and implementations. Can run a method multiple times every time increase count of execution by defied step until max iteration count will not be reached. Preserve timings for each iteration and generate JSON file with results.

FatTest attribute source code with defaults:

	[AttributeUsage(AttributeTargets.Method)]
    public class FatTestAttribute : Attribute {
        public FatTestAttribute(uint maxIterations = 1000u, 
                                uint warmUpIterations = 50, 
                                uint step = 100){
            
            MaxIterations = maxIterations;
            WarmUpIterations = warmUpIterations;
            Step = step;
        }
        public uint MaxIterations { get; set; }
        public uint WarmUpIterations { get; set; }
        public uint Step { get; set; }
    }

Example of class decorated by FatRunner attributes:

	public class InitDisposeFatTestsContainer {
        //optional
		[FatInit]
        public void Init() {}
		
		//optional
        [FatCleanup]
        public void Cleanup() {}

        [FatTest]
        public void Test1() {
            Thread.Sleep(1);
        }
    }

Console client parameters:

	n:[number]  - Count of iterations
	path:[path] - File or directory where test assemblies are located.\n 
				  Can be used multiple times.
	test:[name] - Full class name + method name like ""MyNamespace.MyClass.MethodToTest"".\n
	              Can be used multiple times.



FattyRunner written on F# except C# assemblies with attributes.

Near plans:

* Results report comparer
* WPF UI 
* Memory counters

Fell free to send me a feature requests and bug reports.


Copyright 2014 Alexey Suvorov - Provided under the [MIT license](https://github.com/alexeysuvorov/FattyRunner/blob/master/LICENSE).
