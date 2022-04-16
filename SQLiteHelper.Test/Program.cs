using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SQLiteHelper;

namespace SQLiteHelper.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            string dbPath = $"{Directory.GetCurrentDirectory()}\\Test.db";
            SQLiteDatabase db = new SQLiteDatabase(dbPath);
            //db.Tables["TestTable"].Delete();

            SQLiteTable table = new SQLiteTable(db, "TestTable",
                new SQLiteTextColumn("Name", null, true, true, true),
                new SQLiteIntColumn("Age", 0));
            db.Tables["TestTable"].Delete();

            SQLiteTable table2 = new SQLiteTable(db, "TestTable");
        }
    }
}
