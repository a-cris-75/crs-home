namespace Crs.Home.ApocalypsApp
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button btnTabellone;
        private System.Windows.Forms.Button btnAnalisi;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Panel panelContainer;

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
            btnTabellone = new Button();
            btnAnalisi = new Button();
            panelHeader = new Panel();
            panelContainer = new Panel();
            panelHeader.SuspendLayout();
            SuspendLayout();
            // 
            // btnTabellone
            // 
            btnTabellone.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnTabellone.Location = new Point(20, 7);
            btnTabellone.Name = "btnTabellone";
            btnTabellone.Size = new Size(100, 51);
            btnTabellone.TabIndex = 0;
            btnTabellone.Text = "Tabellone";
            btnTabellone.Click += BtnTabellone_Click;
            // 
            // btnAnalisi
            // 
            btnAnalisi.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnAnalisi.Location = new Point(130, 7);
            btnAnalisi.Name = "btnAnalisi";
            btnAnalisi.Size = new Size(100, 51);
            btnAnalisi.TabIndex = 1;
            btnAnalisi.Text = "Analisi";
            btnAnalisi.Click += BtnAnalisi_Click;
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.LightGray;
            panelHeader.Controls.Add(btnTabellone);
            panelHeader.Controls.Add(btnAnalisi);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(1184, 67);
            panelHeader.TabIndex = 1;
            // 
            // panelContainer
            // 
            panelContainer.BackColor = Color.White;
            panelContainer.Dock = DockStyle.Fill;
            panelContainer.Location = new Point(0, 67);
            panelContainer.Name = "panelContainer";
            panelContainer.Size = new Size(1184, 594);
            panelContainer.TabIndex = 0;
            // 
            // MainForm
            // 
            ClientSize = new Size(1184, 661);
            Controls.Add(panelContainer);
            Controls.Add(panelHeader);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Lotto Application";
            panelHeader.ResumeLayout(false);
            ResumeLayout(false);
        }
    }
    
}
