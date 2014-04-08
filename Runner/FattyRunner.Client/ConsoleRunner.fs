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
    open Azon.Helpers.Extensions
    open ConfigurationHelpers
    open FattyRunner.Engine

    [<Literal>]
    let helpMessage = @"Use 
                        n:[number]  - Count of iterations

                        path:[path] - File or directory where test assemblies are located.\n 
                                      Can be used multiple times.
                    
                        test:[name] - Full class name + method name like ""MyNamespace.MyClass.MethodToTest"".\n
                                      Can be used multiple times."

    let writeConfigurationIncorrect() =
        printfn "%s" helpMessage

    let runFile (s:string) (cfg:RunConfiguration) = 
        let config : EnvironmentConfiguration =
            { Count = cfg.IterationsCount; Context = None }
        let asm = TestLoader.loadAssemblyFromFile s
        match asm with 
        | Some(asm) -> 
            let tests = TestLoader.loadTests asm config |> Seq.toList
            let results = TestRunnerEngine.run tests config
            ()
        | _ -> raise (new System.Exception("Cannot load test from assembly"))

    let runDir (s:string) cfg = ()

    let run args = 
        let cfg = readConfigFromArgs (args |> List.ofArray)
        match cfg.AssemblyLocation with
        | x when System.IO.File.Exists x -> runFile x cfg
        | x when System.IO.Directory.Exists x -> runDir x cfg
        | _ -> do writeConfigurationIncorrect()
        0
