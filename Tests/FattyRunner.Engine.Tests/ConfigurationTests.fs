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

    let emptyGConf = { Context = None; Count = None }

    [<Fact>]
    let ``Should read configuration from attribute``() =
        let t = typeof<AssemblyLoadTests.PrimitiveFatTestsContainer>
        let test = TestLoader.loadTests t.Assembly emptyGConf 
                   |> Seq.find (fun x -> x.Reference.Type = t)
        
        let actual = test.Configuration
        actual.Equals({ Count = 3000u; WarmUp = 150u; ProgressiveStep = 300u })
        |> should be True

    [<Fact>]
    let ``Should find proper assemblies in directory``() =
        let dir = System.AppDomain.CurrentDomain.BaseDirectory
        let foundAssemblies = 
            TestLoader.loadAllFromDirectory dir |> Seq.toList
        let expectedName = 
            typeof<AssemblyLoadTests.PrimitiveFatTestsContainer>.Assembly.FullName
        
        foundAssemblies |> Seq.length
                        |> should greaterThan 1

        foundAssemblies |> Seq.exists (fun x-> x.FullName = expectedName)
                        |> should be True

    [<Fact>]
    let ``Assembly loader should find all types with fatty methods``() =
        let asm = typeof<AssemblyLoadTests.PrimitiveFatTestsContainer>.Assembly
        let tests = TestLoader.loadTests asm emptyGConf |> Seq.toList

        tests.IsEmpty |> should be False
        tests.Length |> should equal 2
        
        tests |> List.filter (fun x-> Option.isSome x.Reference.Init) 
              |> List.length 
              |> should equal 1
        
        tests |> List.filter (fun x-> Option.isSome x.Reference.Dispose) 
              |> List.length 
              |> should equal 1
