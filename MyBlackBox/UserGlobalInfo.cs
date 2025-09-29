using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyBlackBoxCore
{
    public static class UserGlobalInfo
    {
        //-- User info
        private static string user;
        private static int idUser;
        private static string language;
        private static int idProfile;
        //-- DB info: in base a parametro offline
        private static int dbTypeAppl;
        private static int dbTypeConfig;
        private static bool isOffline;
        //-- data info
        private static int currentJobId;
        private static int currentPhaseNo;
        private static int paramPageJobId;
        private static int paramPagePhaseNo;
        private static int paramPageNoteId;
        //-- intervallo in secondi per rinfrescare i dati
        private static int refreshTimer; 
        //-- dati di licenza
        private static string activationCode;
        private static string registrationName;
        private static int usersNo;
        private static DateTime startTrialDate;
        //-- log level
        private static int logLevel;
        //-- opzione gestione costi
        private static bool manageCosts;

        public static bool IsUserChanged
        {
            set;
            get;
        }

        public static int CurrentJobId
        {
            set { currentJobId = value; }
            get { return currentJobId; }
        }

        public static int CurrentPhaseNo
        {
            set { currentPhaseNo = value; }
            get { return currentPhaseNo; }
        }

        public static int ParamPageJobId
        {
            set { paramPageJobId = value; }
            get { return paramPageJobId; }
        }

        public static int ParamPagePhaseNo
        {
            set { paramPagePhaseNo= value; }
            get { return paramPagePhaseNo; }
        }

        public static int ParamPageNoteId
        {
            set { paramPageNoteId = value; }
            get { return paramPageNoteId; }
        }

        public static string User
        {
            set { user = value; }
            get { return user; }
        }

        public static int IDUser
        {
            set { idUser = value; }
            get { return idUser; }
        }

        public static int IDProfile
        {
            set { idProfile = value; }
            get { return idProfile; }
        }

        public static string Language
        {
            set { language = value; }
            get { return language; }
        }

        public static int RefreshTimer
        {
            set { refreshTimer = value; }
            get { return refreshTimer; }
        }

        public static int DBTypeApplication
        {
            set { dbTypeAppl= value; }
            get { return dbTypeAppl; }
        }

        public static int DBTypeConfigurator
        {
            set { dbTypeConfig = value; }
            get { return dbTypeConfig; }
        }

        public static bool IsOffline
        {
            set { isOffline = value; }
            get { return isOffline; }
        }

        public static string RegistrationName
        {
            set { registrationName = value; }
            get { return registrationName; }
        }

        public static string ActivationCode
        {
            set { activationCode = value; }
            get { return activationCode; }
        }

        public static int LicenseUsersNo
        {
            set { usersNo = value; }
            get { return usersNo; }
        }

        public static DateTime StartTrialLicenseDate
        {
            set { startTrialDate = value; }
            get { return startTrialDate; }
        }

        public static int LogLevel
        {
            set { logLevel = value; }
            get { return logLevel; }
        }

        public static bool ManageCosts
        {
            set { manageCosts = value; }
            get { return manageCosts; }
        }
    }
}
