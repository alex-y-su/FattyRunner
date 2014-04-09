module MainApp

open System
open System.Windows
open System.Windows.Controls
open System.Windows.Markup

open FattyRunner.Client

//mainWindowViewModel.DataContext <- new MainWindowViewModel() 

// Application Entry point
[<STAThread>]
[<EntryPoint>]
let main args =
    if args.Length > 0 then ConsoleRunner.run args
    else
        let mainWindowViewModel = 
            Application.LoadComponent(new System.Uri("MainWindow.xaml", UriKind.Relative)) :?> Window 
        (new Application()).Run(mainWindowViewModel)
    