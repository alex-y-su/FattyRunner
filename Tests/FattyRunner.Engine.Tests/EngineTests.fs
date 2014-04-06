namespace FattyRunner.Engine.Tests

type Counter(init) = 
    let mutable _val = init
    member this.Count :uint32 = _val
    member this.Add() = _val <- _val + 1u

module ReflectiponHelper = 
    open System
    open System.Reflection
    open Microsoft.FSharp.Metadata
    
    let getMethodReference (t : Type) name = t.GetMethod(name)


module TestHelpers =
    open FattyRunner.Engine
    open ReflectiponHelper
    
    let createTestRef (t:System.Type) mName =
        let m = ReflectiponHelper.getMethodReference t mName
        { Type = t
          Run = m
          Init = None
          Dispose = None } : TestReference

module ``Engine objects tests`` =
    open Xunit
    open FsUnit.Xunit
    open FattyRunner.Engine
    
    open ReflectiponHelper
    open TestHelpers
    type TestTypeWithoutCtor() =
        member x.Run() = ()

    [<Fact>]
    let ``Test without context can create instance of type with empty ctor``() =
        let config = 
            { Count = 1u; ProgressiveStep = 1u; WarmUp = 0u }
        
        let test : Test = 
            { Reference = createTestRef typeof<TestTypeWithoutCtor> "Run"
              Configuration = config }
        
        TestRunnerEngine.runTest { Context = None } |> ignore    

module ``Engine counter tests`` = 
    open Xunit
    open FsUnit.Xunit
    open FattyRunner.Engine
    open ReflectiponHelper
    open TestHelpers

    type TestType(ctx : Map<string, obj>) = 
        let counter = ctx.["counter"] :?> Counter
        member x.Run() = counter.Add()

    [<Fact>]
    let ``Should call method body N times``() = 
        let counter = new Counter(0u)
        
        let config = 
            { Count = 10u; ProgressiveStep = 1u; WarmUp = 0u }
        
        let context = 
            seq { yield "counter", (counter :> obj) } |> Map.ofSeq
        
        let test : Test = 
            { Reference = createTestRef typeof<TestType> "Run"
              Configuration = config }
        
        TestRunnerEngine.runTest { Context = Some(context) } test |> ignore
        let expected = seq {1..10} |> Seq.sum |> uint32
        counter.Count |> should equal expected

    [<Fact>]
    let ``Should call warm up``() =
        let counter = new Counter(0u)
        
        let config = 
            { Count = 1u; ProgressiveStep = 1u; WarmUp = 10u }
        
        let context = 
            seq { yield "counter", (counter :> obj) } |> Map.ofSeq
        
        let test : Test = 
            { Reference = createTestRef typeof<TestType> "Run"
              Configuration = config }
        
        TestRunnerEngine.runTest { Context = Some(context) } test |> ignore

        counter.Count |> should equal 11u