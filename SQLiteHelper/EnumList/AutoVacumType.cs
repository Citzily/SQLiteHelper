using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteHelper
{
    /// <summary>
    /// Query or set the auto-vacuum status in the database.
    /// </summary>
    public enum AutoVacuumType
    {
        /// <summary>
        /// The default setting for auto-vacuum is 0 or "none", 
        /// unless the SQLITE_DEFAULT_AUTOVACUUM compile-time option is used. 
        /// The "none" setting means that auto-vacuum is disabled.
        /// When auto-vacuum is disabled and data is deleted data from a database, 
        /// the database file remains the same size. 
        /// Unused database file pages are added to a "freelist" and reused for subsequent inserts. 
        /// So no database file space is lost. 
        /// However, the database file does not shrink. 
        /// In this mode the VACUUM command can be used to rebuild the entire database file and thus reclaim unused disk space.
        /// </summary>
        NONE = 0,

        /// <summary>
        /// When the auto-vacuum mode is 1 or "full", 
        /// the freelist pages are moved to the end of the database file and the database file is truncated to remove the freelist pages at every transaction commit. 
        /// Note, however, that auto-vacuum only truncates the freelist pages from the file. 
        /// Auto-vacuum does not defragment the database nor repack individual database pages the way that the VACUUM command does. In fact, 
        /// \because it moves pages around within the file, auto-vacuum can actually make fragmentation worse.
        /// </summary>
        FULL = 1,

        /// <summary>
        /// When the value of auto-vacuum is 2 or "incremental" then the additional information needed to do auto-vacuuming is stored in the database file but auto-vacuuming does not occur automatically at each commit as it does with auto_vacuum=full. 
        /// In incremental mode, the separate incremental_vacuum pragma must be invoked to cause the auto-vacuum to occur.
        /// </summary>
        INCREMENTAL = 2
    }
}
