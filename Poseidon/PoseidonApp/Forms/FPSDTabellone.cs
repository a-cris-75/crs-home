using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CRS.Library;
using PoseidonData.DataEntities;

namespace PoseidonApp.Forms
{
    public partial class FPSDTabellone : Form
    {
        public FPSDTabellone()
        {
            InitializeComponent();
        }

        public DateTime DATA_EVENTO_DA;
        public DateTime DATA_EVENTO_A;
        List<PSD_MATH_MATRIX_NUM> MATRIX1 = new List<PSD_MATH_MATRIX_NUM>();
        List<PSD_MATH_MATRIX_NUM> MATRIX2 = new List<PSD_MATH_MATRIX_NUM>();
        List<PSD_MATH_MATRIX_NUM> MATRIX3 = new List<PSD_MATH_MATRIX_NUM>();
        List<PSD_MATH_MATRIX_NUM> MATRIX4 = new List<PSD_MATH_MATRIX_NUM>();
        List<PSD_MATH_MATRIX_NUM> MATRIX5 = new List<PSD_MATH_MATRIX_NUM>();

        public void FillTabellone(DateTime dt1, DateTime dt2)
        {
            this.DATA_EVENTO_DA = dt1;
            this.DATA_EVENTO_A = dt2;
            psdTabellone1.FillTabellone(dt1, dt2);
            //psdTabellone1.SetColorGrid(lstOccorrenze, Color.Transparent);
        }

        public void ColoraTabellone(List<PSD_ESTR_SUCCESSI> lstOccorrenze, Color c)
        {
            psdTabellone1.SetColorGrid(lstOccorrenze, c);
        }
        
        public void ColoraTabelloneByPos<T>(List<T> matrix, int tag, Color cback, Color cfore,int deltaX_from1)
        {
            psdTabellone1.SetColorGridByPos<T>(matrix, cback,cfore, deltaX_from1);
            switch (tag)
            {
                case 1:
                    if(matrix is List<PSD_MATH_MATRIX_NUM>)
                        MATRIX1 = Utils.CopyListToList<PSD_MATH_MATRIX_NUM>( (matrix as List<PSD_MATH_MATRIX_NUM>).ToList<object>());
                    break;
                case 2:
                    if (matrix is List<PSD_MATH_MATRIX_NUM>)
                        MATRIX2 = Utils.CopyListToList<PSD_MATH_MATRIX_NUM>((matrix as List<PSD_MATH_MATRIX_NUM>).ToList<object>());
                    break;
                case 3:
                    if (matrix is List<PSD_MATH_MATRIX_NUM>)
                        MATRIX3 = Utils.CopyListToList<PSD_MATH_MATRIX_NUM>((matrix as List<PSD_MATH_MATRIX_NUM>).ToList<object>());
                    break;
                case 4:
                    if (matrix is List<PSD_MATH_MATRIX_NUM>)
                        MATRIX4 = Utils.CopyListToList<PSD_MATH_MATRIX_NUM>((matrix as List<PSD_MATH_MATRIX_NUM>).ToList<object>());
                    break;
                case 5:
                    if (matrix is List<PSD_MATH_MATRIX_NUM>)
                        MATRIX5 = Utils.CopyListToList<PSD_MATH_MATRIX_NUM>((matrix as List<PSD_MATH_MATRIX_NUM>).ToList<object>());
                    break;
            }
        }

        private void BtnRicarica_Click(object sender, EventArgs e)
        {
            FillTabellone(this.DATA_EVENTO_DA, this.DATA_EVENTO_A);
            if(chkColoraStelle.Checked)
                psdTabellone1.SetColorGridByPos<PSD_MATH_MATRIX_NUM>(MATRIX1, Color.Transparent, chkColoraStelle.ForeColor, 2);
            if (chkColoraPianeti.Checked)
                psdTabellone1.SetColorGridByPos<PSD_MATH_MATRIX_NUM>(MATRIX2, Color.Transparent, chkColoraPianeti.ForeColor, 2);
            if (chkColoraPianetiDopoStelleSature.Checked)
                psdTabellone1.SetColorGridByPos<PSD_MATH_MATRIX_NUM>(MATRIX3, chkColoraPianetiDopoStelleSature.BackColor, Color.Transparent, 2);
            if (chkColoraPianetiDopoStelleNonSature.Checked)
                psdTabellone1.SetColorGridByPos<PSD_MATH_MATRIX_NUM>(MATRIX4, chkColoraPianetiDopoStelleNonSature.BackColor, Color.Transparent, 2);
            if (chkColoraStelleSatureFinoY.Checked)
                psdTabellone1.SetColorGridByPos<PSD_MATH_MATRIX_NUM>(MATRIX5, Color.Transparent, chkColoraStelleSatureFinoY.ForeColor, 2);
        }
    }
}
