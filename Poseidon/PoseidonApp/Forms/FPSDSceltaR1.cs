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
    public partial class FPSDSceltaR1 : Form
    {
        public FPSDSceltaR1()
        {
            InitializeComponent();
        }

        public void Init(PoseidonData.DataEntities.PSD_PARAMETRI_R1 p)
        {
            //psdSceltaR11.PARAMETRI_R1 = new PoseidonData.DataEntities.PSD_PARAMETRI_R1();
            psdSceltaR11.COLPI = new List<int>();
            psdSceltaR11.TIPO_FREQUENZA = new List<int>();
            psdSceltaR11.POS_DEC_R12 = new List<string>();
            psdSceltaR11.ACCOPPIATE_DEC_R12 = new List<string>();
            psdSceltaR11.COLPI = p.COLPI;
            psdSceltaR11.TIPO_FREQUENZA = p.TIPO_FREQUENZA;
            psdSceltaR11.POS_DEC_R12 = p.POS_DEC_R12;
            psdSceltaR11.ACCOPPIATE_DEC_R12 = p.ACCOPPIATE_DEC;
        }

        /*public PoseidonData.DataEntities.PSD_PARAMETRI_R1 PARAMETRI_R1
        {
            set {
                psdSceltaR11.PARAMETRI_R1 = new PoseidonData.DataEntities.PSD_PARAMETRI_R1();
                psdSceltaR11.PARAMETRI_R1 = value;
            }
            get {
                return psdSceltaR11.PARAMETRI_R1;
            }
        }*/


        private void btnOk_Click(object sender, EventArgs e)
        {
            APPGlobalInfo.PARAMETRI_R1 = new PoseidonData.DataEntities.PSD_PARAMETRI_R1();
            APPGlobalInfo.PARAMETRI_R1.COLPI = new List<int>();
            APPGlobalInfo.PARAMETRI_R1.TIPO_FREQUENZA  = new List<int>();
            APPGlobalInfo.PARAMETRI_R1.POS_DEC_R12 = new List<string>();
            APPGlobalInfo.PARAMETRI_R1.ACCOPPIATE_DEC = new List<string>();

            APPGlobalInfo.PARAMETRI_R1.COLPI = psdSceltaR11.COLPI;
            APPGlobalInfo.PARAMETRI_R1.TIPO_FREQUENZA = psdSceltaR11.TIPO_FREQUENZA;
            APPGlobalInfo.PARAMETRI_R1.POS_DEC_R12 = psdSceltaR11.POS_DEC_R12;
            APPGlobalInfo.PARAMETRI_R1.ACCOPPIATE_DEC = psdSceltaR11.ACCOPPIATE_DEC_R12;
            APPGlobalInfo.TYPE_CALC_R10_NUMSCELTI1 = 0;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
