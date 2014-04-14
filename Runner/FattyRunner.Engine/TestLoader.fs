namespace FattyRunner.Engine

module TestLoader =
    open System.Reflection
    open ReflectionHelper

    let markedByAttr t (m:MethodInfo) =
            m.CustomAttributes |> Seq.exists (fun a -> a.AttributeType = t)

    let fatAttrType = typeof<FattyRunner.Interfaces.FatTestAttribute>
    let isFattyMethod = markedByAttr fatAttrType

    let getTestConfiguration (t:TypeInfo) (m:MethodInfo) =
        let attr = m.GetCustomAttribute(fatAttrType) 
                   :?> FattyRunner.Interfaces.FatTestAttribute 
        
        { Count = attr.MaxIterations
          WarmUp = attr.WarmUpIterations
          ProgressiveStep = attr.Step 
          Data = if null=attr.Data then None else Some(attr.Data) }:TestConfiguration

    let createTestRference t m =
        { Type    = t:>System.Type
          Run     = m }:TestReference
    
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
            let createTest' m =
                let c = getTestConfiguration t m
                { Reference = createTestRference t m
                  Configuration = mergeConfigs c cfg }:Test                
            methods |> Seq.map createTest'
        
        types |> Seq.map createTest |> Seq.concat