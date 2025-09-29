using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyBlackBoxCore.DataEntities
{
    public class MBBSyncroPending
    {
        public DbNoteDoc DocLocal { set; get; }
        public DbNoteDoc DocRemote { set; get; }
        //-- se in upload ricalcolo versione altrimenti la versione sarà uguale a quella del file remote
        public int  Version { set; get; }
        public string Message { set; get; }
        public string UploadOrDownload { set; get; }
        public string UserOwner { set; get; }
        public bool IsPermitted { set; get; }
        public bool IsCompletedOp { set; get; }
        public DateTime DateTimeSyncro { set; get; }
    }
}
