using MyBlackBoxCore.DataEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using CRS.WPFControlsLib;
using CRS.Library;

namespace MyBlackBoxCore
{
    /// <summary>
    /// Interaction logic for MBBActivities.xaml
    /// </summary>
    public partial class MBBActivity : UserControl
    {
        public MBBUserInfo UserInfo = null;
        public delegate void EventAddNote();
        private EventAddNote eventAddNote;
        public delegate void EventStartActivity();
        private EventStartActivity eventStartActicity;
        public delegate void EventHide();
        private EventHide eventHide;
     
        // 0: view, 1: insert, 2: modify
        private int mode = 0;
        public string TITLE;
        public string TEXT;
        private DispatcherTimer timerActivity = new DispatcherTimer();

        public WPFActivity ACTIVITY = new WPFActivity();
        //public List<DbActivity> ActivitiesModToSend = new List<DbActivity>();
        //public List<DbActivityLogTypeOp> ActivityLogModToSend = new List<DbActivityLogTypeOp>();

        private List<DbNote> LST_NOTES = new List<DbNote>();

        public MBBActivity()
        {
            InitializeComponent();
        }

        #region PROPERTIES
        public bool SHOW_TOP_HEADER
        {
            set {
                if(value)
                    pnlTopHeader.Visibility = Visibility.Visible;
                else pnlTopHeader.Visibility = Visibility.Collapsed;
            }
            get { return pnlTopHeader.Visibility == Visibility.Visible; }
        }
        /// <summary>
        /// -1: solo lista,
        /// 0: mostra lista e attività scelta,
        /// 1: inserimento,
        /// 2: modifica
        /// </summary>
        public int MODE
        {
            set
            {
                this.mode = value;
                // mostra solo lista attività
                if (mode == -1)
                {
                    pnlGridMain.RowDefinitions[2].Height = new GridLength(0);
                    pnlPlayStop.Visibility = Visibility.Collapsed;
                    pnlOkCanc.Visibility = Visibility.Collapsed;
                    pnlProgBar.Visibility = Visibility.Collapsed;
                }
                else
                // mostra lista attività e attività scelta
                if (mode == 0)
                {
                    pnlGridMain.RowDefinitions[2].Height = new GridLength(125);
                    pnlPlayStop.Visibility = Visibility.Visible;
                    pnlOkCanc.Visibility = Visibility.Collapsed;
                    pnlProgBar.Visibility = Visibility.Visible;
                }
                else
                // mostra lista attività e attività in modifica o inserimento
                if (mode >= 1)
                {
                    pnlGridMain.RowDefinitions[2].Height = new GridLength(125);
                    pnlPlayStop.Visibility = Visibility.Collapsed;
                    pnlOkCanc.Visibility = Visibility.Visible;
                    pnlProgBar.Visibility = Visibility.Collapsed;
                }
                /*
                // mostra lista attività e attività non in modifica o inserimento
                if (mode == 2)
                {
                    pnlGridMain.RowDefinitions[2].Height = new GridLength(125);
                    pnlPlayStop.Visibility = Visibility.Collapsed;
                    pnlOkCanc.Visibility = Visibility.Collapsed;
                    pnlProgBar.Visibility = Visibility.Collapsed;
                }*/
            }
            get { return this.mode; }
        }
        #endregion

        #region INIT
        public void SetEventAddNote(EventAddNote evn)
        {
            this.eventAddNote = evn;
        }

        public void Init(MBBUserInfo user, EventStartActivity evn1, EventAddNote evn2, EventHide evn3)
        {
            this.UserInfo = user;
            this.eventStartActicity = evn1;
            this.eventAddNote = evn2;
            this.eventHide = evn3;
            SetTimers();
            this.ACTIVITY = new WPFActivity();
            this.ACTIVITY.ListLog = new List<DbActivityLog>();
            // inizio del mese           
            dtpDateFromLogs.SelectedDate = DateTime.Now.AddDays(-30);
            dtpDateToLogs.SelectedDate = DateTime.Now.AddDays(1);
            dtpDateFromLogs.DisplayDate = DateTime.Now.AddDays(-30);
            dtpDateToLogs.DisplayDate = DateTime.Now.AddDays(1);
            dtFilter1.SelectedDate = DateTime.Now.AddDays(-30);
            dtFilter2.SelectedDate = DateTime.Now;
            dtFilter1.DisplayDate = DateTime.Now.AddDays(-30);
            dtFilter2.DisplayDate = DateTime.Now;

            FillCombo();
        }

        public void SetParams(DateTime dt1, DateTime dt2, bool showonlywithnotes)
        {
            dtFilter1.DisplayDate = dt1;
            dtFilter2.DisplayDate = dt2;
            dtFilter1.SelectedDate = dt1;
            dtFilter2.SelectedDate = dt2;
            chkStsWithNotes.IsChecked = showonlywithnotes;
        }

        public void GetParams(out DateTime dt1, out DateTime dt2, out bool showonlywithnotes)
        {
            dt1 = dtFilter1.DisplayDate;
            dt2 = dtFilter2.DisplayDate;
            showonlywithnotes = chkStsWithNotes.IsChecked.Value;
        }

        private void FillCombo()
        {
            cbStsIns.Items.Clear();
            cbStsIns.Items.Add("Waiting");
            cbStsIns.Items.Add("Started");
            cbStsIns.Items.Add("Closed");
            cbStsIns.Items.Add("Deleted");

            cbTypeIns.Items.Clear();
            cbTypeIns.Items.Add("Incident");
            cbTypeIns.Items.Add("Backlog");
            cbTypeIns.Items.Add("Developement");
            cbTypeIns.Items.Add("Office");
            cbTypeIns.Items.Add("Burocracy");
        }
        
        public void EventGoToNotes(object container)
        {
            WinMBBStickNotes wst = null;
            foreach (Window win in Application.Current.Windows)
            {
                if (win is WinMBBStickNotes)
                {
                    wst = win as WinMBBStickNotes;
                    break;
                }
            }

            if (wst == null)
                wst = new WinMBBStickNotes();
            Window win1 = Window.GetWindow(this.Parent);
            if (win1 is WinMBBCore)
            {
                this.Visibility = Visibility.Hidden;
                if (this.eventHide != null)
                    eventHide();
            }
            List<DbNote> lstNotes = this.LST_NOTES.Where(X => X.IDActivity == (container as CRSContainerData).ID).ToList();
            wst.SetParamsFilterControlActivity(lstNotes.Min(X=>X.DateTimeInserted), DateTime.Now.AddDays(1), true);
            wst.Init(this.UserInfo, lstNotes, null, null, 1, true);
            wst.FillActivitiesParams(false);
            wst.Show();
        }

        public void EventGoToNotesWithDate(object container)
        {
            WinMBBStickNotes wst = null;
            foreach (Window win in Application.Current.Windows)
            {
                if (win is WinMBBStickNotes)
                {
                    wst = win as WinMBBStickNotes;
                    break;
                }
            }

            if (wst == null)
                wst = new WinMBBStickNotes();

            CRSContainerData c = (container as CRSContainerData);
            List<DbNote> lstNotes = DbDataAccess.GetActivityNotes(c.ID);
            if (lstNotes.Count > 0 && c.DATE2 != null && c.DATE2 > DateTime.MinValue)
                lstNotes = lstNotes.Where(X => X.DateTimeLastMod >= c.DATE && X.DateTimeLastMod <= c.DATE2).ToList();
            //wst.FillNotes(lstNotes, 0, 1);
            wst.Init(this.UserInfo, lstNotes, null, null, 1, true);
            wst.Show();
        }
        #endregion

        #region METHODS LAYOUT POS
        public void SetDimPos(int step, double fixedwdt)
        {
            Window win = Window.GetWindow(this.Parent);
            // lascio un po di spaio in basso
            double screenHeight = System.Windows.SystemParameters.WorkArea.Height - 64;
            double screenWgt = System.Windows.SystemParameters.WorkArea.Width - 12;

            if (fixedwdt > 0)
                win.Width = fixedwdt;
            // step 0: auto heigth
            // step 1: sposta su lato dx schermo a 3/4
            // step 2: sposta su lato dx schermo in tutta altezza
            // step 3: sposta su lato dx schermo in tutta altezza
            if (step == 0)
            {
                double hgtact = 0;
                foreach(CRSContainerData c in pnlActivitiesContainer.Children)
                {
                    hgtact = hgtact + c.Height;
                }

                hgtact = pnlBtns.Height +
                    pnlGridMain.RowDefinitions[0].Height.Value +
                    pnlGridMain.RowDefinitions[2].Height.Value +
                    hgtact + 12 + 36;

                if (hgtact >= screenHeight)
                {
                    //altezza di default
                    hgtact = screenHeight;
                }

                win.Height = hgtact;
                win.Top = screenHeight - win.Height;
                win.Left = screenWgt - win.Width;
            }
            else
            if (step == 1)
            {
                win.Height = (screenHeight / 2);
                win.Top = screenHeight - win.Height;
                win.Left = screenWgt - win.Width;
            }
            else
            if (step == 2)
            {
                win.Height = (screenHeight / 3) * 2;
                win.Top = screenHeight - win.Height;
                win.Left = screenWgt - win.Width;
            }
            else
            if (step == 3)
            {
                win.Top = 12;
                win.Height = screenHeight;
                win.Left = screenWgt - win.Width;
            }
        }

        private void ResizeProportionalLogActivities(List<DbActivityLog> lstlogs, int totalHeigthPanelContainer)
        {
            int mintowork = (int)lstlogs.Where(Y => Y.StopActivityLog > DateTime.MinValue).Sum(X => X.StopActivityLog.Subtract(X.StartActivityLog).TotalMinutes);
            int idx = 0;
            foreach (DbActivityLog log in lstlogs)
            {
                int totminact = 0;
                if (log.StopActivityLog > DateTime.MinValue)
                    totminact = (int)log.StopActivityLog.Subtract(log.StartActivityLog).TotalMinutes;
                float perc = 0;
                if (mintowork > 0) perc = ((float)totminact / mintowork) * 100;

                CRSContainerData c = FindActivity(log.IDActivity, log.StartActivityLog);
                if (c != null)
                {
                    double len = (double)perc * totalHeigthPanelContainer / 100;
                    if (len < 40)
                        len = 40;
                    pnlLogActivities.RowDefinitions[idx].Height = new GridLength(len, GridUnitType.Auto);
                }
                idx++;
            }
        }
        #endregion
        #region METHODS UTILITIES

        #region OPEN START STOP
        /// <summary>
        /// Apre e imposta l'attività corrente: si vede nel pannello in basso
        /// </summary>
        /// <param name="idactivity"></param>
        private void OpenActivity(long idactivity)
        {
            pnlContainerNewCurrentAct.RowDefinitions[0].Height = new GridLength(0);
            pnlContainerNewCurrentAct.RowDefinitions[1].Height = new GridLength(200, GridUnitType.Star);

            if (this.ACTIVITY != null && !this.ACTIVITY.IsStarted)
            {
                DbActivity a = DbDataAccess.GetActivity(idactivity);
                List<DbNote> lstNotes = DbDataAccess.GetActivityNotes(idactivity);
                Utils.CopyObjectToObject(a, this.ACTIVITY);
                this.ACTIVITY.ListLog = DbDataAccess.GetActivityLogs(idactivity);
                
                SetContentCurrentActivity();
                ctrlCurrentActivity.ID = a.IDActivity;
                ctrlCurrentActivity.SHOW_PNLBTN_GOTONOTES = lstNotes.Count() > 0;
                ctrlCurrentActivity.SHOW_ICO_DOCS = lstNotes.Where(X => X.Docs != null && X.Docs.Count() > 0).Count() > 0;                

                double hgt = ctrlCurrentActivity.SetHeigth(0, true);        
                if (hgt > 220)
                    hgt =220;
                if (hgt < 100) hgt = 200;
                pnlContainerNewCurrentAct.RowDefinitions[1].Height = new GridLength(hgt, GridUnitType.Star);              
            }
            else
            {
                if (this.ACTIVITY != null && this.ACTIVITY.IDActivity > 0)
                    MessageBox.Show(CRSTranslation.TranslateDialog(CRSTranslation.MSG_CLOSE_ACTIVITY_BEFORE, this.UserInfo.Language));
                else
                    StartStopActivity();
            }
        }

        public void StartStopActivity()
        {
            // NOTE: Start Activity è il momento in cui prendo in carico un'attivittà
            //  - non necessariamente ci sto ancora lavorando
            //  - l'insieme degli items in ListIntervals definisce quando e per quanto tempo ho lavorato all'attività
            //  - non posso lavorare a due attività contemporaneamente
            bool istostart = true;
            if (this.ACTIVITY.ListLog.Count > 0)
            {
                // se è già avviata la stoppo
                if (this.ACTIVITY.ListLog.Last().StartActivityLog > DateTime.MinValue &&
                    this.ACTIVITY.ListLog.Last().StopActivityLog == DateTime.MinValue)
                {
                    istostart = false;
                    this.ACTIVITY.IsStarted = false;
                    this.ACTIVITY.ListLog.Last().StopActivityLog = DateTime.Now;
                    this.ACTIVITY.StopActivity = DateTime.Now;
                    timerActivity.Stop();
                    DbDataUpdate.UpdateActivityLog(this.ACTIVITY.IDActivity
                        , this.ACTIVITY.ListLog.Last().StartActivityLog
                        , this.ACTIVITY.ListLog.Last().StartActivityLog
                        , this.ACTIVITY.ListLog.Last().StopActivityLog);
                    // start dell'activity empty
                    DbDataUpdate.UpdateEmptyActivityLog(-1, DateTime.Now.AddSeconds(1), DateTime.MinValue, this.UserInfo.IDUser);

                }
            }
            // se è da iniziare la inizio
            if (istostart)
            {
                DbActivityLog a = new DbActivityLog();
                a.IDActivity = this.ACTIVITY.IDActivity;
                a.StartActivityLog = DateTime.Now;
                a.StopActivityLog = DateTime.MinValue;
                this.ACTIVITY.ListLog.Add(a);
                this.ACTIVITY.IsStarted = true;

                // stop dell'activity empty
                DbDataUpdate.UpdateEmptyActivityLog(-1, DateTime.MinValue, DateTime.Now.AddSeconds(-1), this.UserInfo.IDUser);

                timerActivity.Start();
            }

            SetImgStartStopActivity(this.ACTIVITY != null && this.ACTIVITY.IDActivity > 0 && this.ACTIVITY.IsStarted);
        }
        /// <summary>
        /// fermo l'attività corrente o l'empty activity
        /// </summary>
        public void StopActivity()
        {
            // NOTE: Fermo l'attività corrente se è avviata
            if (this.ACTIVITY.ListLog.Count > 0)
            {
                // se è già avviata la stoppo
                if (this.ACTIVITY.ListLog.Last().StartActivityLog > DateTime.MinValue &&
                    this.ACTIVITY.ListLog.Last().StopActivityLog == DateTime.MinValue)
                {
                    this.ACTIVITY.ListLog.Last().StopActivityLog = DateTime.Now;
                    this.ACTIVITY.StopActivity = DateTime.Now;
                    this.ACTIVITY.IsStarted = false;
                    timerActivity.Stop();
                    DbDataUpdate.UpdateActivityLog(this.ACTIVITY.IDActivity
                        , this.ACTIVITY.ListLog.Last().StartActivityLog
                        , this.ACTIVITY.ListLog.Last().StartActivityLog
                        , this.ACTIVITY.ListLog.Last().StopActivityLog);
                }
            }
            if (this.ACTIVITY == null || (this.ACTIVITY != null && !this.ACTIVITY.IsStarted))
            {
                // fermo l'empty activity
                DbDataUpdate.UpdateEmptyActivityLog(-1, DateTime.MinValue, DateTime.Now.AddSeconds(-1), this.UserInfo.IDUser);
            }
        }

        public void StartEmptyActivity()
        {
            if (this.ACTIVITY == null || (this.ACTIVITY != null && !this.ACTIVITY.IsStarted))
            {
                // avvio l'empty activity
                DbDataUpdate.UpdateEmptyActivityLog(-1, DateTime.Now.AddSeconds(-1), DateTime.MinValue, this.UserInfo.IDUser);
            }
        }
        #endregion
        public int CalcSecondsCurrentActivity(out string hhmmss)
        {
            int res = 0;
            /*foreach(DbActivityLog a in this.ACTIVITY.ListLog)
            {
                if(a.StartActivityLog> DateTime.MinValue && a.StopActivityLog>DateTime.MinValue)
                    res += Convert.ToInt32(a.StopActivityLog.Subtract(a.StartActivityLog).TotalSeconds);
            }*/
            TimeSpan time = TimeSpan.FromSeconds(this.ACTIVITY.GetTotSecActivity());

            //here backslash is must to tell that colon is
            //not the part of format, it just a character that we want in output
            hhmmss = time.ToString(@"hh\:mm\:ss");

            return res;
        }

        private void WriteInXls()
        {

        }

        private void ReadFromXls()
        {

        }

        #region SELECT DESELECT
        public CRSContainerData GetSelectedActivity()
        {
            CRSContainerData res = new CRSContainerData();
            foreach (Control c in pnlActivitiesContainer.Children)
            {
                if (c is CRSContainerData)
                {
                    if ((c as CRSContainerData).IS_SELECTED)
                    {
                        res = c as CRSContainerData;
                        break;
                    }
                }
            }
            return res;
        }

        private List<CRSContainerData> GetSelectedActivities()
        {
            List<CRSContainerData> res = new List<CRSContainerData>();
            foreach (Control c in pnlActivitiesContainer.Children)
            {
                if (c is CRSContainerData)
                {
                    if ((c as CRSContainerData).IS_SELECTED)
                        res.Add(c as CRSContainerData);
                }
            }
            return res;
        }

        private CRSContainerData GetSelectedLogActivity()
        {
            CRSContainerData res = new CRSContainerData();
            foreach (Control c in pnlLogActivities.Children)
            {
                if (c is CRSContainerData)
                {
                    if ((c as CRSContainerData).IS_SELECTED)
                    {
                        res = c as CRSContainerData;
                        break;
                    }
                }
            }
            return res;
        }

        private CRSContainerData GetSelectedStronglyLogActivity_CRSContainerData()
        {
            CRSContainerData res = new CRSContainerData();
            foreach (Control c in pnlLogActivities.Children)
            {
                if (c is CRSContainerData)
                {
                    if ((c as CRSContainerData).IS_SELECTED_HIGH)
                    {
                        res = c as CRSContainerData;
                        break;
                    }
                }
            }
            return res;
        }

        private void DeselectActivities()
        {
            Mouse.SetCursor(Cursors.Wait);
            try
            {
                foreach (Control c in pnlActivitiesContainer.Children)
                {
                    if (c is CRSContainerData)
                    {
                        (c as CRSContainerData).IS_SELECTED = false;
                    }
                }
            }
            catch { }
            Mouse.SetCursor(Cursors.Arrow);
        }

        private void DeselectActivityLog_CRSContainerData()
        {
            Mouse.SetCursor(Cursors.Wait);
            try
            {
                foreach (Control c in pnlLogActivities.Children)
                {
                    if (c is CRSContainerData)
                    {
                        (c as CRSContainerData).IS_SELECTED = false;
                    }
                }
            }
            catch { }
            Mouse.SetCursor(Cursors.Arrow);
        }

        private void DeselectActivityLog()
        {
            DeselectActivityLog_CRSContainerData();
            DeselectActivityLog_ClearColorPnlLog();
            this.isModeSelectionStrongly = false;
        }

        private void DeselectActivityLog(bool deselectOnlyNotStrongly)
        {
            Mouse.SetCursor(Cursors.Wait);
            try
            {
                foreach (Control c in pnlLogActivities.Children)
                {
                    if (c is CRSContainerData)
                    {
                        if (!deselectOnlyNotStrongly || (deselectOnlyNotStrongly && !(c as CRSContainerData).IS_SELECTED_HIGH))
                            (c as CRSContainerData).IS_SELECTED = false;
                    }
                }
            }
            catch { }
            Mouse.SetCursor(Cursors.Arrow);
        }
        #endregion

        private long SaveActivity()
        {
            long residact = 0;
            try
            {
                int totminprev = 0;
                string status = cbStsIns.Text;
                int idx = cbStsIns.Text.IndexOf("-");
                if (idx >= 0)
                    status = cbStsIns.Text.Substring(idx + 1);

                string typeins = cbTypeIns.Text;
                idx = cbTypeIns.Text.IndexOf("-");
                if (idx >= 0)
                    typeins = cbTypeIns.Text.Substring(idx + 1);

                if (!string.IsNullOrEmpty(status))
                    status = status.Trim();
                if (!string.IsNullOrEmpty(typeins))
                    typeins = typeins.Trim();

                if (!string.IsNullOrEmpty(txtHPrev.Text) && Utils.IsNumber(txtHPrev.Text))
                    totminprev = Convert.ToInt32(txtHPrev.Text) * 60;

                long idact = this.ACTIVITY.IDActivity;
                if (mode == 1)
                    residact = DbDataUpdate.InsertActivity(0, 0, txtTextIns.Text, txtTitleIns.Text, DateTime.MinValue, DateTime.MinValue, DateTime.Now, totminprev, this.UserInfo.IDUser, status, typeins);
                else
                if (mode == 2)
                    residact = DbDataUpdate.UpdateActivity(0, idact, txtTextIns.Text, txtTitleIns.Text, totminprev, this.UserInfo.IDUser, status, typeins);

                DbActivity act = new DbActivity();
                act.IDActivity = residact;
                act.Status = status;
                act.TextActivity = txtTextIns.Text;
                act.TitleActivity = txtTitleIns.Text;
                act.TotMinPreview = totminprev;
                act.TypeActivity = typeins;
                act.IDUser = this.UserInfo.IDUser;
                act.DateTimeInserted = DateTime.Now;
                act.DateTimeLastMod = act.DateTimeInserted;
                MBBSyncroUtility.WriteActivitySyncroFile(act, this.UserInfo.Name, MBBCommon.syncroParams);
                //this.ActivitiesModToSend.Add(act);
            }
            catch { residact = -1; }

            return residact;
        }

        private List<string> GetStatus()
        {
            List<string> res = new List<string>();
            if (chkStsClose.IsChecked.Value) res.Add("Closed");
            if (chkStsDel.IsChecked.Value) res.Add("Deleted");
            if (chkStsStart.IsChecked.Value) res.Add("Started");
            if (chkStsWait.IsChecked.Value) res.Add("Waiting");
            return res;
            throw new NotImplementedException();
        }

        #region FILL
        private void FillActivities(List<string> status, DateTime? dt1, DateTime? dt2, string typeWhAndOr, bool fillNotes, bool fillPerc, int typefilter_0Ins1Mod, bool withnotes)
        {
            List<DbActivity> lst = new List<DbActivity>();
            if (typeWhAndOr.ToUpper().Contains("AND"))
                lst = DbDataAccess.GetActivities(this.UserInfo.IDUser, dt1, dt2, status, typefilter_0Ins1Mod, withnotes);
            else
            {
                List<DbActivity> lsttmp = DbDataAccess.GetActivities(this.UserInfo.IDUser, null, null, status, typefilter_0Ins1Mod, withnotes);
                lst = DbDataAccess.GetActivities(this.UserInfo.IDUser, dt1, dt2, null, typefilter_0Ins1Mod, withnotes);
                lst.AddRange(lsttmp.Where(Y => lst.Where(X => Y.TitleActivity == X.TitleActivity).Count() == 0));
                if (typefilter_0Ins1Mod == 0)
                    lst = lst.OrderByDescending(X => X.DateTimeInserted).ToList();
                else
                    lst = lst.OrderByDescending(X => X.DateTimeLastMod).ToList();
            }

            int idx = 0;
            pnlActivitiesContainer.Children.Clear();
            this.LST_NOTES.Clear();
            foreach (DbActivity a in lst)
            {
                DateTime dtheader = a.LastStartLogActivity;
                if (dtheader == null || dtheader != null && dtheader == DateTime.MinValue)
                    dtheader = a.StartActivity;
                CRSContainerData c = new CRSContainerData(idx, idx, null, new List<string> { a.TextActivity }, a.TitleActivity, "", dtheader
                     , "", false, true, true, true, this.UserInfo.Language, a.IDActivity, 0, 0);
                c.IS_FIXED_SIZE = false;
                c.SHOW_RESIZE = false;
                c.SetHeigth((int)c.GetAutoHeigthTitle(), false);

                DockPanel.SetDock(c, Dock.Top);
                c.SetEventSelect(DeselectActivities);
                c.SetEventDblClick(btnOpen_Click);
                c.SetEventGoToNotesClick(EventGoToNotes);

                if (fillNotes)
                {                   
                    List<DbNote> lstn = DbDataAccess.GetActivityNotes(a.IDActivity);
                    this.LST_NOTES.AddRange(lstn);
                    c.SHOW_PNLBTN_GOTONOTES = lstn.Count() > 0;
                    c.SHOW_ICO_DOCS = lstn.Where(X=>X.Docs!=null && X.Docs.Count()>0).Count() > 0;                  
                }
                // per poter fare un SetHeight che funzioni prma devo attaccare il controllo al children
                pnlActivitiesContainer.Children.Add(c);
                if (fillPerc)
                {
                    a.ListLog = DbDataAccess.GetActivityLogs(a.IDActivity);
                    double perc = 0;
                    if (a.TotMinPreview > 0)
                        perc = ((double)a.GetTotSecActivity() / (a.TotMinPreview * 60)) * 100;
                    
                    c.SHOW_TEXT = true;
                    c.SHOW_RESIZE = true;
                    c.InitExtraPerc(perc);
                    c.SetHeigth(0, true);
                }
                idx++;
            }
        }

        /// <summary>
        /// Calcola le empty activities dagli spazi fra un'attività e un'altra
        /// </summary>
        /// <param name="lstlogs"></param>
        public void FillEmptyActivity(List<DbActivityLog> lstlogs)
        {
            List<string> lstlbl = new List<string>();
            List<string> lsttxt = new List<string>();
            // tempo totale di attività non assegnate + percentuale sul totale: comincia a contare dal parametro 'Date from'
            lstlbl.Add("Tot time: ");
            // numero di giorni in cui le attività non assegnate si sono distribuite: tiene conto del parametro 'Working hour day'
            lstlbl.Add("Days interval: ");

            DateTime dt1 = dtpDateFromLogs.SelectedDate.Value;
            DateTime dt2 = dtpDateToLogs.SelectedDate.Value;
            // totale minuti non assegnati ad attività: 
            //    =>  tempo considerato totale - (minuti non da lavorare al giorno * num giorni) - minuti assegnati ad attività
            int mintowork = (int)dt2.Subtract(dt1).TotalMinutes -
                            ((24 - Convert.ToInt32(txtHoursDayLogs.Text)) * 60) * (int)dt2.Subtract(dt1).TotalDays;
            int minwork = (int)lstlogs.Where(Y => Y.StartActivityLog > dt1 && Y.StopActivityLog > DateTime.MinValue).Sum(X => X.StopActivityLog.Subtract(X.StartActivityLog).TotalMinutes);
            float totminempty = mintowork - minwork;

            float perc = 0;
            if (mintowork > 0) perc = (float)minwork / mintowork * 100;
            TimeSpan time = TimeSpan.FromMinutes(totminempty);

            lsttxt.Add(time.ToString(@"hh\:mm\:ss") + " (" + perc.ToString("0.00") + " % in total time registered)");
            lsttxt.Add(dt2.Subtract(dt1).TotalDays.ToString());
            ctrlContainerEmptyAct.Init(-2, lstlbl, lsttxt, "Empty activity", "", dtpDateFromLogs.SelectedDate.Value, "", false, true, true, this.UserInfo.Language);
        }

        /// <summary>
        ///  prende dal db le empty activity registrate
        /// </summary>
        /// <param name="lstlogs"></param>
        public void FillEmptyActivityFromEmpyInDb(List<DbActivityLog> lstlogs)
        {
            if (pnlLogActivitiesAndEmptyLog.RowDefinitions[1].Height.Value == 0)
            {
                pnlLogActivitiesAndEmptyLog.RowDefinitions[1].Height = new GridLength(100);
            }

            List<string> lstlbl = new List<string>();
            List<string> lsttxt = new List<string>();
            lstlbl.Add(CRSTranslation.TranslateDefaultWords(CRSTranslation.WRD_TOT_TIME, this.UserInfo.Language) + ": ");

            // totale minuti non assegnati ad attività: 
            //    =>  tempo considerato totale - (minuti non da lavorare al giorno * num giorni) - minuti assegnati ad attività
            int mintot = (int)lstlogs.Where(Y => Y.StopActivityLog > DateTime.MinValue)
                                     .Sum(X => X.StopActivityLog.Subtract(X.StartActivityLog).TotalMinutes);

            List<DbActivityLog> lstlogsempty = lstlogs.Where(X => X.IDActivity < 0).ToList();
            int minempty = (int)lstlogsempty.Where(Y => Y.StopActivityLog > DateTime.MinValue)
                                            .Sum(X => X.StopActivityLog.Subtract(X.StartActivityLog).TotalMinutes);

            float perc = 0;
            if (mintot > 0) perc = (float)minempty / mintot * 100;
            TimeSpan time = TimeSpan.FromMinutes(minempty);

            lsttxt.Add(time.ToString(@"hh\:mm\:ss") + " (" + perc.ToString("0.00") + " %)");
            //lsttxt.Add(Convert.ToString(lstlogs.Select(X=>X.StartActivityLog.DayOfYear).Count()));
            ctrlContainerEmptyAct.Init(-2, lstlbl, lsttxt, "Empty activity", "", dtpDateFromLogs.SelectedDate.Value, "", false, true, true, this.UserInfo.Language);
            ctrlContainerEmptyAct.LEVEL_1_10_FONT_STYLE_TEXT = 6;
        }

        /// <summary>
        /// recupera tutte le attività comprese le empty activities che registro come normali attività solo con idactivity<0
        /// </summary>
        /// <param name="lstlogs"></param>
        /// <returns></returns>
        public List<DbActivityLog> FillLogActivitiesWithEmpty(List<DbActivityLog> lstlogs, int typeLayout_1percdim_2percfont, int totalHeigthPanelContainer = 0)
        {
            List<DbActivityLog> res = new List<DbActivityLog>();

            if (totalHeigthPanelContainer == 0)
                totalHeigthPanelContainer = lstlogs.Count() * this.HGT_MED_CONTAINER_ACT;

            pnlLogActivities.Children.Clear();
            pnlLogActivities.RowDefinitions.Clear();
            pnlLogActivitiesListPanels.Children.Clear();
            try
            {
                List<long> lstidact = lstlogs.Select(X => X.IDActivity).Distinct().ToList();
                int idx = 0;
                List<string> lstheader = new List<string>();
                //int mintowork = (int)lstlogs.Where(Y => Y.StopActivityLog > DateTime.MinValue).Sum(X => X.StopActivityLog.Subtract(X.StartActivityLog).TotalMinutes);

                lstheader.Add(CRSTranslation.TranslateDefaultWords(CRSTranslation.WRD_INTERVAL, this.UserInfo.Language));

                List<DbActivityLog> lstlogstmp = lstlogs.ToList();
                foreach (DbActivityLog log in lstlogs)
                {
                    CRSContainerData c = new CRSContainerData();
                    // devo identificare il log in maniera univoca per poterlo ricercare successivamente
                    c.SetEventDblClick(EvnDblCliclLog);
                    c.ID = log.IDActivity;
                    c.SEQ = idx;

                    double hgt;  float perc;
                    SetContainerControlActivityLog(c, typeLayout_1percdim_2percfont, totalHeigthPanelContainer, lstidact.IndexOf(log.IDActivity), log, lstlogs, out hgt, out perc);

                    MakeControlInGridAndPanel(c, hgt, perc);
                    res.Add(log);
                    idx++;
                }
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog(ex.Message, "debug user");
            }
            return res;
        }

        private void SetContentCurrentActivity()
        {           
            // hhmmss = time.ToString(@"hh\:mm\:ss");
            double totsec = this.ACTIVITY.GetTotSecActivity();
            int hh = Convert.ToInt32(Math.Truncate((double) totsec / 60 / 60));
            int mm = Convert.ToInt32(Math.Truncate((double)(totsec - hh * 60 * 60) / 60));
            int ss = Convert.ToInt32(Math.Truncate((double)(totsec - hh * 60 * 60 - mm * 60)));
            double perc = 0;
            if (this.ACTIVITY.TotMinPreview > 0)
                perc = ((double) totsec / (this.ACTIVITY.TotMinPreview * 60)) * 100;

            ctrlCurrentActivity.Init(-1, new List<string>(), new List<string> {
                                                                    "Time elapsed: " + GetHourFormatted(hh,mm,ss,perc),
                                                                    this.ACTIVITY.TextActivity }
                                            , this.ACTIVITY.TitleActivity, "", this.ACTIVITY.StartActivity, ""
                                            , false, true, true, this.UserInfo.Language);
            pbPerc.Value = perc;
            if (perc > 80) pbPerc.Foreground = Brushes.Yellow;
            if (perc > 100) pbPerc.Foreground = Brushes.Red;
            txtPercProgBar.Text = perc.ToString("0.0") + "%";
            this.ACTIVITY.PercProgress = perc;
        }
        #endregion

        private void SetContainerControlActivityLog(CRSContainerData c, int typeLayout_1percdim_2percfont, int totalHeigthPanelContainer, long indexofact,  DbActivityLog log, List<DbActivityLog> lstlogs, out double hgt, out float perc)
        {
            List<string> lstheader = new List<string>(); 
            //lstheader.Add(CRSTranslation.TranslateDefaultWords(CRSTranslation.WRD_INTERVAL, this.UserInfo.Language));

            int mintowork = (int)lstlogs.Where(Y => Y.StopActivityLog > DateTime.MinValue).Sum(X => X.StopActivityLog.Subtract(X.StartActivityLog).TotalMinutes);

            int totminact = 0;
            if (log.StopActivityLog > DateTime.MinValue)
                totminact = (int)log.StopActivityLog.Subtract(log.StartActivityLog).TotalMinutes;
            perc = 0;
            if (mintowork > 0) perc = ((float)totminact / mintowork) * 100;

            List<string> lstval = new List<string>();
            /*lstval.Add(TimeSpan.FromMinutes(log.StopActivityLog.Subtract(log.StartActivityLog).TotalMinutes).ToString(@"hh\:mm") +
                        " (" + perc.ToString("0.00") + "%), " +
                        log.StartActivityLog.ToShortDateString() + " " + log.StartActivityLog.ToShortTimeString() + " - " +
                        log.StopActivityLog.ToShortTimeString()
                       );*/

            lstval.Add("       " + TimeSpan.FromMinutes(log.StopActivityLog.Subtract(log.StartActivityLog).TotalMinutes).ToString(@"hh\:mm") +
                        " " + perc.ToString("0.00") + "%");

            long idxmod = indexofact;
            if (log.IDActivity < 0) idxmod = -2;
            if (log.StopActivityLog.Subtract(log.StartActivityLog).TotalMinutes <= 0) idxmod = -9;
            string info2 = log.StopActivityLog.ToShortTimeString();
            c.Init(idxmod, lstheader, lstval, log.TitleActivity, "", log.StartActivityLog, info2, false, true, true);
            c.DATE2 = log.StopActivityLog;
            c.InitExtraPerc(perc);
            c.SetEventGoToNotesClick(EventGoToNotesWithDate);
            if (this.LST_NOTES.Count > 0 && c.DATE2 != null && c.DATE2 > DateTime.MinValue)
            {
                List<DbNote> lsttmp = this.LST_NOTES.Where(X => X.DateTimeLastMod >= c.DATE && X.DateTimeLastMod <= c.DATE2).ToList();
                c.SHOW_PNLBTN_GOTONOTES = lsttmp.Count()>0;               
                c.SHOW_ICO_DOCS = lsttmp.Where(X=> X.Docs!=null && X.Docs.Count()>0).Count()>0;
            }

            c.Height = Double.NaN;
            hgt = c.GetAutoHeigthTitle();
            if (typeLayout_1percdim_2percfont == 1)
                hgt = (double)perc * totalHeigthPanelContainer / 100;
            if (hgt < 40)
                hgt = 40;

            if (typeLayout_1percdim_2percfont == 2)
            {
                c.LEVEL_1_10_FONT_STYLE_TEXT = (int)perc / 10;
                hgt = c.GetAutoHeigthTitleText();
            }
            if (typeLayout_1percdim_2percfont == 1)
            {
                //c.SHOW_RESIZE = false;
                c.IS_FIXED_SIZE = true;
            }
        }

        private void EvnDblCliclLog(object sender, RoutedEventArgs e)
        {
            CRSContainerData c = sender as CRSContainerData;
            SelectStronglyActivityLog(null, c);
        }

        private void MakeControlInGridAndPanel(CRSContainerData activity, double heigthact, double perc)
        {
            Grid container = pnlLogActivities;
            container.RowDefinitions.Add(new RowDefinition());
            int idx1 = container.RowDefinitions.Count - 1;
            container.RowDefinitions.Add(new RowDefinition());
            int idx2 = container.RowDefinitions.Count - 1;
            container.RowDefinitions[idx1].Height = new GridLength(heigthact/*, GridUnitType.Star*/);
            container.RowDefinitions[idx2].Height = new GridLength();
            container.Tag = idx1;

            activity.VerticalAlignment = VerticalAlignment.Stretch;
            activity.HorizontalAlignment = HorizontalAlignment.Stretch;
            activity.SetEventSelect(DeselectActivityLog);

            GridSplitter split = new GridSplitter();
            split.Height = 5;
            split.Background = Brushes.Transparent;
            split.VerticalAlignment = VerticalAlignment.Center;
            split.HorizontalAlignment = HorizontalAlignment.Stretch;
            split.Tag = idx1;
            split.DragCompleted += GridSplitter_DragCompleted;
            split.DragStarted += GridSplitter_DragStarted;

            container.Children.Add(activity);
            Grid.SetRow(activity, idx1);
            container.Children.Add(split);
            Grid.SetRow(split, idx2);

            if (perc > 0)
            {
                Border b = new Border();
                b.BorderThickness = new Thickness(1);
                b.BorderBrush = Brushes.LightGray;
                b.Margin = new Thickness(0, 2, 2, 2);
                b.MouseEnter += BorderPanel_MouseEnter;
                b.MouseLeave += BorderPanel_MouseLeave;
                b.MouseDown += BorderPanel_MouseDown;
                b.Background = activity.GetBackgroundColorCurrent();
                b.ToolTip = activity.TITLE + "\n" + activity.TEXT;
                b.Tag = activity.ID.ToString() + "|" + Convert.ToDateTime(activity.DATE).ToString();
                // Width="10" HorizontalAlignment="Left" ToolTip="tot min: "
                DockPanel d = new DockPanel();
                d.HorizontalAlignment = HorizontalAlignment.Left;
                d.ToolTip = activity.TITLE + "\n" + activity.TEXT;
                d.Width = (double)(400 - 4 - this.LST_LOGS.Count() * 2) * perc / 100;

                if (d.Width < 7)
                    d.Width = 7;

                b.Child = d;
                pnlLogActivitiesListPanels.Children.Add(b);
            }
        }

        private CRSContainerData FindActivity(long idact, DateTime startact)
        {
            CRSContainerData res = null;
            foreach (Control c in pnlLogActivities.Children)
            {
                if (c is CRSContainerData)
                {
                    CRSContainerData cc = c as CRSContainerData;
                    if (cc.ID == idact && cc.DATE == startact)
                    {
                        res = cc;
                        break;
                    }
                }
            }
            return res;
        }
        #endregion

        #region TIMERS
        //-- Set and start the timer
        public void SetTimers()
        {
            timerActivity.Tick += new EventHandler(dispatcherTimer_Tick);
            timerActivity.Interval = new TimeSpan(0, 0, 10);
        }

        /// <summary>
        /// Esegue auto save, allinea i documenti e salva le note condivise (da file di testo) alla scadenza di this.RefreshTimer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            //-- se l'applicazione non si sta inizializzando (cioè sta partendo)
            if (this.ACTIVITY.IDActivity > 0 && this.ACTIVITY.TotMinPreview * 60 > this.ACTIVITY.GetTotSecActivity())
            {
                // fai qualcosa, evidenzia in base ai parametri in modo più o meno evidente
                SetContentCurrentActivity();
            }
        }

        private string GetHourFormatted(int hh, int mm, int ss, double perc)
        {
            string res = "";
            res = perc.ToString("0.00") +
                       "% (" + hh.ToString().PadLeft(2, '0') +
                       ":" + mm.ToString().PadLeft(2, '0') +
                       ":" + ss.ToString().PadLeft(2, '0') +
                       ")";
            return res;
        }

        #endregion

        #region EVENTS in LIST ACTIVITIES
        private void imgExit_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            if (this.eventHide != null)
                eventHide();
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            this.MODE = 0;
            // metti l'attività nel pannello dell'attività in primo piano in modo da poterla avviare
            // col tasto play
            // deve comparire in un CRSContainerData, a fianco c'è il pannello coi tasti di avvio
            // se non c'è niente nel pannello apare il testo: Cosa stai facendo?
            OpenActivity(this.GetSelectedActivity().ID);
        }

        private void btnAddAct_Click(object sender, RoutedEventArgs e)
        {
            // aggiunge un'attività nel db
            // nel container in basso compaiono i campi da compilare: titolo e testo come per le note
            // a fianco ci devono essere i tasti per salvare o cancellare
            pnlContainerNewCurrentAct.RowDefinitions[0].Height = new GridLength(230, GridUnitType.Star);
            pnlContainerNewCurrentAct.RowDefinitions[1].Height = new GridLength(0);

            cbStsIns.SelectedIndex = 0;
            cbTypeIns.SelectedIndex = -1;
            txtTextIns.Text = "";
            txtTitleIns.Text = "";
            txtHPrev.Text = "40";
            this.MODE = 1;
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            CRSContainerData c = this.GetSelectedActivity();

            if (c != null && c.ID > 0)
            {
                // tasto per eliminare un'attività dal db
                MessageBoxResult r = MessageBox.Show(CRSTranslation.TranslateDialog(CRSTranslation.MSG_DEL_ITEMS, this.UserInfo.Language) +
                                        "\n(.." + c.TITLE + ")",
                                       CRSTranslation.TranslateDialog(CRSTranslation.MSG_WARNING, this.UserInfo.Language), MessageBoxButton.YesNo);

                if (r == MessageBoxResult.Yes)
                {
                    //foreach (CRSContainerData c in this.GetSelectedActivities())
                    {
                        DbDataUpdate.UpdateActivityStatus(c.ID, this.UserInfo.IDUser, "Deleted");
                    }
                    FillActivitiesParams(false);
                }
            }
            else
                MessageBox.Show(CRSTranslation.TranslateDialog(CRSTranslation.MSG_HAVE_TO_SEL_ITEM, this.UserInfo.Language));
        }

        private void btnSyncroLocal_Click(object sender, RoutedEventArgs e)
        {
            // serve per aggiornare l'attività importate da un file excel quando questa funzionalità esisterà
        }

        private void btnShowFilter_Click(object sender, RoutedEventArgs e)
        {
            if (pnlGridMain.RowDefinitions[0].Height.Value <= 40)
            {
                //pnlFilters.Visibility = Visibility.Collapsed;
                pnlGridMain.RowDefinitions[0].Height = new GridLength(138);
            }
            else
            {
                //pnlFilters.Visibility = Visibility.Visible;
                pnlGridMain.RowDefinitions[0].Height = new GridLength(34);
            }
        }

        private void btnFind_Click(object sender, RoutedEventArgs e)
        {
            FillActivitiesParams(false);
            /*btnFind.Tag = Convert.ToInt32(btnFind.Tag) + 1;
            if (Convert.ToInt32(btnFind.Tag) == 2)
            {
                btnFind.Tag = 0;
                btnFind.LabelText = ">";
            }
            else btnFind.LabelText = ">>";*/
        }

        private void btnFindPerc_Click(object sender, RoutedEventArgs e)
        {
            FillActivitiesParams(true);
        }

        public void FillActivitiesParams(bool findWithPerc)
        {
            DateTime? dt1 = null;
            DateTime? dt2 = null;
            if (dtFilter1.SelectedDate != null && dtFilter1.SelectedDate.Value > DateTime.MinValue)
                dt1 = dtFilter1.SelectedDate.Value;
            // aggiungo un giorno dato cha altrimenti escludo la data visualizzata (es il giorno 27 è in realtà il 28 - 1 minuto)
            if (dtFilter2.SelectedDate != null && dtFilter2.SelectedDate.Value > DateTime.MinValue)
                dt2 = dtFilter2.SelectedDate.Value.AddDays(1);

            int typeord = (rbFindIns.IsChecked.Value ? 0 : 1);

            bool withnotes = chkStsWithNotes.IsChecked.Value;

            List<string> sts = this.GetStatus();
            FillActivities(sts, dt1, dt2, "AND", true, findWithPerc, typeord, withnotes);//Convert.ToInt32(btnFind.Tag) == 1);
        }

        private void imgOk_MouseDown(object sender, MouseButtonEventArgs e)
        {
            long id = SaveActivity();
            FillActivitiesParams(true);
            this.MODE = 0;
            OpenActivity(id);
        }

        private void imgCanc_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.MODE = -1;
        }

        private void imgBlackBoxPlay_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // cambia icona
            try
            {              
                // registra start activity
                this.StartStopActivity();
                if (this.eventStartActicity != null)
                    this.eventStartActicity();
            }
            catch { }
        }

        private void SetImgStartStopActivity(bool isstarted)
        {
            //if (Convert.ToInt32(imgPlay.Tag) == 0)
            if(!isstarted)
            {
                imgPlay.Source = CRSImage.CreateImage(CRSGlobalParams.ImageStartPathName);
                imgPlay.Tag = 1;
                pnlCurrentActLbl.Background = Brushes.AliceBlue;
            }
            else
            {
                imgPlay.Source = CRSImage.CreateImage(CRSGlobalParams.ImageStopPathName);
                imgPlay.Tag = 0;
                pnlCurrentActLbl.Background = Brushes.LightSkyBlue;
            }
        }

        private void btnModAct_Click(object sender, RoutedEventArgs e)
        {
            pnlContainerNewCurrentAct.RowDefinitions[0].Height = new GridLength(200, GridUnitType.Star);
            pnlContainerNewCurrentAct.RowDefinitions[1].Height = new GridLength(0);

            DbActivity a = DbDataAccess.GetActivity(this.GetSelectedActivity().ID);
            Utils.CopyObjectToObject(a, this.ACTIVITY);

            cbStsIns.SelectedIndex = cbStsIns.Items.IndexOf(a.Status);
            cbTypeIns.SelectedIndex = cbTypeIns.Items.IndexOf(a.TypeActivity);
            //cbStsIns.SelectedItem = a.Status;
            txtTextIns.Text = a.TextActivity;
            txtTitleIns.Text = a.TitleActivity;
            txtHPrev.Text = ((float)a.TotMinPreview / 60).ToString("0.00");
            this.MODE = 2;
        }

        private void btnNote_Click(object sender, RoutedEventArgs e)
        {
            // associa una nota all'activity
            //  1) apri nuova nota
            //  2) quando salvi nota scrivi in ACTIVITY_NOTE
            if (this.eventAddNote != null)
                this.eventAddNote();
        }

        private void imgResize_MouseDown(object sender, MouseButtonEventArgs e)
        {           
            imgResize.Tag = Convert.ToInt32(imgResize.Tag) + 1;           
            if (Convert.ToInt32(imgResize.Tag) == 4)
                imgResize.Tag = 0;

            SetDimPos(Convert.ToInt32(imgResize.Tag), 0);
        }

        private void dtFilter1_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((dtFilter1.SelectedDate != null && dtFilter1.SelectedDate > DateTime.MinValue) ||
               (dtFilter2.SelectedDate != null && dtFilter2.SelectedDate > DateTime.MinValue) &&
                chkStsWait.IsChecked.Value)
            {
                chkStsStart.IsChecked = true;
            }
        }
        #endregion

        #region EVENTS in REPORT
        private List<DbActivityLog> LST_LOGS = new List<DbActivityLog>();
        private void btnSetParamsLog_Click(object sender, RoutedEventArgs e)
        {
            this.LST_LOGS = DbDataAccess.GetActivityLogs(dtpDateFromLogs.SelectedDate.Value, dtpDateToLogs.SelectedDate.Value, false);

            if (Convert.ToInt32(btnSetParamsLog.Tag) == 0)
            {
                btnSetParamsLog.Tag = 2;
                btnSetParamsLog.LabelText = ">>";
            }
            else
            {
                btnSetParamsLog.Tag = 0;
                btnSetParamsLog.LabelText = ">";
            }
            FillEmptyActivityFromEmpyInDb(this.LST_LOGS);
            FillLogActivitiesWithEmpty(this.LST_LOGS, Convert.ToInt32(btnSetParamsLog.Tag));
            SetLayoutOkCancLogBtns(0, false);
        }

        private void btnResizeLog_Click(object sender, RoutedEventArgs e)
        {            
            if (this.LST_LOGS.Count() == 0)
                this.LST_LOGS = DbDataAccess.GetActivityLogs(dtpDateFromLogs.SelectedDate.Value, dtpDateToLogs.SelectedDate.Value, false);

            FillEmptyActivityFromEmpyInDb(this.LST_LOGS);
            FillLogActivitiesWithEmpty(this.LST_LOGS, 1);
            SetLayoutOkCancLogBtns(1, false);
        }

        private void btnChangeLog_Click(object sender, RoutedEventArgs e)
        {
            List<string> status = new List<string>();
            status.Add("Started");
            status.Add("Waiting");
            int typeord = (rbFindIns.IsChecked.Value ? 0 : 1);
            List<DbActivity> lsttmp = DbDataAccess.GetActivities(this.UserInfo.IDUser, null, null, status, typeord, chkStsWithNotes.IsChecked.Value);
            //cbListActivitiesChg.ItemsSource = new List<Tuple<string, long>>(lsttmp.Select(X => new Tuple<string,long>(X.TitleActivity, X.IDActivity)).ToList());
            cbListActivitiesChg.ItemsSource = new List<string>(lsttmp.Select(X => X.TitleActivity).ToList());

            CRSContainerData c = this.GetSelectedStronglyLogActivity_CRSContainerData();
            DbActivityLog l = this.LST_LOGS.Where(X => X.IDActivity == c.ID && X.StartActivityLog == c.DATE).FirstOrDefault();
            if (c != null)
            {
                cbListActivitiesChg.SelectedItem = c.TITLE;
                dtpDateFromLogsIns.SelectedDateTime = c.DATE;
                if (l != null)
                    dtpDateToLogsIns.SelectedDateTime = l.StopActivityLog;
            }
            SetLayoutOkCancLogBtns(2, false);
        }

        private void btnInsLog_Click(object sender, RoutedEventArgs e)
        {
            List<string> status = new List<string>();
            status.Add("Started");
            status.Add("Waiting");
            int typeord = (rbFindIns.IsChecked.Value ? 0 : 1);
            List<DbActivity> lsttmp = DbDataAccess.GetActivities(this.UserInfo.IDUser, null, null, status, typeord, chkStsWithNotes.IsChecked.Value);
            cbListActivitiesChg.ItemsSource = new List<string>(lsttmp.Select(X => X.TitleActivity).ToList());
            SetLayoutOkCancLogBtns(3, false);
        }

        private void TabItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (dtpDateFromLogs.SelectedDate == null || (dtpDateFromLogs.SelectedDate != null && dtpDateFromLogs.SelectedDate.Value == DateTime.MinValue))
            {
                dtpDateFromLogs.SelectedDate = DateTime.Now.AddDays(-3);
                dtpDateToLogs.SelectedDate = DateTime.Now.AddDays(1);
            }
            this.LST_LOGS = DbDataAccess.GetActivityLogs(dtpDateFromLogs.SelectedDate.Value, dtpDateToLogs.SelectedDate.Value, false);
            FillEmptyActivityFromEmpyInDb(this.LST_LOGS);

            FillLogActivitiesWithEmpty(this.LST_LOGS, Convert.ToInt32(btnSetParamsLog.Tag));
            SetLayoutOkCancLogBtns(0, false);
        }

        private int heigthRowInit = 0;
        private int currentLogActivityTag = -1;

        List<Tuple<DbActivityLog, DbActivityLog>> LST_LOG_MOD = new List<Tuple<DbActivityLog, DbActivityLog>>();
        private void GridSplitter_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            //if (pnlRepOkCanc.Visibility == Visibility.Visible)
            if (Convert.ToInt32(pnlRepOkCanc.Tag) == 1)
            {
                SetLayoutOkCancLogBtnsBeforeSave(Convert.ToInt32(pnlRepOkCanc.Tag),2);

                int tag = Convert.ToInt32((sender as GridSplitter).Tag);
                this.currentLogActivityTag = tag;
                int heigthRowFinal = 0;
                double incperc = 0;
                try
                {
                    if (pnlLogActivities.Children.Count > tag)
                        heigthRowFinal = (int)pnlLogActivities.RowDefinitions[tag].Height.Value;

                    CRSContainerData c1 = null;
                    CRSContainerData c2 = null;
                    if (pnlLogActivities.Children.Count > tag && pnlLogActivities.Children[tag] is CRSContainerData)
                        c1 = pnlLogActivities.Children[tag] as CRSContainerData;
                    if (pnlLogActivities.Children.Count > tag + 2 && pnlLogActivities.Children[tag + 2] is CRSContainerData)
                        c2 = pnlLogActivities.Children[tag + 2] as CRSContainerData;

                    if (c1 != null && c2 != null)
                    {

                        DbActivityLog log1 = this.LST_LOGS.Where(X => X.IDActivity == c1.ID && X.StartActivityLog == c1.DATE).FirstOrDefault();
                        DbActivityLog log2 = this.LST_LOGS.Where(X => X.IDActivity == c2.ID && X.StartActivityLog == c2.DATE).FirstOrDefault();

                        DbActivityLog tmp1 = new DbActivityLog();
                        DbActivityLog tmp2 = new DbActivityLog();
                        Utils.CopyObjectToObject(log1, tmp1);
                        Utils.CopyObjectToObject(log2, tmp2);
                        this.LST_LOG_MOD.Add(new Tuple<DbActivityLog, DbActivityLog>(tmp1, tmp2));

                        int totsec = (int)log1.StopActivityLog.Subtract(log1.StartActivityLog).TotalSeconds +
                                     (int)log2.StopActivityLog.Subtract(log2.StartActivityLog).TotalSeconds;
                        int totsec1 = (int)log1.StopActivityLog.Subtract(log1.StartActivityLog).TotalSeconds;
                        int totsec2 = (int)log2.StopActivityLog.Subtract(log2.StartActivityLog).TotalSeconds;
                        // calcolo la percentuale incrementata o decrementata rispetto alla prima attività (senza usare l'altezza della riga della griglia)
                        // il calcolo potrebbe essere più preciso dato che dovrei calcolare la percentuale sul totale del tempo
                        incperc = ((double)(heigthRowFinal - heigthRowInit) / heigthRowInit) * 100;

                        int newtotsec1 = totsec1 + (int)(totsec1 * incperc / 100);
                        int newtotsec2 = totsec - newtotsec1;//(int)(totsec2 * (100 - incperc) / 100);

                        if (newtotsec2 < 0)
                        {
                            newtotsec1 = totsec;
                            newtotsec2 = 0;
                        }
                        if (newtotsec1 < 0)
                        {
                            newtotsec2 = totsec;
                            newtotsec1 = 0;
                        }

                        log1.StopActivityLog = log1.StartActivityLog.AddSeconds(newtotsec1);
                        log2.StartActivityLog = log2.StopActivityLog.AddSeconds(-newtotsec2);
                        this.LST_LOG_MOD.Add(new Tuple<DbActivityLog, DbActivityLog>(log1, log2));

                        FillEmptyActivityFromEmpyInDb(this.LST_LOGS);
                        //FillLogActivitiesWithEmpty(this.LST_LOGS, 1);

                        int idx1 = this.LST_LOGS.Select(X => X.IDActivity).Distinct().ToList().IndexOf(log1.IDActivity);
                        int idx2 = this.LST_LOGS.Select(X => X.IDActivity).Distinct().ToList().IndexOf(log2.IDActivity);
                        double hgt; float perc;
                        SetContainerControlActivityLog(c1, 1, LST_LOGS.Count() * this.HGT_MED_CONTAINER_ACT, idx1, log1, this.LST_LOGS, out hgt, out perc);
                        SetContainerControlActivityLog(c2, 1, LST_LOGS.Count() * this.HGT_MED_CONTAINER_ACT, idx2, log2, this.LST_LOGS, out hgt, out perc);
                        //FindActivity(log1.IDActivity, log1.StartActivityLog).IS_SELECTED = true;
                    }
                }
                catch (Exception ex)
                {
                    CRSLog.WriteLog(ex.Message, this.UserInfo.Name);
                }
            }
        }

        private void GridSplitter_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            // nascondo i tasti di salvataggio
            int tag = Convert.ToInt32((sender as GridSplitter).Tag);
            if (pnlLogActivities.Children.Count > tag)
            {
                heigthRowInit = (int)pnlLogActivities.RowDefinitions[tag].Height.Value;
                this.LST_LOG_MOD.Clear();
            }
        }

        private void imgRepCanc_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.LST_LOG_MOD.Clear();
            FillLogActivitiesWithEmpty(this.LST_LOGS, 2);
            SetLayoutOkCancLogBtns(0, false);
        }

        private void imgRepOk_MouseDown(object sender, MouseButtonEventArgs e)
        {
            bool res = false;
            if (Convert.ToInt32(pnlRepOkCanc.Tag) == 1)
            {
                res = SaveResizeLogs();
                FillLogActivitiesWithEmpty(this.LST_LOGS, 1);
                
            }
            else
            if (Convert.ToInt32(pnlRepOkCanc.Tag) == 2)
            {
                res = SaveChangeLog();
                FillLogActivitiesWithEmpty(this.LST_LOGS, 2);
            }
            else
            if (Convert.ToInt32(pnlRepOkCanc.Tag) == 3)
            {
                res = SaveInsLog();
                FillLogActivitiesWithEmpty(this.LST_LOGS, 2);
            }
           
            if (!res)
                MessageBox.Show(CRSTranslation.TranslateDialog(CRSTranslation.MSG_DATA_NOT_SAVED, this.UserInfo.Language));

            if (pnlLogActivities.Children.Count > this.currentLogActivityTag)
                ScrollIntoContainer(pnlLogActivities.Children[this.currentLogActivityTag] as CRSContainerData);
            SetLayoutOkCancLogBtns(Convert.ToInt32(pnlRepOkCanc.Tag), true);
        }

        private void SetLayoutOkCancLogBtns(int tagpnlRepOkCanc, bool execOk)
        {
            btnChangeLog.IsEnabled = true;
            btnInsLog.IsEnabled = true;
            btnResizeLog.IsEnabled = true;
            pnlChgInsLog.Visibility = Visibility.Visible;
            pnlLogActivitiesAndEmptyLog.RowDefinitions[0].Height = new GridLength(65);
            // nessuna modifica in atto
            if (tagpnlRepOkCanc == 0)
            {
                if(Convert.ToInt32(btnSetParamsLog.Tag)==0)
                    txtInfoActivityLogOp.Text = "View list logs titles";
                else txtInfoActivityLogOp.Text = "View list logs text proportional";
                this.isModeSelectionStrongly = false;
                btnChangeLog.IsEnabled = true;
                btnInsLog.IsEnabled = true;
                btnResizeLog.IsEnabled = true;
                pnlRepOkCanc.Tag = 0;
                pnlRepOkCanc.Visibility = Visibility.Hidden;
                pnlChgInsLog.Visibility = Visibility.Hidden;
            }
            // resize intervallo: si attivano i bottoni solo quando si fa drag su activity da ridimensionare
            if (tagpnlRepOkCanc == 1)
            {
                txtInfoActivityLogOp.Text = "Resize interval..";
                btnChangeLog.IsEnabled = false;
                btnInsLog.IsEnabled = false;
                btnResizeLog.IsEnabled = true;

                pnlRepOkCanc.Tag = 1;
                pnlRepOkCanc.Visibility = Visibility.Visible;
                pnlRepOkCanc.IsEnabled = false;
                pnlRepOkCanc.Opacity = 0.5;
            }
            // cambio activity
            if (tagpnlRepOkCanc == 2)
            {
                txtInfoActivityLogOp.Text = "Change activity..";
                btnChangeLog.IsEnabled = true;
                btnInsLog.IsEnabled = false;
                btnResizeLog.IsEnabled = false;

                pnlRepOkCanc.Tag = 2;
                pnlRepOkCanc.Visibility = Visibility.Visible;
                pnlRepOkCanc.IsEnabled = false;
                pnlRepOkCanc.Opacity = 0.5;

                // mostra combo con lista activities attuali
                pnlLogActivitiesAndEmptyLog.RowDefinitions[0].Height = new GridLength(165);
                dtpDateFromLogsIns.IsEnabled = false;
                dtpDateToLogsIns.IsEnabled = false;

            }
            // add activity
            if (tagpnlRepOkCanc == 3)
            {
                txtInfoActivityLogOp.Text = "Add new activity log..";
                btnChangeLog.IsEnabled = false;
                btnInsLog.IsEnabled = true;
                btnResizeLog.IsEnabled = false;

                pnlRepOkCanc.Tag = 3;
                pnlRepOkCanc.Visibility = Visibility.Visible;
                pnlRepOkCanc.IsEnabled = false;               
                pnlRepOkCanc.Opacity = 0.5;

                // mostra combo con lista activities attuali
                pnlLogActivitiesAndEmptyLog.RowDefinitions[0].Height = new GridLength(165);
                dtpDateFromLogsIns.IsEnabled = true;
                dtpDateToLogsIns.IsEnabled = true;

            }

            if (execOk)
            {
                //pnlLogActivitiesAndEmptyLog.RowDefinitions[0].Height = new GridLength(65);
                pnlRepOkCanc.IsEnabled = false;
                pnlRepOkCanc.Opacity = 0.5;
            }
        }
        /// <summary>
        /// Imposta il layout dei bottoni di salvataggio o cancellazione delle modifiche: il secondo parametro (type_event_1selstrongly_2enddrag)
        /// indica la provenienza dell'evento da cui devo usare questo metodo.
        /// Tipi di tag:
        /// 1: resize intervallo log
        /// 2: sostituzione id activity del log
        /// 3: inserimento nuovo log
        /// </summary>
        /// <param name="tagpnlRepOkCanc"></param>
        /// <param name="typeevent_1selstrongly_2enddrag"></param>
        private void SetLayoutOkCancLogBtnsBeforeSave(int tagpnlRepOkCanc, int typeevent_1selstrongly_2enddrag)
        {
            pnlRepOkCanc.IsEnabled = false;
            pnlRepOkCanc.Opacity = 0.5;
            // 2: sostituzione id activity del log
            // 3: inserimento nuovo log
            if ((tagpnlRepOkCanc == 2 || tagpnlRepOkCanc == 3) && typeevent_1selstrongly_2enddrag == 1)
            {
                if (this.isModeSelectionStrongly)
                {
                    pnlRepOkCanc.IsEnabled = true;
                    pnlRepOkCanc.Opacity = 1;
                }
            }
            // 1: resize intervallo log
            if (tagpnlRepOkCanc == 1 && typeevent_1selstrongly_2enddrag == 2)
            {
                pnlRepOkCanc.IsEnabled = true;
                pnlRepOkCanc.Opacity = 1;
            }
        }

        private bool SaveChangeLog()
        {
            bool res = false;
            CRSContainerData c = this.GetSelectedStronglyLogActivity_CRSContainerData();
            if (c != null && cbListActivitiesChg.SelectedItem != null)
            {
                string title = Convert.ToString(cbListActivitiesChg.SelectedItem);
                long newid = DbDataUpdate.ChangeActivityLog(c.ID, Convert.ToDateTime(c.DATE), title);
                res = true;

                // cancello la vecchia activity e inserisco la nuova sul file di sincronizzazione
                WriteLogSyncro("D", c.ID, Convert.ToDateTime(c.DATE), Convert.ToDateTime(c.DATE), Convert.ToDateTime(c.DATE2));
                WriteLogSyncro("U", newid, Convert.ToDateTime(c.DATE), Convert.ToDateTime(c.DATE), Convert.ToDateTime(c.DATE2));

            }
            else
            {
                MessageBox.Show(CRSTranslation.TranslateDialog(CRSTranslation.MSG_HAVE_TO_SEL_ITEM, this.UserInfo.Language));
            }
            return res;
        }

        private bool SaveInsLog()
        {
            bool res = false;
            DbActivity l = DbDataAccess.GetActivity(Convert.ToString(cbListActivitiesChg.SelectedItem));
            if (l != null && l.IDActivity != 0)
            {
                res = DbDataUpdate.InsertActivityLog(l.IDActivity, Convert.ToDateTime(dtpDateFromLogsIns.SelectedDateTime), Convert.ToDateTime(dtpDateToLogsIns.SelectedDateTime));
                if (!res)
                {
                    MessageBox.Show(CRSTranslation.TranslateDialog(CRSTranslation.MSG_OPERATION_NOT_ALLOWED, this.UserInfo.Language) +
                        "\n" + CRSTranslation.TranslateDialog(CRSTranslation.MSG_CHECK_PARAMETERS, this.UserInfo.Language));
                }
                else {
                    WriteLogSyncro("U", l.IDActivity, Convert.ToDateTime(dtpDateFromLogsIns.SelectedDateTime), Convert.ToDateTime(dtpDateFromLogsIns.SelectedDateTime), Convert.ToDateTime(dtpDateToLogsIns.SelectedDateTime));
                }
                
            }
            return res;
        }

        private bool SaveResizeLogs()
        {
            bool res = false;
            if (this.currentLogActivityTag >=0 &&
                pnlLogActivities.Children.Count > this.currentLogActivityTag + 2 &&
                pnlLogActivities.Children[this.currentLogActivityTag] is CRSContainerData &&
                pnlLogActivities.Children[this.currentLogActivityTag + 2] is CRSContainerData)
            {

                if (this.LST_LOG_MOD.Count() > 1)
                {
                    DbActivityLog log1 = this.LST_LOG_MOD.First().Item1;
                    DbActivityLog log2 = this.LST_LOG_MOD.First().Item2;//this.LST_LOGS.Where(X => X.IDActivity == c2.ID && X.StartActivityLog == c2.DATE).FirstOrDefault();
                    DbActivityLog log1new = this.LST_LOG_MOD.Last().Item1;
                    DbActivityLog log2new = this.LST_LOG_MOD.Last().Item2;

                    MessageBoxResult r = MessageBox.Show(
                        CRSTranslation.TranslateDialog(CRSTranslation.MSG_RESIZE_LOG_ACTIVITY, this.UserInfo.Language) + 
                        "\n 1: " + log1new.TitleActivity + 
                        "\n >> " + log1new.StartActivityLog.ToShortDateString() + " " + log1new.StartActivityLog.ToShortTimeString() +
                        " - "    + log1new.StopActivityLog.ToShortDateString()  + " " + log1new.StopActivityLog.ToShortTimeString() +
                        "\n 2: " + log2new.TitleActivity +
                        "\n >> " + log2new.StartActivityLog.ToShortDateString() + " " + log2new.StartActivityLog.ToShortTimeString() +
                        " - "    + log2new.StopActivityLog.ToShortDateString() + " " + log2new.StopActivityLog.ToShortTimeString() +
                        "\n\n" + CRSTranslation.TranslateDialog(CRSTranslation.MSG_DO_YOU_WANT_CONTINUE, this.UserInfo.Language)
                        , CRSTranslation.TranslateDialog(CRSTranslation.MSG_WARNING, this.UserInfo.Language)
                        , MessageBoxButton.YesNo);
                    if (r == MessageBoxResult.Yes)
                    {
                        if (log1 != null && log1new != null)
                        {
                           
                            if (log1new.StopActivityLog.Subtract(log1new.StartActivityLog).TotalMinutes > 0)
                            {
                                DbDataUpdate.UpdateActivityLog(log1.IDActivity
                                , log1.StartActivityLog
                                , log1new.StartActivityLog
                                , log1new.StopActivityLog);
                                WriteLogSyncro("U",log1.IDActivity, log1.StartActivityLog, log1new.StartActivityLog, log1new.StopActivityLog);
                            }
                            else
                            {
                                DbDataUpdate.DeleteActivityLog(log1new.IDActivity, log1new.StartActivityLog);
                                WriteLogSyncro("D", log1.IDActivity, log1.StartActivityLog, log1new.StartActivityLog, log1new.StopActivityLog);
                            }
                            
                        }
                        else res = false;
                        if (log2 != null && log2new != null)
                        {                           
                            if (log2new.StopActivityLog.Subtract(log2new.StartActivityLog).TotalMinutes > 0)
                            {
                                DbDataUpdate.UpdateActivityLog(log2.IDActivity
                                    , log2.StartActivityLog
                                    , log2new.StartActivityLog
                                    , log2new.StopActivityLog);
                                WriteLogSyncro("U", log2.IDActivity, log2.StartActivityLog, log2new.StartActivityLog, log2new.StopActivityLog);
                            }
                            else
                            {
                                DbDataUpdate.DeleteActivityLog(log2.IDActivity, log2.StartActivityLog);
                                WriteLogSyncro("D", log2.IDActivity, log2.StartActivityLog, log2new.StartActivityLog, log2new.StopActivityLog);
                            }
                           
                        }
                        else res = false;
                        res = true;
                    }
                }
            }
            this.LST_LOG_MOD.Clear();
            return res;
        }

        private void WriteLogSyncro(string typeopUPDDEL, long idact, DateTime dtstartkey, DateTime dtstart, DateTime dtstop)
        {
            DbActivityLog act = new DbActivityLog();
            act.IDActivity = idact;
            act.StartActivityLog = dtstart;
            act.StopActivityLog = dtstop;
            MBBSyncroUtility.WriteActivityLogSyncroFile(typeopUPDDEL, dtstartkey, act, this.UserInfo.Name, MBBCommon.syncroParams);

            /*DbActivityLogTypeOp acto = new DbActivityLogTypeOp();
            acto.IDActivity = idact;
            acto.StartActivityLogKey = dtstartkey;
            acto.StartActivityLog = dtstart;
            acto.StopActivityLog = dtstop;
            acto.TypeOperation = typeopUPDDEL;
            this.ActivityLogModToSend.Add(acto);*/
        }

        /// <summary>
        /// Metodo utile dettato dallo stesso comportamento che devo avere selezionando l'attività o dal pannello header in alto
        /// o dalla lista di container data impilati 
        /// </summary>
        /// <param name="sourceborder"></param>
        /// <param name="sourcecontainer"></param>
        private void SelectStronglyActivityLog(Border sourceborder, CRSContainerData sourcecontainer)
        {
            this.isModeSelectionStrongly = true;//!this.isModeSelectionStrongly;
            // a seconda che i parametri siano passati dal doppio click su CRSContainerData o su selezione strongly da border 
            // (pannello header delle activities) ricerco le activity 
            if (sourceborder == null && sourcecontainer != null)
            {
                sourceborder = FindPnlLogActivity(sourcecontainer.ID, Convert.ToDateTime(sourcecontainer.DATE));
            }
            if (sourceborder != null && sourcecontainer == null)
            {
                string t = Convert.ToString(sourceborder.Tag);
                sourcecontainer = FindLogActivityFromTag(t);
                ScrollIntoContainer(sourcecontainer);
            }

            if (sourceborder != null && sourcecontainer != null)
            {
                DeselectActivityLog_ClearColorPnlLog();
                DeselectActivityLog_CRSContainerData();
                sourceborder.Background = Brushes.Aquamarine;

                //this.isActStronglySelected = true;
                if (this.isModeSelectionStrongly)
                {
                    sourceborder.BorderBrush = Brushes.Blue;
                    sourcecontainer.IS_SELECTED_HIGH = true;
                }
                SetLayoutOkCancLogBtnsBeforeSave(Convert.ToInt32(pnlRepOkCanc.Tag),1);
                this.currentLogActivityTag = Convert.ToInt32(sourcecontainer.Tag);

                if (Convert.ToInt32(pnlRepOkCanc.Tag) == 2 || Convert.ToInt32(pnlRepOkCanc.Tag) == 3)
                {
                    DbActivityLog l = this.LST_LOGS.Where(X => X.IDActivity == sourcecontainer.ID && X.StartActivityLog == sourcecontainer.DATE).FirstOrDefault();
                    cbListActivitiesChg.SelectedItem = sourcecontainer.TITLE;
                    dtpDateFromLogsIns.SelectedDateTime = sourcecontainer.DATE;
                    if (l != null)
                        dtpDateToLogsIns.SelectedDateTime = l.StopActivityLog;
                }
            }
        }

        private void ScrollIntoContainer(CRSContainerData sourcecontainer)
        {
            double offset = 0;
            int idx = 0;
            foreach (RowDefinition r in pnlLogActivities.RowDefinitions)
            {
                if (sourcecontainer.SEQ == (int)idx / 2)
                    break;
                offset = offset + r.Height.Value;
                idx++;
            }

            scrollpnlListContainerLogs.ScrollToVerticalOffset(offset);
        }

        private void DeselectActivityLog_ClearColorPnlLog()
        {
            foreach (Border b in pnlLogActivitiesListPanels.Children)
            {
                if (b.Background == Brushes.Aquamarine)
                {
                    CRSContainerData cc = FindLogActivityFromTag(Convert.ToString(b.Tag));
                    if(cc!=null) b.Background = cc.GetBackgroundColorOriginal();                
                }
                b.BorderBrush = Brushes.LightGray;
            }
        }

        private Border FindPnlLogActivity(long idactivity, DateTime startact)
        {
            Border res = null;
            foreach (Border b in pnlLogActivitiesListPanels.Children)
            {
                // b.Tag = activity.ID.ToString() + "|" + Convert.ToDateTime(activity.DATE).ToString();
                if (Convert.ToString(b.Tag) == idactivity.ToString() + "|" + startact.ToString())
                {
                    res = b;
                    break;
                }
            }
            return res;
        }

        private CRSContainerData FindLogActivityFromTag(string tag)
        {
            //string t = Convert.ToString(d.Tag);
            long id = Convert.ToInt64(tag.Substring(0, tag.IndexOf("|")));
            DateTime dt = Convert.ToDateTime(tag.Substring(tag.IndexOf("|") + 1));
            return this.FindActivity(id, dt);
        }

        private void BorderPanel_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!this.isModeSelectionStrongly)
            {
                Border d = (Border)sender;
                d.Background = Brushes.Aquamarine;
                string t = Convert.ToString(d.Tag);
                long id = Convert.ToInt64(t.Substring(0, t.IndexOf("|")));
                DateTime dt = Convert.ToDateTime(t.Substring(t.IndexOf("|") + 1));
                CRSContainerData c = this.FindActivity(id, dt);
                DeselectActivityLog_CRSContainerData();

                if (c != null)
                    c.IS_SELECTED = true;

                //ScrollIntoContainer(c);
            }
        }

        /// <summary>
        /// Serve per identificare la modalità di selezione: se è true si seleziona un elemento e la selezione rimane sull'elemeto anche se il
        /// cursore si sposta sugli altri. Finchè non si sblocca con un'altro click
        /// </summary>
        bool isModeSelectionStrongly = false;
        private void BorderPanel_MouseDown(object sender, MouseEventArgs e)
        {
            Border d = (Border)sender;
            SelectStronglyActivityLog(d, null);
        }

        private void BorderPanel_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!this.isModeSelectionStrongly)
            {
                Border d = (Border)sender;
                string t = Convert.ToString(d.Tag);
                long id = Convert.ToInt64(t.Substring(0, t.IndexOf("|")));
                DateTime dt = Convert.ToDateTime(t.Substring(t.IndexOf("|") + 1));
                CRSContainerData c = this.FindActivity(id, dt);
                d.Background = c.GetBackgroundColorOriginal();
            }
        }
        #endregion

        private int HGT_MED_CONTAINER_ACT = 80;  
        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.HGT_MED_CONTAINER_ACT = Convert.ToInt32(txtMedHgtContainerAct.Text);
            if (txtMedHgtContainerAct.Visibility == Visibility.Visible)
                txtMedHgtContainerAct.Visibility = Visibility.Collapsed;
            else txtMedHgtContainerAct.Visibility = Visibility.Visible;
        }

        private void btnChainEmptyActivityNotes_Click(object sender, RoutedEventArgs e)
        {
            // mostra tutte le note non associate ad attività
            WinMBBStickNotes wst = null;
            foreach (Window win in Application.Current.Windows)
            {
                if (win is WinMBBStickNotes)
                {
                    wst = win as WinMBBStickNotes;
                    break;
                }
            }

            if (wst == null)
                wst = new WinMBBStickNotes();

            
            List<DbNote> lstNotes = DbDataAccess.GetEmptyActivitiesNotes(dtFilter1.SelectedDate, dtFilter2.SelectedDate);
            wst.Init(this.UserInfo, lstNotes, null, null, 0, true);
            wst.Show();
        }
    }
}
