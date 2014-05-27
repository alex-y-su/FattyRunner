namespace FattyRunner.Engine

open System
open System.IO
open System.Reflection

module AppDomainWrapper =
    type TestLoaderWrapper() =
        member x.execute (cfg:EnvironmentConfiguration) (path: string) =
            let asm = Assembly.LoadFile(path)
            let tests = TestLoader.loadTests cfg asm |> Seq.toList
            TestRunnerEngine.run tests cfg

    let runForAssembly (cfg: EnvironmentConfiguration) (path: string) =
        let basePath = Path.GetDirectoryName path

        let runnerAsm = Assembly.GetExecutingAssembly().Location
        let runnerDir = AppDomain.CurrentDomain.BaseDirectory
        let runnerAssembly = AppDomain.CurrentDomain;
        let setup = new AppDomainSetup(ApplicationName="FattyRunner", 
                                       ShadowCopyFiles = "true", 
                                       PrivateBinPath = runnerDir,
                                       ApplicationBase = basePath)
        let domain = AppDomain.CreateDomain(path, AppDomain.CurrentDomain.Evidence, setup)
  
        let executor = domain.CreateInstanceFromAndUnwrap(runnerAsm, typeof<TestLoaderWrapper>.FullName) :?> TestLoaderWrapper
        executor.execute cfg path