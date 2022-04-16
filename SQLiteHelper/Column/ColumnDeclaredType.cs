namespace SQLiteHelper
{
    public enum ColumnDeclaredType
    {
        /// <summary>
        /// The value is a signed integer, stored in 0, 1, 2, 3, 4, 6, or 8 bytes depending on the magnitude of the value.
        /// </summary>
        TEXT = 0,

        NUMERIC = 1,
        INTEGER = 2,
        REAL = 3,
        BLOB = 4

    }
}
