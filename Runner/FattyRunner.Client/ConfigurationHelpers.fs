namespace FattyRunner.Client

type RunConfiguration = 
    { AssemblyLocation : string
      TestList : string list
      PathToOutputFile: string option
      IterationsCount : uint32 option }

module ConfigurationHelpers = 
    open Azon.Helpers.Extensions
    
    let normalize (s : string) = s.SubstringAfter ":"
    
    let (|Location|_|) (s : string) = 
        if s.StartsWith("path:") then Some(normalize s)
        else None

    let (|Out|_|) (s : string) = 
        if s.StartsWith("out:") then Some(normalize s)
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
              PathToOutputFile = None
              IterationsCount = None }


    let readConfigFromArgs (args : string list) : RunConfiguration = 
        let rec read' args cfg = 
            match args with
            | h :: t -> 
                let cfg' = 
                    match h with
                    | Location(x) -> { cfg with AssemblyLocation = x }
                    | Out(x)      -> { cfg with PathToOutputFile = Some(x) }
                    | Test(x)     -> { cfg with TestList = cfg.TestList @ [ x ] }
                    | Count(x)    -> { cfg with IterationsCount = Some(x) }
                    | _ -> cfg
                read' t cfg'
            | [] -> cfg
        read' args defaultConfiguration