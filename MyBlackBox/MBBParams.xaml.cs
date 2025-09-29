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
using System.IO;
using CRS.DBLibrary;
using System.Diagnostics;
using Path = System.IO.Path;
using System.Threading;

namespace MyBlackBoxCore
{
    /// <summary>
    /// Interaction logic for MBBParams.xaml
    /// </summary>
    public partial class MBBParams : UserControl
    {

        private int currentIdUser;
        private string lang = "IT";
        
        public MBBParams()
        {
            InitializeComponent();
            
        }

        public void InitLanguage(string lang)
        {
            //-- init style
            try
            {
                //-- init info               
                ctrlMBBSyncroPendings.Lang = lang;
                //ctrlMBBCurrentPendings.Lang = lang;
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("Error Init control MBBParams: " + ex.Message, "");
            }
        }

        public bool IsUserChanged
        {
            set; get;
        }

         public bool IsRemoteDirChanged
        {
            set; get;
        }

        public void Save()
        {
            try
            {
                CRSIniFile.SetPropertyValue(APPConstants.SEC_MYBLACKBOX, APPConstants.PROP_LOCALTMPDIR, ctrlDirStorageTemp.GetControlValue().ToString());
                if (string.IsNullOrEmpty(Convert.ToString(ctrlDirStorageTemp.GetControlValue().ToString())))
                {
                    ctrlDirStorageTemp.SetControlValue(".\\Docs");
                    CRSIniFile.SetPropertyValue(APPConstants.SEC_MYBLACKBOX, APPConstants.PROP_LOCALTMPDIR, ".\\Docs");
                }
                CRSIniFile.SetPropertyValue(APPConstants.SEC_MYBLACKBOX, APPConstants.PROP_DROPBOXDIR, ctrlDirStorageDropBox.GetControlValue().ToString());
                CRSIniFile.SetPropertyValue(APPConstants.SEC_MYBLACKBOX, APPConstants.PROP_SHARINGDIR, ctrlDirSharing.GetControlValue().ToString());
                CRSIniFile.SetPropertyValue(APPConstants.SEC_MYBLACKBOX, APPConstants.PROP_NEWFILEEXTENSION, ctrlNewFileExtension.GetControlValue().ToString());

                CRSIniFile.SetPropertyValue(APPConstants.SEC_MYBLACKBOX, APPConstants.PROP_AUTOSYNC, ctrlAutoSync.GetControlValue().ToString());
                CRSIniFile.SetPropertyValue(APPConstants.SEC_MYBLACKBOX, APPConstants.PROP_AUTOSAVE, ctrlAutoSave.GetControlValue().ToString());

                CRSIniFile.SetPropertyValue(APPConstants.SEC_MYBLACKBOX, APPConstants.PROP_CONFIRMOP, ctrlConfirmSyncroOp.GetControlValue().ToString());
                CRSIniFile.SetPropertyValue(APPConstants.SEC_MYBLACKBOX, APPConstants.PROP_OPNOOVERWRITEOWNER, rbSyncOwner.IsChecked.ToString());
                CRSIniFile.SetPropertyValue(APPConstants.SEC_MYBLACKBOX, APPConstants.PROP_OPOVERWRITEOLD, rbOverwriteOldVer.IsChecked.ToString());
                // AL MOMENTO NON SEMBRANO USATRI DATO CHE IL REFRESHTIMER è UN'ALTRO PARAMETRO SEMPRE SU FILE INI (nelle intenzioni)
                CRSIniFile.SetPropertyValue(APPConstants.SEC_MYBLACKBOX, APPConstants.PROP_TIMER_SYNCRO, ctrlTimerSyncro.GetControlValue().ToString());
                CRSIniFile.SetPropertyValue(APPConstants.SEC_MYBLACKBOX, APPConstants.PROP_TIMER_SYNCRO_DOCS, ctrlTimerSyncroForDocs.GetControlValue().ToString());
                CRSIniFile.SetPropertyValue(APPConstants.SEC_MYBLACKBOX, APPConstants.PROP_ISUNIQUEFILENAME, ctrlIsUniqueFileName.GetControlValue().ToString());
                CRSIniFile.SetPropertyValue(APPConstants.SEC_MYBLACKBOX, APPConstants.PROP_SYNCROFILES, chkVersionAndSyncro.IsChecked.ToString());
                CRSIniFile.SetPropertyValue(APPConstants.SEC_MYBLACKBOX, APPConstants.PROP_SYNCRO_DAYS, ctrlDaysSyncro.GetControlValue().ToString());

                //CRSIniFile.SetPropertyValue(APPConstants.SEC_MYBLACKBOX, APPConstants.PROP_SEPARTATION_CHAR_TITLE, ctrlSeparationCharTitle.GetControlValue().ToString());
                if(rbSepChar.IsChecked.Value)
                    CRSIniFile.SetPropertyValue(APPConstants.SEC_MYBLACKBOX, APPConstants.PROP_SEPARTATION_CHAR_TITLE, txtSepCharTitle.Text);
                else CRSIniFile.SetPropertyValue(APPConstants.SEC_MYBLACKBOX, APPConstants.PROP_SEPARTATION_CHAR_TITLE, " ");

                CRSIniFile.SetPropertyValue(APPConstants.SEC_MYBLACKBOX, APPConstants.PROP_FILE_DIM_DAYS, ctrlExportDaysNotes.GetControlValue().ToString());

                CRSIniFile.SetPropertyValue(APPConstants.SEC_MYBLACKBOX, APPConstants.PROP_EMAIL_SERVER_IMAP, txtImap.Text);
                CRSIniFile.SetPropertyValue(APPConstants.SEC_MYBLACKBOX, APPConstants.PROP_EMAIL_SERVER_IMAP_PORT, txtImapPort.Text);
                CRSIniFile.SetPropertyValue(APPConstants.SEC_MYBLACKBOX, APPConstants.PROP_EMAIL_SERVER_SMTP, txtSmtp.Text);
                CRSIniFile.SetPropertyValue(APPConstants.SEC_MYBLACKBOX, APPConstants.PROP_EMAIL_SERVER_SMTP_PORT, txtSmtpPort.Text);
                CRSIniFile.SetPropertyValue(APPConstants.SEC_MYBLACKBOX, APPConstants.PROP_EMAIL_SERVER_POP3, txtPop3.Text);
                CRSIniFile.SetPropertyValue(APPConstants.SEC_MYBLACKBOX, APPConstants.PROP_EMAIL_SERVER_POP3_PORT, txtPop3Port.Text);

                string s = ctrlUser.GetControlValue().ToString();
                CRSIniFile.SetPropertyValue(APPConstants.SEC_APPL, APPConstants.PROP_USER, s);
                string pwd = ctrlPwd.GetControlValue().ToString();
                string appName = CRSIniFile.GetApplicationNameFromIni();
                string appVers = CRSIniFile.GetApplicationVersionFromIni();
                pwd = CRSLicense.EncodeRegCode(appName, appVers, pwd, 1);
                CRSIniFile.SetPropertyValue(APPConstants.SEC_APPL, APPConstants.PROP_USERPWD, pwd);              
                CRSIniFile.SetPropertyValue(APPConstants.SEC_APPL, APPConstants.PROP_LANGUAGE, comboLang.Text);

                string pwdEmail = "";
                if (ctrlPwdEmail.GetControlValue() != null && !string.IsNullOrEmpty(Convert.ToString(ctrlPwdEmail.GetControlValue())))
                    pwdEmail = ctrlPwdEmail.GetControlValue().ToString();
                else 
                {
                    pwdEmail = pwd;
                }
                pwdEmail = CRSLicense.EncodeRegCode(appName, appVers, pwdEmail, 1);
                CRSIniFile.SetPropertyValue(APPConstants.SEC_MYBLACKBOX, APPConstants.PROP_EMAIL_PWD, pwd);

                if (this.IsRemoteDirChanged)
                {
                    CRSIniFile.SetPropertyValue(APPConstants.SEC_MYBLACKBOX, APPConstants.PROP_LASTSYNCRO_OWNER, 0);
                    //CRSIniFile.SetPropertyValue(APPConstants.SEC_MYBLACKBOX, APPConstants.PROP_LASTSYNCRO_OWNER, 0);
                }

                //CRSIniFile.SetPropertyValue(APPConstants.SEC_MYBLACKBOX, APPConstants.PROP_SYNCRO_WITH_EMAIL, chkSyncroWithEmail.IsChecked.ToString());
                CRSIniFile.SetPropertyValue(APPConstants.SEC_MYBLACKBOX, APPConstants.PROP_SYNCRO_WITH_EMAIL, chkSyncroWithEmail.GetControlValue().ToString());

            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("Save Error: " + ex.Message, "");
            }
        }

        public void Fill()
        {
            try
            {
                // NB: da 03/2018 assume il significato di directory per note condivise:
                // all'interno ci sono le cartelle con gli utenti con cui condivido preceduti da SHARED_: per ogni cartella ci sono i file syncroDbUPD.txt, DEL, DOCS
                // ci sarà una cartella generica (DOCS pesr esempio) che conterrà tutti i documenti condivisi 
                // quindi:
                //  - per le note c'è una cartella per singolo utente (syncro_nomeutente)
                //  - per i documenti c'è un'unica cartella di condivisione se il documento è condiviso (DOCS)
                //  - per i docs NON condivisi c'è una cartella personale (nomeutente)
                ctrlDirSharing.SetControlValue(CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_SHARINGDIR));
                //ctrlDirStorageShared.SetControlValue(CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_SHAREDDIR));  
                ctrlDirStorageDropBox.SetControlValue(CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_DROPBOXDIR));  
                ctrlDirStorageTemp.SetControlValue(CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_LOCALTMPDIR));
                ctrlAutoSync.SetControlValue(CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_AUTOSYNC,1));
                ctrlAutoSave.SetControlValue(CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_AUTOSAVE, 1));
                ctrlTimerSyncro.SetControlValue(CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_TIMER_SYNCRO,30));
                ctrlTimerSyncroForDocs.SetControlValue(CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_TIMER_SYNCRO_DOCS,15));
                ctrlConfirmSyncroOp.SetControlValue(CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_CONFIRMOP));
                ctrlIsUniqueFileName.SetControlValue(CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_ISUNIQUEFILENAME,1));
                ctrlDaysSyncro.SetControlValue(CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_SYNCRO_DAYS, 5));
                ctrlNewFileExtension.SetControlValue(CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_NEWFILEEXTENSION, "doc"));
                ctrlExportDaysNotes.SetControlValue(CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_FILE_DIM_DAYS, 365) );
                //ctrlSeparationCharTitle.SetControlValue(CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_SEPARTATION_CHAR_TITLE, " "));
                string sep = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_SEPARTATION_CHAR_TITLE, " ");
                if (string.IsNullOrEmpty(sep))
                {
                    rbSepChar.IsChecked = false;
                    rbSepCharSpace.IsChecked = true;
                }
                txtSepCharTitle.Text = sep;

                string val = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_SYNCROFILES,true);
                if(string.IsNullOrEmpty(val)==false)
                    chkVersionAndSyncro.IsChecked = Convert.ToBoolean(val); 
                              
                val = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_OPNOOVERWRITEOWNER);
                
                bool chk = false;
                if (string.IsNullOrEmpty(val) == false)
                    chk = Convert.ToBoolean(val);
                rbSyncOwner.IsChecked = chk;
                rbOverwriteOldVer.IsChecked = !chk;
                
                ctrlUser.SetControlValue(CRSIniFile.GetPropertyValue(APPConstants.SEC_APPL,APPConstants.PROP_USER));
                string pwd = CRSIniFile.GetPropertyValue(APPConstants.SEC_APPL, APPConstants.PROP_USERPWD);
                string appName = CRSIniFile.GetApplicationNameFromIni();
                string appVers = CRSIniFile.GetApplicationVersionFromIni();
                pwd = CRSLicense.DecodeCode(appName, appVers, pwd, 1);
                ctrlPwd.SetControlValue(pwd);

                this.currentIdUser = DbDataAccess.GetIDUser(ctrlUser.GetControlValue().ToString());

                this.lang = CRSIniFile.GetLanguageFromIni();
                InitLanguage(this.lang);
                comboLang.ItemsSource = DbDataAccess.GetLanguages();
                comboLang.Text = this.lang;

                txtVersion.Text = GetPublishedVersion();
                txtSmtp.Text = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_EMAIL_SERVER_SMTP);
                txtSmtpPort.Text = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_EMAIL_SERVER_SMTP_PORT);
                txtImap.Text = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_EMAIL_SERVER_IMAP);
                txtImapPort.Text = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_EMAIL_SERVER_IMAP_PORT);
                txtPop3.Text = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_EMAIL_SERVER_POP3);
                txtPop3Port.Text = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_EMAIL_SERVER_POP3_PORT);

                string pwdemail = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_EMAIL_PWD);
                pwdemail = CRSLicense.DecodeCode(appName, appVers, pwdemail, 1);
                ctrlPwdEmail.SetControlValue(pwdemail);

                //val = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_SYNCRO_WITH_EMAIL);
                //if (!string.IsNullOrEmpty(val))
                //    chk = Convert.ToBoolean(val);
                //chkSyncroWithEmail.SetControlValue(chk);
                chkSyncroWithEmail.SetControlValue(CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_SYNCRO_WITH_EMAIL, false));
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("Save Error: " + ex.Message, "");
            }
        }

        private string GetPublishedVersion()
        {
            if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
            {
                return System.Deployment.Application.ApplicationDeployment.CurrentDeployment.
                    CurrentVersion.ToString();
            }
            else
                return CRSIniFile.GetApplicationVersionFromIni(); ;

        }


        public void FillSyncroPendings(List<MBBSyncroPending> pendings)
        {
            ctrlMBBSyncroPendings.FillDocsSyncro(pendings);
        }

        private void btnSyncroShared_Click(object sender, RoutedEventArgs e)
        {
            Mouse.SetCursor(Cursors.Wait);
            //-- call syncrodb delete
            int numdays = Convert.ToInt32(ctrlDaysSyncro.GetControlValue());
            CRSIniFile.SetPropertyValue(APPConstants.SEC_MYBLACKBOX, APPConstants.PROP_LASTSYNCRO_SHARED, DateTime.Now.AddDays(-numdays).ToFileTime());
            string user = DbDataAccess.GetUser(this.currentIdUser);
            bool res1 = MBBSyncroUtility.SyncroDBFromFileShared(user);
            Mouse.SetCursor(Cursors.Arrow);
        }

        private void ctrlUser_ContentControlChanged(object sender, EventArgs e)
        {
            this.IsUserChanged = true;
            Tuple<string ,string> smtp = CRSMailRepository.GetEmailDefaultParams(ctrlUser.GetControlValue().ToString(), "smtp");
            Tuple<string, string> imap = CRSMailRepository.GetEmailDefaultParams(ctrlUser.GetControlValue().ToString(), "imap");
            Tuple<string, string> pop3 = CRSMailRepository.GetEmailDefaultParams(ctrlUser.GetControlValue().ToString(), "pop3");
            txtSmtp.Text = smtp.Item1;
            txtSmtpPort.Text = smtp.Item2;
            txtImap.Text = imap.Item1;
            txtImapPort.Text = imap.Item2;
            txtPop3.Text = pop3.Item1;
            txtPop3Port.Text = pop3.Item2;
        }

        private void ctrlDirStorage_ContentControlChanged(object sender, EventArgs e)
        {
            this.IsRemoteDirChanged = true;
        }

        private void btnSyncroOwner_Click(object sender, RoutedEventArgs e)
        {
            Mouse.SetCursor(Cursors.Wait);
            //-- call syncrodb delete
            int numdays = Convert.ToInt32(ctrlDaysSyncro.GetControlValue());
            CRSIniFile.SetPropertyValue(APPConstants.SEC_MYBLACKBOX, APPConstants.PROP_LASTSYNCRO_OWNER, DateTime.Now.AddDays(-numdays).ToFileTime());

            string info;
            string user = DbDataAccess.GetUser(this.currentIdUser);
            DateTime newdtsyncro;
            bool res1 = MBBSyncroUtility.SyncroDBFromFileOwner(user, DateTime.Now.AddDays(-numdays), 0, out info, out newdtsyncro );
            CRSLog.WriteLog("Notes imported by click Syncro Owner in Params: " + info + "(days before now: " + numdays.ToString() + ")", "MBB");
            Mouse.SetCursor(Cursors.Arrow);
        }

        private void chkVersionAndSyncro_Click(object sender, RoutedEventArgs e)
        {
            gbSync.IsEnabled = chkVersionAndSyncro.IsChecked.Value;
            ctrlTimerSyncro.IsEnabled = chkVersionAndSyncro.IsChecked.Value;
            ctrlTimerSyncroForDocs.IsEnabled = chkVersionAndSyncro.IsChecked.Value;
            ctrlIsUniqueFileName.IsEnabled = chkVersionAndSyncro.IsChecked.Value;
        }

        private void btnChangePwd_Click(object sender, RoutedEventArgs e)
        {
            pnlChangePwd.Visibility = System.Windows.Visibility.Visible;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            bool res = SaveNewPwd();
            if (res) pnlChangePwd.Visibility = System.Windows.Visibility.Collapsed;
        }

        private bool SaveNewPwd()
        {
            ClearErrors();
            bool res = false;
            bool isValidO = ctrlNewPwd.ValidateControl();
            bool isValidN = ctrlOldPwd.ValidateControl();
            bool isValidC = ctrlConfirmPwd.ValidateControl();

            bool isValid = (isValidO) && (isValidN) && (isValidC);

            if (isValid)
            {
                isValid = (ctrlConfirmPwd.GetControlValue().ToString() == ctrlNewPwd.GetControlValue().ToString());
                if (isValid == false)
                {
                    //textBlockError.Text = "Confirmation error!! Retype confirm..";
                    ctrlConfirmPwd.SetValidationError("Confirmation error!! Retype confirm..");
                }
            }

            if (isValid)
            {
                string provider = CRSIniFile.GetProviderFromIni(APPConstants.SEC_DBSERVER);
                string connstr = CRSIniFile.GetConnectionStringFromIni(APPConstants.SEC_DBSERVER);
                DbFactory connSql = new DbFactory(connstr, provider);
                int c = connSql.GetRecordCount("select count(*) from userapp where name=@s and password=@s",
                             new object[] { Convert.ToString(ctrlUser.GetControlValue()), Convert.ToString(ctrlOldPwd.GetControlValue()) });

                if (c == 0)
                {
                    isValid = false;
                    ctrlOldPwd.SetValidationError("Old password or user invalid!");
                    textBlockErrChangePwd.Text = "Old password or user invalid!";
                }
                else
                {
                    /*res = Convert.ToInt32(connSql.GetExecuteScalar("select id from userapp where name=@s"
                                        , Convert.ToString(ctrlUser.GetControlValue())));*/
                    //-- inserisco dati nel db
                    connSql.ExecUpdSql("update userapp set password=@s, oldpassword=@s where name=@s",
                            new object[]{Convert.ToString(ctrlNewPwd.GetControlValue()),
                                        Convert.ToString(ctrlOldPwd.GetControlValue()),
                                        Convert.ToString(ctrlUser.GetControlValue())});

                    
                    res = true;
                }
                ctrlPwd.SetControlValue(ctrlNewPwd.GetControlValue());               
            }

            return res;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ClearErrors();
            pnlChangePwd.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void ClearErrors()
        {
            ctrlUser.ClearValidationError();
            ctrlPwd.ClearValidationError();
            ClearErrorsNewPwd();
        }

        private void ClearErrorsNewPwd()
        {
            ctrlConfirmPwd.ClearValidationError();
            ctrlNewPwd.ClearValidationError();
            ctrlOldPwd.ClearValidationError();
            textBlockErrChangePwd.Text = "";
        }

        private void btnExportNotesOwner_Click(object sender, RoutedEventArgs e)
        {
            Mouse.SetCursor(Cursors.Wait);
            int numDays = 5000;
            if (ctrlExportDaysNotes.GetControlValue() != null)
                numDays = Convert.ToInt32(ctrlExportDaysNotes.GetControlValue());
            TaskExportNotes(numDays);
            Mouse.SetCursor(Cursors.Arrow);
        }

        public Thread StartThreadSyncro(int numdays)
        {
            Thread newt = new Thread(new ParameterizedThreadStart(TaskExportNotes));
            newt.SetApartmentState(ApartmentState.STA);
            newt.Name = "ThreadIntro";
            newt.IsBackground = false;
            newt.Start(numdays);
            return newt;
        }

        private void TaskExportNotes(object numDays)
        {
            MBBUserInfo user = new MBBUserInfo();
            try
            {             
                WinMBBIntro wi = new WinMBBIntro(user, ExportNotes);
                wi.ActivateTypeSyncro(false, false, false, true, Convert.ToInt32(numDays));
                wi.Show();            
                System.Windows.Threading.Dispatcher.Run();
                
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("Exception in TaskSyncroNotes: " + ex.Message, user.Name);
            }
        }
    

        public void ExportNotes(int numdays)
        {
            string userOwner = DbDataAccess.GetUser(this.currentIdUser);
            MBBCommon.syncroParams.InitParamsFromIni();
            string remoteDir = MBBSyncroUtility.GetUserRemoteDir(userOwner, false, MBBCommon.syncroParams);

            var dlg = new System.Windows.Forms.FolderBrowserDialog();
            dlg.SelectedPath = remoteDir;
            string newdirname = remoteDir;
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                newdirname = dlg.SelectedPath; ;
                MBBFindParams p = new MBBFindParams();
                //int numdays = 5000;
                //if (ctrlExportDaysNotes.GetControlValue() != null)
                //    numdays = Convert.ToInt32(ctrlExportDaysNotes.GetControlValue());
                p.Dt1 = DateTime.Now.AddDays(-numdays);
                p.Dt2 = DateTime.Now;
                p.IDUser = this.currentIdUser;

                // esporto solo le note dell'utente: gli altri utenti avranno le note in un'altra cartella
                List<DbNote> notes = DbDataAccess.GetNotesAndDocsFilterModified(p.IDUser, p.Dt1, p.Dt2, 0, ">=", "", false, false);

                //string remoteDir = MBBSyncroUtility.GetUserRemoteDir(userOwner, false, MBBCommon.syncroParams);

                // 1) fa backup nella stessa directory con nuovo nome (nome prec + _anno mese giorno ora) se l'utente scegliere di sportare nella stessa directory
                if (remoteDir.ToLower() == newdirname.ToLower())
                {
                    string nameFile = MBBSyncroUtility.GetNameFileSync(MBBConst.SYNCRO_DOCS, remoteDir);
                    CRSFileUtil.BackupFile(remoteDir, nameFile);
                    // 2) elimino il file
                    File.Delete(nameFile);

                    nameFile = MBBSyncroUtility.GetNameFileSync(MBBConst.SYNCRO_DEL, remoteDir);
                    CRSFileUtil.BackupFile(remoteDir, nameFile);
                    File.Delete(nameFile);

                    nameFile = MBBSyncroUtility.GetNameFileSync(MBBConst.SYNCRO_UPD, remoteDir);
                    CRSFileUtil.BackupFile(remoteDir, nameFile);
                    File.Delete(nameFile);

                    nameFile = MBBSyncroUtility.GetNameFileSync(MBBConst.SYNCRO_ACTIVITIES, remoteDir);
                    CRSFileUtil.BackupFile(remoteDir, nameFile);
                    File.Delete(nameFile);

                    nameFile = MBBSyncroUtility.GetNameFileSync(MBBConst.SYNCRO_ACTIVITIES_LOG, remoteDir);
                    CRSFileUtil.BackupFile(remoteDir, nameFile);
                    File.Delete(nameFile);
                }
                bool res1 = notes.Count()==0; bool res2 = true; bool res3 = true; bool res4 = true;
                CRSIni fileSyncroDB = MBBSyncroUtility.GetIniFileSync(MBBConst.SYNCRO_UPD, newdirname);
                CRSIni fileSyncroDB_Doc = MBBSyncroUtility.GetIniFileSync(MBBConst.SYNCRO_DOCS, newdirname);
                CRSIni fileSyncroDB_Act = MBBSyncroUtility.GetIniFileSync(MBBConst.SYNCRO_ACTIVITIES, newdirname);
                CRSIni fileSyncroDB_ActLog = MBBSyncroUtility.GetIniFileSync(MBBConst.SYNCRO_ACTIVITIES_LOG, newdirname);
                // 3) riempio il file
                foreach (DbNote n in notes)
                {
                    /*MBBSyncroUtility.WriteNoteSyncroFile(n.ID, n.Title, n.Text, n.Rate, n.DateTimeInserted, n.DateTimeInserted, userOwner, 0, "", n.IsFavorite, MBBCommon.syncroParams);
                    MBBSyncroUtility.WriteNoteSyncroFileShared(n.ID, n.Title, n.Text, n.Rate, n.DateTimeInserted, n.DateTimeInserted, userOwner, n.SharedUsers, 0, "", n.IsFavorite, MBBCommon.syncroParams);
                    MBBSyncroUtility.WriteDocsSyncroFile(n.ID, n.DateTimeInserted, userOwner, null, n.Docs, MBBCommon.syncroParams);
                    MBBSyncroUtility.WriteDocsSyncroFileShared(n.ID, n.DateTimeInserted, userOwner, n.Docs, n.SharedUsers , MBBCommon.syncroParams);*/
                    res1 = MBBSyncroUtility.WriteNoteSyncroFileNewFile(n.ID, n.Title, n.Text, n.Rate, n.DateTimeInserted, n.DateTimeLastMod, userOwner, n.IDNoteParent, n.UserNameParent, n.IsFavorite, fileSyncroDB);
                    res2 = MBBSyncroUtility.WriteDocsSyncroFileNewFile(n.ID, n.DateTimeInserted, userOwner, n.SharedUsers, n.Docs, fileSyncroDB_Doc);
                }

                List<DbActivity> act = DbDataAccess.GetActivities(this.currentIdUser, p.Dt1, p.Dt2, null, 0, false);
                foreach (DbActivity a in act)
                {
                    res3 = MBBSyncroUtility.WriteActivitySyncroFileNewFile(userOwner, a, fileSyncroDB_Act);
                }
                List<DbActivityLog> actl = DbDataAccess.GetActivityLogs(p.Dt1, p.Dt2, false);
                foreach (DbActivityLog l in actl)
                {
                    res4 = MBBSyncroUtility.WriteActivityLogSyncroFileNewFile("U", userOwner, l.StartActivityLog, l, fileSyncroDB_ActLog);
                }
                if (res1 && res2 && res3 && res4)
                    System.Windows.MessageBox.Show(CRSTranslation.TranslateDialog(CRSTranslation.MSG_COMMAND_EXECUTED_CORRECTLY, UserGlobalInfo.Language), "");
                else
                    System.Windows.MessageBox.Show(CRSTranslation.TranslateDialog(CRSTranslation.MSG_OPERATION_FAILS, UserGlobalInfo.Language), "");

            }
        }

        private void btnModSepDB_Click(object sender, RoutedEventArgs e)
        {
            string oldchar = txtCurrentTitleSep.Text;
            string newchar = txtNewTitleSep.Text;

            MessageBoxResult r = System.Windows.MessageBox.Show(CRSTranslation.TranslateDialog(CRSTranslation.MSG_MOD_SEPARATOR_TITLE, UserGlobalInfo.Language),
                                                     CRSTranslation.TranslateDialog(CRSTranslation.MSG_WARNING, UserGlobalInfo.Language),
                                                     MessageBoxButton.YesNo);
            if (r == MessageBoxResult.Yes)
            {
                DbDataUpdate.UpdateSeparatorTitleNote(oldchar, newchar, this.currentIdUser);
            }
        }

        private void BtnExecScript_Click(object sender, RoutedEventArgs e)
        {
            string provider = CRSIniFile.GetProviderFromIni(APPConstants.SEC_DBSERVER);
            string connstr = CRSIniFile.GetConnectionStringFromIni(APPConstants.SEC_DBSERVER);
            DbFactory connSql = new DbFactory(connstr, provider);
            bool res = connSql.ExecuteCommandSql(txtScriptDB.Text);
            if (res)
                MessageBox.Show(CRSTranslation.TranslateDialog(CRSTranslation.MSG_COMMAND_EXECUTED_CORRECTLY, UserGlobalInfo.Language),
                                                     CRSTranslation.TranslateDialog(CRSTranslation.MSG_WARNING, UserGlobalInfo.Language));

        }

        private void btnGetAppIni_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txtPathsInfo.Text = txtPathsInfo.Text + "\n" + CRSIniFile.IniPathName;
                Process.Start(CRSIniFile.IniPathName);
            }
            catch { }
        }

        private void btnGetLog_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txtPathsInfo.Text = txtPathsInfo.Text + "\n" + CRSGlobalParams.LogPathName;
                Process.Start(CRSGlobalParams.LogPathName);
            }
            catch { }
        }

        private void btnGetLogListener_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string filename = (string)CRSIniFile.GetPropertyValue("EVENTS", "LogFileName");

                if (filename == null)
                {
                    filename = CRSGlobalParams.ApplicationPath + "\\Log\\EventsUserLog.txt";
                }
                txtPathsInfo.Text = txtPathsInfo.Text + "\n" + filename;
                Process.Start(filename);
            }
            catch { }
        }

        private void btnChangePwdEmail_Click(object sender, RoutedEventArgs e)
        {
            pnlChangePwdEmail.Visibility = System.Windows.Visibility.Visible;
        }

        private void btnSavePwsEmail_Click(object sender, RoutedEventArgs e)
        {
            SaveNewPwdEmail();
            pnlChangePwdEmail.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void SaveNewPwdEmail()
        {
            string pwd = ctrlNewPwdEmail.GetControlValue().ToString();
            string appName = CRSIniFile.GetApplicationNameFromIni();
            string appVers = CRSIniFile.GetApplicationVersionFromIni();
            pwd = CRSLicense.EncodeRegCode(appName, appVers, pwd, 1);
            
            CRSIniFile.SetPropertyValue(APPConstants.SEC_MYBLACKBOX, APPConstants.PROP_EMAIL_PWD, pwd);
        }

        private void btnCancelPwdEmail_Click(object sender, RoutedEventArgs e)
        {
            pnlChangePwdEmail.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void comboServerEmail_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Tuple<string, string> t1 = CRSMailRepository.GetEmailDefaultParams(comboServerEmail.SelectedValue.ToString(), "smtp");
            Tuple<string, string> t2 = CRSMailRepository.GetEmailDefaultParams(comboServerEmail.SelectedValue.ToString(), "imap");

            txtSmtp.Text = t1.Item1;
            txtImap.Text = t2.Item1;
            txtSmtpPort.Text = t1.Item2;
            txtImapPort.Text = t2.Item2;
        }

        private void btnImportNotes_Click(object sender, RoutedEventArgs e)
        {
            Mouse.SetCursor(Cursors.Wait);

            DateTime newdtsyncro;
            string filename = Convert.ToString(ctrlFileNameImport.GetControlValue());
            CRSIni fileSyncroDB_UPD = new CRSIni(filename);
            string infonotes;
            bool res1 = MBBSyncroUtility.ReadFileSaveToDBUpdNotes(fileSyncroDB_UPD, DateTime.MinValue, 0, out infonotes, out newdtsyncro, true);
            CRSLog.WriteLog("Notes imported by click Import Notes in Params: " + infonotes, "MBB");
            Mouse.SetCursor(Cursors.Arrow);
        }

        
    }
}
