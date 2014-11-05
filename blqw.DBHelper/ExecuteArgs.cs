using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace blqw
{
    public struct ExecuteArgs
    {
        public string CommandText { get; set; }
        public CommandType CommandType { get; set; }
        public List<DbParameter> DbParameters { get; set; }
    }
}
