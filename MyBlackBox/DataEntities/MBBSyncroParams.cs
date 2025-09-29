using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRS.Library;

namespace MyBlackBoxCore.DataEntities
{
    public class MBBSyncroParams
    {
        public string DropBoxRootDir { set; get; }
        public string SharingDir { set; get; }
        //public string RemoteSharedDir { set; get; }
        public string LocalTempDir { set; get; }
        public bool SyncroOnlyOwner { set; get; }       
        public bool IsUniqueFileName { set; get; }
        // quando seleziono un file da inserire in myBlackBox lo sposto, non solo lo copio
        public bool MoveFilesInLocalDir { set; get; }
        public bool AutoSync { set; get; }
        public bool AutoSave { set; get; }
        public bool SyncFiles { set; get; }
        public string NewFileExtension { set; get; }
        public int  SyncFileDimDays { set; get; }

        public void InitParamsFromIni()
        {
            //MBBSyncroParams syncParams = new MBBSyncroParams();
            try
            {
                //-- init info               
                this.SharingDir = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_SHARINGDIR);
                this.LocalTempDir = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_LOCALTMPDIR);
                this.DropBoxRootDir = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_DROPBOXDIR);

                this.AutoSync = false;
                string s = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_AUTOSYNC);
                if (string.IsNullOrEmpty(s) == false && (s == "1" || s.ToLower() == "true"))
                    this.AutoSync = true;

                this.AutoSave = true;
                s = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_AUTOSAVE);
                if (string.IsNullOrEmpty(s) == false && (s == "1" || s.ToLower() == "true"))
                    this.AutoSave = true;

                this.SyncFiles = false;
                string ss = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_SYNCROFILES);
                if (string.IsNullOrEmpty(ss) == false && (ss == "1" || ss.ToLower() == "true"))
                    this.SyncFiles = true;

                s = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_ISUNIQUEFILENAME, 1);
                if (string.IsNullOrEmpty(s) == false && (s == "1" || s.ToLower() == "true"))
                    this.IsUniqueFileName = true;

                s = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_OPNOOVERWRITEOWNER, 1);
                if (string.IsNullOrEmpty(s) == false && (s == "1" || s.ToLower() == "true"))
                    this.SyncroOnlyOwner = true;

                s = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_FILE_DIM_DAYS, 180);
                if (string.IsNullOrEmpty(s) == false)
                    this.SyncFileDimDays = Convert.ToInt32(s);

                this.NewFileExtension = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_NEWFILEEXTENSION);

            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("Error GetSyncroParamsFromIni: " + ex.Message, "");
            }

            //return syncParams;
        }

    }

    public class MBBEmailParams
    {
        public string EmailAddress { set; get; }
        public string EmailPwd { set; get; }
        public string ImapServer { set; get; }
        public int ImapPort { set; get; }
        public string SmtpServer { set; get; }
        public int SmtpPort { set; get; }
        public string Pop3Server { set; get; }
        public int Pop3Port { set; get; }

        public void InitParamsFromIni()
        {
            try
            {
                //-- init info               
                this.EmailAddress = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_USER);
                this.EmailPwd = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_USERPWD);
                this.ImapServer = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_EMAIL_SERVER_IMAP);
                this.ImapPort = Convert.ToInt32( CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_EMAIL_SERVER_IMAP_PORT));
                this.SmtpServer = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_EMAIL_SERVER_SMTP);
                this.SmtpPort = Convert.ToInt32(CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_EMAIL_SERVER_SMTP_PORT));
                this.Pop3Server = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_EMAIL_SERVER_POP3);
                this.Pop3Port = Convert.ToInt32(CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_EMAIL_SERVER_POP3_PORT));

            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("Error GetEmailParamsFromIni: " + ex.Message, "");
            }

            //return emailParams;
        }
    }
}
