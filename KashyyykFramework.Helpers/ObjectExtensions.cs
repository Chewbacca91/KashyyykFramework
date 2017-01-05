using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;

namespace KashyyykFramework.Helpers
{
    /// <summary>
    /// Extensions pour object
    /// </summary>
    public static class ObjectExtensions
    {
        #region Reflection

        /// <summary>
        /// Renvoi une chaine de caractères représentant l'object avec ses propriétés et leurs valeurs sous forme de string
        /// </summary>
        /// <typeparam name="T">Object ou n'importe quel type dérivant</typeparam>
        /// <param name="obj">objet pour lequel obtenir les informations propriétés / valeurs</param>
        /// <returns>
        /// Chaine contenant les paires propriétés/valeurs sous forme : 
        /// {PropName} = {PropValue.ToString()}
        /// </returns>
        public static string ToStringPropsValues<T>(this T obj)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var entry in GetPropertiesWithValues(obj))
            {
                sb.AppendLine(string.Format("{0} = {1}", entry.Key, (entry.Value == null ? "NULL" : entry.Value.ToString())));
            }
            return sb.ToString();
        }

        /// <summary>
        /// Renvoi un Dictionary contetant la liste des propriétés d'un objet sous forme PropName (clé) / PropValue (value)
        /// </summary>
        /// <typeparam name="T">Object ou n'importe quel type dérivant</typeparam>
        /// <param name="obj">objet pour lequel obtenir la liste des propriétés avec valeurs</param>
        /// <returns>
        /// Dictionary Clé PropName (String), Valeur PropValue (Object)
        /// </returns>
        public static IDictionary<string, object> GetPropertiesWithValues<T>(this T obj)
        {
            IDictionary<string, object> dic = new Dictionary<string, object>();
            if (obj == null) return dic;

            foreach (PropertyInfo prop in obj.GetType().GetProperties())
            {
                dic.Add(prop.Name, prop.GetValue(obj, null));
            }

            return dic;
        }

        #endregion        
    }
}
