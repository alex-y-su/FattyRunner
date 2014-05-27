namespace FattyRunner.Engine

open System
open System.IO
open System.Reflection

module AppDomainWrapper =
    type ITestLoaderWrapper =
        abstract member execute : EnvironmentConfiguration -> string -> TestResult list 

    type TestLoaderWrapper() =
        inherit System.MarshalByRefObject()
        interface ITestLoaderWrapper with
            member x.execute (cfg:EnvironmentConfiguration) (path: string) =
                let asm = Assembly.LoadFile(path)
                let tests = TestLoader.loadTests cfg asm |> Seq.toList
                let x = AppDomain.CurrentDomain.FriendlyName
                TestRunnerEngine.run tests cfg

    let runForAssembly (cfg: EnvironmentConfiguration) (path: string) =
        let basePath = Path.GetDirectoryName path
        let x = AppDomain.CurrentDomain.FriendlyName

        let runnerAsm = Assembly.GetExecutingAssembly().Location
        let runnerDir = AppDomain.CurrentDomain.BaseDirectory
        let runnerAssembly = AppDomain.CurrentDomain;
        let setup = new AppDomainSetup(ApplicationName="FattyRunner", 
                                       ShadowCopyFiles = "true", 
                                       PrivateBinPath = runnerDir,
                                       ApplicationBase = runnerDir)

        let domain = AppDomain.CreateDomain(path, AppDomain.CurrentDomain.Evidence, setup)
  
        let executor = domain.CreateInstanceFromAndUnwrap(runnerAsm, typeof<TestLoaderWrapper>.FullName) :?> ITestLoaderWrapper
        executor.execute cfg path