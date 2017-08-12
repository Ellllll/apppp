using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace App1
{
    class Sqlite1:SQLiteConnection
    {
        static string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "userinfo.db3");
        public Sqlite1() : base(dbPath) { }

    }
}
