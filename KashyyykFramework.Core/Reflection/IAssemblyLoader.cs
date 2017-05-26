using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;

namespace KashyyykFramework.Core.Reflection
{
    public interface IAssemblyLoader
    {
        Assembly Assembly { get; }
        IList<Type> Types { get; }
        bool AreTypesLoaded { get; }

        void LoadAssembly(string assemblyFilePath);
        Assembly GetAssembly(string assemblyFilePath);
        Task LoadAssemblyAsync(string assemblyFilePath);
        Task<Assembly> GetAssemblyAsync(string assemblyFilePath);
        
        void ExploreAssemblyTypes();
        IEnumerable<Type> ListAssemblyTypes(Assembly assembly);
        Task ExploreAssemblyTypesAsync();
        Task<IEnumerable<Type>> ListAssemblyTypesAsync(Assembly assembly);
 
        Type LoadType(string typeFullName);
        Type GetType(Assembly assembly, string typeFullName);
        Task<Type> LoadTypeAsync(string typeFullName);
        Task<Type> GetTypeAsync(Assembly assembly, string typeFullName);

        IEnumerable<MethodInfo> ListMethodInfo(Type type);
        IEnumerable<MethodInfo> ListMethodInfo(string type);
        IEnumerable<MethodInfo> ListMethodInfo(Assembly assembly, string type);
        Task<IEnumerable<MethodInfo>> ListMethodInfoAsync(Type type);
        Task<IEnumerable<MethodInfo>> ListMethodInfoAsync(string type);
        Task<IEnumerable<MethodInfo>> ListMethodInfoAsync(Assembly assembly, string type);
        
        object CreateObject(Assembly assembly, Type type);
        object InstanciateObject(Type type);
        Task<object> CreateObjectAsync(Assembly assembly, Type type);
        Task<object> InstanciateObjectAsync(Type type);
    }
}
