using System;
using System.IO;
using System.Reflection;

namespace PilotLookUp.Utils
{
    internal static class Resolver
    {
        internal static Assembly ResolveAssembly(object sender, ResolveEventArgs args)
        {
            string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string assemblyName = new AssemblyName(args.Name).Name + ".dll";
            string fullPath = Path.Combine(assemblyPath, assemblyName);

            if (File.Exists(fullPath))
            {
                return Assembly.LoadFrom(fullPath);
            }
            return null;
        }
    }
}
