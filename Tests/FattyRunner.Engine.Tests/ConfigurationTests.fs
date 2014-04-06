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
        
        TestRunnerEngine.runTest { Context = None; Count = None } |> ignore

module ``Assembly reading test`` =
    open Xunit
    open FsUnit.Xunit
    open FattyRunner.Engine
    open ReflectiponHelpers
    open TestHelpers
    open NHamcrest
    open NHamcrest.Core

    let Empty<'a> = CustomMatcher<'a list>("Collection is empty", fun coll -> coll.Length > 0) 

    [<Fact>]
    let ``Assembly loader should find all types with fatty methods``() =
        let asm = typeof<AssemblyLoadTests.PrimitiveFatTestsContainer>.Assembly
        let tests = TestLoader.load asm { Context = None; Count = None } |> Seq.toList

        tests.IsEmpty |> should be False
        tests.Length |> should equal 2
        
        tests |> List.filter (fun x-> Option.isSome x.Reference.Init) 
              |> List.length 
              |> should equal 1
        
        tests |> List.filter (fun x-> Option.isSome x.Reference.Dispose) 
              |> List.length 
              |> should equal 1
