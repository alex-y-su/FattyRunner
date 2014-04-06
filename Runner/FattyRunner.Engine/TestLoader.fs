namespace FattyRunner.Engine

module TestLoader =
    open System.Reflection

    let markedByAttr t (m:MethodInfo) =
            m.CustomAttributes |> Seq.exists (fun a -> a.AttributeType = t)

    let isFattyMethod = markedByAttr typeof<FattyRunner.Interfaces.FatTestAttribute>
    let isDisposeMethod = markedByAttr typeof<FattyRunner.Interfaces.FatCleanupAttribute>
    let isInitMethod = markedByAttr typeof<FattyRunner.Interfaces.FatInitAttribute>

    let getTestConfiguration (t:TypeInfo) (m:MethodInfo) =
         { Count = 100u
           WarmUp = 10u
           ProgressiveStep = 1u }:TestConfiguration

    let createTestRference t m init desp =
        { Type    = t:>System.Type
          Run     = m
          Init    = init
          Dispose = desp }:TestReference
    
    let mergeConfigs c (g:EnvironmentConfiguration) =
        c

    let load (asm: Assembly) (cfg:EnvironmentConfiguration) = 
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

