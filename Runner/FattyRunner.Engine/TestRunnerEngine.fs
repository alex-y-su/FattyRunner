namespace FattyRunner.Engine

module TestRunnerEngine = 
    open System
    open System.Diagnostics
    open FattyRunner.Interfaces

    let prepareEnvironment cfg = ()
    let shutdownEnvironment cfg = ()
    let beforeStart config = ()
    let afterEnd config = ()
    
    let rec exec' f n =
            if n = 0u then ()
            else do f() |> ignore  
                 exec' f (n-1u)

    let executeStep ctor (t:Test) (n:uint32)  =
        let instance = ctor n

        let fu() = 
            t.Reference.Run.Invoke(instance,null)

        match t.Configuration.WarmUp with
        | 0u -> () 
        | x  -> seq {1..int x } |> Seq.iter (fun _-> fu() |> ignore)
        
        ReflectionHelper.callOption instance t.Reference.AfterWarmUp
        do System.Threading.Thread.Sleep(5)

        let sw = Stopwatch.StartNew();
        do exec' fu n
        do sw.Stop()

        ReflectionHelper.getDisposeReference t.Reference.Type 
        |> ReflectionHelper.callOption instance
        
        sw.ElapsedMilliseconds
    
    let runTest (cfg:EnvironmentConfiguration) (testRec : Test) = 
        do prepareEnvironment cfg

        let ctor iters =
            let userData = if Option.isNone testRec.Configuration.Data then null
                           else Option.get testRec.Configuration.Data 
            let stepContext =
                new ExternalContext(iters,testRec.Configuration.WarmUp ,testRec.Reference.Run.Name, cfg.Logger, userData)
            ReflectionHelper.instantiate testRec.Reference.Type stepContext

        let decoratedExecute n = 
            do beforeStart cfg
            let res = executeStep ctor testRec n
            do afterEnd cfg
            res
        
        let step = 
            match testRec.Configuration.ProgressiveStep with
            | 0u -> 1u 
            | x when x > testRec.Configuration.Count -> 
                testRec.Configuration.Count
            | x -> x

        let count = testRec.Configuration.Count
        
        let timings = 
            seq { step..step..count }
            |> Seq.map (fun x ->  x, decoratedExecute x)
            |> Seq.map (fun (n,t) -> { IterationCount = n 
                                       Time = uint64 t} : TimeMeasure)
            |> Seq.toList
        
        do shutdownEnvironment cfg
        let testName = sprintf "%s.%s" testRec.Reference.Type.FullName testRec.Reference.Run.Name
        { TestName = testName; Timings = timings} : TestResult
    
    let run (tests : Test list) (cfg: EnvironmentConfiguration) : TestResult list = 
         List.map (runTest cfg) tests
