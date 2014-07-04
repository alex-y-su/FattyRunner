namespace FattyRunner.Client

[<assembly: System.Runtime.CompilerServices.InternalsVisibleToAttribute("FattyRunner.Tests")>]
do()

module MainApp =
    [<EntryPoint>]
    let main args = ConsoleRunner.run args
           
    