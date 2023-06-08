using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;

namespace HomeBase
{
    public class DBManager : IDisposable
    {
        private SQLiteConnection _connection;
        private SQLiteTransaction _transaction;
        private bool _disposed;
        private static readonly object LockObject = new object();

        public DBManager()
        {
            string connectionString = ConfigurationHelper.GetConnectionString();
            _connection = new SQLiteConnection(connectionString);
            _connection.Open();
        }

        public SQLiteConnection Connection
        {
            get
            {
                if (_connection.State != ConnectionState.Open)
                    _connection.Open();

                return _connection;
            }
        }

        public void BeginTransaction()
        {
            _transaction = _connection.BeginTransaction();
        }

        public void CommitTransaction()
        {
            _transaction.Commit();
            _transaction = null;
        }

        public void RollbackTransaction()
        {
            _transaction.Rollback();
            _transaction = null;
        }

        public int ExecuteNonQuery(string query, Dictionary<string, object> parameters = null)
        {
            using (SQLiteCommand command = CreateCommand(query, parameters))
            {
                return command.ExecuteNonQuery();
            }
        }

        public object ExecuteScalar(string query, Dictionary<string, object> parameters = null)
        {
            using (SQLiteCommand command = CreateCommand(query, parameters))
            {
                return command.ExecuteScalar();
            }
        }

        public SQLiteDataReader ExecuteReader(string query, Dictionary<string, object> parameters = null)
        {
            SQLiteCommand command = CreateCommand(query, parameters);
            return command.ExecuteReader();
        }

        private SQLiteCommand CreateCommand(string query, Dictionary<string, object> parameters)
        {
            SQLiteCommand command = _connection.CreateCommand();
            command.CommandText = query;
            command.Transaction = _transaction;

            if (parameters != null)
            {
                foreach (KeyValuePair<string, object> parameter in parameters)
                {
                    SQLiteParameter sqlParameter = new SQLiteParameter(parameter.Key, parameter.Value);
                    command.Parameters.Add(sqlParameter);
                }
            }

            return command;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_transaction != null)
                    {
                        _transaction.Dispose();
                        _transaction = null;
                    }

                    if (_connection != null)
                    {
                        lock (LockObject)
                        {
                            _connection.Close();
                            _connection.Dispose();
                            _connection = null;
                        }
                    }
                }

                _disposed = true;
            }
        }

    }



    public static class ConfigurationHelper
    {
        public static string GetConnectionString()
        {
            return ConfigurationManager.AppSettings["ConnectionString"];
        }
    }
}
