using System;

namespace KashyyykFramework.Mobile.ComponentModel
{
    /// <summary>
    /// Attribut spécifiant le Type contenant les libellés localisés correspondant aux valeurs d'une enum
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum)]
    public class LocalizedEnumResourceTypeAttribute : Attribute
    {
        private Type _resourceManagerType = null;

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="resourceType">
        /// Indique le type de resource dans lequel les valeurs sont localisées
        /// </param>
        public LocalizedEnumResourceTypeAttribute(Type resourceType)
        {
            this._resourceManagerType = resourceType;
        }

        /// <summary>
        /// Type à instancier par le resource Manager souhaitant accèder aux libellés localisés de l'enum
        /// </summary>
        public Type ResourceManagerType
        {
            get
            {
                return this._resourceManagerType;
            }
        }
    }
}
