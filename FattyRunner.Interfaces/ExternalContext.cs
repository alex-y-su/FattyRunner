namespace FattyRunner.Interfaces {
    public class ExternalContext {
        public ExternalContext(uint iterationsCount,
                               uint warmUpIterations,
                               string runMethodName, 
                               IFatLogger fatLogger, 
                               object data = null) {
            this.Logger = fatLogger;
            this.Data = data;
            this.RunMethodName = runMethodName;
            this.IterationsCount = iterationsCount;
            this.WarmUpIterations = warmUpIterations;
        }

        public IFatLogger Logger { get; private set; }
        public object Data { get; set; }
        public uint IterationsCount { get; private set; }
        public string RunMethodName { get; private set; }
        public uint WarmUpIterations { get; private set; }
    }
}