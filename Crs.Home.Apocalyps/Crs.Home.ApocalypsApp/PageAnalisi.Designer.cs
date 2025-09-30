

namespace Crs.Home.ApocalypsApp
{
    partial class PageAnalisi
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblAnalisi;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblAnalisi = new System.Windows.Forms.Label();

            // Main UserControl
            this.SuspendLayout();
            this.Size = new System.Drawing.Size(1000, 600);
            this.BackColor = System.Drawing.Color.White;

            // Label di esempio
            this.lblAnalisi.Text = "PAGINA ANALISI - Da implementare";
            this.lblAnalisi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAnalisi.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblAnalisi.Font = new System.Drawing.Font("Arial", 16, System.Drawing.FontStyle.Bold);
            this.lblAnalisi.ForeColor = System.Drawing.Color.DarkBlue;

            this.Controls.Add(lblAnalisi);
            this.ResumeLayout(false);
        }
    }
}
