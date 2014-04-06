namespace FattyRunner.Engine.Tests

module ``Engine objects tests`` = 
    open Xunit
    open FsUnit.Xunit
    open FattyRunner.Engine
    open ReflectiponHelpers
    open TestHelpers
    
    type TestTypeWithoutCtor() = 
        member x.Run() = ()
    
    [<Fact>]
    let ``Test without context can create instance of type with empty ctor``() = 
        let config = 
            { Count = 1u
              ProgressiveStep = 1u
              WarmUp = 0u }
        
        let test : Test = 
            { Reference = createTestRef typeof<TestTypeWithoutCtor> "Run"
              Configuration = config }
        
        TestRunnerEngine.runTest { Context = None } |> ignore
