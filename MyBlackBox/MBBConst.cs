using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MyBlackBoxCore
{
    public static class MBBConst
    {
        public const int MODE_INS = 0;
        public const int MODE_UPD = 1;
        public const int MODE_FIND = 2;
        public const int MODE_SYNCRO_NOTES = 3;
        public const int MODE_CLOSE = 4;
        public const int MODE_SYNCRO_DOCS = 5;
        //NON USATO
        public const int SYNCRO_INS = 0;
        public const int SYNCRO_UPD = 1;
        public const int SYNCRO_DEL = 2;
        public const int SYNCRO_USERS = 3;
        public const int SYNCRO_DOCS = 4;
        public const int SYNCRO_ACTIVITIES = 5;
        public const int SYNCRO_ACTIVITIES_LOG = 6;
        public const int SYNCRO_ACTIVITIES_NOTES = 7;

        public static string FILE_NAME_INS = "syncroDbINS.txt";
        public static string FILE_NAME_UPD = "syncroDbUPD.txt";
        public static string FILE_NAME_DEL = "syncroDbDEL.txt";
        public static string FILE_NAME_DOCS = "syncroDbDOCS.txt";
        public static string FILE_NAME_ACT = "syncroDbACTIVITIES.txt";
        public static string FILE_NAME_ACTLOG = "syncroDbACTIVITIESLOG.txt";
        public static string FILE_NAME_USERS = "syncroDbUSERS.txt";
        // PER ORA NON USATA
        public static string FILE_NAME_ACTNOTES = "syncroDbACTIVITIESNOTES.txt";
        public static string FILE_NAME_ALTERDB = "alterDB.txt";

        private static string dirname = Path.Combine(Path.GetTempPath(), "\\MBB");
        public static string FILE_PATH_SYNCRO_UPD_EMAIL = dirname + "\\syncroDbUPD.txt";
        public static string FILE_PATH_SYNCRO_UPD_EMAIL_DOWNLOAD = Path.Combine(dirname, "\\syncroDbUPD_DOWNLOAD.txt");//"C:\\Temp\\MBB\\syncroDbUPD_DOWNLOAD.txt";
        public static string FILE_PATH_SYNCRO_DEL_EMAIL = Path.Combine(dirname, "\\syncroDbDEL.txt");
        public static string FILE_PATH_SYNCRO_DOC_EMAIL = Path.Combine(dirname, "\\syncroDbDOCS.txt");
        public static string FILE_PATH_SYNCRO_DEL_EMAIL_DOWNLOAD = Path.Combine(dirname, "\\syncroDbDEL_DOWNLOAD.txt");
        public static string DIR_PATH_SYNCRO_EMAIL_DOWNLOAD = Path.Combine(dirname, "\\MBB\\");
        //public const int SYNCRO_SHARED = 5;

        
    }
}
