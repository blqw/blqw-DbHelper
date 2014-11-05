using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace blqw
{
    public interface IExecuteResult
    {
        public ExecuteAction Action { get; }
        public int NonQuery { get; }
        public IDataReader DataReader { get; }
        public object Scalar { get; }
        public DataTable DataTable { get; }
        public DataSet DataSet { get; }
    }
}
