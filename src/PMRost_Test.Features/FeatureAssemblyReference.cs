

using System.Reflection;

namespace PMRost_Test.Features;

public sealed class FeatureAssemblyReference
{
    public static Assembly Assembly => typeof(FeatureAssemblyReference).Assembly;
}
