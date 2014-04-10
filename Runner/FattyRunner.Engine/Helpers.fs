namespace FattyRunner.Engine

module ReflectionHelper = 
    open System
    open System.Reflection
    
    let contextType = typeof<FattyRunner.Interfaces.ExternalContext>

    let implements (implemented:Type) (implementor:Type) =
        implemented.IsAssignableFrom implementor

    let implementsDispose t = implements typeof<IDisposable> t

    let getDisposeReference (t: Type) =
        if implements typeof<IDisposable> t then
            Some(typeof<IDisposable>.GetMethod("Dispose"))
        else None

    let instantiate (t : Type) (cts: FattyRunner.Interfaces.ExternalContext) = 
        let parameteredCtor = t.GetConstructor([|contextType|])
        if null = parameteredCtor then Activator.CreateInstance(t, null) 
        else Activator.CreateInstance(t, [| cts :> obj |])

    let callOption instance (f:System.Reflection.MethodInfo option) =
        match f with
        | Some(f) -> f.Invoke(instance,null) |> ignore
        | _ -> ()

            