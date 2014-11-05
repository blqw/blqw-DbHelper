using System;
using System.Collections.Generic;
using System.Text;

namespace blqw
{
    public enum ExecuteAction
    {
        Unknown = 0,
        NonQuery,
        DataReader,
        Scalar,
        DataTable,
        DataSet,
    }
}
