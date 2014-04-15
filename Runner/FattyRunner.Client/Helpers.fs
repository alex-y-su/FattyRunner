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
        if s.StartsWith("path:") || s.StartsWith("asm:") then Some(normalize s)
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

module internal AssemblyHelpers =
    let tryLoadAssembly (s:string) =
        try 
            let testAssembly = System.Reflection.AssemblyName.GetAssemblyName(s);
            System.Reflection.Assembly.LoadFile(s) |> Some
        with
        | :? System.IO.FileNotFoundException -> None
        | :? System.BadImageFormatException -> None
        | :? System.IO.FileLoadException -> None

    let loadAllAssembliesFromDirectory (dir:string) =
        let dir = System.AppDomain.CurrentDomain.BaseDirectory
        let fileRecords = System.IO.Directory.GetFiles(dir,"*.dll")
        fileRecords |> Seq.map tryLoadAssembly
                    |> Seq.filter Option.isSome
                    |> Seq.map Option.get

    let loadAssemblyFromFile = tryLoadAssembly
