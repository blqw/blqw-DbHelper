using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Text;
using System.Data.SqlClient;

namespace blqw
{
    sealed class SqlServerHelper : IDBHelper
    {
        public SqlServerHelper(string connectionName)
        {
            var conn = System.Configuration.ConfigurationManager.ConnectionStrings[connectionName];
            if (conn == null)
            {
                throw new KeyNotFoundException("不存在名为 " + connectionName + " 的节点");
            }
            ConnectionString = conn.ConnectionString;
            ProviderName = conn.ProviderName;
            Name = conn.Name;
            Init();
        }

        public SqlServerHelper(string connectionString, string providerName)
        {
            ConnectionString = connectionString;
            ProviderName = providerName;
            Name = string.Empty;
            Init();
        }
        private void Init()
        {
            Assertor.AreNullOrWhiteSpace(ConnectionString, "ConnectionString");
            Assertor.AreNullOrWhiteSpace(ProviderName, "ProviderName");
            _connectionKey = string.Concat(ProviderName, "\f", ConnectionString);
            _connection = (SqlConnection)ConnectionPool.Get(_connectionKey, GetConnection);
        }

        private string _connectionKey;
        private SqlConnection _connection;
        private SqlTransaction _transaction;

        private DbConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        private DbCommand GetCommand(CommandType commandType, string commandText, DbParameter[] parameters)
        {
            var cmd = new SqlCommand(commandText, _connection, _transaction);
            if (parameters != null && parameters.Length > 0)
            {
                var p = parameters as SqlParameter[];
                if (p == null)
                {
                    throw new ArgumentOutOfRangeException("DbParameter类型错误");
                }
                cmd.Parameters.AddRange(p);
            }
            return cmd;
        }
        public string ConnectionString { get; private set; }

        public string Name { get; private set; }

        public string ProviderName { get; private set; }

        public void Close()
        {
            _connection.Close();
        }

        public void Open()
        {
            if ((_connection.State & ConnectionState.Open) == 0)
            {
                _connection.Open();
            }
        }

        public void Begin()
        {
            if (_transaction != null)
            {
                throw new NotSupportedException("重复开启事务");
            }
            Open();
            _transaction = _connection.BeginTransaction();
        }

        public void Commit()
        {
            if (_transaction == null)
            {
                throw new NotSupportedException("尚未开启事务");
            }
            _transaction.Commit();
            _transaction = null;
        }

        public void Rollback()
        {
            if (_transaction == null)
            {
                throw new NotSupportedException("尚未开启事务");
            }
            _transaction.Rollback();
            _transaction = null;
        }

        public DbTransaction Begin(IsolationLevel iso)
        {
            Open();
            return _connection.BeginTransaction(iso);
        }

        public int ExecuteNonQuery(CommandType commandType, string commandText, params DbParameter[] parameters)
        {
            using (var cmd = GetCommand(commandType, commandText, parameters))
            {
                Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public IDataReader ExecuteReader(CommandType commandType, string commandText, params DbParameter[] parameters)
        {
            using (var cmd = GetCommand(commandType, commandText, parameters))
            {
                Open();
                return cmd.ExecuteReader();
            }
        }

        public object ExecuteScalar(CommandType commandType, string commandText, params DbParameter[] parameters)
        {
            using (var cmd = GetCommand(commandType, commandText, parameters))
            {
                Open();
                return cmd.ExecuteScalar();
            }
        }

        public DataTable ExecuteTable(CommandType commandType, string commandText, params DbParameter[] parameters)
        {
            using (var cmd = GetCommand(commandType, commandText, parameters))
            using (var adp = new SqlDataAdapter((SqlCommand)cmd))
            {
                Open();
                var table = new DataTable();
                adp.Fill(table);
                return table;
            }
        }

        public DataSet ExecuteSet(CommandType commandType, string commandText, params DbParameter[] parameters)
        {
            using (var cmd = GetCommand(commandType, commandText, parameters))
            using (var adp = new SqlDataAdapter((SqlCommand)cmd))
            {
                Open();
                var dataset = new DataSet();
                adp.Fill(dataset);
                return dataset;
            }
        }

        public int ExecuteNonQuery(ExecuteArgs e)
        {
            return ExecuteNonQuery(e.CommandType, e.CommandText, e.DbParameters.ToArray());
        }

        public IDataReader ExecuteReader(ExecuteArgs e)
        {
            return ExecuteReader(e.CommandType, e.CommandText, e.DbParameters.ToArray());
        }

        public object ExecuteScalar(ExecuteArgs e)
        {
            return ExecuteScalar(e.CommandType, e.CommandText, e.DbParameters.ToArray());
        }

        public DataTable ExecuteTable(ExecuteArgs e)
        {
            return ExecuteTable(e.CommandType, e.CommandText, e.DbParameters.ToArray());
        }

        public DataSet ExecuteSet(ExecuteArgs e)
        {
            return ExecuteSet(e.CommandType, e.CommandText, e.DbParameters.ToArray());
        }

        public IExecuteResult Execute(params ExecuteArgs[] args)
        {
            throw new NotSupportedException("当前版本不支持!");
        }

        public void Dispose()
        {
            ConnectionPool.GiveBack(_connectionKey);
            _connection = null;
        }
    }
}
