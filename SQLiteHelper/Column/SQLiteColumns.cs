using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteHelper
{
    public class SQLiteColumns : List<SQLiteColumn>
    {
        private SQLiteTable _table = null;

        internal SQLiteColumns(SQLiteTable table)
        {
            _table = table;
        }

        public SQLiteColumn this[string name]
        {
            get
            {
                foreach (SQLiteColumn item in this)
                {
                    if (item.Name == name)
                        return item;
                }

                throw new ApplicationException($"Collection does not contain an item with name '{name}'");
            }

            set
            {
                foreach (SQLiteColumn item in this)
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
        /// Add the column to the end of the table.
        /// </summary>
        /// <param name="newColumn">The column whose should be added to the end of the table columns.</param>
        public new void Add(SQLiteColumn newColumn)
        {
            base.Add(newColumn);
            OrganizeColumnIndex();
        }

        /// <summary>
        /// Adds the list of column to the end of the table.
        /// </summary>
        /// <param name="collection">The collection whose should be added to the end of the table columns.</param>
        public new void AddRange(IEnumerable<SQLiteColumn> collection)
        {
            base.AddRange(collection);
            OrganizeColumnIndex();
        }

        #endregion

        /// <summary>
        /// Organize column number in current list.
        /// </summary>
        private void OrganizeColumnIndex()
        {
            for (int i = 0; i < Count; i++)
            {
                this[i].Index = i;
            }
        }
    }
}
