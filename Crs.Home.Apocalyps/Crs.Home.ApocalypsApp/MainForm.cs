using Crs.Base.CommonUtilsLibrary;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Crs.Home.ApocalypsApp
{
    public partial class MainForm : Form
    {
        private PageTabellone pageTabellone;
        private PageAnalisi pageAnalisi;

        public MainForm()
        {
            InitializeComponent();
            InitLocal();
            InitializeUserControls();
            ShowPageTabellone();
        }

        private void InitializeUserControls()
        {
            pageTabellone = new PageTabellone();
            pageTabellone.Dock = DockStyle.Fill;

            pageAnalisi = new PageAnalisi();
            pageAnalisi.Dock = DockStyle.Fill;
        }

        private void ShowPageTabellone()
        {
            panelContainer.Controls.Clear();
            panelContainer.Controls.Add(pageTabellone);
            UpdateButtonStyles(true);
        }

        private void ShowPageAnalisi()
        {
            panelContainer.Controls.Clear();
            panelContainer.Controls.Add(pageAnalisi);
            pageAnalisi.Init();
            UpdateButtonStyles(false);
        }

        private void UpdateButtonStyles(bool isTabelloneActive)
        {
            btnTabellone.BackColor = isTabelloneActive ? Color.LightSteelBlue : Color.WhiteSmoke;
            btnTabellone.ForeColor = isTabelloneActive ? Color.White : Color.Black;

            btnAnalisi.BackColor = !isTabelloneActive ? Color.LightSteelBlue : Color.WhiteSmoke;
            btnAnalisi.ForeColor = !isTabelloneActive ? Color.White : Color.Black;
        }

        public bool InitLocal()
        {
            ConfigManager AppConfigManagerDB = new ConfigManager("", "DatabaseSettings");
            string? path_db = AppConfigManagerDB.GetValue("DB_LOCAL_PATH");
            try
            {
                List<string> foldersDll = new List<string>();
                List<string> foldersDB = new List<string>();

                string dd2 = "C:";

                if (!string.IsNullOrEmpty(path_db))
                {
                    path_db = path_db.Trim();
                    dd2 = path_db.Substring(0, 2).ToUpper();
                    foldersDB.AddRange(path_db.Split('\\').Where(X => !string.IsNullOrEmpty(X) && !X.Contains(":") && !X.ToUpper().Contains(".MDF")).ToList());
                }

                foreach (string d in foldersDB)
                {
                    dd2 += "\\" + d;
                    if (!Directory.Exists(dd2))
                        Directory.CreateDirectory(dd2);
                }
            }
            catch { }

            bool createnew2 = InitLocalDB(path_db);

            return createnew2;
        }

        /// <summary>
        /// Copio il db dalla cartella locale alla cartella in app.config (C:\ALP\DB_MDF)
        /// </summary>
        /// <returns></returns>
        private bool InitLocalDB(string? path_db)
        {
            bool createnew = false;
            try
            {
                if (!string.IsNullOrEmpty(path_db))
                {
                    if (!File.Exists(path_db))
                    {
                        string dir = Path.Combine(Directory.GetCurrentDirectory(), "DB");
                        string locdb = Path.Combine(dir, "DB_LOTTO_APOCALYPS.mdf");
                        if (File.Exists(locdb))
                        {
                            File.Copy(locdb, path_db);
                            createnew = true;
                        }
                    }
                }
            }
            catch { }
            return createnew;
        }


        private void BtnTabellone_Click(object sender, EventArgs e)
        {
            ShowPageTabellone();
        }

        private void BtnAnalisi_Click(object sender, EventArgs e)
        {
            ShowPageAnalisi();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            Form f = new FormSettings();
            DialogResult r = f.ShowDialog();
        }
    }
}

