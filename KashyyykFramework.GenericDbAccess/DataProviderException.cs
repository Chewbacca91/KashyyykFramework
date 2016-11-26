using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KashyyykFramework.GenericDbAccess
{
    public class DataProviderException : Exception
    {
        public DataProviderException(string message, Exception inner) : base(message, inner) { }
        public DataProviderException(string message) : this(message, null) { }
    }
}
