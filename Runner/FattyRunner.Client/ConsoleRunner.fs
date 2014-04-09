namespace FattyRunner.Client

type RunConfiguration = 
    { AssemblyLocation : string
      TestList : string list
      IterationsCount : uint32 option }

module ConfigurationHelpers = 
    open Azon.Helpers.Extensions
    
    let normalize (s : string) = s.SubstringAfter ":"
    
    let (|Location|_|) (s : string) = 
        if s.StartsWith("path:") then Some(normalize s)
        else None
    
    let (|Test|_|) (s : string) = 
        if s.StartsWith("test:") then Some(normalize s)
        else None
    
    let (|Count|_|) (s : string) = 
        if s.StartsWith("n:") then 
            let n = normalize s
            Some(System.UInt32.Parse n)
        else None
    
    let defaultConfiguration =
            { AssemblyLocation = ""
              TestList = []
              IterationsCount = None }


    let readConfigFromArgs (args : string list) : RunConfiguration = 
        let rec read' args cfg = 
            match args with
            | h :: t -> 
                let cfg' = 
                    match h with
                    | Location(x) -> { cfg with AssemblyLocation = x }
                    | Test(x) -> { cfg with TestList = cfg.TestList @ [ x ] }
                    | Count(x) -> { cfg with IterationsCount = Some(x) }
                    | _ -> cfg
                read' t cfg'
            | [] -> cfg
        read' args defaultConfiguration


module ConsoleRunner = 
    open System.Linq
    open Azon.Helpers.Extensions
    open ConfigurationHelpers
    open FattyRunner.Engine

    [<Literal>]
    let helpMessage = @"Usage: 
                        n:[number]  - Count of iterations

                        path:[path] - File or directory where test assemblies are located.\n 
                                      Can be used multiple times.
                    
                        test:[name] - Full class name + method name like ""MyNamespace.MyClass.MethodToTest"".\n
                                      Can be used multiple times."

    let writeConfigurationIncorrect() =
        printfn "%s" helpMessage
        List<Test>.Empty

    let testsFromFile (s:string) (envConf) = 
        let asm = TestLoader.loadAssemblyFromFile s
        match asm with 
        | Some(asm) -> TestLoader.loadTests envConf asm |> Seq.toList
        | _ -> List<Test>.Empty

    let testsFromDir (s:string) envConf = 
        let assemblies =  TestLoader.loadAllAssembliesFromDirectory s

        assemblies |> Seq.map (TestLoader.loadTests envConf)
                   |> Seq.concat
                   |> Seq.toList

    let runForConfig (cfg: RunConfiguration) =
        let envConf : EnvironmentConfiguration =
            { Count = cfg.IterationsCount; Context = None }

        let testNames = cfg.TestList |> Set.ofList
        
        let filter (t:Test) =
            if not(testNames.Any()) then true
            else
                let name = sprintf "%s.%s" t.Reference.Type.FullName t.Reference.Run.Name
                testNames.Contains name
        
        let tests = match cfg.AssemblyLocation with
                    | x when System.IO.File.Exists x -> 
                        testsFromFile x envConf
                    | x when System.IO.Directory.Exists x -> 
                        testsFromDir x envConf
                    | _ -> writeConfigurationIncorrect()

        let testsToRun = List.filter filter tests
        TestRunnerEngine.run testsToRun envConf    

    let run args = 
        let cfg = readConfigFromArgs (args |> List.ofArray)
        let results = runForConfig cfg
        
        0
