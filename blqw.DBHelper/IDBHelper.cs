using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace blqw.Data
{
    /// <summary> 用于操作数据库的帮助类接口
    /// </summary>
    public interface IDBHelper : IDisposable
    {
        #region config
        /// <summary> 定义数据库连接字符串值
        /// </summary>
        string ConnectionString { get; }
        /// <summary> 对象名称
        /// </summary>
        string Name { get; }
        /// <summary> 数据提供程序的名称
        /// </summary>
        string ProviderName { get; }
        #endregion

        #region transaction
        /// <summary> 开启默认事务
        /// </summary>
        void Begin();
        /// <summary> 提交默认事务
        /// </summary>
        void Commit();
        /// <summary> 回滚默认事务
        /// </summary>
        void Rollback();
        /// <summary> 开启指定事务
        /// </summary>
        /// <param name="il"></param>
        DbTransaction Begin(IsolationLevel il);
        #endregion

        #region execute
        int ExecuteNonQuery(CommandType commandType, string commandText, params DbParameter[] parameters);
        IDataReader ExecuteReader(CommandType commandType, string commandText, params DbParameter[] parameters);
        object ExecuteScalar(CommandType commandType, string commandText, params DbParameter[] parameters);
        DataTable ExecuteTable(CommandType commandType, string commandText, params DbParameter[] parameters);
        DataSet ExecuteSet(CommandType commandType, string commandText, params DbParameter[] parameters);

        int ExecuteNonQuery(ExecuteArgs e);
        IDataReader ExecuteReader(ExecuteArgs e);
        object ExecuteScalar(ExecuteArgs e);
        DataTable ExecuteTable(ExecuteArgs e);
        DataSet ExecuteSet(ExecuteArgs e);

        IExecuteResult Execute(params ExecuteArgs[] args); 
        #endregion
    }
}
