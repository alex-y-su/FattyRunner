namespace FattyRunner.Engine

type TestReference = 
    { Type : System.Type
      Run : System.Reflection.MethodInfo
      Init : System.Reflection.MemberInfo option
      Dispose : System.Reflection.MemberFilter option }

type TestConfiguration = 
    { Count : uint32
      WarmUp : uint32
      ProgressiveStep: uint32 }

type Test = 
    { Reference : TestReference
      Configuration : TestConfiguration }

type Configuration = 
    { Count : uint32
      TestsToRun : Test list }

type EnvironmentConfiguration = 
    { Context : Map<string, obj> option }