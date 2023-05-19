using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace HomeBase
{
    internal static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // DBの初期化
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=database.db"))
            {
                DBInitializer initializer = new DBInitializer(connection);
                initializer.CreateTables(connection);
            }
            Application.Run(new Form1());
        }
    }
}
