namespace FattyRunner.Engine

module TestLoader =
    val loadMultistepTestRef : System.Reflection.Assembly -> TestDefenition -> TestReference
    val findMultistepTests : System.Reflection.Assembly -> TestDefenition list
    val loadMultistepTest : EnvironmentConfiguration -> TestReference -> MultistepTest