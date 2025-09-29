using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyBlackBoxCore.DataEntities
{
    public class DbNoteSharing
    {
        public long IDNote { set; get; }
        public int IDUser { set; get; }
        public int IDUserSharing { set; get; }
        public string UserNameSharing { set; get; }
        public int IsToRead { set; get; }

        public string Title { set; get; }
        public string Text { set; get; }
        public DateTime DateTimeInserted { set; get; }
        public DateTime DateTimeLastMod { set; get; }
        public int Rate { set; get; }
        public string UserOwner { set; get; }
        public List<DbNoteDoc> Docs { set; get; }
    }
}
