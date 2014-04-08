namespace FattyRunner.Client

type RunConfiguration = 
    { AssemblyLocation : string
      TestName : string option
      IterationsCount : uint32 option }

module ConfigurationHelpers =
    open Azon.Helpers.Extensions

    let normalize (s:string) = s.SubstringAfter ":"

    let (|Location|_|) (s: string) =
        if s.StartsWith("path:") then Some(normalize s)
        else None

    let (|Test|_|) (s: string) =
        if s.StartsWith("test:") then Some(normalize s)
        else None

    let (|Count|_|) (s: string) =
        if s.StartsWith("n:") then 
            let n = normalize s
            Some(System.UInt32.Parse n)
        else None

    let rec readConfigFromArgs (args:string list) cfg = 
        match args with
        | h::t -> 
            let cfg' = 
                match h with
                | Location(x) -> { cfg with AssemblyLocation = x }
                | Test(x) -> { cfg with TestName = Some(x) }
                | Count(x) -> { cfg with IterationsCount = Some(x) }
                | _ -> cfg
            readConfigFromArgs t cfg
        | [] -> cfg

    let defaultConfiguration = 
        { AssemblyLocation = System.AppDomain.CurrentDomain.BaseDirectory
          TestName = None
          IterationsCount = None}


module ConsoleRunner = 
    open Azon.Helpers.Extensions
    open ConfigurationHelpers

    let run args =
        let cfg = readConfigFromArgs (args |> List.ofArray) defaultConfiguration

        0