namespace FattyRunner.Client

module ConsoleRunner = 
    val run : string array -> int 
    val runForConfig : RunConfiguration -> FattyRunner.Engine.TestResult list