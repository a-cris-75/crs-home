using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyBlackBoxCore.DataEntities
{
    public class DbNoteDoc
    {
        public Int64 IDNote { set; get; }
        public string DocName { set; get; }
        public int Version { set; get; }
        public DateTime DateTimeLastMod { set; get; }
        public int IDUser { set; get; }
        //-- IDUserNoteOwner=IDUser ma lo esplicito per comodità di lettura
        //public int IDUserNoteOwner { set; get; }
        public string UserNoteOwner { set; get; }
        public string UserMod { set; get; }
        //public string FileNameRemote{ set; get; }
        //-- rappresenta il nome della cartella (che è l'id della nota) contenente il doc su remote server
        //-- è abbinata al path scelto nei parametri 
        public string InternalDirRemote { set; get; }
        public string FileNameLocal { set; get; }
        public bool IsNewFile { set; get; }
    }

    public class DbNoteDocSyncro
    {
        public Int64 IDNote { set; get; }
        public string DocName { set; get; }
        public int Version { set; get; }
        public DateTime DateTimeLastMod { set; get; }
        public int IDUser { set; get; }
        //public int IDUserNoteOwner { set; get; }
        public string UserNoteOwner { set; get; }
        public string UserMod { set; get; }
        public string InternalDirRemote { set; get; }
        public string FileNameLocal { set; get; }
        //public bool IsToSync { set; get; }
        public string Message { set; get; }
        public string UploadOrDownload { set; get; }
        public bool IsPermitted { set; get; }
        public DateTime DateTimeSyncro { set; get; }
    }
}
