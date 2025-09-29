using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyBlackBoxCore.DataEntities;

namespace MyBlackBoxCore.DataEntities
{
    public class DbNote
    {
        public Int64 ID { set; get; }
        public int IDUser { set; get; }
        public string Title { set; get; }
        public string Text { set; get; }
        public DateTime DateTimeInserted { set; get; }
        public DateTime DateTimeLastMod { set; get; }
        public int Rate { set; get; }
        public bool IsPrivate { set; get; }
        public bool IsFavorite { set; get; }
        public string Image{ set; get; }
        public int IDUserLockedBy { set; get; }
        public List<DbNoteDoc> Docs { set; get; }
        //public List<DbNoteSharing> SharedUsers { set; get; }
        public List<string> SharedUsers { set; get; }
        public Int64 IDNoteParent { set; get; }
        public string UserNameParent { set; get; }
        public Int64 IDActivity { set; get; }
    }

    public class DbNoteStatus: DbNote
    {
        // 0: insert, 1: mod, 2: deleted
        public int Status { set; get; }
       
    }
}
