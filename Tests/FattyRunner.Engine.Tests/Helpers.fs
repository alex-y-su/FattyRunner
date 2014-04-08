namespace FattyRunner.Engine.Tests

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
          Run = m
          Init = None
          Dispose = None } : TestReference
