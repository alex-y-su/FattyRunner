namespace FattyRunner.Client

module MainApp =
    open System
    open System.Windows.Controls
    open System.Windows.Markup

    open FattyRunner.Client

    //mainWindowViewModel.DataContext <- new MainWindowViewModel() 

    // Application Entry point
    [<STAThread>]
    [<EntryPoint>]
    let main args = ConsoleRunner.run args
           
    