namespace FattyRunner.Engine

module TestLoader = 
    open System.Reflection
    open ReflectionHelper

    let markedByAttr t (m:MethodInfo) =
            m.CustomAttributes |> Seq.exists (fun a -> a.AttributeType = t)
    
    let fatAfterWarm = typeof<FattyRunner.Interfaces.FatAfterWarmupAttribute>
    let isAfterWarmMethod = markedByAttr fatAfterWarm

    let fatAttrType = typeof<FattyRunner.Interfaces.FatTestAttribute>
    let isFattyMethod = markedByAttr fatAttrType

    let createTestRference t m afterWarm =
        { Type    = t:>System.Type
          Run     = m 
          AfterWarmUp = afterWarm}: TestReference

    let getTypes (asm: Assembly) (typesToRun: string list) =
        let filter (t: System.Type) =
            typesToRun |> List.exists (fun x -> x = t.FullName)
        asm.DefinedTypes |> Seq.filter filter

    let getTestConfiguration (tr: TestReference) =
        let attr = tr.Run.GetCustomAttribute(fatAttrType) 
                   :?> FattyRunner.Interfaces.FatTestAttribute 
        
        { Count = attr.MaxIterations
          WarmUp = attr.WarmUpIterations
          ProgressiveStep = attr.Step 
          Data = if null=attr.Data then None else Some(attr.Data) }: MultistepTestConfiguration

    let mergeConfigs (c: MultistepTestConfiguration) (g:EnvironmentConfiguration) =
        match g.Count with
        | Some(n) -> { c with Count = n }
        | _ -> c

    let loadMultistepTest (globCfg: EnvironmentConfiguration) (testRef: TestReference) =
        let localCfg = getTestConfiguration testRef
        let cfg = mergeConfigs localCfg globCfg
        { Reference = testRef; Configuration = cfg } : MultistepTest

    let loadMultistepTestRef (asm: System.Reflection.Assembly) (testDef: TestDefenition) =
        let t = asm.DefinedTypes |> Seq.find (fun x -> x.FullName = testDef.TypeName)
        let m = t.GetMethod testDef.TestName
        let afterWarm = t.DeclaredMethods |> Seq.tryFind isAfterWarmMethod
        { Type = t; Run = m; AfterWarmUp = afterWarm } : TestReference

    let findMultistepTests (asm: System.Reflection.Assembly) : TestDefenition list =
        let flattedMethods = seq { for t in asm.DefinedTypes do
                                       for m in t.DeclaredMethods do
                                           yield m }
        let fattyMethods = flattedMethods |> Seq.filter isFattyMethod

        let getDef (m: System.Reflection.MethodInfo) =
            { TestName = m.Name
              TypeName = m.DeclaringType.FullName
              AssemblyName = m.DeclaringType.Assembly.FullName } : TestDefenition
        
        fattyMethods |> Seq.map getDef |> List.ofSeq
        
//        query { for t in asm.DefinedTypes do
//                                  for m in t.DeclaredMethods do
//                                  where isFattyMethod m 
//                                  select m)
//        
//        let getTypes (asm: Assembly) (typesToRun: string list) =
//            let filter (t: System.Type) =
//                typesToRun |> List.exists (fun x -> x = t.FullName)
//            asm.DefinedTypes |> Seq.filter filter
//
//        let loadAllAsmTests (asm: Assembly) =
//            query { for x in asm.DefinedTypes do
//                    where (x.DeclaredMethods |> Seq.exists isFattyMethod)
//                    select x }

(*
    open System.Reflection
    open ReflectionHelper

    let markedByAttr t (m:MethodInfo) =
            m.CustomAttributes |> Seq.exists (fun a -> a.AttributeType = t)

    let fatAfterWarm = typeof<FattyRunner.Interfaces.FatAfterWarmupAttribute>
    let isAfterWarmMethod = markedByAttr fatAfterWarm
    
    let fatAttrType = typeof<FattyRunner.Interfaces.FatTestAttribute>
    let isFattyMethod = markedByAttr fatAttrType

    let getTestConfiguration (t:TypeInfo) (m:MethodInfo) =
        let attr = m.GetCustomAttribute(fatAttrType) 
                   :?> FattyRunner.Interfaces.FatTestAttribute 
        
        { Count = attr.MaxIterations
          WarmUp = attr.WarmUpIterations
          ProgressiveStep = attr.Step 
          Data = if null=attr.Data then None else Some(attr.Data) }:TestConfiguration

    let createTestRference t m afterWarm =
        { Type    = t:>System.Type
          Run     = m 
          AfterWarmUp = afterWarm}:TestReference
    
    let mergeConfigs (c: TestConfiguration) (g:EnvironmentConfiguration) =
        match g.Count with
        | Some(n) -> { c with Count = n }
        | _ -> c

    let getTypes (asm: Assembly) (typesToRun: string list) =
        let filter (t: System.Type) =
            typesToRun |> List.exists (fun x -> x = t.FullName)
        asm.DefinedTypes |> Seq.filter filter

    let loadAllAsmTests (asm: Assembly) =
        query { for x in asm.DefinedTypes do
                where (x.DeclaredMethods |> Seq.exists isFattyMethod)
                select x }


    let loadTests (cfg:EnvironmentConfiguration) (asm: Assembly) (testsToRun: string list) = 
    *)
//        let types =
//            match testsToRun with
//            | [] -> loadAllAsmTests asm
//            | x -> 
        
        //| lst -> lst |> List.exists (fun x -> x = sprintf "%s.%s" s.DeclaringType.Name s.Name)

        (*
        let createTest (t: TypeInfo) =
            let methods = t.DeclaredMethods |> Seq.filter isFattyMethod
            let afterWarm = t.DeclaredMethods |> Seq.tryFind isAfterWarmMethod
            let createTest' m =
                let c = getTestConfiguration t m
                { Reference = createTestRference t m afterWarm
                  Configuration = mergeConfigs c cfg }:Test                
            methods |> Seq.filter toBeRun |> Seq.map createTest'
        
        types |> Seq.map createTest |> Seq.concat*)