FattyRunner
===========

![FattyRunner](https://raw.githubusercontent.com/alexeysuvorov/FattyRunner/master/Wiki/Ui/Images/Screen1.png)

Load test runner. Supposed to be used for fast and general check of algorithms and implementations. Can run a method multiple times every time increase count of execution by defied step until max iteration count will not be reached. Preserve timings for each iteration and generate JSON file with results.

FatTest attribute source code with defaults:

	[AttributeUsage(AttributeTargets.Method)]
    public class FatTestAttribute : Attribute {
        public FatTestAttribute(uint maxIterations = 1000u, 
                                uint warmUpIterations = 50, 
                                uint step = 100,
								object data = null){
            
            MaxIterations = maxIterations;
            WarmUpIterations = warmUpIterations;
            Step = step;
			Data = data;
        }
        public uint MaxIterations { get; set; }
        public uint WarmUpIterations { get; set; }
        public uint Step { get; set; }
		public object Data { get; set; }
    }

Example of class decorated by FatRunner attributes:

	public class InitDisposeFatTestsContainer: IDisposable {
        //optional
        public void InitDisposeFatTestsContainer(ExternalContext ctx /*optional*/) {}
		
		//optional
        public void Dispose() {}

        [FatTest]
        public void Test1() {
            Thread.Sleep(1);
        }
    }

**ExternalContext**

ExternalContext contains data about count of iterations for current instance, name of test method, optional user constatnt and instance of IFatLogger implemented by runner. Test progress should be reported through this logger. 

Instance of class which contains FatTest will be created for each step for each method separately. 
Example: 

	public class A {
		public A(ExternalContext ctx){
			Console.WriteLine("Test: {0} N:{1}",
							  ctx.RunMethodName,
							  ctx.IterationsCount)
		}
		FatTest(maxIterations = 10u, warmUpIterations=0u,step=5u)
		public void Method1(){}

		FatTest(maxIterations = 100u, warmUpIterations=0u,step=25u)
		public void Method2(){}
	}

Here instance of class A will be created 6 times and console output will be:

	Test: Method1 N:5
	Test: Method1 N:10
	Test: Method2 N:25
	Test: Method2 N:50
	Test: Method2 N:75
	Test: Method2 N:100


**After warm up attribute**

You can call some code just after warm up but before actual test call

	public class InitDisposeFatTestsContainer {
        [FatAfterWarmup]
        public void Reset() {
			//reset some counters e.t.c.
		}

        [FatTest]
        public void Test1() {}
    }


Installation 
============

**NuGet**

	PM> Install-Package FattyRunner

You can find **FattyRunner.Client.exe** in **packages**\FattyRunner[version]\client. 
Right now it provides only command line interface.  

**Console client parameters:**

	n:[number]  - Count of iterations
	path:[path] - Path to assembly file. Can be used multiple times.
	test:[name] - Full class name + method name like "MyNamespace.MyClass.MethodToTest".
	              Can be used multiple times. If not defined then all tests will be runned.
	out:[path]  - Specify file when execution results will be stored.
				  If file already exists it will be overridden.


FattyRunner is written on F# except C# assemblies with attributes.

**Planned:**

* Add AfterWramUp method for users who want to reset some state after warm up completes
* Results report comparer
* WPF UI 
* Memory counters

Fell free to send me a feature requests and bug reports.


Copyright 2014 Alexey Suvorov - Provided under the [MIT license](https://github.com/alexeysuvorov/FattyRunner/blob/master/LICENSE).

Release Notes
=============

0.0.3.0

* Added visual client with basic UI.

0.0.2.2

* Data property added to ExternalContext, so user can pass some data throught attribute
	
Example:

	public class A {
		public A(ExternalContext ctx){
			//Data will be 10 for Method1 runs
			//and 20 for Method2 runs
			ctx.Logger.Write("Entropy: {0}", ctx.Data);
		}
		
		FatTest(Data = 10)
		public void Method1(){}

		FatTest(maxIterations = 100u, warmUpIterations=0u,step=25u,data=20)
		public void Method2(){}
	}

0.0.2.0

* Init/Clean attributes replaced with instance constructor and Dispose method calls, so now spirit of FattyRunner is closer to xUnit then to mstest
* Added example of script to run end to end tests
* Class constructor now can receive external context which contains iterations will be ran on current step, method name and instance of IFatLogger
* Added IFatLogger which allow tests to report it's internal steps status

0.0.1.5

* First working version
* Supported Init/Clean attributes
* Command line runner implemented
