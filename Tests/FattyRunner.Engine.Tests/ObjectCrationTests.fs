namespace FattyRunner.Engine.Tests

module ``Engine objects tests`` = 
    open Xunit
    open FsUnit.Xunit
    open FattyRunner.Engine
    open ReflectiponHelpers
    open TestHelpers
    
    type TestTypeWithoutCtor() = 
        member x.Run() = ()

    type TestTypeWithCtor (ctx : FattyRunner.Interfaces.ExternalContext) =
        do ctx.Logger.Write("UserData",ctx.Data)
        member x.Run() = ()

    type CtorValidator() =
        let items = 
            new System.Collections.Generic.Dictionary<string,obj>()
        interface FattyRunner.Interfaces.IFatLogger with
            member this.Write(id,x) =
                items.[id] <- x.[0]

        member this.Items with get() = items
        member this.UserData with get() = items.["UserData"]

    [<Fact>]
    let ``Test without context can create instance of type with empty ctor``() = 
        let config = 
            { Count = 1u
              ProgressiveStep = 1u
              WarmUp = 0u
              Data = None }
        
        let test : Test = 
            { Reference = createTestRef typeof<TestTypeWithoutCtor> "Run"
              Configuration = config }
        
        TestRunnerEngine.runTest { Logger = EmptyLogger.Instance; Count = None } test |> ignore
    
    [<Fact>]
    let ``Should pass user data to .ctor`` () =
        let logger = new CtorValidator()

        let config = 
            { Count = 1u
              ProgressiveStep = 1u
              WarmUp = 0u
              Data = Some(box 42) }
        
        let test : Test = 
            { Reference = createTestRef typeof<TestTypeWithCtor> "Run"
              Configuration = config }
        
        let res = TestRunnerEngine.runTest { Logger = logger; Count = None } test |> ignore
        int (unbox logger.UserData) |> should equal 42
