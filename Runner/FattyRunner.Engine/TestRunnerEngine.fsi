namespace FattyRunner.Engine

module TestRunnerEngine = 
    val runTest : EnvironmentConfiguration -> Test -> TestResult
    val runTests : EnvironmentConfiguration -> Test list -> TestResult list
