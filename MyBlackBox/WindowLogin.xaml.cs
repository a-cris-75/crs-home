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
using System.Configuration;
using System.Data;
using System.IO;
using System.Reflection;
using CRS.DBLibrary;
using System.Data.SqlServerCe;
using System.Windows.Navigation;

namespace MyBlackBoxCore
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class WindowLogin : Window
    {
        readonly string provider;
        readonly string connString;
        readonly string lang;
        public delegate void EventLoginFailure();
        private EventLoginFailure eventLoginFailure;
        
        public WindowLogin()
        {
            InitializeComponent();
            string assemblyname = Assembly.GetExecutingAssembly().GetName().Name;
            MBBCommon.InitApplicationInstall(assemblyname);

            //-- la stringa di connessione inizialmente punto sul server, eventualmente in seguito prova offline
            this.provider = CRSIniFile.GetProviderFromIni(APPConstants.SEC_DBSERVER);
            this.connString = CRSIniFile.GetConnectionStringFromIni(APPConstants.SEC_DBSERVER);            
            this.lang = CRSIniFile.GetLanguageFromIni();

            ctrlLogin.InitIni();
            //-- Event Exit controlla la licenza oltre ad aprire la form principale dell'applicazione
            bool res = ctrlLogin.InitControlParams(connString, provider, this.lang, EventCheckLicense, EventInsUpdUser, SyncroDBUsers);

            //-- non gestisco il db offline
            if (ctrlLogin.IsOffline)
            {
                this.provider = CRSIniFile.GetProviderFromIni(APPConstants.SEC_DBSERVER_OFFLINE);
                this.connString = CRSIniFile.GetConnectionStringFromIni(APPConstants.SEC_DBSERVER_OFFLINE);
                res = ctrlLogin.InitControlParams(connString, provider, this.lang, EventCheckLicense, EventInsUpdUser, SyncroDBUsers);
            }

            if (res)
            {
                //-- E' SUPERFLUO DATO CHE IN ctrlLogin PASSANDO L'EVENTO DI CONTROLLO DELLA LICENZA, questo stesso viene fatto
                //-- all'interno del controllo
                
                //-- controllo se l'utente è impostato su file ini:
                //-- se si accedo con l'utente controllando la licenza, altrimenti mostro questa form 
                if (ctrlLogin.Login())
                {
                    //-- se è tutto ok chiamo EventExit che controlla la licenza
                    //-- se anche la licenza è ok, chiude tutto e apre MBBCore
                    bool isValidLicense = EventCheckLicense();
                }
                else
                {
                    //-- in realtà non ho ancora la lingua finchè l'utente non digita il suo nome
                    //ctrlLogin.InsertDefaultData();
                }
            }
            // InitControlParams restituisce false e non c'è connessione al DB
            // potrebbe essere necessario aggiornare la versione di sql server ce alla 4.0
            else
            {
                if (!this.UpgradeDB_SqlCE35_to40(connString))
                {
                    MessageBoxResult br = System.Windows.MessageBox.Show(CRSTranslation.TranslateDialog(CRSTranslation.MSG_INSTALL_DB, this.lang),
                                                     CRSTranslation.TranslateDialog(CRSTranslation.MSG_WARNING, this.lang),
                                                     MessageBoxButton.OK);
                    this.Hide();
                    if (br == MessageBoxResult.OK)
                    {
                        WindowInstallDB win = new WindowInstallDB();
                        win.Show();
                        //this.Close();
                    }
                }
                else
                {
                    res = ctrlLogin.InitControlParams(connString, provider, this.lang, EventCheckLicense, EventInsUpdUser, SyncroDBUsers);
                    // ci riprovo
                    if (ctrlLogin.Login())
                    {
                        bool isValidLicense = EventCheckLicense();
                    }
                }
            }
        }

        private bool UpgradeDB_SqlCE35_to40(string connStringNamedb)
        {
            bool res = false;
            try
            {
                SqlCeEngine engine = new SqlCeEngine(@connStringNamedb);
                // The collation of the database will be case sensitive because of 
                // the new connection string used by the Upgrade method.       
                string namedb = connStringNamedb.Substring(0, connStringNamedb.IndexOf(".sdf"));
                string connstrnew = namedb;
                namedb = namedb.Replace("Data Source= ", "").Replace("data source =","").Replace("User=","").Replace("Password=","");
                //string connstringrest = connStringNamedb.Substring(connStringNamedb.IndexOf(".sdf") + 4);

                if(File.Exists(namedb + "_new.sdf"))
                    File.Delete(namedb + "_new.sdf");      
                
                engine.Upgrade(connstrnew + "_new.sdf");
                // connStringNamedb corrisponde al nome del file
                File.Copy(namedb + ".sdf", namedb + "35.sdf", true);
                File.Copy(namedb + "_new.sdf", namedb + ".sdf",true);
                res = true;
            }
            catch(Exception ex)
            {
                res = false;
                CRSLog.WriteLog("Exception UpgradeDB: " + ex.Message, "MBB");
            }
            return res;
        }

        public void SetEventLoginFailure(EventLoginFailure eventClose)
        {
            eventLoginFailure = eventClose;
        }

        private bool EventCheckLicense()
        {
            //-- controllo sulla licenza
            
            bool isValidLicense = false;
            UserGlobalInfo.RegistrationName = CRSIniFile.GetRegistrationNameFromIni();
            if (UserGlobalInfo.RegistrationName == CRSGlobalParams.FREE_ACTIVATION_REGISTRY_NAME)
                //-- controlla solo numero DialogResult utenti nel DB escluso utente root
                isValidLicense = CheckFreeLicense();
            else isValidLicense = CheckLicense();

            if (isValidLicense == false)
            {
                MessageBoxResult br = System.Windows.MessageBox.Show(CRSTranslation.TranslateDialog(CRSTranslation.MSG_LICENCE_ERROR, this.lang),
                                                 CRSTranslation.TranslateDialog(CRSTranslation.MSG_WARNING, this.lang),
                                                 MessageBoxButton.OKCancel);
                this.Hide();
                if (br == MessageBoxResult.OK)
                {
                    //this.Visibility = System.Windows.Visibility.Hidden;
                    WindowLicenseAct win = new WindowLicenseAct(this.connString,this.provider);
                    win.ShowDialog();
                    this.Close();
                    //return;
                }
                if (br == MessageBoxResult.Cancel)
                {
                    //this.Visibility = System.Windows.Visibility.Hidden;
                    if (eventLoginFailure != null) eventLoginFailure();
                    this.Close();
                    //return;
                }
            }
            else
            {
                this.Hide();
                WinMBBCore mainWindow = new WinMBBCore();
                //-- serve per passare il riferimento alla finestra di login che deve essere chiusa quando chiudo la main
                //mainWindow.InitApplication(this, ctrlLogin.User, ctrlLogin.IsOffline);
                mainWindow.Show();
                //this.Hide();
                //-- potrei forzare la chiusura dall'esterno
                if (this != null) this.Close();
            }

            //isValid = false;
            //-- dovrebbe essere superfluo dato che prima di fare il test sulla licenza controllo che il login sia valido
            //if (isValidLicense)
            //    isValid = ctrlLogin.IsValid;


            return (isValidLicense);
        }

        private void EventInsUpdUser(int iduser, string username, string pwd, string lang, string group, DateTime dtIns, DateTime dtUpd, string operation)
        {
            //MBBP
            bool founddir = false;
            string remdir = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_SHARINGDIR);
            if (string.IsNullOrEmpty(remdir) == false && Directory.Exists(remdir))
                founddir = true;

            if (founddir)
            {
                CRSIni fileSyncroDB = MBBSyncroUtility.GetIniFileSync(MBBConst.SYNCRO_USERS, CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_SHARINGDIR));
                if (string.IsNullOrEmpty(group)) group = "";
                if (string.IsNullOrEmpty(operation)) operation = "";
                if (dtIns == DateTime.MinValue) dtIns = DateTime.Now;
                if (dtUpd == DateTime.MinValue) dtUpd = DateTime.Now;
                //-- devo scrivere su file di sincronizzazione i dati del nuovo utente iscritto
                fileSyncroDB.SetValue(iduser.ToString(), "name", username);
                fileSyncroDB.SetValue(iduser.ToString(), "pwd", pwd);
                fileSyncroDB.SetValue(iduser.ToString(), "lang", lang);
                fileSyncroDB.SetValue(iduser.ToString(), "group", group);
                fileSyncroDB.SetValue(iduser.ToString(), "operation", operation);
                fileSyncroDB.SetValue(iduser.ToString(), "datetimeIns", dtIns.ToFileTime().ToString());
                fileSyncroDB.SetValue(iduser.ToString(), "datetimeUpd", dtUpd.ToFileTime().ToString());
            }
        }

        private bool SyncroDBUsers()
        {
            bool res = true;
            try
            {
                string remdir = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_SHARINGDIR);
                if (string.IsNullOrEmpty(remdir) == false && Directory.Exists(remdir))
                    res = false;

                if (res)
                {
                    CRSIni fileSyncroDB = MBBSyncroUtility.GetIniFileSync(MBBConst.SYNCRO_USERS, CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_SHARINGDIR));
                    // potrebbe essere che il file è null: capita al primo avvio dell'applicazione quando l'utente non ha ancora selezionato le cartelle di destinazione dei file in drop box
                    if (fileSyncroDB != null)
                    {
                        int idx = 1;
                        string secid = fileSyncroDB.GetNextSection(ref idx);
                        while (string.IsNullOrEmpty(secid) == false)
                        {
                            int id = Convert.ToInt32(secid);
                            //-- indica operazione di tipo: insert(I),update(U),delete(D)
                            string operation = fileSyncroDB.GetValue(id.ToString(), "operation").ToString();
                            string name = fileSyncroDB.GetValue(id.ToString(), "name").ToString();
                            string pwd = fileSyncroDB.GetValue(id.ToString(), "pwd").ToString();
                            string group = fileSyncroDB.GetValue(id.ToString(), "group").ToString();
                            string lang = fileSyncroDB.GetValue(id.ToString(), "lang").ToString();

                            object o = fileSyncroDB.GetValue(id.ToString(), "datetimeIns");
                            DateTime dtins = DateTime.MinValue;
                            if (o != null)
                                dtins = DateTime.FromFileTime(Convert.ToInt64(o));

                            o = fileSyncroDB.GetValue(id.ToString(), "datetimeUpd");
                            DateTime dtupd = DateTime.MinValue;
                            if (o != null)
                                dtupd = DateTime.FromFileTime(Convert.ToInt64(o));

                            bool res2 = DbDataUpdate.InsUpdDelUser(id, name, pwd, lang, group, dtins, dtupd, operation);

                            secid = fileSyncroDB.GetNextSection(ref idx);
                        }
                    }
                }
            }
            catch { }

            return res;
        }
        /*
        public void CloseLogin()
        {
            if (mainWindow != null) mainWindow.Close();
            this.Close();
        }*/

        #region manage LICENSE
        //-- recupera info licenza da file ini: se è free controlla solo il numero di utenti 
        //-- salva nei parametri utente le info su licenza e numero max utenti
        private bool CheckFreeLicense()
        {
            DbFactory connSqlApp = new DbFactory(this.connString, this.provider);
            string appName = CRSIniFile.GetApplicationNameFromIni();
            string appVers = CRSIniFile.GetApplicationVersionFromIni();
            bool res = true;
            //-- 1) recupera dati da file ini
            UserGlobalInfo.RegistrationName = CRSIniFile.GetRegistrationNameFromIni();
            UserGlobalInfo.ActivationCode = CRSIniFile.GetActivationCodeFromIni();
            UserGlobalInfo.LicenseUsersNo = Convert.ToInt32(CRSGlobalParams.FREE_USERS_NO.TrimStart('0'));//CRSIniFile.GetUsersNoFromIni();

            try
            {
                if (UserGlobalInfo.RegistrationName == CRSGlobalParams.FREE_ACTIVATION_REGISTRY_NAME)
                //-- controlla il numero di utenti nel db
                {
                    int numusers = connSqlApp.GetRecordCount("select count(*) from userapp where id<>0");
                    if (numusers > UserGlobalInfo.LicenseUsersNo)
                        res = false;
                }
            }
            catch
            {
                res = false;
            }

            return res;
        }

        //-- recupera licenza da file ini o da DB
        //-- fai check e controlla data nel caso di trial version
        //-- salva nei parametri utente le info su licenza valida o scaduta e numero max utenti
        private bool CheckLicense()
        {
            DbFactory connSqlApp = new DbFactory(this.connString, this.provider);
            string appName = CRSIniFile.GetApplicationNameFromIni();
            string appVers = CRSIniFile.GetApplicationVersionFromIni();
            bool res = true;
            //-- 1) recupera dati da file ini
            UserGlobalInfo.RegistrationName = CRSIniFile.GetRegistrationNameFromIni();
            UserGlobalInfo.ActivationCode = CRSIniFile.GetActivationCodeFromIni();
            UserGlobalInfo.LicenseUsersNo = CRSIniFile.GetUsersNoFromIni();

            try
            {
                //-- recupero info su data da codice di attivazione:
                //-- se esiste il codice di attivazione deduco il codice di registrazione
                string regCode = "";
                if (string.IsNullOrEmpty(UserGlobalInfo.ActivationCode) == false)
                {
                    regCode = CRSLicense.DecodeCode(appName, appVers, UserGlobalInfo.ActivationCode, UserGlobalInfo.LicenseUsersNo);
                    UserGlobalInfo.StartTrialLicenseDate = new DateTime(Convert.ToInt32(regCode.Substring(0, 4)), Convert.ToInt32(regCode.Substring(4, 2)), Convert.ToInt32(regCode.Substring(6, 2)));
                }
                //-- se non esiste un codice di attivazione oppure la versione è trial, imposta il codice su file ini
                if ((string.IsNullOrEmpty(UserGlobalInfo.ActivationCode)) || (UserGlobalInfo.RegistrationName == CRSGlobalParams.TRIAL_ACTIVATION_REGISTRY_NAME))
                {
                    string actCode = RetrieveLicenceFromDB(appName, appVers);
                    //-- solo se nel db non c'è niente e il codice ancora non esiste lo rigenera altrimenti mantiene quello del file ini
                    if ((string.IsNullOrEmpty(actCode)) && (string.IsNullOrEmpty(UserGlobalInfo.ActivationCode)))
                    {
                        //-- imposta i parametri globali in UserGlobalInfo
                        actCode = GenerateTrialCode(appName, appVers);
                        //-- salva i dati nel DB
                        CRSParameter p = new CRSParameter(connSqlApp);
                        p.SetParameter(CRSGlobalParams.PARAMS_KEY_ACTIVATION, CRSGlobalParams.PARAMSGROUP_APPLICATION_DATA, UserGlobalInfo.ActivationCode, 
                                        Convert.ToString(UserGlobalInfo.LicenseUsersNo)/*null*/, UserGlobalInfo.RegistrationName, 1, true);
                        SetActivationInIniFile(actCode, UserGlobalInfo.RegistrationName, UserGlobalInfo.LicenseUsersNo);
                    }

                    if (string.IsNullOrEmpty(actCode) == false)
                        SetActivationInIniFile(actCode, UserGlobalInfo.RegistrationName, UserGlobalInfo.LicenseUsersNo);
                }

                //-- il codice di registazione codificato mi restituisce il codice di attivazione
                //-- viceversa il codice di attivazione decodificato restituisce il codice di registrazione
                //-- il codice di reg. è così fatto: anno,mese,giorno,nome di registrazione,numero di utenti per licenza
                regCode = UserGlobalInfo.StartTrialLicenseDate.Year.ToString() +
                                UserGlobalInfo.StartTrialLicenseDate.Month.ToString().PadLeft(2, '0') +
                                UserGlobalInfo.StartTrialLicenseDate.Day.ToString().PadLeft(2, '0') +
                                UserGlobalInfo.RegistrationName + UserGlobalInfo.LicenseUsersNo.ToString().PadLeft(3, '0');
                string actTest = CRSLicense.EncodeRegCode30(appName, appVers, regCode, UserGlobalInfo.LicenseUsersNo);

                if (actTest != UserGlobalInfo.ActivationCode)
                    res = false;

                TimeSpan t = DateTime.Now.Subtract(UserGlobalInfo.StartTrialLicenseDate);
                if ((UserGlobalInfo.RegistrationName == CRSGlobalParams.TRIAL_ACTIVATION_REGISTRY_NAME) && (t.Days > CRSGlobalParams.TRIAL_DURATION_DAYS))
                    res = false;

                //-- controlla il numero di utenti nel db
                if (res == true)
                {
                    int numusers = connSqlApp.GetRecordCount("select count(*) from userapp where id<>0");
                    if (numusers > UserGlobalInfo.LicenseUsersNo)
                        res = false;
                }
            }
            catch
            {
                res = false;
            }

            return res;
        }

        private string RetrieveLicenceFromDB(string appName, string appVers)
        {
            DbFactory connSqlApp = new DbFactory(this.connString, this.provider);
            CRSParameter p = new CRSParameter(connSqlApp);
            DataRow r = p.GetParam(CRSGlobalParams.PARAMSGROUP_APPLICATION_DATA, CRSGlobalParams.PARAMS_KEY_ACTIVATION);

            string regName = "";
            string actCode = "";
            int numUsers = 1;
            if (r != null)
            {
                regName = Convert.ToString(r["description"]);
                actCode = Convert.ToString(r["val1"]);
                if (string.IsNullOrEmpty(Convert.ToString(r["val2"]))==false) numUsers = Convert.ToInt32(r["val2"]);
            }
            if (string.IsNullOrEmpty(actCode) == false)
            {
                string regCode = CRSLicense.DecodeCode(appName, appVers, actCode,numUsers);
                UserGlobalInfo.ActivationCode = actCode;
                UserGlobalInfo.RegistrationName = regName;
                UserGlobalInfo.LicenseUsersNo = Convert.ToInt32(regCode.Substring(regCode.Length - 3, 3).TrimStart('0'));
                UserGlobalInfo.StartTrialLicenseDate = new DateTime(Convert.ToInt32(regCode.Substring(0, 4)), Convert.ToInt32(regCode.Substring(4, 2)), Convert.ToInt32(regCode.Substring(6, 2)));
            }
            return actCode;
        }

        private string GenerateTrialCode(string appName, string appVers)
        {
            DbFactory connSqlApp = new DbFactory(this.connString, this.provider);
            DataRow r = connSqlApp.GetSingleRow("select DateRegistration from userapp where name='root' or id=0");
            DateTime rootDate = DateTime.Now;
            if ((r != null) && (r[0] != null)) rootDate = Convert.ToDateTime(r[0]);

            string regCode = rootDate.Year.ToString() + rootDate.Month.ToString().PadLeft(2, '0') + rootDate.Day.ToString().PadLeft(2, '0') +
                            CRSGlobalParams.TRIAL_ACTIVATION_REGISTRY_NAME + CRSGlobalParams.TRIAL_USERS_NO;
            int numUsers = Convert.ToInt32(CRSGlobalParams.TRIAL_USERS_NO.TrimStart('0'));
            string actCode = CRSLicense.EncodeRegCode30(appName, appVers, regCode,numUsers);

            if (string.IsNullOrEmpty(actCode) == false)
            {
                UserGlobalInfo.ActivationCode = actCode;
                UserGlobalInfo.RegistrationName = CRSGlobalParams.TRIAL_ACTIVATION_REGISTRY_NAME;
                UserGlobalInfo.LicenseUsersNo = Convert.ToInt32(CRSGlobalParams.TRIAL_USERS_NO.TrimStart('0'));
                UserGlobalInfo.StartTrialLicenseDate = rootDate;//DateTime.Now;
            }

            return actCode;
        }

        private string GenerateFreeCode(string appName, string appVers)
        {
            DbFactory connSqlApp = new DbFactory(this.connString, this.provider);
            DataRow r = connSqlApp.GetSingleRow("select DateRegistration from userapp where name='root' or id=0");
            DateTime rootDate = DateTime.Now;
            if ((r != null) && (r[0] != null)) rootDate = Convert.ToDateTime(r[0]);

            string regCode = rootDate.Year.ToString() + rootDate.Month.ToString().PadLeft(2, '0') + rootDate.Day.ToString().PadLeft(2, '0') +
                            CRSGlobalParams.FREE_ACTIVATION_REGISTRY_NAME + CRSGlobalParams.FREE_USERS_NO;
            int numUsers = Convert.ToInt32(CRSGlobalParams.FREE_USERS_NO.TrimStart('0'));
            string actCode = CRSLicense.EncodeRegCode30(appName, appVers, regCode, numUsers);

            if (string.IsNullOrEmpty(actCode) == false)
            {
                UserGlobalInfo.ActivationCode = actCode;
                UserGlobalInfo.RegistrationName = CRSGlobalParams.FREE_ACTIVATION_REGISTRY_NAME;
                UserGlobalInfo.LicenseUsersNo = Convert.ToInt32(CRSGlobalParams.FREE_USERS_NO.TrimStart('0'));
                UserGlobalInfo.StartTrialLicenseDate = rootDate;
            }

            return actCode;
        }

        private void SetActivationInIniFile(string actCode, string regName, int numUsers)
        {
            //-- questo metodo viene chiamato solo se sia nel db che nel file ini non ci sono dati di attivazione
            //-- salva il codice su file ini
            CRSIniFile.SetPropertyValue(APPConstants.SEC_APPL, APPConstants.PROP_ACTIVATION_CODE, actCode/*UserGlobalInfo.ActivationCode*/);
            CRSIniFile.SetPropertyValue(APPConstants.SEC_APPL, APPConstants.PROP_REGISTRATION_NAME, regName/*UserGlobalInfo.RegistrationName*/);
            CRSIniFile.SetPropertyValue(APPConstants.SEC_APPL, APPConstants.PROP_USERS_NO, numUsers/*UserGlobalInfo.LicenseUsersNo*/);
        }

        #endregion

        #region trash or not used
        /*
        public WindowLogin(string conn, string connProvider, EventCloseMain eventClose)
        {
            InitializeComponent();
            //connSql = new DbFactory(conn, connProvider);
            eventCloseMain = eventClose;
            ctrlLogin.SetParameters(conn, connProvider, EventExit);
            ctrlLogin.InsertDefaultData();
        }*/

        #endregion
    }
}
