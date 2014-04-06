using System;

namespace FattyRunner.Interfaces {
    public interface IRequireContextMarker {}

    [AttributeUsage(AttributeTargets.Method)]
    public class FatInitAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method)]
    public class FatCleanupAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method)]
    public class FatTestAttribute : Attribute {}
}