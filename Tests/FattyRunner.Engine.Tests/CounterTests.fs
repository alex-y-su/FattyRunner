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
    
    type TestTypeWithInint(ctx : Map<string, obj>) =
        let counter = ctx.["counter"] :?> Counter
        member x.Service() = counter.Add()
        member x.Run() = ()

    [<Fact>]
    let ``Should call init if initialized N/Step times``() =
        let counter = new Counter(0u)
        
        let config = 
            { Count = 100u
              ProgressiveStep = 10u
              WarmUp = 100u }
        
        let context = seq { yield "counter", (counter :> obj) } |> Map.ofSeq
        let t = typeof<TestTypeWithInint>
        let testRef = 
            { Type = t
              Run = getMethodReference t "Run"
              Init = Some(getMethodReference t "Service")
              Dispose = None}
        
        let test : Test = 
            { Reference = testRef
              Configuration = config }
        
        let envConfig = { Context = Some(context); Count = None }
        let results = TestRunnerEngine.runTest envConfig test
        counter.Count |> should equal 10u
    
    [<Fact>]
    let ``Should call cleanup if initialized N/Step times``() =
        let counter = new Counter(0u)
        
        let config = 
            { Count = 100u
              ProgressiveStep = 10u
              WarmUp = 100u }
        
        let context = seq { yield "counter", (counter :> obj) } |> Map.ofSeq
        let t = typeof<TestTypeWithInint>
        let testRef = 
            { Type = t
              Run = getMethodReference t "Run"
              Init = None
              Dispose = Some(getMethodReference t "Service") }
        
        let test : Test = 
            { Reference = testRef
              Configuration = config }
        
        let envConfig = { Context = Some(context); Count = None }
        let results = TestRunnerEngine.runTest envConfig test
        counter.Count |> should equal 10u

    [<Fact>]
    let ``Should call init/cleanup if initialized N/Step times``() =
        let counter = new Counter(0u)
        
        let config = 
            { Count = 100u
              ProgressiveStep = 10u
              WarmUp = 100u }
        
        let context = seq { yield "counter", (counter :> obj) } |> Map.ofSeq
        let t = typeof<TestTypeWithInint>
        let testRef = 
            { Type = t
              Run = getMethodReference t "Run"
              Init = Some(getMethodReference t "Service")
              Dispose = Some(getMethodReference t "Service") }
        
        let test : Test = 
            { Reference = testRef
              Configuration = config }
        
        let envConfig = { Context = Some(context); Count = None }
        let results = TestRunnerEngine.runTest envConfig test
        counter.Count |> should equal 20u
    
    [<Fact>]
    let ``Should don't call init/cleanup if not initialized``() =
        let counter = new Counter(0u)
        
        let config = 
            { Count = 100u
              ProgressiveStep = 10u
              WarmUp = 100u }
        
        let context = seq { yield "counter", (counter :> obj) } |> Map.ofSeq
        let t = typeof<TestTypeWithInint>
        let testRef = 
            { Type = t
              Run = getMethodReference t "Run"
              Init = None
              Dispose = None }
        
        let test : Test = 
            { Reference = testRef
              Configuration = config }
        
        let envConfig = { Context = Some(context); Count = None }
        let results = TestRunnerEngine.runTest envConfig test
        counter.Count |> should equal 0u

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
