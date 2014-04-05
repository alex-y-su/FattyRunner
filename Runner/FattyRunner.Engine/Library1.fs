namespace FattyRunner.Engine

type Test = 
    { FullName: string }

type Configuration = 
    { Count: int 
      TestsToRun: Test list }

module TestRunner =
    
    

    let WarmUp() =
        ()

    type Class1() = 
        member this.X = "F#"
