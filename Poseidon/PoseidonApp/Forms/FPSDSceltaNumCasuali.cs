using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PoseidonApp.Forms
{
    public partial class FPSDSceltaNumCasuali : Form
    {
        public FPSDSceltaNumCasuali()
        {
            InitializeComponent();
            psdTavolaNumCasuali1.IsSetMode = false;
        }

        public Dictionary<string, List<int>> NUMERI_CASUALI
        {
            set { psdTavolaNumCasuali1.NUMERI_CASUALI = value; }
            get { return psdTavolaNumCasuali1.NUMERI_CASUALI; }
        }


        private void btnOk_Click(object sender, EventArgs e)
        {
            APPGlobalInfo.NUMERI_CASUALI = psdTavolaNumCasuali1.NUMERI_CASUALI;
            APPGlobalInfo.TYPE_CALC_R10_NUMSCELTI1 = 1;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
