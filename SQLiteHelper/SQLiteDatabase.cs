using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteHelper
{
    public class SQLiteDatabase
    {

        private FileInfo _dbInfo = null;
        internal string ConnectionString => "data source=" + _dbInfo.FullName;

        public string Name => _dbInfo.Name;
        public string Path => _dbInfo.DirectoryName;
        public bool Exist => _dbInfo.Exists;
        public bool IsCreateNewDB { get; private set; } = false;

        #region Pragmas

        /// <summary>
        /// The user-version is an integer that is available to applications to use however they want. 
        /// SQLite makes no use of the user-version itself.
        /// </summary>
        public int UserVersion
        {
            get
            {
                int result = 0;
                try
                {
                    using (SQLiteConnection cnn = new SQLiteConnection(ConnectionString))
                    {
                        cnn.Open();
                        result = ReadInts(cnn, "pragma user_version;").FirstOrDefault();
                        cnn.Close();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return result;
            }
            set
            {
                try
                {
                    using (SQLiteConnection cnn = new SQLiteConnection(ConnectionString))
                    {
                        cnn.Open();
                        DoCommands(cnn, $"pragma user_version = {value};");
                        cnn.Close();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// The total number of pages in the database file.
        /// </summary>
        public int PageCount
        {
            get
            {
                int result = 0;
                try
                {
                    using (SQLiteConnection cnn = new SQLiteConnection(ConnectionString))
                    {
                        cnn.Open();
                        result = ReadInts(cnn, "pragma page_count;").FirstOrDefault();
                        cnn.Close();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return result;
            }

        }

        /// <summary>
        /// Query or set the page size of the database. 
        /// The page size must be a power of two between 512 and 65536 inclusive.
        /// </summary>
        public int PageSize
        {
            get
            {
                int result = 0;
                try
                {
                    using (SQLiteConnection cnn = new SQLiteConnection(ConnectionString))
                    {
                        cnn.Open();
                        result = ReadInts(cnn, "pragma page_size;").FirstOrDefault();
                        cnn.Close();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return result;
            }
        }

        /// <summary>
        /// Query or set the maximum number of pages in the database file. 
        /// The second form attempts to modify the maximum page count.
        /// </summary>
        public int MaxPageCount
        {
            get
            {
                int result = 0;
                try
                {
                    using (SQLiteConnection cnn = new SQLiteConnection(ConnectionString))
                    {
                        cnn.Open();
                        result = ReadInts(cnn, "pragma max_page_count;").FirstOrDefault();
                        cnn.Close();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return result;
            }

            set
            {
                if (value <= PageCount)
                    throw new Exception("The maximum page count cannot be reduced below the current database size.");

                try
                {
                    using (SQLiteConnection cnn = new SQLiteConnection(ConnectionString))
                    {
                        cnn.Open();
                        DoCommands(cnn, $"pragma max_page_count = {value};");
                        cnn.Close();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }

        public int DefaultCatchSize
        {
            get
            {
                int result = 0;
                try
                {
                    using (SQLiteConnection cnn = new SQLiteConnection(ConnectionString))
                    {
                        cnn.Open();
                        result = ReadInts(cnn, "pragma default_cache_size ;").FirstOrDefault();
                        cnn.Close();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return result;
            }

            set
            {
                if (value <= 0)
                    throw new Exception("Default catch size can not lower then 1.");

                try
                {
                    using (SQLiteConnection cnn = new SQLiteConnection(ConnectionString))
                    {
                        cnn.Open();
                        DoCommands(cnn, $"pragma default_cache_size = {value};");
                        cnn.Close();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        #endregion

        private SQLiteTables _tables = null;

        public SQLiteTables Tables
        {
            get { return _tables; }
            set { _tables = value; }
        }


        public SQLiteDatabase(string path)
        {
            InitializeDBFile(path);
            LoadTables();
        }

        private bool InitializeDBFile(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path),
                    "Path of the DB file is null or empty.");
            }

            try
            {
                _dbInfo = new FileInfo(path);

                //Check directory
                if (!Directory.Exists(_dbInfo.DirectoryName))
                    Directory.CreateDirectory(_dbInfo.DirectoryName);

                //Check db file
                if (!_dbInfo.Exists)
                {
                    SQLiteConnection.CreateFile(_dbInfo.FullName);
                    IsCreateNewDB = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;
        }

        private void LoadTables()
        {
            _tables = new SQLiteTables(this);
        }

        internal void DoCommands(SQLiteConnection cnn, params string[] commands)
        {
            SQLiteCommand sqlite = cnn.CreateCommand();
            foreach (string command in commands)
            {
                sqlite.CommandText = command;
                sqlite.ExecuteNonQuery();
            }
        }

        internal List<string> ReadStrings(SQLiteConnection cnn, string command)
        {
            List<string> reault = new List<string>();
            using (SQLiteCommand sqlite_cmd = cnn.CreateCommand())
            {
                sqlite_cmd.CommandText = command;
                using (SQLiteDataReader sqlite_datareader = sqlite_cmd.ExecuteReader())
                {
                    while (sqlite_datareader.Read())
                    {
                        reault.Add(sqlite_datareader.GetString(0));
                    }
                }
            }

            return reault;
        }

        internal List<int> ReadInts(SQLiteConnection cnn, string command)
        {
            List<int> reault = new List<int>();
            using (SQLiteCommand sqlite_cmd = cnn.CreateCommand())
            {
                sqlite_cmd.CommandText = command;
                using (SQLiteDataReader sqlite_datareader = sqlite_cmd.ExecuteReader())
                {
                    while (sqlite_datareader.Read())
                    {
                        reault.Add(sqlite_datareader.GetInt32(0));
                    }
                }
            }

            return reault;
        }

        internal List<bool> ReadBoolean(SQLiteConnection cnn, string command)
        {
            List<bool> reault = new List<bool>();
            using (SQLiteCommand sqlite_cmd = cnn.CreateCommand())
            {
                sqlite_cmd.CommandText = command;
                using (SQLiteDataReader sqlite_datareader = sqlite_cmd.ExecuteReader())
                {
                    while (sqlite_datareader.Read())
                    {
                        reault.Add(sqlite_datareader.GetBoolean(0));
                    }
                }
            }
            return reault;
        }

    }
}
