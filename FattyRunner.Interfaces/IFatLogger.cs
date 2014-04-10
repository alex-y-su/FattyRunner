namespace FattyRunner.Interfaces {
    public interface IFatLogger {
        void Write(string format, params object[] prms);
    }
}