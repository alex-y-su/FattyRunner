namespace FattyRunner.Client

type ConsoleLogger() =
    interface FattyRunner.Interfaces.IFatLogger with
        member this.Write (fmt,args) =
            let s = System.String.Format(fmt,args)
            System.Console.WriteLine(s)

module ConsoleRunner = 
    open System.Linq
    open Azon.Helpers.Extensions
    open ConfigurationHelpers
    open FattyRunner.Engine

    type AssemblyTestDef = {
        Test : TestDefenition list
        Asmbl : System.Reflection.Assembly
    }

    [<Literal>]
    let private helpMessage = @"Usage: 
                        n:[number]  - Count of iterations

                        path:[path] - File or directory where test assemblies are located.\n 
                                      Can be used multiple times.
                    
                        test:[name] - Full class name + method name like ""MyNamespace.MyClass.MethodToTest"".\n
                                      Can be used multiple times."

    let private writeConfigurationIncorrect() =
        printfn "%s" helpMessage

    let getDefenitionFromAsm asm (selectedTests : string list) =
        let tests = TestLoader.findTestDefenitions asm
        
        let testsToRun =  
            match selectedTests with
            | [] -> tests
            | x ->
                let testNames = x |> Set.ofList
                let filter (t:TestDefenition) =
                    let name = sprintf "%s.%s" t.TypeName t.TestName
                    testNames.Contains name
                List.filter filter tests
        
        List.map (TestLoader.loadMultistepTestRef asm) testsToRun
    
    let runForConfig (conf: EnvironmentConfiguration) (tests: TestReference list) =
        let instances = List.map (TestLoader.loadMultistepTest conf) tests
        TestRunnerEngine.runTests conf instances

    let getResults globalCfg =
        let testsToRun = 
            match globalCfg.AssemblyLocation with
            | f when System.IO.File.Exists f -> 
                let asm = AssemblyHelpers.loadAssemblyFromFile f
                getDefenitionFromAsm asm.Value globalCfg.TestList
            | dir when System.IO.Directory.Exists dir -> 
                let assemblies = AssemblyHelpers.loadAllAssembliesFromDirectory dir
                seq { 
                    for asm in assemblies do
                        for x in getDefenitionFromAsm asm globalCfg.TestList do
                            yield x
                } |> Seq.toList
            | _ -> 
                do writeConfigurationIncorrect()
                List<TestReference>.Empty

        let envConf : EnvironmentConfiguration =
            { Count = globalCfg.IterationsCount 
              Logger = new ConsoleLogger() }

        runForConfig envConf testsToRun 

    let run args = 
        let globalCfg = readConfigFromArgs (args |> List.ofArray)
        let results = getResults globalCfg
        
        let fileName = 
            match globalCfg.PathToOutputFile with
            | Some(f) -> f
            | _ -> "Results.json"

        use sw = System.IO.File.CreateText fileName
        let res = TestResultePersister.serialize results 
        sw.Write(res)
        do sw.Flush()
        0
