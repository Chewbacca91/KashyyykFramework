using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KashyyykFramework.GenericDbAccess
{
    sealed class OleDBDataProvider : DataProviderBase
    {
        internal OleDBDataProvider() : base() { }
        public override IDbDataAdapter GetAdapter(IDbCommand command)
        {
            try
            {
                return new OleDbDataAdapter((OleDbCommand)command);
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
                return new OleDbConnection(connectionString);
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
                    return new OleDbParameter(name, DBNull.Value);
                else
                    return new OleDbParameter(name, value);
            }
            catch (Exception ex)
            {
                throw new DataProviderException(ex.Message, ex);
            }
        }
    }
}
