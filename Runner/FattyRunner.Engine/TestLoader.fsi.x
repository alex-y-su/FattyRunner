namespace FattyRunner.Engine

module TestLoader =
    val loadTests : EnvironmentConfiguration -> System.Reflection.Assembly -> string list -> Test seq