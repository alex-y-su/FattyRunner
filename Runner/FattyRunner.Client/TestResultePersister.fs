namespace FattyRunner.Client

module TestResultePersister =
    open System
    open FattyRunner.Engine
    open Newtonsoft.Json

    type TestResultContainer = 
        { Tests: TestResult seq }
    
    let serialize (tests: TestResult seq) =
        JsonConvert.SerializeObject({ Tests = tests }, Formatting.Indented)
