namespace FattyRunner.Engine.Tests

module ``Engine counter tests`` = 
    open Xunit
    open FsUnit.Xunit
    open FattyRunner.Engine
    open ReflectiponHelpers
    open TestHelpers
    
    type Counter(init) = 
        let mutable _val = init
        member this.Count : uint32 = _val
        member this.Add() = _val <- _val + 1u
    
    type TestType(ctx : Map<string, obj>) = 
        let counter = ctx.["counter"] :?> Counter
        member x.Run() = counter.Add()
    
    [<Fact>]
    let ``Stack owerflow should not happen on big N``() = 
        let counter = new Counter(0u)
        
        let config = 
            { Count = 1000000u
              ProgressiveStep = 1000000u
              WarmUp = 0u }
        
        let context = seq { yield "counter", (counter :> obj) } |> Map.ofSeq
        
        let test : Test = 
            { Reference = createTestRef typeof<TestType> "Run"
              Configuration = config }
        let envConfig = { Context = Some(context); Count = None }
        let results = TestRunnerEngine.runTest envConfig test
        counter.Count |> should equal 1000000u

    [<Fact>]
    let ``Should call method body N times``() = 
        let counter = new Counter(0u)
        
        let config = 
            { Count = 10u
              ProgressiveStep = 1u
              WarmUp = 0u }
        
        let context = seq { yield "counter", (counter :> obj) } |> Map.ofSeq
        
        let test : Test = 
            { Reference = createTestRef typeof<TestType> "Run"
              Configuration = config }
        let envConfig = { Context = Some(context); Count = None }
        TestRunnerEngine.runTest envConfig test |> ignore
        let expected = 
            seq { 1..10 }
            |> Seq.sum
            |> uint32
        counter.Count |> should equal expected
    
    [<Fact>]
    let ``Should call warm up``() = 
        let counter = new Counter(0u)
        
        let config = 
            { Count = 1u
              ProgressiveStep = 1u
              WarmUp = 10u }
        
        let context = seq { yield "counter", (counter :> obj) } |> Map.ofSeq
        
        let test : Test = 
            { Reference = createTestRef typeof<TestType> "Run"
              Configuration = config }
        let envConfig = { Context = Some(context); Count = None }
        TestRunnerEngine.runTest envConfig test |> ignore
        counter.Count |> should equal 11u
