namespace FattyRunner.Engine

type TestReference = 
    { Type : System.Type
      Run : System.Reflection.MethodInfo
      Init : System.Reflection.MethodInfo option
      Dispose : System.Reflection.MethodInfo option }

type TestConfiguration = 
    { Count : uint32
      WarmUp : uint32
      ProgressiveStep: uint32 }

type Test = 
    { Reference : TestReference
      Configuration : TestConfiguration }

type Configuration = 
    { TestsToRun : Test list }

type EnvironmentConfiguration = 
    { Context : Map<string, obj> option
      Count : uint32 option }

type TimeMeasure = 
    { IterationCount: uint32
      Time: uint64 }

type TestResult = 
    { TestName : string
      Timings  : TimeMeasure list }