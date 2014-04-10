namespace FattyRunner.Interfaces {
    public class ExternalContext {
        public ExternalContext(uint iterationsCount, string runMethodName, IFatLogger fatLogger) {
            this.Logger = fatLogger;
            this.RunMethodName = runMethodName;
            this.IterationsCount = iterationsCount;
        }

        public IFatLogger Logger { get; private set; }
        public uint IterationsCount { get; private set; }
        public string RunMethodName { get; private set; }
    }
}