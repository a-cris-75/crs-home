using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CRS.Library;
using MyBlackBoxCore.DataEntities;

namespace MyBlackBoxCore
{
    /// <summary>
    /// Interaction logic for MBBSharingNotes.xaml
    /// </summary>
    public partial class MBBSharingNotes : UserControl
    {
        private int idUser;
        private string user;
        private string lang = "";
        private int refreshTimer = 0;
        private List<DbNote> currentDayNotes = new List<DbNote>();
        //-- currentRootNote ( o NoteCurrent) è la nota attuale radice, da cui dipendono le note figlie che 
        //-- rappresentano tutte le risposte
        //private DbNoteSharing currentRootNote;
        private DbNote currentRootNote;
        private DbNote currentNoteToSave;

        private MBBFindParams findParams;


        public MBBSharingNotes()
        {
            InitializeComponent();
        }

        public void Init(DbNote currentRootNote)
        {
            InitFromIniOptions();
            this.currentRootNote = currentRootNote;
        }

        private void InitFromIniOptions()
        {
            //-- init style
            try
            {
                DbDataAccess.Init(CRSIniFile.GetConnectionStringFromIni(), CRSIniFile.GetProviderFromIni());
                DbDataUpdate.Init(CRSIniFile.GetConnectionStringFromIni(), CRSIniFile.GetProviderFromIni());
                //-- init info               
                //-- l'utente che condivide (user) è l'utente stesso
                this.user = CRSIniFile.GetUserFromIni();
                this.lang = CRSIniFile.GetLanguageFromIni();
                this.idUser = DbDataAccess.GetIDUser(this.user);
                
                string s = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_REFRESH_TIMER_SHARING, 15);
                if (string.IsNullOrEmpty(s) == false)
                    this.refreshTimer = Convert.ToInt32(s);

                if (this.idUser < 0)
                {
                    //-- apri form di inserimento nuovo utente
                }
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("Error Init control MBBSharingNotes: " + ex.Message, "");
            }
        }

        public void SetFindParams(MBBFindParams p)
        {
            this.findParams = p;
        }

        #region PROPERTIES
        public bool ShowReply
        {
            set
            {
                if (value)
                {
                    pnlGridTextNote.RowDefinitions[1].Height = new GridLength(60);
                }
                else
                {
                    pnlGridTextNote.RowDefinitions[1].Height = new GridLength(0);
                }
            }
            get
            {
                if (pnlGridTextNote.RowDefinitions[1].Height.Value == 0)
                    return false;
                else return true;
            }
        }

        public bool ShowDocs
        {
            set
            {
                if (value)
                {
                    pnlGridMain.RowDefinitions[3].Height = new GridLength(120);
                }
                else
                {
                    pnlGridMain.RowDefinitions[3].Height = new GridLength(0);
                }
            }
            get
            {
                if (pnlGridMain.RowDefinitions[3].Height.Value == 0)
                    return false;
                else return true;
            }
        }

        //public DbNoteSharing NoteCurrent
        public DbNote NoteCurrent
        {
            set
            {
                this.currentRootNote = value;
                
                if (ctrlMBBDocs != null && value != null)
                    ctrlMBBDocs.FillDocs(this.currentRootNote.Docs);
                if (value == null || value.ID == 0)
                {
                    if (value == null)
                    {
                        this.currentRootNote = new DbNote();
                        this.currentNoteToSave = new DbNote();
                    }
                    this.currentRootNote.ID = 0;
                    this.currentRootNote.Docs = new List<DbNoteDoc>();
                    this.currentRootNote.SharedUsers = new List<string>();
                    this.txtMsg.Clear();
                    this.txtTitle.Clear();
                    //this.txtDate.Clear();
                    this.dtDate.SelectedDate = DateTime.Now;

                    SetRate(0);
                }
                else
                {
                    txtMsg.Text = value.Text;
                    txtTitle.Text = value.Title;
                    //txtDate.Text = value.DateTimeInserted.ToShortDateString() + " " + value.DateTimeInserted.ToShortTimeString();
                    this.dtDate.SelectedDate = value.DateTimeInserted;
                    SetRate(value.Rate);
                }

                if(this.currentNoteToSave==null)
                    this.currentNoteToSave = new DbNote();
                this.currentNoteToSave.Title = this.currentRootNote.Title;
                this.currentNoteToSave.Text = this.currentRootNote.Text;
                
                this.currentNoteToSave.IDUser = this.idUser;
                this.currentNoteToSave.IDNoteParent = this.currentRootNote.ID;
                this.currentNoteToSave.SharedUsers = new List<string>();
                //this.currentNoteToSave.Docs = this.currentRootNote.Docs;
                this.currentNoteToSave.UserNameParent = DbDataAccess.GetUser(this.currentRootNote.IDUser);
            }
            get { return this.currentRootNote; }
        }

        public bool IsPermanentlyClosed
        {
            set;
            get;
        }
        #endregion

        #region FILL
        public void GotoCurrentNote(List</*DbNoteSharing*/DbNote> lst)
        {
            this.currentDayNotes = lst;
            //-- mi riposiziono sulla nota corrente: devo farlo dato che ricarico la lista
            if (this.currentRootNote != null && this.currentRootNote.ID>0)
                this.NoteCurrent = this.currentDayNotes.FirstOrDefault(s => s.ID == this.currentRootNote.ID && s.IDUser == this.currentRootNote.IDUser);
            else
            {
                if(this.currentDayNotes.Count>0)
                    this.NoteCurrent = this.currentDayNotes.ElementAt(this.currentDayNotes.Count - 1);
            }

            if (this.currentDayNotes.Count > 0)
            {
                btnNext.IsEnabled = true;
                btnPrev.IsEnabled = true;
            }
            else
            {
                btnNext.IsEnabled = false;
                btnPrev.IsEnabled = false;
            }
        }

        //public List<DbNoteSharing> FillSharingNotes(MBBFindParams p)
        public List<DbNote> FillSharingNotes(MBBFindParams p)
        {
            this.findParams = p;
            string usersh = DbDataAccess.GetUser(p.IDUser);
            //List<DbNoteSharing> notes = DbDataAccess.GetNotesAndDocsForSharing(usersh, p.Dt1, p.Dt2, p.Rate, p.SimbolRate, p.Title, true);
            List<DbNote> notes = DbDataAccess.GetNotesAndDocsShared(usersh, p.Dt1, p.Dt2, p.Rate, p.SimbolRate, p.Title, true);
            
            //-- mi riposiziono sulla nota corrente: devo farlo dato che ricarico la lista
            GotoCurrentNote(notes);
            txtMsg.ScrollToEnd();
            return notes;
        }

        public List<DbNote> FillSharingNotes(List<DbNote> notes)
        {
            // DEVO RICOSTRUIRE L'ALBERO DELLE NOTE PADRE FIGLIO: NUOVA STRUTTURA
            //-- mi riposiziono sulla nota corrente: devo farlo dato che ricarico la lista
            notes = PrepareNotes(notes);
            GotoCurrentNote(notes);
            txtMsg.ScrollToEnd();
            return notes;
        }

        private List<DbNote> PrepareNotes(List<DbNote> notes)
        {
            List<DbNote> res = new List<DbNote>();

            var grp = notes.GroupBy(X => X.IDNoteParent);
            List<Int64> keys = grp.Select(X => X.Key).Distinct().ToList();
            foreach (int k in keys)
            {
                List<DbNote> notesgrp = notes.Where(X => X.IDNoteParent == k).ToList();
                DbNote newnote = new DbNote();
                string txt = "";
                int idx = 1;
                foreach (DbNote n in notesgrp)
                {
                    string user = DbDataAccess.GetUser(n.IDUser);
                    txt = txt + "\n-" + idx.ToString()+"-[" + n.DateTimeLastMod.ToShortDateString() +"]" + user + "\n" + n.Text + "\n";
                    idx++;
                }
                if (notesgrp.Count > 1)
                    newnote = notesgrp.First();

                newnote.Text = txt;
                res.Add(newnote);
            }

            return res;

        }
        #endregion

        #region SAVE NOTE-RESPONSE
        /*public void SaveNote()
        {
            string title = txtTitle.Text;
            string text = txtNoteResponse.Text;
            //string text = txtMsg.Text + 
            //              "\n\n" + "- " + this.user + " [" + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + "]" +
            //              "\n" + txtNoteResponse.Text;

            try
            {
              
                //-- salva nel db la nota
                //string userOwnerNote = this.currentRootNote.UserOwner;
                string userOwnerNote = DbDataAccess.GetUser(this.currentRootNote.IDUser);
                int rate = this.GetRate();
                
                // 2 POSSIBITA':
                // 1) INSERISCO UNA NUOVA NOTA
                // 2) AGGIORNO LA NOTA ESISTENTE sommando la risposta
                //  -quando raggiungo il limite di 3000 caratteri devo creare un file di testo
                int idresponse = DbDataUpdate.InsertNote(-1, title, text, this.idUser, this.currentRootNote.IDUser, this.currentRootNote.ID, userOwnerNote, rate, DateTime.Now);
               
                MBBSyncroParams p = new MBBSyncroParams();
                p.DropBoxRootDir = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_DROPBOXDIR);
                p.SharingDir = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_SHARINGDIR);
                //-- insert su file della nota per sincronizzazione db
                List<string> userSh = new List<string>();
                // userOwnerNote potrebbe essere l'utente corrente 
                //userSh.Add(userOwnerNote);
                //MBBSyncroUtility.WriteNoteSyncroDBFile(idresponse, title, text, rate, DateTime.Now, DateTime.Now, this.user, this.currentRootNote.IDNote, userOwnerNote, false, p );
                //MBBSyncroUtility.WriteNoteSyncroDBFileShared(idresponse, title, text, rate, DateTime.Now, DateTime.Now, this.user, userSh, this.currentRootNote.IDNote, userOwnerNote, p);
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("Error SaveNote: " + ex.Message, "");
            }
        }*/

        public void SaveNoteSingle()
        {
            string title = txtTitle.Text;
            string text = txtMsg.Text + 
                          "\n\n" + "- " + this.user + " [" + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + "]" +
                          "\n" + txtNoteReply.Text;

            try
            {

                //-- salva nel db la nota
                string userOwnerNote = DbDataAccess.GetUser(this.currentRootNote.IDUser);
                int rate = this.GetRate();
                Int64 idnote = 0;

                if (this.currentNoteToSave == null)
                {
                    DbNote newnote = new DbNote();
                    newnote.ID = DbDataAccess.GetNextIDNote(this.idUser);
                    newnote.SharedUsers = new List<string>();
                    newnote.Rate = rate;

                    this.currentNoteToSave = newnote;
                    //modeInsIpd = MBBConst.MODE_INS;
                }
                else idnote = this.currentRootNote.ID;

                this.currentNoteToSave.Text = text;
                this.currentNoteToSave.Title = title;
                this.currentNoteToSave.IDUser = this.idUser;
                this.currentNoteToSave.IDNoteParent = this.currentRootNote.ID;
                this.currentNoteToSave.UserNameParent = userOwnerNote;
                this.currentNoteToSave.Docs = new List<DbNoteDoc>();
                foreach (DbNoteDoc d in this.currentRootNote.Docs)
                {
                    DbNoteDoc dd = new DbNoteDoc();
                    dd.DateTimeLastMod = d.DateTimeLastMod;
                    dd.DocName = d.DocName;
                    dd.FileNameLocal = "";
                    dd.IDNote = currentNoteToSave.ID;
                    dd.IDUser = currentNoteToSave.IDUser;
                    dd.UserNoteOwner = userOwnerNote;
                    dd.Version = d.Version;
                    dd.UserMod = d.UserMod;
                    dd.IsNewFile = d.IsNewFile;
                    dd.InternalDirRemote = d.InternalDirRemote;

                    this.currentNoteToSave.Docs.Add(dd);
                }

                // 2 POSSIBITA':
                // 1) INSERISCO UNA NUOVA NOTA
                // 2) AGGIORNO LA NOTA ESISTENTE sommando la risposta
                //  -quando raggiungo il limite di 3000 caratteri devo creare un file di testo
                //int idresponse = DbDataUpdate.InsertNote(-1, title, text, this.idUser, this.currentRootNote.IDUser, this.currentRootNote.IDNote, userOwnerNote, rate, DateTime.Now);
                DbNote n = DbDataAccess.GetNote(this.idUser, this.currentNoteToSave.ID);
                /*bool isnewnote = text.Length > 3000 || n.ID == 0;
                Int64 idresponse = 0;
                if (n.ID == 0)
                    idresponse = DbDataUpdate.InsertNote(-1, title, text, this.idUser, this.currentRootNote.IDUser, this.currentNoteToSave.ID, userOwnerNote, rate, DateTime.Now);
                else
                    DbDataUpdate.UpdateNote(this.currentNoteToSave.ID, title, text, this.idUser, this.currentRootNote.IDUser, this.currentNoteToSave.ID, userOwnerNote, rate, DateTime.Now, DateTime.Now, false, false, false);
                */
                List<DbNote> lstNotesIns;
                List<DbNote> lstNotesDel;
                DbDataUpdate.InsertUpdateNoteWithSplit(
                             idnote
                            , title, text, this.idUser, 0, rate, false
                            , n.DateTimeInserted, DateTime.Now
                            , true
                            , this.currentNoteToSave.Docs, true
                            , userOwnerNote, out lstNotesIns, out lstNotesDel);


            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("Error SaveNoteSingle: " + ex.Message, "");
            }
        }

        public void ReplyNoteToOwner()
        {
            string title = txtTitle.Text;
            string text = txtNoteReply.Text;

            try
            {
                //-- salva nel db la nota
                string userOwnerNote = DbDataAccess.GetUser(this.currentRootNote.IDUser);
                int rate = this.GetRate();

                // SCRIVO LA NOTA NEL FILE DELL'OWNER A CUI RISPONDO
                MBBSyncroParams p = new MBBSyncroParams();
                p.DropBoxRootDir = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_DROPBOXDIR);
                p.SharingDir = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_SHARINGDIR);
                //-- insert su file della nota per sincronizzazione db
                List<string> userSh = new List<string>();
                // userOwnerNote potrebbe essere l'utente corrente 
                // l'ownwer potrebbe essere l'utente stesso che ha cominciato la conversazione:
                // DEVO SAPERE CHI HA RISPOSTO PER ULTIMO O MANDARE SEMPRE A TUTTI
                userSh.Add(userOwnerNote);

                MBBSyncroUtility.WriteNoteSyncroFileShared(this.currentRootNote.ID, title, text, rate, DateTime.Now, DateTime.Now, this.user, userSh, this.currentRootNote.ID, userOwnerNote,this.currentRootNote.IsFavorite, p);
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("Error SaveNote: " + ex.Message, "");
            }
        }

        public void ReplayNoteToAll()
        {
            string title = txtTitle.Text;
            string text = txtNoteReply.Text;

            try
            {
                //-- salva nel db la nota
                string userOwnerNote = DbDataAccess.GetUser(this.currentRootNote.IDUser);
                int rate = this.GetRate();

                // SCRIVO LA NOTA NEL FILE DELL'OWNER A CUI RISPONDO
                MBBSyncroParams p = new MBBSyncroParams();
                p.DropBoxRootDir = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_DROPBOXDIR);
                p.SharingDir = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_SHARINGDIR);
                // mando la risposta anche a me stesso: poi verrà recuperata nella conversazione
                //if (this.currentRootNote.SharedUsers.Contains(this.user))
                //    this.currentRootNote.SharedUsers.Remove(this.user);

                // se negli utenti condivisi non c'è l'owner devo aggiungerlo altrimenti non riceverà la risposta
                if (!this.currentRootNote.SharedUsers.Contains(userOwnerNote))
                    this.currentRootNote.SharedUsers.Add(userOwnerNote);

                MBBSyncroUtility.WriteNoteSyncroFileShared(this.currentRootNote.ID, title, text, rate, DateTime.Now, DateTime.Now, this.user, this.currentRootNote.SharedUsers, this.currentRootNote.ID, userOwnerNote,this.currentRootNote.IsFavorite, p);
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("Error SaveNote: " + ex.Message, "");
            }
        }
        #endregion

        #region manage RATE - SHARE
        public int GetRate()
        {
            int rate = 0;
            if (Convert.ToInt32(imgRat1.Tag) >= 10) rate++;
            if (Convert.ToInt32(imgRat2.Tag) >= 10) rate++;
            if (Convert.ToInt32(imgRat3.Tag) >= 10) rate++;
            if (Convert.ToInt32(imgRat4.Tag) >= 10) rate++;
            if (Convert.ToInt32(imgRat5.Tag) >= 10) rate++;

            return rate;
        }
       
        private void SetRate(int rate)
        {
            string star = CRSGlobalParams.IconRateYellowPathName;

            imgRat1.Source = CRSImage.CreateImage(CRSGlobalParams.IconRateGrayPathName);
            imgRat2.Source = CRSImage.CreateImage(CRSGlobalParams.IconRateGrayPathName);
            imgRat3.Source = CRSImage.CreateImage(CRSGlobalParams.IconRateGrayPathName);
            imgRat4.Source = CRSImage.CreateImage(CRSGlobalParams.IconRateGrayPathName);
            imgRat5.Source = CRSImage.CreateImage(CRSGlobalParams.IconRateGrayPathName);
            imgRat1.Tag = 1;
            imgRat2.Tag = 2;
            imgRat3.Tag = 3;
            imgRat4.Tag = 4;
            imgRat5.Tag = 5;
            switch (rate)
            {
                case 1:
                    imgRat1.Source = CRSImage.CreateImage(star);
                    imgRat1.Tag = 10;
                    break;
                case 2:
                    imgRat1.Source = CRSImage.CreateImage(star);
                    imgRat2.Source = CRSImage.CreateImage(star);
                    imgRat1.Tag = 10;
                    imgRat2.Tag = 20;
                    break;
                case 3:
                    imgRat1.Source = CRSImage.CreateImage(star);
                    imgRat2.Source = CRSImage.CreateImage(star);
                    imgRat3.Source = CRSImage.CreateImage(star);
                    imgRat1.Tag = 10;
                    imgRat2.Tag = 20;
                    imgRat3.Tag = 30;
                    break;
                case 4:
                    imgRat1.Source = CRSImage.CreateImage(star);
                    imgRat2.Source = CRSImage.CreateImage(star);
                    imgRat3.Source = CRSImage.CreateImage(star);
                    imgRat4.Source = CRSImage.CreateImage(star);
                    imgRat1.Tag = 10;
                    imgRat2.Tag = 20;
                    imgRat3.Tag = 30;
                    imgRat4.Tag = 40;
                    break;
                case 5:
                    imgRat1.Source = CRSImage.CreateImage(star);
                    imgRat2.Source = CRSImage.CreateImage(star);
                    imgRat3.Source = CRSImage.CreateImage(star);
                    imgRat4.Source = CRSImage.CreateImage(star);
                    imgRat5.Source = CRSImage.CreateImage(star);
                    imgRat1.Tag = 10;
                    imgRat2.Tag = 20;
                    imgRat3.Tag = 30;
                    imgRat4.Tag = 40;
                    imgRat5.Tag = 50;
                    break;
            }
        }

        private void SetRateFromImg(Image sender)
        {
            int rate = 0;
            if (Convert.ToInt32((sender as Image).Tag) >= 10)
                rate = Convert.ToInt32((sender as Image).Tag) / 10 - 1;
            else
                rate = Convert.ToInt32((sender as Image).Tag);

            SetRate(rate);
        }

        #endregion

        #region ctrl EVENTS functions prev/next/new

        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            GoPrevNote();
            txtMsg.ScrollToEnd();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            GoNextNote();
            txtMsg.ScrollToEnd();
        }

        private void GoNextNote()
        {
            this.btnPrev.IsEnabled = true;
            /*if (this.currentRootNote != null && this.modeInsUpdFind == MODE_FIND)
                idx = this.todayNotes.IndexOf(this.currentRootNote);*/
            if (this.currentDayNotes != null && this.currentRootNote != null)
            {
                int idx = this.currentDayNotes.IndexOf(this.currentRootNote);
                if (idx >= 0 && idx < this.currentDayNotes.Count - 1)
                {
                    this.NoteCurrent = this.currentDayNotes.ElementAt(idx + 1);

                    //chkIsRead.IsChecked = this.NoteCurrent.IsToRead == 0 ? true : false;
                    //-- è importante che ModeInsUpdFind sia assegnato dopo l'assegnazione a nodeCurrent altrimenti
                    //-- al cambiamento di txtMsg o txtTitle con focus potrebbe cambiare lo stato a MODE_UPD

                    ManageWinDoc();
                }
                else
                {   // modalità inseriemnto se vado oltre l'ultima nota             
                    this.btnNext.IsEnabled = false;
                }
            }
        }

        private void GoPrevNote()
        {
            this.btnNext.IsEnabled = true;
            if (this.currentDayNotes != null && this.currentRootNote != null)
            {
                int idx = this.currentDayNotes.IndexOf(this.currentRootNote);
                if (idx < 0)
                    idx = this.currentDayNotes.Count;
                if (idx > 0)
                {
                    this.NoteCurrent = this.currentDayNotes.ElementAt(idx - 1);
                    //chkIsRead.IsChecked = this.NoteCurrent.IsToRead == 0 ? true : false;
                    ManageWinDoc();
                }
                if (idx == 0)
                    this.btnPrev.IsEnabled = false;
            }
        }

        private void GoToNote(int iduser, Int64 idnote)
        {
            this.NoteCurrent = this.currentDayNotes.Where(X => X.IDUser == iduser && X.ID == idnote).FirstOrDefault();
        }

        private int ManageWinDoc()
        {
            int numdocs = 0;
            if (this.NoteCurrent != null && this.NoteCurrent.Docs != null && this.NoteCurrent.Docs.Count > 0)
            {
                numdocs = ctrlMBBDocs.FillDocs(this.NoteCurrent.Docs);
            }
            return numdocs;
        //-- da gestire layout: si deve vedere solo se esistono docs condivisi
        }

        private void btnNewNote_Click(object sender, RoutedEventArgs e)
        {
            //-- da gestire risposta
        }

        #endregion

        #region EVENTS
        private void imgRat1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SetRateFromImg(sender as Image);
        }

               
        private void btnGoResponde_Click(object sender, RoutedEventArgs e)
        {
            if (pnlGridTextNote.RowDefinitions[1].Height.Value > 0)
            {
                pnlGridTextNote.RowDefinitions[1].Height = new GridLength(0);
                btnGoResponde.LabelText = ">";
            }
            else
            {
                pnlGridTextNote.RowDefinitions[1].Height = new GridLength(60);
                btnGoResponde.LabelText = "<";
            }
        }

        private void btnSendAll_Click(object sender, RoutedEventArgs e)
        {
            //-- rispondi a tutti
            /*int iduser = this.currentRootNote.IDUser;
            int idnote = this.currentRootNote.IDNote;
            SaveNote();
            txtNoteResponse.Clear();
            FillSharingNotes(this.findParams);
            this.GoToNote(iduser, idnote);*/
            ReplayNoteToAll();
            txtNoteReply.Clear();
        }

        private void btnSendSingle_Click(object sender, RoutedEventArgs e)
        {
            //-- rispondi all'owner della nota condivisa
            ReplyNoteToOwner();
            txtNoteReply.Clear();
        }

        private void imgOk_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //-- insert note response
            try
            {
                int iduser = this.currentRootNote.IDUser;
                Int64 idnote = this.currentRootNote.ID;
                SaveNoteSingle();
                txtNoteReply.Clear();
                FillSharingNotes(this.findParams);
                this.GoToNote(iduser, idnote);
            }
            catch { }
        }

        #endregion

        private void chkIsRead_Click(object sender, RoutedEventArgs e)
        {
            /*
            bool res = DbDataUpdate.UpdateSharedNoteRead(this.currentRootNote.ID, this.currentRootNote.IDUser, this.user, !chkIsRead.IsChecked.Value);
            List<DbNoteSharing> sn = FillSharingNotes(this.findParams);
            this.GoToNote(this.currentRootNote.IDUser, this.currentRootNote.ID);
            */
        }

        // ORA HA POCO SENSO DATO CHE POSSO IN SEGUITO CANCELLARE LA NOTA SALVATA DA MBBCore direttamente
        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            /*
            // concedo cancellazione solo se sono owner della nota
            if (this.currentRootNote.IDUser == this.idUser)
            {

                MessageBoxResult r = MessageBox.Show(CRSTranslation.TranslateDialog(CRSTranslation.MSG_DEL_ITEMS, this.lang),
                                        CRSTranslation.TranslateDialog(CRSTranslation.MSG_WARNING, this.lang), MessageBoxButton.YesNo);

                if (r == MessageBoxResult.Yes)
                {
                    //-- prima scrive su file di cancellazione delle note per allineamento
                    //-- poi cancella altrimenti elimino i riferimenti su db per scrittura su file delle condivisioni
                    MBBSyncroParams p = new MBBSyncroParams();
                    p.DropBoxRootDir = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_DROPBOXDIR);
                    p.SharingDir = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_SHARINGDIR);
                    MBBSyncroUtility.WriteDelNote(DateTime.Now, this.currentRootNote.IDNote, this.currentRootNote.UserOwner, p);
                    bool res = DbDataUpdate.DeleteNote(this.currentRootNote.IDNote, this.currentRootNote.IDUser);

                    List<string> l = DbDataAccess.GetSharedUsers(this.currentRootNote.IDNote, this.currentRootNote.IDUser);
                    foreach(string dest in l)
                        MBBSyncroUtility.WriteDelNoteShared(DateTime.Now, this.currentRootNote.IDNote, this.user, dest, p);

                    // DEVO ELIMARE ANCHE TUTTE LE NOTE FIGLIE CHE RIMANGONO ORFANE
                }
            }
             * */
        }

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            this.Opacity = 1;
        }

        private void imgClose_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var parent = VisualTreeHelper.GetParent(this);
            while (!(parent is Window))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            (parent as Window).Hide();
            
        }

        private void imgExit_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var parent = VisualTreeHelper.GetParent(this);
            while (!(parent is Window))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
            this.IsPermanentlyClosed = true;
            (parent as Window).Hide();
        }

        private void btnNextDay_Click(object sender, RoutedEventArgs e)
        {
            dtDate.SelectedDate = ((DateTime)dtDate.SelectedDate.Value).AddDays(1);
            DateTime dt1 = dtDate.SelectedDate.Value.AddHours(-dtDate.SelectedDate.Value.Hour);
            dt1 = dt1.AddMinutes(-dt1.Minute);
            //DateTime dt2 = dtDate.SelectedDate.Value.AddHours(23-dtDate.SelectedDate.Value.Hour);
            //dt2 = dt2.AddMinutes(59-dt1.Minute);
            List<DbNote> lstSh = MBBSyncroUtility.SyncroGetNotesFromFileShared(this.user, dt1, 1);

            txtMsg.Clear();
            lstSh = PrepareNotes(lstSh);
            if (lstSh.Count > 0)
                this.NoteCurrent = lstSh.First();
            txtMsg.ScrollToEnd();
        }

        private void btnPrevDay_Click(object sender, RoutedEventArgs e)
        {
            dtDate.SelectedDate = ((DateTime)dtDate.SelectedDate.Value).AddDays(-1);
            DateTime dt1 = dtDate.SelectedDate.Value.AddHours(-dtDate.SelectedDate.Value.Hour);
            dt1 = dt1.AddMinutes(-dt1.Minute);
            //DateTime dt2 = dtDate.SelectedDate.Value.AddHours(23 - dtDate.SelectedDate.Value.Hour);
            //dt2 = dt2.AddMinutes(59 - dt1.Minute);
            List<DbNote> lstSh = MBBSyncroUtility.SyncroGetNotesFromFileShared(this.user, dt1, 1);

            txtMsg.Clear();
            lstSh = PrepareNotes(lstSh);
            if(lstSh.Count>0)
                this.NoteCurrent = lstSh.First();

            txtMsg.ScrollToEnd();
        }

        private void imgSync_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DateTime dt1 = dtDate.SelectedDate.Value.AddHours(-dtDate.SelectedDate.Value.Hour);
                dt1 = dt1.AddMinutes(-dt1.Minute);
                //DateTime dt2 = dtDate.SelectedDate.Value.AddHours(23 - dtDate.SelectedDate.Value.Hour);
                //dt2 = dt2.AddMinutes(59 - dt1.Minute);
                List<DbNote> lstSh = MBBSyncroUtility.SyncroGetNotesFromFileShared(this.user, dt1, 1);
                
                FillSharingNotes(lstSh);
                
            }
            catch { }
        }

       
        
    }
}
