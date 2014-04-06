using System;

namespace FattyRunner.Interfaces {
    public interface IRequireContextMarker {}

    [AttributeUsage(AttributeTargets.Method)]
    public class FatTestAttribute : Attribute {}
}