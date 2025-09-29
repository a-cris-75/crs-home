using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRS.Library;
using System.IO;
using MyBlackBoxCore.DataEntities;
using System.Windows.Documents;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using System.Windows.Forms;
using CRS.DBLibrary;

namespace MyBlackBoxCore
{
    public static class MBBCommon
    {
        public static string SeparationCharTitle { set; get; }
        public static string[] GetSeparationCharTitleParam ()
        { 
            List<string> res = new List<string>();
            res.Add(SeparationCharTitle);
            return res.ToArray();
        }
        public static MBBSyncroParams syncroParams = new MBBSyncroParams();
        public static MBBEmailParams emailParams = new MBBEmailParams();

        /// <summary>
        ///  Si occupa di copiare i file del DB, il file ini e il file contenente le stringhe sql di upgrade del db,
        ///  su una cartella locale in modo che a successivi aggiornamenti / installazioni il db e i file originali 
        ///  non siano sovrascritti.
        ///  In partcolare per il file alterDB.txt verifica le differenze fra il file della versione scaricata e quella attualmente installata
        ///  sul sistema. Tiene buone le righe non commentate del file originale e aggiiunge quelle non presetni sl file originali dal file che sto installando.
        ///  In questo modo verranno eseguite le nuove stringhe sql, dato che le vecchie sono commentate.
        /// </summary>
        /// <param name="assemblyName"></param>
        public static void InitApplicationInstall(string assemblyName)
        {
            CRSIniFile.InitApplicationIni(assemblyName);
            DbFactory.InitDbFile(MBBGlobalParams.DbFileNameSqlServerCE_MBB, assemblyName);
            
            string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string userFilePath = Path.Combine( Path.Combine(localAppData, assemblyName), "DB");

            if (!Directory.Exists(userFilePath))
                Directory.CreateDirectory(userFilePath);
            string destFilePath = Path.Combine(userFilePath, "alterDB.txt");

            string sourceFilePath = Path.Combine(System.Windows.Forms.Application.StartupPath, "DB");
            sourceFilePath = Path.Combine(sourceFilePath, "alterDB.txt");
            if (File.Exists(destFilePath))
            {
                // se il file esiste significa che precedentemetne il db ha già subito un upgrade (con alter di qualcosa)
                // devo confrontare il contenuto del file presente col nuovo e commentare le parti identiche (se non sono già commentate, dovrebbero esserlo),
                // dato che presumibilemte sono già state applicate
                CRSFileUtil.BackupFile(userFilePath, "alterDB.txt", false);
                CRSFileUtil.CompareFilesMakeDiff(sourceFilePath, destFilePath, ';', "GO") ;
            }
            else
            {
                File.Copy(sourceFilePath, destFilePath);
            }
        }

        public static MBBSyncroParams GetSyncroParamsFromIni()
        {
            MBBSyncroParams syncParams = new MBBSyncroParams();
            try
            {
                //-- init info               
                syncParams.SharingDir = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_SHARINGDIR);
                syncParams.LocalTempDir = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_LOCALTMPDIR);
                syncParams.DropBoxRootDir = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_DROPBOXDIR);

                syncParams.AutoSync = false;
                string s = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_AUTOSYNC);
                if (string.IsNullOrEmpty(s) == false && (s == "1" || s.ToLower() == "true"))
                    syncParams.AutoSync = true;

                syncParams.AutoSave = true;
                s = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_AUTOSAVE);
                if (string.IsNullOrEmpty(s) == false && (s == "1" || s.ToLower() == "true"))
                    syncParams.AutoSave = true;

                syncParams.SyncFiles = false;
                string ss = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_SYNCROFILES);
                if (string.IsNullOrEmpty(ss) == false && (ss == "1" || ss.ToLower() == "true"))
                    syncParams.SyncFiles = true;

                s = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_ISUNIQUEFILENAME, 1);
                if (string.IsNullOrEmpty(s) == false && (s == "1" || s.ToLower() == "true"))
                    syncParams.IsUniqueFileName = true;

                s = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_OPNOOVERWRITEOWNER, 1);
                if (string.IsNullOrEmpty(s) == false && (s == "1" || s.ToLower() == "true"))
                    syncParams.SyncroOnlyOwner = true;

                s = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_FILE_DIM_DAYS, 180);
                if (string.IsNullOrEmpty(s) == false)
                    syncParams.SyncFileDimDays = Convert.ToInt32(s);

                syncParams.NewFileExtension = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_NEWFILEEXTENSION);

                syncParams.SyncWithEmail = false;
                s = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_SYNCRO_WITH_EMAIL, 1);
                if (string.IsNullOrEmpty(s) == false && (s == "1" || s.ToLower() == "true"))
                    syncParams.SyncWithEmail = true;
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("Error GetSyncroParamsFromIni: " + ex.Message, "");
            }

            return syncParams;
        }

        public static MBBEmailParams GetEmailParamsFromIni()
        {
            MBBEmailParams emailParams = new MBBEmailParams();
            try
            {
                //-- init info               
                emailParams.EmailAddress = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_USER);
                emailParams.EmailPwd = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_USERPWD);
                emailParams.ImapServer = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_EMAIL_SERVER_IMAP, CRSMailRepository.DEFAULT_IMAP_SERVER );
                emailParams.ImapPort = Convert.ToInt32( CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_EMAIL_SERVER_IMAP_PORT, CRSMailRepository.DEFAULT_IMAP_PORT_993));
                emailParams.SmtpServer = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_EMAIL_SERVER_SMTP, CRSMailRepository.DEFAULT_SMTP_SERVER);
                emailParams.SmtpPort = Convert.ToInt32(CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_EMAIL_SERVER_SMTP_PORT, CRSMailRepository.DEFAULT_SMTP_PORT_587));
                emailParams.Pop3Server = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_EMAIL_SERVER_POP3);
                emailParams.Pop3Port = Convert.ToInt32(CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_EMAIL_SERVER_POP3_PORT, 0));

            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("Error GetEmailParamsFromIni: " + ex.Message, "");
            }

            return emailParams;
        }

        public static MBBUserInfo GetUserInfoFromIni()
        {
            MBBUserInfo user = new MBBUserInfo();
            try
            {
                //-- init info: superfluo               
                /*user.Name = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_USER);
                user.Password = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_USERPWD);
                user.Language = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_LANGUAGE);*/
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("Error GetUserInfoFromIni: " + ex.Message, "");
            }

            return user;
        }

        public static List<DbNote> MergeNotesParent(List<DbNote> notes)
        {

            List<DbNote> res = new List<DbNote>();

            try
            {
                // 1) aggiungo le note per cui non esiste un padre
                res.AddRange(notes.Where(X => X.IDNoteParent == 0).ToList());

                var grp = notes.Where(X => X.IDNoteParent > 0).GroupBy(X => X.IDNoteParent);
                List<Int64> keys = grp.Select(X => X.Key).Distinct().ToList();
                foreach (Int64 k in keys)
                {
                    // la clausola X.ID = k è utile nel caso che per la nota radice l'IDNoteParent sia 0
                    List<DbNote> notesgrp = notes.Where(X => X.IDNoteParent == k || X.ID == k).ToList();
                    if (notesgrp.Count > 0)
                    {
                        DbNote newnote = new DbNote();
                        string txt = string.Join(" ", notesgrp.Select(Y => Y.Text).ToList());

                        newnote = notesgrp.First();
                        newnote.Text = txt;

                        res.Add(newnote);
                    }
                }
                res = res.OrderBy(X => X.DateTimeInserted).ToList();
            }
            catch { }

            return res;
        }

        public static bool OpenDoc(DbNoteDoc doc, int openLocal0Remote1, string lang)
        {
            bool res = true;
            string intDirRemote = "";

            try
            {
                //intDirRemote = System.IO.Path.Combine(MBBCommon.syncroParams.DropBoxRootDir, MBBSyncroUtility.MakeFolderNameUser(doc.IDNote, doc.UserNoteOwner));
                intDirRemote = System.IO.Path.Combine(MBBCommon.syncroParams.DropBoxRootDir, doc.InternalDirRemote);
                string filePath = System.IO.Path.Combine(intDirRemote, doc.DocName);
                //-- ora gestisco più docs per nota
                if (string.IsNullOrEmpty(doc.DocName) == false)
                {
                    if (openLocal0Remote1 == 0)
                    {
                        if (File.Exists(doc.FileNameLocal)) Process.Start(doc.FileNameLocal);
                        else
                        {
                            System.Windows. MessageBox.Show(CRSTranslation.TranslateDialog(CRSTranslation.MSG_FILE_NOT_FOUND, lang),
                                            CRSTranslation.TranslateDialog(CRSTranslation.MSG_WARNING, lang));
                            res = false;
                        }
                    }
                    if (openLocal0Remote1 == 1)
                    {
                        if (File.Exists(filePath)) Process.Start(filePath);
                        else
                        {
                            System.Windows.MessageBox.Show(CRSTranslation.TranslateDialog(CRSTranslation.MSG_FILE_NOT_FOUND, lang),
                                            CRSTranslation.TranslateDialog(CRSTranslation.MSG_WARNING, lang));
                            res = false;
                        }
                    }
                }
                else
                {
                    System.Windows.MessageBox.Show(CRSTranslation.TranslateDialog(CRSTranslation.MSG_FILE_NOT_FOUND, lang),
                                CRSTranslation.TranslateDialog(CRSTranslation.MSG_WARNING, lang ));
                    res = false;
                }
                if (!res)
                {
                    OpenFileDialog fd = new OpenFileDialog();
                    fd.InitialDirectory = System.IO.Path.GetDirectoryName(intDirRemote);
                }
                return res;
            }
            catch
            {
                return false;
            }
        }

        #region ORDINA NOTE IN CARTELLE secondo 3 algoritmi
        /// <summary>
        /// USATA => FUNZIONAMENTO: questo metodo serve per ottenere un albero di directory delle note coerente.
        /// Viene riordinato il Title di ogni nota in base al criterio di occorrenze con cui ogni parola del title compare nell'insieme di note analizzato.
        /// </summary>

        public static List<string> GetListDirTitlesOrderByOccurrencyKey(List<string> lsttitles)
        {
            //tvNotes.Items.Clear();

            // FUNZIONAMENTO: questo metodo serve per ottenere un albero di directory delle note coerente
            // L'header di ogni nota rappresenta l'insieme di parole chiave che può essere inteso come un insieme di di directory annidate
            // ES di titoli di note => 
            //      .titolo 1: fisica meccanica formule
            //      .titolo 2: fisica fluidodinamica teoria 
            //      .titolo 3: fisica fluidodinamica formule
            //      .titolo 4: fisica teoria fluidodinamica TODESCHINI
            //      .titolo 5: fisica teoria fluidodinamica einstein
            // DOVREI OTTENERE un albero di titoli directory in cui a sx ho la parola chiave  che compare più frequentemente
            // In questo caso l'orine è:  fisica(5), fluidodinamica (4), teoria (3), formule (2), meccanica (1), einstein (1), todeschini (1)
            //      .titolo 1: fisica formule meccanica
            //      .titolo 2: fisica fluidodinamica teoria 
            //      .titolo 3: fisica fluidodinamica formule
            //      .titolo 4: fisica fluidodinamica teoria TODESCHINI
            //      .titolo 5: fisica fluidodinamica teoria einstein
            // COME SI VEDE, nel caso di titolo 1 il risutato non è soddisfacente dato che può essere che molte note contengano una parola chiave (formule)
            // che compare più volte rispetto ad un argomento più generico
            // MA NON HO ALTRI MODI PER OTTENERE IL RISULTATO VOLUTO: dovrei avere una tabella con priorità
            // OPPURE: 

            // lista di tutte le parole chiave: un record una parola
            List<string> lstkeys = new List<string>();
            // lista di tutti i titoli di note disponibili
            //List<string> lsttitles = new List<string>();
            // lista dei titoli con i soli titoli che non si ripetono
            List<string> lsttitlesnew = new List<string>();

            //lsttitles = lst.Select(X => X.Title).Distinct().ToList();
            if (lsttitles != null && lsttitles.Count > 0)
            {

                // 1) determina tutte le liste di note raggruppate per ogni parola chiave (con associato numero di occorrenze)
                // 2) ordina la lista per quantità (occorrenze) di note
                // 3) genera l'albero creando per ogni voce della lista una radice

                foreach (string d in lsttitles)
                {
                    List<string> keys = d.Split(MBBCommon.GetSeparationCharTitleParam(), StringSplitOptions.RemoveEmptyEntries).ToList();
                    lstkeys.AddRange(keys);
                }
                // lista in cui compare il valore chiave e il numero di occorrenze della parola chiave nel titolo delle note
                Dictionary<string, int> dd = lstkeys.GroupBy(X => X).ToDictionary(g => g.Key, g => g.ToList().Count);//new Dictionary<string, int>();
                List<KeyValuePair<string, int>> lstkeysnum = dd.ToList();
                // ordino per numero di occorrenze
                lstkeysnum = lstkeysnum.OrderByDescending(X => X.Value).ToList();
                List<string> titles = lsttitles.ToList();
                // devo sistemare la lista dei titoli che contengono la parola chiave analizzata (corrente)
                // per ogni parola chiave controlla la posizione rispetto alla lista ordinata
                foreach (string t in titles)
                {
                    List<string> keystitle = t.Split(MBBCommon.GetSeparationCharTitleParam(), StringSplitOptions.RemoveEmptyEntries).ToList();

                    keystitle = keystitle.OrderBy(d => lstkeysnum.Select(X => X.Key).ToList().IndexOf(d)).ToList();
                    // nuovo titolo riordinato
                    var orderedByIDList = from i in keystitle
                                          join o in lstkeysnum.Select(X => X.Key)
                                          on i equals o
                                          select o;

                    // string newt = string.Join(" ", keystitle);
                    string newt = string.Join(MBBCommon.SeparationCharTitle, orderedByIDList);
                    lsttitlesnew.Add(newt);
                }

                lsttitlesnew = lsttitlesnew.Distinct().ToList();

            }

            return lsttitlesnew;
        }

        // NON USATA MA UTILE
        private static List<string> GetListDirTitlesOrderByMatchSmaller(List<string> lsttitles)
        {
            //tvNotes.Items.Clear();

            // FUNZIONAMENTO: partendo dalla lista determinata in GetListDirTitles, in base alla priorità stabilita dall'occorrenza delle parole chiave, 
            // determina una nuova priorità in base alla presenza del path più corto contenente tutte le parole chiave di un path più lungo
            // ES: fra parentesi le occorrenze e il path determinato da GetListDirTitles
            //      1) fisica(5) meccanica(3) quantistica(2)
            //      2) fisica(5) formule(4) meccanica(3) quantistica(2)
            // =>   2) fisica(5) meccanica(3) quantistica(2) formule(4): dato che la presenza di un path più corto determina il path più lungo
            // ATTENZIONE:  se ci sono due titles su cui scegliere scelgo quello per numero di occorrenze
            // ES:
            //      1) fisica(5) meccanica(3) quantistica(2)
            //      2) fisica(5) meccanica(3) formule(4)
            //      3) fisica(5) formule(4) meccanica(3) quantistica(2)
            // =>   3) fisica(5) meccanica(3) formule(4) quantistica(2) 
            // LIMITI: dall'ordinamento non vengono inclusi i casi in cui solo una parte del path si ripete in un altro path
            // anche in questo caso dovrei prendere la parte in comune e ordinare in base all'ordine più usato dell parte comune
            // - dal punto di vista computazionale diventa oneroso dato che per ogni path devo estrapolare le parti e confrontarle trovando gli altri 
            //   titoli "compatibili"

            // ALGORITMO: per ogni title trova la lista di title più lunghi che contengono il path analizzato
            // se esistono ordina la lista per numero di items
            // ordina gli items in base all'ordine degli items del path più corto


            // lista dei titoli con i soli titoli che non si ripetono
            List<string> lsttitlesnew = new List<string>();
            // lista dei titoli analoghi ma più lunghi
            List<string> lsttitlechilds = new List<string>();

            // per ottimizzare devo prima ordinare per numero di chiavi
            lsttitles = lsttitles.OrderBy(k => k.Split(MBBCommon.GetSeparationCharTitleParam(), StringSplitOptions.RemoveEmptyEntries).Count()).ToList();
            lsttitles.ForEach(X => X.TrimStart().TrimEnd());
            foreach (string d in lsttitles)
            {
                List<string> lstkeys = d.Split(MBBCommon.GetSeparationCharTitleParam(), StringSplitOptions.RemoveEmptyEntries).ToList();
                lstkeys.ForEach(X => X.TrimStart().TrimEnd());

                List<string> lstkeyspart = new List<string>();
                int numkeys = numkeys = lstkeys.Count();
                // lista dei titoli più corti contenuti nell'attuale
                /*lsttitleparents = lsttitles.Where(X =>
                    X.Split(MBBCommon.GetSeparationCharTitleParam(), StringSplitOptions.RemoveEmptyEntries).ToList().Count < numkeys &&
                    X.Split(MBBCommon.GetSeparationCharTitleParam(), StringSplitOptions.RemoveEmptyEntries).Except(lstkeys).Count() == 0).ToList();
                */
                // lista dei titoli più lunghi che contengono l'attuale (anche una parte)
                lsttitlechilds = lsttitles.Where(X =>
                    X.Split(MBBCommon.GetSeparationCharTitleParam(), StringSplitOptions.RemoveEmptyEntries).ToList().Count > numkeys &&
                    lstkeys.Except(X.Split(MBBCommon.GetSeparationCharTitleParam(), StringSplitOptions.RemoveEmptyEntries)).Count() == 0
                    //lstkeys.Where(Y=>Y.Split(MBBCommon.GetSeparationCharTitleParam(), StringSplitOptions.RemoveEmptyEntries).Count() >= 0 
                    ).ToList();

                // se non ce ne sono mantengo l'attuale se non esiste già
                if (lsttitlechilds.Count == 0 && lsttitlesnew.Where(X => X == d).Count() == 0)
                    lsttitlesnew.Add(d);

                foreach (string c in lsttitlechilds)
                {
                    List<string> keystitle = c.Split(MBBCommon.GetSeparationCharTitleParam(), StringSplitOptions.RemoveEmptyEntries).ToList();
                    keystitle.ForEach(X => X.TrimStart().TrimEnd());
                    // ordino la lista in base all'ordine delle chiavi del titolo corrente (che è quello più corto di partenza)
                    //keystitle = keystitle.OrderBy(k => lstkeys.Select(X => X).ToList().IndexOf(k)).ToList();
                    /*var orderedByIDList = from i in lstkeys
                                          join o in lstkeysnum.Select(X => X.Key)
                                          on i equals o
                                          select o;*/
                    List<string> newlst = new List<string>();
                    foreach (string s in lstkeys)
                    {
                        // i primi sono quelli del padre
                        newlst.Add(s);
                        keystitle.Remove(s);
                    }
                    if (keystitle.Count > 0) newlst.AddRange(keystitle);


                    string newtitle = string.Join(MBBCommon.SeparationCharTitle, newlst/*keystitle*/);

                    // ora devo sostituire il nuovo titolo se non esiste già
                    if (lsttitlesnew.Where(X => X == newtitle).Count() == 0)
                        lsttitlesnew.Add(newtitle);
                }
            }

            return lsttitlesnew;
        }

        /// <summary>
        /// USATA => 
        /// FUNZIONAMENTO: partendo dalla lista lsttitles DETERMINA UNA LISTA ORDINATA DI PAROLE CHIAVE. 
        /// L'ordine della lista determina l'ordine delle parole chiave nel titolo. 
        /// L'ordine è calcolato in base al numero di volte che la parola compare nelle prime posizioni 
        /// (come nel medagliere: un oro ha priorità su 2 o più argenti anche se la nazione classificata
        ///  dopo ha più medaglie in totale => 
        ///  cioè il totale delle medaglie "inferiori" è maggiore al totale delle medaglie "superiori")
        /// </summary>
        /// <param name="lsttitles"></param>
        /// <returns></returns>
        public static List<string> GetListDirTitlesOrderByOccurrencyPositionKey(List<string> lsttitles)
        {
            // lista dei titoli con i soli titoli che non si ripetono
            List<string> lsttitlesnew = new List<string>();
            // lista dei titoli analoghi ma più lunghi
            List<string> lsttitlechilds = new List<string>();

            // per ottimizzare devo prima ordinare per numero di chiavi
            lsttitles = lsttitles.OrderBy(k => k.Split(MBBCommon.GetSeparationCharTitleParam(), StringSplitOptions.RemoveEmptyEntries).Count()).ToList();
            lsttitles.ForEach(X => X.TrimStart().TrimEnd());
            // lista per chiave ordinata a medagliere
            List<KeyValuePair<string, int>> lstkeysnum = GetListDirKeysPosition(lsttitles).ToList();
            foreach (string t in lsttitles)
            {
                List<string> keystitle = t.Split(MBBCommon.GetSeparationCharTitleParam(), StringSplitOptions.RemoveEmptyEntries).ToList();
                // ordino le chiavi del titolo in base all'ordine determinato prima in lstkeysnum
                keystitle = keystitle.OrderBy(d => lstkeysnum.Select(X => X.Key).ToList().IndexOf(d)).ToList();
                // nuovo titolo riordinato
                var orderedByIDList = from i in keystitle
                                      join o in lstkeysnum.Select(X => X.Key)
                                      on i equals o
                                      select o;
                string newt = string.Join(MBBCommon.SeparationCharTitle, orderedByIDList);
                lsttitlesnew.Add(newt);
            }
            lsttitlesnew = lsttitlesnew.Distinct().OrderBy(X => X).ToList();

            return lsttitlesnew;
        }
        // 
        /// <summary>
        /// NON USATA MA UTILE: 
        /// Partendo dalla lista dei titoli (dir) precedentemente determinata con  GetListDirTitlesOrderByOccurrency o GetListDirTitlesOrderByOccurrency
        /// restitruisce la stessa lista con al posto delle parole chiave, le stesse parole chiave e il numero di occorrenze
        ///      key1 key2 key3 => ke1(numkey1) key2(numkey1) key3(numkey3)
        /// </summary>
        /// <param name="lsttitles"></param>
        /// <returns></returns>
        private static List<string> GetListNewDirTitlesOccurrencesKey(List<string> lsttitles)
        {
            // lista dei titoli con i soli titoli che non si ripetono
            List<string> lsttitlesnew = new List<string>();
            List<KeyValuePair<string, int>> lstkeysnum = GetListDirKeysOccurrences(lsttitles);
            // ordino per numero di occorrenze
            lstkeysnum = lstkeysnum.OrderByDescending(X => X.Value).ToList();

            // devo sistemare la lista dei titoli sostituendo la chiave con chiave(occorrenze)
            // per ogni parola chiave controlla la posizione rispetto alla lista ordinata
            foreach (string t in lsttitles)
            {
                foreach (string k in t.Split(MBBCommon.GetSeparationCharTitleParam(), StringSplitOptions.RemoveEmptyEntries).ToList())
                {
                    string newt = lstkeysnum.Where(X => X.Key == k).First().Key + "(" + lstkeysnum.Where(X => X.Key == k).First().Value + ")";
                    lsttitlesnew.Add(newt);
                }
            }

            lsttitlesnew = lsttitlesnew.Distinct().ToList();

            return lsttitlesnew;
        }
        /// <summary>
        /// Data la lista dei titoli delle note restituisce una lista di parole chiave con associate le occorrenze delle stesse nella lista
        /// </summary>
        /// <param name="lsttitles"></param>
        /// <returns></returns>
        public static List<KeyValuePair<string, int>> GetListDirKeysOccurrences(List<string> lsttitles)
        {
            // lista di tutte le parole chiave: un record una parola
            List<string> lstkeys = new List<string>();

            foreach (string d in lsttitles)
            {
                List<string> keys = d.Split(MBBCommon.GetSeparationCharTitleParam(), StringSplitOptions.RemoveEmptyEntries).ToList();
                lstkeys.AddRange(keys);
            }
            // lista in cui compare il valore chiave e il numero di occorrenze della parola chiave nel titolo delle note
            Dictionary<string, int> dd = lstkeys.GroupBy(X => X).ToDictionary(g => g.Key, g => g.ToList().Count);
            List<KeyValuePair<string, int>> lstkeysnum = dd.ToList();

            return lstkeysnum;
        }

        private static List<KeyValuePair<string, int>> GetListDirKeysPosition(List<string> lsttitles)
        {
            // lista di tutte le parole chiave: un record una parola
            List<string> lstkeys = new List<string>();

            foreach (string d in lsttitles)
            {
                List<string> keys = d.Split(MBBCommon.GetSeparationCharTitleParam(), StringSplitOptions.RemoveEmptyEntries).ToList();
                lstkeys.AddRange(keys);

            }
            // lista in cui compare il valore chiave e il numero di occorrenze della parola chiave nel titolo delle note
            Dictionary<string, int> dd = lstkeys.GroupBy(X => X).ToDictionary(g => g.Key, g => g.ToList().Count);
            // lista con chiave, occorrenze
            List<KeyValuePair<string, int>> lstkeysnum = dd.ToList();
            // lista con chiave, posizione, num occorrenze nella poszione, occorrenze totali 
            List<Tuple<string, int, int, int>> lstkeynumpos = new List<Tuple<string, int, int, int>>();

            foreach (KeyValuePair<string, int> k in lstkeysnum)
            {
                // considero solo i titoli contenenti la chiave
                List<string> lsttitlestmp = lsttitles.Where(X =>
                    X.Split(MBBCommon.GetSeparationCharTitleParam(), StringSplitOptions.RemoveEmptyEntries).ToList().Where(Y => Y == k.Key).Count() > 0).ToList();

                /*int maxnumkeys = lsttitlestmp.Max(X => X.Split(MBBCommon.GetSeparationCharTitleParam(), StringSplitOptions.RemoveEmptyEntries).ToList().Count);

                for(int pos=0; pos < maxnumkeys; pos++)
                {
                    // recupero numero di volte in cui la chiave si trova in posizione pos
                    int numpos = lsttitlestmp.Where(X => 
                        // considera i titoli che contengono la parola chiave, quelli con un num di parole chiave adeguato e 
                        X.Split(MBBCommon.GetSeparationCharTitleParam(), StringSplitOptions.RemoveEmptyEntries).ToList().Where(Y=>Y == k.Key).Count()>0 &&
                        X.Split(MBBCommon.GetSeparationCharTitleParam(), StringSplitOptions.RemoveEmptyEntries).ToList().Count() <= pos && 
                        X.Split(MBBCommon.GetSeparationCharTitleParam(), StringSplitOptions.RemoveEmptyEntries)[pos] == k.Key).Count();

                    //List<Tuple<int, int>> lstposnumpos = lsttitlestmp.GroupBy(X =>
                    //    X.Split(MBBCommon.GetSeparationCharTitleParam(), StringSplitOptions.RemoveEmptyEntries).ToList().IndexOf(k.Key)).ToList();

                    // contiene per ogni chiave, la posizione occupata almeno una volta, il numero di volte in cui la pos è occupata e le occorrenze totali
                    if(numpos>0)
                        lstkeynumpos.Add(new Tuple<string,int,int,int>(k.Key,pos,numpos, k.Value));
                }*/

                // raggruppo per pos e num di presenze della chiave nella posizione
                Dictionary<int, int> lstposnumpos = lsttitlestmp.GroupBy(X =>
                        X.Split(MBBCommon.GetSeparationCharTitleParam(), StringSplitOptions.RemoveEmptyEntries).ToList().IndexOf(k.Key)
                        ).ToDictionary(g => g.Key, g => g.ToList().Count);

                foreach (KeyValuePair<int, int> kp in lstposnumpos)
                {
                    lstkeynumpos.Add(new Tuple<string, int, int, int>(k.Key, kp.Key, kp.Value, k.Value));
                }
            }
            // ordino per pos, poi per numpos, e alla fine per occorrenze: ottengo una lista ordinata per posizione e numero di occorrenze nella posizione 
            // (immagina il medagliere: l'oro ha sempre la priorità sulle altre medaglie)
            // NB: in linq si ragiona al contrario: l'order by più esterno domina sugli altri
            lstkeynumpos = lstkeynumpos.OrderByDescending(Y => Y.Item4).OrderByDescending(Y => Y.Item3).OrderBy(X => X.Item2).ToList();

            List<KeyValuePair<string, int>> lstkeysnumres = new List<KeyValuePair<string, int>>();
            // devo compilare una lista con una sola chiave per riga e la posizione
            foreach (Tuple<string, int, int, int> t in lstkeynumpos)
            {
                if (lstkeysnumres.Where(X => X.Key == t.Item1).Count() == 0)
                    lstkeysnumres.Add(new KeyValuePair<string, int>(t.Item1, t.Item4));
            }
            // il risultato è una lista di singole chiavi (con associato il numero di occorrenze che serve per stabilire un'ordine per tutti i titoli
            return lstkeysnumres;
        }

        /// <summary>
        /// Data una lista di titles con i tags, parole chiave, restituisce una lista di titoli non duplicati con le parole ordinate per ordine alfabetico:
        /// - es: fisica todeschini einstein, fisica einstein todeschini = einstein fisica todeschini 
        /// </summary>
        /// <returns></returns>
        public static List<string> NormalizeListTitles(List<string> lsttitles)
        {
            List<string> res = new List<string>();
            foreach (string t in lsttitles)
            {
                List<string> lstkeys = t.Split(MBBCommon.GetSeparationCharTitleParam(), StringSplitOptions.RemoveEmptyEntries).ToList();
                string newt = string.Join(MBBCommon.SeparationCharTitle, lstkeys.OrderBy(X => X).ToArray());
                if (res.Where(X => X == newt).Count() == 0) res.Add(newt);
            }
            return res;

        }
        /// <summary>
        /// Semplifica l'albero compattando i nodi che non hanno foglie e i rami singoli
        /// </summary>
        /// <param name="lst"></param>
        /// <param name="typecalcdir"></param>
        public static TreeViewItem NormalizeTreeView(ItemCollection nodes)
        {
            TreeViewItem itemRes = null;
            try
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    if (nodes[i] is TreeViewItem)
                    {
                        TreeViewItem item = (TreeViewItem)nodes[i];
                        // se sul nodo ho un solo figlio attacco il figlio al padre ed elimino il nodo
                        if (item.Items.Count <= 1)
                        {
                            //item.Header = " to remove: "+ item.Tag.ToString();
                            //item.Tag = "toremove";
                            if (item.Items[0] is TreeViewItem && item.Parent is TreeViewItem)
                            {
                                TreeViewItem newchild = (TreeViewItem)item.Items[0];
                                TreeViewItem parent = (TreeViewItem)item.Parent;
                                parent.Header = parent.Header + " " + item.Header;
                                item.Items.Remove(newchild);

                                parent.Items.Add(newchild);
                                parent.Items.Remove(item);
                            }
                            else
                            {
                                if (item.Items[0] is CRSListViewItemNote && item.Parent is TreeViewItem)
                                {
                                    CRSListViewItemNote newchild = (CRSListViewItemNote)item.Items[0];
                                    TreeViewItem parent = (TreeViewItem)item.Parent;
                                    parent.Header = parent.Header + " " + item.Header;
                                    item.Items.Remove(newchild);

                                    parent.Items.Add(newchild);
                                    parent.Items.Remove(item);
                                }
                            }
                            /*
                            if (item.Parent != null  && item.Parent is TreeViewItem)
                            {
                                TreeViewItem parent = (TreeViewItem)item.Parent;
                                parent.Header = parent.Header + " " + item.Header; 
                                TreeViewItem parentnewchild = new TreeViewItem();
                                if (item.Items[0] is TreeViewItem)
                                {
                                    TreeViewItem newchild = (TreeViewItem)item.Items[0];
                                    parentnewchild = (TreeViewItem)newchild.Parent;
                                    parentnewchild.Items.Remove(newchild);
                                    parent.Items.Add(newchild);
                                }
                                else if (item.Items[0] is CRSListViewItemNote)
                                {
                                    CRSListViewItemNote newchild = (CRSListViewItemNote)item.Items[0];
                                    parentnewchild = (TreeViewItem)newchild.Parent;
                                    parentnewchild.Items.Remove(newchild);
                                    parent.Items.Add(newchild);
                                }
                                
                                parent.Items.Remove(item);
                            }
                            else
                            {
                                itemRes = NormalizeTreeView(item.Items);
                            }*/
                        }
                        else
                        {
                            itemRes = NormalizeTreeView(item.Items);
                        }
                        itemRes = null;
                    }
                    else return null;
                }

                return itemRes;

            }
            catch (Exception ex)
            {
                string e = ex.ToString();
                return null;
            }
        }

        public static TreeViewItem GetItemFirstLevelTree(string itemtag, ItemCollection nodes)
        {
            TreeViewItem res = null;
            foreach (TreeViewItem itm in nodes)
            {
                if (itm.Tag.ToString() == itemtag)
                {
                    res = itm;
                    break;
                }
            }
            return res;
        }

        public static TreeViewItem GetExistingItem(string itemtag, ItemCollection nodes)
        {
            TreeViewItem itemRes = null;
            try
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    if (nodes[i] is TreeViewItem)
                    {
                        TreeViewItem item = (TreeViewItem)nodes[i];
                        //if (item.Tag.Equals(itemtag))
                        if (item.Tag.Equals(itemtag))
                        {
                            itemRes = item;
                            break;
                        }
                        else
                        {
                            // continua solo se la prima parte batte (itemtag.contains(item.tag))
                            string itmtg = item.Tag.ToString();
                            if (itmtg.Length <= itemtag.Length && itmtg.Equals(itemtag.Substring(0, itmtg.Length)))
                            {
                                itemRes = GetExistingItem(itemtag, item.Items);
                                if (itemRes != null) break;
                            }
                            //else break;
                        }
                    }
                }
                return itemRes;

            }
            catch (Exception ex)
            {
                string e = ex.ToString();
                return null;
            }
        }
        #endregion   
    }
}
