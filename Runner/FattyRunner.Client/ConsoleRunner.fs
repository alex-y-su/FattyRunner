namespace FattyRunner.Client

type RunConfiguration = 
    { AssemblyFile: string
      TestName: string option
      IterationsCount: uint32 option }




module ConsoleRunner = 
    let fillConfigFromArgs args =
        ()
    
    let run args =
        
        0

