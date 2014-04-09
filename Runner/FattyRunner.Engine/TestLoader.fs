namespace FattyRunner.Engine

module TestLoader =
    open System.Reflection

    let markedByAttr t (m:MethodInfo) =
            m.CustomAttributes |> Seq.exists (fun a -> a.AttributeType = t)

    let fatAttrType = typeof<FattyRunner.Interfaces.FatTestAttribute>
    let isFattyMethod = markedByAttr fatAttrType
    let isDisposeMethod = markedByAttr typeof<FattyRunner.Interfaces.FatCleanupAttribute>
    let isInitMethod = markedByAttr typeof<FattyRunner.Interfaces.FatInitAttribute>

    let getTestConfiguration (t:TypeInfo) (m:MethodInfo) =
        let attr = m.GetCustomAttribute(fatAttrType) 
                   :?> FattyRunner.Interfaces.FatTestAttribute 
        
        { Count = attr.MaxIterations
          WarmUp = attr.WarmUpIterations
          ProgressiveStep = attr.Step }:TestConfiguration

    let createTestRference t m init desp =
        { Type    = t:>System.Type
          Run     = m
          Init    = init
          Dispose = desp }:TestReference
    
    let mergeConfigs (c: TestConfiguration) (g:EnvironmentConfiguration) =
        match g.Count with
        | Some(n) -> { c with Count = n }
        | _ -> c

    let tryLoadAssembly (s:string) =
        try 
            let testAssembly = System.Reflection.AssemblyName.GetAssemblyName(s);
            System.Reflection.Assembly.LoadFile(s) |> Some
        with
        | :? System.IO.FileNotFoundException -> None
        | :? System.BadImageFormatException -> None
        | :? System.IO.FileLoadException -> None

    let loadAllAssembliesFromDirectory (dir:string) =
        let dir = System.AppDomain.CurrentDomain.BaseDirectory
        let fileRecords = System.IO.Directory.GetFiles(dir,"*.dll")
        fileRecords |> Seq.map tryLoadAssembly
                    |> Seq.filter Option.isSome
                    |> Seq.map Option.get

    let loadAssemblyFromFile = tryLoadAssembly

    let loadTests (cfg:EnvironmentConfiguration) (asm: Assembly) = 
        let types = query { for x in asm.DefinedTypes do
                            where (x.DeclaredMethods |> Seq.exists isFattyMethod)
                            select x }
        
        let createTest (t: TypeInfo) =
            let methods = t.DeclaredMethods |> Seq.filter isFattyMethod
            let init = t.DeclaredMethods |> Seq.tryFind isInitMethod
            let disp = t.DeclaredMethods |> Seq.tryFind isDisposeMethod
            let createTest' m =
                let c = getTestConfiguration t m
                { Reference = createTestRference t m init disp
                  Configuration = mergeConfigs c cfg }:Test                
            methods |> Seq.map createTest'
        
        types |> Seq.map createTest |> Seq.concat