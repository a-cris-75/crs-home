namespace Crs.Home.ApocalypsApp
{
    partial class PageTabellone: UserControl
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panelFiltri;
        private System.Windows.Forms.Panel panelCentrale;
        private System.Windows.Forms.Panel panelDestra;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.DataGridView grigliaEstrazioni;
        private System.Windows.Forms.Label lblTitoloFiltri;
        private System.Windows.Forms.Label lblTitoloPannelli;

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
            panelFiltri = new Panel();
            headerTabellone1 = new Crs.Home.ApocalypsApp.UserControls.HeaderTabellone();
            lblTitoloFiltri = new Label();
            panelCentrale = new Panel();
            grigliaEstrazioni = new DataGridView();
            panelDestra = new Panel();
            lblTitoloPannelli = new Label();
            splitContainer = new SplitContainer();
            panelFiltri.SuspendLayout();
            panelCentrale.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)grigliaEstrazioni).BeginInit();
            panelDestra.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
            splitContainer.Panel1.SuspendLayout();
            splitContainer.Panel2.SuspendLayout();
            splitContainer.SuspendLayout();
            SuspendLayout();
            // 
            // panelFiltri
            // 
            panelFiltri.BackColor = Color.Gainsboro;
            panelFiltri.Controls.Add(headerTabellone1);
            panelFiltri.Controls.Add(lblTitoloFiltri);
            panelFiltri.Dock = DockStyle.Top;
            panelFiltri.Location = new Point(0, 0);
            panelFiltri.Name = "panelFiltri";
            panelFiltri.Size = new Size(1209, 68);
            panelFiltri.TabIndex = 1;
            // 
            // headerTabellone1
            // 
            headerTabellone1.BackColor = Color.LightSkyBlue;
            headerTabellone1.Dock = DockStyle.Fill;
            headerTabellone1.GrigliaDestinazione = null;
            headerTabellone1.Location = new Point(0, 0);
            headerTabellone1.Name = "headerTabellone1";
            headerTabellone1.Padding = new Padding(10);
            headerTabellone1.Size = new Size(1209, 68);
            headerTabellone1.TabIndex = 2;
            // 
            // lblTitoloFiltri
            // 
            lblTitoloFiltri.AutoSize = true;
            lblTitoloFiltri.Font = new Font("Arial", 10F, FontStyle.Bold, GraphicsUnit.Point);
            lblTitoloFiltri.Location = new Point(3, 13);
            lblTitoloFiltri.Name = "lblTitoloFiltri";
            lblTitoloFiltri.Size = new Size(112, 16);
            lblTitoloFiltri.TabIndex = 0;
            lblTitoloFiltri.Text = "Filtri Estrazioni";
            lblTitoloFiltri.Visible = false;
            // 
            // panelCentrale
            // 
            panelCentrale.BackColor = Color.White;
            panelCentrale.Controls.Add(grigliaEstrazioni);
            panelCentrale.Dock = DockStyle.Fill;
            panelCentrale.Location = new Point(0, 0);
            panelCentrale.Name = "panelCentrale";
            panelCentrale.Padding = new Padding(5);
            panelCentrale.Size = new Size(967, 532);
            panelCentrale.TabIndex = 0;
            // 
            // grigliaEstrazioni
            // 
            grigliaEstrazioni.AllowUserToAddRows = false;
            grigliaEstrazioni.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            grigliaEstrazioni.BackgroundColor = Color.White;
            grigliaEstrazioni.Dock = DockStyle.Fill;
            grigliaEstrazioni.Location = new Point(5, 5);
            grigliaEstrazioni.Name = "grigliaEstrazioni";
            grigliaEstrazioni.ReadOnly = true;
            grigliaEstrazioni.Size = new Size(957, 522);
            grigliaEstrazioni.TabIndex = 0;
            // 
            // panelDestra
            // 
            panelDestra.AutoScroll = true;
            panelDestra.BackColor = Color.WhiteSmoke;
            panelDestra.Controls.Add(lblTitoloPannelli);
            panelDestra.Dock = DockStyle.Fill;
            panelDestra.Location = new Point(0, 0);
            panelDestra.Name = "panelDestra";
            panelDestra.Padding = new Padding(5);
            panelDestra.Size = new Size(237, 532);
            panelDestra.TabIndex = 0;
            // 
            // lblTitoloPannelli
            // 
            lblTitoloPannelli.Dock = DockStyle.Top;
            lblTitoloPannelli.Font = new Font("Arial", 9F, FontStyle.Bold, GraphicsUnit.Point);
            lblTitoloPannelli.Location = new Point(5, 5);
            lblTitoloPannelli.Name = "lblTitoloPannelli";
            lblTitoloPannelli.Size = new Size(227, 25);
            lblTitoloPannelli.TabIndex = 0;
            lblTitoloPannelli.Text = "Pannelli Colori";
            lblTitoloPannelli.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // splitContainer
            // 
            splitContainer.Dock = DockStyle.Fill;
            splitContainer.Location = new Point(0, 68);
            splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            splitContainer.Panel1.Controls.Add(panelCentrale);
            // 
            // splitContainer.Panel2
            // 
            splitContainer.Panel2.Controls.Add(panelDestra);
            splitContainer.Size = new Size(1209, 532);
            splitContainer.SplitterDistance = 967;
            splitContainer.SplitterWidth = 5;
            splitContainer.TabIndex = 0;
            // 
            // PageTabellone
            // 
            BackColor = Color.White;
            Controls.Add(splitContainer);
            Controls.Add(panelFiltri);
            Name = "PageTabellone";
            Size = new Size(1209, 600);
            panelFiltri.ResumeLayout(false);
            panelFiltri.PerformLayout();
            panelCentrale.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)grigliaEstrazioni).EndInit();
            panelDestra.ResumeLayout(false);
            splitContainer.Panel1.ResumeLayout(false);
            splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer).EndInit();
            splitContainer.ResumeLayout(false);
            ResumeLayout(false);
        }
        private UserControls.HeaderTabellone headerTabellone1;
    }
}