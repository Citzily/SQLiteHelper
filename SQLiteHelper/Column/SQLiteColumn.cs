using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteHelper
{
    public abstract class SQLiteColumn
    {
        /// <summary>
        /// Column data type.
        /// </summary>
        public ColumnDeclaredType Type { get; protected set; }

        /// <summary>
        /// Column index number in table.
        /// </summary>
        public int Index { get; internal set; }

        /// <summary>
        /// Column Name.
        /// </summary>
        public string Name { get; private set; }
        public bool Unique { get; private set; }
        public bool PrimaryKey { get; private set; }
        public bool NotNull { get; private set; }
        public bool Hidden { get; private set; }
        public virtual object InitialValue { get; protected set; }
        internal abstract string CreateString { get; }

        protected string BaseCreateString()
        {
            string command = $"{Name} {Type}";

            if (NotNull) command += " NOT NULL";
            if (Unique) command += " UNIQUE";
            command += $",";
            return command;

        }

        internal SQLiteColumn(string name, bool unique, bool primaryKey, bool notNull, bool hidden = false)
        {
            Name = name;
            Unique = unique;
            NotNull = notNull;
            PrimaryKey = primaryKey;
            Hidden = hidden;
        }

    }
}
