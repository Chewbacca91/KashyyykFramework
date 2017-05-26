using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;
using System.IO;

namespace KashyyykFramework.Core.Reflection
{
    public class AssemblyLoader : IAssemblyLoader
    {
        // Assembly chargée par reflection
        private Assembly _assembly = null;
        public Assembly Assembly { get { return this._assembly; } }

        // Liste des types de l'assembly
        private IList<Type> _types = null;
        public IList<Type> Types { get { return this._types; } }

        // Indique si les types de l'assembly sont chargés dans la liste
        private bool _areTypesLoaded;
        public bool AreTypesLoaded { get { return this._areTypesLoaded; } }

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public AssemblyLoader()
        {
            //
        }

        /// <summary>
        /// Chargement de l'Assembly
        /// Renseigne la propriété Assembly
        /// </summary>
        /// <param name="assemblyFilePath">
        /// Chemin complet de la DLL à charger
        /// </param>
        public void LoadAssembly(string assemblyFilePath)
        {
            this._assembly = this.GetAssembly(assemblyFilePath);
        }

        /// <summary>
        /// Chargement de l'Assembly
        /// </summary>
        /// <param name="assemblyFilePath">
        /// Chemin complet de la DLL à charger
        /// </param>
        /// <returns>
        /// Renvoi l'assembly chargée
        /// </returns>
        public Assembly GetAssembly(string assemblyFilePath)
        {
            if (string.IsNullOrWhiteSpace(assemblyFilePath)) throw new ArgumentException("assemblyFilePath");
            //if (!File.Exists(assemblyFilePath)) throw new FileNotFoundException("Le fichier d'assembly n'existe pas !", assemblyFilePath);

            return AssemblyLoaderHelper.LoadAssembly(assemblyFilePath);
        }

        /// <summary>
        /// Version asynchrone de LoadAssembly
        /// </summary>
        /// <param name="assemblyFilePath"></param>
        /// <returns></returns>
        public async Task LoadAssemblyAsync(string assemblyFilePath)
        {
            await Task.Run(()=> this.LoadAssembly(assemblyFilePath));
        }

        /// <summary>
        /// Version asynchrone de GetAssembly
        /// </summary>
        /// <param name="assemblyFilePath"></param>
        /// <returns></returns>
        public async Task<Assembly> GetAssemblyAsync(string assemblyFilePath)
        {
            return await Task.Run<Assembly>(() => this.GetAssembly(assemblyFilePath));
        }

        /// <summary>
        /// Exploration des types de l'assembly
        /// Renseigne la propriétés Types
        /// </summary>
        public void ExploreAssemblyTypes()
        {
            if (this.Assembly == null) throw new NullReferenceException("LoadAssembly doit être appelée avant l'appel à ExploreAssemblyTypes");
            IEnumerable<Type> types = this.ListAssemblyTypes(this.Assembly);
            this._types = types.ToList();
            this._areTypesLoaded = true;
        }

        /// <summary>
        /// Renvoi la liste des Types contenus dans l'assembly spécifiée
        /// </summary>
        /// <param name="assembly">
        /// Assembly contenant les types
        /// </param>
        /// <returns>
        /// Liste des types définis dans l'assembly
        /// </returns>
        public IEnumerable<Type> ListAssemblyTypes(Assembly assembly)
        {
            return AssemblyLoaderHelper.ListAssemblyTypes(assembly);
        }

        /// <summary>
        /// Version asynchrone de ExploreAssemblyTypes
        /// </summary>
        /// <returns></returns>
        public async Task ExploreAssemblyTypesAsync()
        {
            await Task.Run(() => this.ExploreAssemblyTypes());
        }

        /// <summary>
        /// Version asynchrone de ListAssemblyTypes
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Type>> ListAssemblyTypesAsync(Assembly assembly)
        {
            return await Task.Run<IEnumerable<Type>>(() => this.ListAssemblyTypes(assembly));
        }

        /// <summary>
        /// Chargement d'un type depuis l'assembly chargée
        /// </summary>
        /// <param name="typeFullName">
        /// Nom du type à charger
        /// </param>
        /// <returns>
        /// Type
        /// </returns>
        /// <remarks>
        /// La propriété Assembly ne doit pas être null
        /// </remarks>
        public Type LoadType(string typeFullName)
        {
            if (this.Assembly == null) throw new NullReferenceException("LoadAssembly doit être appelée avant l'appel à LoadType");
            return this.GetType(this.Assembly, typeFullName);
        }

        /// <summary>
        /// Version asynchrone de LoadType
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public async Task<Type> LoadTypeAsync(string typeFullName)
        {
            return await Task.Run<Type>(() => this.LoadType(typeFullName));
        }

        /// <summary>
        /// Charge un type dans l'assembly spécifiée
        /// </summary>
        /// <param name="assembly">
        /// Assembly contenant le Type à charger
        /// </param>
        /// <param name="typeFullName">
        /// Nom du Type à charger
        /// </param>
        /// <returns>
        /// Type
        /// </returns>
        public Type GetType(Assembly assembly, string typeFullName)
        {
            if (assembly == null) throw new ArgumentException("assembly");
            if (string.IsNullOrWhiteSpace(typeFullName)) throw new ArgumentException("typeName");

            return AssemblyLoaderHelper.LoadType(assembly, typeFullName);
        }

        /// <summary>
        /// Version asynchrone de GetType
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public async Task<Type> GetTypeAsync(Assembly assembly, string typeFullName)
        {
            return await Task.Run<Type>(() => this.GetType(assembly, typeFullName));
        }

        /// <summary>
        /// Renvoi la liste des méthodes d'un type en fournissant le type
        /// </summary>
        /// <param name="type">
        /// Type à explorer
        /// </param>
        /// <returns>
        /// Enumération des méthodes contenues dans le type
        /// </returns>
        public IEnumerable<MethodInfo> ListMethodInfo(Type type)
        {
            throw new NotImplementedException();
            //if (type == null) throw new ArgumentNullException("type");
            //foreach (MethodInfo mi in type.GetMethods())
            //{
            //    yield return mi;
            //}
        }

        /// <summary>
        /// Renvoi la liste des méthodes d'un type en fournissant le nom complet du type
        /// Recherche dans l'assembly Chargée avec LoadAssembly
        /// </summary>
        /// <param name="type">
        /// Nom complet du type à explorer
        /// </param>
        /// <returns>
        /// Enumération des méthodes contenues dans le type
        /// </returns>
        public IEnumerable<MethodInfo> ListMethodInfo(string type)
        {
            if (string.IsNullOrWhiteSpace(type)) throw new ArgumentNullException("type");
            if (this.Assembly == null) throw new NullReferenceException("LoadAssembly doit être appelée avant l'appel à ListMethodInfo");

            Type t = this.LoadType(type);
            if (t == null) throw new Exception("Type inconnu dans l'assembly");

            return this.ListMethodInfo(t);
        }

        /// <summary>
        /// Renvoi la liste des méthodes d'un type en fournissant le nom complet du type et l'assembly contenant le type
        /// </summary>
        /// <param name="assembly">Assembly devant contenir le type pour chargement</param>
        /// <param name="type">Nom complet du type à charger et explorer</param>
        /// <returns>
        /// Enumération des méthodes contenues dans le type
        /// </returns>
        public IEnumerable<MethodInfo> ListMethodInfo(Assembly assembly, string type)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");
            if (string.IsNullOrWhiteSpace(type)) throw new ArgumentNullException("type");

            Type t = this.GetType(assembly, type);
            if (t == null) throw new Exception("Type inconnu dans l'assembly");

            return this.ListMethodInfo(t);
        }

        /// <summary>
        /// Version asynchrone renvoyant la liste des méthodes contenues dans le type
        /// </summary>
        /// <param name="type">
        /// Type à explorer
        /// </param>
        /// <returns>
        /// Enumération des méthodes contenues dans le type
        /// </returns>
        public async Task<IEnumerable<MethodInfo>> ListMethodInfoAsync(Type type)
        {
            return await Task.Run<IEnumerable<MethodInfo>>(() => this.ListMethodInfo(type));
        }

        /// <summary>
        /// Version asynchrone 
        /// Renvoi la liste des méthodes d'un type en fournissant le nom complet du type
        /// Recherche dans l'assembly Chargée avec LoadAssembly
        /// </summary>
        /// <param name="type">
        /// Nom complet du type à explorer
        /// </param>
        /// <returns>
        /// Enumération des méthodes contenues dans le type
        /// </returns>
        public async Task<IEnumerable<MethodInfo>> ListMethodInfoAsync(string type)
        {
            return await Task.Run<IEnumerable<MethodInfo>>(() => this.ListMethodInfo(type));
        }

        /// <summary>
        /// Version asynchrone 
        /// Renvoi la liste des méthodes d'un type en fournissant le nom complet du type et l'assembly contenant le type
        /// </summary>
        /// <param name="assembly">Assembly devant contenir le type pour chargement</param>
        /// <param name="type">Nom complet du type à charger et explorer</param>
        /// <returns>
        /// Enumération des méthodes contenues dans le type
        /// </returns>
        public async Task<IEnumerable<MethodInfo>> ListMethodInfoAsync(Assembly assembly, string type)
        {
            return await Task.Run<IEnumerable<MethodInfo>>(() => this.ListMethodInfo(assembly, type));
        }

        /// <summary>
        /// Instanciation d'un objet par reflection
        /// </summary>
        /// <param name="assembly">
        /// Assembly déclarant le type de l'objet
        /// </param>
        /// <param name="type">
        /// Type de l'objet
        /// </param>
        /// <returns>
        /// Objet de type instancié
        /// </returns>
        public object CreateObject(Assembly assembly, Type type)
        {
            if (assembly == null) throw new ArgumentException("assembly");
            if (type == null) throw new ArgumentException("type");

            return AssemblyLoaderHelper.Instanciate(assembly, type);
        }

        /// <summary>
        /// Version asynchrone de CreateObject
        /// </summary>
        /// <param name="assembly">
        /// Assembly déclarant le type de l'objet
        /// </param>
        /// <param name="type">
        /// Type de l'objet
        /// </param>
        /// <returns>
        /// Objet de type instancié
        /// </returns>
        public async Task<object> CreateObjectAsync(Assembly assembly, Type type)
        {
            return await Task.Run<object>(() => this.CreateObject(assembly, type));
        }
        
        /// <summary>
        /// Instanciation d'un objet par reflection
        /// </summary>
        /// <param name="type">
        /// Type à instancier
        /// </param>
        /// <returns>
        /// objet du type spécifié instancié
        /// </returns>
        public object InstanciateObject(Type type)
        {
            if (this.Assembly == null) throw new NullReferenceException("LoadAssembly doit être appelée avant l'appel à LoadType");
            if (type == null) throw new ArgumentException("type");

            return this.CreateObject(this.Assembly, type);
        }

        /// <summary>
        /// Version asynchrone de InstanciateObject
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<object> InstanciateObjectAsync(Type type)
        {
            return await Task.Run<object>(() => this.InstanciateObject(type));
        }
    }
}
