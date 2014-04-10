namespace FattyRunner.Engine.Tests

type EmptyLogger() =
    interface FattyRunner.Interfaces.IFatLogger with
        member this.Write (_,__) = ()
    static member Instance
        with get() = new EmptyLogger()

module ReflectiponHelpers = 
    open System
    open System.Reflection
    open Microsoft.FSharp.Metadata
    
    let getMethodReference (t : Type) name = t.GetMethod(name)

module TestHelpers = 
    open FattyRunner.Engine
    open ReflectiponHelpers

    let createTestRef (t : System.Type) mName = 
        let m = getMethodReference t mName
        { Type = t
          Run = m } : TestReference
