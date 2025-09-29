using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRS.Library;
using MyBlackBoxCore.DataEntities;
using System.Data.Common;
using CRS.DBLibrary;
using System.Data;
using System.IO;

namespace MyBlackBoxCore
{
    public static class DbDataUpdate
    {
        private static string connectionString;
        private static string providerName;

        public static void Init(string connstring, string provider)
        {
            connectionString = connstring;
            providerName = provider;
        }

        /// <summary>
        ///  Aggiorna il db se nel file che passo ci sono stringhe che terminano con GO..
        ///  Da GO a GO trovo le istruzioni sql di aggiornamento del db con ALTER e quant'altro
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static bool UpgradeDB_Schema(string filename)
        {
            bool res = false;
            try
            {
                DbFactory conn = new DbFactory(connectionString, providerName);
                List<string> lines = File.ReadAllLines(filename).ToList<string>();
                List<string> linesexec = new List<string>();
                // AGGIUNGO LE RIGHE COMMENTATE DATO CHE NEL FILE DI DESTINAZIONE SUCCESSIVAMENTE MI SERVIRANNO PER CAPIRE 
                // se sono state eseguite oppure no
                linesexec.AddRange(lines.Where(X => !string.IsNullOrEmpty(X) && X[0] == ';').ToList());
                lines = lines.Where(X => !string.IsNullOrEmpty(X) && X[0] != ';').ToList();

                if (lines.Count > 0)
                {
                    int idxfirst = 0;
                    int idx = lines.IndexOf("GO", idxfirst);

                    while (idx >= 0 && idxfirst < idx)
                    {
                        string commandsql = "";
                        try
                        {
                            List<string> s = lines.GetRange(idxfirst, idx - idxfirst);
                            commandsql = string.Join(" ", s);
                            bool resupd = conn.ExecuteCommandSql(commandsql);

                            idxfirst = idx + 1;
                            idx = lines.IndexOf("GO", idxfirst);
                            linesexec.Add(";" + commandsql);
                            linesexec.Add(";GO");
                        }
                        catch (Exception ex)
                        {
                            res = false;
                            CRSLog.WriteLog("Exception UpgradeDB_Schema in command sql: " + commandsql  + "\n"+ ex.Message, "MBB");
                        }
                    }
                }
                res = true;
                if(linesexec.Count>0)
                    File.WriteAllLines(filename, linesexec);
            }
            catch (Exception ex)
            {
                res = false;
                CRSLog.WriteLog("Exception UpgradeDB_Schema: " + ex.Message, "MBB");
            }
            return res;
        }


        /// <summary>
        /// Data una nota con un testo, inserisce la nota nel DB.
        /// Nel caso che il testo abbia un numero di caratteri >3000, splitta la nota e inserisce su DB tante note
        /// quanto è il valore text.length / 3000.
        /// NB: idNote deve essere valorizzato sempre, anche nell'inserimento di una nuova nota.
        /// Se non è così viene dedotto dalla data di inserimento: essendo un Int64 risulta univoco in tutto il sistema se prevedo di inserire 
        /// una nota almeno in momenti diversi scanditi al secondo. Comunque essendo la chiave user/id sono sicuro che la stessa chiave sia univoca.
        /// </summary>
        /// <param name="idnote"></param>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="iduser"></param>
        /// <param name="iduserShare"></param>
        /// <param name="rate"></param>
        /// <param name="isFavorite"></param>
        /// <param name="dtIns"></param>
        /// <param name="manageDocs"></param>
        /// <param name="lstDocs"></param>
        /// <param name="isNoteShared"></param>
        /// <param name="userOwner"></param>
        /// <param name="lstNotesIns"></param>
        /// <param name="lstNotesDel"></param>
        /// <returns></returns>
        public static Int64 InsertUpdateNoteWithSplit( 
            Int64 idnote 
            , string title, string text, int iduser, int iduserShare, int rate, bool isFavorite
            ,DateTime dtIns, DateTime dtLastMod
            ,bool manageDocs
            ,List<DbNoteDoc> lstDocs, bool isNoteShared, string userOwner
            ,out List<DbNote> lstNotesIns, out List<DbNote> lstNotesDel
            , bool insOnlyNewID = false
            )
        {
            lstNotesIns = new List<DbNote>();
            lstNotesDel = new List<DbNote>();
            // idnote potrebbe non essere valorizzato (forse lo è sempre)
            // se non è valorizzato determino un valore dalla data di inserimento          
            if(dtIns == DateTime.MinValue) dtIns = DateTime.Now;
            if(idnote<=0) idnote = dtIns.ToFileTime();
            Int64 idres = idnote;
            bool res1;
            // aggiungo lo spazio per poter calcolare correttamente l'indice dell'ultimo spazio sul testo
            if (text.LastIndexOf(' ') < text.Length)
                text = text + ' ';
            string txttmp = text;
            int numcharssaved = 0;
            bool delupd = false;

            Int64 idnoteFirst = idnote;
            bool firstSplitInserted = false;

            DbNote existentNote = DbDataAccess.GetNote(iduser, idnote);
            bool existsNote = existentNote!=null && existentNote.ID > 0;

            // con insOnlyNewID: INSERISCO solo se non impongo di sovrascrivere note con lo stesso ID presenti nel DB (è il caso standard: normalemnte inserisco o aggiorno)
            // - è utile se decido di importare da un vecchio file, dato che all'inizio l'id non era bassato sulla data e poteva non essere univoco
            if (!existsNote || (existsNote && !insOnlyNewID))
            {
                while (txttmp.Length > 0)
                {
                    int idxend = txttmp.LastIndexOf(' ');
                    string txttmp3000 = "";

                    #region SPLIT NOTA LUNGA 
                    if (idxend >= 3000)
                    {
                        // trovo la stringa <= 3000 che termina con uno spazio in modo da non troncare la parola
                        txttmp3000 = txttmp.Substring(0, 3000);
                        idxend = txttmp3000.LastIndexOf(' ');
                        txttmp = txttmp.Substring(0, idxend);
                        
                        // se sono in questa condizione, cioè la stringa è più lunga di 3000
                        // elimino la nota da aggiornare (una volta sola) e rifaccio tutto
                        if (!delupd && existsNote)
                        {
                            // mi serve recuperare la data di inserimento per non modificare l'ordinamento temporale della nota 
                            dtIns = existentNote.DateTimeInserted;
                            lstNotesDel.Add(existentNote);
                            // elimina la nota e le sue figlie splittate dall'originale
                            DbDataUpdate.DeleteNote(idnote, iduser, true);
                            delupd = true;
                        }
                        
                    }
                    #endregion

                    #region SALVA SU DB
                    // se sto inserendo una nota nuova, oppure sono in modifica e la nota è lunga (vuol dire che l'ho eliminata) e consento l'aggiornamento di note esistenti con lo stesso id
                    if (!existsNote || (existsNote && delupd))
                    {
                        // dato che è possibile che una nota venga splittata, l'id è calcolato con la datIns.ToFileTime che è sempre lo stesso:
                        // devo quindi incrementarlo ad ogni passaggio in modo che risulti diverso
                        if (idnoteFirst <= 0) idnoteFirst = idnote;
                        if (firstSplitInserted)
                        {
                            dtIns = dtIns.AddSeconds(1);
                            idnote = -1;
                        }
                        // inserisco la nota con l'idnote che passo come parametro (nel file di testo è valorizzato in modo che sia univoco per lo user attuale)
                        idnote = DbDataUpdate.InsertNote(idnote
                            , title, txttmp, iduser, iduserShare, idnoteFirst, "", rate, dtIns);

                        firstSplitInserted = true;
                        idres = idnoteFirst;
                        lstNotesIns.Add(DbDataAccess.GetNote(iduser, idnote));
                    }
                   
                    // SITUAZIONE PIU' FREQUENTE: se sono in aggiornamento e la nota non è lunga più di 3000
                    if (existsNote && !delupd)
                    {
                        res1 = DbDataUpdate.UpdateNote(idnote, title, text, iduser, iduserShare, idnote, "", rate
                            , dtLastMod, dtIns, false, false, isFavorite, true);
                        lstNotesIns.Add(DbDataAccess.GetNote(iduser, idnote));
                    }
                    
                    #endregion

                    #region RICALCOLO PARTE RIMENENTE TESTO (se i caratteri sono + di 3000)
                    numcharssaved = numcharssaved + txttmp.Length;
                    // il nuovo segmento di stringa è quello che rimane più i prossimi 3000 char di text (l'originale)
                    if (text.Length > numcharssaved)
                    {
                        txttmp = text.Substring(numcharssaved);
                    }
                    // dovrebbe essere superfluo
                    else
                    {
                        // può capitare solo se la nota da salvare termina al char 3000 con uno spazio 
                        if (txttmp3000.Length > idxend)
                            txttmp = txttmp3000.Substring(idxend).Trim() + " ";
                        else txttmp = "";
                    }
                    #endregion
                }

                // inserisco i documenti solo per la prima nota: solo se decido di gestire anche i documenti (potrebbe non essere opportuno dato che se
                // chiamo questo metodo per aggiornare nota da file di testo i documenti sono gestiti in altro modo)
                if (manageDocs)
                {
                    bool resDoc = DbDataUpdate.DeleteDocsNote(idnoteFirst, iduser);
                    string intDirRemote = MBBSyncroUtility.MakeFolderNameForDocs(userOwner, isNoteShared, -1);
                    foreach (DbNoteDoc docnote in lstDocs)
                        resDoc = DbDataUpdate.InsertDoc(idnoteFirst, docnote.DocName, docnote.Version, docnote.DateTimeLastMod, intDirRemote, docnote.FileNameLocal, iduser, true);
                }
            }
            return idres;
        }
       
        public static Int64 InsertNote(Int64 idnote, string title, string text, int iduser, int iduserShare, Int64 idnoteparent, string userParent, int rate, DateTime dtIns)
        {
            try
            {
                DbFactory conn = new DbFactory(connectionString, providerName);

                bool res1 = false;
                //-- viene passato nel caso della sincronizzazione del db
                Int64 id = idnote;
                if (idnote <= 0)
                    //id = conn.GetMaxFieldValInKey("note", "id", new string[] { "iduser" }, new object[] { iduser }) + 1;
                    id = dtIns.ToFileTime();

                if (idnoteparent == 0)
                    idnoteparent = id;

                using (DbConnection dbconn = conn.GetConnection())
                {
                    dbconn.Open();
                    DbTransaction transaction = dbconn.BeginTransaction();
                    res1 = conn.ExecNonQueryOpenConn(dbconn, transaction,
                                    "insert into NOTE " +
                                    " (ID,TITLE,TEXT,DATETIMEINSERTED,DATETIMELASTMOD,IDUSER,RATE,ISPRIVATE,USERNAMEPARENT,IDNOTEPARENT,audituser,AUDITDATETIME) " +
                                    " values (@n, @s, @s, @dt, @dt, @n, @n, @b, @s, @n, @s, @dt) "
                                    , id
                                    , title, text
                                    , dtIns, DateTime.Now
                                    , iduser, rate, false, userParent, idnoteparent, "mbb", DateTime.Now);

                    //int iduserShare = DbDataAccess.GetIDUser(userParent);
                    if (iduserShare > 0 && iduserShare!=iduser)
                    {
                        int c = Convert.ToInt32(conn.GetExecuteScalar("select count(*) from note_sharing where idnote=@n and iduser=@n and idusersharing=@n"
                                , idnote, iduser, iduserShare));
                        if (c == 0)
                        {
                            string usersh = DbDataAccess.GetUser(iduserShare);
                            res1 = conn.ExecNonQueryOpenConn(dbconn, transaction,
                                //conn.ExecuteSqlUID(
                                    "insert into NOTE_sharing " +
                                    " (IDNOTE,IDUSER,IDUSERSHARING,USERNAMESHARING,ISTOREAD) " +
                                    " values (@n, @n, @n, @s, @b) "
                                    , idnote, iduser, iduserShare, usersh, true);
                        }
                    }

                    if (res1)
                        transaction.Commit();
                    else transaction.Rollback();
                }
                return id;
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("Error InserNote: " + ex.Message, "mbb");
                return -1;
            }
        }

        public static Int64 InsertActivity(Int64 idnote, Int64 idactivity
            , string text, string title
            , DateTime startAct, DateTime stopAct, DateTime insAct
            , int totMinPreview, int iduser, string status, string typeAct)
        {
            // l'id note può non essere definito
            // la nuova attività ha idactivity=0 altrimenti la recupero
            // se idnote>0 scrivo in activity_note un record

            // se è attiva la registrazione 
            //  1) creo attività
            //  2) registro tempo inizio
            //  3) ad ogni pressione del tasto del mouse (per selezionare una finestra) o tab per spostarmi su una finestra
            //     registro su db (nuova tabella?) l'applicazione usata focused
            //      - tabella activity_log: IDActivity, nameApp, startusing, stop using
            //      - chiave: idactivity, nameapp, startusing
            //  4) registra solo se l'app corrente è diversa all'app focused
            //      - l'app corrente diventa appfocusedint64 

            DbFactory conn = new DbFactory(connectionString, providerName);

            Int64 id = idactivity;
            if (idactivity == 0)
                id = insAct.ToFileTime();

            bool istoinsnoteact = false;
            if (idnote > 0)
            {
                string sqlexistactnote = "select * from activity_note where idactivity=@n and idnote=@n";
                DataTable dt = conn.ExecuteSql(sqlexistactnote, id, idnote);
                if (dt.Rows.Count == 0)
                    istoinsnoteact = true;
            }

            using (DbConnection dbconn = conn.GetConnection())
            {
                dbconn.Open();
                DbTransaction transaction = dbconn.BeginTransaction();
                bool qryok = conn.ExecNonQueryOpenConn(dbconn, transaction,
                                "insert into ACTIVITY                           " +
                                " (IDActivity,TITLEACTIVITY,TEXTACTIVITY        " +
                                " ,STARTACTIVITY,STOPACTIVITY,DATETIMEINSERTED  " +
                                " ,DATETIMELASTMOD                              " + 
                                " ,TOTMINPREVIEW,STATUS,IDUSER,TYPEACTIVITY)    " +
                                " values (@n, @s, @s, @dt, @dt, @dt, @dt, @n, @s, @n, @s) "
                                , id
                                , title, text
                                , startAct, stopAct, insAct, insAct
                                , totMinPreview
                                , status, iduser, typeAct );


                bool qryok1 = true;
                if (istoinsnoteact)
                {
                    qryok1 = conn.ExecNonQueryOpenConn(dbconn, transaction,
                                //conn.ExecuteSqlUID(
                                "insert into activity_note " +
                                " (IDNOTE,IDACTIVITY) " +
                                " values (@n, @n) "
                                , idnote, id);
                    
                }

                if (qryok)
                    transaction.Commit();
                else transaction.Rollback();
            }
            return id;
        }

        public static Int64 UpdateActivity(Int64 idnote, Int64 idactivity
            , string text, string title
            //, DateTime startAct, DateTime stopAct
            , int totMinPreview
            , int iduser, string status, string typeAct)
        {

            DbFactory conn = new DbFactory(connectionString, providerName);
            Int64 id = idactivity;
           
            bool istoinsnoteact = false;
            if (idnote > 0)
            {
                string sqlexistactnote = "select * from activity_note where idactivity=@n and idnote=@n";
                DataTable dt = conn.ExecuteSql(sqlexistactnote, id, idnote);
                if (dt.Rows.Count == 0)
                    istoinsnoteact = true;
            }

            //string sql = "SELECT * FROM ACTIVITY_LOG WHERE IDACTIVITY=@n";
            //conn.ExecuteSql<DbActivity>(dbconn, transaction,
            using (DbConnection dbconn = conn.GetConnection())
            {
                dbconn.Open();
                DbTransaction transaction = dbconn.BeginTransaction();
                bool qryok = conn.ExecNonQueryOpenConn(dbconn, transaction,
                                "UPDATE ACTIVITY SET                    " +
                                " TITLEACTIVITY=@s                      " +
                                ",TEXTACTIVITY=@s                       " +
                                //",STARTACTIVITY=@dt" +
                                //",STOPACTIVITY=@dt" +
                                ",TOTMINPREVIEW=@n                      " +
                                ",STATUS=@s                             " +
                                ",TYPEACTIVITY=@s                       " +
                                ",DATETIMELASTMOD=@dt                   " +
                                " WHERE IDActivity = @n AND iduser=@n   "
                                , title, text
                                //, startAct, stopAct
                                , totMinPreview
                                , status,typeAct
                                , DateTime.Now
                                , id,iduser); 


                bool qryok1 = true;
                if (istoinsnoteact)
                {
                    qryok1 = conn.ExecNonQueryOpenConn(dbconn, transaction,
                                //conn.ExecuteSqlUID(
                                "insert into activity_note " +
                                " (IDNOTE,IDACTIVITY) " +
                                " values (@n, @n) "
                                , idnote, id);

                }

                if (qryok)
                    transaction.Commit();
                else transaction.Rollback();
            }
            return id;
        }

        public static Int64 UpdateActivityStatus(Int64 idactivity, int iduser, string status)
        {
            DbFactory conn = new DbFactory(connectionString, providerName);
            Int64 id = idactivity;

            using (DbConnection dbconn = conn.GetConnection())
            {
                dbconn.Open();
                DbTransaction transaction = dbconn.BeginTransaction();
                bool qryok = conn.ExecNonQueryOpenConn(dbconn, transaction,
                                "UPDATE ACTIVITY SET                    " +
                                " STATUS=@s                             " +
                                ",DATETIMELASTMOD=@dt                   " +
                                " WHERE IDActivity = @n AND iduser=@n   "
                                , status
                                , DateTime.Now
                                , id, iduser);

                if (qryok)
                    transaction.Commit();
                else transaction.Rollback();
            }
            return id;
        }


        public static void UpdateActivityLog(Int64 idactivity
            , DateTime startAct, DateTime newstartAct, DateTime newstopAct)
        {
            DbFactory conn = new DbFactory(connectionString, providerName);
            List<DbActivityLog> lstlog = DbDataAccess.GetActivityLogs(idactivity);
            DbActivityLog log = lstlog.Where(X =>X.IDActivity==idactivity && X.StartActivityLog == startAct).FirstOrDefault();
            
            using (DbConnection dbconn = conn.GetConnection())
            {
                dbconn.Open();
                DbTransaction transaction = dbconn.BeginTransaction();
                bool qryok = false;
                DateTime datelastmod = newstartAct;
                if(newstopAct>DateTime.MinValue)
                    datelastmod = newstopAct;

                // se non ci sono log inserisci il primo e aggiorna la STARTACTIVITY in activity
                if (log == null || (log != null && log.IDActivity == 0))
                {
                    qryok = conn.ExecNonQueryOpenConn(dbconn, transaction,
                           "update ACTIVITY SET         " +
                           " STARTACTIVITY=@dt          " +
                           " ,DATETIMELASTMOD=@dt          " +
                           " WHERE IDActivity = @n      "  +
                           " AND STARTACTIVITY IS NULL  "
                           , newstartAct, datelastmod, idactivity
                           );
                    qryok = conn.ExecNonQueryOpenConn(dbconn, transaction,
                           "INSERT INTO ACTIVITY_LOG                                    " +
                           "(IDACTIVITY,STARTACTIVITYlog,STOPACTIVITYlog,LOGACTIVITY)   " +
                           " VALUES(@n,@dt,@dt,@s)                                      " 
                           , idactivity
                           , newstartAct, newstopAct, ""
                           );
                }
                else 
                { 
                    qryok = conn.ExecNonQueryOpenConn(dbconn, transaction,
                           "UPDATE ACTIVITY_LOG SET  " +
                           "  STARTACTIVITYlog = @dt " + 
                           " ,STOPACTIVITYlog = @dt " +
                           " WHERE IDActivity = @n   " +
                           " and STARTACTIVITYlog=@dt"
                           , newstartAct
                           , newstopAct
                           , idactivity
                           , startAct);
                    // modifico solo se la data è più recente dell'ultima modifica: è un campo che uso per l'ordinamento e la ricerca
                    // delle attività
                    qryok = conn.ExecNonQueryOpenConn(dbconn, transaction,
                           "update ACTIVITY SET         " +
                           " DATETIMELASTMOD=@dt        " +
                           " WHERE IDActivity = @n      " +
                           " and DATETIMELASTMOD<@dt    "
                           , datelastmod, idactivity, datelastmod
                           );
                }
                if (qryok)
                    transaction.Commit();
                else transaction.Rollback();
            }
        }

        public static long ChangeActivityLog(long idactivity, DateTime startAct, string titlenew)
        {
            DbFactory conn = new DbFactory(connectionString, providerName);
            List<DbActivityLog> lstlog = DbDataAccess.GetActivityLogs(idactivity);
            DbActivityLog log = lstlog.Where(X => X.IDActivity == idactivity && X.StartActivityLog == startAct).FirstOrDefault();
            long idactivityNEW = 0;
            using (DbConnection dbconn = conn.GetConnection())
            {
                dbconn.Open();
                DbTransaction transaction = dbconn.BeginTransaction();
                bool qryok = false;
                // ci deve essere un log da cambiare altrimenti non fa niente
                if (log != null && log.IDActivity != 0)               
                {
                    qryok = conn.ExecNonQueryOpenConn(dbconn, transaction,
                           "DELETE FROM ACTIVITY_LOG   " +
                           " WHERE IDActivity = @n   " +
                           " and STARTACTIVITYlog=@dt"
                           , idactivity
                           , startAct);

                    idactivityNEW = DbDataAccess.GetActivity(titlenew).IDActivity;
                    qryok = conn.ExecNonQueryOpenConn(dbconn, transaction,
                    "INSERT INTO ACTIVITY_LOG                                    " +
                    "(IDACTIVITY,STARTACTIVITYlog,STOPACTIVITYlog,LOGACTIVITY)   " +
                    " VALUES(@n,@dt,@dt,@s)                                      "
                    , idactivityNEW
                    , log.StartActivityLog, log.StopActivityLog, ""
                    );
                }
                if (qryok)
                    transaction.Commit();
                else transaction.Rollback();
            }

            return idactivityNEW;
        }

        public static bool InsertActivityLog(long idactivity, DateTime startAct, DateTime stopAct)
        {
            DbFactory conn = new DbFactory(connectionString, providerName);
            List<DbActivityLog> lstlog = DbDataAccess.GetActivityLogs(startAct,stopAct, false);
            //DbActivityLog log = lstlog.Where(X => X.IDActivity == idactivity && X.StartActivityLog == startAct).FirstOrDefault();
            bool res = false;
            using (DbConnection dbconn = conn.GetConnection())
            {
                dbconn.Open();
                DbTransaction transaction = dbconn.BeginTransaction();
                //bool qryok = false;
                // controlla che non ci siano altri log nell'intervallo temporale
                if (lstlog.Count() == 0 && startAct<stopAct)
                {
                    res = conn.ExecNonQueryOpenConn(dbconn, transaction,
                    "INSERT INTO ACTIVITY_LOG                                    " +
                    "(IDACTIVITY,STARTACTIVITYlog,STOPACTIVITYlog,LOGACTIVITY)   " +
                    " VALUES(@n,@dt,@dt,@s)                                      "
                    , idactivity
                    , startAct, stopAct, ""
                    );
                }
                if (res)
                    transaction.Commit();
                else transaction.Rollback();
            }
            return res;
        }

        /// <summary>
        /// idactivity<0 identifica le activities empty: normalmente l'id è -1
        /// </summary>
        /// <param name="idactivity"></param>
        /// <param name="startAct"></param>
        /// <param name="stopAct"></param>
        public static void UpdateEmptyActivityLog(Int64 idactivity
            , DateTime startAct, DateTime stopAct, int iduser)
        {
            DbFactory conn = new DbFactory(connectionString, providerName);
            DbActivity emptyact = DbDataAccess.GetActivity(idactivity);
            List<DbActivityLog> lstlog = DbDataAccess.GetActivityLogs(idactivity);
            
            // se non esiste l'attività empty creala
            if (emptyact == null || (emptyact != null && emptyact.IDActivity >= 0))
            {
                InsertActivity(-1, -1, "", "Empty activity", startAct, stopAct, DateTime.Now, 0, iduser, "Waiting", "Empty");
            }

            using (DbConnection dbconn = conn.GetConnection())
            {
                dbconn.Open();
                DbTransaction transaction = dbconn.BeginTransaction();
                bool qryok = false;

                // CASO 1: non esiste ancora il primo log oppure passo entrambi i poarametri di data => lo inserisco
                if (lstlog.Count() == 0 || (startAct > DateTime.MinValue && stopAct > DateTime.MinValue))
                {
                    qryok = conn.ExecNonQueryOpenConn(dbconn, transaction,
                              "INSERT INTO ACTIVITY_LOG                                    " +
                              "(IDACTIVITY,STARTACTIVITYlog,STOPACTIVITYlog,LOGACTIVITY)   " +
                              " VALUES(@n,@dt,@dt,@s)                                      "
                              , idactivity
                              , startAct, stopAct, ""
                              );
                }
                // CASO 2: passo come parametro data start e minValue per data stop: inizio una nuova attività (insert)
                else
                if (startAct > DateTime.MinValue && stopAct==DateTime.MinValue)
                {
                    qryok = conn.ExecNonQueryOpenConn(dbconn, transaction,
                            "INSERT INTO ACTIVITY_LOG                    " +
                            "(IDACTIVITY,STARTACTIVITYlog,LOGACTIVITY)   " +
                            " VALUES(@n,@dt,@s)                          "
                            , idactivity
                            , startAct, ""
                            );
                }
                // CASO 3: passo come parametro data stop e minValue per data start: aggiorno lo stop dell'ultima attività
                else
                if (startAct == DateTime.MinValue && stopAct > DateTime.MinValue)
                {
                    DbActivityLog lastlog = lstlog.OrderBy(X=>X.StartActivityLog).Last();

                    qryok = conn.ExecNonQueryOpenConn(dbconn, transaction,
                           "UPDATE ACTIVITY_LOG SET     " +
                           " STOPACTIVITYlog=@dt        " +
                           " WHERE IDActivity = @n      " +
                           " AND STARTACTIVITYlog = @dt "
                           , stopAct
                           , idactivity
                           , lastlog.StartActivityLog);
                }
                if (qryok)
                    transaction.Commit();
                else transaction.Rollback();
            }
        }

        public static void InsertActivityNote(long idnote, long idactivity)
        {
            DbActivityNote an = DbDataAccess.GetActivityNote(idactivity, idnote);

            if (an==null || (an != null && an.IDActivity == 0))
            {
                DbFactory conn = new DbFactory(connectionString, providerName);

                using (DbConnection dbconn = conn.GetConnection())
                {
                    dbconn.Open();
                    DbTransaction transaction = dbconn.BeginTransaction();
                    bool qryok1 = conn.ExecNonQueryOpenConn(dbconn, transaction,
                                    //conn.ExecuteSqlUID(
                                    "insert into activity_note " +
                                    " (IDNOTE,IDACTIVITY) " +
                                    " values (@n, @n) "
                                    , idnote, idactivity);
                    if (qryok1)
                        transaction.Commit();
                    else transaction.Rollback();
                }
            }
        }

        public static void DeleteActivityNote(long idnote, long idactivity)
        {
            DbFactory conn = new DbFactory(connectionString, providerName);

            using (DbConnection dbconn = conn.GetConnection())
            {
                dbconn.Open();
                DbTransaction transaction = dbconn.BeginTransaction();
                bool qryok1 = conn.ExecNonQueryOpenConn(dbconn, transaction,
                                //conn.ExecuteSqlUID(
                                "delete from activity_note          " +
                                " where IDNOTE=@n and IDACTIVITY=@n " 
                                , idnote, idactivity);
                if (qryok1)
                    transaction.Commit();
                else transaction.Rollback();
            }
        }

        public static void DeleteActivity(Int64 iduser, Int64 idactivity)
        {
            DbFactory conn = new DbFactory(connectionString, providerName);

            using (DbConnection dbconn = conn.GetConnection())
            {
                dbconn.Open();
                DbTransaction transaction = dbconn.BeginTransaction();
                bool qryok = conn.ExecNonQueryOpenConn(dbconn, transaction,
                                "DELETE FROM ACTIVITY WHERE IDActivity = @n AND iduser=@n "
                                , idactivity, iduser);
                qryok = conn.ExecNonQueryOpenConn(dbconn, transaction,
                                "DELETE FROM ACTIVITY_LOG WHERE IDActivity = @n  "
                                , idactivity);
                qryok = conn.ExecNonQueryOpenConn(dbconn, transaction,
                                "DELETE FROM ACTIVITY_NOTE WHERE IDActivity = @n  "
                                , idactivity);
                if (qryok)
                    transaction.Commit();
                else transaction.Rollback();
            }

        }

        public static void DeleteActivityLog( Int64 idactivity, DateTime startActivityLog)
        {
            DbFactory conn = new DbFactory(connectionString, providerName);

            using (DbConnection dbconn = conn.GetConnection())
            {
                dbconn.Open();
                DbTransaction transaction = dbconn.BeginTransaction();
                bool qryok = conn.ExecNonQueryOpenConn(dbconn, transaction,
                                "DELETE FROM ACTIVITY_LOG WHERE IDActivity = @n and startactivitylog = @dt "
                                , idactivity, startActivityLog);

                if (qryok)
                    transaction.Commit();
                else transaction.Rollback();
            }
        }

        public static int GetNewIDNote(int iduser)
        {
            DbFactory conn = new DbFactory(connectionString, providerName);
            int id = 1;
            id = conn.GetMaxFieldValInKey("note", "id", new string[] { "iduser" }, new object[] { iduser }) + 1;
            return id;
        }

        public static bool UpdateNote(Int64 idnote, string title, string text, int iduser, int iduserShare, Int64 idnoteparent, string userparent, int rate, DateTime dtLastMod, DateTime dtIns, bool ispriv, bool withKeys, bool isfavorite, bool updateOnlyLastModGreaterThan)
        {
            try
            {
                DbFactory conn = new DbFactory(connectionString, providerName);

                bool res1 = false;
                bool res2 = true;

                using (DbConnection dbconn = conn.GetConnection())
                {
                    dbconn.Open();
                    DbTransaction transaction = dbconn.BeginTransaction();

                    // aggiorno solo le note più recenti: seve se faccio aggiornamento da file diversi e su file diversi ho date di ultimo aggiornamento diverse
                    // => quindi se import da file "vecchio" non devo sovrascrivere nota scritta su db più recentemente
                    string whlastmod = "";
                    if (updateOnlyLastModGreaterThan)
                    {
                        whlastmod = " and DATETIMELASTMOD<@dt ";
                        whlastmod = DbUtils.PrepareSqlStatement(providerName, whlastmod, dtLastMod);
                    }

                    res1 = conn.ExecNonQueryOpenConn(dbconn, transaction,
                                    "update NOTE set " +
                                    " TITLE=@s,TEXT=@s,DATETIMELASTMOD=@dt,DATETIMEINSERTED=@dt,RATE=@n,isprivate=@b,idnoteparent=@n,usernameparent=@s,audituser=@s,AUDITDATETIME=@dt,isfavorite=@b " +
                                    " where id=@n and iduser=@n" 
                                     + whlastmod
                                    , title, text
                                    , dtLastMod, dtIns, rate, ispriv, idnoteparent, userparent,"mbb", DateTime.Now, isfavorite ,idnote,iduser);

                    if (iduserShare > 0 && iduser != iduserShare)
                    {
                        int c = Convert.ToInt32(conn.GetExecuteScalar("select count(*) from note_sharing where idnote=@n and iduser=@n and idusersharing=@n"
                                , idnote, iduser, iduserShare));
                        string usrsh = DbDataAccess.GetUser(iduserShare);
                        if (c == 0)
                        {
                           
                            res1 = conn.ExecNonQueryOpenConn(dbconn, transaction,
                                    "insert into NOTE_sharing " +
                                    " (IDNOTE,IDUSER,IDUSERSHARING,USERNAMESHARING,ISTOREAD) " +
                                    " values (@n, @n, @n, @s, @b) "
                                    , idnote, iduser, iduserShare, usrsh, true);
                        }
                        else
                        {
                            res1 = conn.ExecNonQueryOpenConn(dbconn, transaction,
                                    "UPDATE NOTE_sharing SET " +
                                    " ISTOREAD=@b " +
                                    " WHERE IDNOTE=@n AND iduser=@n and idusersharing=@n"
                                    , true, idnote, iduser, iduserShare);

                        }
                    }

                    List<string> words = title.Split(MBBCommon.GetSeparationCharTitleParam(), StringSplitOptions.RemoveEmptyEntries).ToList();
                    
                    res2 = conn.ExecNonQueryOpenConn(dbconn, transaction,
                                        "delete from NOTE_KEY " +
                                        " where idnote=@n and iduser=@n"
                                        , idnote,iduser);

                    if (withKeys)
                    {
                        foreach (string s in words)
                        {
                            if (string.IsNullOrEmpty(s) == false)
                            {
                                res2 = conn.ExecNonQueryOpenConn(dbconn, transaction,
                                            "insert into NOTE_KEY " +
                                            " (IDnote,IDUser,Name,level) " +
                                            " values (@n, @n, @s, @n) "
                                            , idnote, iduser, s.Trim(), CRSGlobalParams.LEVEL_KEY_TITLE_NOTE);

                                if (!res2) break;
                            }
                        }
                    }
                    
                    if (res1 && res2)
                        transaction.Commit();
                    else transaction.Rollback();
                }
                return res1 && res2;
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("Error UpdateNote: " + ex.Message, "mbb");
                return false;
            }
        }

        public static bool UpdateSharedNoteRead(Int64 idnote, int iduserowner, string sharinguser, bool istoread)
        {
            try
            {
                DbFactory conn = new DbFactory(connectionString, providerName);

                bool res1 = false;
                bool res2 = false;
                List<DbNote> parentnote = new List<DbNote>();
                using (DbConnection dbconn = conn.GetConnection())
                {
                    dbconn.Open();
                    DbTransaction transaction = dbconn.BeginTransaction();
                    string sqlupd = "update NOTE_SHARING set " +
                                    " IsToRead=@b " +
                                    " where idnote=@n and iduser=@n and usernamesharing=@s";

                    res1 = conn.ExecNonQueryOpenConn(dbconn, transaction,
                        sqlupd, istoread, idnote, iduserowner, sharinguser);

                    Int64 idnoteparent = 0;
                    string userparent = "";
                    // per ogni nota condivisa c'è un solo padre che è la root note 
                    // tutte le note figlie sono un solo livello più sotto
                    string sql = "select * from note where id=@n and iduser=@n ";
                    parentnote = conn.GetListData<DbNote>(sql, idnote, iduserowner);
                   
                    if (parentnote.Count > 0 && string.IsNullOrEmpty(parentnote.First().UserNameParent)==false)
                    {
                        Int64 iduserparent = DbDataAccess.GetIDUser(parentnote.First().UserNameParent);
                        idnoteparent = parentnote.First().IDNoteParent;
                        userparent = parentnote.First().UserNameParent;
                        res2 = conn.ExecNonQueryOpenConn(dbconn, transaction, 
                            sqlupd, istoread, idnoteparent, iduserparent, sharinguser);
                    }
                    
                    if (res1)
                        transaction.Commit();
                    else transaction.Rollback();
                }
                return res1;
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("Error UpdateSharedNoteRead: " + ex.Message, "mbb");
                return false;
            }
        }

        public static bool UpdateSeparatorTitleNote(string oldsep, string newsep, int iduser)
        {
            try
            {
                DbFactory conn = new DbFactory(connectionString, providerName);

                bool res1 = false;

                using (DbConnection dbconn = conn.GetConnection())
                {
                    dbconn.Open();
                    DbTransaction transaction = dbconn.BeginTransaction();

                    List<DbNote> lstnotes = DbDataAccess.GetNotes(iduser, DateTime.MinValue, DateTime.MinValue, 0, ">=", "", false, false);

                    string[] delimiterChars = { oldsep };
                //List<string> l = filter.Split(delimiterChars).ToList();
                    foreach (DbNote n in lstnotes)
                    {
                        string newtitle = string.Join(newsep, n.Title.Split(delimiterChars, StringSplitOptions.RemoveEmptyEntries).ToList());
                        //int
                        res1 = conn.ExecNonQueryOpenConn(dbconn, transaction,
                                    "update NOTE set    " +
                                    " TITLE=@s          " +
                                    " where id=@n and iduser=@n"
                                    , newtitle, n.ID, iduser);
                    }

                    

                    if (res1)
                        transaction.Commit();
                    else transaction.Rollback();
                }
                return res1;
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("Error UpdateSeparatorTitleNote: " + ex.Message, "mbb");
                return false;
            }
        }

        /// <summary>
        /// Inserisce il record nel db. E' importante conoscere anche l'internal dir remote dato che il documento associato alla nota
        /// potrebbe già essere stato usato per un'altra nota, in questo caso devo fare riferimento alla nota precedentemente inserita.
        /// Non verrà eseguito upload del file su cartella nota corrente.
        /// </summary>
        /// <param name="idnote"></param>
        /// <param name="docname"></param>
        /// <param name="version"></param>
        /// <param name="internaldirremote">IDnote corrente o della nota contenente il file più recente</param>
        /// <param name="filenameloc"></param>
        /// <param name="iduser"></param>
        /// <param name="istosync"></param>
        /// <returns></returns>
        public static bool InsertDoc(Int64 idnote, string docname, int version, DateTime dtMod, string internaldirremote, string filenameloc, int iduser, bool istosync)
        {
            try
            {
                DbFactory conn = new DbFactory(connectionString, providerName);

                bool res1 = false;
                bool res2 = false;

                if (dtMod <= DateTime.MinValue)
                    dtMod = DateTime.Now;

                
                //-- non mi interessa: può cambiare e comunque a me interessa il nome del file
                //-- posso così copiare la cartella remota in un altro posto senza compromettere il recupero dei dati
                //-- devo solo cambiare nelle opzioni la dir remota
                //string filenameremote = dirpathremote + "\\" + "ID" + Convert.ToString(idnote).PadLeft(7, '0')+"\\"+docname;

                string sqlsel = "SELECT * FROM NOTE_DOC WHERE IDNote=@n and IDUser=@n and docname=@s";
                DataTable dt = conn.ExecuteSql(sqlsel, idnote, iduser, docname);

                if (dt.Rows.Count == 0)
                {
                    using (DbConnection dbconn = conn.GetConnection())
                    {
                        dbconn.Open();
                        DbTransaction transaction = dbconn.BeginTransaction();

                        res1 = conn.ExecNonQueryOpenConn(dbconn, transaction,
                                        "insert into NOTE_DOC " +
                                        " (IDnote, docname,version,DATETIMELASTMOD,IDUSER,INTERNALDIRREMOTE,FILENAMELOCAL,TOSYNCRONIZE) " +
                                        " values (@n, @s, @n, @dt, @n, @s, @s, @b) "
                                        , idnote, docname, version, dtMod
                                        , iduser, internaldirremote, filenameloc, istosync);

                        res2 = true;

                        if (res1 && res2)
                            transaction.Commit();
                        else transaction.Rollback();
                    }
                }

                return res1 && res2;
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("Error InserNote: " + ex.Message, "mbb");
                return false;
            }
        }

        public static bool DeleteDocsNote(Int64 idnote, int iduser)
        {
            try
            {
                DbFactory conn = new DbFactory(connectionString, providerName);

                bool res1 = false;

                using (DbConnection dbconn = conn.GetConnection())
                {
                    dbconn.Open();
                    DbTransaction transaction = dbconn.BeginTransaction();
                    res1 = conn.ExecNonQueryOpenConn(dbconn, transaction,
                                    "delete from NOTE_DOC " +
                                    " where idnote=@n and iduser=@n"
                                    , idnote, iduser);

                    if (res1)
                        transaction.Commit();
                    else transaction.Rollback();
                }
                return res1;
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("Error DeleteDocsNote: " + ex.Message, "mbb");
                return false;
            }
        }

        public static bool DeleteDocNote(Int64 idnote, int iduser, string docname)
        {
            try
            {
                DbFactory conn = new DbFactory(connectionString, providerName);

                bool res1 = false;

                using (DbConnection dbconn = conn.GetConnection())
                {
                    dbconn.Open();
                    DbTransaction transaction = dbconn.BeginTransaction();
                    res1 = conn.ExecNonQueryOpenConn(dbconn, transaction,
                                    "delete from NOTE_DOC " +
                                    " where idnote=@n and iduser=@n and docname=@s"
                                    , idnote,iduser,docname);

                    if (res1)
                        transaction.Commit();
                    else transaction.Rollback();
                }
                return res1;
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("Error DeleteDocNote: " + ex.Message, "mbb");
                return false;
            }
        }

        public static bool DeleteNote(Int64 idnote, int iduser, bool delchildnotes = false)
        {
            try
            {
                DbFactory conn = new DbFactory(connectionString, providerName);

                bool res1 = false;
                bool res2 = false;
                bool res3 = false;
                bool res4 = false;
                bool res5 = false;
                using (DbConnection dbconn = conn.GetConnection())
                {
                    dbconn.Open();
                    DbTransaction transaction = dbconn.BeginTransaction();
                    res1 = conn.ExecNonQueryOpenConn(dbconn, transaction,
                                    "delete from NOTE_DOC " +
                                    " where idnote=@n and iduser=@n "
                                    , idnote, iduser);

                    res2 = conn.ExecNonQueryOpenConn(dbconn, transaction,
                                    "delete from NOTE_KEY " +
                                    " where idnote=@n and iduser=@n "
                                    , idnote, iduser);

                    res3 = conn.ExecNonQueryOpenConn(dbconn, transaction,
                                    "delete from NOTE_SHARING " +
                                    " where idnote=@n and iduser=@n "
                                    , idnote, iduser);

                    res4 = conn.ExecNonQueryOpenConn(dbconn, transaction,
                                    "delete from NOTE " +
                                    " where id=@n and iduser=@n "
                                    , idnote, iduser);

                    // cancello tutte le note figlie della nota che sto cancellando
                    if (delchildnotes)
                    {
                        res5 = conn.ExecNonQueryOpenConn(dbconn,transaction,
                                    "delete from NOTE " +
                                    " where idnoteparent=@n and iduser=@n and idnoteparent > 0"
                                    , idnote, iduser);
                    }

                    if (res1 && res2 && res3 && res4)
                        transaction.Commit();
                    else transaction.Rollback();
                }
                return res1 && res2 && res3 && res4;
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("Error DeleteNote: " + ex.Message, "mbb");
                return false;
            }
        }

        /// <summary>
        /// non usata
        /// </summary>
        /// <param name="idnote"></param>
        /// <param name="iduser"></param>
        /// <returns></returns>
        public static bool DeleteNoteSharing(int idnote, int iduser)
        {
            try
            {
                DbFactory conn = new DbFactory(connectionString, providerName);
                bool res = false;
                using (DbConnection dbconn = conn.GetConnection())
                {
                    dbconn.Open();
                    DbTransaction transaction = dbconn.BeginTransaction();

                    res = conn.ExecNonQueryOpenConn(dbconn, transaction, 
                                                    "delete from note_sharing where iduser=@n and idnote=@n"
                                                    ,iduser ,idnote);

                    if (res)
                        transaction.Commit();
                    else transaction.Rollback();
                }
                return res;
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("Error DeleteNoteSharing: " + ex.Message, "mbb");
                return false;
            }
        }

        public static bool UpdateDocInfo(Int64 idnote, string docname, int version, string pathfilenameloc, string newdocname, int iduser, DateTime dtLastMod, string destFoldername)
        {
            try
            {
                DbFactory conn = new DbFactory(connectionString, providerName);

                bool res1 = false;
                //DateTime dt = DateTime.Now;
                string s = "";
                if (string.IsNullOrEmpty(pathfilenameloc) == false){
                    s = ",FILENAMELOCAL='" + pathfilenameloc + "'";
                    //dt = System.IO.File.GetLastWriteTime(filenameloc);
                }

                if (string.IsNullOrEmpty(destFoldername) == false)
                    s = s + ",INTERNALDIRREMOTE='" + destFoldername + "'";

                if(!string.IsNullOrEmpty(newdocname))
                    s = s + ",docname='" + newdocname + "'";

                if(version > 0)
                    s = s + ",version=" + version.ToString();

                if (dtLastMod > DateTime.MinValue)
                    s = s + ",DATETIMELASTMOD='" + Convert.ToDateTime(dtLastMod).ToString(conn.GetFormatDate()) + "'";

                if (s.IndexOf(',') == 0)
                    s = s.Substring(1);

                using (DbConnection dbconn = conn.GetConnection())
                {
                    dbconn.Open();
                    DbTransaction transaction = dbconn.BeginTransaction();
                    res1 = conn.ExecNonQueryOpenConn(dbconn, transaction,
                                    "update NOTE_DOC set " +
                                    //" DATETIMELASTMOD=@dt "+
                                    s +
                                    " where idnote=@n and iduser=@n and docname=@s "
                                    , idnote, iduser, docname);

                    if (res1)
                        transaction.Commit();
                    else transaction.Rollback();
                }
                return res1;
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("Error UpdateDocInfo: " + ex.Message, "mbb");
                return false;
            }
        }

        public static bool InsertNewUser(string username, string pwd)
        {
            try
            {
                DbFactory conn = new DbFactory(connectionString, providerName);
                bool res = false;
                int iduser = DbDataAccess.GetIDUser(username);
                using (DbConnection dbconn = conn.GetConnection())
                {
                    dbconn.Open();
                    DbTransaction transaction = dbconn.BeginTransaction();
                    
                    if (iduser < 0)
                    {
                        int id = conn.GetMaxFieldVal("userapp", "id") + 1;
                        res = conn.ExecNonQueryOpenConn(dbconn, transaction, 
                                                    "insert into userapp (id,name,password) values(@n,@s,@s)"
                                                    , id
                                                    , username
                                                    , pwd);

                        if (res)
                            transaction.Commit();
                        else transaction.Rollback();
                    }
                    else res = false;
                }
                return res;
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("Error InserNote: " + ex.Message, "mbb");
                return false;
            }
        }

        public static bool InsertUserGroup(string username, string group)
        {
            try
            {
                DbFactory conn = new DbFactory(connectionString, providerName);
                bool res = false;

                using (DbConnection dbconn = conn.GetConnection())
                {
                    dbconn.Open();
                    DbTransaction transaction = dbconn.BeginTransaction();

                    int iduser = DbDataAccess.GetIDUser(username);
                    res = conn.ExecNonQueryOpenConn(dbconn, transaction, 
                        "insert into user_group (iduser,groupname,username) values(@n,@s,@s)"
                                                , iduser
                                                , group
                                                , username);

                    if (res)
                        transaction.Commit();
                    else transaction.Rollback();
                }
                return res;
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("Error InserUserGroup: " + ex.Message, "mbb");
                return false;
            }
        }

        public static bool DeleteUserGroup(string username, string group)
        {
            try
            {
                DbFactory conn = new DbFactory(connectionString, providerName);
                bool res = false;

                using (DbConnection dbconn = conn.GetConnection())
                {
                    dbconn.Open();
                    DbTransaction transaction = dbconn.BeginTransaction();
                    res = conn.ExecNonQueryOpenConn(dbconn, transaction, 
                                    "delete from user_group where username=@s and groupname=@s"
                                                , username, group);

                    if (res)
                        transaction.Commit();
                    else transaction.Rollback();
                }
                return res;
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("Error DeleteUserGroup: " + ex.Message, "mbb");
                return false;
            }
        }

        public static bool DeleteUsersGroup(string group)
        {
            try
            {
                DbFactory conn = new DbFactory(connectionString, providerName);
                bool res = false;

                using (DbConnection dbconn = conn.GetConnection())
                {
                    dbconn.Open();
                    DbTransaction transaction = dbconn.BeginTransaction();
                    res = conn.ExecNonQueryOpenConn(dbconn, transaction, 
                                                "delete from user_group where group=@s)"
                                                ,group);

                    if (res)
                        transaction.Commit();
                    else transaction.Rollback();
                }
                return res;
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("Error DeleteUsersGroup: " + ex.Message, "mbb");
                return false;
            }
        }

        /// <summary>
        /// Inserimento in tabella note_sharing
        /// </summary>
        /// <param name="users"></param>
        /// <param name="idnote"></param>
        /// <param name="iduserowner"></param>
        /// <returns></returns>
        public static bool InsertNoteSharing(List<string> users, Int64 idnote, int iduserowner)
        {
            try
            {
                DbFactory conn = new DbFactory(connectionString, providerName);
                bool res = false;

                using (DbConnection dbconn = conn.GetConnection())
                {
                    dbconn.Open();
                    DbTransaction transaction = dbconn.BeginTransaction();

                    res = conn.ExecNonQueryOpenConn(dbconn, transaction, 
                                                "delete from note_sharing where iduser=@n and idnote=@n"
                                                , iduserowner, idnote);

                    foreach(string u in users.Distinct()){
                        //int id = conn.GetMaxFieldVal("userapp", "id") + 1;
                        int idusersh = DbDataAccess.GetIDUser(u);
                        //if(iduser<=0)
                        //    res = DbDataUpdate.InsertNewUser(
                        res = conn.ExecNonQueryOpenConn(dbconn, transaction, 
                                                "insert into note_sharing (idnote,iduser,idusersharing,usernamesharing) values(@n,@n,@n,@s)"
                                                , idnote
                                                , iduserowner
                                                , idusersh
                                                , u);
                    }
                    if (res)
                        transaction.Commit();
                    else transaction.Rollback();
                }
                return res;
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("Error InserNote: " + ex.Message, "mbb");
                return false;
            }
        }

        /// <summary>
        /// Gestisce allineamento del db con info relative allo user.
        /// Associato alla sincronizzazione di db locali tramite file ini intermedio.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="pwd"></param>
        /// <param name="group"></param>
        /// <param name="dtIns"></param>
        /// <param name="dtLastMod"></param>
        /// <param name="operation">I: insert, U: update, D: delete</param>
        /// <returns></returns>
        public static bool InsUpdDelUser(int iduser, string username, string pwd, string lang, string group, DateTime dtIns, DateTime dtLastMod, string operation )
        {
            try
            {
                DbFactory conn = new DbFactory(connectionString, providerName);
                bool res = false;

                using (DbConnection dbconn = conn.GetConnection())
                {
                    dbconn.Open();
                    DbTransaction transaction = dbconn.BeginTransaction();
                    int id = iduser;
                    if (operation == "I")
                    {
                        if(iduser<0)
                            id = conn.GetMaxFieldVal("userapp", "id") + 1;
                        res = conn.ExecNonQueryOpenConn(dbconn, transaction, 
                                                "insert into userapp (id,name,password,language,dateregistration,auditdatetime) "+
                                                " values(@n,@s,@s,@s,@dt,@dt)"
                                                , id, username, pwd, lang, dtIns, dtLastMod);
                    }

                    if (operation == "U")
                    {
                        //-- aggiorno solo se la data di ultimo aggiornamento è inferiore  quella rilevata su file e passata come parametro
                        res = conn.ExecNonQueryOpenConn(dbconn, transaction,
                                                "update userapp set name=@s, password=@s, language=@s, workgroup=@s, auditdatetime=@dt " +
                                                " where id=@n and auditdatetime<@dt"
                                                , username, pwd, lang, group, dtLastMod, dtLastMod
                                                , id);
                    }

                    if (operation == "D")
                    {
                        res = conn.ExecNonQueryOpenConn(dbconn, transaction,
                                                "update userapp set status=@s, auditdatetime=@dt " +
                                                " where id=@n and auditdatetime<@dt"
                                                , "DELETED", dtLastMod, id, dtLastMod);
                    }
                    if (res)
                        transaction.Commit();
                    else transaction.Rollback();
                }
                return res;
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("Error InserNote: " + ex.Message, "mbb");
                return false;
            }
        }
    }
}
