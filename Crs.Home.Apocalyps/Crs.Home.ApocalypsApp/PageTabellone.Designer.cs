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
            panelCentrale = new Panel();
            grigliaEstrazioni = new DataGridView();
            panelDestra = new Panel();
            lblTitoloPannelli = new Label();
            splitContainer = new SplitContainer();
            pnlHeader = new Panel();
            panelFiltri.SuspendLayout();
            panelCentrale.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)grigliaEstrazioni).BeginInit();
            panelDestra.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
            splitContainer.Panel1.SuspendLayout();
            splitContainer.Panel2.SuspendLayout();
            splitContainer.SuspendLayout();
            pnlHeader.SuspendLayout();
            SuspendLayout();
            // 
            // panelFiltri
            // 
            panelFiltri.BackColor = Color.Silver;
            panelFiltri.Controls.Add(headerTabellone1);
            panelFiltri.Dock = DockStyle.Fill;
            panelFiltri.Location = new Point(2, 2);
            panelFiltri.Name = "panelFiltri";
            panelFiltri.Padding = new Padding(1);
            panelFiltri.Size = new Size(1205, 68);
            panelFiltri.TabIndex = 1;
            // 
            // headerTabellone1
            // 
            headerTabellone1.BackColor = Color.LightSteelBlue;
            headerTabellone1.Dock = DockStyle.Fill;
            headerTabellone1.GrigliaDestinazione = null;
            headerTabellone1.Location = new Point(1, 1);
            headerTabellone1.Name = "headerTabellone1";
            headerTabellone1.Padding = new Padding(10);
            headerTabellone1.Size = new Size(1203, 66);
            headerTabellone1.TabIndex = 2;
            // 
            // panelCentrale
            // 
            panelCentrale.BackColor = Color.White;
            panelCentrale.Controls.Add(grigliaEstrazioni);
            panelCentrale.Dock = DockStyle.Fill;
            panelCentrale.Location = new Point(0, 0);
            panelCentrale.Name = "panelCentrale";
            panelCentrale.Padding = new Padding(5);
            panelCentrale.Size = new Size(967, 528);
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
            grigliaEstrazioni.Size = new Size(957, 518);
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
            panelDestra.Size = new Size(237, 528);
            panelDestra.TabIndex = 0;
            // 
            // lblTitoloPannelli
            // 
            lblTitoloPannelli.Dock = DockStyle.Top;
            lblTitoloPannelli.Font = new Font("Calibri", 10F, FontStyle.Bold, GraphicsUnit.Point);
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
            splitContainer.Location = new Point(0, 72);
            splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            splitContainer.Panel1.Controls.Add(panelCentrale);
            // 
            // splitContainer.Panel2
            // 
            splitContainer.Panel2.Controls.Add(panelDestra);
            splitContainer.Size = new Size(1209, 528);
            splitContainer.SplitterDistance = 967;
            splitContainer.SplitterWidth = 5;
            splitContainer.TabIndex = 0;
            // 
            // pnlHeader
            // 
            pnlHeader.Controls.Add(panelFiltri);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Padding = new Padding(2);
            pnlHeader.Size = new Size(1209, 72);
            pnlHeader.TabIndex = 1;
            // 
            // PageTabellone
            // 
            BackColor = Color.White;
            Controls.Add(splitContainer);
            Controls.Add(pnlHeader);
            Name = "PageTabellone";
            Size = new Size(1209, 600);
            panelFiltri.ResumeLayout(false);
            panelCentrale.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)grigliaEstrazioni).EndInit();
            panelDestra.ResumeLayout(false);
            splitContainer.Panel1.ResumeLayout(false);
            splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer).EndInit();
            splitContainer.ResumeLayout(false);
            pnlHeader.ResumeLayout(false);
            ResumeLayout(false);
        }
        private UserControls.HeaderTabellone headerTabellone1;
        private Panel pnlHeader;
    }
}