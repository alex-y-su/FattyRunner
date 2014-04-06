namespace FattyRunner.Engine

type TestReference =
    { Type    : System.Type
      Run     : System.Reflection.MethodInfo
      Init    : System.Reflection.MemberInfo option
      Dispose : System.Reflection.MemberFilter option }

type TestConfiguration = 
    { Count  : int
      WarmUp : int }

type Test = 
    { Reference     : TestReference
      Configuration : TestConfiguration
      Context       : Map<string,obj> option }

type Configuration = 
    { Count: int 
      TestsToRun: Test list }

type EnvironmentConfiguration = 
    { ProfileMemory: bool }

module ReflectionHelper =
    open System
    open System.Reflection

    let instantiate (t:Type) (args: obj[])=
        Activator.CreateInstance(t,args)

module TestRunnerEngine =
    open System
    
    let prepareEnvironment cfg =
        ()

    let shutdownEnvironment cfg =
        ()

    let beforeStart config =
        ()

    let afterEnd config =
        ()

    let executeOnce x =
        let startTime = DateTime.Now.Ticks
        do x() |> ignore
        let endTime = DateTime.Now.Ticks
        endTime - startTime

    let runTest (cfg: EnvironmentConfiguration) (t:Test) =
        do prepareEnvironment cfg
        
        let instance = 
            match t.Context with 
            | Some(ctx) ->
                ReflectionHelper.instantiate t.Reference.Type [|ctx|]
            | None ->
                ReflectionHelper.instantiate t.Reference.Type null

        let fu() = 
            t.Reference.Run.Invoke(instance,null) 

        let decoratedExecute x = 
            beforeStart cfg
            let res = executeOnce x
            afterEnd cfg
            res

        let timings = seq { 1..t.Configuration.Count } 
                        |> Seq.map (fun x -> (x, decoratedExecute fu))
                        |> Seq.toArray
        
        do shutdownEnvironment cfg
        timings
    
    let run (tests:Test list) config = 
        tests |> Seq.map (runTest config)
    