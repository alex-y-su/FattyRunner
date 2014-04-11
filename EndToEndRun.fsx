#r @"Runner\FattyRunner.Client\bin\Debug\Azon.Helpers.dll"
#r @"Runner\FattyRunner.Client\bin\Debug\Newtonsoft.Json.dll"
#r @"Runner\FattyRunner.Client\bin\Debug\FattyRunner.Interfaces.dll"
#r @"Runner\FattyRunner.Client\bin\Debug\FattyRunner.Engine.dll"
#r @"Runner\FattyRunner.Client\bin\Debug\FattyRunner.Client.exe"
 
let assembliesToRun = [ 
                        //@".\Tests\Resources\AssemblyLoadTest\bin\Debug\AssemblyLoadTests.dll" 
                        @".\Tests\Resources\PerfTestsContainer\bin\Debug\PerfTestsContainer.dll" 
                      ]
 
module InteractiveRunner = 
    open FattyRunner.Client
    open FattyRunner.Engine
    
    let fileToConf f = 
        let f' = System.IO.Path.GetFileName f
        { AssemblyLocation = f
          IterationsCount = None
          PathToOutputFile = Some(f')
          TestList = [] } : RunConfiguration
    
    let runOne (file : string) = 
        let cfg = fileToConf file
        ConsoleRunner.runForConfig cfg
    
    let run (files : string list) = 
        let print (tr:TestResult) =
           let max = tr.Timings |> List.maxBy (fun x-> x.IterationCount)
           printfn "Test: %s Result: %d iters -> %d ms" tr.TestName max.IterationCount max.Time 
 
        files
        |> List.map runOne
        |> List.concat
        |> List.iter print
 
let getAbsPath s = System.IO.Path.Combine(__SOURCE_DIRECTORY__, s)
let exists f = System.IO.File.Exists f
let absPaths = assembliesToRun |> List.map getAbsPath
 
if absPaths |> List.forall exists then 
    printfn "------------------------------ Test results ---------------------------"
    InteractiveRunner.run absPaths
    printfn "-----------------------------------------------------------------------"
else 
    let notFound = absPaths |> List.filter (not << exists)
    notFound |> printfn "======================= These files was not found: ====================\n %A 
                        \n====================================================================="