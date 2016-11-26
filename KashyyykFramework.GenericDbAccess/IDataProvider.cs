using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;

namespace KashyyykFramework.GenericDbAccess
{
    /// <summary>
    /// Interface qui fournit les méthodes d'accès aux données.
    /// </summary>
    public interface IDataProvider
    {
        /* TimeOut */
        int Timeout { get; }
        void SetTimeout(int timeout);

        /* Abstraction */
        IDbConnection GetConnection(string connectionString);
        IDbDataAdapter GetAdapter(IDbCommand command);
        IDbDataParameter[] GetParams(IDictionary<string, object> values);
        IDbDataParameter GetParam(string name, object value);

        /* Execution de requêtes - Renvoi DataTable */
        DataTable GetDataTable(IDbCommand command);
        DataTable ExecuteDataTable(string connectionString, string query, object[] paramIn);
        DataTable ExecuteDataTable(string connectionString, string query, IDbDataParameter[] paramIn);
        DataTable ExecuteDataTableProcedure(string connectionString, string procedureName, IDbDataParameter[] paramIn, IDbDataParameter[] paramOut, IDbDataParameter paramReturn);

        /* Execution de requêtes - Renvoi DataSet */
        DataSet ExecuteDataSet(string connectionString, string query, object[] paramIn);
        DataSet ExecuteDataSet(string connectionString, string query, IDbDataParameter[] paramIn);
        DataSet ExecuteDataSetProcedure(string connectionString, string procedureName, IDbDataParameter[] paramIn, IDbDataParameter[] paramOut, IDbDataParameter paramReturn);

        /* Execution de requêtes - Non Query (Update, Insert, Delete, ...) */
        int ExecuteNonQuery(string connectionString, string query, object[] paramIn);
        int ExecuteNonQuery(string connectionString, string query, IDbDataParameter[] paramIn);
        int ExecuteNonQueryProcedure(string connectionString, string procedureName, IDbDataParameter[] paramIn, IDbDataParameter[] paramOut, IDbDataParameter paramReturn);

        /* Exécution de requêtes - ExecuteScalar (Renvoi d'une valeur unique) */
        object ExecuteScalar(string connectionString, string query, object[] paramIn);
        object ExecuteScalar(string connectionString, string query, IDbDataParameter[] paramIn);
        object ExecuteScalarProcedure(string connectionString, string procedureName, IDbDataParameter[] paramIn, IDbDataParameter[] paramOut, IDbDataParameter paramReturn);        
    }
}
