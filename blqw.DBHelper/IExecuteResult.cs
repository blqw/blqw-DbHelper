using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace blqw
{
    public interface IExecuteResult
    {
        ExecuteAction Action { get; }
        int NonQuery { get; }
        IDataReader DataReader { get; }
        object Scalar { get; }
        DataTable DataTable { get; }
        DataSet DataSet { get; }
    }
}
