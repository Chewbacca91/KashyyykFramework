using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;

namespace KashyyykFramework.Core.Reflection
{
    /// <summary>
    /// Classe utilitaire exposant des méthodes pour accéder aux types, méthodes, propriétés, etc d'une assembly par reflection
    /// </summary>
    internal static class AssemblyLoaderHelper
    {
        public static Assembly LoadAssembly(string assemblyFilePath)
        {
            throw new NotImplementedException();
            //return Assembly.LoadFrom(assemblyFilePath);
        }

        public static Type LoadType(Assembly assembly, string typeFullName)
        {
            return assembly.GetType(typeFullName);
        }

        public static object Instanciate(Assembly assembly, Type type)
        {
            throw new NotImplementedException();
            //object obj = assembly.CreateInstance(type.FullName, false, BindingFlags.CreateInstance, null, null, null, null);
            //return obj;
        }

        /// <summary>
        /// Renvoi la liste des types définis dans l'assembly
        /// </summary>
        /// <param name="assembly">Assembly à explorer</param>
        /// <returns>
        /// Enumeration contenant les types de l'assembly
        /// </returns>
        public static IEnumerable<Type> ListAssemblyTypes(Assembly assembly)
        {
            throw new NotImplementedException();
            //foreach (Type t in assembly.GetTypes())
            //{
            //    yield return t;
            //}
        }
    }
}
