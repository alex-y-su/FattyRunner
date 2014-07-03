namespace FattyRunner.Engine.Tests

open FattyRunner.Engine
open TestLoaderTestAssembly

module TestLoaderTest =    
    let ``Should filter assembly tests by input``() =
        let cfg = TestHelpers.emptyConfig
        let asm = typeof<LoadTestsByList>.Assembly
        let res = TestLoader.loadTests cfg asm ["TestLoaderTestAssembly.LoadTestsByList.ShouldBeLoaded"] |> Seq.toList
        ()
