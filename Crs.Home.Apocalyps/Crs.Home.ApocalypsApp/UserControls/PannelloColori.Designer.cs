namespace Crs.Home.ApocalypsApp.UserControls
{
    partial class PannelloColori
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitolo;

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
            lblTitolo = new Label();
            SuspendLayout();
            // 
            // lblTitolo
            // 
            lblTitolo.BackColor = Color.Gainsboro;
            lblTitolo.Dock = DockStyle.Top;
            lblTitolo.Font = new Font("Arial", 9F, FontStyle.Bold);
            lblTitolo.Location = new Point(5, 5);
            lblTitolo.Name = "lblTitolo";
            lblTitolo.Size = new Size(188, 25);
            lblTitolo.TabIndex = 0;
            lblTitolo.Text = "Pannelli Colori";
            lblTitolo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // PannelloColori
            // 
            BackColor = Color.WhiteSmoke;
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(lblTitolo);
            Name = "PannelloColori";
            Padding = new Padding(5);
            Size = new Size(198, 598);
            ResumeLayout(false);
        }
    }
}
