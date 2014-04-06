namespace FattyRunner.Engine

module ReflectionHelper = 
    open System
    open System.Reflection
    
    let getAttributeInstance (m:MethodInfo) (attrType: CustomAttributeData) =
        ()

    let instantiate (t : Type) (cts: Map<string,obj> option) = 
        match cts with
        | Some(cts) -> 
            Activator.CreateInstance(t, [| cts :> obj |])
        | None -> 
            Activator.CreateInstance(t, null)
