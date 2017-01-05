using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace KashyyykFramework.Helpers
{
    public static class XmlHelper
    {
        /// <summary>
        /// Sérialisation en XML d'un objet de type T
        /// </summary>
        /// <typeparam name="T">Object ou n'importe quel type dérivant</typeparam>
        /// <param name="obj">objet à sérialiser en XML</param>
        /// <returns>
        /// Représentation XML de l'objet sous forme de chaine de caractères
        /// </returns>
        public static string SerializeXML<T>(T obj)
        {
            XmlSerializer serial = new XmlSerializer(typeof(T));
            StringBuilder sb = new StringBuilder();
            using (StringWriter sw = new StringWriter(sb))
            {
                using (XmlTextWriter xmlTW = new XmlTextWriter(sw))
                {
                    xmlTW.Formatting = Formatting.Indented;
                    xmlTW.Indentation = 8;
                    serial.Serialize(xmlTW, obj);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Désérialisation d'une chaine XML en objet le représentant
        /// </summary>
        /// <typeparam name="T">Type à désérialiser</typeparam>
        /// <param name="document">document XML représentant l'objet</param>
        public static T DeserializeXML<T>(string document)
        {
            T obj = default(T);
            using (StringReader sr = new StringReader(document))
            {
                XmlSerializer mySerializer = new XmlSerializer(typeof(T));
                obj = (T)mySerializer.Deserialize(sr);
            }
            return obj;
        }
    }
}
