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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            btnTabellone = new Button();
            imageList1 = new ImageList(components);
            btnAnalisi = new Button();
            panelHeader = new Panel();
            btnSettings = new Button();
            panelContainer = new Panel();
            panelHeader.SuspendLayout();
            SuspendLayout();
            // 
            // btnTabellone
            // 
            btnTabellone.BackColor = Color.LightSteelBlue;
            btnTabellone.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point);
            btnTabellone.ForeColor = SystemColors.ControlLightLight;
            btnTabellone.ImageAlign = ContentAlignment.MiddleLeft;
            btnTabellone.ImageKey = "icons8-search-property-24.png";
            btnTabellone.ImageList = imageList1;
            btnTabellone.Location = new Point(12, 7);
            btnTabellone.Name = "btnTabellone";
            btnTabellone.Size = new Size(120, 51);
            btnTabellone.TabIndex = 0;
            btnTabellone.Text = "Tabellone";
            btnTabellone.TextAlign = ContentAlignment.MiddleRight;
            btnTabellone.UseVisualStyleBackColor = false;
            btnTabellone.Click += BtnTabellone_Click;
            // 
            // imageList1
            // 
            imageList1.ColorDepth = ColorDepth.Depth24Bit;
            imageList1.ImageStream = (ImageListStreamer)resources.GetObject("imageList1.ImageStream");
            imageList1.TransparentColor = Color.Transparent;
            imageList1.Images.SetKeyName(0, "copy.png");
            imageList1.Images.SetKeyName(1, "icons8-add-24.png");
            imageList1.Images.SetKeyName(2, "icons8-address-book-24.png");
            imageList1.Images.SetKeyName(3, "icons8-aggiornamenti-disponibili-24.png");
            imageList1.Images.SetKeyName(4, "icons8-automation-24.png");
            imageList1.Images.SetKeyName(5, "icons8-checkmark-24.png");
            imageList1.Images.SetKeyName(6, "icons8-close-pane-24.png");
            imageList1.Images.SetKeyName(7, "icons8-comments-24.png");
            imageList1.Images.SetKeyName(8, "icons8-copy-24.png");
            imageList1.Images.SetKeyName(9, "icons8-copy-24_1.png");
            imageList1.Images.SetKeyName(10, "icons8-delete-24.png");
            imageList1.Images.SetKeyName(11, "icons8-done-24.png");
            imageList1.Images.SetKeyName(12, "icons8-doppia-a-destra-24.png");
            imageList1.Images.SetKeyName(13, "icons8-double-left-24 (3).png");
            imageList1.Images.SetKeyName(14, "icons8-double-left-24.png");
            imageList1.Images.SetKeyName(15, "icons8-double-left-32.png");
            imageList1.Images.SetKeyName(16, "icons8-double-right-24.png");
            imageList1.Images.SetKeyName(17, "icons8-double-right-32.png");
            imageList1.Images.SetKeyName(18, "icons8-download-24.png");
            imageList1.Images.SetKeyName(19, "icons8-drag-24.png");
            imageList1.Images.SetKeyName(20, "icons8-edit-24 (3).png");
            imageList1.Images.SetKeyName(21, "icons8-edit-24 (4).png");
            imageList1.Images.SetKeyName(22, "icons8-edit-24.png");
            imageList1.Images.SetKeyName(23, "icons8-edit-24_2.png");
            imageList1.Images.SetKeyName(24, "icons8-edit-file-24.png");
            imageList1.Images.SetKeyName(25, "icons8-enter-24.png");
            imageList1.Images.SetKeyName(26, "icons8-error-24.png");
            imageList1.Images.SetKeyName(27, "icons8-external-link1-24.png");
            imageList1.Images.SetKeyName(28, "icons8-filter-24.png");
            imageList1.Images.SetKeyName(29, "icons8-forward-26.png");
            imageList1.Images.SetKeyName(30, "icons8-future-32.png");
            imageList1.Images.SetKeyName(31, "icons8-help-24.png");
            imageList1.Images.SetKeyName(32, "icons8-home-page-24.png");
            imageList1.Images.SetKeyName(33, "icons8-info-24.png");
            imageList1.Images.SetKeyName(34, "icons8-ingranaggio-24.png");
            imageList1.Images.SetKeyName(35, "icons8-istogramma-24.png");
            imageList1.Images.SetKeyName(36, "icons8-level-up-24.png");
            imageList1.Images.SetKeyName(37, "icons8-list-24.png");
            imageList1.Images.SetKeyName(38, "icons8-meno-24 (1).png");
            imageList1.Images.SetKeyName(39, "icons8-meno-24.png");
            imageList1.Images.SetKeyName(40, "icons8-minus-24.png");
            imageList1.Images.SetKeyName(41, "icons8-notification-24.png");
            imageList1.Images.SetKeyName(42, "icons8-ok-24.png");
            imageList1.Images.SetKeyName(43, "icons8-open-door-24.png");
            imageList1.Images.SetKeyName(44, "icons8-pause-button-24a.png");
            imageList1.Images.SetKeyName(45, "icons8-pause-button-24b.png");
            imageList1.Images.SetKeyName(46, "icons8-pause-button-24c.png");
            imageList1.Images.SetKeyName(47, "icons8-play-24.png");
            imageList1.Images.SetKeyName(48, "icons8-processo-24.png");
            imageList1.Images.SetKeyName(49, "icons8-rimuovere-24.png");
            imageList1.Images.SetKeyName(50, "icons8-ripeti-24.png");
            imageList1.Images.SetKeyName(51, "icons8-ruota-a-destra-24.png");
            imageList1.Images.SetKeyName(52, "icons8-save-24.png");
            imageList1.Images.SetKeyName(53, "icons8-search-24 (1).png");
            imageList1.Images.SetKeyName(54, "icons8-search-24.png");
            imageList1.Images.SetKeyName(55, "icons8-search-property-24.png");
            imageList1.Images.SetKeyName(56, "icons8-shift-down-24.png");
            imageList1.Images.SetKeyName(57, "icons8-shift-up-24.png");
            imageList1.Images.SetKeyName(58, "icons8-stop1-24.png");
            imageList1.Images.SetKeyName(59, "icons8-stop-24.png");
            imageList1.Images.SetKeyName(60, "icons8-support-24.png");
            imageList1.Images.SetKeyName(61, "icons8-timer1-24.png");
            imageList1.Images.SetKeyName(62, "icons8-timer-24.png");
            // 
            // btnAnalisi
            // 
            btnAnalisi.BackColor = Color.WhiteSmoke;
            btnAnalisi.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point);
            btnAnalisi.ImageAlign = ContentAlignment.MiddleLeft;
            btnAnalisi.ImageKey = "icons8-istogramma-24.png";
            btnAnalisi.ImageList = imageList1;
            btnAnalisi.Location = new Point(139, 7);
            btnAnalisi.Name = "btnAnalisi";
            btnAnalisi.Size = new Size(120, 51);
            btnAnalisi.TabIndex = 1;
            btnAnalisi.Text = "Analisi";
            btnAnalisi.TextAlign = ContentAlignment.MiddleRight;
            btnAnalisi.UseVisualStyleBackColor = false;
            btnAnalisi.Click += BtnAnalisi_Click;
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.Gainsboro;
            panelHeader.BorderStyle = BorderStyle.FixedSingle;
            panelHeader.Controls.Add(btnSettings);
            panelHeader.Controls.Add(btnTabellone);
            panelHeader.Controls.Add(btnAnalisi);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(1525, 67);
            panelHeader.TabIndex = 1;
            // 
            // btnSettings
            // 
            btnSettings.BackColor = Color.WhiteSmoke;
            btnSettings.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point);
            btnSettings.ImageAlign = ContentAlignment.MiddleLeft;
            btnSettings.ImageKey = "icons8-ingranaggio-24.png";
            btnSettings.ImageList = imageList1;
            btnSettings.Location = new Point(269, 7);
            btnSettings.Name = "btnSettings";
            btnSettings.Size = new Size(135, 51);
            btnSettings.TabIndex = 2;
            btnSettings.Text = "Impostazioni";
            btnSettings.TextAlign = ContentAlignment.MiddleRight;
            btnSettings.UseVisualStyleBackColor = false;
            btnSettings.Click += btnSettings_Click;
            // 
            // panelContainer
            // 
            panelContainer.BackColor = Color.White;
            panelContainer.Dock = DockStyle.Fill;
            panelContainer.Location = new Point(0, 67);
            panelContainer.Name = "panelContainer";
            panelContainer.Size = new Size(1525, 660);
            panelContainer.TabIndex = 0;
            // 
            // MainForm
            // 
            ClientSize = new Size(1525, 727);
            Controls.Add(panelContainer);
            Controls.Add(panelHeader);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Lotto Application";
            panelHeader.ResumeLayout(false);
            ResumeLayout(false);
        }
        private Button btnSettings;
        private ImageList imageList1;
    }
    
}
