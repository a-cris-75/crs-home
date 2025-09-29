using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Net;
using System.IO;
using System.IO.Compression;
using PoseidonData;
using PoseidonData.DataEntities;
using CRS.CommonControlsLib;

namespace PoseidonApp.UserControls
{
    public partial class PSDOptionsAppMain : UserControl
    {
        public PSDOptionsAppMain()
        {
            InitializeComponent();
            
        }

        //public string VERSION;
        //public string FILE_IMPORT;
        
        public void InitFromGlobal()
        {
            //APPGlobalInfo.InitIniInfo();
            lblEstrUpdTo.Text = "";
            lblFileImport.Text = APPGlobalInfo.IMPORT_FILE_PATH;
            lblUser.Text = APPGlobalInfo.USER;
            lblEstrUpdTo.Text = DbDataAccess.GetLastDataEstrImported().ToLongDateString();

            try
            {
                lblVersion.Text = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                PSD_ESTR_LOTTO estr = DbDataAccess.GetLastEstrImported();
            }
            catch { }
        }

        private void btnViewOptions_Click(object sender, EventArgs e)
        {
            Forms.FPSDOptionsApp f = new Forms.FPSDOptionsApp();
            f.Show();
        }

        private void btnDownbload_Click(object sender, EventArgs e)
        {
            string fileNameUrl = APPGlobalInfo.IMPORT_FILE_URL;

            string fileNameTxt = APPGlobalInfo.IMPORT_FILE_PATH;
            string dirName = "";
            int idx = APPGlobalInfo.IMPORT_FILE_PATH.LastIndexOf(@"\");
            if (idx >= 0)
            {
                fileNameTxt = APPGlobalInfo.IMPORT_FILE_PATH.Substring(idx + 1);
                dirName = APPGlobalInfo.IMPORT_FILE_PATH.Substring(0,idx + 1);
            }

            if (fileNameUrl.Contains(".zip"))
            {
                fileNameUrl = fileNameTxt.Substring(0, fileNameTxt.IndexOf('.')) + ".zip";
            }

            Cursor.Current = Cursors.WaitCursor;

            

            try
            {

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                WebClient myWebClient = new WebClient();
                //var webClient = new WebClient();
                var s = myWebClient.DownloadString(APPGlobalInfo.IMPORT_FILE_URL);

                // Download the Web resource and save it into the current filesystem folder.
                myWebClient.DownloadFile(APPGlobalInfo.IMPORT_FILE_URL, dirName + fileNameUrl);

                ABSMessageBox.Show("Successfully Downloaded File " + fileNameUrl + " from " + APPGlobalInfo.IMPORT_FILE_URL + 
                                    "\nDownloaded file saved in the following file system folder:\n " +
                                    dirName
                                    , "DOWNLOAD");

                if (fileNameUrl.Contains(".zip"))
                {
                    FileInfo fi = new FileInfo(dirName + fileNameUrl);
                    bool isOk = CRS.Library.CRSFileUtil.Decompress(fi);

                    if (!isOk)
                    {
                        openFileDialog1.FileName = dirName + fileNameUrl;
                        DialogResult r = openFileDialog1.ShowDialog();
                    }
                }
            }
            catch(Exception ex) {
                ABSMessageBox.Show("Errore in download: " + ex.Message, "ERROR");
            }

            Cursor.Current = Cursors.Default;
        }
    }
}
