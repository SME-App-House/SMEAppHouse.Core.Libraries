using System;
using System.IO;
using System.Reflection;

namespace SMEAppHouse.Core.CodeKits.Helpers
{
    public static class AssemblyHelper
    {
        /// <summary>
        /// 
        /// </summary>
        public static string AppLocation
        {
            get
            {
                var module = typeof(CodeKit).Assembly.GetModules()[0].FullyQualifiedName;
                var directory = Path.GetDirectoryName(module);
                return directory;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string GetVersionInfo(Assembly assembly = null)
        {
            if(assembly == null) assembly = Assembly.GetExecutingAssembly();
            var version = assembly.FullName.Split(',')[1];
            var fullversion = version.Split('=')[1];
            return fullversion;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetAssemblyAttribute<T>(Assembly assembly, Func<T, string> value) where T : Attribute
        {
            var attribute = (T)Attribute.GetCustomAttribute(assembly, typeof(T));
            return value.Invoke(attribute);
        }
    }
}
