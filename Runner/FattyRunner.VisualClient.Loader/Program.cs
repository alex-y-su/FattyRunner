using System;
using System.IO;
using System.Reflection;

namespace FattyRunner.VisualClient.Loader {
    internal class Program {
        [LoaderOptimization(LoaderOptimization.MultiDomainHost)]
        [STAThread]
        private static void Main() {
            var startupPath = Path.GetDirectoryName(
                Assembly
                    .GetExecutingAssembly().Location);

            var cachePath = Path.Combine(startupPath, "__cache");
            try {
                var configFile = Path.Combine(startupPath, "FattyRunner.VisualClient.exe.config");
                var assembly = Path.Combine(startupPath, "FattyRunner.VisualClient.exe");

                var setup = new AppDomainSetup {
                    ApplicationName = "FattyRunnerVisualClient",
                    ShadowCopyFiles = "true",
                    CachePath = cachePath,
                    ConfigurationFile = configFile
                };

                var domain = AppDomain.CreateDomain(
                    "FattyRunnerVisualClient",
                    AppDomain.CurrentDomain.Evidence,
                    setup);

                domain.ExecuteAssembly(assembly);

                AppDomain.Unload(domain);
            }
            finally {
                Directory.Delete(cachePath, true);
            }
        }
    }
}