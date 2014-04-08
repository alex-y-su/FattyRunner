module MainApp

open System
open System.Windows
open System.Windows.Controls
open System.Windows.Markup

open FattyRunner.Client

// Create the View and bind it to the View Model
let mainWindowViewModel = Application.LoadComponent(
                             new System.Uri("/App;component/mainwindow.xaml", UriKind.Relative)) :?> Window

//mainWindowViewModel.DataContext <- new MainWindowViewModel() 

// Application Entry point
[<STAThread>]
[<EntryPoint>]
let main args =
    if args.Length > 0 then ConsoleRunner.run args
    else (new Application()).Run(mainWindowViewModel)