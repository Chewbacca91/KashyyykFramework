using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Runtime.Serialization.Json;

namespace KashyyykFramework.Mobile.Helpers
{
    public static class JsonHelper
    {
        /// <summary>
        /// Sérialisation d'un objet en JSON
        /// </summary>
        /// <typeparam name="T">Type de l'objet</typeparam>
        /// <param name="obj">objet à sérialiser</param>
        /// <returns>
        /// Chaine représentant l'objet en JSON
        /// </returns>
        public static string SerializeJson<T>(T obj)
        {
            DataContractJsonSerializer jsonSerializer = CreateDataContractJsonSerializer(obj);
            byte[] streamArray = null;
            using (var memoryStream = new MemoryStream())
            {
                jsonSerializer.WriteObject(memoryStream, obj);
                streamArray = memoryStream.ToArray();
            }
            string json = Encoding.UTF8.GetString(streamArray, 0, streamArray.Length);
            return json;
        }

        /// <summary>
        /// Désérialisation d'un objet JSON
        /// </summary>
        /// <typeparam name="T">Type de l'objet</typeparam>
        /// <param name="obj">objet dans lequel sérialiser</param>
        /// <param name="json">chaine json contenant les données de l'objet</param>
        public static T DeserializeJson<T>(string json)
        {
            T obj = default(T);
            if (string.IsNullOrWhiteSpace(json)) return obj;
            DataContractJsonSerializer jsonSerializer = CreateDataContractJsonSerializer(obj);
            using (Stream s = json.ToStream())
            {
                obj = (T)jsonSerializer.ReadObject(s);
            }
            return obj;
        }

        /// <summary>
        /// Méthode de création d'un jSon serializer pour le type d'objet spécifié
        /// Settings peut etre spécifié
        /// </summary>
        /// <typeparam name="T">Type d'objet pour lequel instancier le sérializer</typeparam>
        /// <param name="type">Type d'objet pour lequel instancier le sérializer</param>
        /// <param name="settings">Settings (optionels)</param>
        /// <returns>
        /// DataContractJsonSerializer instancié
        /// </returns>
        private static DataContractJsonSerializer CreateDataContractJsonSerializer<T>(T type, DataContractJsonSerializerSettings settings = null)
        {
            if (settings != null) return new DataContractJsonSerializer(typeof(T), settings);
            return new DataContractJsonSerializer(typeof(T));
        }

    }
}
