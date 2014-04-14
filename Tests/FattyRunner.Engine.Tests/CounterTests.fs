namespace FattyRunner.Engine.Tests

module ``Engine counter tests`` = 
    open Xunit
    open FsUnit.Xunit
    open FattyRunner.Engine
    open ReflectiponHelpers
    open TestHelpers
    open FattyRunner.Interfaces
    
    type CountLogger() = 
        let items = 
            new System.Collections.Generic.Dictionary<string,uint32>()
        interface IFatLogger with
            member this.Write(id,_) =
                if items.ContainsKey id then
                    items.[id] <- items.[id] + 1u
                else items.[id] <- 1u

        member this.Items with get() = items
        member this.RunCount with get() = items.["Run"]
        member this.InitCount with get() = items.[".ctor"]
        member this.DisposeCount with get() = items.["Dispose"]
    
    type TestType(ctx : ExternalContext) = 
        do ctx.Logger.Write(".ctor")
        interface System.IDisposable with
            member this.Dispose() =
                ctx.Logger.Write("Dispose")        
        member this.Run() = ctx.Logger.Write("Run")
    
    [<Fact>]
    let ``Stack owerflow should not happen on big N``() = 
        let counter = new CountLogger()
        
        let config = 
            { Count = 1000000u
              ProgressiveStep = 1000000u
              WarmUp = 0u
              Data = None }
        
        let test : Test = 
            { Reference = createTestRef typeof<TestType> "Run"
              Configuration = config }
        let envConfig = { Logger = counter; Count = None }
        let results = TestRunnerEngine.runTest envConfig test
        counter.RunCount |> should equal 1000000u

    [<Fact>]
    let ``Should call method body N times``() = 
        let counter = new CountLogger()
        
        let config = 
            { Count = 10u
              ProgressiveStep = 1u
              WarmUp = 0u
              Data = None }
        
        let test : Test = 
            { Reference = createTestRef typeof<TestType> "Run"
              Configuration = config }

        let envConfig = { Logger = counter; Count = None }
        TestRunnerEngine.runTest envConfig test |> ignore
        let expected = 
            seq { 1..10 }
            |> Seq.sum
            |> uint32

        counter.RunCount |> should equal expected
    
    [<Fact>]
    let ``Should call init N/step time``()=
        let counter = new CountLogger()

        let config = 
            { Count = 10u
              ProgressiveStep = 1u
              WarmUp = 0u
              Data = None }
        
        let test : Test = 
            { Reference = createTestRef typeof<TestType> "Run"
              Configuration = config }
        let envConfig = { Logger = counter; Count = None }
        TestRunnerEngine.runTest envConfig test |> ignore
        counter.InitCount |> should equal 10u

    [<Fact>]
    let ``Should call dispose N/step time``()=
        let counter = new CountLogger()

        let config = 
            { Count = 10u
              ProgressiveStep = 1u
              WarmUp = 0u
              Data = None }
        
        let test : Test = 
            { Reference = createTestRef typeof<TestType> "Run"
              Configuration = config }
        let envConfig = { Logger = counter; Count = None }
        TestRunnerEngine.runTest envConfig test |> ignore
        counter.DisposeCount |> should equal 10u

    [<Fact>]
    let ``Should call warm up``() = 
        let counter = new CountLogger()
        
        let config = 
            { Count = 1u
              ProgressiveStep = 1u
              WarmUp = 10u
              Data = None }
        
        let test : Test = 
            { Reference = createTestRef typeof<TestType> "Run"
              Configuration = config }
        
        let envConfig = { Logger = counter; Count = None }
        
        TestRunnerEngine.runTest envConfig test |> ignore

        counter.RunCount |> should equal 11u
