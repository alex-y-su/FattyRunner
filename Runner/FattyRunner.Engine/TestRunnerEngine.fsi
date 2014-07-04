namespace FattyRunner.Engine

module TestRunnerEngine = 
    val runTest : EnvironmentConfiguration -> MultistepTest -> TestResult
    val runMultistepTests : MultistepTest list -> EnvironmentConfiguration -> TestResult list
