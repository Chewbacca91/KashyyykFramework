using System;

namespace KashyyykFramework.Mobile.ComponentModel
{
    /// <summary>
    /// Attribut décrivant un élément
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class DescriptionAttribute : Attribute
    {
        private string _description = null;

        /// <summary>
        /// Constructeur inialisant la description de l'élément
        /// </summary>
        /// <param name="description">
        /// Descpription à appliquer à l'élément
        /// </param>
        public DescriptionAttribute(string description)
        {
            this._description = description;
        }

        /// <summary>
        /// Propriété permettant d'accéder à la description
        /// </summary>
        public virtual string Description
        {
            get { return this._description; }
        }
    }
}
