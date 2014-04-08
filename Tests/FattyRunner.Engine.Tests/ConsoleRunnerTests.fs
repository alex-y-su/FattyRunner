namespace FattyRunner.Client.Tests

module ``Console runner tests`` =
    open Xunit
    open FsUnit.Xunit
    open FattyRunner.Engine
    open FattyRunner.Client

    [<Fact>]
    let ``End to end test should pass``() =
        let sb = new System.Text.StringBuilder()
        use tw = new System.IO.StringWriter(sb)
        System.Console.SetOut(tw)
        
        let dir = System.AppDomain.CurrentDomain.BaseDirectory
        let fn = System.IO.Path.Combine(dir,"PerfTestsContainer.dll")
        let args = [| sprintf "path:%s" fn; "n:20" |]
        ConsoleRunner.run args |> should equal 0
       
        do tw.Flush()
        let res = sb.ToString()
        //n = 20 + 50 warm up (default) and 1 because of split
        res.Split('\n').Length-1 |> should equal (20+50)

    [<Fact>]
    let ``Should print help when args are wrong`` () =
        let sb = new System.Text.StringBuilder()
        use tw = new System.IO.StringWriter(sb)
        System.Console.SetOut(tw)
        let args = [|"123456"|]
        ConsoleRunner.run args |> ignore
        do tw.Flush()
        let res = sb.ToString().Contains("n:[number]") &&
                  sb.ToString().Contains("path:[path]") &&
                  sb.ToString().Contains("test:[name]")
        res |> should be True


    [<Fact>]
    let ``Should parse path to assembly``() =
        let args = [@"path:C:\Dir\Path"]

        let cfg = ConfigurationHelpers.readConfigFromArgs args

        cfg.AssemblyLocation |> should equal "C:\Dir\Path"

    [<Fact>]
    let ``Should parse all test``() =
        let args = ["test:MyClass.MyMethod1"
                    "test:MyClass.MyMethod2"
                    "test:MyClass.MyMethod3"]
        
        let cfg = ConfigurationHelpers.readConfigFromArgs args
        
        let arr = cfg.TestList |> List.toArray
        cfg.TestList |> List.length |> should equal 3
        arr.[0] |> should equal "MyClass.MyMethod1"
        arr.[1] |> should equal "MyClass.MyMethod2"
        arr.[2] |> should equal "MyClass.MyMethod3"

    [<Fact>]
    let ``Should parse test name``() =
        let args = ["test:MyClass.MyMethod"]
        let cfg = ConfigurationHelpers.readConfigFromArgs args
        cfg.TestList |> List.length |> should equal 1
        cfg.TestList |> List.head |> should equal "MyClass.MyMethod"

    [<Fact>]
    let ``Should parse n in command line`` () =
        let args = ["n:1000"]
        let cfg = ConfigurationHelpers.readConfigFromArgs args 
        cfg.IterationsCount |> Option.get |> should equal 1000u

