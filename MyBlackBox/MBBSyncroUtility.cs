using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRS.Library;
using MyBlackBoxCore.DataEntities;
using System.IO;
using System.Security.AccessControl;
using MimeKit;
using System.Activities.Presentation.Debug;

namespace MyBlackBoxCore
{
    public static class MBBSyncroUtility
    {       
        private static Int64 startSessionTime;

        public static void Init(DateTime startSession)
        {
            startSessionTime = startSession.ToFileTime();
            BackupAndCleanFilesSyncro(startSession);
        }

        #region GET utility
        public static Int64 GetStartSessionTime()
        {
            return startSessionTime;
        }

        /// <summary>
        /// Restituisce la directory in cui cercare i file su cui leggere o scrivere per la sincronizzazione. 
        /// Si usa la convenzione syncro_userowner per esportare le note su file, shared_userowner per le note condivise.
        /// Con DropBox devo ricordare che la directory condivisa (che configuro da dropbox stesso) viene scaricata 
        /// nella radice della cartella locale di dropbox. Quindi per le note condivise cercherò sempre nella dir dropbox.
        /// </summary>
        /// <param name="userowner"></param>
        /// <param name="isShared"></param>
        /// <param name="write0orRead1"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static string GetUserRemoteDir(string userOwnerOrDest, bool isShared, MBBSyncroParams p)
        {
            string res = "";

            // in sostanza:
            // NOTE NORMALI: 
            //  .leggo e scrivo nella dir dropBoxRooDir\SYNCRO_nome utente\
            // NOTE CONDIVISE:
            //  .leggo dalla dir SharingDir\SHARED_mio nome utente: sono note eventualemte scritte da altri utenti per me
            //  .scrivo su dir SharingDir\SHARED_nome utente con cui condivido\

            if (!isShared)
                res = Path.Combine(p.DropBoxRootDir, "SYNCRO_" + userOwnerOrDest);
            else
                // se condivido la nota userowner è l'utente destinatario
                res = Path.Combine(p.SharingDir, "SHARED_" + userOwnerOrDest);
                  
            return res;
        }

        public static string GetUserRemoteDir_DEPRECATED(string userowner, bool isShared, int write0orRead1, MBBSyncroParams p)
        {
            string res = "";

            switch (write0orRead1)
            {

                case 0:
                    if (!isShared)
                        res = Path.Combine(p.SharingDir, "SYNCRO_" + userowner);//Path.Combine(userFilePath, "Application.ini");
                    else res = Path.Combine(p.DropBoxRootDir, "SHARED_" + userowner);
                    break;

                case 1:
                    //-- caso lettura: usando drop box devo andare a leggere dalla cartella che ho ereditato come ospite:
                    //-- tale cartella si posiziona nella cartella di dropbox (nella radice) col nome della condivisione
                    //-- ES: se condivido la cartella \DropBox\MyBlackBox\shared_a.cris.75@gmail.com sull'ospite verrà copiata la cartella:
                    //-- \DropBox\shared_a.cris.75@gmail.com
                    if (!isShared)
                        res = Path.Combine(p.SharingDir, "SYNCRO_" + userowner);
                    else
                    {
                        //if (string.IsNullOrEmpty(p.DropBoxRootDir) == false && p.DropBoxRootDir.Substring(p.DropBoxRootDir.Length - 1, 1) == @"\")
                        //    p.DropBoxRootDir = p.DropBoxRootDir.Substring(0, p.DropBoxRootDir.Length - 2);
                        res = Path.Combine(p.DropBoxRootDir, "SHARED_" + userowner);
                    }
                    break;
            }

            return res;
        }

        public static CRSIni GetIniFileSync(int typesyncro, string remoteUserDir)
        {
            try
            {
                string dir = remoteUserDir;
                string syncroname = GetNameFileSync(typesyncro, remoteUserDir);
            
                if (Directory.Exists(dir) == false)
                {
                    Directory.CreateDirectory(dir);                
                }
                
                if (File.Exists(syncroname) == false)
                {
                    File.Create(syncroname).Dispose();
                }
                CRSIni fileSyncroDB = new CRSIni(syncroname);

                return fileSyncroDB;
            }
            catch(Exception ex)
            {
                CRSLog.WriteLog("GetIniFileSync: " + ex.Message, "");
                return null;
            }
            
        }

        public static string GetNameFileSync(int typesyncro, string remoteUserDir)
        {
            string syncroname = "";
            try
            {
                string dir = remoteUserDir;

                syncroname = Path.Combine(dir, MBBConst.FILE_NAME_UPD);// "\\syncroDbUPD" +".txt";
                if (typesyncro == MBBConst.SYNCRO_INS)
                    syncroname = Path.Combine(dir, MBBConst.FILE_NAME_INS);
                if (typesyncro == MBBConst.SYNCRO_DEL)
                    syncroname = Path.Combine(dir, MBBConst.FILE_NAME_DEL);// + "\\syncroDbDEL.txt";
                if (typesyncro == MBBConst.SYNCRO_DOCS)
                    syncroname = Path.Combine(dir, MBBConst.FILE_NAME_DOCS);// + "\\syncroDbDOCS.txt";
                //-- questo file è generico, riguarda tutti gli utenti, non il singolo
                if (typesyncro == MBBConst.SYNCRO_USERS)
                    syncroname = Path.Combine(dir, MBBConst.FILE_NAME_USERS);// + "\\syncroDbUSERS.txt";
                if (typesyncro == MBBConst.SYNCRO_ACTIVITIES)
                    syncroname = Path.Combine(dir, MBBConst.FILE_NAME_ACT);// + "\\syncroDbACTIVITIES.txt";
                if (typesyncro == MBBConst.SYNCRO_ACTIVITIES_LOG)
                    syncroname = Path.Combine(dir, MBBConst.FILE_NAME_ACTLOG);// + "\\syncroDbACTIVITIESLOG.txt";
                if (typesyncro == MBBConst.SYNCRO_ACTIVITIES_NOTES)
                    syncroname = Path.Combine(dir, MBBConst.FILE_NAME_ACTNOTES);// + "\\syncroDbACTIVITIESNOTES.txt";
            }
            catch { }
            return syncroname;
        }

        private static string GetUserOwner(string sec)
        {
            return sec.Substring(sec.IndexOf(':') + 1);
        }

        /// <summary>
        /// Recupera l'id da una stringa formattata in questo modo:
        ///     data in formato long (ToFileTime)-id(nota o activity) in formato long:user owner (es. email address)
        /// </summary>
        /// <param name="sec"></param>
        /// <returns></returns>
        private static long GetIDFromSectionStd(string sec)
        {
            int startidx = sec.IndexOf('-') + 1;
            int endidx = sec.IndexOf(':');
            int len = endidx - startidx;
            return Convert.ToInt64(sec.Substring(startidx, len));
        }

        public static Int64 GetDTFill(string sec)
        {
            return Convert.ToInt64(sec.Substring(0, 18));
        }
        /// <summary>
        /// Compone una stringa che identifica la sezione su file ini.
        /// Utile per scrivere l'id di una nota su file da sincronizzare in modo che possa essere recuperato facilmente univocamente
        /// </summary>
        /// <param name="dtfill"></param>
        /// <param name="idnote"></param>
        /// <param name="userowner"></param>
        /// <returns></returns>
        public static string GetSectionName(DateTime dtfill, Int64 idnote, string userowner)
        {
            return dtfill.ToFileTime().ToString() + "-" + idnote.ToString() + ":" + userowner;
        }
        #endregion

        #region UTILITIES
        public static void DeleteNoteFromUPDFile(Int64 idnote, string userowner, MBBSyncroParams p)
        {
            try
            { // non ho il metodo corrispettivo shared dato che l'elimiazione vale sempre per tutti
                string remoteDir = GetUserRemoteDir(userowner, false, p);
                CRSIni fileSyncroDB = GetIniFileSync(MBBConst.SYNCRO_UPD, remoteDir);

                if (fileSyncroDB.ReadFileFirst(0))
                {
                    string sec = "-" + idnote.ToString() + ":" + userowner;
                    fileSyncroDB.RemovePartialSectionsReadFile(sec);
                    fileSyncroDB.WriteFileEnd();
                }
            }
            catch { }
        }
        #endregion

        #region SYNCRO WRITE Files: scrittura file di testo per interscambio

        /// <summary>
        /// Scrive nel file di sincronizzazione una nota con sezione (section) sempre diversa in modo da non sovrascrivere una nota.
        /// Passo il parametro sec dato che questo metodo deve andare a scrivere nella nota appena inserita
        /// </summary>
        /// <param name="typesyncro"></param>
        /// <param name="idnote"></param>
        /// <param name="idx"></param>
        /// <param name="filename"></param>
        /// <param name="intdirremote"></param>
        /// <param name="version"></param>
        /// <param name="user"></param>
        /// <param name="userowner"></param>
        /// <param name="remoteDir"></param>
       public static bool WriteDocSyncroFile(DateTime dtfill, Int64 idnote, int idx, string filename, string intdirremote, int version
            , string userowner, MBBSyncroParams p)
        {
            string remoteDir = GetUserRemoteDir(userowner, false, p);
            CRSIni fileSyncroDB = GetIniFileSync(MBBConst.SYNCRO_DOCS, remoteDir);
            bool res1 = WriteDocSyncroFileNewFile(dtfill, idnote, idx, filename, intdirremote, version, userowner, null, fileSyncroDB);

            remoteDir = Path.GetTempPath();
            CRSIni fileSyncroDBEmail = GetIniFileSync(MBBConst.SYNCRO_DOCS, remoteDir);
            bool res2 = WriteDocSyncroFileNewFile(dtfill, idnote, idx, filename, intdirremote, version, userowner, null, fileSyncroDBEmail);

            return res1 && res2;
        }

        private static bool WriteDocSyncroFileNewFile(DateTime dtfill, long idnote, int idx, string filename, string intdirremote, int version
            , string userowner, List<string> usersSh, CRSIni fileSyncroDB)
        {
            bool res;
            try
            {
                string users = "";
                if(usersSh!=null && usersSh.Count>0) 
                    users = String.Join(", ", usersSh.ToArray());

                if (fileSyncroDB.ReadFileFirst(0))
                {
                    string sec = GetSectionName(dtfill, idnote, userowner);
                    DateTime dtMod = System.IO.File.GetLastWriteTime(filename);
                    fileSyncroDB.SetValueReadFile(sec, "usersdest", users);
                    fileSyncroDB.SetValueReadFile(sec, "doc" + idx.ToString(), filename);
                    fileSyncroDB.SetValueReadFile(sec, "internaldirremote" + idx.ToString(), intdirremote);
                    fileSyncroDB.SetValueReadFile(sec, "version" + idx.ToString(), version.ToString());
                    fileSyncroDB.SetValueReadFile(sec, "datetimeLastMod" + idx.ToString(), dtMod.ToFileTime().ToString());
                    fileSyncroDB.WriteFileEnd();
                }
                res = true;
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("Error WriteDocSyncroFileNewFile: " + ex.Message, "");
                res = false;
            }

            return res;
        }
        /*
        /// <summary>
        /// Scrive una nota condivisa all'utente destinatario (cioè nel file dell'utente destinatario).
        /// La nota ha come userowner comunque il mandante nell'intestazione
        /// </summary>
        public static bool WriteDocSyncroFileShared(DateTime dtfill, Int64 idnote, int idx, string filename, string intdirremote, int version,string userowner, List<string> usersSh, MBBSyncroParams p)
        {
            bool res = true;
            if (usersSh.Count > 0)
            {
                foreach (string dest in usersSh)
                {
                    //string remoteDir = GetUserRemoteDir(userowner, true, 0, p);
                    // cerco la dir condivisa dell'utente destinatario da cui poi leggerà
                    string remoteDir = GetUserRemoteDir(dest, true, p);
                    CRSIni fileSyncroDB = GetIniFileSync(MBBConst.SYNCRO_DOCS, remoteDir);
                    res = WriteDocSyncroFileNewFile(dtfill, idnote, idx, filename, intdirremote, version, userowner, usersSh, fileSyncroDB);
                    //try
                    //{
                    //    
                    //    if (fileSyncroDB.ReadFileFirst(0))
                    //    {
                    //        string users = String.Join(", ", usersSh.ToArray());
                    //        string sec = GetSectionName(dtfill, idnote, userowner);
                    //        DateTime dtMod = System.IO.File.GetLastWriteTime(filename);
                    //
                    //        fileSyncroDB.SetValueReadFile(sec, "usersdest", users);
                    //        fileSyncroDB.SetValueReadFile(sec, "doc" + idx.ToString(), filename);
                    //        fileSyncroDB.SetValueReadFile(sec, "internaldirremote" + idx.ToString(), intdirremote);
                    //        fileSyncroDB.SetValueReadFile(sec, "version" + idx.ToString(), version.ToString());
                    //        fileSyncroDB.SetValueReadFile(sec, "datetimeLastMod" + idx.ToString(), dtMod.ToFileTime().ToString());
                    //        fileSyncroDB.WriteFileEnd();
                    //    }
                    //}
                    //catch (Exception ex)
                    //{
                    //    CRSLog.WriteLog("Error FillDocSyncroDBFile: " + ex.Message, "");
                    //    res = false;
                    //}
                }
            }
            return res;
        }*/

        public static bool WriteDocsSyncroFile(Int64 idnote, DateTime dtFill, string userowner, List<string> usersSh, List<DbNoteDoc> docs, MBBSyncroParams p)
        {
            string remoteDir = GetUserRemoteDir(userowner, false, p);
            //-- devo scrivere l'info sul file dell'iduser, ma anche sul file degli utenti con cui condivido la nota
            CRSIni fileSyncroDB = GetIniFileSync(MBBConst.SYNCRO_DOCS, remoteDir);
            bool res1 = WriteDocsSyncroFileNewFile(idnote, dtFill, userowner, usersSh, docs, fileSyncroDB);

            remoteDir = Path.GetTempPath();
            CRSIni fileSyncroDBEmail = GetIniFileSync(MBBConst.SYNCRO_DOCS, remoteDir);
            bool res2 = WriteDocsSyncroFileNewFile(idnote, dtFill, userowner, usersSh, docs, fileSyncroDBEmail);

            return res1 && res2;
        }

        /// <summary>
        /// Riempie il file di sincronizzazione per le note e i documenti associati alle note. 
        /// Ricerca la nota, elimina i documenti e li riaggiunge.
        /// Usata dopo cancellazione documenti di una nota.
        /// </summary>
        /// <param name="typesyncro"></param>
        /// <param name="iduser"></param>
        /// <param name="docs"></param>
        /// <param name="par"></param>
        public static bool WriteDocsSyncroFileNewFile(Int64 idnote, DateTime dtFill, string userowner, List<string> usersSh, List<DbNoteDoc> docs, CRSIni fileSyncroDB)
        {
            bool res = true;
            //string remoteDir = GetUserRemoteDir(userowner, false, p);
            //-- devo scrivere l'info sul file dell'iduser, ma anche sul file degli utenti con cui condivido la nota
            //CRSIni fileSyncroDB = GetIniFileSync(MBBConst.SYNCRO_DOCS, remoteDir);
            docs = docs.OrderBy(X => X.IDUser).ToList();
            try
            {
                string users = "";
                if (usersSh != null && usersSh.Count > 0)
                    users = String.Join(", ", usersSh.ToArray());
                if (fileSyncroDB.ReadFileFirst(0))
                {
                    int idx = 0;
                    string sec = GetSectionName(dtFill, idnote, userowner);
                    fileSyncroDB.SetValueReadFile(sec, "usersdest" , users);
                    // devo rimuovere per la nota tutti i documenti associati per poi reinserirli aggiornati
                    // serve per pulire il file dai vecchi inserimenti / modifiche
                    string partialsecname = idnote.ToString() + ":" + userowner;
                    fileSyncroDB.RemovePartialSections(partialsecname, dtFill);

                    foreach (DbNoteDoc dd in docs)
                    {                        
                        fileSyncroDB.SetValueReadFile(sec.ToString(), "doc" + idx.ToString(), dd.FileNameLocal);
                        fileSyncroDB.SetValueReadFile(sec.ToString(), "internaldirremote" + idx.ToString(), dd.InternalDirRemote);
                        fileSyncroDB.SetValueReadFile(sec.ToString(), "version" + idx.ToString(), dd.Version.ToString());
                        fileSyncroDB.SetValueReadFile(sec.ToString(), "datetimeLastMod" + idx.ToString(), dd.DateTimeLastMod.ToFileTime().ToString());
                        idx++;
                    }

                    fileSyncroDB.WriteFileEnd();
                    res = true;
                }
            }
            catch (Exception ex)
            {
                res = false;
                CRSLog.WriteLog("Error FillDocsSyncroFile: " + ex.Message, "");
            }
            return res;
        }

        /// <summary>
        /// Riempie il file di sincronizzazione per le note e i documenti associati alle note. 
        /// Ricerca la nota, elimina i documenti e li riaggiunge.
        /// Usata dopo cancellazione documenti di una nota.
        /// </summary>
        /// <param name="typesyncro"></param>
        /// <param name="iduser"></param>
        /// <param name="docs"></param>
        /// <param name="par"></param>
        public static bool WriteDocsSyncroFileShared(Int64 idnote, DateTime dtFill, string userowner, List<DbNoteDoc> docs, List<string> usersSh, MBBSyncroParams p)
        {
            bool res = true;
            if (usersSh.Count > 0)
            {
                foreach (string dest in usersSh)
                {
                    string remoteDir = GetUserRemoteDir(dest, true, p);
                    //-- devo scrivere l'info sul file dell'iduser, ma anche sul file degli utenti con cui condivido la nota
                    CRSIni fileSyncroDB = GetIniFileSync(MBBConst.SYNCRO_DOCS, remoteDir);
                    res = WriteDocsSyncroFileNewFile(idnote, dtFill, userowner, usersSh, docs, fileSyncroDB);

                    //docs = docs.OrderBy(X => X.IDUser).ToList();
                    //try
                    //{
                    //    if (fileSyncroDB.ReadFileFirst(0))
                    //    {
                    //        int idx = 0;
                    //        string users = String.Join(", ", usersSh.ToArray());
                    //
                    //        string sec = GetSectionName(dtFill, idnote, userowner);
                    //        fileSyncroDB.SetValue(sec.ToString(), "usersdest", users);
                    //
                    //        foreach (DbNoteDoc dd in docs)
                    //        {
                    //            fileSyncroDB.SetValueReadFile(sec.ToString(), "doc" + idx.ToString(), dd.FileNameLocal);
                    //            fileSyncroDB.SetValueReadFile(sec.ToString(), "internaldirremote" + idx.ToString(), dd.InternalDirRemote);
                    //            fileSyncroDB.SetValueReadFile(sec.ToString(), "version" + idx.ToString(), dd.Version.ToString());
                    //            fileSyncroDB.SetValueReadFile(sec.ToString(), "datetimeLastMod" + idx.ToString(), dd.DateTimeLastMod.ToFileTime().ToString());
                    //            idx++;
                    //        }
                    //        fileSyncroDB.WriteFileEnd();
                    //    }
                    //}
                    //catch (Exception ex)
                    //{
                    //    CRSLog.WriteLog("Error FillDocsSyncroFile: " + ex.Message, "");
                    //}
                }
            }
            return res;
        }
       
        public static bool WriteDelNoteSyncroFile(DateTime dtFill, Int64 idnote, string userowner, MBBSyncroParams p)
        {
            bool res1 = true;
            bool res2 = true;
            try
            {
                // non ho il metodo corrispettivo shared dato che l'eliminazione vale sempre per tutti
                string remoteDir = GetUserRemoteDir(userowner, false, p);
                CRSIni fileSyncroDB = GetIniFileSync(MBBConst.SYNCRO_DEL, remoteDir);
                res1 = WriteDelNoteSyncroFileNewFile(dtFill, idnote, userowner, fileSyncroDB);
               
                // non ho il metodo corrispettivo shared dato che l'eliminazione vale sempre per tutti
                remoteDir = Path.GetTempPath();
                CRSIni fileSyncroDBEmail = GetIniFileSync(MBBConst.SYNCRO_DEL, remoteDir);
                res2 = WriteDelNoteSyncroFileNewFile(dtFill, idnote, userowner, fileSyncroDBEmail);
            }
            catch { res1 = false; }
            return res1 && res2;
        }

        private static bool WriteDelNoteSyncroFileNewFile(DateTime dtFill, Int64 idnote, string userowner, CRSIni fileSyncroDB)
        {
            bool res = true;
            
            try
            {
                // non ho il metodo corrispettivo shared dato che l'eliminazione vale sempre per tutti
                //string remoteDir = GetUserRemoteDir(userowner, false, p);
                
                string sec = GetSectionName(dtFill, idnote, userowner);
                fileSyncroDB.SetSection(sec);
                //-- ripulisco il file dalle vecchie istanze della nota già cancellata precedentemente
                string partialsecname = idnote.ToString() + ":" + userowner;
                fileSyncroDB.RemovePartialSections(partialsecname, dtFill);
            }
            catch { res = false; }
            return res;
        }

        public static bool WriteDelNoteSyncroFileShared(DateTime dtFill, Int64 idnote, string userowner, string userdest, MBBSyncroParams p)
        {
            bool res = true;
            try
            {
                // non ho il metodo corrispettivo shared dato che l'elimiazione vale sempre per tutti
                string remoteDir = GetUserRemoteDir(userdest, true, p);
                CRSIni fileSyncroDB = GetIniFileSync(MBBConst.SYNCRO_DEL, remoteDir);
                string sec = GetSectionName(dtFill, idnote, userowner);
                fileSyncroDB.SetSection(sec);
                //-- ripulisco il file dalle vecchie istanze della nota già cancellata precedentemente
                string partialsecname = idnote.ToString() + ":" + userowner;
                fileSyncroDB.RemovePartialSections(partialsecname, dtFill);
            }
            catch { res = false; }
            return res;
        }

        /// <summary>
        /// Inserisce o aggiorna una nota nel file di testo per la sincronizzazione del singolo utente owner.
        /// </summary>
        /// <param name="typesyncro"></param>
        /// <param name="idnote"></param>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="rate"></param>
        /// <param name="dtIns"></param>
        /// <param name="idUser"></param>
        /// <param name="user"></param>
        /// <param name="par"></param>        
        public static bool WriteNoteSyncroFile(Int64 idnote, string title, string text, int rate, DateTime dtIns, DateTime dtHeaderNote
                                                , string userowner
                                                , Int64 idnoteparent, string userparentnote
                                                , bool isFavorite
                                                , MBBSyncroParams p)
        {
            bool res1 = true;
            bool res2 = true;
            try
            {
                string remoteDir = GetUserRemoteDir(userowner, false, p);
                CRSIni fileSyncroDB = GetIniFileSync(MBBConst.SYNCRO_UPD, remoteDir);
                res1 = WriteNoteSyncroFileNewFile(idnote, title, text, rate, dtIns, dtHeaderNote
                                                , userowner, idnoteparent, userparentnote
                                                , isFavorite, fileSyncroDB
                                                );

                remoteDir = Path.GetTempPath();
                CRSIni fileSyncroDBEmail = GetIniFileSync(MBBConst.SYNCRO_UPD, remoteDir);
                res2 = WriteNoteSyncroFileNewFile(idnote, title, text, rate, dtIns, dtHeaderNote
                                                , userowner, idnoteparent, userparentnote
                                                , isFavorite, fileSyncroDBEmail
                                                );
            }
            catch { res1 = false; }
            return res1 && res2;
        }

        /// <summary>
        /// Inserisce nota su file di testo da condividere con altri utenti
        /// </summary>
        /// <param name="idnote"></param>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="rate"></param>
        /// <param name="dtIns"></param>
        /// <param name="dtHeaderNote"></param>
        /// <param name="userowner"></param>
        /// <param name="usersSh"></param>
        /// <param name="idnoteparent"></param>
        /// <param name="userparentnote"></param>
        /// <param name="p"></param>
        public static bool WriteNoteSyncroFileShared(Int64 idnote, string title, string text, int rate, DateTime dtIns, DateTime dtHeaderNote
                                                , string userowner
                                                , List<string> usersSh
                                                , Int64 idnoteparent, string userparentnote
                                                , bool isFavorite
                                                , MBBSyncroParams p)
        {
            bool res = true;
            try
            {
                if (usersSh.Count > 0)
                {
                    foreach (string dest in usersSh)
                    {
                        string remoteDir = GetUserRemoteDir(dest, true, p);
                        CRSIni fileSyncroDB = GetIniFileSync(MBBConst.SYNCRO_UPD, remoteDir);
                        WriteNoteSyncroFileNewFile(idnote, title, text, rate, dtIns, dtHeaderNote
                                                , userowner
                                                , idnoteparent, userparentnote
                                                , isFavorite, fileSyncroDB
                                                );
                    }
                }
            }
            catch { res = false; }
            return res;
        }


        public static bool WriteNoteSyncroFileNewFile(Int64 idnote, string title, string text, int rate, DateTime dtIns, DateTime dtHeaderNote
                                                , string userowner, Int64 idnoteparent, string userparentnote
                                                , bool isFavorite
                                                , CRSIni fileSyncroDB)
        {
            bool res = true;           
            try
            {
                if (fileSyncroDB.ReadFileFirst(0))
                {

                    title = title.Replace('[', '(');
                    title = title.Replace(']', ')');

                    string sec = GetSectionName(dtHeaderNote, idnote, userowner);

                    //-- è utile per poter ricominciare a fare update dall'ultima data memorizzata nei parametri senza rifare tutto
                    fileSyncroDB.SetValueReadFile(sec, "dateop", DateTime.Now.ToFileTime());
                    fileSyncroDB.SetValueReadFile(sec, "title", title);
                    //fileSyncroDB.SetValue(idnote.ToString(), "iduser", idUser.ToString());
                    fileSyncroDB.SetValueReadFile(sec, "userowner", Convert.ToString(userowner));

                    fileSyncroDB.SetValueReadFile(sec, "idnoteparent", idnoteparent);
                    fileSyncroDB.SetValueReadFile(sec, "usernameparent", userparentnote);

                    fileSyncroDB.SetValueReadFile(sec, "rate", rate.ToString());
                    fileSyncroDB.SetValueReadFile(sec, "datetimeIns", dtIns.ToFileTime().ToString());

                    //-- col nuovo sistema inserisco sempre nuove sezioni essendo il nome della sezione univoco
                    fileSyncroDB.SetValueReadFile(sec, "text", CRSIni.CHARS_TEXT_BEGIN + text + CRSIni.CHARS_TEXT_END);
                    fileSyncroDB.SetValueReadFile(sec, "isfavorite", Convert.ToString(isFavorite));

                    //-- devo eliminare le sezioni precedentemente inserite riferite all'idnote e user owner dato che ormai
                    //-- sono obsolete: serve per ripulire il file 
                    string partialsecname = idnote.ToString() + ":" + userowner;
                    fileSyncroDB.RemovePartialSectionsReadFile(partialsecname, GetDTFill(sec));

                    fileSyncroDB.WriteFileEnd();
                }
            }
            catch { res = false; }
            return res;
        }

        public static bool WriteActivitySyncroFile(DbActivity act, string userowner
                                               , MBBSyncroParams p)
        {
            bool res1 = true;
            bool res2 = true;
            try
            {
                string remoteDir = GetUserRemoteDir(userowner, false, p);
                CRSIni fileSyncroDB = GetIniFileSync(MBBConst.SYNCRO_ACTIVITIES, remoteDir);
                res1 = WriteActivitySyncroFileNewFile(userowner, act, fileSyncroDB);

                remoteDir = Path.GetTempPath();
                CRSIni fileSyncroDBEmail = GetIniFileSync(MBBConst.SYNCRO_ACTIVITIES, remoteDir);
                res2 = WriteActivitySyncroFileNewFile(userowner, act, fileSyncroDBEmail);
            }
            catch { res1 = false; }
            return res1 && res2;
        }
        public static bool WriteActivitySyncroFileNewFile(string userowner
                                                , DbActivity act
                                                , CRSIni fileSyncroDB)
        {
            bool res = true;
            
            try
            {
                if (fileSyncroDB.ReadFileFirst(0))
                {

                    act.TitleActivity = act.TitleActivity.Replace('[', '(');
                    act.TitleActivity = act.TitleActivity.Replace(']', ')');
                    // 
                    //string sec = GetSectionName(dtHeaderAct, act.IDActivity, userowner);
                    string sec = GetSectionName(act.DateTimeLastMod, act.IDActivity, userowner);

                    //-- è utile per poter ricominciare a fare update dall'ultima data memorizzata nei parametri senza rifare tutto
                    fileSyncroDB.SetValueReadFile(sec, "dateop", DateTime.Now.ToFileTime());
                    fileSyncroDB.SetValueReadFile(sec, "titleactivity", act.TitleActivity);
                    //fileSyncroDB.SetValue(idnote.ToString(), "iduser", idUser.ToString());
                    fileSyncroDB.SetValueReadFile(sec, "userowner", Convert.ToString(userowner));

                    fileSyncroDB.SetValueReadFile(sec, "idactivity", act.IDActivity);
                    
                    fileSyncroDB.SetValueReadFile(sec, "status", act.Status);
                    fileSyncroDB.SetValueReadFile(sec, "typeactivity", act.TypeActivity);
                    if (act.DateTimeInserted <= DateTime.MinValue)
                        act.DateTimeInserted = DateTime.Now;
                    fileSyncroDB.SetValueReadFile(sec, "datetimeinserted", act.DateTimeInserted.ToFileTime().ToString());
                    if(act.StartActivity>DateTime.MinValue)
                        fileSyncroDB.SetValueReadFile(sec, "stopactivity", act.StopActivity.ToFileTime().ToString());

                    //-- col nuovo sistema inserisco sempre nuove sezioni essendo il nome della sezione univoco
                    fileSyncroDB.SetValueReadFile(sec, "textactivity", CRSIni.CHARS_TEXT_BEGIN + act.TextActivity + CRSIni.CHARS_TEXT_END);
                    fileSyncroDB.SetValueReadFile(sec, "totminpreview", Convert.ToString(act.TotMinPreview));

                    //-- devo eliminare le sezioni precedentemente inserite riferite all'idnote e user owner dato che ormai
                    //-- sono obsolete: serve per ripulire il file 
                    string partialsecname = act.IDActivity.ToString() + ":" + userowner;
                    fileSyncroDB.RemovePartialSectionsReadFile(partialsecname, GetDTFill(sec));

                    fileSyncroDB.WriteFileEnd();
                }
            }
            catch { res = false; }
            return res;
        }

        public static bool WriteActivityLogSyncroFile(string typeopUPDDEL, DateTime startActKey, DbActivityLog act, string userowner
                                               , MBBSyncroParams p)
        {
            bool res1 = true;
            bool res2 = true;
            try
            {
                string remoteDir = GetUserRemoteDir(userowner, false, p);
                CRSIni fileSyncroDB = GetIniFileSync(MBBConst.SYNCRO_ACTIVITIES_LOG, remoteDir);
                res1 = WriteActivityLogSyncroFileNewFile(typeopUPDDEL, userowner, startActKey, act, fileSyncroDB);

                remoteDir = remoteDir = Path.GetTempPath();
                CRSIni fileSyncroDBEmail = GetIniFileSync(MBBConst.SYNCRO_ACTIVITIES_LOG, remoteDir);
                res2 = WriteActivityLogSyncroFileNewFile(typeopUPDDEL, userowner, startActKey, act, fileSyncroDBEmail);
            }
            catch { res1 = false; }
            return res1 && res2;
        }
        public static bool WriteActivityLogSyncroFileNewFile(string typeopUPDDEL, string userowner, DateTime startActKey
                                                , DbActivityLog log , CRSIni fileSyncroDB)
        {
            bool res = true;
            //fileSyncroDB = GetIniFileSync(MBBConst.SYNCRO_ACTIVITIES_LOG, remoteDir);
            try
            {
                if (fileSyncroDB.ReadFileFirst(0))
                {
                    //(log.StartActivityLog==DateTime.MinValue)
                    string sec = GetSectionName(startActKey, log.IDActivity, userowner);

                    //-- è utile per poter ricominciare a fare update dall'ultima data memorizzata nei parametri senza rifare tutto
                    fileSyncroDB.SetValueReadFile(sec, "dateop", DateTime.Now.ToFileTime());
                    fileSyncroDB.SetValueReadFile(sec, "typeop", typeopUPDDEL);
                    fileSyncroDB.SetValueReadFile(sec, "StartActivityLog", startActKey.ToFileTime().ToString());
                    fileSyncroDB.SetValueReadFile(sec, "NewStartActivityLog", log.StartActivityLog.ToFileTime().ToString());
                    fileSyncroDB.SetValueReadFile(sec, "NewStopActivityLog", log.StopActivityLog.ToFileTime().ToString());
                    fileSyncroDB.SetValueReadFile(sec, "TitleActivity", log.TitleActivity);
                    fileSyncroDB.SetValueReadFile(sec, "userowner", Convert.ToString(userowner));
                    fileSyncroDB.SetValueReadFile(sec, "idactivity", log.IDActivity);


                    // NB: devo trovare un'altr strsategia altrimenti cancello sezioni modificate precedentemetne non ancora importate


                    //-- devo eliminare le sezioni precedentemente inserite riferite all'idactivity e startdate (chiave) e user owner dato che ormai
                    //-- sono obsolete: serve per ripulire il file 
                    //string partialsecname = startActKey.ToFileTime().ToString() + "-" +  log.IDActivity.ToString() + ":" + userowner;
                    //fileSyncroDB.RemovePartialSectionsReadFile(partialsecname, GetDTFill(sec));

                    fileSyncroDB.WriteFileEnd();
                }
            }
            catch { res = false; }
            return res;
        }
        #endregion

        #region SYNCRO READ Files: lettura da file di testo e scrittura su DB
        /// <summary>
        /// Sincronizza la tabella degli utenti
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool SyncroDBFromFileUsers(string user)
        {
            //-- Si tratta di sincronizzarsi con file FileSyncroDB_USERS che contiene l'immagine della tabella del db
            //-- Le info fondamentali sono: ID, Name, Password, WorkGroup 
            //-- ALLINEAMENTO: dato che ci possono essere inserimenti, aggiornamenti(di pwd per es) o cancellazioni
            //-- devo poter distinguere i diversi casi
            //-- considerando il fatto che i record saranno sempre limitati (gli utenti non saranno migliaia) posso eseguire queste 
            //-- operazioni frequentemente

            //-- FORMATO FILE
            //-- [ID] => sezione con id numerico, trattino, lettera identificativo del tipo di operazione: 
            //-- campi:  operation(I:insert, U: Update, D: del),name,pwd,group,lang, dateinsert(nel caso in ins), dateupdate         
            //-- la sezione deve essere univoca 
            //-- PROBLEMA DELETE: se elimino e reinserisco uno user potrei avere difficoltà ad allinearmi dato che una sezione potrebbe 
            //-- essere ripetuta
            //-- SOLUZIONE:  a seconda della licenza (che ogni utente dovrà inserire in ogni dispositivo) dovrei conoscere il numero
            //-- tot di utenti, inoltre posso specificare la data di inserimento e la dat di ultima modifica
            //-- Quindi il file potrebbe avere n sezioni fisse (= num utenti) e all'interno l'informazione se l'utente è attivo (status DELETED se cancellato)
            //-- Con questi dati dovrei avere tutto ciò che mi serve per fare allineamento corretto

            MBBSyncroParams p = new MBBSyncroParams();
            p.SharingDir = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_SHARINGDIR);
            p.DropBoxRootDir = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_DROPBOXDIR);
            string remoteDir = GetUserRemoteDir(user, false, p);
            CRSIni fileSyncroDB_USERS = GetIniFileSync(MBBConst.SYNCRO_USERS, remoteDir);

            bool res = true;
            int idx = 0;
            if (fileSyncroDB_USERS.ReadFileFirst(0))
            {
                string secid = fileSyncroDB_USERS.GetNextSectionReadFile(0, ref idx);
                while (string.IsNullOrEmpty(secid) == false)
                {
                    int id = Convert.ToInt32(secid);
                    //-- indica operazione di tipo: insert(I),update(U),delete(D)
                    string operation = fileSyncroDB_USERS.GetValueReadFile(idx, secid, "operation").ToString();
                    //string status = this.FileSyncroDB_USERS.GetValue(id.ToString(), "status").ToString();
                    string name = fileSyncroDB_USERS.GetValueReadFile(idx, secid, "name").ToString();
                    string pwd = fileSyncroDB_USERS.GetValueReadFile(idx, secid, "pwd").ToString();
                    string group = fileSyncroDB_USERS.GetValueReadFile(idx, secid, "group").ToString();
                    string lang = fileSyncroDB_USERS.GetValueReadFile(idx, secid, "lang").ToString();

                    object o = fileSyncroDB_USERS.GetValueReadFile(idx, secid, "datetimeIns");
                    DateTime dtins = DateTime.MinValue;
                    if (o != null)
                        dtins = DateTime.FromFileTime(Convert.ToInt64(o));

                    o = fileSyncroDB_USERS.GetValueReadFile(idx, secid, "datetimeUpd");
                    DateTime dtupd = DateTime.MinValue;
                    if (o != null)
                        dtupd = DateTime.FromFileTime(Convert.ToInt64(o));

                    bool res2 = DbDataUpdate.InsUpdDelUser(id, name, pwd, lang, group, dtins, dtupd, operation);

                    secid = fileSyncroDB_USERS.GetNextSectionReadFile(idx + 1, ref idx);
                }
            }
            return res;
        }

        /// <summary>
        /// Sincronizza le note dell'utente Owner (di connessione)
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public static bool SyncroDBFromFileOwner(string currentUser, DateTime lastSYNCRO, int numdaysInterval, out string infoNotes, out DateTime newdtfromSyncro)
        {
            infoNotes = "";
            MBBSyncroParams p = new MBBSyncroParams();
            p.SharingDir = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_SHARINGDIR);
            p.DropBoxRootDir = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_DROPBOXDIR);
            string remoteDir = GetUserRemoteDir(currentUser, false, p);

            bool res = SyncroDBFromFile(remoteDir, lastSYNCRO, numdaysInterval, true, false, out infoNotes, out newdtfromSyncro);            
            return res;
        }

        private static bool SyncroDBFromFile(string remoteDir, DateTime fromLastSYNCRO, int numdaysInterval, bool updInIniLastSyncro, bool updInIniLastSyncroEmail, out string infonotes, out DateTime newdtfromSyncro)
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
                if (fromLastSYNCRO==DateTime.MinValue || fromLastSYNCRO.ToFileTime() < startSessionTime)
                {
                    ReadFileSaveToDBDelNotes(remoteDir, fromLastSYNCRO, numdaysInterval);
                    ReadFileSaveToDBUpdNotes(remoteDir, fromLastSYNCRO, numdaysInterval, out infonotes, out newdtfromSyncro, false);
                    ReadFileSaveToDBUpdDocs(remoteDir, fromLastSYNCRO, numdaysInterval);

                    ReadFileSaveToDBUpdActivities(remoteDir, fromLastSYNCRO, numdaysInterval);
                    ReadFileSaveToDBUpdActivitiesLog(remoteDir, fromLastSYNCRO, numdaysInterval);

                    DateTime todateSyncro = DateTime.Now;
                    if (numdaysInterval > 0)
                        todateSyncro = fromLastSYNCRO.AddDays(numdaysInterval);
                    if (updInIniLastSyncro)
                        CRSIniFile.SetPropertyValue(APPConstants.SEC_MYBLACKBOX, APPConstants.PROP_LASTSYNCRO_OWNER, Convert.ToDateTime (todateSyncro).ToFileTime());
                    if (updInIniLastSyncroEmail)
                        CRSIniFile.SetPropertyValue(APPConstants.SEC_MYBLACKBOX, APPConstants.PROP_LASTSYNCRO_EMAIL, Convert.ToDateTime(todateSyncro).ToFileTime());

                    res = true;
                }
            }
            catch(Exception ex)
            {
                string err = ex.Message;
                res = false;
            }
            return res;
        }

        /// <summary>
        /// Per ogni utente con cui condivido cerco nuove note che sono state messe a disposizione nella mia cartella di condivisione. 
        /// dal 10/05/2014: 
        /// 1) l'utente owner scrive nelle cartelle SHARED_utenteDest 
        /// 2) la cartella deve essere condivisa con altri utenti da drop box
        /// 3) l'utente "destinatario" deve prendersi la briga di controllare la sua cartella SHARED_.. per verificare la presenza di nuove note condivise 
        /// </summary>
        /// <param name="shUsers"></param>
        /// <returns></returns>
        public static bool SyncroDBFromFileShared(string currentUser)
        {
            DateTime lastSYNCRO = DateTime.FromFileTime(
                                   Convert.ToInt64(CRSIniFile.GetPropertyValueMBB(
                                                       APPConstants.PROP_LASTSYNCRO_SHARED, 0)));

            MBBSyncroParams p = new MBBSyncroParams();
            p.SharingDir = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_SHARINGDIR);
            p.DropBoxRootDir = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_DROPBOXDIR);
            string remoteDir = GetUserRemoteDir(currentUser, true, p);
            string info;
            DateTime newdtSyncro;
            bool res1 = ReadFileSaveToDBDelNotes(remoteDir, lastSYNCRO.AddMinutes(-5), 0);
            bool res2 = ReadFileSaveToDBUpdNotes(remoteDir, lastSYNCRO.AddMinutes(-5), 0, out info, out newdtSyncro, false);
            bool res3 = ReadFileSaveToDBUpdDocs(remoteDir, lastSYNCRO.AddMinutes(-5), 0);

            CRSIniFile.SetPropertyValue(APPConstants.SEC_MYBLACKBOX, APPConstants.PROP_LASTSYNCRO_SHARED, DateTime.Now.ToFileTime());
            //return res;

            return res1 && res2 && res3;
        }

        /// <summary>
        /// Leggo le note condivise senza salvarle
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public static List<DbNote> SyncroGetNotesFromFileShared(string currentUser, DateTime dtFrom, int numdaysInterval)
        {
            MBBSyncroParams p = new MBBSyncroParams();
            p.SharingDir = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_SHARINGDIR);
            p.DropBoxRootDir = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_DROPBOXDIR);
            string remoteDir = GetUserRemoteDir(currentUser, true, p);

            DateTime newdtFrom;
            CRSIni fileSyncroDB_UPD = GetIniFileSync(MBBConst.SYNCRO_UPD, remoteDir);
            List<DbNote> res = ReadFileGetNotes(fileSyncroDB_UPD, dtFrom, numdaysInterval, out newdtFrom);
            return res;
        }

        public static List<string> GetSharedUsersFromDir(string currentUser)
        {
            List<string> res = new List<string>();

            string dropBoxRootDir = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_DROPBOXDIR);

            List<string> dirs = Directory.GetDirectories(dropBoxRootDir).ToList();
            dirs = dirs.Where(X => X.Contains("SHARED_")).ToList();

            foreach (string d in dirs)
            {
                string usr = d.Substring(d.LastIndexOf("SHARED_") + 7);
                if(usr!=currentUser)
                    res.Add(usr);
            }
            return res;
        }

        private static bool ReadFileSaveToDBDelNotes(string remoteUserDir, DateTime lastSYNCRO, int numdaysInterval)
        {
            bool res = false;
            CRSIni fileSyncroDB_DEL = GetIniFileSync(MBBConst.SYNCRO_DEL, remoteUserDir);
            if (fileSyncroDB_DEL != null && fileSyncroDB_DEL.ReadFileFirst(0))
                res = ReadFileSaveToDBDelNotes(fileSyncroDB_DEL, lastSYNCRO, numdaysInterval);
            /*bool res = false;
            int idx = 0;

            try
            {
                Int64 idtFromTime = 0;
                Int64 idtToTime = DateTime.Now.ToFileTime();
                if (lastSYNCRO != null && lastSYNCRO > DateTime.MinValue)
                {
                    idtFromTime = Convert.ToDateTime(lastSYNCRO).ToFileTime();
                    if (numdaysInterval>0)
                        idtToTime = Convert.ToDateTime(lastSYNCRO).AddDays(numdaysInterval).ToFileTime();
                }
                if (fileSyncroDB_DEL!=null && fileSyncroDB_DEL.ReadFileFirst(0))
                {
                    string secid = fileSyncroDB_DEL.GetNextSectionReadFile(0, ref idx);
                    while (string.IsNullOrEmpty(secid) == false)
                    {
                        Int64 id = GetIDFromSectionStd(secid);
                        Int64 idtFillNote = GetDTFill(secid);
                        string userowner = GetUserOwner(secid);

                        if (idtFromTime == 0)
                        {
                            // è possibile che la data iniziale non sia impostata (per esempio al primo avvio dell'applicazione)
                            idtFromTime = idtFillNote - 1;
                            if (numdaysInterval > 0)
                                idtToTime = DateTime.FromFileTime(idtFillNote).AddDays(numdaysInterval).ToFileTime();
                        }

                        //-- elimino la nota dello userowner che non è necessariamente il currentUser
                        //-- che potrebbe essere l'utente con cui condivido la nota
                        //if (lastSYNCRO==DateTime.MinValue || idtFillNote > lastSYNCRO.ToFileTime())
                        if (idtFillNote > idtFromTime && idtFillNote <= idtToTime)
                        {
                            int iduser = DbDataAccess.GetIDUser(userowner);
                            res = DbDataUpdate.DeleteNote(id, iduser, false);
                        }
                        secid = fileSyncroDB_DEL.GetNextSectionReadFile(idx + 1, ref idx);
                    }
                    res = true;
                }
            }
            catch { }*/
            return res;
        }

        public static bool ReadFileSaveToDBDelNotes(CRSIni fileSyncroDB_DEL, DateTime lastSYNCRO, int numdaysInterval)
        {
            bool res = false;
            int idx = 0;

            try
            {
                Int64 idtFromTime = 0;
                Int64 idtToTime = DateTime.Now.ToFileTime();
                if (lastSYNCRO != null && lastSYNCRO > DateTime.MinValue)
                {
                    idtFromTime = Convert.ToDateTime(lastSYNCRO).ToFileTime();
                    if (numdaysInterval > 0)
                        idtToTime = Convert.ToDateTime(lastSYNCRO).AddDays(numdaysInterval).ToFileTime();
                }
                if (fileSyncroDB_DEL.Lines.Count() == 0)
                    fileSyncroDB_DEL.ReadFileFirst(0);

                idx = GetNextIdxSectionReadFile(fileSyncroDB_DEL, idtFromTime);
                
                if (idx >= 0)
                {
                    //if (fileSyncroDB_DEL != null )//&& fileSyncroDB_DEL.ReadFileFirst(0))
                    //{
                    string secid = fileSyncroDB_DEL.GetNextSectionReadFile(idx, ref idx);
                    while (string.IsNullOrEmpty(secid) == false)
                    {
                        Int64 id = GetIDFromSectionStd(secid);
                        Int64 idtFillNote = GetDTFill(secid);
                        string userowner = GetUserOwner(secid);

                        if (idtFromTime == 0)
                        {
                            // è possibile che la data iniziale non sia impostata (per esempio al primo avvio dell'applicazione)
                            idtFromTime = idtFillNote - 1;
                            if (numdaysInterval > 0)
                                idtToTime = DateTime.FromFileTime(idtFillNote).AddDays(numdaysInterval).ToFileTime();
                        }

                        //-- elimino la nota dello userowner che non è necessariamente il currentUser
                        //-- che potrebbe essere l'utente con cui condivido la nota
                        //if (lastSYNCRO==DateTime.MinValue || idtFillNote > lastSYNCRO.ToFileTime())
                        if (idtFillNote > idtFromTime && idtFillNote <= idtToTime)
                        {
                            int iduser = DbDataAccess.GetIDUser(userowner);
                            res = DbDataUpdate.DeleteNote(id, iduser, false);
                        }
                        else 
                            break;
                        secid = fileSyncroDB_DEL.GetNextSectionReadFile(idx + 1, ref idx);
                    }
                    
                    res = true;
                }
            }
            catch { }
            return res;
        }

        /// <summary>
        /// A partire da una directory e usando i nomi configurati dei file, legge da file le note e le inserisce nel DB.
        /// Con insOnlyNewID = true, inserisce solo le note con id diversi per evitare sovrascritture: serve se voglio importsre note vecchie senza preoccupazioni.
        /// Dato che all'inizio l'ID non era basato su DateTime al secondo.
        /// </summary>
        /// <param name="remoteUserDir"></param>
        /// <param name="lastSYNCRO"></param>
        /// <param name="numdaysInterval"></param>
        /// <param name="infonotes"></param>
        /// <param name="newdtFrom"></param>
        /// <param name="insOnlyNewID"></param>
        /// <returns></returns>
        private static bool ReadFileSaveToDBUpdNotes(string remoteUserDir, DateTime? lastSYNCRO, int numdaysInterval, out string infonotes, out DateTime newdtFrom
            , bool insOnlyNewID)
        {
            bool res = false;
            infonotes = "";
            if (lastSYNCRO != null)
                newdtFrom = Convert.ToDateTime(lastSYNCRO);
            else newdtFrom = DateTime.MinValue;
            try
            {
                CRSIni fileSyncroDB_UPD = GetIniFileSync(MBBConst.SYNCRO_UPD, remoteUserDir);
                if (fileSyncroDB_UPD != null && fileSyncroDB_UPD.GetSize()>0 && fileSyncroDB_UPD.ReadFileFirst(0))
                    res = ReadFileSaveToDBUpdNotes(fileSyncroDB_UPD, lastSYNCRO, numdaysInterval, out infonotes, out newdtFrom, insOnlyNewID);
            }
            catch (Exception ex)
            {
                res = false;
                string s = ex.Message;
            }

            return res;
        }

        /// <summary>
        /// Legge da file le note e le inserisce nel DB.
        /// Con insOnlyNewID = true, inserisce solo le note con id diversi per evitare sovrascritture: serve se voglio importare note vecchie senza preoccupazioni.
        /// Dato che all'inizio l'ID non era basato su DateTime al secondo.
        /// </summary>
        /// <param name="fileSyncroDB_UPD"></param>
        /// <param name="lastSYNCRO"></param>
        /// <param name="numdaysInterval"></param>
        /// <param name="infonotes"></param>
        /// <param name="newdtFrom"></param>
        /// <param name="insOnlyNewID"></param>
        /// <returns></returns>
        public static bool ReadFileSaveToDBUpdNotes(CRSIni fileSyncroDB_UPD, DateTime? lastSYNCRO, int numdaysInterval, out string infonotes, out DateTime newdtFrom
            , bool insOnlyNewID)
        {
            bool res = false;
            infonotes = "";
            if (lastSYNCRO != null)
                newdtFrom = Convert.ToDateTime(lastSYNCRO);
            else newdtFrom = DateTime.MinValue;
            try
            {
                List<DbNote> lst = ReadFileGetNotes(fileSyncroDB_UPD, lastSYNCRO, numdaysInterval, out newdtFrom);
                infonotes = lst.Count().ToString();
                foreach (DbNote n in lst)
                {
                    Int64 newid = 0;
                    int iduserParentNote = DbDataAccess.GetIDUser(n.UserNameParent);

                    string textcontent = Utils.GetTextContentRTF(n.Text);
                    bool isToSave = (!string.IsNullOrEmpty(n.Title) || !string.IsNullOrEmpty(textcontent));

                    if (isToSave)
                    {
                        List<DbNote> lstNotesIns;
                        List<DbNote> lstNotesDel;
                        newid = DbDataUpdate.InsertUpdateNoteWithSplit(n.ID
                            , n.Title, n.Text, n.IDUser, 0, n.Rate, n.IsFavorite
                            , n.DateTimeInserted, n.DateTimeLastMod
                            , false
                            , n.Docs, (n.SharedUsers.Count > 0)
                            , n.UserNameParent, out lstNotesIns, out lstNotesDel, insOnlyNewID);
                    }
                }
                res = true;
            }
            catch (Exception ex)
            {
                res = false;
                string s = ex.Message;
            }

            return res;
        }

        private static List<DbNote> ReadFileGetNotes(CRSIni fileSyncroDB_UPD, DateTime? dtFrom, int numdaysInterval, out DateTime newdtFrom)
        {
            int idx = 0;

            List<DbNote> res1 = new List<DbNote>();

            Int64 idtFromTime = 0;
            Int64 idtToTime = DateTime.Now.ToFileTime();
            newdtFrom = DateTime.MinValue; 

            if (dtFrom != null && dtFrom > DateTime.MinValue)
            {
                newdtFrom = Convert.ToDateTime(dtFrom);
                idtFromTime = Convert.ToDateTime(dtFrom).ToFileTime();
                if (numdaysInterval > 0)
                    idtToTime = Convert.ToDateTime(dtFrom).AddDays(numdaysInterval).ToFileTime();
            }

            try
            {
                // dovrebbe essere superfluo
                if (fileSyncroDB_UPD.Lines.Count() == 0)
                    fileSyncroDB_UPD.ReadFileFirst(0);

                idx = GetNextIdxSectionReadFile(fileSyncroDB_UPD, idtFromTime);
                //string secid = fileSyncroDB_UPD.GetNextSectionReadFile(0, ref idx);
                if (idx >= 0)
                {
                    string secid = fileSyncroDB_UPD.GetNextSectionReadFile(idx, ref idx);
                    while (string.IsNullOrEmpty(secid) == false)
                    {
                        //DbNote note = new DbNote();

                        Int64 nextnote = GetIDFromSectionStd(secid);
                        Int64 idtFillNote = GetDTFill(secid);
                        DateTime dt = DateTime.FromFileTime(idtFillNote);
                        if (idtFromTime == 0)
                        {
                            // è possibile che la data iniziale non sia impostata (per esempio al primo avvio dell'applicazione)
                            newdtFrom = dt;
                            idtFromTime = idtFillNote - 1;
                            if (numdaysInterval > 0)
                                idtToTime = newdtFrom.AddDays(numdaysInterval).ToFileTime();
                        }

                        if (idtFillNote > idtFromTime && idtFillNote <= idtToTime)
                        {
                            DbNote note = new DbNote();

                            string title = Convert.ToString(fileSyncroDB_UPD.GetValueReadFile(idx, secid, "title"));
                            string text = Convert.ToString(fileSyncroDB_UPD.GetMultipleLineDelimitedReadFile(idx, secid, "text"));
                            string userowner = Convert.ToString(fileSyncroDB_UPD.GetValueReadFile(idx, secid, "userowner"));
                            if (string.IsNullOrEmpty(userowner))
                                userowner = GetUserOwner(secid);

                            object val = fileSyncroDB_UPD.GetValueReadFile(idx, secid, "idnoteparent");
                            Int64 idnoteparent = (val == null || string.IsNullOrEmpty(Convert.ToString(val).Trim())) ? 0
                                : Convert.ToInt64(val);
                            string usernameparent = fileSyncroDB_UPD.GetValueReadFile(idx, secid, "usernameparent") == null ? ""
                                : Convert.ToString(fileSyncroDB_UPD.GetValueReadFile(idx, secid, "usernameparent"));

                            int rate = Convert.ToInt32(fileSyncroDB_UPD.GetValueReadFile(idx, secid, "rate"));
                            val = fileSyncroDB_UPD.GetValueReadFile(idx, secid, "isfavorite");
                            bool isfavorite = false;
                            if (val != null)
                            {
                                if (Convert.ToString(val).ToLower() == "true" || Convert.ToString(val).ToLower() == "false")
                                    isfavorite = Convert.ToBoolean(val);
                                else
                                    if (Utils.IsNumber(Convert.ToString(val)))
                                    isfavorite = (Convert.ToInt32(val) == 1) ? true : false;
                            }

                            int iduser = DbDataAccess.GetIDUser(userowner);
                            //int iduserSh = DbDataAccess.GetIDUser(currentUser);
                            int iduserParentNote = DbDataAccess.GetIDUser(usernameparent);

                            DateTime dt1 = dt;
                            string o = fileSyncroDB_UPD.GetValueReadFile(idx, secid, "datetimeLastMod");
                            if (!string.IsNullOrEmpty(o) && Utils.IsNumber(o))
                                dt1 = DateTime.FromFileTime(Convert.ToInt64(o));

                            DateTime dt2 = dt;
                            string oo = fileSyncroDB_UPD.GetValueReadFile(idx, secid, "datetimeIns");
                            if (!string.IsNullOrEmpty(oo) && Utils.IsNumber(oo))
                                dt2 = DateTime.FromFileTime(Convert.ToInt64(oo));

                            if (dt1 > DateTime.MinValue)
                                dt = dt1;
                            if (dt2 < dt1)
                                dt = dt2;

                            note.ID = nextnote;
                            note.IDNoteParent = idnoteparent;
                            note.IDUser = iduser;
                            note.DateTimeLastMod = dt1;
                            note.DateTimeInserted = dt2;
                            note.IsFavorite = isfavorite;
                            note.IsPrivate = false;
                            note.Rate = rate;
                            note.SharedUsers = Convert.ToString(fileSyncroDB_UPD.GetValueReadFile(idx, secid, "usersdest")).Split(',', ';').ToList();
                            note.Text = text;
                            note.Title = title;
                            note.UserNameParent = usernameparent;

                            res1.Add(note);

                            //if (DbDataAccess.ExistsNote(nextnote, iduser))
                            //    res = DbDataUpdate.UpdateNote(nextnote, title, text, iduser, iduserParentNote, idnoteparent, usernameparent, rate, dt1, dt2, false, false, isfavorite);
                            //else newid = DbDataUpdate.InsertNote(nextnote, title, text, iduser, iduserParentNote, idnoteparent, usernameparent, rate, dt);
                        }
                        else 
                            break;

                        secid = fileSyncroDB_UPD.GetNextSectionReadFile(idx + 1, ref idx);
                    }
                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }

            return res1;
        }

        /// <summary>
        /// Cerco nell'header della nota il primo l'id sec (l'id della sezione rappresentato dalla dato di inserimento o modifica della nota su file di testo)
        /// Il file contenente le note, ha le sezioni ordinate per data di inserimento o modifica della nota stessa: in questo modo sono sicuro che dall'indice in poi
        /// le note saranno più recenti
        /// </summary>
        /// <param name="secidcontains"></param>
        /// <returns></returns>
        public static int GetNextIdxSectionReadFile(CRSIni fileSyncroDB_UPD, long idtFromTime)
        {
            int newidx = -1;
            try
            {
                //if ((!line[0].Equals("[") && !line[line.Length-1].Equals("]")) || charsComment.Contains(line[0]))
                newidx = fileSyncroDB_UPD.Lines.FindIndex(X => X.Length>0 && X[0].Equals('[') && X[X.Length-1].Equals(']') &&
                                GetDTFill(X.Substring(1, X.Length-2)) >= idtFromTime);
            }
            catch { }
            return newidx;
        }

        private static List<DbNote> ReadFileDocGetNotes(CRSIni fileSyncroDB_DOCS, DateTime dtFrom, DateTime dtTo)
        {
            int idx = 0;

            List<DbNote> res1 = new List<DbNote>();
            Int64 idtFromTime = 0;
            if (dtFrom>DateTime.MinValue)
                idtFromTime = dtFrom.ToFileTime();
            Int64 idtToTime = DateTime.MaxValue.ToFileTime();
            if (dtTo<DateTime.MaxValue)
                idtToTime = dtTo.ToFileTime();
            try
            {
                //if (fileSyncroDB_DOCS.ReadFileFirst(0))
                if (fileSyncroDB_DOCS.Lines.Count() == 0)
                    fileSyncroDB_DOCS.ReadFileFirst(0);
                //{
                DbNote note;
                idx = GetNextIdxSectionReadFile(fileSyncroDB_DOCS, idtFromTime);
                //string secid = fileSyncroDB_UPD.GetNextSectionReadFile(0, ref idx);
                if (idx >= 0)
                {
                    string secid = fileSyncroDB_DOCS.GetNextSectionReadFile(idx, ref idx);
                    while (string.IsNullOrEmpty(secid) == false)
                    {
                        Int64 nextnote = GetIDFromSectionStd(secid);
                        Int64 idtFillNote = GetDTFill(secid);
                        DateTime dt = DateTime.FromFileTime(idtFillNote);

                        if (idtFillNote > idtFromTime && idtFillNote <= idtToTime)
                        {
                            if (res1.Where(X => X.ID == nextnote).Count() == 0)
                            {
                                note = new DbNote();
                                note.ID = nextnote;
                                note.Docs = new List<DbNoteDoc>();
                            }
                            else note = res1.Where(X => X.ID == nextnote).First();


                            List<string> sectionLines = fileSyncroDB_DOCS.GetSectionLinesReadFile(idx, secid);
                            List<string> docs = sectionLines.Where(X => X.Contains("doc")).ToList();
                            List<string> dirs = sectionLines.Where(X => X.Contains("internaldirremote")).ToList();
                            List<string> vers = sectionLines.Where(X => X.Contains("version")).ToList();
                            List<string> dates = sectionLines.Where(X => X.Contains("datetimeLastmod")).ToList();
                            int i = 0;
                            foreach (string filepathname in docs)
                            {
                                DbNoteDoc doc = new DbNoteDoc();

                                if (dirs.Count > i) doc.InternalDirRemote = dirs[i].Substring(dirs[i].IndexOf('=', 0) + 1);
                                if (vers.Count > i) doc.Version = Convert.ToInt32(vers[i].Substring(vers[i].IndexOf('=', 0) + 1).Trim());
                                if (dates.Count > i) doc.DateTimeLastMod = DateTime.FromFileTime(Convert.ToInt64((dates[i].Substring(dates[i].IndexOf('=', 0) + 1).Trim())));

                                string filenameTrim = filepathname;
                                if (filepathname.Contains('='))
                                    filenameTrim = filepathname.Substring(filepathname.IndexOf('=') + 1);

                                doc.DocName = System.IO.Path.GetFileName(filenameTrim);
                                doc.FileNameLocal = filenameTrim;
                                note.Docs.Add(doc);
                                i++;
                            }
                        }
                        else
                            break;

                        secid = fileSyncroDB_DOCS.GetNextSectionReadFile(idx + 1, ref idx);
                    }
                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }

            return res1;
        }

        private static bool ReadFileSaveToDBUpdDocs(string remoteUserDir, DateTime lastSYNCRO, int numdaysInterval)
        {
            CRSIni fileSyncroDB = GetIniFileSync(MBBConst.SYNCRO_DOCS, remoteUserDir);
            bool res = false;
            
            Int64 idtFromTime = 0;
            Int64 idtToTime = DateTime.Now.ToFileTime();
            if (lastSYNCRO != null && lastSYNCRO > DateTime.MinValue)
            {
                idtFromTime = Convert.ToDateTime(lastSYNCRO).ToFileTime();
                if (numdaysInterval>0)
                    idtToTime = Convert.ToDateTime(lastSYNCRO).AddDays(numdaysInterval).ToFileTime();
            }

            if (fileSyncroDB!=null && fileSyncroDB.ReadFileFirst(0))
            {
                ReadFileSaveToDBUpdDocs(fileSyncroDB, lastSYNCRO, numdaysInterval);
                /*
                int idx = 0; 
                string secid = fileSyncroDB.GetNextSectionReadFile(0, ref idx);

                while (string.IsNullOrEmpty(secid) == false)
                {
                    Int64 nextnote = GetIDFromSectionStd(secid);
                    Int64 idtFillNote = GetDTFill(secid);

                    if (idtFromTime == 0)
                    {
                        // è possibile che la data iniziale non sia impostata (per esempio al primo avvio dell'applicazione)
                        idtFromTime = idtFillNote - 1;
                        if (numdaysInterval > 0)
                            idtToTime = DateTime.FromFileTime(idtFillNote).AddDays(numdaysInterval).ToFileTime();
                    }
                    try
                    {
                        //if (lastSYNCRO==DateTime.MinValue || idtFillNote > lastSYNCRO.ToFileTime())
                        if (idtFillNote > idtFromTime && idtFillNote <= idtToTime)
                        {
                            string userowner = GetUserOwner(secid);
                            int iduser = DbDataAccess.GetIDUser(userowner);
                            int i = 0;

                            List<string> sectionLines = fileSyncroDB.GetSectionLinesReadFile(idx, secid);
                            List<string> docs = sectionLines.Where(X => X.Contains("doc")).ToList();
                            List<string> dirs = sectionLines.Where(X => X.Contains("internaldirremote")).ToList();
                            List<string> vers = sectionLines.Where(X => X.Contains("version")).ToList();
                            List<string> dates = sectionLines.Where(X => X.Contains("datetimeLastmod")).ToList();

                            foreach (string filename in docs)
                            {
                                string intdir = "";
                                int ver = 0;
                                DateTime dt = DateTime.Now;
                                string dirname = System.IO.Path.GetDirectoryName(filename);
                                string docname = System.IO.Path.GetFileName(filename);

                                if (dirs.Count > i) intdir = dirs[i].Substring(dirs[i].IndexOf('=', 0) + 1);
                                if (vers.Count > i) ver = Convert.ToInt32(vers[i].Substring(vers[i].IndexOf('=', 0) + 1).Trim());
                                if (dates.Count > i) dt = DateTime.FromFileTime(Convert.ToInt64((dates[i].Substring(dates[i].IndexOf('=', 0) + 1).Trim())));

                                string filenameTrim = filename;
                                if(filename.Contains('='))
                                    filenameTrim = filename.Substring(filename.IndexOf('=') + 1);

                                bool res2 = DbDataUpdate.InsertDoc(nextnote, docname, ver, dt, intdir, filenameTrim, iduser, true);
                                i++;
                            }
                        }
                        res = true;
                    }
                    catch { res = false; }
                    secid = fileSyncroDB.GetNextSectionReadFile(idx + 1, ref idx);
                }*/
            }
            return res;
        }

        public static bool ReadFileSaveToDBUpdDocs(CRSIni fileSyncroDB, DateTime lastSYNCRO, int numdaysInterval)
        {
            //CRSIni fileSyncroDB = GetIniFileSync(MBBConst.SYNCRO_DOCS, remoteUserDir);
            bool res = false;
            int idx = 0;

            Int64 idtFromTime = 0;
            Int64 idtToTime = DateTime.Now.ToFileTime();
            if (lastSYNCRO != null && lastSYNCRO > DateTime.MinValue)
            {
                idtFromTime = Convert.ToDateTime(lastSYNCRO).ToFileTime();
                if (numdaysInterval > 0)
                    idtToTime = Convert.ToDateTime(lastSYNCRO).AddDays(numdaysInterval).ToFileTime();
            }
            if (fileSyncroDB.Lines.Count() == 0)
                fileSyncroDB.ReadFileFirst(0);
            idx = GetNextIdxSectionReadFile(fileSyncroDB, idtFromTime);
            //string secid = fileSyncroDB_UPD.GetNextSectionReadFile(0, ref idx);
            if (idx >= 0)
            {
                string secid = fileSyncroDB.GetNextSectionReadFile(idx, ref idx);

                while (string.IsNullOrEmpty(secid) == false)
                {
                    Int64 nextnote = GetIDFromSectionStd(secid);
                    Int64 idtFillNote = GetDTFill(secid);

                    if (idtFromTime == 0)
                    {
                        // è possibile che la data iniziale non sia impostata (per esempio al primo avvio dell'applicazione)
                        idtFromTime = idtFillNote - 1;
                        if (numdaysInterval > 0)
                            idtToTime = DateTime.FromFileTime(idtFillNote).AddDays(numdaysInterval).ToFileTime();
                    }
                    try
                    {
                        //if (lastSYNCRO==DateTime.MinValue || idtFillNote > lastSYNCRO.ToFileTime())
                        if (idtFillNote > idtFromTime && idtFillNote <= idtToTime)
                        {
                            string userowner = GetUserOwner(secid);
                            int iduser = DbDataAccess.GetIDUser(userowner);
                            int i = 0;

                            List<string> sectionLines = fileSyncroDB.GetSectionLinesReadFile(idx, secid);
                            List<string> docs = sectionLines.Where(X => X.Contains("doc")).ToList();
                            List<string> dirs = sectionLines.Where(X => X.Contains("internaldirremote")).ToList();
                            List<string> vers = sectionLines.Where(X => X.Contains("version")).ToList();
                            List<string> dates = sectionLines.Where(X => X.Contains("datetimeLastmod")).ToList();

                            foreach (string filename in docs)
                            {
                                string intdir = "";
                                int ver = 0;
                                DateTime dt = DateTime.Now;
                                string dirname = System.IO.Path.GetDirectoryName(filename);
                                string docname = System.IO.Path.GetFileName(filename);

                                if (dirs.Count > i) intdir = dirs[i].Substring(dirs[i].IndexOf('=', 0) + 1);
                                if (vers.Count > i) ver = Convert.ToInt32(vers[i].Substring(vers[i].IndexOf('=', 0) + 1).Trim());
                                if (dates.Count > i) dt = DateTime.FromFileTime(Convert.ToInt64((dates[i].Substring(dates[i].IndexOf('=', 0) + 1).Trim())));

                                string filenameTrim = filename;
                                if (filename.Contains('='))
                                    filenameTrim = filename.Substring(filename.IndexOf('=') + 1);

                                bool res2 = DbDataUpdate.InsertDoc(nextnote, docname, ver, dt, intdir, filenameTrim, iduser, true);
                                i++;
                            }
                        }
                        else
                            break;

                        res = true;
                    }
                    catch { res = false; }
                    secid = fileSyncroDB.GetNextSectionReadFile(idx + 1, ref idx);
                }
               
            }
            return res;
        }

        public static string MakeFolderNameUser(Int64 idnote, string user)
        {
            //-- Ver 1: idnote_iduser
            //return "ID" + idnote.ToString().PadLeft(CRSGlobalParams.MAX_NUMDEC_NOTES, '0') + "_" + iduser.ToString().PadLeft(5, '0');
            //-- Ver 2: idnote_user
            //return "ID" + idnote.ToString().PadLeft(CRSGlobalParams.MAX_NUMDEC_NOTES, '0') + "_" + user;
            //-- Ver 3: user\\idnote
            return user + "\\ID" + idnote.ToString().PadLeft(CRSGlobalParams.MAX_NUMDEC_NOTES, '0');
        }

        public static string MakeFolderNameForDocs(string userOwner, bool isSharedNote, Int64 idnote = -1, bool useIDNote = false)
        {
            // ATTENZIONE: ho scelto di copiare il file in ogni cartella diversa (una per nota) dato che 
            // con lo stesso nome di file potrei avere file provenienti da due cartelle diverse (e quindi potenzialmente diversi)

            // per modificare questa logica, quando copio, dovrei assicurarmi che il file proviene dalla stessa origine e sovrascriverlo,
            // altrimenti dovrei creare una copia con nome diverso (ci agguiungo un progressivo)
            // VANTAGGIO: non ho copie inutili in cartelle diverse
            // SVANTAGGIO: gestione più complicata al momento dell'inserimento della nota
            string res = "";
            if (useIDNote && idnote>0)
            {
               res = "DOCS\\ID" + idnote.ToString().PadLeft(CRSGlobalParams.MAX_NUMDEC_NOTES, '0');
                if (!isSharedNote)
                    res = userOwner + "\\ID" + idnote.ToString().PadLeft(CRSGlobalParams.MAX_NUMDEC_NOTES, '0');
            }
            else
            {
                res = "DOCS";
                if (!isSharedNote)
                    res = userOwner;
            }
            return res;
        }

        private static List<DbActivity> ReadFileGetActivities(CRSIni fileSyncroDB_UPD, DateTime dtFrom, int numdaysInterval, out string userowner)
        {
            int idx = 0;
            List<DbActivity> res1 = new List<DbActivity>();    
            userowner = "";
            try
            {
                Int64 idtFromTime = 0;
                Int64 idtToTime = DateTime.Now.ToFileTime();
                if (dtFrom > DateTime.MinValue)
                {
                    idtFromTime = dtFrom.ToFileTime();
                    if (numdaysInterval>0)
                        idtToTime = Convert.ToDateTime(dtFrom).AddDays(numdaysInterval).ToFileTime();
                }
                if (fileSyncroDB_UPD.Lines.Count() == 0)
                    fileSyncroDB_UPD.ReadFileFirst(0);

                idx = GetNextIdxSectionReadFile(fileSyncroDB_UPD, idtFromTime);
                if (idx >= 0)
                {
                    string secid = fileSyncroDB_UPD.GetNextSectionReadFile(idx, ref idx);
                    while (string.IsNullOrEmpty(secid) == false)
                    {
                        DbActivity act = new DbActivity();

                        Int64 nextactivity = GetIDFromSectionStd(secid);
                        Int64 idtFillAct = GetDTFill(secid);
                        DateTime dt = DateTime.FromFileTime(idtFillAct);

                        if (idtFromTime == 0)
                        {
                            // è possibile che la data iniziale non sia impostata (per esempio al primo avvio dell'applicazione)
                            idtFromTime = idtFillAct - 1;
                            if (numdaysInterval > 0)
                                idtToTime = dt.AddDays(numdaysInterval).ToFileTime();
                        }

                        if (idtFillAct > idtFromTime && idtFillAct <= idtToTime)
                        {
                            string title = Convert.ToString(fileSyncroDB_UPD.GetValueReadFile(idx, secid, "titleactivity"));
                            string text = Convert.ToString(fileSyncroDB_UPD.GetMultipleLineDelimitedReadFile(idx, secid, "textactivity"));

                            string status = Convert.ToString(fileSyncroDB_UPD.GetValueReadFile(idx, secid, "status"));
                            string typeact = Convert.ToString(fileSyncroDB_UPD.GetValueReadFile(idx, secid, "typeactivity"));
                            int totminprev = Convert.ToInt32(fileSyncroDB_UPD.GetValueReadFile(idx, secid, "totminpreview"));

                            userowner = Convert.ToString(fileSyncroDB_UPD.GetValueReadFile(idx, secid, "userowner"));
                            if (string.IsNullOrEmpty(userowner))
                                userowner = GetUserOwner(secid);

                            int iduser = DbDataAccess.GetIDUser(userowner);

                            DateTime dtstop = dt;
                            string o = fileSyncroDB_UPD.GetValueReadFile(idx, secid, "stopactivity");
                            if (!string.IsNullOrEmpty(o) && Utils.IsNumber(o))
                                dtstop = DateTime.FromFileTime(Convert.ToInt64(o));

                            DateTime dtins = dt;
                            string oo = fileSyncroDB_UPD.GetValueReadFile(idx, secid, "datetimeinserted");
                            if (!string.IsNullOrEmpty(oo) && Utils.IsNumber(oo))
                                dtins = DateTime.FromFileTime(Convert.ToInt64(oo));

                            if (dtstop > DateTime.MinValue)
                                dt = dtstop;
                            if (dtins < dtstop)
                                dt = dtins;

                            act.IDActivity = nextactivity;
                            act.Status = status;
                            act.TypeActivity = typeact;
                            act.StopActivity = dt;
                            act.DateTimeInserted = dtins;
                            act.TotMinPreview = totminprev;
                            act.TitleActivity = title;
                            act.TextActivity = text;
                            act.IDUser = iduser;

                            res1.Add(act);

                            //if (DbDataAccess.ExistsNote(nextnote, iduser))
                            //    res = DbDataUpdate.UpdateNote(nextnote, title, text, iduser, iduserParentNote, idnoteparent, usernameparent, rate, dt1, dt2, false, false, isfavorite);
                            //else newid = DbDataUpdate.InsertNote(nextnote, title, text, iduser, iduserParentNote, idnoteparent, usernameparent, rate, dt);
                        }
                        else
                            break;

                        secid = fileSyncroDB_UPD.GetNextSectionReadFile(idx + 1, ref idx);
                    }
                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }

            return res1;
        }

        private static bool ReadFileSaveToDBUpdActivities(string remoteUserDir, DateTime lastSyncro, int numdaysInterval)
        {
            bool res = false;

            CRSIni fileSyncroDB = GetIniFileSync(MBBConst.SYNCRO_ACTIVITIES, remoteUserDir);
            if (fileSyncroDB != null && fileSyncroDB.ReadFileFirst(0))
                res = ReadFileSaveToDBUpdActivities(fileSyncroDB, lastSyncro, numdaysInterval);
            /*try
            {
                string userowner;
                List<DbActivity> lst = ReadFileGetActivities(fileSyncroDB_UPD, lastSyncro, numdaysInterval, out userowner);

                foreach (DbActivity n in lst)
                {
                    Int64 newid = 0;

                    bool isToSave = (!string.IsNullOrEmpty(n.TitleActivity) || !string.IsNullOrEmpty(n.TextActivity));

                    if (isToSave)
                    {
                        int iduser = DbDataAccess.GetIDUser(userowner);
                        newid = DbDataUpdate.InsertActivity(0, n.IDActivity, n.TextActivity, n.TitleActivity, n.StartActivity, n.StopActivity
                            , n.DateTimeInserted, n.TotMinPreview, iduser, n.Status, n.TypeActivity);
                    }
                }
                res = true;
            }
            catch (Exception ex)
            {
                res = false;
                string s = ex.Message;
            }*/

            return res;
        }

        public static bool ReadFileSaveToDBUpdActivities(CRSIni fileSyncroDB_UPD, DateTime lastSyncro, int numdaysInterval)
        {
            bool res = false;
            try
            {
                //CRSIni fileSyncroDB_UPD = GetIniFileSync(MBBConst.SYNCRO_ACTIVITIES, remoteUserDir);
                string userowner;
                List<DbActivity> lst = ReadFileGetActivities(fileSyncroDB_UPD, lastSyncro, numdaysInterval, out userowner);

                foreach (DbActivity n in lst)
                {
                    Int64 newid = 0;

                    bool isToSave = (!string.IsNullOrEmpty(n.TitleActivity) || !string.IsNullOrEmpty(n.TextActivity));

                    if (isToSave)
                    {
                        int iduser = DbDataAccess.GetIDUser(userowner);
                        DbActivity act = DbDataAccess.GetActivity(n.IDActivity);
                        if(act!= null && act.IDActivity > 0)
                        {
                            newid = DbDataUpdate.UpdateActivity(0, act.IDActivity, n.TextActivity, n.TitleActivity, n.TotMinPreview, n.IDUser, n.Status, n.TypeActivity);
                        }
                        else
                            newid = DbDataUpdate.InsertActivity(0, n.IDActivity, n.TextActivity, n.TitleActivity, n.StartActivity, n.StopActivity
                                                                , n.DateTimeInserted, n.TotMinPreview, iduser, n.Status, n.TypeActivity);
                    }
                }
                res = true;
            }
            catch (Exception ex)
            {
                res = false;
                string s = ex.Message;
            }

            return res;
        }

        private static bool  ReadFileSaveToDBUpdActivitiesLog( string remoteUserDir, DateTime lastSyncro, int numdaysInterval)
        {
            //bool res1 = false;
            //int idx = 0;
            CRSIni fileSyncroDB_UPD = GetIniFileSync(MBBConst.SYNCRO_ACTIVITIES, remoteUserDir);
            bool res1 = ReadFileSaveToDBUpdActivitiesLog(fileSyncroDB_UPD, lastSyncro, numdaysInterval);

            /*
            Int64 idtFromTime = 0;
            Int64 idtToTime = DateTime.Now.ToFileTime();//dtTo.ToFileTime();
            if (lastSyncro > DateTime.MinValue)
            {
                idtFromTime = lastSyncro.ToFileTime();               
                if (numdaysInterval>0)
                    idtToTime = Convert.ToDateTime(lastSyncro).AddDays(numdaysInterval).ToFileTime();
            }

            string userowner ="";
            try
            {
                if (fileSyncroDB_UPD.ReadFileFirst(0))
                {
                    idx = GetNextIdxSectionReadFile(fileSyncroDB_UPD, idtFromTime);
                    if (idx >= 0)
                    {
                        string secid = fileSyncroDB_UPD.GetNextSectionReadFile(idx, ref idx);
                        while (string.IsNullOrEmpty(secid) == false)
                        {
                            //DbActivityLog act = new DbActivityLog();

                            Int64 nextactivity = GetIDFromSectionStd(secid);
                            Int64 idtFillAct = GetDTFill(secid);
                            DateTime dt = DateTime.FromFileTime(idtFillAct);
                            if (idtFromTime == 0)
                            {
                                // è possibile che la data iniziale non sia impostata (per esempio al primo avvio dell'applicazione)
                                idtFromTime = idtFillAct - 1;
                                if (numdaysInterval > 0)
                                    idtToTime = dt.AddDays(numdaysInterval).ToFileTime();
                            }
                            if (idtFillAct > idtFromTime && idtFillAct <= idtToTime)
                            {

                                long idactivity = Convert.ToInt32(fileSyncroDB_UPD.GetValueReadFile(idx, secid, "idactivity"));

                                userowner = Convert.ToString(fileSyncroDB_UPD.GetValueReadFile(idx, secid, "userowner"));
                                if (string.IsNullOrEmpty(userowner))
                                    userowner = GetUserOwner(secid);

                                int iduser = DbDataAccess.GetIDUser(userowner);

                                DateTime dtstart = dt;
                                string o = fileSyncroDB_UPD.GetValueReadFile(idx, secid, "StartActivityLog");
                                if (!string.IsNullOrEmpty(o) && Utils.IsNumber(o))
                                    dtstart = DateTime.FromFileTime(Convert.ToInt64(o));

                                DateTime dtstart2 = dt;
                                o = fileSyncroDB_UPD.GetValueReadFile(idx, secid, "NewStartActivityLog");
                                if (!string.IsNullOrEmpty(o) && Utils.IsNumber(o))
                                    dtstart2 = DateTime.FromFileTime(Convert.ToInt64(o));

                                DateTime dtstop2 = dt;
                                o = fileSyncroDB_UPD.GetValueReadFile(idx, secid, "NewStopActivityLog");
                                if (!string.IsNullOrEmpty(o) && Utils.IsNumber(o))
                                    dtstop2 = DateTime.FromFileTime(Convert.ToInt64(o));

                                DateTime dtins = dt;
                                string oo = fileSyncroDB_UPD.GetValueReadFile(idx, secid, "datetimeinserted");
                                if (!string.IsNullOrEmpty(oo) && Utils.IsNumber(oo))
                                    dtins = DateTime.FromFileTime(Convert.ToInt64(oo));

                                //act.IDActivity = idactivity;
                                //act.StartActivityLog = dtstart;
                                //act.StopActivityLog = typeact;

                                string typeop = fileSyncroDB_UPD.GetValueReadFile(idx, secid, "typeop");
                                //res1.Add(new Tuple<DbActivityLog, string> (act,typeop));

                                if (typeop == "D") DbDataUpdate.DeleteActivityLog(idactivity, dtstart);
                                else
                                if (typeop == "U") DbDataUpdate.UpdateActivityLog(idactivity, dtstart, dtstart2, dtstop2);

                            }
                            else
                                break;

                            secid = fileSyncroDB_UPD.GetNextSectionReadFile(idx + 1, ref idx);
                        }
                    }
                    res1 = true;
                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }
            */
            return res1;
        }

        public static bool ReadFileSaveToDBUpdActivitiesLog(CRSIni fileSyncroDB_UPD, DateTime lastSyncro, int numdaysInterval)
        {
            bool res1 = false;
            int idx = 0;
            //CRSIni fileSyncroDB_UPD = GetIniFileSync(MBBConst.SYNCRO_ACTIVITIES, remoteUserDir);

            Int64 idtFromTime = 0;
            Int64 idtToTime = DateTime.Now.ToFileTime();//dtTo.ToFileTime();
            if (lastSyncro > DateTime.MinValue)
            {
                idtFromTime = lastSyncro.ToFileTime();
                if (numdaysInterval > 0)
                    idtToTime = Convert.ToDateTime(lastSyncro).AddDays(numdaysInterval).ToFileTime();
            }
            //string userowner = "";
            try
            {
                //if (fileSyncroDB_UPD.ReadFileFirst(0))
                if (fileSyncroDB_UPD.Lines.Count() == 0)
                    fileSyncroDB_UPD.ReadFileFirst(0);

                //{
                string secid = fileSyncroDB_UPD.GetNextSectionReadFile(0, ref idx);
                while (string.IsNullOrEmpty(secid) == false)
                {
                    //DbActivityLog act = new DbActivityLog();

                    Int64 nextactivity = GetIDFromSectionStd(secid);
                    Int64 idtFillAct = GetDTFill(secid);
                    DateTime dt = DateTime.FromFileTime(idtFillAct);
                    if (idtFromTime == 0)
                    {
                        // è possibile che la data iniziale non sia impostata (per esempio al primo avvio dell'applicazione)
                        idtFromTime = idtFillAct - 1;
                        if (numdaysInterval > 0)
                            idtToTime = dt.AddDays(numdaysInterval).ToFileTime();
                    }

                    if (idtFillAct > idtFromTime && idtFillAct <= idtToTime)
                    {
                        long idactivity = Convert.ToInt32(fileSyncroDB_UPD.GetValueReadFile(idx, secid, "idactivity"));

                        string userowner = Convert.ToString(fileSyncroDB_UPD.GetValueReadFile(idx, secid, "userowner"));
                        if (string.IsNullOrEmpty(userowner))
                            userowner = GetUserOwner(secid);

                        int iduser = DbDataAccess.GetIDUser(userowner);

                        DateTime dtstart = dt;
                        string o = fileSyncroDB_UPD.GetValueReadFile(idx, secid, "StartActivityLog");
                        if (!string.IsNullOrEmpty(o) && Utils.IsNumber(o))
                            dtstart = DateTime.FromFileTime(Convert.ToInt64(o));

                        DateTime dtstart2 = dt;
                        o = fileSyncroDB_UPD.GetValueReadFile(idx, secid, "NewStartActivityLog");
                        if (!string.IsNullOrEmpty(o) && Utils.IsNumber(o))
                            dtstart2 = DateTime.FromFileTime(Convert.ToInt64(o));

                        DateTime dtstop2 = dt;
                        o = fileSyncroDB_UPD.GetValueReadFile(idx, secid, "NewStopActivityLog");
                        if (!string.IsNullOrEmpty(o) && Utils.IsNumber(o))
                            dtstop2 = DateTime.FromFileTime(Convert.ToInt64(o));

                        DateTime dtins = dt;
                        string oo = fileSyncroDB_UPD.GetValueReadFile(idx, secid, "datetimeinserted");
                        if (!string.IsNullOrEmpty(oo) && Utils.IsNumber(oo))
                            dtins = DateTime.FromFileTime(Convert.ToInt64(oo));

                        //act.IDActivity = idactivity;
                        //act.StartActivityLog = dtstart;
                        //act.StopActivityLog = typeact;

                        string typeop = fileSyncroDB_UPD.GetValueReadFile(idx, secid, "typeop");
                        //res1.Add(new Tuple<DbActivityLog, string> (act,typeop));

                        if (typeop == "D") DbDataUpdate.DeleteActivityLog(idactivity, dtstart);
                        else
                        if (typeop == "U") DbDataUpdate.UpdateActivityLog(idactivity, dtstart, dtstart2, dtstop2);

                    }

                    secid = fileSyncroDB_UPD.GetNextSectionReadFile(idx + 1, ref idx);
                }

                res1 = true;
                //}
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }

            return res1;
        }

        #endregion        

        #region SYNCRO Docs and Files: download e upload dei file       
        /// <summary>
        /// Sincronizza il file (che è il nome del file con path), viene richiamato allo scadere del timer per ogni file dell'owner.
        /// La sincronizzazione (copia su dir dropbox del file eventualemntne modificato in locale, in modo da avere versioni allineate) è
        /// necessaria dato che un file potrebbe essere condiviso con più utenti. 
        /// PROBLEMA: non c'è limite al numero dei file che potrbbe diventare alto per ogni utente
        /// </summary>
        /// <param name="file"></param>
        /// <param name="idnote">id della nota del file, serve solo per aggiungere un attributo in fase di salvataggio dei pendings</param>
        /// <param name="destFoldername">identifica il nome della cartella di destinazione all'interno della directory condivisa</param>
        /// <returns></returns>
        public static MBBSyncroPending SyncroFileOBSOLETE(string file, string fileremote, Int64 idnote, int idUser, string user, MBBSyncroParams par)
        {
            try
            {
                MBBSyncroPending docpending = null;
                DateTime lastModified = System.IO.File.GetLastWriteTime(file);
                //-- filename: nome del file senza path
                string fileName = System.IO.Path.GetFileName(file);
                string newFileName = fileName;
               
                DbNoteDoc docLastVer = null;
                if (par.IsUniqueFileName)
                    docLastVer = DbDataAccess.GetDocInfo(fileName, idUser);
                else
                    docLastVer = DbDataAccess.GetDocInfo(fileName, idUser, idnote);

                string destFoldername = fileremote.Replace(par.SharingDir,"");
                destFoldername = fileremote.Replace(docLastVer.DocName, "");

                DateTime lastModifiedRemote = DateTime.MinValue;
                try
                {
                    lastModifiedRemote = System.IO.File.GetLastWriteTime(fileremote);
                }
                catch
                {
                    lastModifiedRemote = docLastVer.DateTimeLastMod;
                }
                //-- se il file da sincronizzare è della stessa nota di quello dell'ultima versione non faccio niente
                //-- DOMANDA:  qualcuno potrebbe aver modificato il file della stessa nota?
                //--    si: l'utente stesso su un altro pc
                if (docLastVer != null && docLastVer.IDNote > 0)
                {
                    //-------------------------------------------
                    //-----         CASO UPLOAD            ------
                    //-------------------------------------------
                    TimeSpan diff1 = lastModified.Subtract(lastModifiedRemote/*docLastVer.DateTimeLastMod*/);
                    if (diff1.TotalMilliseconds > 1000 || diff1.TotalMilliseconds < -1000)
                    {
                        if (DateTime.Compare(lastModified, lastModifiedRemote) > 0)
                        {
                            docpending = SyncroFileUploadOBSOLETE(file, idnote, docLastVer.Version, idUser, par, destFoldername, out newFileName);
                            //int ver = CRSFileUtil.UploadFile(par.DropBoxRootDir, sourceDir, ref fileName, destFoldername, true);
                            //-- devo aggiornare i dati su ultima modifica e versione su db
                            bool res = false;
                            if (docpending.IsCompletedOp)
                                res = DbDataUpdate.UpdateDocInfo(idnote, docLastVer.DocName, docpending.Version, file, newFileName, idUser, lastModified, destFoldername);
                        }
                        //-------------------------------------------
                        //-----         CASO DOWNLOAD           -----
                        //-------------------------------------------
                        if (DateTime.Compare(lastModified, lastModifiedRemote) < 0)
                        {
                            docpending = SyncroFileDownload(file, docLastVer,idUser,par);
                            //-- non dovrebbe capitare dato che le info sul file (da db e file fisico)  dovrebbero essere allinete
                            //-- e il file remoto dovrebbe essere quello più aggiornato
                            bool res = false;
                            if (DateTime.Compare(lastModifiedRemote, docLastVer.DateTimeLastMod) != 0)
                                res = DbDataUpdate.UpdateDocInfo(idnote, docLastVer.DocName, docpending.Version, file, "", idUser, lastModifiedRemote, destFoldername);
                        }
                    }
                }

                return docpending;
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("SyncroFile error: " + ex.Message, "");
                return null;
            }
        }

        public static MBBSyncroPending SyncroFile(string file, string fileremote, Int64 idnote, int idUser, string user, MBBSyncroParams par)
        {
            try
            {
                MBBSyncroPending docpending = null;
                DateTime lastModified = System.IO.File.GetLastWriteTime(file);
                //-- filename: nome del file senza path
                string fileName = System.IO.Path.GetFileName(file);
                string newFileName = fileName;
                string sourceDir = Path.GetDirectoryName(file);

                DbNoteDoc docLastVer = null;
                if (par.IsUniqueFileName)
                    docLastVer = DbDataAccess.GetDocInfo(fileName, idUser);
                else
                    docLastVer = DbDataAccess.GetDocInfo(fileName, idUser, idnote);

                string destFoldername = fileremote.Replace(par.SharingDir, "");
                destFoldername = fileremote.Replace(docLastVer.DocName, "");

                DateTime lastModifiedRemote = DateTime.MinValue;
                try
                {
                    lastModifiedRemote = System.IO.File.GetLastWriteTime(fileremote);
                }
                catch
                {
                    lastModifiedRemote = docLastVer.DateTimeLastMod;
                }
                //-- se il file da sincronizzare è della stessa nota di quello dell'ultima versione non faccio niente
                //-- DOMANDA:  qualcuno potrebbe aver modificato il file della stessa nota?
                //--    si: l'utente stesso su un altro pc
                if (docLastVer != null && docLastVer.IDNote > 0)
                {
                    //-------------------------------------------
                    //-----         CASO UPLOAD            ------
                    //-------------------------------------------
                    TimeSpan diff1 = lastModified.Subtract(lastModifiedRemote/*docLastVer.DateTimeLastMod*/);
                    if (diff1.TotalMilliseconds > 1000 || diff1.TotalMilliseconds < -1000)
                    {
                        if (DateTime.Compare(lastModified, lastModifiedRemote) > 0)
                        {
                            //docpending = SyncroFileUploadOBSOLETE(file, idnote, docLastVer.Version, idUser, par, destFoldername, out newFileName);
                            int ver = CRSFileUtil.UploadFile(par.DropBoxRootDir, sourceDir, ref fileName, destFoldername, true);
                            docLastVer.DocName = fileName;
                            //-- devo aggiornare i dati su ultima modifica e versione su db
                            bool res = false;
                            if (docpending.IsCompletedOp)
                                res = DbDataUpdate.UpdateDocInfo(idnote, docLastVer.DocName, docpending.Version, file, newFileName, idUser, lastModified, destFoldername);
                        }
                        //-------------------------------------------
                        //-----         CASO DOWNLOAD           -----
                        //-------------------------------------------
                        if (DateTime.Compare(lastModified, lastModifiedRemote) < 0)
                        {
                            string dirOrigin = Path.Combine(par.DropBoxRootDir, destFoldername);
                            //docpending = SyncroFileDownload(file, docLastVer, idUser, par);
                            int ver = CRSFileUtil.UploadFile(sourceDir, dirOrigin, ref fileName, destFoldername, true);
                            docLastVer.DocName = fileName;
                            //-- non dovrebbe capitare dato che le info sul file (da db e file fisico)  dovrebbero essere allinete
                            //-- e il file remoto dovrebbe essere quello più aggiornato
                            bool res = false;
                            if (DateTime.Compare(lastModifiedRemote, docLastVer.DateTimeLastMod) != 0)
                                res = DbDataUpdate.UpdateDocInfo(idnote, docLastVer.DocName, docpending.Version, file, newFileName, idUser, lastModifiedRemote, destFoldername);

                        }
                    }
                }

                return docpending;
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("SyncroFile error: " + ex.Message, "");
                return null;
            }
        }

        /// <summary>
        /// Sincronizzo il file posizionandolo nella cartella di origine (cartella in cui è stato posizionato al primo upload).
        /// Allo stesso tempo versiono il file preesistente.
        /// Se il file è nuovo questo metodo non viene chiamato, ma viene fatto un upload diretto (vedi SaveNote).
        /// </summary>
        /// <param name="filepath">path del file originale</param>
        /// <param name="docLastVer"></param>
        /// <param name="destFoldername">è la directory di destinazione del file, normalmente è DOCS\id nota</param>
        /// <returns></returns>
        public static MBBSyncroPending SyncroFileUploadOBSOLETE(string filepath, Int64 idnote, int existentVersion/*, DbNoteDoc docLastVer*/, int idUser, MBBSyncroParams par, string destFoldername, out string newNameDestFile)
        {
           
            //-- PREREQUISITI:
            //--    - docLastVer!= null: sincronizzo solo file preesistenti
            //--    - idnote>0: la nota da cui scaturisce la sincronizzazione esiste (ovvio)
            //-- Chiamo questo metodo per ogni file appartenente allo user, già salvato in remoto

            //string idnotefolder = MakeFolderNameUser(docLastVer.IDNote, docLastVer.UserNoteOwner);
            if (string.IsNullOrEmpty(destFoldername))
                destFoldername = "\\DOCS";
            string fileName = System.IO.Path.GetFileName(filepath);

            newNameDestFile = fileName;


            int ver = existentVersion;
            string sourceDir = System.IO.Path.GetDirectoryName(filepath);
            string user = DbDataAccess.GetUser(idUser);

            //if (docnote.DateTimeLastMod > docLastVer.DateTimeLastMod)

            MBBSyncroPending docpending = new MBBSyncroPending();
            DbNoteDoc docCurrent = new DbNoteDoc();
            docCurrent.IDNote = idnote;
            docCurrent.IDUser = idUser;
            //docCurrent.IDUserNoteOwner = idUser;
            docCurrent.UserMod = user;
            docCurrent.UserNoteOwner = user;
            docCurrent.DateTimeLastMod = System.IO.File.GetLastWriteTime(filepath);
            docCurrent.DocName = fileName;
            docCurrent.FileNameLocal = filepath;

            docpending.UserOwner = user;
            docpending.UploadOrDownload = "UPLOAD";
            docpending.DocLocal = docCurrent;
            //docpending.DocRemote = docLastVer;

            //if (docLastVer.IDUser != idUser)
            // se non esistono versioni precedenti (cioè file con lo stesso nome) existentVersion = -1
            if (existentVersion>=0)
            {
                if (par.SyncroOnlyOwner)
                {
                    docpending.Message = "Upload file verso owner non permesso (vedi parametri)";
                    docpending.IsPermitted = false;
                    docpending.IsCompletedOp = false;
                }
                else
                {
                    docpending.Message = "Upload file verso owner permesso e avvenuto";
                    docpending.IsPermitted = true;
                    docpending.IsCompletedOp = true;
                    ver = CRSFileUtil.UploadFile(par.SharingDir, sourceDir, ref fileName, destFoldername, true);
                }
            }
            else
            {
                docpending.Message = "Upload file permesso, salvato nella nota preesistente (presente versione meno recente, ora con nuova versione)";
                docpending.IsPermitted = true;
                docpending.IsCompletedOp = true;
                ver = CRSFileUtil.UploadFile(par.SharingDir, sourceDir, ref fileName, destFoldername, true);
                
            }
            docCurrent.DocName = fileName;
            //}

            //------------------- NOTE ------------------
            //-- PROBLEMA CONDIVISIONE:
            //-- es: creo nuova nota e inserisco documento che è già condiviso ma di cui non sono owner
            //--    INCOMPATIBILITA': sono owner della nota che sto creando ma non del documento !!
            //--    SOLUZIONE: EVIDENZIA , cosiglia di eliminare condivisione
            //--
            //-- il file che intendo sicronizzare ha una data più recente rispetto a quello
            //-- su remote dir:
            //--    1) se l'opzione è di sicronizzare solo se l'utente corrente è l'owner faccio upload e versiono
            //--    2) altrimenti chiedo se sovrascriverre file corrente con download ( se è attiva l'opzione di 
            //--       effetture la richiesta
            //-------------------------------------------

            docpending.Version = ver;
            docpending.DateTimeSyncro = DateTime.Now;
            CRSLog.WriteLog("Syncro Uplaod file: [dir source:" + sourceDir + "] [filename (with version):" + fileName + "]" + "[dir dest:" + par.SharingDir + "]", "");
            return docpending;
        }

        public static MBBSyncroPending SyncroFileDownload(string file, DbNoteDoc docLastVer, int idUser, MBBSyncroParams par)
        {
            bool res = false;
            MBBSyncroPending docpending = new MBBSyncroPending();
            docpending.DocLocal = null;
            docpending.DocRemote = docLastVer;
            docpending.UserOwner = docLastVer.UserNoteOwner;
            docpending.UploadOrDownload = "DOWNLOAD";

            int ver = docLastVer.Version + 1;
            string idNoteLastVer = MakeFolderNameUser(docLastVer.IDNote, docLastVer.UserNoteOwner);
            string sourceDir = par.SharingDir+ "\\" + idNoteLastVer;
            string destDir = "";
            string fileName = System.IO.Path.GetFileName(file);
            //-- dove faccio downlod?
            //-- su db ho la dir locale dell'utente che ha creto la versione in upload
            //-- devo recuperare il filenamelocal del documento associato all'utente attuale
            //-- potrebbe non esistere? Si se l'utente attuale è stato semplicemente messo nella lista degli user che condividono il file
            //--    1) in questo caso il download avviene nella cartella locale definita nei parametri
            //--    2) altrimenti recupero l'ultima nota (con versione più alta del doc) appartenente all'utente attuale
            //--    3) se esiste la cartella faccio download altrimenti salvo in dir locale dei parametri

            if (idUser == docLastVer.IDUser)
            {
                //-- solo le note e quindi i file di cui l'utente è owner sono scaricabili nella posizione 
                //-- salvata nel db: in questo caso se cambio computer ricostruisco il filesystem
                destDir = System.IO.Path.GetDirectoryName(docLastVer.FileNameLocal);
            }
            else
            {
                //-- se non sei owner della nota scarica il documento nella cartella locale temporanea
                //-- così il file è sempre consultabile
                destDir = par.LocalTempDir + "\\" + idNoteLastVer;
            }

            string msg = "";
            if (docLastVer.IDUser != idUser)
                msg = "Download file più recente di cui NON sei owner avvenuto";
            else
                msg = "Download file più recente di cui sei owner avvenuto";

            docpending.Message = msg;
            docpending.IsPermitted = true;
            docpending.IsCompletedOp = true;
            docpending.DateTimeSyncro = DateTime.Now;
            //-- se destDir non esiste viene creata
            res = CRSFileUtil.DownloadFile(sourceDir, fileName, destDir, par.LocalTempDir, idNoteLastVer);

            CRSLog.WriteLog("Syncro Download file: [dir:" + sourceDir + "] [filename:"+ fileName +"]" + "[destdir:" + destDir+"]","");
            return docpending;
        }

        /// <summary>
        /// L'intento è semplicemente di allineare due file che hanno date di modifica diversa: 
        /// la data più recente sarà il file che sovrascriverà l'altro.
        /// Verrà fatta una copia di backup del file sovrascritto nella cartella MBB (quella di dropbox per intenderci)
        /// </summary>
        /// <param name="fileOriginal"></param>
        /// <param name="fileMBB"></param>
        /// <param name="idnote"></param>
        /// <param name="idUser"></param>
        /// <param name="user"></param>
        /// <param name="par"></param>
        /// <returns></returns>
        public static bool SyncroFilesLocal(string fileOriginal, string fileMBB)
        {
            bool res = false;
            try
            {
                DateTime lastModifiedOriginal = DateTime.MinValue;
                // se il file è stato importato da drop box non esiste nella cartella originaria
                if(File.Exists(fileOriginal))
                    lastModifiedOriginal = System.IO.File.GetLastWriteTime(fileOriginal);
                DateTime lastModifiedMBB = System.IO.File.GetLastWriteTime(fileMBB);

                string sourceDir = Path.GetDirectoryName(fileMBB);

                if (lastModifiedOriginal > lastModifiedMBB)
                {
                    res = CRSFileUtil.BackupFile(sourceDir, Path.GetFileName(fileMBB));
                    File.Copy(fileOriginal, fileMBB, true);
                }
                else
                if (lastModifiedOriginal < lastModifiedMBB)
                {
                    res = CRSFileUtil.BackupFile(sourceDir, Path.GetFileName(fileOriginal));
                    File.Copy(fileMBB, fileOriginal, true);
                }
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("SyncroFilesLocal error: " + ex.Message, "");
                return false;
            }

            return res;
        }
        #endregion

        #region OLD VERSION SYNCRO READ Files MENO PERFORMANTE: lettura da file di testo e scrittura su DB
        // USANO I VECCHI METODI DI CRSIni class

        /// <summary>
        /// Sincronizza la tabella degli utenti
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool SyncroDBUsers_(string user)
        {
            //-- Si tratta di sincronizzarsi con file FileSyncroDB_USERS che contiene l'immagine della tabella del db
            //-- Le info fondamentali sono: ID, Name, Password, WorkGroup 
            //-- ALLINEAMENTO: dato che ci possono essere inserimenti, aggiornamenti(di pwd per es) o cancellazioni
            //-- devo poter distinguere i diversi casi
            //-- considerando il fatto che i record saranno sempre limitati (gli utenti non saranno migliaia) posso eseguire queste 
            //-- operazioni frequentemente

            //-- FORMATO FILE
            //-- [ID] => sezione con id numerico, trattino, lettera identificativo del tipo di operazione: 
            //-- campi:  operation(I:insert, U: Update, D: del),name,pwd,group,lang, dateinsert(nel caso in ins), dateupdate         
            //-- la sezione deve essere univoca 
            //-- PROBLEMA DELETE: se elimino e reinserisco uno user potrei avere difficoltà ad allinearmi dato che una sezione potrebbe 
            //-- essere ripetuta
            //-- SOLUZIONE:  a seconda della licenza (che ogni utente dovrà inserire in ogni dispositivo) dovrei conoscere il numero
            //-- tot di utenti, inoltre posso specificare la data di inserimento e la dat di ultima modifica
            //-- Quindi il file potrebbe avere n sezioni fisse (= num utenti) e all'interno l'informazione se l'utente è attivo (status DELETED se cancellato)
            //-- Con questi dati dovrei avere tutto ciò che mi serve per fare allineamento corretto

            MBBSyncroParams p = new MBBSyncroParams();
            p.SharingDir = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_SHARINGDIR);
            p.DropBoxRootDir = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_DROPBOXDIR);
            string remoteDir = GetUserRemoteDir(user, false,  p);
            //string remoteSharedDir = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_SHAREDDIR);
            CRSIni fileSyncroDB_USERS = GetIniFileSync(MBBConst.SYNCRO_USERS, remoteDir);

            bool res = true;
            int idx = 1;
            string secid = fileSyncroDB_USERS.GetNextSection(ref idx);
            while (string.IsNullOrEmpty(secid) == false)
            {
                int id = Convert.ToInt32(secid);
                //-- indica operazione di tipo: insert(I),update(U),delete(D)
                string operation = fileSyncroDB_USERS.GetValue(id.ToString(), "operation").ToString();
                //string status = this.FileSyncroDB_USERS.GetValue(id.ToString(), "status").ToString();
                string name = fileSyncroDB_USERS.GetValue(id.ToString(), "name").ToString();
                string pwd = fileSyncroDB_USERS.GetValue(id.ToString(), "pwd").ToString();
                string group = fileSyncroDB_USERS.GetValue(id.ToString(), "group").ToString();
                string lang = fileSyncroDB_USERS.GetValue(id.ToString(), "lang").ToString();

                object o = fileSyncroDB_USERS.GetValue(id.ToString(), "datetimeIns");
                DateTime dtins = DateTime.MinValue;
                if (o != null)
                    dtins = DateTime.FromFileTime(Convert.ToInt64(o));

                o = fileSyncroDB_USERS.GetValue(id.ToString(), "datetimeUpd");
                DateTime dtupd = DateTime.MinValue;
                if (o != null)
                    dtupd = DateTime.FromFileTime(Convert.ToInt64(o));

                bool res2 = DbDataUpdate.InsUpdDelUser(id, name, pwd, lang, group, dtins, dtupd, operation);

                secid = fileSyncroDB_USERS.GetNextSection(ref idx);
            }

            return res;
        }

        private static bool SyncroFileToDBDelNotes_(string remoteUserDir, DateTime lastSYNCRO)
        {

            CRSIni fileSyncroDB_DEL = GetIniFileSync(MBBConst.SYNCRO_DEL, remoteUserDir);

            bool res = true;
            int idx = 0;
            string secid = fileSyncroDB_DEL.GetNextSection(ref idx);
            while (string.IsNullOrEmpty(secid) == false)
            {
                Int64 id = GetIDFromSectionStd(secid);
                Int64 idtFillNote = GetDTFill(secid);
                string userowner = GetUserOwner(secid);
                //-- elimino la nota dello userowner che non è necessariamente il currentUser
                //-- che potrebbe essere l'utente con cui condivido la nota
                if (idtFillNote > lastSYNCRO.ToFileTime())
                {
                    int iduser = DbDataAccess.GetIDUser(userowner);
                    res = DbDataUpdate.DeleteNote(id, iduser, true);
                }
                secid = fileSyncroDB_DEL.GetNextSection(ref idx);
            }

            return res;
        }

        private static bool SyncroFileToDBDelSharedNotes_(string remoteUserDir, string currentUser, DateTime lastSYNCRO)
        {
            CRSIni fileSyncroDB_DEL = GetIniFileSync(MBBConst.SYNCRO_DEL, remoteUserDir);

            bool res = true;
            int idx = 0;
            string secid = fileSyncroDB_DEL.GetNextSection(ref idx);
            while (string.IsNullOrEmpty(secid) == false)
            {
                Int64 id = GetIDFromSectionStd(secid);
                Int64 idtFillNote = GetDTFill(secid);
                string userowner = GetUserOwner(secid);
                //-- elimino la nota dello userowner che non è necessariamente il currentUser
                //-- che potrebbe essere l'utente con cui condivido la nota
                if (idtFillNote > lastSYNCRO.ToFileTime())
                {
                    List<string> usersDest = Convert.ToString(fileSyncroDB_DEL.GetValue(secid, "usersdest")).Split(',', ';').ToList();
                    if (usersDest.Count > 0 && usersDest.Contains(currentUser))
                    {
                        int iduser = DbDataAccess.GetIDUser(userowner);
                        res = DbDataUpdate.DeleteNote(id, iduser, true);
                    }
                }
                secid = fileSyncroDB_DEL.GetNextSection(ref idx);
            }

            return res;
        }

        private static bool SyncroFileToDBUpdNotes_(string remoteUserDir, string currentUser, DateTime lastSYNCRO)
        {
            CRSIni fileSyncroDB_UPD = GetIniFileSync(MBBConst.SYNCRO_UPD, remoteUserDir);
            bool res = true;
            int idx = 0;
            string secid = fileSyncroDB_UPD.GetNextSection(ref idx);
            Int64 idtToTime = lastSYNCRO.ToFileTime();

            while (string.IsNullOrEmpty(secid) == false)
            {
                Int64 nextnote = GetIDFromSectionStd(secid);
                Int64 idtFillNote = GetDTFill(secid);
                try
                {
                    if (idtFillNote > idtToTime)
                    {

                        string title = Convert.ToString(fileSyncroDB_UPD.GetValue(secid, "title"));
                        string text = Convert.ToString(fileSyncroDB_UPD.GetValueWithParentesis(secid, "text"));
                        string userowner = Convert.ToString(fileSyncroDB_UPD.GetValue(secid, "userowner"));
                        if (string.IsNullOrEmpty(userowner))
                            userowner = GetUserOwner(secid);

                        object val = fileSyncroDB_UPD.GetValue(secid, "idnoteparent");
                        Int64 idnoteparent = val == null ? 0
                            : Convert.ToInt64(val);
                        string usernameparent = fileSyncroDB_UPD.GetValue(secid, "usernameparent") == null ? ""
                            : Convert.ToString(fileSyncroDB_UPD.GetValue(secid, "usernameparent"));

                        int rate = Convert.ToInt32(fileSyncroDB_UPD.GetValue(secid, "rate"));
                        val = fileSyncroDB_UPD.GetValue(secid, "isfavorite");
                        bool isfavorite = false;
                        if (val != null)
                        {
                            if (Convert.ToString(val) == "true" || Convert.ToString(val) == "false")
                                isfavorite = Convert.ToBoolean(val);
                            else
                                if (Utils.IsNumber(Convert.ToString(val)))
                                    isfavorite = (Convert.ToInt32(val) == 1) ? true : false;
                        }


                        int iduser = DbDataAccess.GetIDUser(userowner);
                        int iduserSh = DbDataAccess.GetIDUser(currentUser);
                        DateTime dt = DateTime.Now;
                        DateTime dt1 = DateTime.Now;
                        object o = fileSyncroDB_UPD.GetValue(secid, "datetimeLastMod");
                        if (o != null)
                            dt1 = DateTime.FromFileTime(Convert.ToInt64(o));

                        DateTime dt2 = DateTime.Now;
                        object oo = fileSyncroDB_UPD.GetValue(secid, "datetimeIns");
                        if (oo != null)
                            dt2 = DateTime.FromFileTime(Convert.ToInt64(oo));

                        dt = dt1;
                        if (dt2 < dt1)
                            dt = dt2;

                        Int64 newid = 0;
                        if (DbDataAccess.ExistsNote(nextnote, iduser))
                            res = DbDataUpdate.UpdateNote(nextnote, title, text, iduser, iduserSh, idnoteparent, usernameparent, rate, dt1, dt2, false, false, isfavorite, true);
                        else newid = DbDataUpdate.InsertNote(nextnote, title, text, iduser, iduserSh, idnoteparent, usernameparent, rate, dt/*, false, false*/);
                    }
                }
                catch { }
                secid = fileSyncroDB_UPD.GetNextSection(ref idx);
            }

            return res;
        }

        private static bool SyncroFileToDBUpdSharedNotes_(string remoteUserDir, string currentUser, DateTime lastSYNCRO)
        {
            CRSIni fileSyncroDB_UPD = GetIniFileSync(MBBConst.SYNCRO_UPD, remoteUserDir);
            bool res = true;
            int idx = 0;
            string secid = fileSyncroDB_UPD.GetNextSection(ref idx);

            while (string.IsNullOrEmpty(secid) == false)
            {
                Int64 nextnote = GetIDFromSectionStd(secid);
                Int64 idtFillNote = GetDTFill(secid);
                try
                {
                    Int64 lstsync = lastSYNCRO.ToFileTime();
                    if (idtFillNote > lstsync)
                    {
                        List<string> usersDest = Convert.ToString(fileSyncroDB_UPD.GetValue(secid, "usersdest")).Split(',', ';').ToList();
                        if (usersDest.Count > 0 && usersDest.Contains(currentUser))
                        {
                            string title = Convert.ToString(fileSyncroDB_UPD.GetValue(secid, "title"));
                            string text = Convert.ToString(fileSyncroDB_UPD.GetValueWithParentesis(secid, "text"));
                            string userowner = Convert.ToString(fileSyncroDB_UPD.GetValue(secid, "userowner"));
                            if (string.IsNullOrEmpty(userowner))
                                userowner = GetUserOwner(secid);

                            object val = fileSyncroDB_UPD.GetValue(secid, "idnoteparent");
                            int idnoteparent = val == null ? 0
                                : Convert.ToInt32(val);
                            string usernameparent = fileSyncroDB_UPD.GetValue(secid, "usernameparent") == null ? ""
                                : Convert.ToString(fileSyncroDB_UPD.GetValue(secid, "usernameparent"));

                            int rate = Convert.ToInt32(fileSyncroDB_UPD.GetValue(secid, "rate"));

                            val = fileSyncroDB_UPD.GetValue(secid, "isfavorite");
                            int isfavorite = 0;
                            if (val != null) isfavorite = Convert.ToInt32(val);

                            int iduser = DbDataAccess.GetIDUser(userowner);
                            int iduserSh = DbDataAccess.GetIDUser(currentUser);
                            DateTime dt = DateTime.Now;
                            DateTime dt1 = DateTime.Now;
                            object o = fileSyncroDB_UPD.GetValue(secid, "datetimeLastMod");
                            if (o != null)
                                dt1 = DateTime.FromFileTime(Convert.ToInt64(o));

                            DateTime dt2 = DateTime.Now;
                            object oo = fileSyncroDB_UPD.GetValue(secid, "datetimeIns");
                            if (oo != null)
                                dt2 = DateTime.FromFileTime(Convert.ToInt64(oo));

                            dt = dt1;
                            if (dt2 < dt1)
                                dt = dt2;

                            Int64 newid = 0;
                            if (DbDataAccess.ExistsNote(nextnote, iduser))
                                res = DbDataUpdate.UpdateNote(nextnote, title, text, iduser, iduserSh, idnoteparent, usernameparent, rate, dt1,dt2, false, false, (isfavorite == 1), true);
                            else newid = DbDataUpdate.InsertNote(nextnote, title, text, iduser, iduserSh, idnoteparent, usernameparent, rate, dt/*, false, false*/);
                        }
                    }
                }
                catch { }
                secid = fileSyncroDB_UPD.GetNextSection(ref idx);
            }

            return res;
        }

        private static bool SyncroFileToDBUpdDocs_(string remoteUserDir, DateTime lastSYNCRO)
        {
            CRSIni fileSyncroDB = GetIniFileSync(MBBConst.SYNCRO_DOCS, remoteUserDir);
            bool res = true;
            int idx = 0;
            string secid = fileSyncroDB.GetNextSection(ref idx);

            while (string.IsNullOrEmpty(secid) == false)
            {
                Int64 nextnote = GetIDFromSectionStd(secid);
                Int64 idtFillNote = GetDTFill(secid);
                try
                {
                    if (idtFillNote > lastSYNCRO.ToFileTime())
                    {
                        string userowner = GetUserOwner(secid);
                        int iduser = DbDataAccess.GetIDUser(userowner);
                        //idx = 0;
                        int i = 0;
                        while (fileSyncroDB.HasEntry(nextnote.ToString(), "doc" + i.ToString()))
                        {
                            string filename = Convert.ToString(fileSyncroDB.GetValue(nextnote.ToString(), "doc" + i.ToString()));
                            string intdir = Convert.ToString(fileSyncroDB.GetValue(nextnote.ToString(), "internaldirremote" + i.ToString()));
                            int ver = Convert.ToInt32(fileSyncroDB.GetValue(nextnote.ToString(), "version" + i.ToString(), 0));
                            //-- ATTENZIONE: è possibile che i file non siano ancora nell'hd e che quindi non esistano
                            string dirname = System.IO.Path.GetDirectoryName(filename);
                            string docname = System.IO.Path.GetFileName(filename);

                            DateTime dt = DateTime.Now;
                            object o = fileSyncroDB.GetValue(nextnote.ToString(), "datetimeLastMod" + i.ToString());
                            if (o != null)
                                dt = DateTime.FromFileTime(Convert.ToInt64(o));

                            bool res2 = DbDataUpdate.InsertDoc(nextnote, docname, ver, dt, intdir, filename, iduser, true);

                            idx++;
                            i++;
                        }
                    }
                }
                catch { }
                secid = fileSyncroDB.GetNextSection(ref idx);
            }

            return res;
        }

        private static bool SyncroFileToDBUpdDocsShared_(string remoteUserDir, string currentUser, DateTime lastSYNCRO)
        {
            CRSIni fileSyncroDB = GetIniFileSync(MBBConst.SYNCRO_DOCS, remoteUserDir);
            bool res = true;
            int idx = 0;
            string secid = fileSyncroDB.GetNextSection(ref idx);

            while (string.IsNullOrEmpty(secid) == false)
            {
                Int64 nextnote = GetIDFromSectionStd(secid);
                Int64 idtFillNote = GetDTFill(secid);
                try
                {
                    if (idtFillNote > lastSYNCRO.ToFileTime())
                    {
                        List<string> usersDest = Convert.ToString(fileSyncroDB.GetValue(secid, "usersdest")).Split(',', ';').ToList();
                        if (usersDest.Count > 0 && usersDest.Contains(currentUser))
                        {
                            string userowner = GetUserOwner(secid);
                            int iduser = DbDataAccess.GetIDUser(userowner);
                            //idx = 0;
                            int i = 0;
                            while (fileSyncroDB.HasEntry(nextnote.ToString(), "doc" + i.ToString()))
                            {
                                string filename = Convert.ToString(fileSyncroDB.GetValue(nextnote.ToString(), "doc" + i.ToString()));
                                string intdir = Convert.ToString(fileSyncroDB.GetValue(nextnote.ToString(), "internaldirremote" + i.ToString()));
                                int ver = Convert.ToInt32(fileSyncroDB.GetValue(nextnote.ToString(), "version" + i.ToString(), 0));
                                //-- ATTENZIONE: è possibile che i file non siano ancora nell'hd e che quindi non esistano
                                string dirname = System.IO.Path.GetDirectoryName(filename);
                                string docname = System.IO.Path.GetFileName(filename);

                                DateTime dt = DateTime.Now;
                                object o = fileSyncroDB.GetValue(nextnote.ToString(), "datetimeLastMod" + i.ToString());
                                if (o != null)
                                    dt = DateTime.FromFileTime(Convert.ToInt64(o));

                                bool res2 = DbDataUpdate.InsertDoc(nextnote, docname, ver, dt, intdir, filename, iduser, true);

                                idx++;
                                i++;
                            }
                        }
                    }
                }
                catch { }
                secid = fileSyncroDB.GetNextSection(ref idx);
            }

            return res;
        }

        #endregion

        #region EMAIL SYNCRO
        private static List<CRSIni> PrepareFileSyncroToSend()
        {
            // uso il name del parent per comodità, serve solo per formare il nome del file
            CRSIni fileSyncroDB = GetIniFileSync(MBBConst.SYNCRO_UPD, Path.GetTempPath());
            CRSIni fileSyncroDBDel = GetIniFileSync(MBBConst.SYNCRO_DEL, Path.GetTempPath());
            CRSIni fileSyncroDBDoc = GetIniFileSync(MBBConst.SYNCRO_DOCS, Path.GetTempPath());

            CRSIni fileSyncroDBAct = GetIniFileSync(MBBConst.SYNCRO_ACTIVITIES, Path.GetTempPath());
            CRSIni fileSyncroDBActLog = GetIniFileSync(MBBConst.SYNCRO_ACTIVITIES_LOG, Path.GetTempPath());
            
            /*BackupAndCleanFilesSyncro (fileSyncroDB, fileSyncroDBDel, fileSyncroDBDoc, fileSyncroDBAct, fileSyncroDBActLog, dtName);
            //
            string remoteDir = Path.GetTempPath();
            // creo i file da inviare via email per la successiva sincronizzazione
            // devo avere la lista delle note modificate dato che non scrivo direttamente su file di testo come per le note normali su dropbox
            foreach (DbNoteStatus n in lstnotes)
            {
                if (n.Status == 1)
                    WriteNoteSyncroFileNewFile(remoteDir, n.ID, n.Title, n.Text, n.Rate, n.DateTimeInserted, n.DateTimeLastMod
                        , userowner, n.IDNoteParent, n.UserNameParent, n.IsFavorite, out fileSyncroDB);
                else
                if (n.Status == 2)
                    WriteDelNoteSyncroFile(DateTime.Now, n.ID, userowner, remoteDir, out fileSyncroDBDel);

                if (n.Docs != null && n.Docs.Count >= 0)
                    WriteDocsSyncroFile(n.ID, n.DateTimeInserted, userowner, n.Docs, MBBCommon.syncroParams);

            }

            foreach (DbActivity a in lstActivities)
            {
                WriteActivitySyncroFileNewFile(remoteDir, userowner, a, out fileSyncroDBAct);
            }

            foreach (DbActivityLogTypeOp a in lstActivitiyLogs)
            {
                WriteActivityLogSyncroFileNewFile(remoteDir, a.TypeOperation, userowner, a.StartActivityLogKey, a, out fileSyncroDBActLog);
            }*/


            List<CRSIni> lst = new List<CRSIni>();

            if (fileSyncroDB != null)
            {
                FileInfo f = new FileInfo(fileSyncroDB.Name);
                if (f.Length > 0)
                    lst.Add(fileSyncroDB);
            }
            if (fileSyncroDBDel != null)
            {
                FileInfo f = new FileInfo(fileSyncroDBDel.Name);
                if (f.Length > 0)
                    lst.Add(fileSyncroDBDel);
            }
            if (fileSyncroDBDoc != null)
            {
                FileInfo f = new FileInfo(fileSyncroDBDoc.Name);
                if (f.Length > 0)
                    lst.Add(fileSyncroDBDoc);
            }
            if (fileSyncroDBAct != null)
            {
                FileInfo f = new FileInfo(fileSyncroDBAct.Name);
                if (f.Length > 0)
                    lst.Add(fileSyncroDBAct);
            }
            if (fileSyncroDBActLog != null)
            {
                FileInfo f = new FileInfo(fileSyncroDBActLog.Name);
                if (f.Length > 0)
                    lst.Add(fileSyncroDBActLog);
            }
            return lst;
        }

        private static void BackupAndCleanFilesSyncro(DateTime startSession)
        {
            CRSIni fileSyncroDB = GetIniFileSync(MBBConst.SYNCRO_UPD, Path.GetTempPath());
            CRSIni fileSyncroDBDel = GetIniFileSync(MBBConst.SYNCRO_DEL, Path.GetTempPath());
            CRSIni fileSyncroDBDoc = GetIniFileSync(MBBConst.SYNCRO_DOCS, Path.GetTempPath());

            CRSIni fileSyncroDBAct = GetIniFileSync(MBBConst.SYNCRO_ACTIVITIES, Path.GetTempPath());
            CRSIni fileSyncroDBActLog = GetIniFileSync(MBBConst.SYNCRO_ACTIVITIES_LOG, Path.GetTempPath());

            if(startSession<=DateTime.MinValue) 
                startSession = DateTime.Now;
            // copia di backup del precedente se esiste, ed elimino il file per poi ricrearlo
            if (fileSyncroDB != null && File.Exists(fileSyncroDB.Name))
            {
                FileInfo f = new FileInfo(fileSyncroDB.Name);
                if (f.Length > 0)
                    File.Copy(fileSyncroDB.Name, Path.GetFileNameWithoutExtension(fileSyncroDB.Name) + "_" + startSession.Year.ToString() + startSession.Month.ToString().PadLeft(2, '0') + startSession.Day.ToString().PadLeft(2, '0')
                    + "_" + startSession.Hour.ToString().PadLeft(2, '0') + startSession.Minute.ToString().PadLeft(2, '0') + startSession.Second.ToString().PadLeft(2, '0')
                    + "." + Path.GetExtension(fileSyncroDB.Name));

                if (fileSyncroDB != null) File.Delete(fileSyncroDB.Name);
            }

            if (fileSyncroDBDel != null && File.Exists(fileSyncroDBDel.Name))
            {
                FileInfo f = new FileInfo(fileSyncroDBDel.Name);
                if (f.Length > 0)
                    File.Copy(fileSyncroDBDel.Name, Path.GetFileNameWithoutExtension(fileSyncroDBDel.Name) + "_" + startSession.Year.ToString() + startSession.Month.ToString().PadLeft(2, '0') + startSession.Day.ToString().PadLeft(2, '0')
                    + "_" + startSession.Hour.ToString().PadLeft(2, '0') + startSession.Minute.ToString().PadLeft(2, '0') + startSession.Second.ToString().PadLeft(2, '0')
                    + Path.GetExtension(fileSyncroDB.Name));

                if (fileSyncroDBDel != null) File.Delete(fileSyncroDBDel.Name);
            }

            if (fileSyncroDBDoc != null && File.Exists(fileSyncroDBDoc.Name) )
            {
                FileInfo f = new FileInfo(fileSyncroDBDoc.Name);
                if(f.Length>0)
                    File.Copy(fileSyncroDBDoc.Name, 
                        Path.GetFileNameWithoutExtension(fileSyncroDBDoc.Name) + "_" + startSession.Year.ToString() + startSession.Month.ToString().PadLeft(2, '0') + startSession.Day.ToString().PadLeft(2, '0')
                        + "_" + startSession.Hour.ToString().PadLeft(2, '0') + startSession.Minute.ToString().PadLeft(2, '0') + startSession.Second.ToString().PadLeft(2, '0')
                        + Path.GetExtension(fileSyncroDB.Name));

                if (fileSyncroDBDoc != null) File.Delete(fileSyncroDBDoc.Name);
            }

            if (fileSyncroDBAct != null && File.Exists(fileSyncroDBAct.Name))
            {
                FileInfo f = new FileInfo(fileSyncroDBAct.Name);
                if (f.Length > 0)
                    File.Copy(fileSyncroDBAct.Name, Path.GetFileNameWithoutExtension(fileSyncroDBAct.Name) + "_" + startSession.Year.ToString() + startSession.Month.ToString().PadLeft(2, '0') + startSession.Day.ToString().PadLeft(2, '0')
                    + "_" + startSession.Hour.ToString().PadLeft(2, '0') + startSession.Minute.ToString().PadLeft(2, '0') + startSession.Second.ToString().PadLeft(2, '0')
                    + Path.GetExtension(fileSyncroDBAct.Name));

                if (fileSyncroDBAct != null) File.Delete(fileSyncroDBAct.Name);
            }

            if (fileSyncroDBActLog != null && File.Exists(fileSyncroDBActLog.Name))
            {
                FileInfo f = new FileInfo(fileSyncroDBActLog.Name);
                if (f.Length > 0)
                    File.Copy(fileSyncroDBActLog.Name, Path.GetFileNameWithoutExtension(fileSyncroDBActLog.Name) + "_" + startSession.Year.ToString() + startSession.Month.ToString().PadLeft(2, '0') + startSession.Day.ToString().PadLeft(2, '0')
                    + "_" + startSession.Hour.ToString().PadLeft(2, '0') + startSession.Minute.ToString().PadLeft(2, '0') + startSession.Second.ToString().PadLeft(2, '0')
                    + Path.GetExtension(fileSyncroDBActLog.Name));

                if (fileSyncroDBActLog != null) File.Delete(fileSyncroDBActLog.Name);
            }
        }

        public static bool SyncroSendToEmail(List<DbNoteStatus> NotesModToSend, MBBUserInfo user)
        {
            bool res = false;
            try
            {
                if (NotesModToSend.Count > 0)
                {
                    // se i destinatari sono impostati manda email
                    MBBEmailParams p = MBBCommon.GetEmailParamsFromIni();
                    string appName = CRSIniFile.GetApplicationNameFromIni();
                    string appVers = CRSIniFile.GetApplicationVersionFromIni();
                    string pwd = CRSLicense.DecodeCode(appName, appVers, user.Password, 1);
                    CRSMailRepository rep = new CRSMailRepository(p.ImapServer, p.ImapPort, p.SmtpServer, p.SmtpPort
                        , true, user.Name, pwd, user.RegistryName + " " + user.RegistrySurname);
                    // 1) prepara file attach da inviare
                    // 2) prepara allegati (docs da inviare)
                    // 3) invia email a te stesso
                    List<CRSIni> lstf = MBBSyncroUtility.PrepareFileSyncroToSend();
                    List<string> lstfilestosend = new List<string>();

                    // inserisco nella lista dei documenti anche il file con le note da aggiornare
                    lstfilestosend.AddRange(lstf.Select(X=>X.Name).Distinct());
                    // inserisco i path dei documenti: SendMail provvede a recuperare i file e ad allegarli sull'email
                    foreach (DbNote n in NotesModToSend)
                    {
                        if(n.Docs != null && n.Docs.Count() > 0)
                            lstfilestosend.AddRange(n.Docs.Select(X => X.FileNameLocal).ToList());
                    }
                    res = rep.SendMail(user.Name, "In attached files notes and docs to syncronize..", "MBB SYNCRO NOTES", lstfilestosend);

                    if (!res)
                    {
                        foreach (CRSIni i in lstf)
                        {
                            File.Copy(i.Name, GetFileNameFromDate(Path.GetFileName( i.Name)
                                , Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\MAIL_NOT_SENT"
                                , DateTime.Now));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                res = false;
            }

            return res;
        }

        public static bool SyncroReadFromEmail(MBBUserInfo user)
        {
            bool res = false;
            string msgout = "";
            List<DbNote> NotesModToSend = new List<DbNote>();
            try
            {
                MBBEmailParams p = MBBCommon.GetEmailParamsFromIni();
                string appName = CRSIniFile.GetApplicationNameFromIni();
                string appVers = CRSIniFile.GetApplicationVersionFromIni();
                string pwd = CRSLicense.DecodeCode(appName, appVers, user.Password, 1);
                // contiene la lista dei file syncroDbDOCS allegati a tutte le email: serve per poi identificare le note e copiare i file nelle cartelle interne (dropbox)
                List<string> lstdocs = new List<string>();

                CRSMailRepository rep = new CRSMailRepository(p.ImapServer, p.ImapPort, p.SmtpServer, p.SmtpPort
                        , true, user.Name, pwd, user.RegistryName + " " + user.RegistrySurname);

                // 1) recupera ultime email dalla casella di posta (potrebbe esserci più di una)
                // 2) dall'email più vecchia recupera allegati
                // 3) dagli allegati salva dati su DB

                // la prima volta prendo un intervallo di tempo ampio (2 mesi) per analizzare le email arrivate dai diversi pc
                long dtfiletime = Convert.ToInt64(CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_LASTSYNCRO_EMAIL, DateTime.Now.AddDays(-60).ToFileTime()));
                DateTime dateFrom = DateTime.FromFileTime(dtfiletime);
                var lst = rep.GetMails("MBB SYNCRO NOTES", user.Name, dateFrom, false, out msgout);
                if (!string.IsNullOrEmpty(msgout)) 
                    CRSLog.WriteLog(msgout, user.Name);
                if (lst != null && lst.Count() > 0)
                {
                    foreach (MimeMessage msg in lst.OrderBy(X => X.Date).ToList())
                    {
                        // per ogni mail scarica gli allegati
                        DateTime dt = DateTime.Now;
                        // genera directory in cui salvare gli allegati
                        DirectoryInfo dir = Directory.CreateDirectory(Path.Combine(MBBConst.DIR_PATH_SYNCRO_EMAIL_DOWNLOAD
                                   , dt.Year.ToString() + "-" + dt.Month.ToString().PadLeft(2, '0') + "-" + dt.Day.ToString().PadLeft(2, '0')
                                   + "_" + dt.Hour.ToString().PadLeft(2, '0') + dt.Minute.ToString().PadLeft(2, '0') + dt.Second.ToString().PadLeft(2, '0')
                                   + dt.Millisecond.ToString()
                                   ));

                        foreach (MimeEntity mime in msg.Attachments)
                        {
                            var pathFileName = "";
                            if (mime is MessagePart)
                            {
                                pathFileName = mime.ContentDisposition?.FileName;
                                var rfc822 = (MessagePart)mime;

                                if (string.IsNullOrEmpty(pathFileName))
                                    pathFileName = "attached-message.eml";

                                using (var stream = File.Create(pathFileName))
                                    rfc822.Message.WriteTo(stream);
                            }
                            else
                            {
                                var part = (MimePart)mime;
                                pathFileName = Path.Combine(dir.FullName, part.FileName);

                                using (var stream = File.Create(pathFileName))
                                    part.Content.DecodeTo(stream);
                                // RECUPERO SOLO I DOCUMENTI per poi spostarli nelle cartelle di dropbox
                                if (!pathFileName.Contains("syncroDbDOCS")&& !pathFileName.Contains("syncroDbUPD") && !pathFileName.Contains("syncroDbDEL"))
                                    lstdocs.Add(pathFileName);
                            }

                            #region VECCHIO ESEMPIO NON FUNZIONANTE: crea il file ma non lo riempie
                            /*System.IO.MemoryStream ms = new System.IO.MemoryStream();
                            ms.Position = 0;
                            mime.WriteTo(ms);

                            if (mime.ContentType.Name.Contains("syncroDbUPD"))
                            {
                                using (var fileStream = new FileStream(
                                    Path.Combine(dir.FullName, "syncroDbUPD.txt"), FileMode.CreateNew, FileAccess.ReadWrite))
                                {
                                    ms.CopyTo(fileStream); // fileStream is not populated
                                }
                            }
                            else
                            if (mime.ContentType.Name.Contains("syncroDbDEL"))
                            {
                                using (var fileStream = new FileStream(
                                    Path.Combine(dir.FullName, "syncroDbDEL.txt"), FileMode.CreateNew, FileAccess.ReadWrite))
                                {
                                    ms.CopyTo(fileStream); // fileStream is not populated
                                }
                            }
                            else
                            {
                                string fileName = Path.Combine(dir.FullName, mime.ContentType.Name);
                                using (var fileStream = new FileStream(fileName, FileMode.CreateNew, FileAccess.ReadWrite))
                                {
                                    ms.CopyTo(fileStream); // fileStream is not populated
                                }
                            }

                            ms.Close();*/
                            #endregion
                        }
                        string info;
                        // basta che l'ora sia inferiore
                        DateTime newdtSyncro;
                        bool ressync = SyncroDBFromFile(dir.FullName, DateTime.FromFileTime(MBBSyncroUtility.GetStartSessionTime()).AddMinutes(-1), 0,false, false, out info, out newdtSyncro);
                        CRSLog.WriteLog("Executed method SyncroDBFromFile => imported notes: " +  info , user.Name);
                        // se è tutto ok i documenti in allegato devono essere gli stessi elencati nel file syncroDbDOCS.txt
                        CRSIni fileSyncroDB_DOCS = GetIniFileSync(MBBConst.SYNCRO_DOCS, dir.FullName);
                        List<DbNote> lstnotes = ReadFileDocGetNotes(fileSyncroDB_DOCS, DateTime.MinValue, DateTime.Now);
                        
                        // devo copiare i file dei documenti allegati alle nota nella cartella locale (normalmente quella di drop box)

                        //-- da 03/2018: LOGICA
                        //  1) in directory di dropbox copio il file
                        //      - se NON è condivisdo nella dir dell'utente
                        //      - se è condiviso in una dir generica (DOCS)
                        //  2) quando l'utente apre il file salvato con la nota modifica il file di lavoro copiato in dropbox
                        //      - in questo modo il file originale rimane intonso

                        // UPLOAD FISICO DEL FILE SU DIRECTORY DROPBOX
                        //foreach(string f in lstdocs)
                        foreach (DbNote n in lstnotes)
                        {
                            foreach (DbNoteDoc d in n.Docs)
                            {
                           
                                string intDirRemote = MBBSyncroUtility.MakeFolderNameForDocs(user.Name, false);
                                // UPLOAD FISICO DEL FILE SU DIRECTORY DROPBOX
                                string fileName = d.DocName;
                                string sourceDir = dir.FullName;
                                //File.Copy(Path.Combine(dir.FullName, fileName), destFileName, true);
                                int ver = CRSFileUtil.UploadFile(MBBCommon.syncroParams.DropBoxRootDir, sourceDir, ref fileName, intDirRemote, true);
                                // se il nome è cambiato (essendoci già un file diverso con lo stesso nome)
                                if (fileName != d.DocName)
                                    res = DbDataUpdate.UpdateDocInfo(d.IDNote, fileName, ver, "", "", user.IDUser, DateTime.MinValue, "");
                            }
                        }

                        // CLEAN: se l'importazione è terminata a buon fine non serve che tenga la cartella
                        if (ressync)
                        {
                            if (File.Exists(Path.Combine(dir.FullName, MBBConst.FILE_NAME_UPD)))
                                File.Delete(Path.Combine(dir.FullName, MBBConst.FILE_NAME_UPD));
                            if (File.Exists(Path.Combine(dir.FullName, MBBConst.FILE_NAME_DEL)))
                                File.Delete(Path.Combine(dir.FullName, MBBConst.FILE_NAME_DEL));
                            if (File.Exists(Path.Combine(dir.FullName, MBBConst.FILE_NAME_DOCS)))
                                File.Delete(Path.Combine(dir.FullName, MBBConst.FILE_NAME_DOCS));
                            if (File.Exists(Path.Combine(dir.FullName, MBBConst.FILE_NAME_ACT)))
                                File.Delete(Path.Combine(dir.FullName, MBBConst.FILE_NAME_ACT));
                            if (File.Exists(Path.Combine(dir.FullName, MBBConst.FILE_NAME_ACTLOG)))
                                File.Delete(Path.Combine(dir.FullName, MBBConst.FILE_NAME_ACTLOG));
                            if(Directory.Exists(dir.FullName))
                                Directory.Delete(dir.FullName);
                        }
                    }
                    // alla fine aggiorno la data indicante l'ultima sincrionizzazione con email
                    CRSIniFile.SetPropertyValue(APPConstants.SEC_MYBLACKBOX, APPConstants.PROP_LASTSYNCRO_EMAIL, MBBSyncroUtility.GetStartSessionTime());
                    res = true;
                }
                else
                {
                    CRSLog.WriteLog("Nessuna email trovata per subject MBB SYNCRO NOTES", user.Name);
                }
            }
            catch(Exception ex)
            {
                msgout = ex.Message;
                CRSLog.WriteLog("Exception in SyncroReadFromEmail: \n" + msgout, user.Name);
                res = false;
            }

            return res;
        }
        #endregion

        #region FTP SYNCRO

        #endregion

        public static string GetFileNameFromDate(string nameFileOrigin, string dirDest, DateTime dt)
        {
            return Path.Combine(dirDest, nameFileOrigin + "_" + dt.Year.ToString() + dt.Month.ToString().PadLeft(2, '0') + dt.Day.ToString().PadLeft(2, '0')
                                  + "_" + dt.Hour.ToString().PadLeft(2, '0') + dt.Minute.ToString().PadLeft(2, '0') + dt.Second.ToString().PadLeft(2, '0')
                                  );
            ;
        }
    }
}
