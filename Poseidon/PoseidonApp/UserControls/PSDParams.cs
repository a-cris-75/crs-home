using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CRS.Library;
using CRS.CommonControlsLib;
using PoseidonData.DataEntities;
using PoseidonData;

namespace PoseidonApp
{
    public partial class PSDParams : UserControl
    {
        public PSDParams()
        {
            InitializeComponent();
        }

        public void InitFromGlobal()
        {
            this.DATA_INIZIO = APPGlobalInfo.DATA_INIZIO;
            this.DATA_FINE = APPGlobalInfo.DATA_FINE;
            txtNumEstr.Text = Convert.ToString(DbDataAccess.GetNumEstrazioni(dtpBegin.Value, dtpEnd.Value));
        }

        public DateTime DATA_INIZIO
        {
            set {
                dtpBegin.Value = value;               
            }
            get { return dtpBegin.Value; } 
        }
        public DateTime DATA_FINE
        {
            set {
                dtpEnd.Value = value;
            }
            get { return dtpEnd.Value; }
        }

        public bool SHOW_GIOCA_CON
        {
            set { gbPlay.Visible = value; }
            get { return gbPlay.Visible; }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                string filename = APPGlobalInfo.IMPORT_FILE_PATH;
                string typefile = APPGlobalInfo.IMPORT_FILE_TYPE;

                int tipo = 0;
                if (typefile.Length > 0 && Utils.IsNumber(typefile.Substring(0, 1)))
                    tipo = Convert.ToInt32(typefile.Substring(0, 1));
                List<PSD_ESTR_ESTRAZIONI> estr = PSDEstrazioni.ReadFileEstr(filename, Convert.ToInt32(tipo));

                if (estr.Count > 0)
                {
                    PSDEstrazioni.ImportDB(estr);

                    if (chkAutoUpdR1.Checked)
                        PSDEstrazioni.ImportR1(estr);
                }
                else ABSMessageBox.Show("Non ci sono estrazioni da importare!" , "ESTRAZIONI");
            }
            catch (Exception ex)
            {
                ABSMessageBox.Show("ERROR READING INI FILE: \n" + ex.Message);
            }
        }

        private void btnScegliNumCasuali_Click(object sender, EventArgs e)
        {
            Forms.FPSDSceltaNumCasuali f = new Forms.FPSDSceltaNumCasuali();
            f.NUMERI_CASUALI = APPGlobalInfo.NUMERI_CASUALI;
            f.Show();

            btnPlayNumCasuali.BackColor = Color.Lime;
            btnPlayR1.BackColor = SystemColors.Control;
            APPGlobalInfo.TYPE_CALC_R10_NUMSCELTI1 = 1;
        }

        private void btnPlayR1_Click(object sender, EventArgs e)
        {
            Forms.FPSDSceltaR1 f = new Forms.FPSDSceltaR1();
            f.Init(APPGlobalInfo.PARAMETRI_R1);
            //f.PARAMETRI_R1 = APPGlobalInfo.PARAMETRI_R1;
            f.Show();

            btnPlayNumCasuali.BackColor = SystemColors.Control; 
            btnPlayR1.BackColor = Color.Lime;
            APPGlobalInfo.TYPE_CALC_R10_NUMSCELTI1 = 0;
        }

        private void btnSetIntervalDate_Click(object sender, EventArgs e)
        {
            SetIntervalDate();
        }

        private void SetIntervalDate()
        {
            APPGlobalInfo.DATA_INIZIO = this.DATA_INIZIO;
            APPGlobalInfo.DATA_FINE = this.DATA_FINE;
            txtNumEstr.Text = Convert.ToString(DbDataAccess.GetNumEstrazioni(dtpBegin.Value, dtpEnd.Value));
            CRSIniFile.SetPropertyValue(APPConstants.SEC_APP_PARAMS, APPConstants.PROP_DATAINIZIO, this.DATA_INIZIO.ToShortDateString());
            CRSIniFile.SetPropertyValue(APPConstants.SEC_APP_PARAMS, APPConstants.PROP_DATAFINE, this.DATA_FINE.ToShortDateString());
        }

        private void btnUpdR1_Click(object sender, EventArgs e)
        {
            try
            {
                string filename = APPGlobalInfo.IMPORT_FILE_PATH;
                string typefile = APPGlobalInfo.IMPORT_FILE_TYPE;

                int tipo = 0;
                if (typefile.Length > 0 && Utils.IsNumber(typefile.Substring(0, 1)))
                    tipo = Convert.ToInt32(typefile.Substring(0, 1));
                List<PSD_ESTR_ESTRAZIONI> estr = PSDEstrazioni.ReadFileEstr(filename, Convert.ToInt32(tipo));
                PSDEstrazioni.ImportR1(estr);
            }
            catch (Exception ex)
            {
                ABSMessageBox.Show("ERROR READING INI FILE: \n" + ex.Message);
            }
        }

        private void dtpBegin_ValueChanged(object sender, EventArgs e)
        {
            SetIntervalDate();
        }

        private void chkAutoUpdR1_Click(object sender, EventArgs e)
        {
            btnUpdR1.Enabled = !chkAutoUpdR1.Checked;
        }


        private void btnSetNumEstr_Click(object sender, EventArgs e)
        {
            try
            {
                if (Utils.IsNumber(txtNumEstr.Text))
                {
                    int numestr = Convert.ToInt32(txtNumEstr.Text);
                    this.DATA_INIZIO = DbDataAccess.GetDataEstrazioneNext(this.DATA_FINE, -numestr);
                    SetIntervalDate();
                }
            }
            catch { }
        }
    }
}
