namespace FattyRunner.Engine.Tests

type Counter(init) =
    let mutable _val = init
    member this.Count:int = _val
    member this.Add() = 
        _val <- _val + 1

module ReflectiponHelper =
    open System
    open System.Reflection
    open Microsoft.FSharp.Metadata
    
    let getMethodReference (t:Type) name =
        t.GetMethod(name)

module ``Engine tests`` =
    open Xunit
    open FsUnit.Xunit
    open FattyRunner.Engine

    open ReflectiponHelper
    
    type TestType(ctx:Map<string,obj>) =
        let counter = ctx.["counter"] :?> Counter
        member x.Run() =
            counter.Add() 

    let environmentConfig = { ProfileMemory = false}:EnvironmentConfiguration

    [<Fact>]
    let ``Should call test N times``() = 
        let N = 100
        let counter = new Counter(0)
        
        let t = typeof<TestType>
        let m = ReflectiponHelper.getMethodReference t "Run"
        
        let tRef = { Type = t
                     Run = m
                     Init = None
                     Dispose = None }:TestReference
        
        let config = { Count  = N
                       WarmUp = 0 }: TestConfiguration
        
        let context = seq{ yield "counter", (counter :> obj) } |> Map.ofSeq

        let test = { Reference = tRef; 
                     Configuration = config 
                     Context = Some(context) }: Test

        TestRunnerEngine.runTest environmentConfig test |> ignore

        counter.Count |> should equal N
