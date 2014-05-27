namespace FattyRunner.Engine.Tests

module ``App domain wrapper tests`` =
    open Xunit
    open FsUnit.Xunit
    open FattyRunner.Engine
    open FattyRunner.Client

    open System
    open System.Reflection

    [<Fact>]
    let ``Should load runner assembly correct even if base path broken``() =
        let tasm = typeof<AssemblyLoadTests.PrimitiveFatTestsContainer>.Assembly.Location
        let cfg = { Logger = EmptyLogger.Instance; Count = None }
        let results = AppDomainWrapper.runForAssembly cfg tasm
        results |> List.length |> should equal 2
