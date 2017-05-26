using System;

using System.Reflection;
using System.Resources;

using KashyyykFramework.Mobile.ComponentModel;

namespace KashyyykFramework.Mobile.Helpers
{
    /// <summary>
    /// Méthodes d'extensions utilitaires pour les enums
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Obtient le libellé localisé correspondant à la valeur si existant
        /// Pour accéder au(x) libellé(s) :
        /// - Déclarer un attribut LocalizedEnumResourceType au niveau de l'enum, spécifiant la classe (resx designer) contenant les libellés
        /// - Les clés de ressources dans le resx doivent avoir le format suivant [EnumTypeToString]_[EnumValToString]
        /// - Si non trouvé, renvoi la valeur de l'enum sous forme de chaine de caractères
        /// </summary>
        /// <param name="enumValue">
        /// Valeur de l'énum
        /// </param>
        /// <returns>
        /// Libellé localisé de la valeur
        /// </returns>
        public static string GetLocalizedLib(this Enum enumValue)
        {
            // Info sur la valeur de l'enum
            FieldInfo fi = enumValue.GetType().GetRuntimeField(enumValue.ToString());
            // Récupère l'attribut fournissant le resource type du resource Manager à instancier si existant
            LocalizedEnumResourceTypeAttribute attribute = fi.GetCustomAttribute(typeof(LocalizedEnumResourceTypeAttribute), false) as LocalizedEnumResourceTypeAttribute;
            
            // Si pas d'attribut on renvoi la valeur.ToString()
            if (attribute == null) return enumValue.ToString();

            // Sinon on tente de récupérer la valeur en fichier de resource
            ResourceManager resourceMgr = new ResourceManager(attribute.ResourceManagerType);
            if (resourceMgr == null) return enumValue.ToString();

            string lib = resourceMgr.GetString(string.Format("{0}_{1}", enumValue.GetType().Name, fi.ToString()));
            if (lib == null) return enumValue.ToString();

            return lib;
        }
    }
}
