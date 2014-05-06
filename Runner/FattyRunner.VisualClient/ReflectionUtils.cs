using System;
using System.Reflection;

namespace FattyRunner.VisualClient {
    public class ReflectionUtils {
        public static Assembly LoadAssemblyFromFile(string fileName) {
            try {
                AssemblyName.GetAssemblyName(fileName);
                return Assembly.LoadFile(fileName);
            }
            catch (Exception) {
                return null;
            }
        }
    }
}