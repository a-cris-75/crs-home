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
using System.Windows.Shapes;
using CRS.Library;
using System.Reflection;
using CRS.DBLibrary;
using MyBlackBoxCore.DataEntities;
using System.Threading;
using System.Windows.Threading;
using System.IO;
//using System.Windows.Forms;

namespace MyBlackBoxCore
{
    /// <summary>
    /// Interaction logic for MyBlackBoxCore.xaml
    /// </summary>
    public partial class WinMBBCore : Window
    {
        private const int DIM_W = 260;
        private const int DIM_H = 278;
        private const int DIM_W_R = 184;
        private const int DIM_H_R = 190;

        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenu contextMenu1;
        private System.Windows.Forms.MenuItem menuItem1;

        //private Thread ThreadImportData;
        //private Thread ThreadImportShared;
        readonly DispatcherTimer timerActivity = new DispatcherTimer();

        public bool IS_ACTIVE_KEY_EVENTS
        {
            set { ctrlMBBCore.IS_ACTIVE_KEY_EVENTS = value; }
            get { return ctrlMBBCore.IS_ACTIVE_KEY_EVENTS; }
        }

        private int MODE_APP
        {
            set; get;
        }

        public WinMBBCore()
        {
            InitializeComponent();

            string user;
            //string assemblyname = Assembly.GetExecutingAssembly().GetName().Name;
            //CRSIniFile.InitApplicationIni(assemblyname);
            //DbFactory.InitDbFile(MBBGlobalParams.DbFileNameSqlServerCE_MBB, assemblyname);

            // ESISTE GIA' IN WINDOWLOGIN
            //-- serve a copiare il file sdf nella dir dell'applicazione in modo da non sovrascrivere successivametne il file per ogni upgrade
            //-- dell'applicazione; se il file sdf esiste già non fa niente 
            //MBBCommon.InitApplicationInstall(assemblyname);

            if (DbFactory.TestConnection(CRSIniFile.GetConnectionStringFromIni(), CRSIniFile.GetProviderFromIni()) == false)
            {
                WindowInstallDB win = new WindowInstallDB();
                win.Show();
            }
            else
            {
                Activate();
                this.Topmost = true;
                Focus();
                bool userOk = this.InitApplication(out user);
                this.InitMenu();
                this.InitNotifyIcon();
                this.InitTimerAct();

                if (!userOk)
                {
                    MessageBoxResult r = MessageBox.Show(
                                        CRSTranslation.TranslateDialog(CRSTranslation.MSG_USER_NOT_INSERTED, "EN"),
                                        CRSTranslation.TranslateDialog(CRSTranslation.MSG_WARNING, "EN"), MessageBoxButton.OK);
                    //if (r == MessageBoxResult.OK)
                    {
                        this.Close();
                    }
                }
                // tutto questo è superfluo dato che il login è fatto prima dei aprire il CORE
                // se l'utente non esiste su ini, prima l'operarore deve provvedere ad inserirne uno nuovo
                /*bool loginOk = false;
                if(!userOk)
                    loginOk = this.LoginNewUser();

                if (loginOk) 
                */
                else this.Show();

                //this.StartSyncro();
                ctrlMBBCore.StartThreadSyncroWithFilesAndEmail();
                this.InitStartApp();
            }
        }

        public MBBCore GetCore()
        {
            return ctrlMBBCore;
        }

        #region INIT
        private bool InitApplication(out string user)
        {
            bool userOk = this.ctrlMBBCore.Init(out user, EvnExpandFalse, EvnExpandTrue, EvnGetChainUnchainNoteActivity, EvnGetInfoNotesIntro, EvnHideAndListen);
            this.UpgradeDB_Schema();
            this.pnlGridMain.ColumnDefinitions[1].Width = new GridLength(0);
            this.pnlGridMain.RowDefinitions[1].Height = new GridLength(0);
            this.pnlBtnMenu.Visibility = Visibility.Hidden;
            EvnSelectFunction(-1);

            Application_Startup();

            try
            {
                CRSIni iniFile = new CRSIni();
                iniFile.Name = CRSIniFile.IniPathName;
                string filename = (string)iniFile.GetValue("EVENTS", "LogFileName");

                if (filename != null)
                {
                    iniFile.Name = filename;
                }
                else
                {
                    iniFile.Name = ".\\Log\\EventsUserLog.txt";
                }
                FileNameLog = iniFile.Name;
                if (!File.Exists(FileNameLog))
                {
                    if (!Directory.Exists(System.IO.Path.GetDirectoryName(FileNameLog)))
                        Directory.CreateDirectory(System.IO.Path.GetDirectoryName(FileNameLog));
                    File.Create(FileNameLog);
                }
            }
            catch(Exception ex) 
            { 
                string s = ex.Message; 
            }
            return userOk;
        }

        private bool UpgradeDB_Schema()
        {
            bool res = false;
            string assemblyname = Assembly.GetExecutingAssembly().GetName().Name;
            string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string userFilePath = System.IO.Path.Combine(localAppData, assemblyname);
            userFilePath = System.IO.Path.Combine(userFilePath, "DB");

            string filename = System.IO.Path.Combine(userFilePath, MBBConst.FILE_NAME_ALTERDB);

            if (File.Exists(filename))
            {
                res = DbDataUpdate.UpgradeDB_Schema(filename);
            }
            return res;
        }

        /*private void StartSyncro()
         {            
             //-- devo sicronizzarmi all'avvio dato che altrimenti potrei inserire una nota con id già
             //-- presente su altro db in altra macchina
             //-- ATTENZIONE: dato che l'id corrisponde a un long con la data corrente di inserimento è impossibile che l'id sia duplicato => 
             //   posso fare la sincronizzazione su thread separato mentre uso il programma
             ctrlMBBCore.StartThreadSyncroWithFilesAndEmail();
         }*/

        private void InitNotifyIcon()
        {
            try
            {
                // Create the NotifyIcon.
                this.notifyIcon = new System.Windows.Forms.NotifyIcon();
                this.contextMenu1 = new System.Windows.Forms.ContextMenu();
                this.menuItem1 = new System.Windows.Forms.MenuItem();
                //this.menuItem2 = new System.Windows.Forms.MenuItem();

                // Initialize contextMenu1
                this.contextMenu1.MenuItems.AddRange(
                            new System.Windows.Forms.MenuItem[] { this.menuItem1 });

                // Initialize menuItem1
                this.menuItem1.Index = 0;
                this.menuItem1.Text = "E&xit";
                this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);

                // The Icon property sets the icon that will appear
                // in the systray for this application.
                notifyIcon.Icon = Properties.Resources.BlackBoxIcon64;//new System.Drawing.Icon("/MyBlackBoxCore;component/BlackBoxIcon64.ico");
                notifyIcon.ContextMenu = this.contextMenu1;
                // The Text property sets the text that will be displayed,
                // in a tooltip, when the mouse hovers over the systray icon.
                notifyIcon.Text = "MyBlackBox";
                notifyIcon.Visible = true;

                // Handle the DoubleClick event to activate the form.
                notifyIcon.Click += new System.EventHandler(this.notifyIcon_Click);
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog(ex.Message, "debug");
            }
        }

        private void InitMenu()
        {
            ctrlMBBFunctions.SetEventSelectFunction(EvnSelectFunction);
            this.ctrlActivity.Init(this.ctrlMBBCore.UserInfo, EvnStartActivity, EvnAddNoteToActivity, EvnHideAppCtrl);
            this.ctrlApp.Init(EvnHideAppCtrl, this.ctrlMBBCore.UserInfo);
        }

        private void SetControlCurrentActivity()
        {
            SetFooterHeigth(Convert.ToInt32(imgHideActivity.Tag));
            if (ctrlActivity.ACTIVITY != null && ctrlActivity.ACTIVITY.IsStarted)
            {

                List<string> lstlbl = new List<string>();
                List<string> lst = new List<string>();
                string hhmmss;
                int totsec = ctrlActivity.CalcSecondsCurrentActivity(out hhmmss);

                string perc = "";
                if (totsec > 0 && ctrlActivity.ACTIVITY.TotMinPreview > 0)
                    perc = hhmmss + " " + ((float)(ctrlActivity.ACTIVITY.TotMinPreview * 60 / totsec) * 100).ToString("0.00") + "%";
                ctrlCurrentActivity.Init(-3, lstlbl, lst, ctrlActivity.ACTIVITY.TitleActivity + ".. " + perc, "", DateTime.MinValue, "", false, false, true, ctrlMBBCore.UserInfo.Language);
                pnlGridMain.RowDefinitions[1].Height = new GridLength(ctrlCurrentActivity.GetAutoHeigthTitle() + 3);
            }
            else
            {
                ctrlCurrentActivity.Init(-2, null, null, "Cosa stai facendo?", null, DateTime.MinValue, "", false, false, true, ctrlMBBCore.UserInfo.Language);
            }
        }

        private void EvnGetChainUnchainNoteActivity()
        {
            DbActivityNote ac = DbDataAccess.GetActivityNote(ctrlActivity.ACTIVITY.IDActivity, ctrlMBBCore.CurrentNote.ID);
            if (ac != null && ac.IDActivity > 0)
            {
                ctrlMBBCore.CHAINED_ID_ACTIVITY = ctrlActivity.ACTIVITY.IDActivity;
                imgChain.Source = CRSImage.CreateImage(CRSGlobalParams.IconChain);
            }
            else
            {
                ctrlMBBCore.CHAINED_ID_ACTIVITY = -1;
                imgChain.Source = CRSImage.CreateImage(CRSGlobalParams.IconChainUnchain);
            }
        }

        // in realtà non è usato
        private void EvnGetInfoNotesIntro(string info, double perc)
        {
            WinMBBIntro wst = null;
            foreach (Window win in Application.Current.Windows)
            {
                if (win is WinMBBIntro)
                {
                    wst = win as WinMBBIntro;
                    break;
                }
            }

            if (wst != null)
                wst.SetInfo("wait for import notes", info, perc);
        }

        private void InitTimerAct()
        {
            timerActivity.Tick += new EventHandler(timerActivity_Tick);
            timerActivity.Interval = new TimeSpan(0, 0, 10);
        }

        private void InitStartApp()
        {
            timerActivity.Start();
            ctrlActivity.StartEmptyActivity();
        }

        private void SetFooterHeigth(int tagActivity)
        {
            // mostra
            if (tagActivity == 0)
            {
                ctrlMBBCore.HEIGTH_FOOTER_FOR_REDIM_AUTO = (int)ctrlCurrentActivity.GetAutoHeigthTitle() + 3;
                if (pnlGridMain.RowDefinitions[1].Height.Value == 0)
                {
                    pnlGridMain.RowDefinitions[1].Height = new GridLength(ctrlMBBCore.HEIGTH_FOOTER_FOR_REDIM_AUTO);
                    this.Height = this.Height + pnlGridMain.RowDefinitions[1].Height.Value;
                }
                imgHideActivity.Visibility = Visibility.Visible;
                imgChain.Visibility = Visibility.Visible;
                // set margin
                if (this.MODE_APP == -1)
                {
                    ctrlCurrentActivity.Margin = new Thickness(0, -50, 25, 50);
                    imgHideActivity.Margin = new Thickness(0, -80, 2, 0);
                    imgChain.Visibility = Visibility.Hidden;
                }
                else
                {
                    ctrlCurrentActivity.Margin = new Thickness(0);
                    imgHideActivity.Margin = new Thickness(0, -30, 2, 0);
                    //imgChain.Margin = new Thickness(0, -20, 0, 0);
                    imgChain.Visibility = Visibility.Visible;
                }
            }
            // nascondi footer activity
            else
            {
                pnlGridMain.RowDefinitions[1].Height = new GridLength(0);
                ctrlMBBCore.HEIGTH_FOOTER_FOR_REDIM_AUTO = 0;
                imgHideActivity.Visibility = Visibility.Hidden;
                imgChain.Visibility = Visibility.Hidden;
            }
        }

        private int timesNotShowActivity = 0;
        protected void timerActivity_Tick(object sender, EventArgs e)
        {
            if (Convert.ToInt32(imgHideActivity.Tag) == 0)
            {
                SetControlCurrentActivity();
                //int currentHgtFooter = (int)pnlGridMain.RowDefinitions[1].Height.Value;
                //if (currentHgtFooter == 0)
                //    SetFooterHeigth(0);
            }
            else
            {
                timesNotShowActivity++;
            }
            // se per 5 min non mostro le attività riapri
            if (timesNotShowActivity >= 10)
            {
                timesNotShowActivity = 0;
                imgHideActivity.Tag = 0;
            }
        }
        #endregion

        #region LISTENER KEY EVENTS
        CRSEventListener KListener = new CRSEventListener();

        private void Application_Startup()
        {
            KListener.KeyDown += new RawKeyEventHandler(KListener_KeyDown);
            KListener.KeyUp += new RawKeyEventHandler(KListener_KeyUp);
        }

        string WORD_LISTEN = "";
        private string FileNameLog = "";
        private bool IsShiftChar = false;
        private bool IsCtrlChar = false;
        private bool IsAltChar = false;
        private bool IsCapitalChar = false;

        void KListener_KeyUp(object sender, RawKeyEventArgs args)
        {
            if (this.IS_ACTIVE_KEY_EVENTS)
            {
                string inputlower = args.Key.ToString().ToLower();
                if (inputlower.Contains("shift"))
                {
                    this.IsShiftChar = false;
                }
                if (inputlower.Contains("ctrl"))
                {
                    this.IsCtrlChar = false;
                }
                if (inputlower.Contains("alt"))
                {
                    this.IsAltChar = false;
                }
            }
        }

        void KListener_KeyDown(object sender, RawKeyEventArgs args)
        {
            if (this.IS_ACTIVE_KEY_EVENTS)
            {
                // registra log su db
                string input = args.Key.ToString();
                string inputlower = args.Key.ToString().ToLower();
                if (inputlower.Contains("esc"))
                {
                    this.Visibility = Visibility.Visible;
                    this.ctrlMBBCore.SetLayoutExpand(this.LAST_STATUS_LAYOUT);
                    if (ExistsWinStick() != null)
                        ExistsWinStick().WindowState = WindowState.Maximized;

                    //IS_ACTIVE_KEY_EVENTS = false;
                }
                else
                {
                    string s = InterceptKeys.KeyListenerTranslate(input, ref IsCapitalChar, ref IsShiftChar, ref IsAltChar, ref IsCtrlChar);

                    if (!string.IsNullOrEmpty(s))
                    {
                        WORD_LISTEN = WORD_LISTEN + s;
                    }
                    else
                    // se premo invio o control + s allora scrivo su file di log
                    if (inputlower.Contains("enter") || inputlower.Contains("return") || 
                        (!IsShiftChar && !IsAltChar && IsCtrlChar && inputlower.Contains("s")))
                    {
                        // scrivo solo se c'è da scivere
                        if (!string.IsNullOrEmpty(WORD_LISTEN) && !string.IsNullOrEmpty(FileNameLog))
                        {
                            string sdate = "[" + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + "] ";
                            // copia in log file
                            File.AppendAllText(FileNameLog, "\n" + sdate + WORD_LISTEN);
                            WORD_LISTEN = "";
                        }
                    }
                }
            }
        }

        private void Application_Exit()
        {
            KListener.Dispose();
        }
        #endregion

        #region LOGIN USER
        /// <summary>
        /// NON E' USATO dato che il controllo sullo user è fatto precedentemente sul form WindowLogin.
        /// WindowLogin è il Main Form dell'applicazione, da cui apro WinMBBCore.
        /// Con questo metodo apro la finestra di login data che l'utente non esiste.
        /// Dò comunque la possibilità di cambiare nome utente o di aggiungerlo al db.
        /// </summary>
        /// <returns></returns>
        private bool LoginNewUser()
        {
            bool res = false;
            // superfluo: se faccio login significa che l'utente non esiste
            //int iduser = DbDataAccess.GetIDUser(ctrlMBBCore.UserInfo.Name);
            //if (iduser < 0)
            {
                //-- apri form di login, con proprietà new user
                //WinMBBNewUser winlogin = new WinMBBNewUser();
                WindowLogin winlogin = new WindowLogin();

                res = winlogin.ShowDialog().Value;

                if (winlogin.DialogResult.Value == false)
                {
                    winlogin.Close();
                    this.Close();
                }
                else
                {
                    winlogin.Close();
                }
            }
            return res;
        }
        #endregion

        #region EVENTS WINDOW
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ctrlMBBCore.ExpandLayoutStep != MBBCore.EXPAND_STEP4_MAXIMIZED)
            {
                if (e.ButtonState == MouseButtonState.Pressed)
                {
                    DragMove();
                    WinMBBFindTitle wint = ctrlMBBCore.GetWinFindTitle();
                    WinMBBDocs wind = ctrlMBBCore.GetWinDocs();
                    WinMBBSharingUsers winsu = ctrlMBBCore.GetWinShareUsers();
                    WinMBBSharingNotes winsn = ctrlMBBCore.GetWinShareNotes();
                    if (wint != null) wint.SetLayoutPos(this.Left, this.Width, this.Top, this.Height);
                    bool showd = false;
                    bool showsu = false;
                    bool showsn = false;
                    if (wind != null) showd = wind.Visibility == System.Windows.Visibility.Visible;
                    if (winsu != null) showsu = winsu.Visibility == System.Windows.Visibility.Visible;
                    if (winsn != null) showsn = winsn.Visibility == System.Windows.Visibility.Visible;
                    this.ctrlMBBCore.ManagePosWin(showd, showsu, showsn);
                }
            }
        }

        void notifyIcon_Click(object sender, EventArgs e)
        {
            if (this.WindowState == System.Windows.WindowState.Minimized)
            {
                WindowState = System.Windows.WindowState.Normal;
                this.ctrlMBBCore.RestoreLayout(false);
            }
            else
            {
                WindowState = System.Windows.WindowState.Minimized;
                this.ctrlMBBCore.RestoreLayout(true);
            }
            Show();

        }

        private void menuItem1_Click(object Sender, EventArgs e)
        {
            this.Close();
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == System.Windows.WindowState.Minimized)
                ctrlMBBCore.RestoreLayout(true);
            else ctrlMBBCore.RestoreLayout(false);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                //ctrlMBBCore.SetRecentNotesToIni();
                // sincronizza con email se il flag è attivo
                if (MBBCommon.syncroParams.SyncWithEmail)
                    MBBSyncroUtility.SyncroSendToEmail(ctrlMBBCore.NotesModToSend, this.ctrlMBBCore.UserInfo);

                foreach (Window win in Application.Current.Windows)
                {
                    if (win is WinMBBStickNotes || win is WinMBBDocs || win is WinMBBFind || win is WinMBBSharingUsers || win is WinMBBIntro ||
                        win is WinMBBSharingNotes || win is WinMBBTitlesNotes || win is WinMBBFindTitle)
                    {
                        win.Close();
                    }
                }

                ctrlActivity.StopActivity();

                this.Application_Exit();
            }
            catch { }
        }

        private void Window_MouseEnter(object sender, MouseEventArgs e)
        {
            this.Opacity = 100;
        }

        private void Window_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Opacity = 90;
        }
        #endregion

        #region EXT EVENTS
        private int LAST_STATUS_LAYOUT = 0;
        private void EvnHideAndListen()
        {
            this.Visibility = Visibility.Hidden;
            this.LAST_STATUS_LAYOUT = ctrlMBBCore.ExpandLayoutStep;
            ctrlMBBCore.SetLayoutExpand(MBBCore.EXPAND_FALSE);
            if(ExistsWinStick()!=null)
                ExistsWinStick().WindowState = WindowState.Minimized;

            IS_ACTIVE_KEY_EVENTS = true;
        }

        private void EvnHideAppCtrl()
        {
            EvnSelectFunction(-1);
        }

        private void EvnSelectFunction(int mode_app)
        {
            ctrlMBBFunctions.Visibility = Visibility.Hidden;
            this.MODE_APP = mode_app;
            pnlBtnMenu.Visibility = Visibility.Visible;
            ctrlApp.Visibility = Visibility.Hidden;
            ctrlActivity.Visibility = Visibility.Hidden;
            //pnlBtnMenu.Margin = new Thickness(10, 0, 0, 41);//new Thickness(210, 0, 0, 130);
            if (this.Width < 300) this.Width = 300;
            this.ResizeMode = ResizeMode.CanResizeWithGrip;

            SetFooterHeigth(Convert.ToInt32(imgHideActivity.Tag));

            switch (mode_app)
            {
                // modalità menu
                case -1:
                    ctrlMBBFunctions.Visibility = Visibility.Visible;
                    pnlBtnMenu.Visibility = Visibility.Hidden;
                    this.ResizeMode = ResizeMode.NoResize;
                    ctrlMBBCore.SetLayoutExpand(MBBCore.EXPAND_FALSE);
                    break;
                // note
                case 0:
                    ctrlMBBCore.Visibility = Visibility.Visible;
                    if (ctrlMBBCore.ExpandLayoutStep == 0)
                        ctrlMBBCore.ExpandLayoutStep = 1;
                    ctrlMBBCore.SetLayoutExpand(ctrlMBBCore.ExpandLayoutStep);
                    break;
                // doc
                case 1:
                    ctrlMBBCore.Visibility = Visibility.Visible;
                    ctrlMBBCore.SetLayoutExpand(MBBCore.EXPAND_FALSE);
                    EvnOpenFindWin();
                    break;
                // app
                case 2:
                    ctrlMBBCore.Visibility = Visibility.Hidden;
                    ctrlApp.Visibility = Visibility.Visible;
                    ctrlActivity.SetDimPos(1, 450);
                    //270, 0, 0, 400
                    //pnlBtnMenu.Margin = new Thickness(300, -30, 0, 430);
                    pnlBtnMenu.Visibility = Visibility.Hidden;
                    break;
                // act
                case 3:
                    pnlBtnMenu.Visibility = Visibility.Hidden;
                    ctrlActivity.Visibility = Visibility.Visible;
                    ctrlActivity.FillActivitiesParams(false);
                    ctrlActivity.SetDimPos(1, 450);
                    break;
                // sticky: mostra le note come sticky in bacheca su schermo
                case 4:
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

                    ctrlMBBCore.SetLayoutExpand(MBBCore.EXPAND_STEP2_FREEE);
                    //ctrlMBBCore.ExpandFixed();
                    wst.Show();
                    wst.Init(this.ctrlMBBCore.UserInfo, this.ctrlMBBCore.NotesFound, ctrlMBBCore.FillSingleNote, EvnHideAppCtrl, 2);

                    break;
                default:
                    break;
            }
        }

        private void EvnOpenFindWin()
        {
            //-- open window params
            WinMBBFind f = this.ctrlMBBCore.GetWinFind();
            if (f == null)
                f = new WinMBBFind(this.ctrlMBBCore.UserInfo.IDUser);

            f.IsHeaderExpanded = true;
            f.ctrlMBBDocs.IsBtnExitVisible = false;
            f.FillTitle(this.ctrlMBBCore.UserInfo.IDUser);
            f.IsReduced = false;
            f.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            f.HEADER = "       FIND";
            f.TabItemSelectedIndex = 2;
            f.Visibility = Visibility.Visible;
            f.IsHeaderExpanded = false;
            f.ctrlMBBDocs.FillDocs(f.GetFilterParams());
            f.Show();
        }

        private void EvnExpandFalse()
        {
            pnlBtnMenu.Visibility = Visibility.Hidden;
            ctrlMBBFunctions.Visibility = Visibility.Visible;
            this.MODE_APP = -1;
            this.SetFooterHeigth(Convert.ToInt32(imgHideActivity.Tag));
        }

        private void EvnExpandTrue()
        {
            pnlBtnMenu.Visibility = Visibility.Visible;
            ctrlMBBFunctions.Visibility = Visibility.Hidden;
            this.MODE_APP = 0;
            this.SetFooterHeigth(Convert.ToInt32(imgHideActivity.Tag));
        }

        private void EvnAddNoteToActivity()
        {
            if (ctrlActivity.ACTIVITY.IDActivity > 0)
            {
                ctrlActivity.Visibility = Visibility.Hidden;
                ctrlMBBCore.Visibility = Visibility.Visible;
                //this.MODE_APP = 1;
                ctrlMBBCore.CHAINED_ID_ACTIVITY = ctrlActivity.ACTIVITY.IDActivity;
                ctrlMBBCore.ModeLayoutInsUpdFindClose = MBBConst.MODE_INS;
            }
        }

        private void EvnStartActivity()
        {
            if (ctrlActivity.ACTIVITY.IsStarted)
            {
                ctrlCurrentActivity.Visibility = Visibility.Visible;
                imgHideActivity.Tag = 0;
                SetControlCurrentActivity();
                EvnGetChainUnchainNoteActivity();

                IS_ACTIVE_KEY_EVENTS = true;
                if (!string.IsNullOrEmpty(FileNameLog))
                {
                    string sdate = "[" + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + "] ";
                    // copia in log file
                    File.AppendAllText(FileNameLog, "\n" + sdate + "START ACTIVITY >> " + ctrlActivity.ACTIVITY.TitleActivity);
                    WORD_LISTEN = "";
                }
            }
        }
        #endregion

        #region EVENTS CONTROLS
        private void TextBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            EvnSelectFunction(-1);
        }

        private void imgStartLog_MouseDown(object sender, MouseButtonEventArgs e)
        {
            IS_ACTIVE_KEY_EVENTS = !IS_ACTIVE_KEY_EVENTS;
        }

        private void btnHideActivity_Click(object sender, RoutedEventArgs e)
        {
            this.Height = this.Height - pnlGridMain.RowDefinitions[1].Height.Value;
            pnlGridMain.RowDefinitions[1].Height = new GridLength(0);
            btnHideActivity.Tag = 1;
        }

        private void imgHideActivity_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //this.Height = this.Height - pnlGridMain.RowDefinitions[1].Height.Value;
            pnlGridMain.RowDefinitions[1].Height = new GridLength(0);
            imgHideActivity.Tag = 1;
            SetFooterHeigth(1);
        }

        private void imgChain_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (ctrlMBBCore.CHAINED_ID_ACTIVITY > 0)
            {
                //imgChain.Tag = 0;
                //imgChain.Source = CRSImage.CreateImage(CRSGlobalParams.IconChainUnchain);
                //ctrlMBBCore.CURRENT_ID_ACTIVITY = -1;
                DbDataUpdate.DeleteActivityNote(ctrlMBBCore.CurrentNote.ID, ctrlMBBCore.CHAINED_ID_ACTIVITY);
            }
            else
            {
                //imgChain.Tag = 1;
                //imgChain.Source = CRSImage.CreateImage(CRSGlobalParams.IconChain);
                //ctrlMBBCore.CURRENT_ID_ACTIVITY = ctrlActivity.ACTIVITY.IDActivity;
                if (ctrlActivity.ACTIVITY.IDActivity > 0)
                    DbDataUpdate.InsertActivityNote(ctrlMBBCore.CurrentNote.ID, ctrlActivity.ACTIVITY.IDActivity);
            }

            EvnGetChainUnchainNoteActivity();
        }
        #endregion

        private void Window_Drop(object sender, System.Windows.DragEventArgs e)
        {
            
        }

        private WinMBBStickNotes ExistsWinStick()
        {
            WinMBBStickNotes ws = null;
            foreach (Window win in System.Windows.Application.Current.Windows)
            {
                if (win is WinMBBStickNotes && win.Visibility == Visibility.Visible && win.WindowState != WindowState.Minimized)
                {
                    ws = win as WinMBBStickNotes;
                    break;
                }
            }

            return ws;
        }

        private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            WinMBBStickNotes ws = ExistsWinStick();
            Point pointToWindow = Mouse.GetPosition(this);
            Point pointToScreen = PointToScreen(pointToWindow);
             if (ws != null && pointToScreen.X > ws.Width - 400 && pointToScreen.X < ws.Width && ws.GetVisAcivities() != Visibility.Visible)
            {
                double wdt; double hgt;
                System.Drawing.Point p = ws.GetCorePosition(out wdt, out hgt);
                this.Top = p.Y;
                this.Left = p.X;
                this.Height = hgt;
                this.Width = wdt;
                ctrlMBBCore.ExpandLayoutStep = 2;
            }
        }
    }
}
