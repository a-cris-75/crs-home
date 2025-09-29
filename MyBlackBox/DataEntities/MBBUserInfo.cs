using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRS.Library;

namespace MyBlackBoxCore.DataEntities
{
    public class MBBUserInfo
    {
        public int IDUser { set; get; }
        public string Name { set; get; } // email
        public string Password { set; get; }
        public string Language { set; get; }
        public string RegistryName { set; get; }
        public string RegistrySurname { set; get; }
        public string WorkGroup { set; get; }
        public string PasswordEmail { set; get; }

        public MBBUserInfo()
        {
            InitUserInfoFromIni();
            this.IDUser = DbDataAccess.GetIDUser(this.Name);
        }

        public void InitUserInfoFromIni()
        {
            try
            {
                //-- init info               
                this.Name = CRSIniFile.GetUserFromIni();//CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_USER);
                this.Password = CRSIniFile.GetUserPwdFromIni();//CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_USERPWD);
                this.Language = CRSIniFile.GetLanguageFromIni();       
                this.RegistryName = CRSIniFile.GetPropertyValue(APPConstants.SEC_APPL,APPConstants.PROP_USERS_REG_NAME);
                this.RegistrySurname = CRSIniFile.GetPropertyValue(APPConstants.SEC_APPL, APPConstants.PROP_USERS_REG_SURNAME);
                this.PasswordEmail = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_EMAIL_PWD);
                if (string.IsNullOrEmpty(this.PasswordEmail))
                    this.PasswordEmail = this.Password;
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("Error GetUserInfoFromIni: " + ex.Message, "");
            }
        }
    }
}
