namespace FattyRunner.Engine
open System.Runtime.Serialization

type TestReference = 
    { Type : System.Type
      Run : System.Reflection.MethodInfo
      AfterWarmUp: System.Reflection.MethodInfo option }

type TestConfiguration = 
    { Count : uint32
      WarmUp : uint32
      ProgressiveStep: uint32
      Data: obj option }

type Test = 
    { Reference : TestReference
      Configuration : TestConfiguration }

type Configuration = 
    { TestsToRun : Test list }

[<DataContract>]
type EnvironmentConfiguration = 
    { Logger : FattyRunner.Interfaces.IFatLogger
      Count  : uint32 option }

type TimeMeasure = 
    { IterationCount: uint32
      Time: uint64 }

[<DataContract>]
type TestResult = 
    { TestName : string
      Timings  : TimeMeasure list }