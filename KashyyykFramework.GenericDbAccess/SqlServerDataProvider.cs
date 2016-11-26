using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KashyyykFramework.GenericDbAccess
{
    sealed class SqlServerDataProvider : DataProviderBase
    {
        internal SqlServerDataProvider() : base() { }
        public override IDbDataAdapter GetAdapter(IDbCommand command)
        {
            try
            {
                return new SqlDataAdapter((SqlCommand)command);
            }
            catch (Exception ex)
            {
                throw new DataProviderException(ex.Message, ex);
            }
        }
        public override IDbConnection GetConnection(string connectionString)
        {
            try
            {
                return new SqlConnection(connectionString);
            }
            catch (Exception ex)
            {
                throw new DataProviderException(ex.Message, ex);
            }
        }
        public override IDbDataParameter GetParam(string name, object value)
        {
            try
            {
                if (value == null)
                    return new SqlParameter(name, DBNull.Value);
                else
                    return new SqlParameter(name, value);
            }
            catch (Exception ex)
            {
                throw new DataProviderException(ex.Message, ex);
            }
        }        
    }
}
