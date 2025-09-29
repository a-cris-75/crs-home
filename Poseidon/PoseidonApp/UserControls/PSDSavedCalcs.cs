using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PoseidonData.DataEntities;
using PoseidonData;
using Infragistics.Win.UltraWinGrid;

namespace PoseidonApp.UserControls
{
    public partial class PSDSavedCalcs : UserControl
    {
        public PSDSavedCalcs()
        {
            InitializeComponent();
        }

        public List<PSD_CALCS> LST_CALCOLI = APPGlobalInfo.LST_CALCOLI.ToList();//new List<PSD_CALCS>();

        private void UgResIntervalli_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            //
        }

        private void BtnRicarica_Click(object sender, EventArgs e)
        {
            this.LST_CALCOLI = APPGlobalInfo.LST_CALCOLI.ToList();
            bsCalcs.DataSource = this.LST_CALCOLI;
        }

        private void BtnCalcSaturazione_Click(object sender, EventArgs e)
        {
            List<PSD_ESTR_SEGNALI> lstsegn = new List<PSD_ESTR_SEGNALI>(); 
            foreach(PSD_CALCS c in this.LST_CALCOLI)
            {
                lstsegn.AddRange(c.LST_SEGNALI.ToList());
            }

            bsSegn.DataSource = lstsegn;
        }

        private void BtnAddCalcToDB_Click(object sender, EventArgs e)
        {
            foreach (PSD_CALCS c in this.LST_CALCOLI)
            {
                bool exists;
                DbDataUpdate.InsertCalcInDB(c, out exists);
                if(DbDataAccess.GetSegnali(c.IDCalcolo).Count==0)
                {
                    foreach (PSD_ESTR_SEGNALI s in c.LST_SEGNALI)
                        DbDataUpdate.InsertSegnale(s);
                }
            }
        }

        private void BtnFillCalcFromDB_Click(object sender, EventArgs e)
        {
            this.LST_CALCOLI = DbDataAccess.GetCalcs();
            bsCalcs.DataSource = this.LST_CALCOLI;
        }

        private void BtnDelCalc_Click(object sender, EventArgs e)
        {
            foreach (UltraGridRow r in ugCalcs.Selected.Rows)
            {

                int idxM1 = r.ListIndex;
                //int idxM2 = ugResIntervalli.Selected.Rows[idx + 1].ListIndex;
                PSD_CALCS r1 = ((PSD_CALCS)bsCalcs[idxM1]);
                DbDataUpdate.DeleteCalcInDB(r1);
            }
        }

        private void ChkFilterBySegn_Click(object sender, EventArgs e)
        {
            if (bsCalcs.Current != null)
            {
                List<PSD_ESTR_SEGNALI> lstsegn = new List<PSD_ESTR_SEGNALI>();
                if (chkFilterBySegn.Checked)
                {
                    PSD_CALCS c = (PSD_CALCS)bsCalcs.Current;
                    lstsegn = LST_CALCOLI.Where(X => X.IDCalcolo == c.IDCalcolo).FirstOrDefault().LST_SEGNALI;
                }
                else
                {
                    foreach (PSD_CALCS c in this.LST_CALCOLI)
                    {
                        lstsegn.AddRange(c.LST_SEGNALI.ToList());
                    }
                }
                bsSegn.DataSource = lstsegn;
            }
        }
    }
}
