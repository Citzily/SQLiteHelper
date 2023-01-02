using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;

namespace SQLiteHelper
{
    [System.Diagnostics.DebuggerDisplay("{_debuggerDisplayString, nq}")]
    public class SQLiteTable
    {
        private SQLiteDatabase _sqliteDB = null;
        private DataTable _columnInfo = null;
        private SQLiteColumns _columns = null;

        private string _debuggerDisplayString => $"{_sqliteDB.Name}.{Name} [{_columns.Count}]";

        #region Properties

        /// <summary>
        /// Table name.
        /// </summary>
        public string Name { get; private set; }
        public bool Exist { get; }

        /// <summary>
        /// Table columns.
        /// </summary>
        public SQLiteColumns Columns
        {
            get { return _columns; }
            set { _columns = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Create a table which was already exist in database.
        /// </summary>
        /// <param name="sqliteDB">SQLite database object.</param>
        /// <param name="name">Name of the table.</param>
        internal SQLiteTable(SQLiteDatabase sqliteDB, string name)
        {
            _sqliteDB = sqliteDB;
            Name = name;
            _columns = new SQLiteColumns(this);
        }

        /// <summary>
        /// Create a new table.
        /// </summary>
        /// <param name="sqliteDB">SQLite database which the new table will create in.</param>
        /// <param name="name">Name of the table.</param>
        /// <param name="columns">Columns in new table.</param>
        public SQLiteTable(SQLiteDatabase sqliteDB, string name, params SQLiteColumn[] columns)
        {
            //Check sqliteDB
            if (sqliteDB is null || !sqliteDB.Exist)
                throw new ArgumentException("The SQLiteDatabase is null or not exist.");
            _sqliteDB = sqliteDB;

            //Check name
            if (_sqliteDB.Tables.Contains(name))
                throw new ArgumentException($"The table name is already exist in the {sqliteDB.Name} Database.");
            Name = name;

            //Check column data
            if (columns.Length == 0)
                throw new ArgumentException($"No columns define when create table '{Name}'.");
            _columns = new SQLiteColumns(this);
            _columns.AddRange(columns);

            //Create table
            CreateTable(columns);

            //Add to tables list
            _sqliteDB.Tables.Add(this);
        }

        #endregion

        internal DataTable TableInfo(SQLiteConnection cnn)
        {
            _columnInfo = new DataTable();
            SQLiteDataAdapter db = new SQLiteDataAdapter($"PRAGMA table_info({Name});", cnn);
            db.Fill(_columnInfo);

            foreach (DataRow row in _columnInfo.Rows)
            {
                SQLiteColumn newColumn = null;

                string columnType = row[2].ToString();
                if (columnType == ColumnDeclaredType.TEXT.ToString())
                {
                    newColumn = new SQLiteTextColumn(
                        row[1].ToString(),
                        row[4].ToString(),
                        false,
                        row[5].ToString() == "1",
                        row[3].ToString() == "1");
                }
                else if (columnType == ColumnDeclaredType.NUMERIC.ToString() ||
                    columnType == ColumnDeclaredType.REAL.ToString())
                {
                    double? dflt_value = null;
                    if (double.TryParse(row[4].ToString(), out double value))
                        dflt_value = value;

                    newColumn = new SQLiteRealColumn(
                        row[1].ToString(),
                        dflt_value,
                        false,
                        row[5].ToString() == "1",
                        row[3].ToString() == "1");
                }
                else if (columnType == ColumnDeclaredType.INTEGER.ToString())
                {
                    int? dflt_value = null;
                    if (int.TryParse(row[4].ToString(), out int value))
                        dflt_value = value;

                    newColumn = new SQLiteIntColumn(
                        row[1].ToString(),
                        dflt_value,
                        false,
                        row[5].ToString() == "1",
                        row[3].ToString() == "1");
                }
                else
                {

                }

                if (newColumn != null)
                    _columns.Add(newColumn);

            }

            return _columnInfo;
        }

        private void CreateTable(SQLiteColumn[] columns)
        {
            #region Get create command

            //Create table command
            string createStrings = $"CREATE TABLE {Name} (";

            //Columns command
            foreach (SQLiteColumn item in columns)
            {
                createStrings += item.CreateString;
            }

            //Primary key
            if (columns.Any(col => col.PrimaryKey))
            {
                createStrings += $"PRIMARY KEY ({string.Join(",", columns.Where(col => col.PrimaryKey).Select(col => col.Name))})";
            }

            //End command
            createStrings += ");";

            #endregion

            #region Create table

            try
            {
                using (SQLiteConnection cnn = new SQLiteConnection(_sqliteDB.ConnectionString))
                {
                    cnn.Open();
                    _sqliteDB.DoCommands(cnn, createStrings);
                    cnn.Close();
                }
            }
            catch (Exception ex)
            {
                throw new SQLiteException($"An error throw when creating the '{Name}' table.", ex);
            }

            #endregion
        }

        /// <summary>
        /// Remove table definition and all associated data, indexes, triggers, constraints, and permission specifications for that table.
        /// </summary>
        public void Delete()
        {
            if (!_sqliteDB.Tables.Contains(Name))
                throw new SQLiteException($"Table '{Name}' did not exist in {_sqliteDB}.");

            using (SQLiteConnection cnn = new SQLiteConnection(_sqliteDB.ConnectionString))
            {
                cnn.Open();
                _sqliteDB.DoCommands(cnn, $"DROP TABLE {Name}");
                cnn.Close();
            }

            _sqliteDB.Tables.Remove(this);
        }

        public void InsertData(SQLiteColumn[] columns, string[] values)
        {
            if (columns is null || columns.Length == 0)
                throw new ArgumentException("columns param is null or no item.");

            if (values is null || values.Length == 0)
                throw new ArgumentException("values param is null or no item.");

            if (columns.Length != values.Length)
                throw new ArgumentException("The lengths of the columns and values arrays must be equal.");

            try
            {
                using (SQLiteConnection cnn = new SQLiteConnection(_sqliteDB.ConnectionString))
                {
                    cnn.Open();
                    string insert = $"INSERT INTO {Name} ({string.Join(",", columns.Select(col => col.Name))}) " +
                        $"VALUES({string.Join(",", values)});";
                    _sqliteDB.DoCommands(cnn, insert);
                    cnn.Close();
                }
            }
            catch (Exception ex)
            {
                throw new SQLiteException($"An exception throw when insert data into {Name}", ex);
            }

        }

        public void InsertData<T>(SQLiteColumn[] columns, T value)
        {
            if (columns is null || columns.Length == 0)
                throw new ArgumentException("columns param is null or no item.");

            PropertyInfo[] props = value.GetType().GetProperties();

            string insert = $"INSERT INTO {Name} ({string.Join(",", columns.Select(col => col.Name))}) ";

            string valueString = "(";

           

        }


    }
}
