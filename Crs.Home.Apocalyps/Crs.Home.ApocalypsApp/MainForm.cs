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
            UpdateButtonStyles(false);
        }

        private void UpdateButtonStyles(bool isTabelloneActive)
        {
            btnTabellone.BackColor = isTabelloneActive ? Color.SteelBlue : Color.LightGray;
            btnTabellone.ForeColor = isTabelloneActive ? Color.White : Color.Black;

            btnAnalisi.BackColor = !isTabelloneActive ? Color.SteelBlue : Color.LightGray;
            btnAnalisi.ForeColor = !isTabelloneActive ? Color.White : Color.Black;
        }

        private void BtnTabellone_Click(object sender, EventArgs e)
        {
            ShowPageTabellone();
        }

        private void BtnAnalisi_Click(object sender, EventArgs e)
        {
            ShowPageAnalisi();
        }
    }
}

