using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteHelper
{
    [System.Diagnostics.DebuggerDisplay("{debuggerString, nq}")]
    public class SQLiteIntColumn : SQLiteColumn
    {
        private string debuggerString
        {
            get
            {
                string msg = $"{Index} {Name} {Type.ToString()}";
                if (NotNull) msg += " NotNull";
                if (PrimaryKey) msg += " PK";
                if (InitialValue != null) msg += $" Default {InitialValue}";
                if (Unique) msg += " Unique";
                return msg;
            }
        }

        public new long? InitialValue { get; private set; }

        internal override string CreateString
        {
            get
            {
                string command = BaseCreateString();
                if (InitialValue != null)
                    command.Insert(command.IndexOf(','), $" DEFAULT {InitialValue}");
                return command;
            }
        }

        public SQLiteIntColumn(string name, long? initial = null, bool unique = false, bool primaryKey = false, bool notNull = false)
           : base(name, unique, primaryKey, notNull)
        {
            InitialValue = initial;
            Type = ColumnDeclaredType.INTEGER;
        }
    }
}
