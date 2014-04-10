using System;

namespace FattyRunner.Interfaces {
    [AttributeUsage(AttributeTargets.Method)]
    public class FatAfterWarmupAttribute : Attribute { }
}