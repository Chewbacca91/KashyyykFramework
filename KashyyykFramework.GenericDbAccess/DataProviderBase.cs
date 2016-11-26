using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KashyyykFramework.GenericDbAccess
{
    /// <summary>
    /// Provider d'accès aux données abstrait
    /// Fournit l'interface commune aux classes d'accès aux données
    /// Est un IDataProvider 
    /// V0 Test GitHub
    /// </summary>
    public abstract class DataProviderBase : IDataProvider
    {
        /// <summary>
        /// Timeout pour l'exécution des requêtes en secondes
        /// 0 = pas de timeout
        /// Valeur par défaut 30 secondes
        /// </summary>
        private int _timeout = 30;

        #region IDataProvider

        #region Méthodes abtraites implémentées dans les classes concrètes
        /// <summary>
        /// Obient le Data Adapteur concrèt correspondant à l'implémentation du provider
        /// </summary>
        /// <param name="command">Command abstraite</param>
        /// <returns>
        /// DataAdapter instancié en fonction du provider
        /// </returns>
        public abstract IDbDataAdapter GetAdapter(IDbCommand command);
        /// <summary>
        /// Obtient la connexion concrète à la base de données correspondante à l'implémentation du provider
        /// </summary>
        /// <param name="connectionString">Chaine de connexion à la base de données</param>
        /// <returns>
        /// Connection instanciée en fonction du provider
        /// </returns>
        public abstract IDbConnection GetConnection(string connectionString);
        /// <summary>
        /// Obtient un paramètre DB de type concrèt correspondant à l'implémentation du provider
        /// (paramètre à fournir à un IDBCommand pour exécution d'une requête ou d'une procédure stockée
        /// </summary>
        /// <param name="name">Nom du paramètre</param>
        /// <param name="value">Valeur du paramètre</param>
        /// <returns>
        /// Data Parameter instancié en fonction du provider
        /// </returns>
        public abstract IDbDataParameter GetParam(string name, object value);

        #endregion

        #region Méthodes Communes implémentants l'interface IDataProvider

        /// <summary>
        /// Propriété en lecture seule pour connaitre le timeout défini
        /// Valeur par défaut 30 secondes
        /// </summary>
        public int Timeout => this._timeout;
        /// <summary>
        /// Affectation d'un Timeout différent du timeout par défaut
        /// 0 = pas de timeout
        /// </summary>
        /// <param name="timeout">
        /// Timeout en secondes
        /// </param>
        public void SetTimeout(int timeout) => this._timeout = timeout;

        /// <summary>
        /// Obtention d'une liste de paramètres DB à partir d'un IDictionary
        /// Renvoi les paramètres de type concrèt correspondant à l'implémentation du provider
        /// </summary>
        /// <param name="values">
        /// Liste des paramètres clés/valeurs => name et value des paramètres DB
        /// </param>
        /// <returns>
        /// Array des paramètres instanciés en fonction du type du provider
        /// </returns>
        public IDbDataParameter[] GetParams(IDictionary<string, object> values)
        {
            IList<IDbDataParameter> res = new List<IDbDataParameter>();
            foreach (KeyValuePair<string, object> kvp in values)
            {
                res.Add(this.GetParam(kvp.Key, kvp.Value));
            }
            return res.ToArray();
        }

        #region DataSets
        /// <summary>
        /// Renvoi un DataSet résultant de l'exécution de la requête paramétrée dans l'appel
        /// </summary>
        /// <param name="connectionString">Chaine de connexion à la base de données</param>
        /// <param name="query">Corps de la requête à exécuter</param>
        /// <param name="paramIn">
        /// Liste des valeurs au format Texte à fournir à la requête
        /// Effecute un string.Format(query, paramIn)
        /// </param>
        /// <returns>
        /// DataSet chargé avec les données
        /// </returns>
        public DataSet ExecuteDataSet(string connectionString, string query, object[] paramIn)
        {
            try
            {
                DataSet Ds = new DataSet();
                using (IDbConnection connection = this.GetConnection(connectionString))
                {
                    using (IDbCommand command = connection.CreateCommand())
                    {
                        // Ajout des paramètres
                        if (paramIn != null) query = string.Format(query, paramIn);
                        // Configuration de la command
                        this.ConfigureCommand(command, null, CommandType.Text, query, null, null, null);
                        // Adapter gère le Connection.Open et Close.
                        IDataAdapter adapter = this.GetAdapter(command);
                        adapter.Fill(Ds);
                    }
                }
                return Ds;
            }
            catch (Exception ex)
            {
                throw new DataProviderException(ex.Message, ex);
            }
        }
        /// <summary>
        /// Renvoi un DataSet résultant de l'exécution de la requête paramétrée dans l'appel
        /// </summary>
        /// <param name="connectionString">Chaine de connexion à la base de données</param>
        /// <param name="query">Corps de la requête à exécuter</param>
        /// <param name="paramIn">Liste des paramètres nommés de la requête</param>
        /// <returns>
        /// DataSet chargé avec les données
        /// </returns>
        public DataSet ExecuteDataSet(string connectionString, string query, IDbDataParameter[] paramIn)
        {
            try
            {
                DataSet Ds = new DataSet();
                using (IDbConnection connection = this.GetConnection(connectionString))
                {
                    using (IDbCommand command = connection.CreateCommand())
                    {
                        // Configuration de la command
                        this.ConfigureCommand(command, null, CommandType.Text, query, paramIn, null, null);
                        // Adapter gère le Connection.Open et Close.
                        IDataAdapter adapter = this.GetAdapter(command);
                        adapter.Fill(Ds);
                    }
                }
                return Ds;
            }
            catch (Exception ex)
            {
                throw new DataProviderException(ex.Message, ex);
            }
        }
        /// <summary>
        /// Renvoi un DataSet résultant de l'exécution de la procédure paramétrée dans l'appel
        /// </summary>
        /// <param name="connectionString">Chaine de connexion à la base de données</param>
        /// <param name="procedureName">Nom de la procédure stockée</param>
        /// <param name="paramIn">Liste des paramètres de la procédure stockée en entrée</param>
        /// <param name="paramOut">Liste des paramètres de sortie de la procédure stockée</param>
        /// <param name="paramReturn">Paramètre de retour de la procédure</param>
        /// <returns>
        /// DataSet chargé avec les données
        /// </returns>
        public DataSet ExecuteDataSetProcedure(string connectionString, string procedureName, IDbDataParameter[] paramIn, IDbDataParameter[] paramOut, IDbDataParameter paramReturn)
        {
            try
            {
                DataSet Ds = new DataSet();
                using (IDbConnection connection = this.GetConnection(connectionString))
                {
                    using (IDbCommand command = connection.CreateCommand())
                    {
                        // Configuration de la commande
                        this.ConfigureCommand(command, null, CommandType.StoredProcedure, procedureName, paramIn, paramOut, paramReturn);
                        // Adapter gère le Connection.Open et Close.
                        IDataAdapter adapter = this.GetAdapter(command);
                        adapter.Fill(Ds);
                    }
                }
                return Ds;
            }
            catch (Exception ex)
            {
                throw new DataProviderException(ex.Message, ex);
            }
        }
        #endregion

        #region DataTables
        /// <summary>
        /// Retourne la DataTable résultant de l'exécution du IDBCommand en paramètre
        /// </summary>
        /// <param name="command">
        /// IDBCommand renseigné avec tous les paramètres pour exécution en base
        /// </param>
        /// <returns>
        /// DataTable construite à partir du DataReader résultant de l'exécution de la command
        /// </returns>
        public DataTable GetDataTable(IDbCommand command)
        {
            try
            {
                DataTable table = new DataTable();
                using (IDataReader reader = command.ExecuteReader())
                {
                    DataTable tableSchema = reader.GetSchemaTable();
                    foreach (DataRow row in tableSchema.Rows)
                    {
                        table.Columns.Add(row["ColumnName"].ToString(), Type.GetType(row["DataType"].ToString()));
                    }
                    while (reader.Read())
                    {
                        DataRow row = table.NewRow();
                        for (int i = 0; i < table.Columns.Count; i++)
                        {
                            row[i] = reader.GetValue(i);
                        }
                        table.Rows.Add(row);
                    }
                }
                return table;
            }
            catch (Exception ex)
            {
                throw new DataProviderException(ex.Message, ex);
            }
        }
        /// <summary>
        /// Renvoi la DataTable résultante de l'exécution de la requête paramètrée spécifiée
        /// </summary>
        /// <param name="connectionString">Chaine de connexion à la base de données</param>
        /// <param name="query">Corps de la requête à exécuter</param>
        /// <param name="paramIn">
        /// Liste des valeurs au format Texte à fournir à la requête
        /// Effecute un string.Format(query, paramIn)
        /// </param>
        /// <returns>
        /// DataTable contenant les données fournies par la requête
        /// </returns>
        public DataTable ExecuteDataTable(string connectionString, string query, object[] paramIn)
        {
            try
            {
                DataTable table = new DataTable();
                using (IDbConnection connection = this.GetConnection(connectionString))
                {
                    using (IDbCommand command = connection.CreateCommand())
                    {
                        // Ajout des paramètres
                        if (paramIn != null) query = string.Format(query, paramIn);
                        // Configuration de la command
                        this.ConfigureCommand(command, null, CommandType.Text, query, null, null, null);
                        // Ouverture de la connexion
                        connection.Open();
                        table = this.GetDataTable(command);
                    }
                }
                return table;
            }
            catch (Exception ex)
            {
                throw new DataProviderException(ex.Message, ex);
            }
        }
        /// <summary>
        /// Renvoi la DataTable résultante de l'exécution de la requête paramètrée spécifiée
        /// </summary>
        /// <param name="connectionString">Chaine de connexion à la base de données</param>
        /// <param name="query">Corps de la requête à exécuter</param>
        /// <param name="paramIn">Liste des paramètres nommés de la requête</param>
        /// <returns>
        /// DataTable contenant les données fournies par la requête
        /// </returns>
        public DataTable ExecuteDataTable(string connectionString, string query, IDbDataParameter[] paramIn)
        {
            try
            {
                DataTable table = new DataTable();
                using (IDbConnection connection = this.GetConnection(connectionString))
                {
                    using (IDbCommand command = connection.CreateCommand())
                    {
                        // Configuration de la command
                        this.ConfigureCommand(command, null, CommandType.Text, query, paramIn, null, null);
                        // Ouverture de la connexion
                        connection.Open();
                        table = this.GetDataTable(command);
                    }
                }
                return table;
            }
            catch (Exception ex)
            {
                throw new DataProviderException(ex.Message, ex);
            }
        }
        /// <summary>
        /// Renvoi la DataTable résultante de l'exécution de la procédure stockée paramétrée dans l'appel
        /// </summary>
        /// <param name="connectionString">Chaine de connexion à la base de données</param>
        /// <param name="procedureName">Nom de la procédure stockée</param>
        /// <param name="paramIn">Liste des paramètres de la procédure stockée en entrée</param>
        /// <param name="paramOut">Liste des paramètres de sortie de la procédure stockée</param>
        /// <param name="paramReturn">Paramètre de retour de la procédure</param>
        /// <returns>
        /// DataTable chargé avec les données
        /// </returns>
        public DataTable ExecuteDataTableProcedure(string connectionString, string procedureName, IDbDataParameter[] paramIn, IDbDataParameter[] paramOut, IDbDataParameter paramReturn)
        {
            try
            {
                DataTable table = new DataTable();
                using (IDbConnection connection = this.GetConnection(connectionString))
                {
                    using (IDbCommand command = connection.CreateCommand())
                    {
                        // Configuration de la commande
                        this.ConfigureCommand(command, null, CommandType.StoredProcedure, procedureName, paramIn, paramOut, paramReturn);
                        // Ouverture de la connexion
                        connection.Open();
                        // Exécution de la requête et Récupération de la DataTable
                        table = this.GetDataTable(command);
                    }
                }
                return table;
            }
            catch (Exception ex)
            {
                throw new DataProviderException(ex.Message, ex);
            }
        }
        #endregion

        #region Execute Non Query
        /// <summary>
        /// Exécution de la requête paramétrée spécifiée
        /// Requête de type 'NonQuery' => Insert, Update, Delete, ...
        /// </summary>
        /// <param name="connectionString">Chaine de connexion à la base de données</param>
        /// <param name="query">Corps de la requête à exécuter</param>
        /// <param name="paramIn">
        /// Liste des valeurs au format Texte à fournir à la requête
        /// Effecute un string.Format(query, paramIn)
        /// </param>
        /// <returns>
        /// Nombre de rows affectés
        /// </returns>
        public int ExecuteNonQuery(string connectionString, string query, object[] paramIn)
        {
            try
            {
                int result = 0;
                using (IDbConnection connection = this.GetConnection(connectionString))
                {
                    using (IDbCommand command = connection.CreateCommand())
                    {
                        // Ajout des paramètres
                        if (paramIn != null) query = string.Format(query, paramIn);
                        // Configuration de la command
                        this.ConfigureCommand(command, null, CommandType.Text, query, null, null, null);
                        // Ouverture de la connexion
                        connection.Open();
                        // Exécution de la requête NonQuery
                        result = command.ExecuteNonQuery();
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new DataProviderException(ex.Message, ex);
            }
        }
        /// <summary>
        /// Exécution de la requête paramétrée spécifiée
        /// Requête de type 'NonQuery' => Insert, Update, Delete, ...
        /// </summary>
        /// <param name="connectionString">Chaine de connexion à la base de données</param>
        /// <param name="query">Corps de la requête à exécuter</param>
        /// <param name="paramIn">Liste des paramètres nommés de la requête </param>
        /// <returns>
        /// Nombre de rows affectés
        /// </returns>
        public int ExecuteNonQuery(string connectionString, string query, IDbDataParameter[] paramIn)
        {
            try
            {
                int result = 0;
                using (IDbConnection connection = this.GetConnection(connectionString))
                {
                    using (IDbCommand command = connection.CreateCommand())
                    {
                        // Configuration de la command
                        this.ConfigureCommand(command, null, CommandType.Text, query, paramIn, null, null);
                        // Ouverture de la connexion
                        connection.Open();
                        // Exécution de la requête NonQuery
                        result = command.ExecuteNonQuery();
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new DataProviderException(ex.Message, ex);
            }
        }
        /// <summary>
        /// Exécution de la procédure stockée paramétrée dans l'appel
        /// </summary>
        /// <param name="connectionString">Chaine de connexion à la base de données</param>
        /// <param name="procedureName">Nom de la procédure stockée</param>
        /// <param name="paramIn">Liste des paramètres de la procédure stockée en entrée</param>
        /// <param name="paramOut">Liste des paramètres de sortie de la procédure stockée</param>
        /// <param name="paramReturn">Paramètre de retour de la procédure</param>
        /// <returns>
        /// Nombre de rows affectés
        /// </returns>
        public int ExecuteNonQueryProcedure(string connectionString, string procedureName, IDbDataParameter[] paramIn, IDbDataParameter[] paramOut, IDbDataParameter paramReturn)
        {
            try
            {
                int result = 0;
                using (IDbConnection connection = this.GetConnection(connectionString))
                {
                    using (IDbCommand command = connection.CreateCommand())
                    {
                        // Configuration de la command
                        this.ConfigureCommand(command, null, CommandType.StoredProcedure, procedureName, paramIn, paramOut, paramReturn);
                        // Ouverture de la connexion
                        connection.Open();
                        // Exécution de la requête NonQuery
                        result = command.ExecuteNonQuery();
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new DataProviderException(ex.Message, ex);
            }
        }
        #endregion

        #region Execute Scalar
        /// <summary>
        /// Exécution de la requête paramétrée spécifiée
        /// Requête de type 'Scalar' => Insert, Update, Delete, ...
        /// </summary>
        /// <param name="connectionString">Chaine de connexion à la base de données</param>
        /// <param name="query">Corps de la requête à exécuter</param>
        /// <param name="paramIn">
        /// Liste des valeurs au format Texte à fournir à la requête
        /// Effecute un string.Format(query, paramIn)
        /// </param>
        /// <returns>
        /// Valeur scalaire renvoyée par la requête
        /// </returns>
        public object ExecuteScalar(string connectionString, string query, object[] paramIn)
        {
            try
            {
                object result = null;
                using (IDbConnection connection = this.GetConnection(connectionString))
                {
                    using (IDbCommand command = connection.CreateCommand())
                    {
                        // Ajout des paramètres
                        if (paramIn != null) query = string.Format(query, paramIn);
                        // Configuration de la command
                        this.ConfigureCommand(command, null, CommandType.Text, query, null, null, null);
                        // Ouverture de la connexion
                        connection.Open();
                        // Exécution de la requête Scalaire
                        result = command.ExecuteScalar();
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new DataProviderException(ex.Message, ex);
            }
        }
        /// <summary>
        /// Exécution de la requête paramétrée spécifiée
        /// Requête de type 'Scalar' => Insert, Update, Delete, ...
        /// </summary>
        /// <param name="connectionString">Chaine de connexion à la base de données</param>
        /// <param name="query">Corps de la requête à exécuter</param>
        /// <param name="paramIn">Liste des paramètres nommés de la requête </param>
        /// <returns>
        /// Valeur scalaire  renvoyée par la requête
        /// </returns>
        public object ExecuteScalar(string connectionString, string query, IDbDataParameter[] paramIn)
        {
            try
            {
                object result = null;
                using (IDbConnection connection = this.GetConnection(connectionString))
                {
                    using (IDbCommand command = connection.CreateCommand())
                    {
                        // Configuration de la command
                        this.ConfigureCommand(command, null, CommandType.Text, query, paramIn, null, null);
                        // Ouverture de la connexion
                        connection.Open();
                        // Exécution de la requête Scalaire
                        result = command.ExecuteScalar();
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new DataProviderException(ex.Message, ex);
            }
        }
        /// <summary>
        /// Exécution de la procédure stockée de type 'Scalar' paramétrée dans l'appel
        /// </summary>
        /// <param name="connectionString">Chaine de connexion à la base de données</param>
        /// <param name="procedureName">Nom de la procédure stockée</param>
        /// <param name="paramIn">Liste des paramètres de la procédure stockée en entrée</param>
        /// <param name="paramOut">Liste des paramètres de sortie de la procédure stockée</param>
        /// <param name="paramReturn">Paramètre de retour de la procédure</param>
        /// <returns>
        /// Valeur scalaire renvoyée par la procédure stockée
        /// </returns>
        public object ExecuteScalarProcedure(string connectionString, string procedureName, IDbDataParameter[] paramIn, IDbDataParameter[] paramOut, IDbDataParameter paramReturn)
        {
            try
            {
                object result = null;
                using (IDbConnection connection = this.GetConnection(connectionString))
                {
                    using (IDbCommand command = connection.CreateCommand())
                    {
                        // Configuration de la command
                        this.ConfigureCommand(command, null, CommandType.StoredProcedure, procedureName, paramIn, paramOut, paramReturn);
                        // Ouverture de la connexion
                        connection.Open();
                        // Exécution de la requête Scalaire
                        result = command.ExecuteScalar();
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new DataProviderException(ex.Message, ex);
            }
        }
        #endregion

        #endregion

        #endregion

        #region Méthodes 'utilitaires'

        /// <summary>
        /// Configuration d'une IDBCommand en fonction des paramètres
        /// </summary>
        /// <param name="command">IDbCommand à configurer</param>
        /// <param name="transaction">Transaction dans laquelle exécuter la command</param>
        /// <param name="type">CommandType : Query, StoredProcedure, ...</param>
        /// <param name="commandText">Texte de la commande : corps de requête, nom de procédure, ...</param>
        /// <param name="paramIn">Liste des paramètres d'entrée</param>
        /// <param name="paramOut">Liste des paramètres de sortie</param>
        /// <param name="paramReturn">Paramètre de retour</param>
        private void ConfigureCommand(IDbCommand command, IDbTransaction transaction, CommandType type, string commandText, IDbDataParameter[] paramIn, IDbDataParameter[] paramOut, IDbDataParameter paramReturn)
        {
            // Type de command, text, transaction et timeout
            command.CommandText = commandText;
            command.CommandType = type;
            command.CommandTimeout = this._timeout;
            if (transaction != null) command.Transaction = transaction;

            // Paramètres en entrée
            if (paramIn != null) foreach (IDbDataParameter p in paramIn) { command.Parameters.Add(p); }

            // Paramètres de sortie
            if (paramOut != null) foreach (IDbDataParameter p in paramOut) { command.Parameters.Add(p); }

            // Paramètres de retour
            if (paramReturn != null) command.Parameters.Add(paramReturn);
        }

        #endregion
    }
}
