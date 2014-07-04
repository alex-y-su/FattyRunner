namespace FattyRunner.Engine.Tests

module ``Assembly reading test`` =
    open Xunit
    open FsUnit.Xunit
    open FattyRunner.Engine
    open FattyRunner.Client

    open ReflectiponHelpers
    open TestHelpers
    open NHamcrest
    open NHamcrest.Core

    let emptyGConf = { Count = None; Logger = null }
    
    [<Fact>]
    let ``Global config count should override attribute iterations count``()=
        let gcfg = { Count = Some(999u); Logger = null }
        let t = typeof<AssemblyLoadTests.PrimitiveFatTestsContainer>
        let test = TestLoader.findTestDefenitions t.Assembly
                   |> Seq.map (TestLoader.loadMultistepTestRef t.Assembly)
                   |> Seq.find (fun x -> x.Type = t)
                   |> TestLoader.loadMultistepTest gcfg

        let actual = test.Configuration
        actual.Equals({ Count = 999u; WarmUp = 150u; ProgressiveStep = 300u; Data = Some("UserData" :> obj) })
        |> should be True
    

    
    [<Fact>]
    let ``Should read configuration from attribute``() =
        let t = typeof<AssemblyLoadTests.PrimitiveFatTestsContainer>
        let test = TestLoader.findTestDefenitions t.Assembly
                   |> Seq.map (TestLoader.loadMultistepTestRef t.Assembly)
                   |> Seq.find (fun x -> x.Type = t)
                   |> TestLoader.loadMultistepTest emptyGConf
        
        let actual = test.Configuration
        actual.Equals({ Count = 3000u; WarmUp = 150u; ProgressiveStep = 300u; Data = Some("UserData" :> obj) })
        |> should be True
    

    [<Fact>]
    let ``Should find proper assemblies in directory``() =
        let dir = System.AppDomain.CurrentDomain.BaseDirectory
        
        let foundAssemblies = 
            AssemblyHelpers.loadAllAssembliesFromDirectory dir |> Seq.toList

        let expectedName = 
            typeof<AssemblyLoadTests.PrimitiveFatTestsContainer>.Assembly.FullName
        
        foundAssemblies |> Seq.length
                        |> should greaterThan 1

        foundAssemblies |> Seq.exists (fun x-> x.FullName = expectedName)
                        |> should be True

    [<Fact>]
    let ``Assembly loader should find all types with fatty methods``() =
        let asm = typeof<AssemblyLoadTests.PrimitiveFatTestsContainer>.Assembly
        let tests = TestLoader.findTestDefenitions asm
        tests.Length |> should equal 2
        