using Crs.Base.CommonUtilsLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Crs.Home.ApocalypsApp
{
    public partial class FormSettings : Form
    {
        public FormSettings()
        {
            InitializeComponent();
        }

        private void brnSave_Click(object sender, EventArgs e)
        {
            ConfigManager ConfiguratorManagerDB = new ConfigManager("", "DatabaseSettings");
            ConfigManager ConfiguratorManagerApp = new ConfigManager("", "AppSettings");

            ConfiguratorManagerApp.SetValue("FILE_PATH_ESTRAZIONI", txtPathStorico.Text);
            ConfiguratorManagerApp.SetValue("FORMAT_DATE_FILE", txtFormatDate.Text);
            ConfiguratorManagerApp.SetValue("SEQ_FIELDS_FILE", txtSeqFields.Text);

            string typef = radioFormatoSingolaRiga.Checked ? "2" : "1";
            ConfiguratorManagerApp.SetValue("TYPE_FIELDS_FILE", typef);
            ConfiguratorManagerApp.SetValue("SAVE_TO_DB", chkSaveToDb.Checked.ToString());

            string locpath = txtLocalPathDB.Text.Trim();
            ConfiguratorManagerDB.SetValue("DB_LOCAL_PATH", locpath);

            string? connstr = ConfiguratorManagerDB.GetValue("ConnectionStringLOCALDB");

            if (!string.IsNullOrEmpty(connstr) && connstr.Contains("AttachDbFilename="))
            {
                int idx = connstr.IndexOf("AttachDbFilename=");
                int idx2 = connstr.IndexOf(";", idx);
                if (idx2 > idx)
                {
                    connstr = connstr.Substring(0, idx + 17) + locpath + connstr.Substring(idx2);
                    ConfiguratorManagerDB.SetValue("ConnectionStringLOCALDB", connstr);

                    //string path_db = connstr.Substring(idx + 17, idx2 - (idx + 17));
                    //if (!string.IsNullOrEmpty(path_db))
                    //{
                    //    path_db = path_db.Trim();
                    //    ConfiguratorManagerDB.SetValue("ConnectionStringLOCALDB", path_db);
                    //}
                }
            }

            // AttachDbFilename=D:\\CRS\\DEV\\REPOSITORY_GIT\\crs-home\\Crs.Home.Apocalyps\\Crs.Home.ApocalypsDB\\DB_LOTTO_APOCALYPS.mdf
        }


        private void btnOpenPath_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "File di testo (*.txt)|*.txt|Tutti i file (*.*)|*.*";
                openFileDialog.Title = "Seleziona file estrazioni";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtPathStorico.Text = openFileDialog.FileName;
                }
            }
        }

        private void btnOpedDialogPathDB_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "File di testo (*.mdf)|*.mdf|Tutti i file (*.*)|*.*";
                openFileDialog.Title = "Seleziona file db";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtLocalPathDB.Text = openFileDialog.FileName;

                }
            }
        }

        private void radioFormatoSingolaRiga_CheckedChanged(object sender, EventArgs e)
        {
            if (radioFormatoSingolaRiga.Checked)
            {
                txtSeqFields.Text = "Data;Numero;Numeri";
                txtFormatDate.Text = "dd/MM/yyyy";
            }
            else
            {
                txtSeqFields.Text = "Data;Ruota;Numeri";
                txtFormatDate.Text = "yyyy/MM/dd";
            }
        }
    }
}
