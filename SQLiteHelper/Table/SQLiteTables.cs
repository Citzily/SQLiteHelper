using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteHelper
{
    public class SQLiteTables : List<SQLiteTable>
    {
        private SQLiteDatabase _dbHelper = null;

        internal SQLiteTables(SQLiteDatabase dbHelper)
        {
            _dbHelper = dbHelper;
            LoadTables();
        }

        private void LoadTables()
        {
            using (SQLiteConnection cnn = new SQLiteConnection(_dbHelper.ConnectionString))
            {
                cnn.Open();
                string query = $"SELECT name FROM sqlite_master " +
                    "WHERE type = 'table'";
                List<string> tableNames = _dbHelper.ReadStrings(cnn, query);
                foreach (string name in tableNames)
                {
                    SQLiteTable table = new SQLiteTable(_dbHelper, name);
                    table.TableInfo(cnn);
                    Add(table);
                }

                cnn.Close();
            }
        }

        public SQLiteTable this[string name]
        {
            get
            {
                foreach (SQLiteTable item in this)
                {
                    if (item.Name == name)
                        return item;
                }

                throw new ApplicationException($"Collection does not contain an item with name '{name}'");
            }

            set
            {
                foreach (SQLiteTable item in this)
                {
                    if (item.Name == name)
                    {
                        this[IndexOf(item)] = value;
                    }
                }
            }
        }

        #region List methods

        /// <summary>
        /// Determines whether an table name is in the Database.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns>TRUE if item is found in the Database; otherwise, false.</returns>
        public bool Contains(string tableName)
        {
            if (string.IsNullOrEmpty(tableName)) return false;

            foreach (SQLiteTable table in this)
            {
                if (table.Name == tableName)
                    return true;
            }

            return false;
        }

        internal new void Add(SQLiteTable newColumn)
        {
            base.Add(newColumn);
        }

        #endregion
    }
}
