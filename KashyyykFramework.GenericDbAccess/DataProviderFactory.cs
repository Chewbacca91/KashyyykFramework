using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KashyyykFramework.GenericDbAccess
{
    public sealed class DataProviderFactory
    {
        /// <summary>
        /// Obtention de l'instance DataProvider par défaut (de type SQLServer)
        /// </summary>
        /// <returns></returns>
        public static DataProviderBase GetInstance()
        {
            return GetInstance(DataProviderType.SQLSERVER);
        }

        /// <summary>
        /// Obtention de l'instance DataProvider en spécifiant le type de l'instance
        /// </summary>
        /// <param name="type">
        /// Type du provider
        /// </param>
        /// <returns>
        /// Renvoi l'implémentation concrète du provider correspondant au type
        /// </returns>
        public static DataProviderBase GetInstance(DataProviderType type)
        {
            DataProviderBase instance = null;

            switch (type)
            {
                case DataProviderType.OLEDB:
                    instance = new OleDBDataProvider();
                    break;
                case DataProviderType.SQLSERVER:
                    instance = new SqlServerDataProvider();
                    break;
                case DataProviderType.ODBC:
                    instance = new OdbcDataProvider();
                    break;
                default:
                    instance = new SqlServerDataProvider();
                    break;
            }

            return instance;
        }
    }
}
