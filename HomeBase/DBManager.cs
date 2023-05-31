using System;
using System.Data;
using System.Data.SQLite;
using System.Configuration;

namespace HomeBase
{
    public class DBManager : IDisposable
    {
        private SQLiteConnection _connection;
        private static readonly object LockObject = new object();

        public SQLiteConnection Connection => _connection;

        public DBManager()
        {
            string connectionString = ConfigurationHelper.GetConnectionString();
            _connection = new SQLiteConnection(connectionString);
            _connection.Open();
        }

        public SQLiteConnection GetConnection()
        {
            lock (LockObject)
            {
                if (_connection.State != ConnectionState.Open)
                    _connection.Open();

                return _connection;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
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

