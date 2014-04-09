namespace FattyRunner.Engine

module TestRunnerEngine = 
    open System
    open System.Diagnostics

    let callOption (f:System.Reflection.MethodInfo option) instance =
        match f with
        | Some(f) -> f.Invoke(instance,null) |> ignore
        | _ -> ()

    let prepareEnvironment cfg = ()
    let shutdownEnvironment cfg = ()
    let beforeStart config = ()
    let afterEnd config = ()
    
    let executeStep n x = 
        let rec exec' f n =
            if n = 0 then ()
            else do f() |> ignore  
                 exec' f (n-1)

        let sw = Stopwatch.StartNew();
        do exec' x n
        do sw.Stop()
        sw.ElapsedMilliseconds
    
    let runTest (cfg:EnvironmentConfiguration) (t : Test) = 
        do prepareEnvironment cfg
        let instance = ReflectionHelper.instantiate t.Reference.Type cfg.Context
        
        let before() = 
            match t.Reference.Init with
            | Some(f) -> f.Invoke(instance,null) |> ignore
                         beforeStart cfg
            | _ -> beforeStart cfg

        let after() = 
            match t.Reference.Dispose with
            | Some(f) -> afterEnd cfg
                         f.Invoke(instance,null) |> ignore
            | _ -> afterEnd cfg

        let decoratedExecute n x = 
            do before()
            let res = executeStep n x
            do after()
            res
        
        let step = 
            match t.Configuration.ProgressiveStep with
            | 0u -> 1 
            | x when x > t.Configuration.Count -> 
                int t.Configuration.Count
            | x -> int x

        let count = int t.Configuration.Count
        
        let fu() = t.Reference.Run.Invoke(instance, null)

        match t.Configuration.WarmUp with
        | 0u -> () 
        | x ->
            callOption t.Reference.Init instance
            seq {1..int x } |> Seq.iter (fun _-> fu() |> ignore)
            callOption t.Reference.Dispose instance


        do System.Threading.Thread.Sleep(5)

        let timings = 
            seq { step..step..count }
            |> Seq.map (fun x ->  x, decoratedExecute x fu)
            |> Seq.map (fun (n,t) -> { IterationCount = uint32 n 
                                       Time = uint64 t} : TimeMeasure)
            |> Seq.toList
        
        do shutdownEnvironment cfg
        let testName = sprintf "%s.%s" t.Reference.Type.FullName t.Reference.Run.Name
        { TestName = testName; Timings = timings} : TestResult
    
    let run (tests : Test list) (cfg: EnvironmentConfiguration) : TestResult list = 
        tests |> List.map (runTest cfg)
