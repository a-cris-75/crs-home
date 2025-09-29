using CRS.Library;
using MyBlackBoxCore.DataEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MyBlackBoxCore
{
    /// <summary>
    /// Interaction logic for WinMBBIntro.xaml
    /// </summary>
    public partial class WinMBBIntro : Window
    {
        readonly DispatcherTimer dispatcherTimerRefresh = new DispatcherTimer();

        public string INFO = "";
        private MBBUserInfo UserInfo = null;

        public delegate void EventExternalSyncro(int numdays);
        private EventExternalSyncro eventExternalSyncro;

        public WinMBBIntro(MBBUserInfo user, EventExternalSyncro evn, double l =0, double t=0, double w=0)
        {
            try
            {
                InitializeComponent();
                this.eventExternalSyncro = evn;
                this.UserInfo = user;
                if (l == 0 && t == 0 && w == 0)
                {
                    double screenWidth = Screen.PrimaryScreen.WorkingArea.Width - 3;
                    double screenHeight = Screen.PrimaryScreen.WorkingArea.Height - 3;

                    //bool taskBarOnTopOrBottom = (Screen.PrimaryScreen.WorkingArea.Width == Screen.PrimaryScreen.Bounds.Width);

                    l = screenWidth - this.Width;
                    t = screenHeight - this.Height;
                    w = this.Width;
                }
                this.Top = t;
                this.Left = l;
                this.Width = w;

                /*dispatcherTimerRefresh.Interval = new TimeSpan(0, 0, 5);
                dispatcherTimerRefresh.Tick += new EventHandler(dispatcherTimer_Tick);
                dispatcherTimerRefresh.Start();*/
                //StartThreadSyncroWithFilesAndEmail();
            }
            catch(Exception ex)
            {
                CRSLog.WriteLog("Exception in WinMBBIntro constructor: " + ex.Message, this.UserInfo.Name);
            }
        }

        private bool type_syncro_NOTES = false;
        private bool type_syncro_DOCS = false;
        private bool type_syncro_SHARED = false;

        public void ActivateTypeSyncro(bool type_syncro_notes,bool type_syncro_docs, bool type_syncro_notes_shared, bool type_syncro_external, int numdays)
        {
            this.type_syncro_NOTES = type_syncro_notes;
            this.type_syncro_DOCS = type_syncro_docs;
            this.type_syncro_SHARED = type_syncro_notes_shared;
            
            if (type_syncro_notes)
                StartThreadSyncroWithFilesAndEmail();
            
            if (type_syncro_docs)
                StartThreadSyncroDocs();
            
            if (type_syncro_notes_shared)
                StartThreadSyncroShared();
            
            if (type_syncro_external)
                StartThreadExternalSyncro(numdays);
        }


        public void SetInfo(string header, string info, double perc)
        {
            this.INFO = info;
            this.Dispatcher.Invoke((Action)(() => 
                    txtInfoDataImport.Text = info));
            this.Dispatcher.Invoke((Action)(() =>
                    txtHeader.Text = header));
            if (perc>0)
                this.Dispatcher.Invoke((Action)(() =>
                    barProgress.Value = perc));
            else
                this.Dispatcher.Invoke((Action)(() =>
                    barProgress.IsIndeterminate = true));
            
            //txtInfoDataImport.Dispatcher.Invoke((Action)(() => txtInfoDataImport.Text = "importing notes.. " + info));

        }
        private delegate void EventVoid();
        private bool isEventStarted = false;
        protected void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            //Dispatcher.Invoke(() =>
            //{
            if (!isEventStarted)
            {
                // dopo il primo tick fermo il timer per consentire di eseguire l'import dei dati
                dispatcherTimerRefresh.Stop();
                isEventStarted = true;
                //EventVoid newevn = new EventVoid(EventSyncroData);
                //newevn.BeginInvoke(null,null);
                EventSyncroData();
                SetInfo("import notes from file..", "import FINISHED", 100);
                EventSyncroDataWithEmail();
                SetInfo("import notes from EMAIL..", "import FINISHED", 100);
                // riavvio il timer: al prossimo giro il form si chiuderà dopo aver mostrato per 5 secondi il messaggio di termine import
                //dispatcherTimerRefresh.Start();
            }
            //);
            this.Close();
            
            //dispatcherTimerRefresh.Start();
        }
      
        private void TaskSyncroNotes()
        {
            EventSyncroData();
            SetInfo("import notes..", "import FINISHED", 100);
            EventSyncroDataWithEmail();
            SetInfo("import notes from EMAIL..", "import FINISHED", 100);            
            this.Dispatcher.Invoke((Action)(() => Close()));
            //this.Close();
        }

        /// <summary>
        /// TO DO
        /// </summary>
        private void TaskSyncroDocs()
        {
            // to do
            SetInfo("syncronize docs", "syncro docs FINISHED", 100);
            this.Dispatcher.Invoke((Action)(() => Close()));
        }

        private void TaskSyncroNotesShared()
        {
            EventSyncroSharedTodayNotes();
            SetInfo("syncronize shared notes", "refresh shared ok", 100);
            this.Dispatcher.Invoke((Action)(() => Close()));
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            //SetInfo("importing notes..", "START", 1);
        }

        private void TaskSyncroExternal(object numdays)
        {
            SetInfo("external syncro..", "export START", 0);
            if (this.eventExternalSyncro!= null)
                this.eventExternalSyncro(Convert.ToInt32(numdays));
            SetInfo("external syncro..", "export FINISHED", 100);
            this.Dispatcher.Invoke((Action)(() => Close()));
            //this.Close();
        }

        #region BACKGROUND EVENT
        public Thread StartThreadSyncroWithFilesAndEmail()
        {
            Thread newt = new Thread(new ThreadStart(TaskSyncroNotes));
            newt.SetApartmentState(ApartmentState.STA);
            newt.Name = "ThreadIntroSyncroWithFilesAndEmail";
            newt.IsBackground = true;
            newt.Start();
            return newt;
        }

        public Thread StartThreadSyncroDocs()
        {
            Thread newt = new Thread(new ThreadStart(TaskSyncroDocs));
            newt.SetApartmentState(ApartmentState.STA);
            newt.Name = "ThreadIntroSyncroDocs";
            newt.IsBackground = true;
            newt.Start();
            return newt;
        }

        public Thread StartThreadSyncroShared()
        {
            Thread newt = null;
            try
            {
                newt = new Thread(new ThreadStart(TaskSyncroNotesShared));
                newt.SetApartmentState(ApartmentState.STA);
                newt.Name = "ThreadIntroShared";
                newt.IsBackground = true;
                newt.Start();
            }
            catch(Exception ex)
            {
                CRSLog.WriteLogException("Exception in StartThreadSyncroShared", ex, 1);
            }
            return newt;
        }

        public Thread StartThreadExternalSyncro(int numdays)
        {
            Thread newt = new Thread(new ParameterizedThreadStart(TaskSyncroExternal));
            newt.SetApartmentState(ApartmentState.STA);
            newt.Name = "ThreadIntroSyncroExternal";
            newt.IsBackground = true;
            newt.Start(numdays);
            return newt;
        }

        #endregion



        #region EVENTS
        private void EventSyncroDataWithEmail()
        {
            if (MBBCommon.syncroParams.SyncWithEmail)
                MBBSyncroUtility.SyncroReadFromEmail(this.UserInfo);
        }
        
        private void EventSyncroData()
        {
            DateTime lastSYNCRO = DateTime.FromFileTime(Convert.ToInt64(CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_LASTSYNCRO_OWNER, 0)));
            string infonotes;
            int numsteps = 1;
            // dato che da DateTime.FromFileTime ottengo 01/01/1600 (!!) da data null, vado indietro di 20 anni 
            if (lastSYNCRO > DateTime.Now.AddDays(-7200))
                numsteps = ((int)DateTime.Now.Subtract(lastSYNCRO).TotalDays / 7) + 1;
            else lastSYNCRO = DateTime.MinValue;
            int numstepstmp = numsteps;
            bool recalcnumsteps = false;
            DateTime newdtfromsyncro = lastSYNCRO;
            int totnotes = 0;

            MBBSyncroParams p = new MBBSyncroParams();
            p.SharingDir = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_SHARINGDIR);
            p.DropBoxRootDir = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_DROPBOXDIR);
            string remoteDir = MBBSyncroUtility.GetUserRemoteDir(this.UserInfo.Name, false, p);
            MBBSyncroFilesInstance syncroInstance = new MBBSyncroFilesInstance(remoteDir, lastSYNCRO, DateTime.Now.ToFileTime());
            int numnotestoimport = syncroInstance.GetNumSectionsFile(1, lastSYNCRO);

            if (numnotestoimport > 0)
            {
                // faccio step per settimana
                for (int i = 0; i < numsteps; i++)
                {
                    if (lastSYNCRO < DateTime.Now.AddDays(-7200))
                    {
                        // se la data di ultimo aggiornamento non è inizializzata devo ricalcolare il numero di step: capita al primo avvio dell'applicazione
                        // SyncroDBFromFileOwner e all'interno ReadFileSaveToDBUpdNotes inizializzano lastSYNCRO se è = MinValue
                        recalcnumsteps = true;
                    }

                    syncroInstance.SyncroDBFromFile(lastSYNCRO, 7, true, false, out infonotes, out lastSYNCRO);

                    totnotes = totnotes + Convert.ToInt32(infonotes);
                    infonotes = "to " + lastSYNCRO.AddDays(7).ToShortDateString() + "\n" + totnotes.ToString() + " (" + infonotes + " for week)";

                    double perc = 0;
                    if (numnotestoimport > 0)
                        perc = (double)totnotes / numnotestoimport * 100;

                    this.SetInfo("importing notes..", infonotes, perc);

                    if (recalcnumsteps)
                    {
                        numsteps = (int)DateTime.Now.Subtract(lastSYNCRO).TotalDays / 7;
                        recalcnumsteps = false;
                    }
                    lastSYNCRO = lastSYNCRO.AddDays(7);
                }
            }
        }

        private void EventSyncroSharedTodayNotes()
        {
            try
            {
                List<DbNote> lstSh = MBBSyncroUtility.SyncroGetNotesFromFileShared(this.UserInfo.Name, DateTime.Now.AddDays(-1), 0);

                DateTime dt1 = DateTime.Now.AddDays(-1);
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
                    WinMBBSharingNotes w = null;
                    object wo = Utils.ExistWindowInstance<WinMBBSharingNotes>();
                    if (wo != null)
                        w = wo as WinMBBSharingNotes;

                    if (w != null && !w.ctrlMBBSharingNotes.IsPermanentlyClosed)
                    {
                        w.FillSharingNotes(lstSh);
                        w.ctrlMBBSharingNotes.SetFindParams(p);
                        w.Show();
                    }
                    // qua dentro non ho la nota corrente
                    /*else
                    {
                        if (w == null)
                        {
                            w = new WinMBBSharingNotes(this.currentNote);
                            w.FillSharingNotes(lstSh);
                            w.ctrlMBBSharingNotes.SetFindParams(p);
                            if (!w.ctrlMBBSharingNotes.IsPermanentlyClosed)
                                w.Show();
                        }
                    }*/
                }
            }
            catch { }
        }
        #endregion
    }
}
