namespace FattyRunner.Client

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
                let r = t.Reference
                let name = sprintf "%s.%s" r.Type.FullName r.Run.Name
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
        
        let fileName = 
            match cfg.PathToOutputFile with
            | Some(f) -> f
            | _ -> "Results.json"

        use sw = System.IO.File.CreateText fileName
        let res = TestResultePersister.serialize results 
        sw.Write(res)
        do sw.Flush()
        0
