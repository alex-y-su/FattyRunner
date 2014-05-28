namespace FattyRunner.Engine

open System
open System.Reflection

module AppDomainWrapper =
    type ITestLoaderWrapper =
        abstract member execute : string -> string -> TestResult list 
    
    type TestLoaderWrapper() =
        inherit System.MarshalByRefObject()
        interface ITestLoaderWrapper with
            member x.execute (cfgConten: string) (path: string) =
                let asm = Assembly.LoadFile(path)
                let cfg = SerializationHelper.Deserialize<EnvironmentConfiguration>(cfgConten)
                let tests = TestLoader.loadTests cfg asm |> Seq.toList
                let x = AppDomain.CurrentDomain.FriendlyName
                TestRunnerEngine.run tests cfg

    let runForAssembly (cfg: EnvironmentConfiguration) (assemblyPath: string) =
        let basePath = System.IO.Path.GetDirectoryName assemblyPath
        let x = AppDomain.CurrentDomain.FriendlyName

        let runnerAsm = Assembly.GetExecutingAssembly().Location
        let runnerDir = AppDomain.CurrentDomain.BaseDirectory
        let runnerAssembly = AppDomain.CurrentDomain;
        let setup = new AppDomainSetup(ApplicationName="FattyRunner", 
                                       ShadowCopyFiles = "true", 
                                       PrivateBinPath = runnerDir,
                                       ApplicationBase = runnerDir)

        let domain = AppDomain.CreateDomain(assemblyPath, AppDomain.CurrentDomain.Evidence, setup)
  
        let executor = domain.CreateInstanceFromAndUnwrap(runnerAsm, typeof<TestLoaderWrapper>.FullName) :?> ITestLoaderWrapper
        executor.execute (SerializationHelper.Serialize(cfg)) assemblyPath