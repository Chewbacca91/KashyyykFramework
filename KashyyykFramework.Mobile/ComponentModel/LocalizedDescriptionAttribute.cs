using System;
using System.Resources;

namespace KashyyykFramework.Mobile.ComponentModel
{
    /// <summary>
    /// Attribut permettant de spécifier une description localisée en fichier de ressource
    /// </summary>
    public class LocalizedDescriptionAttribute : DescriptionAttribute
    {
        private readonly string _resourceKey;
        private readonly ResourceManager _resource;

        /// <summary>
        /// Constructeur - Initialise le ResourceManager
        /// </summary>
        /// <param name="resourceKey">Clé en fichier de ressources</param>
        /// <param name="resourceType">Type pour instancier le ResourceManager</param>
        public LocalizedDescriptionAttribute(string resourceKey, Type resourceType) 
            : base(null)
        {
            this._resource = new ResourceManager(resourceType);
            this._resourceKey = resourceKey;
        }

        /// <summary>
        /// Obtient la description -> Texte correspondant à la clé de ressource fourni pour le type de ressource
        /// </summary>
        public override string Description
        {
            get
            {
                string description = this._resource.GetString(_resourceKey);
                return string.IsNullOrWhiteSpace(description) ? string.Format("[[{0}]]", _resourceKey) : description;
            }
        }
    }
}
