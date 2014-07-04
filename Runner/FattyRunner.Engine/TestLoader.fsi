namespace FattyRunner.Engine

module TestLoader =
    val loadMultistepTestRef : System.Reflection.Assembly -> TestDefenition -> TestReference
    val findTestDefenitions : System.Reflection.Assembly -> TestDefenition list
    val loadMultistepTest : EnvironmentConfiguration -> TestReference -> Test