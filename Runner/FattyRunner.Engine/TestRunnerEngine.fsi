namespace FattyRunner.Engine

module TestRunnerEngine = 
    val runTest : EnvironmentConfiguration -> Test -> TestResult
    val run : Test list -> EnvironmentConfiguration -> TestResult list
