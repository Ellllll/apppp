using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace App1
{

    class Sqlite2 : SQLiteConnection
    {
        static string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "sqliteinfo.db3");
        public Sqlite2() : base(dbPath) { }

    }
}
