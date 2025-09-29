using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CRS.DBLibrary;

namespace PoseidonApp.Forms
{
    public partial class FPSDOptionsApp : Form
    {
        public FPSDOptionsApp()
        {
            InitializeComponent();
            psdOptionsApp1.GetIniInfo();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            psdOptionsApp1.SaveIniInfo();
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
