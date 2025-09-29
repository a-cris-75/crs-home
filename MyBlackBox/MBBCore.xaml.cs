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
using System.Windows.Threading;
using System.IO;
using CRS.Library;
using MyBlackBoxCore.DataEntities;
using System.Diagnostics;
using Microsoft.Win32;
using System.Threading;
using System.Threading.Tasks;
//using CRS.ManageEvents;
//using static CRS.ManageEvents.ManageHookEvents;


namespace MyBlackBoxCore
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MBBCore : UserControl
    {
        // questa classe si occupa di gestire gli eventi in base ai parametri del file ini
        //private ManageHookEvents MBBEvents;

        private int refreshTimerSharedNotes = 0;
        private int refreshTimerDocs = 0;

        private Thread ThreadImportData;
        private Thread ThreadImportShared;

        public delegate void EventExpand();
        private EventExpand eventExpandFalse; 
        private EventExpand eventExpandTrue;

        public delegate void EventSelectCurrentNote();
        private EventSelectCurrentNote eventSelectCurrentNote;

        public delegate void EventHideWindow();
        private EventHideWindow eventHideWindow;

        public delegate void EventInitIntro(string intro, double perc);
        private EventInitIntro eventInitIntro;

        //-- lista dei documenti in attesa di conferma di sincronizzazione, da sincro automatica
        private List<MBBSyncroPending> docsPendingSyncro = new List<MBBSyncroPending>();
        //- doc appena inseriti salvati da allineare in upload: la lista viene aggiornata in SaveNote()
        private List<MBBSyncroPending> docsPending = new List<MBBSyncroPending>();

        public List<DbNote> NotesFound = new List<DbNote>();
        private List<DbNote> notesRecent = new List<DbNote>();

        public List<DbNoteStatus> NotesModToSend = new List<DbNoteStatus>();

        private int modeLayoutInsUpdFind = 0; // 0: ins, 1: upd
        private DbNote currentNote;
        // il controllo si sta inizializzando: utile per non far partire il timer all'inizio
        private bool isInitializing = false;

        readonly DispatcherTimer dispatcherTimerWritingNote = new DispatcherTimer();
        readonly DispatcherTimer dispatcherTimerNotifyStatus = new DispatcherTimer();
        readonly DispatcherTimer dispatcherTimerSyncroSharedNotes = new DispatcherTimer();
        readonly DispatcherTimer dispatcherTimerSyncroDocs = new DispatcherTimer();

        private const int DIM_H_BIG_TEXT= 300;
        private const int DIM_HEADER = 140;
        private const int DIM_BORDER = 46;
        private const int DIM_FOOTER = 55;//46;
        private const int DIM_CENTER = 150;
        private const int DIM_W = 300;// 260;
        private const int DIM_H = 480; //300
        private const int DIM_W_R = 300;//260;
        private const int DIM_H_R = 360;//320;

        public const int EXPAND_FALSE = 0;
        public const int EXPAND_STEP1_AUTO = 1;
        public const int EXPAND_STEP2_FREEE = 2;
        public const int EXPAND_STEP3_MAXHGT = 3;
        public const int EXPAND_FALLOW_CURSOR = -1;
        public const int EXPAND_STEP4_MAXIMIZED = 4;
        WinMBBDocs winDocs;
        WinMBBFind winFind;
        WinMBBFindTitle winFindTitle;
        MBBFindParams findParams;
        WinMBBSharingUsers winSharingUsers;
        WinMBBSharingNotes winSharingNotes;
        
        public int ExpandLayoutStep = 0;
        private bool isOpacity = false;
        private int dimFieldTxtMsg = 0;

        public int HEIGTH_FOOTER_FOR_REDIM_AUTO = 0;

        private long chianed_id_activity =-1;
        public long CHAINED_ID_ACTIVITY 
        {
            set {
                this.chianed_id_activity = value;
            }
            get { return this.chianed_id_activity; }
        }

        public bool IS_ACTIVE_KEY_EVENTS
        {
            set
            {
                if(value)
                    imgStartStopLog.Source = CRSImage.CreateImage(CRSGlobalParams.ImageStopPathName);
                else
                    imgStartStopLog.Source = CRSImage.CreateImage(CRSGlobalParams.ImageStartPathName);
            }
            get {
                return imgStartStopLog.Source.ToString().Contains(System.IO.Path.GetFileName(CRSGlobalParams.ImageStopPathName));
            }
        }

        public MBBCore()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                string e = ex.Message;
            }
        }

        #region INIT
        public bool Init(out string user, EventExpand evnF, EventExpand evnT, EventSelectCurrentNote evnSelNote, EventInitIntro evnIntro, EventHideWindow evnHide)
        {
            this.isInitializing = true;

            this.eventExpandFalse = evnF;
            this.eventExpandTrue = evnT;
            this.eventSelectCurrentNote = evnSelNote;
            this.eventInitIntro = evnIntro;
            this.eventHideWindow = evnHide;

            user = InitFromIniResUser();
            bool res = !string.IsNullOrEmpty(user);

            if (res)
            {
                //SetLayoutExpand(-1);

                if (this.winFind == null)
                    this.winFind = new WinMBBFind(this.UserInfo.IDUser);
                if (this.winFindTitle == null)
                    this.winFindTitle = new WinMBBFindTitle(this.UserInfo.IDUser);
                if (this.winDocs == null)
                    this.winDocs = new WinMBBDocs(this.UserInfo.IDUser);

                SetTimers();
                this.winFind.SetEvnSetFilter(FillNotesFromParams);
                this.winFind.SetEvnDblClickNote(FillSingleNote);
                this.winFind.SetEvnMergeNotes(FillMergedNotes);
                this.winFind.SetEvnOpenNotes(FillSelectedNotes);
                this.winFindTitle.SetEvnSelectTitle(EventSelectTitle);

                this.winDocs.SetEventRefresh(FillNotesFromParams);
                MBBSyncroUtility.Init(DateTime.Now);

                //SPOSTATO IN WinMBBCore
                //-- devo sicronizzarmi all'avvio dato che altrimenti potrei inserire una nota con id già
                //-- presente su altro db in altra macchina
                //-- ATTENZIONE: dato che l'id corrisponde a un long con la data corrente di inserimento è impossibile che l'id sia duplicato => 
                //   posso fare la sincronizzazione su thread separato mentre uso il programma
                //SetThreadSyncroNotes();

                MBBSyncroUtility.SyncroDBFromFileUsers(this.UserInfo.Name);

                if (MBBCommon.syncroParams.SyncFileDimDays > 0)
                {
                    /*string remoteDir = MBBSyncroUtility.GetUserRemoteDir(this.User, false, this.syncroParams);
                    CRSIni fileSyncroDB_UPD = MBBSyncroUtility.GetIniFileSync(MBBConst.SYNCRO_UPD, remoteDir);
                    // in realtà devo fare il backup solo quando il backup precedente ha un adata inferiore alla data attuale meno i giorni di
                    // mantenimento dei dati 
                    //CRSFileUtil.BackupFile(remoteDir, fileSyncroDB_UPD.Name);
                    */
                }
                FillNotesFromParams();
                this.CurrentNote = new DbNote();
                this.ModeLayoutInsUpdFindClose = MBBConst.MODE_CLOSE;

                this.dimFieldTxtMsg = DbDataAccess.GetSizeFieldTextMsg();
                this.isInitializing = false;

                this.FillRecentNotesFromIni();
                this.SetPosCorner();

                //MBBEvents = new ManageHookEvents(CRSIniFile.IniPathName);
            }
            return res;
        }

        private string InitFromIniResUser()
        {
            string resUser = "";
            try
            {
                DbDataAccess.Init(CRSIniFile.GetConnectionStringFromIni(), CRSIniFile.GetProviderFromIni());
                DbDataUpdate.Init(CRSIniFile.GetConnectionStringFromIni(), CRSIniFile.GetProviderFromIni());
                MBBCommon.syncroParams.InitParamsFromIni();
                MBBCommon.emailParams.InitParamsFromIni();

                if (string.IsNullOrEmpty(MBBCommon.syncroParams.SharingDir))
                {
                    //-- open window params
                    WinMBBParams win = new WinMBBParams();
                    win.ctrlMBBParams.ctrlDirSharing.SetValidationError("Insert value!");
                    win.ctrlMBBParams.ctrlDirStorageDropBox.SetValidationError("Insert value!");
                    bool isOk = win.ShowDialog().Value;
                    if(isOk){
                        MBBCommon.syncroParams.SharingDir = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_SHARINGDIR);
                        MBBCommon.syncroParams.DropBoxRootDir = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_DROPBOXDIR);
                    }
                }

                if (string.IsNullOrEmpty(MBBCommon.syncroParams.SharingDir))
                {
                    MBBCommon.syncroParams.SharingDir = ".\\MBBSHARED";
                    CRSIniFile.SetPropertyValue(APPConstants.SEC_MYBLACKBOX, APPConstants.PROP_SHARINGDIR, MBBCommon.syncroParams.SharingDir);
                }
                if (string.IsNullOrEmpty(MBBCommon.syncroParams.DropBoxRootDir))
                {
                    MBBCommon.syncroParams.DropBoxRootDir = "\\MyBlackBox";
                    CRSIniFile.SetPropertyValue(APPConstants.SEC_MYBLACKBOX, APPConstants.PROP_DROPBOXDIR, MBBCommon.syncroParams.DropBoxRootDir);
                }
                if (string.IsNullOrEmpty(MBBCommon.syncroParams.LocalTempDir))
                {
                    MBBCommon.syncroParams.LocalTempDir = AppDomain.CurrentDomain.BaseDirectory + "\\Docs";
                    CRSIniFile.SetPropertyValue(APPConstants.SEC_MYBLACKBOX, APPConstants.PROP_LOCALTMPDIR, MBBCommon.syncroParams.LocalTempDir);
                }

                this.UserInfo = new MBBUserInfo();
                resUser = UserInfo.Name;
                
                string s = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_TIMER_SYNCRO, 30);
                if (string.IsNullOrEmpty(s) == false)
                    this.refreshTimerSharedNotes = Convert.ToInt32(s);

                //s = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_REFRESH_TIMER_SHARING, 30);
                s = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_TIMER_SYNCRO_DOCS, 30);
                if (string.IsNullOrEmpty(s) == false)
                    this.refreshTimerDocs = Convert.ToInt32(s);

                //-- attivo il timer solo se l'autosyncro è true
                if (MBBCommon.syncroParams.AutoSync == false)
                    this.refreshTimerSharedNotes = 0;

                if (this.UserInfo.IDUser < 0)
                {
                    //-- se l'id non esiste allora è il primo accesso
                    resUser = "";
                }
                string sep = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_SEPARTATION_CHAR_TITLE, " ");
                if (string.IsNullOrEmpty(sep))
                    sep = " ";
                MBBCommon.SeparationCharTitle = sep;
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("Error Init control MBB core: " + ex.Message, "");
            }

            return resUser;
        }

        #endregion

        #region TASK SYNCRO WITH EMAIL NON USATO
        public Thread StartThreadSyncroWithEmail()
        {
            Thread newt = new Thread(TaskSyncroWithEmail);
            newt.SetApartmentState(ApartmentState.STA);
            newt.IsBackground = true;
            newt.Start();
            newt.Join();
            return newt;
        }

        private  void TaskSyncroWithEmail()
        {
            bool res = MBBSyncroUtility.SyncroReadFromEmail(this.UserInfo);            
        }

        private async void TaskAsynkSyncroWithEmail()
        {
            bool res = await TaskAsynk();
        }

        private Task<bool> TaskAsynk()
        {
            return Task.Run<bool>(()=> MBBSyncroUtility.SyncroReadFromEmail(this.UserInfo));
        }

        #endregion

        #region TASK SYNCRO con form WinMBBIntro

        public Thread StartThreadSyncroWithFilesAndEmail()
        {
            Thread newt = new Thread(new ThreadStart(TaskSyncroNotes));
            newt.SetApartmentState(ApartmentState.STA);
            newt.Name = "ThreadIntroOpenFormIntro";
            newt.IsBackground = false;
            newt.Start();
            this.ThreadImportData = newt;
            return newt;
        }

        public Thread StartThreadSyncroShared()
        {
            Thread newt = null;
            try
            {
                if (this.ThreadImportShared != null) 
                {
                    if(this.ThreadImportShared.IsAlive)
                        this.ThreadImportShared.Abort();
                    this.ThreadImportShared = null;
                }
           
                if (Utils.ExistWindowInstance<WinMBBIntro>() == null)
                {
                    newt = new Thread(new ThreadStart(TaskSyncroNotesShared));
                    newt.SetApartmentState(ApartmentState.STA);
                    newt.Name = "ThreadIntroSharedOpenFormIntro";
                    newt.IsBackground = false;
                    newt.Start();
                }
            }
            catch(Exception ex)
            {
                CRSLog.WriteLogException("Exception in StartThreadSyncroShared", ex, 1);
            }
            this.ThreadImportShared = newt;
            return newt;
        }

        /// <summary>
        /// Apre form WinMBBIntro che all'interno gestisce timer per avviare procedura di import delle note.
        /// L'apertura della form è su nuovo thread in modo da poter gestire le operazioni in background.
        /// Funziona così: 
        /// 1) apro form e avvio timer (
        /// 2) avvio con System.Windows.Threading.Dispatcher.Run() la gestione del thread: serve per poter vedere il form e le informazioni aggiornate (in refresh)
        /// 3) scatta timer e si avvia la procedura di import e stoppo timer
        /// 4) ad ogni ciclo (1 settimana) visualizzo le informazioni: num note della settimana importate, totale note importate
        /// 5) al termine mostro messaggio di fine, chiudo la form
        /// </summary>
        private void TaskSyncroNotes()
        {
            try
            {
                WinMBBIntro winintro = new WinMBBIntro(this.UserInfo, null);
                winintro.ActivateTypeSyncro(true, false, false, false, 0);
                winintro.Show();
                System.Windows.Threading.Dispatcher.Run();
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("Exception in TaskSyncroNotes: " + ex.Message, this.UserInfo.Name);
            }
        }

        private void TaskSyncroNotesShared()
        {
            try
            {
                //if (Utils.ExistWindowInstance<WinMBBIntro>()==null)
                {
                    WinMBBIntro winintro = new WinMBBIntro(this.UserInfo, null);
                    winintro.Name = "WinIntroNotesShared";
                    winintro.ActivateTypeSyncro(false, false, true, false, 0);
                    winintro.Show();
                    System.Windows.Threading.Dispatcher.Run();
                }
                //else CRSLog.WriteLog("WinIntroNotesShared already exists: ", this.UserInfo.Name);
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("Exception in TaskSyncroNotesShared: " + ex.Message, this.UserInfo.Name);
            }
        }
        #endregion

        #region FILL
        private void FillCurrentNotes(List<DbNote> lst)
        {
            this.NotesFound = lst;
            //-- mi riposiziono sulla nota corrente: devo farlo dato che ricarico la lista
            if (this.currentNote != null)
            {
                try
                {                   
                    int offsetpos = txtMsg.Document.ContentStart.GetOffsetToPosition(txtMsg.Selection.Start);
                    this.CurrentNote = this.NotesFound.FirstOrDefault(s => s.ID == this.currentNote.ID && s.IDUser == this.UserInfo.IDUser);
                    if (this.CurrentNote != null && this.CurrentNote.ID > 0)
                    {
                        txtMsg.CaretPosition = txtMsg.Document.ContentStart.GetPositionAtOffset(offsetpos + 1);                     
                    }
                }
                catch(Exception ex)
                {
                    CRSLog.WriteLog("Exception in FillCurrentNotes: \n" + ex.Message, "");
                }
            }

            if (this.NotesFound.Count > 0)
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

        private void FillNotesFromParams()
        {          
            this.findParams = this.winFind.GetFilterParams();
            FillCurrentNotes(this.winFind.FillNotes(this.findParams));
        }

        public void FillSingleNote(DbNote n)
        {
            bool bcont = true;

            if (this.modeLayoutInsUpdFind == MBBConst.MODE_UPD || (this.modeLayoutInsUpdFind == MBBConst.MODE_INS && this.IsNullNote() == false))
            {
                MessageBoxResult r = MessageBox.Show(
                    CRSTranslation.TranslateDialog(CRSTranslation.MSG_DATA_NOT_SAVED, this.UserInfo.Language) + "\n" +
                    CRSTranslation.TranslateDialog(CRSTranslation.MSG_DO_YOU_WANT_CONTINUE, this.UserInfo.Language),
                    CRSTranslation.TranslateDialog(CRSTranslation.MSG_WARNING, this.UserInfo.Language), MessageBoxButton.YesNo);
                if (r == MessageBoxResult.No)
                {
                    bcont = false;
                }
            }

            if (bcont)
            {                
                var res = from p in this.NotesFound
                          where p.ID == n.ID && p.IDUser == n.IDUser
                          select p;

                if (res.Count() == 0)
                {
                    this.NotesFound.Add(n);
                }
                this.CurrentNote = n;
                this.ModeLayoutInsUpdFindClose = MBBConst.MODE_FIND;
                Window win = Window.GetWindow(this.Parent);
                if (win != null)
                {
                    win.Focus();
                }
            }
        }

        private void FillMergedNotes(List<DbNote> lst)
        {
            bool bcont = true;

            if (this.modeLayoutInsUpdFind == MBBConst.MODE_UPD || (this.modeLayoutInsUpdFind == MBBConst.MODE_INS && this.IsNullNote() == false))
            {
                MessageBoxResult r = MessageBox.Show(
                    CRSTranslation.TranslateDialog(CRSTranslation.MSG_DATA_NOT_SAVED, this.UserInfo.Language) + "\n" +
                    CRSTranslation.TranslateDialog(CRSTranslation.MSG_DO_YOU_WANT_CONTINUE, this.UserInfo.Language),
                    CRSTranslation.TranslateDialog(CRSTranslation.MSG_WARNING, this.UserInfo.Language), MessageBoxButton.YesNo);
                if (r == MessageBoxResult.No)
                {
                    bcont = false;
                }
            }

            if (bcont)
            {
                //-- creo una nuova nota mettendo insieme più note;
                this.ModeLayoutInsUpdFindClose = MBBConst.MODE_INS;
                this.CurrentNote.ID = DbDataAccess.GetNextIDNote(this.UserInfo.IDUser);
                foreach (DbNote n in lst)
                {                   
                    //txtMsg.Text = txtMsg.Text +"\n\n" + n.DateTimeInserted.ToShortDateString() + " " + n.DateTimeInserted.ToShortTimeString() + " - " +
                    //                n.Title + "\n" + n.Text;

                    this.SetTextNote(this.GetTextNoteContent() + "\n\n" + n.DateTimeInserted.ToShortDateString() + " " + n.DateTimeInserted.ToShortTimeString() + " - " +
                                    n.Title + "\n" + n.Text);

                    foreach (DbNoteDoc d in n.Docs)
                        this.CurrentNote.Docs.Add(d);
                }
                
                Window win = Window.GetWindow(this.Parent);
                if (win != null)
                {
                    win.Focus();
                }
            }
        }

        private void FillSelectedNotes(List<DbNote> lst)
        {
            bool bcont = true;

            if (this.modeLayoutInsUpdFind == MBBConst.MODE_UPD || (this.modeLayoutInsUpdFind == MBBConst.MODE_INS && this.IsNullNote() == false))
            {
                MessageBoxResult r = MessageBox.Show(
                    CRSTranslation.TranslateDialog(CRSTranslation.MSG_DATA_NOT_SAVED, this.UserInfo.Language) + "\n" +
                    CRSTranslation.TranslateDialog(CRSTranslation.MSG_DO_YOU_WANT_CONTINUE, this.UserInfo.Language),
                    CRSTranslation.TranslateDialog(CRSTranslation.MSG_WARNING, this.UserInfo.Language), MessageBoxButton.YesNo);
                if (r == MessageBoxResult.No)
                {
                    bcont = false;
                }
            }

            if (bcont)
            {
                this.NotesFound = lst;

                this.CurrentNote = lst.Last();
                this.ModeLayoutInsUpdFindClose = MBBConst.MODE_FIND;
                Window win = Window.GetWindow(this.Parent);
                if (win != null)
                {
                    win.Focus();
                }
            }
        }

        public void FillAllDocsUser(DateTime dt1, DateTime dt2)
        {
            this.winDocs.FillDocs(this.UserInfo.IDUser, dt1, dt2);
        }

        #endregion

        #region PROPERTIES
        public MBBUserInfo UserInfo = null;

        public DbNote CurrentNote
        {
            set
            {
                this.currentNote = value;
                if (this.winDocs != null && value!=null)
                    this.winDocs.FillDocs(this.currentNote.Docs);
                if (value==null || value.ID == 0)
                {
                    if (value == null)
                    {
                        this.currentNote = new DbNote();
                        this.currentNote.SharedUsers = new List<string>();
                    }
                    this.currentNote.ID = 0;
                    this.currentNote.Docs = new List<DbNoteDoc>();
                    this.currentNote.SharedUsers = new List<string>();                  
                    this.SetTextNote("");// this.txtMsg.Clear();
                    this.txtTitle.Clear();
                    this.txtDate.Clear();
                    SetRate(0);
                    SetShare(this.currentNote.SharedUsers);
                    SetFavorite(this.currentNote.IsFavorite);
                    RefreshListNotes(value, this.notesRecent, false);
                }
                else
                {                 
                    SetTextNote( value.Text);
                    txtTitle.Text = value.Title;
                    txtDate.Text = value.DateTimeInserted.ToShortDateString() + " " + value.DateTimeInserted.ToShortTimeString();
                    SetRate(value.Rate);
                    SetShare(value.SharedUsers);
                    SetFavorite(value.IsFavorite);   
                    RefreshListNotes(value, this.notesRecent, false);
                    if (eventSelectCurrentNote != null)
                        this.eventSelectCurrentNote();
                }
                
            }
            get { return this.currentNote; }
        }

        /// <summary>
        /// Set e Get per mostrare i bottoni e il testo
        /// </summary>       
        public int ModeLayoutInsUpdFindClose
        {
            set
            {
                this.modeLayoutInsUpdFind = value;
                SetTextInfoNotes(value);

                if (value == MBBConst.MODE_FIND || value == MBBConst.MODE_CLOSE)
                {
                    this.dispatcherTimerNotifyStatus.IsEnabled = false;
                    lblInfo.Visibility = System.Windows.Visibility.Visible;
                    lblInfo.Foreground = Brushes.Gray;
                    
                }
                else
                {
                    this.dispatcherTimerNotifyStatus.IsEnabled = true;
                }

                if (value == MBBConst.MODE_SYNCRO_DOCS)
                {
                }
                else
                {

                }
                if (value == MBBConst.MODE_SYNCRO_NOTES)
                {
                    // riavvia il timer che ricomincia a contare da dove aveva terminato
                    //this.dispatcherTimerSyncroNotesImg.IsEnabled = true;  
                }
                else
                {
                    //this.dispatcherTimerSyncroNotesImg.IsEnabled = false;   
                    //imgSync.Visibility = System.Windows.Visibility.Visible;
                }
                // disabilito timer per sincro se sono in inserimento nota
                if (value == MBBConst.MODE_INS || value == MBBConst.MODE_UPD)
                {
                    // riavvio il timer resettandolo in modo che solo dopo che l'utente non digita per 10 sec la nota viene salvata
                    if (MBBCommon.syncroParams.AutoSave) this.dispatcherTimerWritingNote.Start();
                    this.pnlOkCanc.Visibility = System.Windows.Visibility.Visible;
                    this.imgExit.Visibility = System.Windows.Visibility.Hidden;

                    // non consento azioni finchè non salvo o annullo modifica (anche se c'è il salvacondotto con salvatggio automatico)
                    btnNext.IsEnabled = false;
                    btnPrev.IsEnabled = false;
                    btnNewNote.IsEnabled = false;
                    btnDelNote.IsEnabled = false;
                    btnRecentNext.IsEnabled = false;
                    btnRecentPrev.IsEnabled = false;
                    if (this.ExpandLayoutStep == EXPAND_STEP1_AUTO)
                        this.ExpandAuto();                             
                }
                else
                {
                    if (MBBCommon.syncroParams.AutoSave) this.dispatcherTimerWritingNote.Stop();
                    
                    this.pnlOkCanc.Visibility = System.Windows.Visibility.Hidden;                 
                    this.imgExit.Visibility = System.Windows.Visibility.Visible;

                    btnNext.IsEnabled = true;
                    btnPrev.IsEnabled = true;
                    btnNewNote.IsEnabled = true;
                    btnDelNote.IsEnabled = true;
                    btnRecentNext.IsEnabled = true;
                    btnRecentPrev.IsEnabled = true;
                    int idx = this.NotesFound.IndexOf(this.currentNote);
                    if (idx == 0) btnPrev.IsEnabled = false;
                }

                if (value == MBBConst.MODE_CLOSE)
                {
                    this.imgCanc.Visibility = System.Windows.Visibility.Hidden;
                    lblAutoExpand.Visibility = Visibility.Hidden;
                }
                else
                {
                    this.imgCanc.Visibility = System.Windows.Visibility.Visible;
                    lblAutoExpand.Visibility = Visibility.Visible;
                }
            }
            get
            {
                return this.modeLayoutInsUpdFind;
            }
        }

        private void SetTextInfoNotes(int mode)
        {
            int idx = this.NotesFound.IndexOf(this.currentNote) + 1;
            lblNumNotes.Text = "(" + idx.ToString() + "/" + this.NotesFound.Count.ToString() + " " +
                                CRSTranslation.TranslateDefaultWords(CRSTranslation.WRD_NOTES, this.UserInfo.Language) + ")";
            //lblInfo.Margin = new Thickness(0, 10, 0, 0);
            if (mode == MBBConst.MODE_INS)
            {
                lblInfo.Text = CRSTranslation.TranslateDefaultWords(CRSTranslation.WRD_INSERT, this.UserInfo.Language) + "..";
                this.CurrentNote = new DbNote();
            }
            else
            if (mode == MBBConst.MODE_UPD)
            {
                lblInfo.Text = CRSTranslation.TranslateDefaultWords(CRSTranslation.WRD_MODIFY, this.UserInfo.Language) + "..";
            }
            else
            if (mode == MBBConst.MODE_FIND)
            {

                lblInfo.Text = CRSTranslation.TranslateDefaultWords(CRSTranslation.WRD_FIND, this.UserInfo.Language) +
                                "..\n(" + idx.ToString() + "/" + this.NotesFound.Count.ToString() + " " +
                                CRSTranslation.TranslateDefaultWords(CRSTranslation.WRD_NOTES, this.UserInfo.Language) + ")";

                MBBFindParams p = this.winFind.GetFilterParams();
                if (p != null)
                {
                    string t2 = ((p.Dt2.Subtract(p.Dt1).TotalDays < 365) ? CRSTranslation.TranslateDefaultWords(CRSTranslation.WRD_DAYS, this.UserInfo.Language) + ": " + p.Dt2.Subtract(p.Dt1).TotalDays.ToString() + "  " : "") +
                         ((p.Rate > 0) ? (CRSTranslation.TranslateDefaultWords(CRSTranslation.WRD_RATE, this.UserInfo.Language) + ": " + p.SimbolRate + " " + p.Rate.ToString() + ", ") : "") + ((p.OnlyFavorites) ? "  " + CRSTranslation.TranslateDefaultWords(CRSTranslation.WRD_FAVORITES, this.UserInfo.Language) : "");

                    string tt =
                         (!string.IsNullOrEmpty(p.Title) ? CRSTranslation.TranslateDefaultWords(CRSTranslation.WRD_TITLE, this.UserInfo.Language) + ": " + p.Title : "") +
                         (!string.IsNullOrEmpty(t2) ? "\n" + t2 : "");


                    if (string.IsNullOrEmpty(tt))                 
                        tt = " -- ";                   

                    lblInfo.ToolTip = "<< " + CRSTranslation.TranslateDialog(CRSTranslation.MSG_CURRENT_FILTER, this.UserInfo.Language) + " >> " +
                         "\n" + tt;
                    if (tt.Length > 80) tt = tt.Substring(0, 78) + "..";
                    lblInfo.Text = CRSTranslation.TranslateDialog(CRSTranslation.MSG_CURRENT_FILTER, this.UserInfo.Language) + ": " + tt;
                    
                    if(lblInfo.LineCount == 1)                   
                        lblInfo.Text = CRSTranslation.TranslateDialog(CRSTranslation.MSG_CURRENT_FILTER, this.UserInfo.Language) + ": \n" + tt;
                    
                    //if (lblInfo.LineCount > 2)
                    //    lblInfo.Margin = new Thickness(0, 0, 0, 0);

                    lblNumNotes.Visibility = Visibility.Visible;

                }
            }
            else
            if (mode == MBBConst.MODE_CLOSE)
            {
                string numnotes = "\n(" + this.NotesFound.Count.ToString() + " " +
                                CRSTranslation.TranslateDefaultWords(CRSTranslation.WRD_NOTES, this.UserInfo.Language) + ")";
                if (this.NotesFound.Count == 0) numnotes = "";

                lblInfo.Text = CRSTranslation.TranslateDialog(CRSTranslation.MSG_INIT_MBB_CLOSE, this.UserInfo.Language) + numnotes;
                                
                lblNumNotes.Visibility = Visibility.Hidden;
            }
            else
            if (mode == MBBConst.MODE_SYNCRO_NOTES)
            {
                lblInfo.Text = CRSTranslation.TranslateDefaultWords(CRSTranslation.WRD_SYNCRO, this.UserInfo.Language);
            }
            else
            if (mode == MBBConst.MODE_SYNCRO_DOCS)
            {
                lblInfo.Text = CRSTranslation.TranslateDefaultWords(CRSTranslation.WRD_SYNCRO_DOCS, this.UserInfo.Language);
            }
        }

        #endregion

        #region LAYOUT PROP/METHODS

        public bool Expanded
        {
            set
            {
                this.Visibility = System.Windows.Visibility.Hidden;
                SetVisibilityPanelsOpenClose(!value);
                if (!value)
                {
                    if (this.eventExpandFalse != null)
                        this.eventExpandFalse();

                    this.ExpandLayoutStep = EXPAND_FALSE;

                    Window win = Window.GetWindow(this.Parent);
                    double currentHgtHeader = pnlGridMain.RowDefinitions[0].ActualHeight;
                    pnlTxtMsg.Visibility = Visibility.Hidden;
                    double currenttopBox = currentHgtHeader;
                    if (win != null)
                    {
                        currenttopBox = win.Top + currentHgtHeader;
                        win.Height = DIM_H_R;
                        win.Width = DIM_W_R;
                        win.Top = currenttopBox - pnlGridMain.RowDefinitions[0].ActualHeight - 6;
                        win.ResizeMode = ResizeMode.NoResize;
                    }

                    if (this.winFind != null)
                        this.winFind.Hide();
                    if (this.winFindTitle != null)
                        this.winFindTitle.Hide();
                    if (this.winDocs != null)
                        this.winDocs.Hide();
                    if (this.winSharingUsers != null)
                        this.winSharingUsers.Hide();
                    
                    //SetPosCorner();
                    //-- mode close e Expanded= false è la stessa cosa
                    this.ModeLayoutInsUpdFindClose = MBBConst.MODE_CLOSE;
                }
                else
                {
                    if (this.eventExpandTrue != null)
                        this.eventExpandTrue();

                    Window win = Window.GetWindow(this.Parent);
                    if (win != null)
                    {
                        win.Height = DIM_H;
                        win.Width = DIM_W;
                        win.ResizeMode = ResizeMode.CanResizeWithGrip;
                    }
                    SetPosFormsFromMainForm();
                  
                }
                this.Visibility = System.Windows.Visibility.Visible;
            }
            get
            {
                if (pnlTxtMsg.Visibility == Visibility.Visible)
                    return true;
                else return false;
            }
        }

        public void ExpandFollowCursor()
        {
            this.Cursor = Cursors.AppStarting;
            this.Visibility = System.Windows.Visibility.Hidden;
        }


        /// <summary>
        /// Modalità media con innalzamento text box in cui inserisco il testo della nota
        /// </summary>
        public void ExpandFree(int h = -1, int w = -1)
        {
            Window win = Window.GetWindow(this.Parent);
            this.ExpandLayoutStep = EXPAND_STEP2_FREEE;
            win.Topmost = true;
            win.ShowInTaskbar = false;
            txtMsg.CaretPosition = txtMsg.CaretPosition.DocumentEnd;
            txtMsg.ScrollToEnd();

            if (h > 0) win.Height = h;
            if (w > 0) win.Width = w;

            if (win.Height < DIM_H + 200)
            {
                win.Height = DIM_H + 200;
                win.Width = DIM_W;
            }

            ResetPos();
            SetPosFormsFromMainForm();
                    
        }
      
        /// <summary>
        /// Modalità a pieno schermo
        /// </summary>
        public void ExpandMaximized()
        {
            Window win = Window.GetWindow(this.Parent);
            double screenWidth = System.Windows.SystemParameters.WorkArea.Width - 12;
            double screenHeight = System.Windows.SystemParameters.WorkArea.Height - 12;
            this.ExpandLayoutStep = EXPAND_STEP4_MAXIMIZED;
            win.Topmost = false;
            win.ShowInTaskbar = true;

            double wdtLeftPanel = screenWidth / 3 - 30;
            double hgtLeftPanel = screenHeight / 3;

            if (this.winFind != null)
            {
                this.winFind.Topmost = false;
                this.winFind.Left = 3;
                this.winFind.Top = 3;
                this.winFind.Height = hgtLeftPanel * 2 - 3;
                this.winFind.Width = wdtLeftPanel;
                if (this.winFind.MinWidth > wdtLeftPanel)
                    wdtLeftPanel = this.winFind.MinWidth;
                this.winFind.FillTitle(this.UserInfo.IDUser);
                this.winFind.ApplyFilterTitle(this.findParams.Title);
                this.winFind.IsHeaderExpanded = true;
                this.winFind.pnlWinBtns.Visibility = System.Windows.Visibility.Collapsed;
                //this.winFind.HEADER = "FIND";
                this.winFind.Show();
                imgFind.IsEnabled = false;
            }
            if (this.winFindTitle != null)
            {
                this.winFindTitle.Topmost = false;
                this.winFindTitle.Hide();
            }

            if (this.winSharingUsers != null)
            {
                this.winSharingUsers.Topmost = false;
                this.winSharingUsers.Hide();
            }
            if (this.winDocs == null) this.winDocs = new WinMBBDocs(this.UserInfo.IDUser);
            this.winDocs.FillDocs(this.currentNote.Docs);
            this.winDocs.Show();
            this.winDocs.ctrlMBBDocs.imgExit.Visibility = System.Windows.Visibility.Collapsed;
            this.winDocs.Topmost = false;
            this.winDocs.Left = 3;
            this.winDocs.Top = this.winFind.Top + this.winFind.Height + 3;
            this.winDocs.Height = screenHeight - this.winDocs.Top - 3;//screenHeight / 3;
            this.winDocs.Width = wdtLeftPanel;

            Panel.SetZIndex(win, 15);
            Panel.SetZIndex(txtTitle, 3);
            Panel.SetZIndex(pnlTxtMsg, 2);
            Panel.SetZIndex(pnlOkCanc, 1);
            //Panel.SetZIndex(pnlFunctions, 0);
            Panel.SetZIndex(this.winDocs, 0);

            pnlGridTextMsg.RowDefinitions[1].Height = new GridLength(0);
            //txtMsg.SelectionStart = txtMsg.Text.Length;
            txtMsg.CaretPosition = txtMsg.CaretPosition.DocumentEnd;
            txtMsg.ScrollToEnd();

            if (win != null)
            {// devo considerare anche il toolbar ai lati o in basso
                win.Height = screenHeight;
                win.Width = screenWidth - this.winFind.Width;
                win.Top = 3;
                win.Left = this.winFind.Width + 3;
            }
 
        }

        /// <summary>
        /// Massimizza le dimensioni sulla destra della schermo
        /// </summary>
        public void ExpandMaxHeight()
        {
            Window win = Window.GetWindow(this.Parent);
            double screenWidth = (System.Windows.SystemParameters.WorkArea.Width - 12);
            double screenHeight = System.Windows.SystemParameters.WorkArea.Height - 12;
            this.ExpandLayoutStep = EXPAND_STEP3_MAXHGT;

            //pnlGridTextMsg.RowDefinitions[1].Height = new GridLength(21);
            txtMsg.CaretPosition = txtMsg.CaretPosition.DocumentEnd;
            txtMsg.ScrollToEnd();

            if (win != null)
            {
                win.Height = screenHeight;
                win.Width = screenWidth / 4;
                win.Top = 3;
                win.Left = screenWidth - win.Width - 6;
            }
            SetPosFormsFromMainForm();
        }

        private bool ExpandedAuto
        {
            set
            {
                if (value)
                {
                    ExpandAuto();
                }
                else
                {
                    this.ExpandLayoutStep = EXPAND_FALSE;
                }
            }
            get { return this.ExpandLayoutStep == EXPAND_STEP1_AUTO; }
        }

        private void ExpandAuto()
        {
            Window win = Window.GetWindow(this.Parent);
            this.ExpandLayoutStep = EXPAND_STEP1_AUTO;
            double currentHgtHeader = pnlGridMain.RowDefinitions[0].ActualHeight;
            // top dell'immagine della scatola   
            double fontHeight1 = Math.Ceiling(txtMsg.FontSize + (double)txtMsg.FontFamily.LineSpacing * 2);
            double fontHeight2 = Math.Ceiling(txtTitle.FontSize + (double)txtTitle.FontFamily.LineSpacing * 2);

            double screenWidth = (System.Windows.SystemParameters.WorkArea.Width - 12);
            double screenHeight = System.Windows.SystemParameters.WorkArea.Height - 12;
            int DELTAHGT = 12;

            long numrowsText = GetLinesNote2() + 1;
            long numrowsTitle = txtTitle.LineCount;
            if (numrowsTitle == 0)
                numrowsTitle = 1;

            // 1) ridimensiono pnl title
            pnlTxtTitle.Height = numrowsTitle * fontHeight2 + pnlTxtTitle.Padding.Top + pnlTxtTitle.Padding.Bottom;
            if (pnlTxtTitle.Height < 55)
                pnlTxtTitle.Height = 55;

            // 2) mostro i pannelli opportuni

            // 3) ridimensiono la finestra principale
            if (win != null)
            {
                double newHgtHeader =
                            fontHeight1 * numrowsText + DELTAHGT +
                            toolBarRTF.Height + 21;
                if (newHgtHeader < DIM_HEADER)
                    newHgtHeader = DIM_HEADER;

                double h =
                    newHgtHeader +
                    pnlGridMain.RowDefinitions[1].Height.Value + 
                    pnlTxtTitle.Height + this.HEIGTH_FOOTER_FOR_REDIM_AUTO + 8;
                // il current top è la distanza dell'angolo in alto della parte centrale del controllo (l'immagine della scatola da chiuso)
                double currenttopBox = win.Top + currentHgtHeader;
        
                double w = win.Width;//DIM_W;

                if (h > (screenHeight / 4) * 3)
                {
                    w = win.Width;

                    // se l'altezza auto è > di tre quarti di schermo allora ridimensiona anche la larghezza fino ad un limite
                    while (w < 320 && h > ((screenHeight / 4) * 3))
                    {
                        newHgtHeader = 
                            fontHeight1 * numrowsText + DELTAHGT +
                            toolBarRTF.Height + 21;
                        if (newHgtHeader < DIM_HEADER) 
                            newHgtHeader = DIM_HEADER;

                        //pnlGridMain.RowDefinitions[0].Height = new GridLength(newHgtHeader, GridUnitType.Star);                            
                        h = newHgtHeader +
                            pnlGridMain.RowDefinitions[1].Height.Value +
                            pnlTxtTitle.Height + 8;
                        w = w + 25;

                        win.Height = h;
                        win.Width = w;
                        numrowsText = GetLinesNote2() + 1;

                    }
                    if (h > screenHeight - 3)
                        h = screenHeight - 3;
     
                }

                // tolgo anche HEIGTH_FOOTER_FOR_REDIM_AUTO - 6: non so per quale motivo ma funziona
                double wintop = win.Top + currentHgtHeader - newHgtHeader - this.HEIGTH_FOOTER_FOR_REDIM_AUTO - 6;//currenttopBox - newHgtHeader - 6;
                //double winleft = win.Left;

                win.Top = wintop;
                win.Height = h + this.HEIGTH_FOOTER_FOR_REDIM_AUTO;
                win.Width = w;
                if (win.Height < DIM_H_R)               
                    win.Height = DIM_H_R;                    
                
                if(win.Top + win.Height > screenHeight)
                    win.Top = screenHeight - win.Height - 6;

                ResetPos();
            }
        }

        public void ExpandFixed(int hgt, int wdt)
        {
            Window win = Window.GetWindow(this.Parent);
           
            win.Height = hgt;
            win.Width = wdt;

            ResetPos();           
        }


        private void ExpandAuto_old(bool val)
        {

            Window win = Window.GetWindow(this.Parent);

            if (val)
            {
                this.ExpandLayoutStep = EXPAND_STEP1_AUTO;

                //pnlGridMain.ColumnDefinitions[0].Width = new GridLength(DIM_BORDER);
                //pnlGridMain.ColumnDefinitions[2].Width = new GridLength(DIM_BORDER);

                pnlGridTextMsg.RowDefinitions[1].Height = new GridLength(21);

                double currentHheader = pnlGridMain.RowDefinitions[0].Height.Value;
                // top dell'immagine della scatola   
                double fontHeight1 = Math.Ceiling(txtMsg.FontSize * txtMsg.FontFamily.LineSpacing);
                double fontHeight2 = Math.Ceiling(txtTitle.FontSize * txtTitle.FontFamily.LineSpacing);

                double screenWidth = (System.Windows.SystemParameters.WorkArea.Width - 12);
                double screenHeight = System.Windows.SystemParameters.WorkArea.Height - 12;
                int DELTAHGT = 12;

                //txtMsg.CaretPosition = txtMsg.Document.ContentEnd;
                long numrows = GetLinesNote2();

                pnlGridMain.RowDefinitions[0].Height = new GridLength(numrows * //(double)txtMsg.LineCount * 
                    fontHeight1 + 21 + DELTAHGT
                    + toolBarRTF.Height
                    , GridUnitType.Star);
                //pnlGridMain.RowDefinitions[2].Height = new GridLength((double)txtTitle.LineCount * fontHeight2 + 8);
                pnlTxtTitle.Height = (double)txtTitle.LineCount * fontHeight2 + 8;

                if (pnlGridMain.RowDefinitions[0].Height.Value < DIM_HEADER)
                    pnlGridMain.RowDefinitions[0].Height = new GridLength(DIM_HEADER);

                if (pnlGridMain.RowDefinitions[0].Height.Value >
                    screenHeight - 20 - pnlGridMain.RowDefinitions[1].Height.Value +
                    //pnlGridMain.RowDefinitions[2].Height.Value)
                    pnlTxtTitle.Height)
                    pnlGridMain.RowDefinitions[0].Height = new GridLength(screenHeight - 20 - pnlGridMain.RowDefinitions[1].Height.Value +
                        //pnlGridMain.RowDefinitions[2].Height.Value);
                        pnlTxtTitle.Height);

                //if (pnlGridMain.RowDefinitions[2].Height.Value < DIM_FOOTER)
                //    pnlGridMain.RowDefinitions[2].Height = new GridLength(DIM_FOOTER);

                if (pnlTxtTitle.Height < DIM_FOOTER)
                    pnlTxtTitle.Height = DIM_FOOTER;

                pnlBtnsNewDelNote.Visibility = Visibility.Visible;
                pnlBtnsRecentNote.Visibility = Visibility.Visible;
                pnlBtnEmail.Visibility = Visibility.Visible;
                pnlPrevNext.Visibility = Visibility.Visible;
                pnlTopBox.Visibility = Visibility.Visible;

                if (win != null)
                {

                    double h = pnlGridMain.RowDefinitions[0].Height.Value + pnlGridMain.RowDefinitions[1].Height.Value +
                        // pnlGridMain.RowDefinitions[2].Height.Value + 4;
                        pnlTxtTitle.Height + 4;
                    // il current top è la distanza dell'angolo in alto della parte centrale del controllo (l'immagine della scatola da chiuso)
                    double currenttop = win.Top + currentHheader;
                    double w = DIM_W;

                    if (h > (screenHeight / 4) * 3)
                    {
                        w = win.Width;

                        bool isfirsttime = true;
                        // se l'altezza auto è > di tre quarti di schermo allora ridimensiona anche la larghezza fino ad un limite
                        while (w < 320 && h > ((screenHeight / 4) * 3))
                        {
                            // allargo la prima volta
                            if (isfirsttime)
                            {
                                /*pnlGridMain.ColumnDefinitions[0].Width = new GridLength(DIM_BORDER);
                                pnlGridMain.ColumnDefinitions[2].Width = new GridLength(DIM_BORDER);*/
                                isfirsttime = false;
                            }
                            double h0 = numrows * //(double)txtMsg.LineCount * 
                                fontHeight1 + 21 + DELTAHGT;
                            if (h0 < DIM_HEADER) h0 = DIM_HEADER;
                            pnlGridMain.RowDefinitions[0].Height = new GridLength(h0, GridUnitType.Star);

                            h0 = (double)txtTitle.LineCount * fontHeight2 + 8;
                            if (h0 < DIM_FOOTER) h0 = DIM_FOOTER;
                            //pnlGridMain.RowDefinitions[2].Height = new GridLength(h0);
                            pnlTxtTitle.Height = h0;

                            h = pnlGridMain.RowDefinitions[0].Height.Value + pnlGridMain.RowDefinitions[1].Height.Value +
                                //pnlGridMain.RowDefinitions[2].Height.Value + 4;
                                pnlTxtTitle.Height + 4;
                            w = w + 25;

                        }
                        if (h > screenHeight - 3)
                            h = screenHeight - 3;

                    }


                    //win.Top = currenttop - pnlGridMain.RowDefinitions[0].Height.Value;
                    double wintop = currenttop - pnlGridMain.RowDefinitions[0].Height.Value;
                    double winleft = win.Left;
                    // se va più inbasso del limite inferiore dello schermo
                    if (wintop + h > screenHeight)
                        wintop = screenHeight - h;

                    if (wintop <= 0) wintop = 3;

                    if (winleft + w > screenWidth)
                        winleft = screenWidth - w;

                    win.Top = wintop;
                    win.Left = winleft;
                    win.Height = h;
                    win.Width = w;

                    ResetPos();
                }
            }
            else
            {
                this.ExpandLayoutStep = EXPAND_FALSE;
            }

        }

        private void SetVisibilityPanelsOpenClose(bool isClosed)
        {
            if (isClosed)
            {
                imgExit.Visibility = Visibility.Visible;
                lblAutoExpand.Visibility = Visibility.Hidden;
                pnlBtnsNewDelNote.Visibility = Visibility.Hidden;
                pnlBtnsRecentNote.Visibility = Visibility.Hidden;
                pnlBtnEmail.Visibility = Visibility.Hidden;

                pnlPrevNext.Visibility = Visibility.Hidden;
                pnlDimMode.Visibility = Visibility.Visible;
                pnlTopBox.Visibility = Visibility.Hidden;

                pnlTxtMsg.Visibility = System.Windows.Visibility.Hidden;
                pnlTxtTitle.Visibility = System.Windows.Visibility.Hidden;
                pnlLeftFunctions.Visibility = System.Windows.Visibility.Hidden;
                pnlRigth.Visibility = System.Windows.Visibility.Hidden;

            }
            else
            {
                pnlTxtTitle.Height = DIM_FOOTER;                
                //pnlTxtMsg.Margin = new Thickness(-20, 2, -20, 2);
                pnlTxtMsg.Visibility = Visibility.Visible;
                pnlTxtTitle.Visibility = Visibility.Visible;
                pnlLeftFunctions.Visibility = Visibility.Visible;
                pnlRigth.Visibility = Visibility.Visible;
                pnlBtnsNewDelNote.Visibility = Visibility.Visible;
                pnlBtnsRecentNote.Visibility = Visibility.Visible;
                pnlBtnEmail.Visibility = Visibility.Visible;
                pnlPrevNext.Visibility = Visibility.Visible;
                pnlDimMode.Visibility = Visibility.Visible;
                pnlTopBox.Visibility = Visibility.Visible;

            }
        }

        private void SetPosFormsFromMainForm()
        {
            Window win = Window.GetWindow(this.Parent);

            win.Topmost = true;
            win.ShowInTaskbar = false;
            //pnlLeft.Margin = new Thickness(2);

            if (this.winFind != null)
            {
                this.winFind.Hide();
                this.winFind.Topmost = true;
                this.winFind.pnlWinBtns.Visibility = System.Windows.Visibility.Visible;
            }
            if (this.winFindTitle != null) this.winFindTitle.Hide();
            //if (this.winFavorites != null) this.winFavorites.Hide();
            if (this.winSharingUsers != null) this.winSharingUsers.Hide();
            if (this.winDocs != null)
            {
                this.winDocs.ctrlMBBDocs.imgExit.Visibility = System.Windows.Visibility.Visible;
                this.winDocs.Hide();
                this.winDocs.Topmost = true;
            }

            if (this.winFindTitle != null)
                this.winFindTitle.SetLayoutPos(win.Left, win.Width, win.Top, win.Height);

            //if (this.winFavorites != null)
            //    this.winFavorites.SetLayoutPos(win.Left, win.Width, win.Top, win.Height);

            if (this.winSharingUsers != null)
                this.winSharingUsers.SetLayoutPos(win.Left, win.Width, win.Top, win.Height,1);

            if (this.winSharingNotes != null)
                this.winSharingNotes.SetLayoutPos(win.Left, win.Width, win.Top, win.Height, 0);

            if (this.winDocs != null)
                this.winDocs.SetLayoutPos(win.Left, win.Width, win.Top, win.Height);

            imgFind.IsEnabled = true;
        }

        public void SetLayoutExpand(int val)
        {
            switch (val)
            {
                case EXPAND_FALLOW_CURSOR:
                    this.Expanded = false;
                    this.ExpandFollowCursor();
                    break;
                case EXPAND_FALSE:
                    this.Expanded = false;                   
                    break;
                case EXPAND_STEP1_AUTO: 
                    this.Expanded = true;
                    this.ExpandAuto();
                    break;
                case EXPAND_STEP2_FREEE:
                    this.Expanded = true;
                    int h = -1; 
                    if(this.Height!=double.NaN && this.Height>0) h = Convert.ToInt32(this.Height);
                    int w = -1;
                    if(this.Width!=double.NaN && this.Width>0) w = (int)this.Width;
                    this.ExpandFree(DIM_H, DIM_W);
                    break;
                case EXPAND_STEP3_MAXHGT:
                    this.Expanded = true;
                    this.ExpandMaxHeight();
                    break;
                case EXPAND_STEP4_MAXIMIZED:
                    this.Expanded = true;
                    this.ExpandMaximized();
                    break;
              
            }
        }

        private void SetPosCorner()
        {
            Window win = Window.GetWindow(this.Parent);
            double screenWidth = (System.Windows.SystemParameters.WorkArea.Width - 12);
            double screenHeight = System.Windows.SystemParameters.WorkArea.Height - 12;
            win.Top = screenHeight - win.Height - 6;
            win.Left = screenWidth - win.Width - 6;
        }

        private void ResetPos()
        {
            Window win = Window.GetWindow(this.Parent);
            double screenWidth = (System.Windows.SystemParameters.WorkArea.Width - 12);
            double screenHeight = System.Windows.SystemParameters.WorkArea.Height - 12;

            if (win.Height > screenHeight)
            {
                win.Height = screenHeight;
                win.Top = 3;
            }

            // se va più inbasso del limite inferiore dello schermo
            if (win.Top + win.Height > screenHeight)
                win.Top = screenHeight - win.Height;

            if (win.Top <= 0) win.Top = 3;

            if (win.Width> screenWidth)
            {
                win.Width = win.Width;
                win.Left= 3;
            }

            if (GetSecondaryScreen() == null)
            {
                if (win.Left + win.Width > screenWidth)
                    win.Left = screenWidth - win.Width;
            }
        }
        
        public void RestoreLayout(bool isMinimized)
        {
            Window win = Window.GetWindow(this.Parent);
            if (isMinimized)
            {
                if (this.winDocs != null) this.winDocs.Hide();
                if (this.winFind != null) this.winFind.Hide();

                this.Visibility = System.Windows.Visibility.Hidden;
                win.ShowInTaskbar = true;
            }
            else{
                this.Visibility = System.Windows.Visibility.Visible;
                //if (this.ExpandedMaximized)
                if (this.ExpandLayoutStep == EXPAND_STEP4_MAXIMIZED)
                {
                    this.winDocs.Show();
                    this.winFind.Show();
                }
                else win.ShowInTaskbar = false;
            }
        }

        private System.Windows.Forms.Screen GetSecondaryScreen()
        {
            System.Windows.Forms.Screen res = null;
            foreach (System.Windows.Forms.Screen screen in System.Windows.Forms.Screen.AllScreens)
            {
                if (screen != System.Windows.Forms.Screen.PrimaryScreen)
                    res = screen;
            }
            return res;//System.Windows.Forms.Screen.PrimaryScreen;
        }

        #endregion

        #region METHODS Get and SAVE
        public WinMBBFindTitle GetWinFindTitle()
        {
            return this.winFindTitle;
        }

        public WinMBBDocs GetWinDocs()
        {
            return this.winDocs;
        }

        public WinMBBSharingUsers GetWinShareUsers()
        {
            return this.winSharingUsers;
        }

        public WinMBBSharingNotes GetWinShareNotes()
        {
            return this.winSharingNotes;
        }

        public WinMBBFind GetWinFind()
        {
            return this.winFind;
        }

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
    
        /// <summary>
        /// NON USATA
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public List<DbNote> FindNotes(List<string> keys)
        {        
            List<DbNote> res = DbDataAccess.GetNotesOR(keys);
            return res;
        }

        private string FormatTitle(string title)
        {
            string res = title;
            List<string> lst = title.Split(MBBCommon.GetSeparationCharTitleParam(), StringSplitOptions.RemoveEmptyEntries).ToList();
            List<string> lstres = new List<string>();
            foreach (string s in lst)
                lstres.Add(s.TrimStart().TrimEnd());
            res = string.Join(MBBCommon.SeparationCharTitle, lstres);
            return res;
        }

        public bool SaveNote()
        {
            bool res = false;
            bool resWriteSyncro = true;
            bool resWriteSyncroSh = true;
            bool resWriteSyncroDel = true;
            bool resWriteSyncroDelSh = true;
            bool resDocs = true;
            string title = txtTitle.Text;
            //string text = txtMsg.Text;
            string text = GetTextNoteFormatted();
            string textcontent = GetTextNoteContent();

            title = FormatTitle(title);
            bool isToSave = (!string.IsNullOrEmpty(title) || !string.IsNullOrEmpty(textcontent));
            try
            {
                if (isToSave)
                {               
                    //-- salva nel db la nota
                    DateTime dtHeaderNote = DateTime.Now;
                    List<DbNote> lstNotesIns;
                    List<DbNote> lstNotesDel;
                    int rate = this.GetRate();                   

                    if (this.modeLayoutInsUpdFind == MBBConst.MODE_INS || this.modeLayoutInsUpdFind == MBBConst.MODE_UPD)
                    {
                        this.currentNote.ID = DbDataUpdate.InsertUpdateNoteWithSplit(
                            this.currentNote.ID
                            , title, text, this.UserInfo.IDUser, 0, rate, this.currentNote.IsFavorite
                            , this.currentNote.DateTimeInserted, DateTime.Now
                            , true
                            , this.currentNote.Docs, (this.currentNote.SharedUsers.Count > 0)
                            , this.UserInfo.Name, out lstNotesIns, out lstNotesDel);

                        // refresh della nota
                        this.currentNote = DbDataAccess.GetNoteAndDocs(this.UserInfo.IDUser, this.currentNote.ID, true); 
                        this.RefreshListNotes(this.currentNote, this.NotesFound, false);
                        this.RefreshListNotes(this.currentNote, this.notesRecent, true);

                        #region SCRITTURA SU FILE NOTE INSERITE
                        // per ogni nota inserita (>1 solo se splittata): ù
                        // DA 10/2019: CAMBIATA FILOSOFIA => ora su file di testo salvo sempre la nota completa (senza split)
                        // in questo modo quando su altro pc la ricarico da file di testo e uso la stessa convenzione di splittare la nota completa solo su db
                        /*foreach (DbNote n in lstNotesIns)
                        {
                            //-- insert su file della nota per sincronizzazione db
                            resWriteSyncro = MBBSyncroUtility.WriteNoteSyncroFile(n.ID, n.Title, n.Text, n.Rate, n.DateTimeInserted, dtHeaderNote, this.user, n.IDNoteParent, "", n.IsFavorite, MBBCommon.syncroParams);
                            //la nota è condivisa se gli utenti con cui condivido sono > 1 dato che il primo è l'utente stesso
                            if (this.currentNote.SharedUsers.Count > 0)
                                resWriteSyncroSh = MBBSyncroUtility.WriteNoteSyncroFileShared(n.ID, n.Title, n.Text, n.Rate, n.DateTimeInserted, dtHeaderNote, this.user, this.currentNote.SharedUsers, n.IDNoteParent, this.user, MBBCommon.syncroParams);
                        }*/
                        //-- insert su file della nota per sincronizzazione db
                        resWriteSyncro = MBBSyncroUtility.WriteNoteSyncroFile(this.currentNote.ID, title, text, this.currentNote.Rate, this.currentNote.DateTimeInserted, dtHeaderNote, this.UserInfo.Name, this.currentNote.IDNoteParent, this.currentNote.UserNameParent, this.currentNote.IsFavorite, MBBCommon.syncroParams);
                        //la nota è condivisa se gli utenti con cui condivido sono > 1 dato che il primo è l'utente stesso
                        if (this.currentNote.SharedUsers.Count > 0)
                            resWriteSyncroSh = MBBSyncroUtility.WriteNoteSyncroFileShared(this.currentNote.ID, title, text, this.currentNote.Rate, this.currentNote.DateTimeInserted, dtHeaderNote, this.UserInfo.Name, this.currentNote.SharedUsers, this.currentNote.IDNoteParent, this.UserInfo.Name, this.currentNote.IsFavorite, MBBCommon.syncroParams);

                        #endregion

                        #region SCRITTURA SU FILE NOTE ELIMINATE IN SEGUITO A REINSERIMENTO: se sono in update e devo splittare la nota
                        foreach (DbNote n in lstNotesDel)
                        {
                            resWriteSyncroDel = MBBSyncroUtility.WriteDelNoteSyncroFile(DateTime.Now, n.ID, this.UserInfo.Name, MBBCommon.syncroParams);

                            foreach (string dest in this.currentNote.SharedUsers)
                            {
                                resWriteSyncroDelSh = MBBSyncroUtility.WriteDelNoteSyncroFileShared(DateTime.Now, n.ID, this.UserInfo.Name, dest, MBBCommon.syncroParams);
                            }
                            // ELIMINA LA NOTA ANCHE DAL FILE DI TESTO ALTRIMENTI VIENE RECUPERATA dall'utente su un'altra macchina
                            MBBSyncroUtility.DeleteNoteFromUPDFile(n.ID, this.UserInfo.Name, MBBCommon.syncroParams);
                        }
                        #endregion

                        #region GESTIONE FILE DOCUMENTI
                        resDocs = ManageDocsSimple(dtHeaderNote, false);
                        #endregion

                        SetNotesToSendEmail(this.currentNote, 1);

                        if (this.CHAINED_ID_ACTIVITY > 0)
                        {
                            DbDataUpdate.InsertActivityNote(this.currentNote.ID, this.CHAINED_ID_ACTIVITY);
                        }
                    }
                }
                res = resWriteSyncro && resWriteSyncroSh && resWriteSyncroDel && resWriteSyncroDelSh && resDocs;
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("Error SaveNote: " + ex.Message, "");
                res = false;
            }

            return res;
        }

        private void SetNotesToSendEmail(DbNote currNote, int status_1Ins2Del)
        {
            if (this.NotesModToSend.Where(X => X.ID == currNote.ID).Count() > 0)
                this.NotesModToSend.RemoveAll(X => X.ID == currNote.ID);
            DbNoteStatus n = new DbNoteStatus();
            Utils.CopyObjectToObject(currNote, n);
            n.Status = status_1Ins2Del;
            n.Docs = new List<DbNoteDoc>();
            n.Docs.AddRange(currNote.Docs.ToList());
            this.NotesModToSend.Add(n);
        }

        /// <summary>
        /// Fa upload per ogni utrente condiviso duplicando il file (eventuale) per ogni utente
        /// SOSTITUITA IN 3/2018 DA ManageDocsSimple (metodo semplificato: salva files in dir dell'owner o in DOCS per gli utenti condivisi)
        /// </summary>
        /// <param name="dtHeaderNote"></param>
        /// <returns></returns>
        private bool ManageDocsDifferentSharedUsersOBSOLETE(DateTime dtHeaderNote)
        {
            bool resInsDB = true;
            bool resWrite = true;
            bool resWriteSh = true;
            //-- 1) per ogni documento in attesa di essere sincronizzato verifica l'ultima versione
            //-- 2) se esiste ultima versione controlla la data di ultima modifica rispetto a quella del file attuale
            //--    2a) se è più recente il file dell'ultima versione non fare niente e salva in note_doc il nome del doc 
            //--        con file name locale, con nome cartella della nota dell'ultima versione
            //--    2b) se è più recente il file in corso di upload, salva nella cartella della nota dell'ultima versione questo file e versiona
            //-- 3) se non esiste fai upload
            string intDirRemote = MBBSyncroUtility.MakeFolderNameUser(this.currentNote.ID, this.UserInfo.Name);

            int i = 0;
           
            if (this.currentNote.Docs != null && this.currentNote.Docs.Count > 0)
            {
                foreach (DbNoteDoc dd in this.currentNote.Docs)
                {
                    string sourceDir = System.IO.Path.GetDirectoryName(dd.FileNameLocal);
                    string fileName = System.IO.Path.GetFileName(dd.FileNameLocal);
                    DbNoteDoc docLastVer = DbDataAccess.GetDocInfo(dd.DocName, this.UserInfo.IDUser);
                    DateTime dtMod = dd.DateTimeLastMod;
                    //-- devo fare upload solo se il file attuale è più recente dell'ultima versione dello stesso file che è stata trovata
                    //-- se non è stato trovato un file con lo stesso nome, faccio upload nella cartella della nota attuale

                    dd.IDNote = this.currentNote.ID;

                    if (docLastVer != null && docLastVer.IDNote > 0)
                    {
                        dd.Version = docLastVer.Version + 1;
                        //-- salvo sempre nella cartella della nota dell'ultima versione, anche se il file è meno recente
                        intDirRemote = MBBSyncroUtility.MakeFolderNameUser(docLastVer.IDNote, docLastVer.UserNoteOwner);

                        //-- se esiste una versione del file meno recente faccio upload e versiono
                        //-- altrimenti, se la versione presente è più recente non faccio niente
                        //-- la sincronizzazione provvederà ad allinineare i files
                        if (dd.DateTimeLastMod > docLastVer.DateTimeLastMod)
                        {
                            string idnotefolder = MBBSyncroUtility.MakeFolderNameUser(docLastVer.IDNote, docLastVer.UserNoteOwner);
                            dd.Version = CRSFileUtil.UploadFile(MBBCommon.syncroParams.DropBoxRootDir, sourceDir, ref fileName, intDirRemote, true, true);
                        }
                        else dtMod = docLastVer.DateTimeLastMod;
                    }
                    else
                    {
                        intDirRemote = MBBSyncroUtility.MakeFolderNameUser(dd.IDNote, dd.UserNoteOwner);

                        if (dd.IsNewFile)
                            dd.Version = CRSFileUtil.UploadFile(MBBCommon.syncroParams.DropBoxRootDir, sourceDir, ref fileName, intDirRemote, true, true);
                        else dd.Version = CRSFileUtil.UploadFile(MBBCommon.syncroParams.DropBoxRootDir, sourceDir, ref fileName, intDirRemote, true);

                        dd.DocName = fileName;
                    }

                    resInsDB = DbDataUpdate.InsertDoc(this.currentNote.ID, dd.DocName, dd.Version, dtMod, intDirRemote, dd.FileNameLocal, this.UserInfo.IDUser, true);

                    string remDir = MBBSyncroUtility.GetUserRemoteDir(this.UserInfo.Name, true, MBBCommon.syncroParams);
                    if (this.currentNote.SharedUsers.Count() > 0)
                    {
                        intDirRemote = this.currentNote.ID.ToString().PadLeft(CRSGlobalParams.MAX_NUMDEC_NOTES, '0');
                        CRSFileUtil.UploadFile(remDir, sourceDir, ref fileName, intDirRemote, true);
                    }
                    //----------------------------------------------------------------------------
                    i++;
                }

                //----------------------------------------------------------------------------
                //-- NELL'ATTESA DI IMPLEMENTARE UN MODO PER CONDIVIDERE I FILES SENZA DROPBOX
                //-- copio il file condiviso per ogni utente con cui condivido la nota nella cartella destinata all'utente
                //-- IDEA: potrei evitare di copiare nella cartella SHARED_utentecondiviso il doc da condividere se
                //-- l'owner che condivide scrive direttamente nella sua cartella SHARED: posso andare a pescare il file da qui
                //-- direttamente
                //-- INCONVENIENTE:  un documento condiviso è accessibile da tutti gli utenti che possono vedere su cartella 
                //-- shared_ di owner, anche se quel doc non è condiviso per quell'utente
                //-- NO!! (vedi In REALTA..): SOLUZIONE SCELTA: copio più volte il file (risorse inutili consumate) per permettere una maggiore protezione dei dati
                //-- IN REALTA' comunque se in drop box condivido qualcosa con un utente potro vedere nella sua cartella condivisa
                //-- anche i documenti non condivisi con me ma con un'altro utente

                resWrite = MBBSyncroUtility.WriteDocsSyncroFile( this.currentNote.ID, dtHeaderNote, this.UserInfo.Name, this.currentNote.SharedUsers, this.currentNote.Docs, MBBCommon.syncroParams);
                intDirRemote = MBBSyncroUtility.MakeFolderNameForDocs(this.UserInfo.Name, true, -1);
                if (this.currentNote.SharedUsers.Count > 0)
                // mando il file ad ogni utente (nella sua cartella
                {
                    resWriteSh = MBBSyncroUtility.WriteDocsSyncroFileShared(this.currentNote.ID, dtHeaderNote, this.UserInfo.Name, this.currentNote.Docs, this.currentNote.SharedUsers, MBBCommon.syncroParams);
                }
            }

            return resInsDB && resWrite && resWriteSh;
        }
        /// <summary>
        /// Fa upload dei files mettendoli in directory dell'utente se la nota non è condivisda, o in dir condivisa \\DOCS
        /// </summary>
        /// <param name="dtHeaderNote"></param>
        /// <returns></returns>
        private bool ManageDocsSimple(DateTime dtHeaderNote, bool insertInDB)
        {
            bool resInsDB = true;
            bool resWrite = true;
            bool resWriteSh = true;
            int i = 0;
            
            if (this.currentNote.Docs != null && this.currentNote.Docs.Count > 0)
            {
                bool isNoteShared = (this.currentNote.SharedUsers != null && this.currentNote.SharedUsers.Count > 0);

                string intDirRemote = MBBSyncroUtility.MakeFolderNameForDocs(this.UserInfo.Name, isNoteShared, -1);
                bool isOkDel = true;
                if (insertInDB)
                    isOkDel = DbDataUpdate.DeleteDocsNote(this.currentNote.ID, this.UserInfo.IDUser);
                
                foreach (DbNoteDoc docnote in this.currentNote.Docs)
                {
                    string sourceDir = System.IO.Path.GetDirectoryName(docnote.FileNameLocal);
                    string fileName = System.IO.Path.GetFileName(docnote.FileNameLocal);
                    //DbNoteDoc docLastVer = DbDataAccess.GetDocInfo(docnote.DocName, this.UserInfo.IDUser);
                    DateTime dtMod = docnote.DateTimeLastMod;

                    //-- da 03/2018: LOGICA
                    //  1) in directory di dropbox copio il file
                    //      - se NON è condivisdo nella dir dell'utente
                    //      - se è condiviso in una dir generica (DOCS)
                    //  2) quando l'utente apre il file salvato con la nota modifica il file di lavoro copiato in dropbox
                    //      - in questo modo il file originale rimane intonso

                    //intDirRemote = MBBSyncroUtility.MakeFolderNameForDocs( this.UserInfo.Name, isNoteShared, -1);
                    docnote.IDNote = this.currentNote.ID;

                    // UPLOAD FISICO DEL FILE SU DIRECTORY DROPBOX
                    //MBBSyncroPending doc = MBBSyncroUtility.SyncroFileUpload(docnote.FileNameLocal, docnote.IDNote, docLastVer, this.UserInfo.IDUser, MBBCommon.syncroParams, intDirRemote, );

                    int ver = CRSFileUtil.UploadFile(MBBCommon.syncroParams.DropBoxRootDir, sourceDir, ref fileName, intDirRemote, true);

                    //this.docsPending.Add(doc);
                    // INSERIMENTO RECORD SU DB IN NOTE_DOC
                    if (insertInDB) 
                        resInsDB = DbDataUpdate.InsertDoc(this.currentNote.ID, fileName, docnote.Version, dtMod, intDirRemote, docnote.FileNameLocal, this.UserInfo.IDUser, true);
                    else DbDataUpdate.UpdateDocInfo(this.currentNote.ID, docnote.DocName, ver,"" , fileName, this.currentNote.IDUser, dtMod, "");
                    
                    docnote.DocName = fileName;

                    /*
                    // SCRITTURA RIGA SU FILE DI TESTO
                    resWrite = MBBSyncroUtility.WriteDocSyncroFile(dtHeaderNote, this.currentNote.ID, i, docnote.FileNameLocal, intDirRemote, docnote.Version, this.UserInfo.Name, MBBCommon.syncroParams);
                    // SCRITTURA RIGA SU FILE DI TESTO DI OGNI UTENTE CONDIVISO                 
                    if (this.currentNote.SharedUsers.Count > 0)
                    {
                        string intDirRemote = MBBSyncroUtility.MakeFolderNameForDocs("", true, -1);
                        // scrivo su ogni file degli utenti condivisi (il file è già stato uploadato in DOCS)
                        resWriteSh = MBBSyncroUtility.WriteDocSyncroFileShared(dtHeaderNote, this.currentNote.ID, i, docnote.FileNameLocal, intDirRemote, docnote.Version, this.UserInfo.Name, this.currentNote.SharedUsers, MBBCommon.syncroParams);
                    }*/

                    i++;
                }
                //----------------------------------------------------------------------------
                //-- NELL'ATTESA DI IMPLEMENTARE UN MODO PER CONDIVIDERE I FILES SENZA DROPBOX
                //-- copio il file condiviso per ogni utente con cui condivido la nota nella cartella destinata all'utente
                //-- IDEA: potrei evitare di copiare nella cartella SHARED_utentecondiviso il doc da condividere se
                //-- l'owner che condivide scrive direttamente nella sua cartella SHARED: posso andare a pescare il file da qui
                //-- direttamente
                //-- INCONVENIENTE:  un documento condiviso è accessibile da tutti gli utenti che possono vedere su cartella 
                //-- shared_ di owner, anche se quel doc non è condiviso per quell'utente
                //-- NO!! (vedi In REALTA..): SOLUZIONE SCELTA: copio più volte il file (risorse inutili consumate) per permettere una maggiore protezione dei dati
                //-- IN REALTA' comunque se in drop box condivido qualcosa con un utente potro vedere nella sua cartella condivisa
                //-- anche i documenti non condivisi con me ma con un'altro utente

                // SCRITTURA RIGA SU FILE DI TESTO
                resWrite = MBBSyncroUtility.WriteDocsSyncroFile(this.currentNote.ID, dtHeaderNote, this.UserInfo.Name, this.currentNote.SharedUsers, this.currentNote.Docs, MBBCommon.syncroParams);
                // SCRITTURA RIGA SU FILE DI TESTO DI OGNI UTENTE CONDIVISO                 
                if (isNoteShared)
                {
                    intDirRemote = MBBSyncroUtility.MakeFolderNameForDocs("", true, -1);
                    // scrivo su ogni file degli utenti condivisi (il file è già stato uploadato in DOCS)
                    resWriteSh = MBBSyncroUtility.WriteDocsSyncroFileShared(this.currentNote.ID, dtHeaderNote, this.UserInfo.Name, this.currentNote.Docs, this.currentNote.SharedUsers, MBBCommon.syncroParams);
                }
                //----------------------------------------------------------------------------
            }

            return resInsDB && resWrite && resWriteSh;
        }

        /// <summary>
        /// Salva una nota piu' lunga di 3000 caratteri in un file di testo che viene poi associato alla nota corrente
        /// </summary>
        /// <param name="note"></param>
        /// <param name="txt"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public bool SaveLongNoteInFile(DbNote note, string txt, string title)
        {
            bool res = false;
            try
            {
                string ext = MBBCommon.syncroParams.NewFileExtension;
                string filename = txtTitle.Text;
                string intDirRemote = System.IO.Path.Combine(MBBCommon.syncroParams.LocalTempDir, MBBSyncroUtility.MakeFolderNameUser(note.ID, this.UserInfo.Name));
               
                string fileName = note.ID.ToString()+"_"+ title.Substring(0,10);
                fileName = fileName.Replace(' ', '_');
                string filePath = System.IO.Path.Combine(intDirRemote, fileName);

                filePath = CRSFileUtil.NewFile(intDirRemote, fileName, "txt", txt);
                
                DbNoteDoc doc = new DbNoteDoc();

                doc.DateTimeLastMod = DateTime.Now;
                doc.DocName = fileName;
                doc.FileNameLocal = filePath;
                //-- verrà inizializzata successivamente
                doc.IDNote = note.ID;
                doc.IDUser = this.UserInfo.IDUser;
                doc.UserMod = this.UserInfo.Name;
                doc.UserNoteOwner = this.UserInfo.Name;
                doc.IsNewFile = false;

                this.CurrentNote.Docs.Add(doc);                       
            }
            catch { }
            return res;
        }

        private bool IsNullNote()
        {
            bool res = false;
            string title = txtTitle.Text;
            //string text = txtMsg.Text;
            string text = this.GetTextNoteContent();
            if (string.IsNullOrEmpty(text) && 
                (string.IsNullOrEmpty(title)) && 
                this.CurrentNote.Docs.Count==0)
                res = true;

            return res;
        }
        #endregion

        #region CALLBACK
        private void EventSelectTitle(string val)
        {
            txtTitle.Text = val;
            txtMsg.Focus();
        }
        #endregion

        #region SYNCRONIZING
        /// <summary>
        /// Siuncronizza tutti documenti dell'utente corrente. Scatta su timer.
        /// OBSOLETO: da sostituire con metodo di sincronizzazione puntuale => solo a richiesta
        /// </summary>
        /// <returns></returns>
        private bool SyncroFilesOBSOLETE()
        {
            //-- controlla tutti i file che appartengono all'owner (iduser corrente)
            //-- controlla i file condivisi          
            try
            {
                bool res1 = true;
                string filenameremote = "";
                List<DbNoteDoc> docs = DbDataAccess.GetDocsUser(this.UserInfo.IDUser);
                if (docs != null && docs.Count > 0)
                {
                    foreach (DbNoteDoc d in docs)
                    {
                        filenameremote = MBBCommon.syncroParams.SharingDir + "\\" + d.InternalDirRemote + "\\" + d.DocName;
                        MBBSyncroPending docpending =  MBBSyncroUtility.SyncroFileOBSOLETE(d.FileNameLocal, filenameremote, d.IDNote, this.UserInfo.IDUser, this.UserInfo.Name, MBBCommon.syncroParams);

                        if (docpending != null)
                            this.docsPendingSyncro.Add(docpending);
                        else res1 = false;
                        //break;
                    }
                }

                return res1;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region DROP files
        public void DropEvn(DragEventArgs e)
        {
            if (this.Expanded == false)
            {
                this.Expanded = true;
                this.ModeLayoutInsUpdFindClose = MBBConst.MODE_INS;
            }
            else {
                if (this.ModeLayoutInsUpdFindClose == MBBConst.MODE_FIND)
                    this.ModeLayoutInsUpdFindClose = MBBConst.MODE_UPD;
            }
            
            //-- salva la lista dei documenti in attesa di scrittura su db, che avviene alla pressione del tasto 
            //-- di conferma           
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            files = files.Distinct().ToArray();

            List<string> filepaths = new List<string>();

            foreach (string file in files)
            {
                if (Directory.Exists(file))
                {
                    //Add files from folder
                    filepaths.AddRange(Directory.GetFiles(file));
                }
                else
                {
                    //Add filepath
                    filepaths.Add(file);
                }
            }

            AddNewDocs(filepaths);
                   
        }

        private void AddNewDocs(List<string> filepaths)
        {
            foreach (string file in filepaths)
            {
                bool found = false;
                foreach (DbNoteDoc d in this.CurrentNote.Docs)
                {
                    if (d.FileNameLocal == file)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    string fileName = System.IO.Path.GetFileName(file);
                    DbNoteDoc doc = new DbNoteDoc();

                    doc.DateTimeLastMod = System.IO.File.GetLastWriteTime(file);
                    doc.DocName = fileName;
                    doc.FileNameLocal = file;
                    //-- verrà inizializzata successivamente
                    doc.IDNote = this.currentNote.ID;
                    doc.IDUser = this.UserInfo.IDUser;
                    doc.UserMod = this.UserInfo.Name;
                    //doc.IDUserNoteOwner = this.idUser;
                    doc.UserNoteOwner = this.UserInfo.Name;
                    doc.IsNewFile = false;

                    this.CurrentNote.Docs.Add(doc);
                }
            }
            if (this.winDocs != null)
                this.winDocs.FillDocs(this.CurrentNote.Docs);
        }

        private void UserControl_Drop(object sender, DragEventArgs e)
        {
            DropEvn(e);
        }
        #endregion

        #region manage RATE - SHARE
        private void SetFavorite(bool isFav)
        {
            if (!isFav)
                imgAddFavorites.Source = CRSImage.CreateImage(CRSGlobalParams.IconRateGrayPathName);
            else imgAddFavorites.Source = CRSImage.CreateImage(CRSGlobalParams.IconRateBluePathName);
        }

        private void SetRate(int rate)
        {
            string star = CRSGlobalParams.IconRateYellowPathName;

            try
            {
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
            catch { }
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

        private void SetShare(List<string> users)
        {
            try
            {
                if (UserInfo.Name != null && users.Count > 0)
                {
                    imgShare.Source = CRSImage.CreateImage(CRSGlobalParams.IconSharedPathName);
                    imgSyncShared.Visibility = Visibility.Visible;
                }
                else
                {
                    imgShare.Source = CRSImage.CreateImage(CRSGlobalParams.IconNotSharedPathName);
                    imgSyncShared.Visibility = Visibility.Hidden;
                }
            }
            catch { }
        }

        private void SetImageEmail(int mode_0normal_1tosend_2address)
        {
            if (mode_0normal_1tosend_2address == 0)
                imgSendEmail.Source = CRSImage.CreateImage(CRSGlobalParams.IconEmailNormalBlue);
            else
            if (mode_0normal_1tosend_2address == 1)
                imgSendEmail.Source = CRSImage.CreateImage(CRSGlobalParams.IconEmailToSend);
            else
            if (mode_0normal_1tosend_2address == 2)
                imgSendEmail.Source = CRSImage.CreateImage(CRSGlobalParams.IconEmailAddress);
        }
        #endregion

        #region EVENTS user control
        private void UserControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!this.Expanded)
            {
                this.Expanded = true;
                this.ExpandAuto();
                this.ModeLayoutInsUpdFindClose = MBBConst.MODE_FIND;
                GotoLastNote();
            }
        }

        private void UserControl_DragEnter(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Copy;
        }

        #endregion

        #region EVENTS btns panels SX-DX

        private void imgOptions_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //if (this.ExpandedMaximized == false)
            if(this.ExpandLayoutStep != EXPAND_STEP4_MAXIMIZED)
            {
                MinimizeAllForms();
            }

            //-- open window params
            WinMBBParams win = new WinMBBParams();
            win.FillSyncroPendings(this.docsPendingSyncro);
            //bool isOk = win.ShowDialog().Value;
            try
            {
                bool? isOk = win.ShowDialog();
                if (isOk.HasValue && isOk.Value)
                {
                    this.InitFromIniResUser();
                    // resetto i timers se sono cambiati
                    SetTimers();

                    if (win.IsUserChanged)
                    {
                        Window winmain = Window.GetWindow(this.Parent);
                        if (winmain != null)
                        {
                            System.Windows.Forms.Application.Restart();
                            Application.Current.Shutdown();
                        }
                    }
                }
                else
                {
                    //Window winmain = Window.GetWindow(this.Parent);
                    //if (winmain != null)
                    //{
                    //    System.Windows.Forms.Application.Restart();
                    //    Application.Current.Shutdown();
                    //}
                }
            }
            catch { }
        }

        private void imgOk_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (IsNullNote() == false)
            {
                bool res = this.SaveNote();
                if (res)
                {                 
                    this.findParams.Title = this.GetNewFilter(this.findParams.Title, txtTitle.Text);

                    if (this.winFind != null)
                    {
                        this.winFind.SetFilterTitle(this.findParams.Title);
                    }

                    this.ModeLayoutInsUpdFindClose = MBBConst.MODE_FIND;
                    bool v1 = ManageWinDoc();
                    bool v2 = ManageWinShareUser();
                }
            }
        }

        private string GetNewFilter(string paramsTitle, string newNoteTitle)
        {   
            string newParamsTitle = "";
            newParamsTitle = paramsTitle;

            if (this.findParams.TypeFindAtLeastOne)
            {
                List<string> lst = newNoteTitle.Split( MBBCommon.GetSeparationCharTitleParam(), StringSplitOptions.RemoveEmptyEntries).ToList();
                foreach (string s in lst)
                {
                    // se non c'è la aggiungo ed esco in quanto almeno una parola del nuovo titolo c'è e sarà trovata 
                    if (paramsTitle.Split(MBBCommon.GetSeparationCharTitleParam(), StringSplitOptions.RemoveEmptyEntries).Where(X => X.Trim() == s.Trim()).Count() == 0)
                    {
                        newParamsTitle = newParamsTitle + " " + s;
                        break;
                    }
                }
            }
            else
            {
                // se devo cercare tutte le parole e tutte le parole del filtro attuale non battono devo elimare dal filtro
                // le parole che non battono
                //foreach (string s in paramsTitle.Split(' '))
                foreach (string s in paramsTitle.Split(MBBCommon.GetSeparationCharTitleParam(), StringSplitOptions.RemoveEmptyEntries))
                {
                    // 
                    if (newNoteTitle.Split(MBBCommon.GetSeparationCharTitleParam(), StringSplitOptions.RemoveEmptyEntries).Where(X => X.Trim().Contains(s.Trim())).Count() == 0)
                    {
                        newParamsTitle = newParamsTitle.Remove(newParamsTitle.IndexOf(s), s.Length);
                    }
                }  
            }
            newParamsTitle = newParamsTitle.Trim();
            //this.findParams.Title = newParamsTitle;
            return newParamsTitle;
        }

        private void imgCanc_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if ((this.modeLayoutInsUpdFind == MBBConst.MODE_INS && this.IsNullNote() == false) || this.modeLayoutInsUpdFind == MBBConst.MODE_UPD)
            {
                MessageBoxResult br = System.Windows.MessageBox.Show(
                                                CRSTranslation.TranslateDialog(CRSTranslation.MSG_DATA_NOT_SAVED, this.UserInfo.Language) +"\n" + 
                                                CRSTranslation.TranslateDialog(CRSTranslation.MSG_DO_YOU_WANT_CONTINUE, this.UserInfo.Language),
                                                CRSTranslation.TranslateDialog(CRSTranslation.MSG_WARNING, this.UserInfo.Language),
                                                MessageBoxButton.YesNo);
                if (br == MessageBoxResult.Yes)
                {                   
                    this.CurrentNote = this.currentNote;
                    this.ModeLayoutInsUpdFindClose = MBBConst.MODE_FIND;                  
                }
            }          
        }
       
        private void imgFind_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //-- open window params
            if (this.winFind == null)
                this.winFind = new WinMBBFind(this.UserInfo.IDUser);

            this.winFind.IsHeaderExpanded = true;
            this.winFind.ctrlMBBDocs.IsBtnExitVisible = false;
            this.winFind.FillTitle(this.UserInfo.IDUser);
            this.winFind.IsReduced = false;
            this.winFind.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.winFind.HEADER = "       FIND";
            this.winFind.Show();
        }

        private void imgRat1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SetRateFromImg(sender as Image);
            if (this.modeLayoutInsUpdFind == MBBConst.MODE_FIND)
                this.ModeLayoutInsUpdFindClose = MBBConst.MODE_UPD;
        }

        private void imgAddFavorites_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //this.IsFavorite = !this.currentNote.IsFavorite;
            bool isFav = !this.currentNote.IsFavorite;
            this.CurrentNote.IsFavorite = isFav;
            SetFavorite(isFav);
            // salvo subito i dati dato che setto la proprietà favorite
            if(this.ModeLayoutInsUpdFindClose != MBBConst.MODE_INS) 
                this.ModeLayoutInsUpdFindClose = MBBConst.MODE_UPD;
            bool res = this.SaveNote();
            if (isFav)
                ShowWinFindFavorites();   

            if (res)
            {
                int idx = this.NotesFound.IndexOf(this.currentNote);
                if (idx >= 0) this.NotesFound.ElementAt(idx).IsFavorite = isFav;
            }          

            this.ModeLayoutInsUpdFindClose = MBBConst.MODE_FIND;
        }

        private void imgFavorites_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //-- open window params
            ShowWinFindFavorites();
        }

        public int SetRecentNotesToIni()
        {
            CRSIni iniFile = new CRSIni();
            iniFile.Name = CRSIniFile.IniPathName;
            if (iniFile.HasSection("RECENTNOTES") == false)
            {
                iniFile.SetSection("RECENTNOTES");
            }

            List<string> entries = iniFile.GetEntryNames("RECENTNOTES");
            entries.AddRange(this.notesRecent.Select(X => "NOTE" + X.ID.ToString()));
            int num = entries.Count;// + this.recentNotes.Count;

            try
            {
                // elimina le prime 
                int idx = 0;
                int entriestodel = num - 30;
                if (num > 30)
                {
                    foreach (string s in entries)
                    {
                        //iniFile.RenameEntry("RECENTNOTES", s, "NOTE" + idx.ToString() );
                        iniFile.RemoveEntry("RECENTNOTES", s);
                        idx++;
                        if (idx >= entriestodel)
                            break;
                    }
                    // non serve è troppo laborioso: il valore dell'id lo metto nel nome
                    /*
                    entries = iniFile.GetEntryNames("RECENTNOTES");
                    idx = 1;
                    foreach (string s in entries)
                    {
                        iniFile.RenameEntry("RECENTNOTES", s, "NOTE" + idx.ToString() );
                        idx++;
                    }*/
                }
                if (entriestodel < 0) entriestodel=0;
                // se non esiste la crea
                for (int i = entriestodel; i < entries.Count; i++)
                {
                    iniFile.SetValue("RECENTNOTES", entries[i], entries[i].Substring(4));

                }
            }
            catch(Exception e)
            {
                MessageBox.Show("Error: " + e.Message);
            }
            return Convert.ToInt32(num);
        }

        private void SetModeUpdateAndSharedUsers()
        {
            if (this.ModeLayoutInsUpdFindClose != MBBConst.MODE_INS && this.ModeLayoutInsUpdFindClose != MBBConst.MODE_UPD)
                this.ModeLayoutInsUpdFindClose = MBBConst.MODE_UPD;

            if (this.winSharingUsers != null)
                this.CurrentNote.SharedUsers = this.winSharingUsers.ctrlMBBShare.GetSharing();
        }

        public int FillRecentNotesFromIni()
        {
            CRSIni iniFile = new CRSIni();
            iniFile.Name = CRSIniFile.IniPathName;

            List<string> entries = iniFile.GetEntryNames("RECENTNOTES");
            this.notesRecent.Clear();

            try { 
                foreach (string s in entries)
                {
                    //AddNoteToRecent(DbDataAccess.GetNoteAndDocs(this.UserInfo.IDUser, Convert.ToInt64(s.Substring(4)), true));
                    RefreshListNotes(DbDataAccess.GetNoteAndDocs(this.UserInfo.IDUser, Convert.ToInt64(s.Substring(4)), true), this.notesRecent, false);
                }
            }
            catch { }
            return this.notesRecent.Count;
        }

        private void RefreshListNotes(DbNote note, List<DbNote> notes, bool addtoend)
        {
            if (notes!=null && notes.Count()>0 && notes.Where(X => X.ID == note.ID && X.IDUser == note.IDUser).Count() > 0)
            {
                DbNote n = notes.Where(X => X.ID == note.ID && X.IDUser == note.IDUser).First();
                Utils.CopyObjectToObject(note, n);
                n.Docs = new List<DbNoteDoc>();
                if (note.Docs != null && note.Docs.Count() > 0)
                {
                    foreach (DbNoteDoc dnote in note.Docs)
                    {
                        DbNoteDoc d = new DbNoteDoc();
                        Utils.CopyObjectToObject(dnote, d);
                        n.Docs.Add(d);
                    }
                }
                n.SharedUsers = new List<string>();
                if (note.SharedUsers != null && note.SharedUsers.Count() > 0)
                {
                    foreach (string snote in note.SharedUsers)
                    {
                        n.SharedUsers.Add(snote);
                    }
                }

                if (addtoend)
                {
                    int nn = notes.RemoveAll(X => X.ID == note.ID && X.IDUser == note.IDUser);
                    notes.Add(n);
                }
            }
            // se non è nella lista è una nuova nota da aggiunere
            else
            {
                if (notes==null) notes = new List<DbNote>();
                notes.Add(note);
            }
        }

        // NON USATA: c'è già RefreshListNotes
        private void AddNoteToRecent(DbNote note)
        {
            try
            {
                if (this.notesRecent != null && this.notesRecent.Count > 0)
                {
                    if (this.notesRecent.Where(X => X.ID == note.ID).Count() == 0)
                        this.notesRecent.Add(note);
                }
                else
                {
                    this.notesRecent = new List<DbNote>();
                    this.notesRecent.Add(note);
                }
            }
            catch { }
        }

        private void ShowWinFindFavorites()
        {
            if (this.winFind == null)
                this.winFind = new WinMBBFind(this.UserInfo.IDUser);

            this.winFind.IsHeaderExpanded = false;
            this.winFind.FillFavoritesTitle(this.UserInfo.IDUser);

            MBBFindParams p = new MBBFindParams();
            p.IDUser = this.UserInfo.IDUser;
            p.Dt1 = DateTime.Now.AddYears(-15);
            p.Dt2 = DateTime.Now;
            p.Rate = 0;
            p.SimbolRate = ">=";
            p.OnlyFavorites = true;
            p.Title = "";
            this.winFind.FillNotes(p);
            this.winFind.FillDocs(p);

            this.winFind.IsReduced = false;
            this.winFind.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.winFind.HEADER = "       FAVORITES";
            this.winFind.Show();
        }

        private void imgSyncShared_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (this.winSharingNotes == null)
                    this.winSharingNotes = new WinMBBSharingNotes(this.currentNote);
                this.winSharingNotes.ctrlMBBSharingNotes.IsPermanentlyClosed = false;
                if (this.currentNote.SharedUsers.Count > 0)
                    this.winSharingNotes.Show();

                SyncroSharedTodayNotes();
               
            }
            catch { }
        }

        private void imgShare_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (this.ModeLayoutInsUpdFindClose == MBBConst.MODE_INS)
            {
                this.SaveNote();
                if (this.ModeLayoutInsUpdFindClose != MBBConst.MODE_FIND)
                    this.ModeLayoutInsUpdFindClose = MBBConst.MODE_FIND;
            }

            if (this.winSharingUsers == null)
                this.winSharingUsers = new WinMBBSharingUsers(this.UserInfo.IDUser, this.currentNote.ID, SetModeUpdateAndSharedUsers);
            else
                this.winSharingUsers.ctrlMBBShare.FillUsersSharing(this.currentNote.ID);

            //this.winDocs.Hide();
            ManagePosWin(false, true, true);
            //this.winSharingUsers.Show();
            this.CurrentNote = DbDataAccess.GetNoteAndDocs(this.UserInfo.IDUser, this.currentNote.ID, true);
            // lo fa già in this.CurrentNote
            //SetShare(this.currentNote.SharedUsers);
        }

        #endregion

        #region EVENTS btns PREV/NEXT/NEW

        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            bool bcont = true;
            if (MBBCommon.syncroParams.AutoSave == false && (this.ModeLayoutInsUpdFindClose == MBBConst.MODE_UPD || (this.ModeLayoutInsUpdFindClose == MBBConst.MODE_INS && IsNullNote() == false)))
            {
                MessageBoxResult r = MessageBox.Show(
                    CRSTranslation.TranslateDialog(CRSTranslation.MSG_DATA_NOT_SAVED,this.UserInfo.Language + "\n" +
                    CRSTranslation.TranslateDialog(CRSTranslation.MSG_DO_YOU_WANT_CONTINUE, this.UserInfo.Language)),
                    CRSTranslation.TranslateDialog(CRSTranslation.MSG_WARNING,this.UserInfo.Language), MessageBoxButton.YesNo);
                if (r == MessageBoxResult.No)
                {
                    bcont = false;
                }
            }

            if(bcont)
                GotoPrevNote();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            bool bcont = true;
            //-- non può essere che sto modificando 
            //-- se sto inserendo il tasto è disabilitato quindi il problema non si propone
            if (MBBCommon.syncroParams.AutoSave == false && (this.ModeLayoutInsUpdFindClose == MBBConst.MODE_UPD || (this.ModeLayoutInsUpdFindClose == MBBConst.MODE_INS && IsNullNote() == false)))
            {
                MessageBoxResult r = MessageBox.Show(
                    CRSTranslation.TranslateDialog(CRSTranslation.MSG_DATA_NOT_SAVED, this.UserInfo.Language) + "\n" +
                    CRSTranslation.TranslateDialog(CRSTranslation.MSG_DO_YOU_WANT_CONTINUE, this.UserInfo.Language),
                    CRSTranslation.TranslateDialog(CRSTranslation.MSG_WARNING, this.UserInfo.Language), MessageBoxButton.YesNo);
                if (r == MessageBoxResult.No)
                {
                    bcont = false;
                }
            }
            
            if(bcont)
                GotoNextNote();
        }
         
        private void GotoNextNote()
        {
            this.btnPrev.IsEnabled = true;
            
            int idx = this.NotesFound.IndexOf(this.currentNote);
            if (idx < 0)
            {
                if (this.currentNote != null && this.NotesFound.Where(X => X.ID == this.currentNote.ID && X.IDUser == this.currentNote.IDUser).Count() > 0)
                    idx = this.NotesFound.IndexOf(this.NotesFound.Where(X => X.ID == this.currentNote.ID && X.IDUser == this.currentNote.IDUser).First());
            }

            if (idx >= 0 && idx < this.NotesFound.Count - 1)
            {
                //-- è importante che ModeInsUpdFind sia assegnato dopo l'assegnazione a nodeCurrent altrimenti
                //-- al cambiamento di txtMsg o txtTitle con focus potrebbe cambiare lo stato a MODE_UPD
                if (MBBCommon.syncroParams.AutoSave && (this.ModeLayoutInsUpdFindClose == MBBConst.MODE_INS || this.ModeLayoutInsUpdFindClose == MBBConst.MODE_UPD))
                    SaveNote();

                this.CurrentNote = this.NotesFound.ElementAt(idx + 1);
                
                if (this.ModeLayoutInsUpdFindClose != MBBConst.MODE_FIND) 
                    this.ModeLayoutInsUpdFindClose = MBBConst.MODE_FIND;

                if (this.ExpandLayoutStep == EXPAND_STEP1_AUTO)
                    this.ExpandAuto();

                bool v1 = ManageWinDoc();
                bool v2 = ManageWinShareUser();
                bool v3 = false;
                if (this.winSharingNotes != null) v3 = this.winSharingNotes.Visibility == Visibility.Visible;
                ManagePosWin(v1, v2, v3);
            }
            if(idx == this.NotesFound.Count - 1)
            {   // modalità inseriemnto se vado oltre l'ultima nota
                this.ModeLayoutInsUpdFindClose = MBBConst.MODE_INS;
                this.btnNext.IsEnabled = false;
                if (this.winDocs != null && this.ExpandLayoutStep != EXPAND_STEP4_MAXIMIZED)
                    this.winDocs.Hide();
            }

            SetTextInfoNotes(this.modeLayoutInsUpdFind);
        }

        private void GotoPrevNote()
        {
            this.btnNext.IsEnabled = true;
            int idx = this.NotesFound.IndexOf(this.currentNote);
            if (idx < 0)
            {
                if(this.currentNote!=null && this.NotesFound.Where(X=>X.ID == this.currentNote.ID && X.IDUser==this.currentNote.IDUser).Count()>0)
                    idx = this.NotesFound.IndexOf(this.NotesFound.Where(X => X.ID == this.currentNote.ID && X.IDUser == this.currentNote.IDUser).First());
                else idx = this.NotesFound.Count;
            }
            if (idx > 0)
            {
                if (MBBCommon.syncroParams.AutoSave && (this.ModeLayoutInsUpdFindClose == MBBConst.MODE_INS || this.ModeLayoutInsUpdFindClose == MBBConst.MODE_UPD))
                    SaveNote();
                
                this.CurrentNote = this.NotesFound.ElementAt(idx - 1);
                
                if (this.ModeLayoutInsUpdFindClose != MBBConst.MODE_FIND) 
                    this.ModeLayoutInsUpdFindClose = MBBConst.MODE_FIND;

                if (this.ExpandLayoutStep == EXPAND_STEP1_AUTO)
                    this.ExpandAuto();

                bool v1 = ManageWinDoc();
                bool v2 = ManageWinShareUser();
                // gestisco la finestra solo se l'ìutente l'ha aperta
                bool v3 = false;
                if (this.winSharingNotes != null) 
                    v3 = this.winSharingNotes.Visibility == Visibility.Visible;

                ManagePosWin(v1, v2, v3);
            }
            if(idx==1)
                this.btnPrev.IsEnabled = false;

            SetTextInfoNotes(this.modeLayoutInsUpdFind);
        }

        private void GotoLastNote()
        {
            this.btnPrev.IsEnabled = true;
            this.btnNext.IsEnabled = false;
            
            if (this.winDocs != null && this.ExpandLayoutStep != EXPAND_STEP4_MAXIMIZED)
                this.winDocs.Hide();

            //-- è importante che ModeInsUpdFind sia assegnato dopo l'assegnazione a nodeCurrent altrimenti
            //-- al cambiamento di txtMsg o txtTitle con focus potrebbe cambiare lo stato a MODE_UPD
            if (MBBCommon.syncroParams.AutoSave && (this.ModeLayoutInsUpdFindClose == MBBConst.MODE_INS || this.ModeLayoutInsUpdFindClose == MBBConst.MODE_UPD))
                SaveNote();
            if (this.NotesFound.Count>0)
                this.CurrentNote = this.NotesFound.Last();

            if (this.ModeLayoutInsUpdFindClose != MBBConst.MODE_FIND) 
                this.ModeLayoutInsUpdFindClose = MBBConst.MODE_FIND;
            bool v1 = ManageWinDoc();
            bool v2 = ManageWinShareUser();
            bool v3 = false;
            if(this.winSharingNotes!=null) 
                v3 = this.winSharingNotes.Visibility == Visibility.Visible;

            if (this.ExpandLayoutStep == EXPAND_STEP1_AUTO)
                this.ExpandAuto();

            ManagePosWin(v1, v2, v3);
            SetTextInfoNotes(this.modeLayoutInsUpdFind);   
        }

        private void GotoFirstNote()
        {
            this.btnNext.IsEnabled = true;
            this.btnPrev.IsEnabled = false;

            if (this.winDocs != null && this.ExpandLayoutStep != EXPAND_STEP4_MAXIMIZED)
                this.winDocs.Hide();

            //-- è importante che ModeInsUpdFind sia assegnato dopo l'assegnazione a nodeCurrent altrimenti
            //-- al cambiamento di txtMsg o txtTitle con focus potrebbe cambiare lo stato a MODE_UPD
            if (MBBCommon.syncroParams.AutoSave && (this.ModeLayoutInsUpdFindClose == MBBConst.MODE_INS || this.ModeLayoutInsUpdFindClose == MBBConst.MODE_UPD))
                SaveNote();
            if (this.NotesFound.Count > 0)
                this.CurrentNote = this.NotesFound.First();

            if (this.ModeLayoutInsUpdFindClose != MBBConst.MODE_FIND)
                this.ModeLayoutInsUpdFindClose = MBBConst.MODE_FIND;
            bool v1 = ManageWinDoc();
            bool v2 = ManageWinShareUser();
            bool v3 = false;
            if (this.winSharingNotes != null) 
                v3 = this.winSharingNotes.Visibility == Visibility.Visible;

            if (this.ExpandLayoutStep == EXPAND_STEP1_AUTO)
                this.ExpandAuto();

            ManagePosWin(v1, v2, v3);
            SetTextInfoNotes(this.modeLayoutInsUpdFind);       
        }

        private bool ManageWinDoc()
        {
            bool isVisible = false;
            int numdocs = this.CurrentNote.Docs.Count;
            if (this.winDocs == null)
            {
                this.winDocs = new WinMBBDocs(this.UserInfo.IDUser);             
            }
            
            if (numdocs > 0)
            {
                numdocs = this.winDocs.FillDocs(this.CurrentNote.Docs);
                this.winDocs.Show();
                isVisible = true;
            }
            else
            {
                if(this.ExpandLayoutStep != EXPAND_STEP4_MAXIMIZED && this.winDocs!= null)
                    this.winDocs.Hide();
            }
            return isVisible;
        }

        private bool ManageWinShareUser()
        {
            bool isVisible = false;
            int numsh = 0;
            if (this.CurrentNote!=null) numsh = this.CurrentNote.SharedUsers.Count();
            if (this.winSharingUsers == null)
                this.winSharingUsers = new WinMBBSharingUsers(this.UserInfo.IDUser, this.currentNote.ID, SetModeUpdateAndSharedUsers);
            else
                this.winSharingUsers.ctrlMBBShare.FillUsersSharing(this.CurrentNote.SharedUsers,this.currentNote.ID);

            this.winSharingUsers.ctrlMBBShare.ShowModeInsert = false;

            if (numsh > 0)
            {
                this.winSharingUsers.Show();
                isVisible = true;
            }
            else
            {
                if (this.ExpandLayoutStep != EXPAND_STEP4_MAXIMIZED && this.winSharingUsers!= null)
                    this.winSharingUsers.Hide();
            }
            return isVisible;
        }

        public void ManagePosWin(bool windocVisible, bool winshUsrVisible, bool winshNotesVisible)
        {
            Window win = Window.GetWindow(this.Parent);
            double l = win.Left;
            double t = win.Top;
            double w = win.Width;
            double h = win.Height;
            if (this.winDocs != null && this.winDocs.IsActive) 
            {
                if (windocVisible) this.winDocs.Show();
                else this.winDocs.Hide();
            }
            if (this.winSharingUsers != null)
            {
                if (winshUsrVisible) this.winSharingUsers.Show();
                else this.winSharingUsers.Hide();
            }
            if (this.winSharingNotes != null)
            {
                if (winshNotesVisible) this.winSharingNotes.Show();
                else this.winSharingNotes.Hide();
            }
            if (this.ExpandLayoutStep != EXPAND_STEP4_MAXIMIZED)
            {
                if (this.winDocs != null && windocVisible && this.winSharingUsers != null && winshUsrVisible)
                {
                    h = h / 2 - 2;
                    t = t + h + 2;
                    this.winDocs.SetLayoutPos(l, w, win.Top, h);
                    this.winSharingUsers.SetLayoutPos(l, w, t, h,1);
                    if (this.winSharingNotes != null && winshNotesVisible)
                        this.winSharingNotes.SetLayoutPos(l, w, t, h, 0);
                }
                else
                {
                    if (this.winDocs != null && windocVisible)
                        this.winDocs.SetLayoutPos(l, w, t, h);
                    if (this.winSharingUsers != null && winshUsrVisible)
                        this.winSharingUsers.SetLayoutPos(l, w, t, h, 1);
                    if (this.winSharingNotes != null && winshNotesVisible)
                        this.winSharingNotes.SetLayoutPos(l, w, t, h, 0);
                }
            }
        }

        private void btnNewNote_Click(object sender, RoutedEventArgs e)
        {
            this.ModeLayoutInsUpdFindClose = MBBConst.MODE_INS;

        }

        private void btnFirstNote_Click(object sender, RoutedEventArgs e)
        {
            GotoFirstNote();
        }

        private void btnDelNote_Click(object sender, RoutedEventArgs e)
        {
            if (this.ModeLayoutInsUpdFindClose != MBBConst.MODE_INS && this.ModeLayoutInsUpdFindClose != MBBConst.MODE_UPD)
            {
                MessageBoxResult r = MessageBox.Show(CRSTranslation.TranslateDialog(CRSTranslation.MSG_DEL_ITEMS, this.UserInfo.Language),
                                    CRSTranslation.TranslateDialog(CRSTranslation.MSG_WARNING, this.UserInfo.Language), MessageBoxButton.YesNo);

                if (r == MessageBoxResult.Yes)
                {
                    //-- prima scrive su file di cancellazione delle note per allineamento
                    //-- poi cancella altrimenti elimino i riferimenti su db per scrittura su file delle condivisioni

                    MBBSyncroUtility.WriteDelNoteSyncroFile(DateTime.Now, this.currentNote.ID, this.UserInfo.Name, MBBCommon.syncroParams);
                    bool res = DbDataUpdate.DeleteNote(this.currentNote.ID, this.currentNote.IDUser, true);

                    foreach (string dest in this.currentNote.SharedUsers)
                    {
                        MBBSyncroUtility.WriteDelNoteSyncroFileShared(DateTime.Now, this.currentNote.ID, this.UserInfo.Name, dest, MBBCommon.syncroParams);
                    }
                    // ELIMINA LA NOTA ANCHE DAL FILE DI TESTO ALTRIMENTI VIENE RECUPERATA dall'utente su un'altra macchina
                    MBBSyncroUtility.DeleteNoteFromUPDFile(this.currentNote.ID, this.UserInfo.Name, MBBCommon.syncroParams);

                    SetNotesToSendEmail(this.currentNote, 2);
                    int idx = this.NotesFound.IndexOf(this.currentNote);
                    if (idx >= 0) this.NotesFound.RemoveAt(idx);

                    // ELIMINO LA NOTA DALLE RECENTI
                    int idxrecent = this.notesRecent.IndexOf(this.currentNote);
                    if (idxrecent >= 0) this.notesRecent.RemoveAt(idxrecent);

                    if (idx > 0)
                        this.CurrentNote = this.NotesFound.ElementAt(idx - 1);

                }
            }
        }

        #endregion

        #region TIMERS
        //-- Set and start the timer
        public void SetTimers()
        {
            if (this.refreshTimerSharedNotes > 0)
            {
                // serve per effettuare caricamento note condivise ogni tot secondi (file ini)
                //dispatcherTimerSyncroSharedNotes = new DispatcherTimer();
                this.dispatcherTimerSyncroSharedNotes.Tick += new EventHandler(dispatcherTimerSharedNotes_Tick);
                this.dispatcherTimerSyncroSharedNotes.Interval = new TimeSpan(0, 0, this.refreshTimerSharedNotes);
                this.dispatcherTimerSyncroSharedNotes.Start();
            }
            // non è usato: ora l'allineamento è manuale da parte dell'utente
            if (this.refreshTimerDocs > 0)
            {
                this.dispatcherTimerSyncroDocs.Tick += new EventHandler(dispatcherDocTimer_Tick);
                this.dispatcherTimerSyncroDocs.Interval = new TimeSpan(0, 0, this.refreshTimerDocs);
                this.dispatcherTimerSyncroDocs.Start();
            }
            dispatcherTimerNotifyStatus.Tick += new EventHandler(dispatcherTimerNotifyImg_Tick);
            dispatcherTimerNotifyStatus.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimerWritingNote.Tick += new EventHandler(dispatcherTimerWriting_Tick);
            dispatcherTimerWritingNote.Interval = new TimeSpan(0, 0, 10);
        }
    
        private void SyncroSharedTodayNotes()
        {
            try
            {
                if ((this.IsVisible) && (this.refreshTimerSharedNotes > 0) && (this.modeLayoutInsUpdFind != MBBConst.MODE_INS && this.modeLayoutInsUpdFind != MBBConst.MODE_UPD))
                {
                    int currentmode = this.modeLayoutInsUpdFind;
                    this.ModeLayoutInsUpdFindClose = MBBConst.MODE_SYNCRO_NOTES;
                    //-- aggiorno solo le note condivise (leggo da file se ci sono e metto su DB) dato che le personali devono essere aggiornate
                    //-- solo all'avvio
                    //MBBSyncroUtility.SyncroDBFromFileShared(this.user);
                    List<DbNote> lstSh = MBBSyncroUtility.SyncroGetNotesFromFileShared(this.UserInfo.Name, DateTime.Now.AddDays(-1), 0);
                   
                    DateTime dt1 = DateTime.Now.AddDays(-1);//DateTime.Now.Date;
                    DateTime dt2 = DateTime.Now;
                    //-- controlla le note condivise
                    MBBFindParams p = new MBBFindParams();
                    p.IDUser = this.UserInfo.IDUser;
                    p.Dt1 = dt1;
                    p.Dt2 = dt2;
                    p.Rate = 0;
                    p.SimbolRate = ">=";
                    p.Title = "";

                    if (lstSh.Count >= 1)
                    {
                        if (this.winSharingNotes != null && !this.winSharingNotes.ctrlMBBSharingNotes.IsPermanentlyClosed)
                        {
                            this.winSharingNotes.FillSharingNotes(lstSh);
                            this.winSharingNotes.ctrlMBBSharingNotes.SetFindParams(p);
                            this.winSharingNotes.Show();
                        }
                        else
                        {
                            if (this.winSharingNotes == null)
                            {
                                this.winSharingNotes = new WinMBBSharingNotes(this.currentNote);
                                this.winSharingNotes.FillSharingNotes(lstSh);
                                this.winSharingNotes.ctrlMBBSharingNotes.SetFindParams(p);
                                if (!this.winSharingNotes.ctrlMBBSharingNotes.IsPermanentlyClosed)
                                    this.winSharingNotes.Show();
                            }
                        }
                    }

                    this.ModeLayoutInsUpdFindClose = currentmode;
                }
            }
            catch { }
        }

        /// <summary>
        /// Recupera le note condivisde se la nota corrente condivide con altri utenti
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dispatcherTimerSharedNotes_Tick(object sender, EventArgs e)
        {
            //-- se l'applicazione non si sta inizializzando (cioè sta partendo) e se non è attiva già la sincronizzazione delle note
            if (this.CurrentNote != null && this.CurrentNote.SharedUsers != null && this.CurrentNote.SharedUsers.Count > 0)
            {
                if (!this.isInitializing && Utils.ExistWindowInstance<WinMBBIntro>() == null)
                {
                    dispatcherTimerSyncroSharedNotes.Stop();
                    //SyncroSharedTodayNotes();
                    StartThreadSyncroShared();
                    dispatcherTimerSyncroSharedNotes.Start();
                }
            }
        }

        /// <summary>
        /// Autosave della nota alla scadenza di questo timer che si resetta ogni qual volta l'utente scrive sul testo o sul titolo.
        /// In questo modo l'autosave avviene solo 10 secondi dopo che l'utente ha smesso di scrivere
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dispatcherTimerWriting_Tick(object sender, EventArgs e)
        {
            try
            {
                //-- all'avvio dell'applicazione non deve funzionare
                if (!this.isInitializing)
                {
                    if ((this.IsVisible) && (MBBCommon.syncroParams.AutoSave) &&
                        ((this.modeLayoutInsUpdFind == MBBConst.MODE_INS || this.modeLayoutInsUpdFind == MBBConst.MODE_UPD)))
                    {
                        if (IsNullNote() == false)
                        {
                            bool res = this.SaveNote();
                            if (res)
                            {
                                //-- ricostruisco il filtro in modo da far includere anche il nuovo title della nota appena creata                        
                                this.findParams.Title = this.GetNewFilter(this.findParams.Title, txtTitle.Text);

                                if (this.winFind != null)
                                {
                                    this.winFind.SetFilterTitle(this.findParams.Title);
                                    //FillNotesFromParams();
                                }                             
                                this.ModeLayoutInsUpdFindClose = MBBConst.MODE_FIND;

                                bool v1 = ManageWinDoc();
                                bool v2 = ManageWinShareUser();

                            }
                        }
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// OBSOLETO: Sincronizzazione dei file
        /// ora l'allineamento è puntuale: l'utente sceglie se farlo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dispatcherDocTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                //-- refreshTimer potrebbe essere resettato a 0
                if (!this.isInitializing)
                {
                    if ((this.IsVisible) && (this.refreshTimerDocs > 0) && (this.modeLayoutInsUpdFind != MBBConst.MODE_INS && this.modeLayoutInsUpdFind != MBBConst.MODE_UPD))
                    {
                        this.dispatcherTimerSyncroDocs.Stop();
                        int currentmode = this.modeLayoutInsUpdFind;
                        this.ModeLayoutInsUpdFindClose = MBBConst.MODE_SYNCRO_DOCS;
                        // 12/2019: la sincronizzazione ora è puntuale => nella FINESTRA DELLA LISTA DEI DOCUMENTI 
                        //  sono evidenziati quelli disallineati e l'utente provvede allinearli prendendo come buono quello più recente
                        //if (MBBCommon.syncroParams.SyncFiles)
                        //   SyncroFilesOBSOLETE();
                        this.ModeLayoutInsUpdFindClose = currentmode;                  
                        this.dispatcherTimerSyncroDocs.Start();
                    }
                }
            }
            catch { }
        }

        // NON USATO : uso solo dispatcherTimerNotifyImg_Tick per tutti i timers di notifica operazione in corso
        protected void dispatcherTimerSyncroDocs_Tick(object sender, EventArgs e)
        {
            //-- refreshTimer potrebbe essere resettato a 0
            if (!this.isInitializing && this.IsVisible)
            {
                if (imgBlackBoxSyncro.Visibility == System.Windows.Visibility.Visible)
                    imgBlackBoxSyncro.Visibility = System.Windows.Visibility.Hidden;
                else imgBlackBoxSyncro.Visibility = System.Windows.Visibility.Visible;
            }           
        }

        protected void dispatcherTimerNotifyImg_Tick(object sender, EventArgs e)
        {
            if (!this.isInitializing && this.IsVisible)
            {
                if (lblInfo.Visibility == System.Windows.Visibility.Hidden)
                {
                    lblInfo.Visibility = System.Windows.Visibility.Visible;
                    lblInfo.Foreground = Brushes.Blue;
                }
                else
                {
                    lblInfo.Visibility = System.Windows.Visibility.Hidden;
                }
            }
        }

        // NON USATO : uso solo dispatcherTimerNotifyImg_Tick per tutti i timers di notifica operazione in corso
        protected void dispatcherTimerInsert_Tick(object sender, EventArgs e)
        {
            //-- refreshTimer potrebbe essere resettato a 0
            if (!this.isInitializing && this.IsVisible)
            {
                if (imgBlackBoxInsert.Visibility == System.Windows.Visibility.Visible)
                    imgBlackBoxInsert.Visibility = System.Windows.Visibility.Hidden;
                else imgBlackBoxInsert.Visibility = System.Windows.Visibility.Visible;

                if (lblInfo.Visibility == System.Windows.Visibility.Hidden)
                {
                    lblInfo.Visibility = System.Windows.Visibility.Visible;
                    lblInfo.Foreground = Brushes.Blue;
                }
                else
                {
                    lblInfo.Visibility = System.Windows.Visibility.Hidden;
                }
            }
            
        }

        // NON USATO : uso solo dispatcherTimerNotifyImg_Tick per tutti i timers di notifica operazione in corso
        protected void dispatcherTimerSharing_Tick(object sender, EventArgs e)
        {
            //-- refreshTimer potrebbe essere resettato a 0
            if (!this.isInitializing && this.IsVisible)
            {
                if (imgShare.Visibility == System.Windows.Visibility.Visible)
                    imgShare.Visibility = System.Windows.Visibility.Hidden;
                else imgShare.Visibility = System.Windows.Visibility.Visible;
            }

        }
        #endregion

        #region EVENTS ctrl TEXT and TITLE
        private void TitleFind()
        {
            if (this.winFindTitle == null)
                this.winFindTitle = new WinMBBFindTitle(this.UserInfo.IDUser);

            this.winFindTitle.IsReduced = true;
            this.winFindTitle.TypeFindAtLeastOne = false;
            this.winFindTitle.FillTitle(this.UserInfo.IDUser);
            this.winFindTitle.ApplyFilterTitle(txtTitle.Text);           
                      
            Window win = Window.GetWindow(this.Parent);
            if (win != null)
            {
                if (this.ExpandLayoutStep != EXPAND_STEP4_MAXIMIZED)
                {
                    this.winFindTitle.SetLayoutPos(win.Left, win.Width, win.Top, win.Height);
                }
            }

            this.winFindTitle.Show();
        }

        private void txtTitle_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TitleFind();
            txtTitle.Focus();
        }

        private void txtTitle_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TitleFind();
                Keyboard.Focus(txtTitle);
            }
            if (e.Key == Key.Tab)
            {
                if (this.winFindTitle != null /*&&
                    this.winFind.IsReduced==true*/)
                {
                    txtMsg.IsTabStop = false;
                    //this.winFind.ctrlMBBTitle.lvTitles.Focus();                  
                    if (this.winFindTitle.ctrlMBBTitle.lvTitles.Items.Count > 0)
                    {
                        this.winFindTitle.ctrlMBBTitle.lvTitles.SelectedIndex = 0;
                        ListViewItem item = this.winFindTitle.ctrlMBBTitle.lvTitles.ItemContainerGenerator.ContainerFromIndex(0) as ListViewItem;
                        item.Focus();
                    }
                }
                else txtMsg.IsTabStop = true;
            }
        }

        private void txtTitle_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtTitle.IsFocused && this.modeLayoutInsUpdFind == MBBConst.MODE_FIND &&
                (//!string.IsNullOrEmpty(txtMsg.Text) ||
                !string.IsNullOrEmpty(this.GetTextNoteContent()) ||
                !string.IsNullOrEmpty(txtTitle.Text)))
            {
                if (this.currentNote != null && this.currentNote.ID > 0)
                    this.ModeLayoutInsUpdFindClose = MBBConst.MODE_UPD;
                else this.ModeLayoutInsUpdFindClose = MBBConst.MODE_INS;

                if (!string.IsNullOrEmpty(txtTitle.Text))
                    txttbTitle.Text = "";
                else txttbTitle.Text = CRSTranslation.TranslateDialog(CRSTranslation.MSG_ENTER_TITLE_NOTE, this.UserInfo.Language);
            }


            //-- abilito ricerca titolo solo in inserimento o modifica: quindi sempre dato che sto digitando
            if (this.ModeLayoutInsUpdFindClose != MBBConst.MODE_FIND && this.winFindTitle != null && this.winFindTitle.IsReduced)
            {
                this.winFindTitle.ApplyFilterTitle(txtTitle.Text);
            }
            if (MBBCommon.syncroParams.AutoSave)
            {
                dispatcherTimerWritingNote.Stop();
                dispatcherTimerWritingNote.Start();
            }
        }

        private void txtMsg_TextChanged(object sender, TextChangedEventArgs e)
        {
            string txt = this.GetTextNoteContent();
            if (txtMsg.IsFocused && this.modeLayoutInsUpdFind == MBBConst.MODE_FIND && 
               (!string.IsNullOrEmpty(txt) ||
                !string.IsNullOrEmpty(txtTitle.Text)))
            {
                if (this.currentNote != null && this.currentNote.ID > 0)
                    this.ModeLayoutInsUpdFindClose = MBBConst.MODE_UPD;
                else this.ModeLayoutInsUpdFindClose = MBBConst.MODE_INS;

                if (!string.IsNullOrEmpty(txt))
                    txtblMsg.Text = "";
                else txtblMsg.Text = CRSTranslation.TranslateDialog(CRSTranslation.MSG_ENTER_TEXT_NOTE, this.UserInfo.Language);
            }

            txtChars.Text = txt.Count().ToString() + "/" + this.dimFieldTxtMsg.ToString();
            
            if (MBBCommon.syncroParams.AutoSave)
            {
                dispatcherTimerWritingNote.Stop();
                dispatcherTimerWritingNote.Start();
            }
        }

        private void txtMsg_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        public string GetTextNoteContent()
        {
            string res = "";
            TextRange range = new TextRange(txtMsg.Document.ContentStart, txtMsg.Document.ContentEnd);         
            try
            {
                res = range.Text;
            }
            catch (Exception)
            {
                throw;
            }
            return res;

        }

        public string GetTextNoteFormatted()
        {           
            TextRange range = new TextRange(txtMsg.Document.ContentStart, txtMsg.Document.ContentEnd);
            try
            {
                using (MemoryStream rtfMemoryStream = new MemoryStream())
                {
                    using (StreamWriter rtfStreamWriter = new StreamWriter(rtfMemoryStream))
                    {
                        range.Save(rtfMemoryStream, DataFormats.Rtf);

                        rtfMemoryStream.Flush();
                        rtfMemoryStream.Position = 0;
                        StreamReader sr = new StreamReader(rtfMemoryStream);
                        return sr.ReadToEnd();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }           
        }

        public void SetTextNote(string rtf)
        {
            TextRange range = new TextRange(txtMsg.Document.ContentStart,
                txtMsg.Document.ContentEnd);

            try
            {
                using (MemoryStream rtfMemoryStream = new MemoryStream())
                {
                    using (StreamWriter rtfStreamWriter = new StreamWriter(rtfMemoryStream))
                    {
                        rtfStreamWriter.Write(rtf);
                        rtfStreamWriter.Flush();
                        rtfMemoryStream.Seek(0, SeekOrigin.Begin);

                        if(rtf.Contains(@"rtf1\ansi"))
                            range.Load(rtfMemoryStream, DataFormats.Rtf);
                        else range.Load(rtfMemoryStream, DataFormats.Text);
                        //txtMsg.Selection.Load(rtfMemoryStream, DataFormats.);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long GetLinesNote()
        {
            string s = GetTextNoteContent();
            long count = 1;
            int position = 0;
            while ((position = s.IndexOf("\\par", position)) != -1)
            {
                count++;
                position++;         // Skip this occurance!
            }
            return count;
        }

        private int GetLinesNote2()
        {
            int currentLineNumber = 0;
            TextPointer p = txtMsg.Document.ContentStart.GetLineStartPosition(0);
            while (p.GetLineStartPosition(++currentLineNumber) != null) { }
           
            return currentLineNumber;
        }
       
        #endregion

        #region TOOL BAR RTF
        public void SelectImg()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Image files (*.jpg, *.jpeg,*.gif, *.png) | *.jpg; *.jpeg; *.gif; *.png";
            var result = dlg.ShowDialog();
            if (result.Value)
            {
                Uri uri = new Uri(dlg.FileName, UriKind.Relative);
                BitmapImage bitmapImg = new BitmapImage(uri);
                Image image = new Image();
                image.Stretch = Stretch.Fill;
                image.Width = 250;
                image.Height = 200;
                image.Source = bitmapImg;
                var tp = txtMsg.CaretPosition.GetInsertionPosition(LogicalDirection.Forward);
                new InlineUIContainer(image, tp);
            }
        }
        private void btnToolBarImage_Click(object sender, RoutedEventArgs e)
        {
            SelectImg();
        }

        private void btnToolBarTextcolor_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.ColorDialog colorDialog = new System.Windows.Forms.ColorDialog();
            if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK )
            {
                TextRange range = new TextRange(txtMsg.Selection.Start,
                    txtMsg.Selection.End);

                System.Windows.Media.Color col = new System.Windows.Media.Color();
                col.A = colorDialog.Color.A;
                col.B = colorDialog.Color.B;
                col.G = colorDialog.Color.G;
                col.R = colorDialog.Color.R;

                range.ApplyPropertyValue(FlowDocument.ForegroundProperty, new SolidColorBrush(col));
                btnToolBarTextcolorSel.Background = new SolidColorBrush(col);
            }
        }

        
        private void btnToolBarBackcolor_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.ColorDialog colorDialog = new System.Windows.Forms.ColorDialog();
            if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                TextRange range = new TextRange(txtMsg.Selection.Start,
                    txtMsg.Selection.End);

                System.Windows.Media.Color col = new System.Windows.Media.Color();
                col.A = colorDialog.Color.A;
                col.B = colorDialog.Color.B;
                col.G = colorDialog.Color.G;
                col.R = colorDialog.Color.R;

                range.ApplyPropertyValue(TextElement.BackgroundProperty, new SolidColorBrush(col));
                btnToolBarBackcolorSel.Background = new SolidColorBrush(col);
            }
        }

        private void btnToolBarBackcolorSel_Click(object sender, RoutedEventArgs e)
        {
            TextRange range = new TextRange(txtMsg.Selection.Start,
                    txtMsg.Selection.End);
            range.ApplyPropertyValue(TextElement.BackgroundProperty, btnToolBarBackcolorSel.Background);
        }

        private void btnToolBarTextcolorSel_Click(object sender, RoutedEventArgs e)
        {
            TextRange range = new TextRange(txtMsg.Selection.Start,
                    txtMsg.Selection.End);
            range.ApplyPropertyValue(TextElement.ForegroundProperty, btnToolBarTextcolorSel.Background);
        }

        private void btnClearFormat_Click(object sender, RoutedEventArgs e)
        {
            TextRange range = new TextRange(txtMsg.Selection.Start, txtMsg.Selection.End);
            range.ClearAllProperties();       
        }

        #endregion

        #region events NOT USED
        private void imgRat1_MouseEnter(object sender, MouseEventArgs e)
        {
            //SetRateFromImg(sender as Image);
        }

        private void imgNext_MouseDown(object sender, MouseButtonEventArgs e)
        {
            GotoNextNote();
        }

        private void imgPrev_MouseDown(object sender, MouseButtonEventArgs e)
        {
            GotoPrevNote();
        }
        #endregion

        #region MINIMIZE btns MAXIMIZE/MINIMIZE/CLOSE/A autoexpand
        private void imgExit_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (this.winDocs != null)
                this.winDocs.Close();
            if (this.winFind != null)
                this.winFind.Close();
            if (this.winFindTitle != null)
                this.winFindTitle.Close();
            if (this.winSharingUsers != null)
                this.winSharingUsers.Close();
            if (this.winSharingNotes != null)
                this.winSharingNotes.Close();
            SetRecentNotesToIni();

            if (this.ThreadImportShared != null)
                this.ThreadImportShared.Abort();
            if (this.ThreadImportData != null)
                this.ThreadImportData.Abort();

            Window win = Window.GetWindow(this.Parent);
            if (win != null)
            {
                if (ThreadImportShared!=null && (ThreadImportShared.ThreadState == System.Threading.ThreadState.Running || ThreadImportShared.ThreadState == System.Threading.ThreadState.AbortRequested))
                    ThreadImportShared.Abort();
                if (ThreadImportData != null && (ThreadImportData.ThreadState == System.Threading.ThreadState.Running || ThreadImportData.ThreadState == System.Threading.ThreadState.AbortRequested))
                    ThreadImportData.Abort();
                
                win.Close();
                //System.Windows.Threading.Dispatcher.CurrentDispatcher.Thread.();
            }
        }

        private void imgMinimize_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MinimizeAllForms();
        }

        private void MinimizeAllForms()
        {
            if (this.winDocs != null)
                this.winDocs.Hide();
            if (this.winFind != null)
                this.winFind.Hide();
            if (this.winFindTitle != null)
                this.winFindTitle.Hide();
            if (this.winSharingUsers != null)
                this.winSharingUsers.Hide();
            if (this.winSharingNotes != null)
                this.winSharingNotes.Hide();

            Window win = Window.GetWindow(this.Parent);
            if (win != null)
            {
                win.WindowState = WindowState.Minimized;
            }
        }

        private void imgMaximizeMsg_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (this.ExpandLayoutStep < EXPAND_STEP4_MAXIMIZED)
                this.ExpandLayoutStep = this.ExpandLayoutStep + 1;
            else
                this.ExpandLayoutStep = EXPAND_FALSE;

            SetLayoutExpand(this.ExpandLayoutStep); 
        }

        private void ImgCloseBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (this.Expanded)
            {
                this.Expanded = false;
            }
            else
            {
                SetLayoutExpand(EXPAND_FALSE);
            }
        }

        private void imgStartStopLog_MouseDown(object sender, MouseButtonEventArgs e)
        {
            /*if (IS_ACTIVE_KEY_EVENTS)
                imgStartStopLog.Source = CRSImage.CreateImage(CRSGlobalParams.ImageStopPathName);
            else
                imgStartStopLog.Source = CRSImage.CreateImage(CRSGlobalParams.ImageStartPathName);
            */
            IS_ACTIVE_KEY_EVENTS = !IS_ACTIVE_KEY_EVENTS;
            if (IS_ACTIVE_KEY_EVENTS && this.eventHideWindow != null)
                this.eventHideWindow();
        }

        // non usato dato che eiste già la classe CRSEventListener gestita in WinMBBCore
        private void EventFollowCursorEsc(string s)
        {
            // intercetto l'evento 'pressione di tasto esc
            if (s.Contains(System.Windows.Forms.Keys.Escape.ToString()))
            {
                SetLayoutExpand(EXPAND_STEP1_AUTO);
                //MBBEvents.ResetHookEvents();
            }
        }

        private void txtClose_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (this.ModeLayoutInsUpdFindClose != MBBConst.MODE_INS && this.ModeLayoutInsUpdFindClose != MBBConst.MODE_UPD)
            {
                this.ModeLayoutInsUpdFindClose = MBBConst.MODE_CLOSE;
                this.Expanded = false;
            }
        }

        private void lblAutoExpand_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.ExpandAuto();
        }
        #endregion

        #region EVENTS BTNS DOWN TEXT NOTE - LAYOUT
        private void txtWrap_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //if (txtMsg.TextWrapping == TextWrapping.Wrap)
            if (txtMsg.Document.PageWidth < 1000)
            {
                //txtMsg.TextWrapping = TextWrapping.NoWrap;
                txtMsg.ToolTip = "Wrap ON";

                txtMsg.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
                txtMsg.Document.PageWidth = 1000;
            }
            else
            {
                //txtMsg.TextWrapping = TextWrapping.Wrap;
                txtMsg.ToolTip = "Wrap OFF";
                txtMsg.Document.PageWidth = pnlRTF.Width - 4;

            }
        }

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            Opacity = 1;
            if (this.winDocs != null) this.winDocs.Opacity = 1;
            if (this.winFind != null) this.winFind.Opacity = 1;
            if (this.winSharingUsers != null) this.winSharingUsers.Opacity = 1;
            if (this.winFindTitle != null) this.winFindTitle.Opacity = 1;
            if (this.winSharingNotes != null) this.winSharingNotes.Opacity = 1;
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            if (this.isOpacity)
            {
                if (this.ModeLayoutInsUpdFindClose != MBBConst.MODE_INS && this.ModeLayoutInsUpdFindClose != MBBConst.MODE_UPD)
                {
                    this.Opacity = 0.5;
                    if (this.winDocs != null) this.winDocs.Opacity = 0.5;
                    if (this.winFind != null) this.winFind.Opacity = 0.5;
                    if (this.winSharingUsers != null) this.winSharingUsers.Opacity = 0.5;
                    if (this.winFindTitle != null) this.winFindTitle.Opacity = 0.5;
                    if (this.winSharingNotes != null) this.winSharingNotes.Opacity = 0.5;
                }
            }
        }

        private void txtOpacity_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.isOpacity = !this.isOpacity;
            if (this.isOpacity)
                txtOpacity.ToolTip = "Opacity OFF";
            else txtOpacity.ToolTip = "Opacity ON";
        }
        
        private void txtNotEnlarge_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Window win = Window.GetWindow(this.Parent);
            win.Width = win.Width - 20;
            win.Height = win.Height - 20;
            win.Top = win.Top + 20; 
        }

        private void txtEnlarge_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Window win = Window.GetWindow(this.Parent);
            win.Width = win.Width + 20;
            win.Height = win.Height + 20;
            win.Top = win.Top - 20; 
        }
        
        private void imgNewDoc_MouseDown(object sender, MouseButtonEventArgs e)
        {
            string ext = MBBCommon.syncroParams.NewFileExtension;
            string filename = txtTitle.Text;
            filename = filename.Replace(' ', '_');
            CreateNewFile(ext, filename, MBBCommon.syncroParams.LocalTempDir);
        }

        private void CreateNewFile(string ext, string filename, string localTempDir)
        {
            try
            {
                //string ext = this.syncroParams.NewFileExtension;
                if (ext.Contains('.'))
                    ext.Remove(0, 1);

                if (string.IsNullOrEmpty(ext))
                    ext = "txt";

                int idx = 1;
                //string filename = txtTitle.Text;
                if (string.IsNullOrEmpty(txtTitle.Text))
                    filename = "NewNoteFile";

                string fileLoc = localTempDir + "\\" + filename + idx.ToString().PadLeft(3, '0') + "." + ext;

                while (File.Exists(fileLoc))
                {
                    idx++;
                    fileLoc = localTempDir + "\\" + filename + idx.ToString().PadLeft(3, '0') + "." + ext;
                }

                if (!File.Exists(fileLoc))
                {
                    if (!Directory.Exists(localTempDir))
                        Directory.CreateDirectory(localTempDir);

                    //FileStream fs = File.Create(fileLoc);
                    string fileerr = CreateFileFromTemplate(ext, fileLoc);

                    if (string.IsNullOrEmpty(fileerr))
                    {
                        DbNoteDoc doc = new DbNoteDoc();

                        doc.DateTimeLastMod = DateTime.Now;
                        doc.DocName = filename;
                        doc.FileNameLocal = fileLoc;
                        //-- verrà inizializzata successivamente
                        doc.IDNote = this.currentNote.ID;
                        doc.IDUser = this.UserInfo.IDUser;
                        doc.UserMod = this.UserInfo.Name;
                        doc.UserNoteOwner = this.UserInfo.Name;
                        doc.IsNewFile = true;

                        this.CurrentNote.Docs.Add(doc);
                        OpenDoc(fileLoc);
                    }
                    else MessageBox.Show(fileerr);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private string CreateFileFromTemplate(string ext, string fileLoc)
        {
            string err = "";
            string source = ".\\Templates\\template." + ext;
            if (File.Exists(source))
                File.Copy(source,fileLoc);
            else err = "Template file not exists!\nCreate it in Templates folder.. ";

            return err;
        }

        private bool OpenDoc(string fileNameLocal)
        {
            bool res = true;
            try
            {
                //-- ora gestisco più docs per nota
                if (string.IsNullOrEmpty(fileNameLocal) == false)
                {
                    Process.Start(fileNameLocal);
                }
                else
                {
                    MessageBox.Show(CRSTranslation.TranslateDialog(CRSTranslation.MSG_FILE_NOT_FOUND, this.UserInfo.Language),
                                CRSTranslation.TranslateDialog(CRSTranslation.MSG_WARNING, this.UserInfo.Language));
                }
                return res;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region EVENTS ctrl RECENT
        private void btnLastNote_Click(object sender, RoutedEventArgs e)
        {
            GotoLastNote();
        }

        private void btnRecentPrev_Click(object sender, RoutedEventArgs e)
        {
            int idx = notesRecent.FindIndex(X=>X.ID == this.currentNote.ID);
            if (idx - 1 >=0)
                this.CurrentNote = this.notesRecent[ idx - 1];
        }

        private void btnRecentNext_Click(object sender, RoutedEventArgs e)
        {
            int idx = notesRecent.FindIndex(X => X.ID == this.currentNote.ID);
            if (idx + 1 < this.notesRecent.Count)
                this.CurrentNote = this.notesRecent[idx + 1];
        }
       
        private void btnListRecentNote_MouseDown(object sender, RoutedEventArgs e)
        {
            //-- open window params
            if (this.winFind == null)
            {
                this.winFind = new WinMBBFind(this.UserInfo.IDUser, this.notesRecent);
            }
            else
            {
                if (this.notesRecent.Count > 0)
                    this.winFind.FillExtNotes(this.notesRecent);
            }

            this.winFind.IsHeaderExpanded = false;           
            this.winFind.IsReduced = false;
            this.winFind.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.winFind.HEADER = "       RECENTS";
            this.winFind.Show();
        }

        private void ImgSendEmail_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (this.currentNote.SharedUsers.Count > 0)
            {
                // se i destinatari sono impostati manda email
                MBBEmailParams p = MBBCommon.GetEmailParamsFromIni();               
                string appName = CRSIniFile.GetApplicationNameFromIni();
                string appVers = CRSIniFile.GetApplicationVersionFromIni();
                string pwd = CRSLicense.DecodeCode(appName, appVers, this.UserInfo.PasswordEmail, 1);
                CRSMailRepository rep = new CRSMailRepository(p.ImapServer, p.ImapPort, p.SmtpServer, p.SmtpPort, true, this.UserInfo.Name, pwd, this.UserInfo.RegistryName + " " + this.UserInfo.RegistrySurname);
                bool res = true; bool resfalse = false;
                foreach (string s in this.currentNote.SharedUsers)
                {
                    res = rep.SendMail(s, GetTextNoteContent(), txtTitle.Text, this.CurrentNote.Docs.Select(X => X.FileNameLocal).ToList());
                    if (!res) resfalse = true;
                }
                pnlBtnEmail.Background = Brushes.Transparent;
                pnlBtnEmail.ToolTip = "";
                if (resfalse)
                {
                    pnlBtnEmail.Background = Brushes.LightYellow;
                    pnlBtnEmail.ToolTip = "Error sending email.. open Parameters/Utilities to see logs..";
                }
            }
            else
            {
                if (this.winSharingUsers == null)
                    this.winSharingUsers = new WinMBBSharingUsers(this.UserInfo.IDUser, this.currentNote.ID, SetModeUpdateAndSharedUsers);
                else
                    this.winSharingUsers.ctrlMBBShare.FillUsersSharing(this.currentNote.ID);

                ManagePosWin(false, true, false);
            }
        }

        private void imgSendEmail_MouseEnter(object sender, MouseEventArgs e)
        {
            if(this.currentNote.SharedUsers.Count>0)
                SetImageEmail(1);
            else SetImageEmail(2);
        }

        private void imgSendEmail_MouseLeave(object sender, MouseEventArgs e)
        {
            SetImageEmail(0);
        }

        private void imgPlay_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // cambia icona
            /*if (Convert.ToInt32(imgPlay.Tag) == 0)
            {
                imgPlay.Source = CRSImage.CreateImage(CRSGlobalParams.ImageStartPathName);
                imgPlay.Tag = 1;
            }
            else
            {
                imgPlay.Source = CRSImage.CreateImage(CRSGlobalParams.ImageStopPathName);
                imgPlay.Tag = 0;
            }
            */
            // registra start activity
        }

        private void imgStartLog_MouseDown(object sender, MouseButtonEventArgs e)
        {
            /*if (!IS_ACTIVELOG_REGISTRATION_EVENTS)
                imgStartLog.Source = CRSImage.CreateImage(CRSGlobalParams.IconBulletGreenPathName);
            else imgStartLog.Source = CRSImage.CreateImage(CRSGlobalParams.IconBulletBallYellowPathName);

            IS_ACTIVELOG_REGISTRATION_EVENTS = !IS_ACTIVELOG_REGISTRATION_EVENTS;
            */
        }

        private void txtMsg_MouseEnter(object sender, MouseEventArgs e)
        {
            txtblMsg.Background = Brushes.WhiteSmoke;
        }

        private void txtMsg_MouseLeave(object sender, MouseEventArgs e)
        {
            txtblMsg.Background = Brushes.Transparent;
        }

        #endregion

       
    }
}
