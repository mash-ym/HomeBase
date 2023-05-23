using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeBase
{
    public class DBManager : IDisposable
    {
        private static readonly string ConnectionString = "Your_Connection_String";
        private static readonly object LockObject = new object();
        private SQLiteConnection _connection;

        public SQLiteConnection Connection => _connection;

        public DBManager()
        {
            _connection = new SQLiteConnection(ConnectionString);
            _connection.Open();
        }
        public SQLiteConnection GetConnection()
        {
            if (_connection.State != ConnectionState.Open)
                _connection.Open();

            return _connection;
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
                    _connection.Close();
                    _connection.Dispose();
                    _connection = null;
                }
            }
        }
    }

}
