using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KashyyykFramework.GenericDbAccess
{
    sealed class OdbcDataProvider : DataProviderBase
    {
        internal OdbcDataProvider() : base() { }
        public override IDbDataAdapter GetAdapter(IDbCommand command)
        {
            try
            {
                return new OdbcDataAdapter((OdbcCommand)command);
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
                return new OdbcConnection(connectionString);
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
                    return new OdbcParameter(name, DBNull.Value);
                else
                    return new OdbcParameter(name, value);
            }
            catch (Exception ex)
            {
                throw new DataProviderException(ex.Message, ex);
            }
        }
    }
}
