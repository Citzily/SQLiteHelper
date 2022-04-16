using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteHelper
{
    [System.Diagnostics.DebuggerDisplay("{debuggerString, nq}")]
    public class SQLiteTextColumn : SQLiteColumn
    {
        private string debuggerString
        {
            get
            {
                string msg = $"{Index} {Name} {Type.ToString()}";
                if (NotNull) msg += " NotNull";
                if (PrimaryKey) msg += " PK";
                if (InitialValue != null) msg += $" Default '{InitialValue}'";
                if (Unique) msg += " Unique";
                return msg;
            }
        }

        public new string InitialValue { get; private set; }

        internal override string CreateString
        {
            get
            {
                string command = BaseCreateString();
                if (!string.IsNullOrEmpty(InitialValue))
                    command.Insert(command.IndexOf(','), $" DEFAULT \'{InitialValue}\'");
                return command;
            }
        }

        public SQLiteTextColumn(string name, string initial = "", bool unique = false, bool primaryKey = false, bool notNull = false)
            : base(name, unique, primaryKey, notNull)
        {
            InitialValue = initial;
            Type = ColumnDeclaredType.TEXT;
        }
    }
}
