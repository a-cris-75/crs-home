using CRS.Library;
using MyBlackBoxCore.DataEntities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlackBoxCore
{
    /// <summary>
    /// Classe utile per sincronizzare le note utilizzando i file ini (contengono una sezione e per ogni sezione delle proprietà)
    /// Nella classe, in alternativa alla classe statica MBBSyncroUtility che è statica, vengono istanzati i file, in modo da poterli leggere una sola volta 
    /// e in modo da gestirli più facilmente. 
    /// Ritorna utile nell'aggiornamenro del db, dato che in questo modo posso leggere i file sorgente una sola volta, e importare le note mancanti.
    /// E' indispensabile quando i file sorgente diventano abbastanza grandi da affaticare il sistema.
    /// </summary>
    public class MBBSyncroFilesInstance
    {
        CRSIni fileSyncroDB_UPDNote;
        CRSIni fileSyncroDB_UPDNoteDoc;
        CRSIni fileSyncroDB_UPDNoteDel;
        CRSIni fileSyncroDB_UPDActivitiy;
        CRSIni fileSyncroDB_UPDActivityLog;
        DateTime? lastSYNCRO;
        long startSessionTime;

        public MBBSyncroFilesInstance(string remoteDir, DateTime? lastSyncro, long startSession)
        {
            this.Init(remoteDir, lastSyncro, startSession);
            ReadFiles();
        }

        private void Init(string remoteDir, DateTime? lastSyncro, long startSession)
        {
            this.lastSYNCRO = lastSyncro;
            this.fileSyncroDB_UPDNote = MBBSyncroUtility.GetIniFileSync(MBBConst.SYNCRO_UPD, remoteDir);
            this.fileSyncroDB_UPDNoteDoc = MBBSyncroUtility.GetIniFileSync(MBBConst.SYNCRO_DOCS, remoteDir);
            this.fileSyncroDB_UPDNoteDel = MBBSyncroUtility.GetIniFileSync(MBBConst.SYNCRO_DEL, remoteDir);
            this.fileSyncroDB_UPDActivitiy = MBBSyncroUtility.GetIniFileSync(MBBConst.SYNCRO_ACTIVITIES, remoteDir);
            this.fileSyncroDB_UPDActivityLog = MBBSyncroUtility.GetIniFileSync(MBBConst.SYNCRO_ACTIVITIES_LOG, remoteDir);
            this.startSessionTime = startSession;
        }

        private void ReadFiles()
        {
            if (this.fileSyncroDB_UPDNote.GetSize() > 0)
                this.fileSyncroDB_UPDNote.ReadFileFirst(0);
            if (this.fileSyncroDB_UPDNoteDoc.GetSize() > 0)
                this.fileSyncroDB_UPDNoteDoc.ReadFileFirst(0);
            if (this.fileSyncroDB_UPDNoteDel.GetSize() > 0)
                this.fileSyncroDB_UPDNoteDel.ReadFileFirst(0);
            if (this.fileSyncroDB_UPDActivitiy.GetSize() > 0)
                this.fileSyncroDB_UPDActivitiy.ReadFileFirst(0);
            if (this.fileSyncroDB_UPDActivityLog.GetSize() > 0)
                this.fileSyncroDB_UPDActivityLog.ReadFileFirst(0);
        }

        public int GetNumSectionsFile(int idtypeFile_1UpdNote_2DelNote_3Doc_4Act_5ActLog, DateTime? dtFrom, int intervaldays = 0 )
        {
            int res = 0;
            CRSIni inif = new CRSIni() ;
            switch (idtypeFile_1UpdNote_2DelNote_3Doc_4Act_5ActLog)
            {
                case 1:
                    inif = this.fileSyncroDB_UPDNote;
                    break;
                case 2:
                    inif = this.fileSyncroDB_UPDNoteDel;
                    break;
                case 3:
                    inif = this.fileSyncroDB_UPDNoteDoc;
                    break;
                case 4:
                    inif = this.fileSyncroDB_UPDActivitiy;
                    break;
                case 5:
                    inif = this.fileSyncroDB_UPDActivityLog;
                    break;
            }

            long idtFromTime = 0;
            long idtToTime = DateTime.Now.ToFileTime();
            if (dtFrom != null && dtFrom > DateTime.MinValue)
            {
                idtFromTime = Convert.ToDateTime(dtFrom).ToFileTime();

                if (intervaldays > 0)
                    idtToTime = Convert.ToDateTime(dtFrom).AddDays(intervaldays).ToFileTime();
            }

            res = inif.Lines.Where(X => 
                                X.Length>0 && 
                                X[0].Equals('[') &&
                                X[X.Length-1].Equals(']') &&
                                Utils.IsNumber(X.Substring(1,18)) &&
                                MBBSyncroUtility.GetDTFill(X.Substring(1)) >= idtFromTime && 
                                MBBSyncroUtility.GetDTFill(X.Substring(1)) <= idtToTime).Count();

            return res;
        }

        /*private bool SyncroDBFromFileOwner(string currentUser, DateTime lastSYNCRO, int numdaysInterval, out string infoNotes, out DateTime newdtfromSyncro)
        {
            infoNotes = "";
            MBBSyncroParams p = new MBBSyncroParams();
            p.SharingDir = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_SHARINGDIR);
            p.DropBoxRootDir = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_DROPBOXDIR);
            string remoteDir = MBBSyncroUtility.GetUserRemoteDir(currentUser, false, p);
            bool res = SyncroDBFromFile(lastSYNCRO, numdaysInterval, true, false, out infoNotes, out newdtfromSyncro);
            return res;
        }*/

        public bool SyncroDBFromFile(DateTime fromLastSYNCRO, int numdaysInterval, bool updInIniLastSyncro, bool updInIniLastSyncroEmail, out string infonotes, out DateTime newdtfromSyncro)
        {
            bool res = false;
            infonotes = "";
            newdtfromSyncro = fromLastSYNCRO;
            try
            {
                //DateTime lastSYNCRO = DateTime.FromFileTime(
                //                       Convert.ToInt64(CRSIniFile.GetPropertyValueMBB( APPConstants.PROP_LASTSYNCRO_OWNER, 0)));
                //-- se l'ultima sincronizzazione è avvenuta dopo l'ora di avvio de MBBCore
                //-- non sincronizzare: significa che le note sono state inserite dall'owner attualmente connesso
                if (fromLastSYNCRO == DateTime.MinValue || fromLastSYNCRO.ToFileTime() < this.startSessionTime)
                {
                    MBBSyncroUtility.ReadFileSaveToDBDelNotes(this.fileSyncroDB_UPDNoteDel, fromLastSYNCRO, numdaysInterval);
                    MBBSyncroUtility.ReadFileSaveToDBUpdNotes(this.fileSyncroDB_UPDNote, fromLastSYNCRO, numdaysInterval, out infonotes, out newdtfromSyncro, false);
                    MBBSyncroUtility.ReadFileSaveToDBUpdDocs(this.fileSyncroDB_UPDNoteDoc, fromLastSYNCRO, numdaysInterval);

                    MBBSyncroUtility.ReadFileSaveToDBUpdActivities(this.fileSyncroDB_UPDActivitiy, fromLastSYNCRO, numdaysInterval);
                    MBBSyncroUtility.ReadFileSaveToDBUpdActivitiesLog(this.fileSyncroDB_UPDActivityLog, fromLastSYNCRO, numdaysInterval);

                    DateTime todateSyncro = DateTime.Now;
                    if (numdaysInterval > 0)
                        todateSyncro = fromLastSYNCRO.AddDays(numdaysInterval);
                    if (updInIniLastSyncro)
                        CRSIniFile.SetPropertyValue(APPConstants.SEC_MYBLACKBOX, APPConstants.PROP_LASTSYNCRO_OWNER, Convert.ToDateTime(todateSyncro).ToFileTime());
                    if (updInIniLastSyncroEmail)
                        CRSIniFile.SetPropertyValue(APPConstants.SEC_MYBLACKBOX, APPConstants.PROP_LASTSYNCRO_EMAIL, Convert.ToDateTime(todateSyncro).ToFileTime());

                    res = true;
                }
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                res = false;
            }
            return res;
        }
    }
}
