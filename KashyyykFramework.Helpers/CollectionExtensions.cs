using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KashyyykFramework.Helpers
{
    /// <summary>
    /// Méthodes d'extensions ajoutant des fonctionnalités aux collections
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="array">Collection de départ</param>
        /// <param name="nbDim">Nombre de dimensions en sortie</param>
        /// <returns>
        /// Listes d'énumérables de la taille définie
        /// </returns>
        public static IEnumerable<IEnumerable<T>> SplitMultiDim<T>(this IEnumerable<T> array, int size)
        {
            for (int i = 0; i < (float)array.Count() / size; i++)
            {
                yield return array.Skip(i * size).Take(size);
            }
        }
    }
}
