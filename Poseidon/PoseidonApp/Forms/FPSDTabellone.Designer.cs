namespace PoseidonApp.Forms
{
    partial class FPSDTabellone
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnRicarica = new System.Windows.Forms.Button();
            this.chkColoraPianetiDopoStelleNonSature = new System.Windows.Forms.CheckBox();
            this.chkColoraPianetiDopoStelleSature = new System.Windows.Forms.CheckBox();
            this.chkColoraStelle = new System.Windows.Forms.CheckBox();
            this.chkColoraPianeti = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkColoraStelleSatureFinoY = new System.Windows.Forms.CheckBox();
            this.psdTabellone1 = new PoseidonApp.UserControls.PSDTabellone();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chkColoraStelleSatureFinoY);
            this.panel1.Controls.Add(this.btnRicarica);
            this.panel1.Controls.Add(this.chkColoraPianetiDopoStelleNonSature);
            this.panel1.Controls.Add(this.chkColoraPianetiDopoStelleSature);
            this.panel1.Controls.Add(this.chkColoraStelle);
            this.panel1.Controls.Add(this.chkColoraPianeti);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1524, 62);
            this.panel1.TabIndex = 1;
            // 
            // btnRicarica
            // 
            this.btnRicarica.Location = new System.Drawing.Point(802, 10);
            this.btnRicarica.Name = "btnRicarica";
            this.btnRicarica.Size = new System.Drawing.Size(75, 44);
            this.btnRicarica.TabIndex = 5;
            this.btnRicarica.Text = "Ricarica";
            this.btnRicarica.UseVisualStyleBackColor = true;
            this.btnRicarica.Click += new System.EventHandler(this.BtnRicarica_Click);
            // 
            // chkColoraPianetiDopoStelleNonSature
            // 
            this.chkColoraPianetiDopoStelleNonSature.AutoSize = true;
            this.chkColoraPianetiDopoStelleNonSature.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.chkColoraPianetiDopoStelleNonSature.Checked = true;
            this.chkColoraPianetiDopoStelleNonSature.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkColoraPianetiDopoStelleNonSature.Location = new System.Drawing.Point(547, 36);
            this.chkColoraPianetiDopoStelleNonSature.Name = "chkColoraPianetiDopoStelleNonSature";
            this.chkColoraPianetiDopoStelleNonSature.Size = new System.Drawing.Size(198, 18);
            this.chkColoraPianetiDopoStelleNonSature.TabIndex = 4;
            this.chkColoraPianetiDopoStelleNonSature.Tag = "4";
            this.chkColoraPianetiDopoStelleNonSature.Text = "PIANETI dopo stelle NON sature";
            this.chkColoraPianetiDopoStelleNonSature.UseVisualStyleBackColor = false;
            // 
            // chkColoraPianetiDopoStelleSature
            // 
            this.chkColoraPianetiDopoStelleSature.AutoSize = true;
            this.chkColoraPianetiDopoStelleSature.BackColor = System.Drawing.Color.Yellow;
            this.chkColoraPianetiDopoStelleSature.Checked = true;
            this.chkColoraPianetiDopoStelleSature.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkColoraPianetiDopoStelleSature.Location = new System.Drawing.Point(351, 36);
            this.chkColoraPianetiDopoStelleSature.Name = "chkColoraPianetiDopoStelleSature";
            this.chkColoraPianetiDopoStelleSature.Size = new System.Drawing.Size(171, 18);
            this.chkColoraPianetiDopoStelleSature.TabIndex = 3;
            this.chkColoraPianetiDopoStelleSature.Tag = "3";
            this.chkColoraPianetiDopoStelleSature.Text = "PIANETI dopo stelle sature";
            this.chkColoraPianetiDopoStelleSature.UseVisualStyleBackColor = false;
            // 
            // chkColoraStelle
            // 
            this.chkColoraStelle.AutoSize = true;
            this.chkColoraStelle.Checked = true;
            this.chkColoraStelle.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkColoraStelle.ForeColor = System.Drawing.Color.Blue;
            this.chkColoraStelle.Location = new System.Drawing.Point(158, 14);
            this.chkColoraStelle.Name = "chkColoraStelle";
            this.chkColoraStelle.Size = new System.Drawing.Size(60, 18);
            this.chkColoraStelle.TabIndex = 2;
            this.chkColoraStelle.Tag = "1";
            this.chkColoraStelle.Text = "STELLE";
            this.chkColoraStelle.UseVisualStyleBackColor = true;
            // 
            // chkColoraPianeti
            // 
            this.chkColoraPianeti.AutoSize = true;
            this.chkColoraPianeti.Checked = true;
            this.chkColoraPianeti.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkColoraPianeti.ForeColor = System.Drawing.Color.Red;
            this.chkColoraPianeti.Location = new System.Drawing.Point(351, 14);
            this.chkColoraPianeti.Name = "chkColoraPianeti";
            this.chkColoraPianeti.Size = new System.Drawing.Size(67, 18);
            this.chkColoraPianeti.TabIndex = 1;
            this.chkColoraPianeti.Tag = "2";
            this.chkColoraPianeti.Text = "PIANETI";
            this.chkColoraPianeti.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(29, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "colora";
            // 
            // chkColoraStelleSatureFinoY
            // 
            this.chkColoraStelleSatureFinoY.AutoSize = true;
            this.chkColoraStelleSatureFinoY.Checked = true;
            this.chkColoraStelleSatureFinoY.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkColoraStelleSatureFinoY.ForeColor = System.Drawing.Color.Fuchsia;
            this.chkColoraStelleSatureFinoY.Location = new System.Drawing.Point(158, 38);
            this.chkColoraStelleSatureFinoY.Name = "chkColoraStelleSatureFinoY";
            this.chkColoraStelleSatureFinoY.Size = new System.Drawing.Size(144, 18);
            this.chkColoraStelleSatureFinoY.TabIndex = 6;
            this.chkColoraStelleSatureFinoY.Tag = "1";
            this.chkColoraStelleSatureFinoY.Text = "STELLE SATURE fino a Y";
            this.chkColoraStelleSatureFinoY.UseVisualStyleBackColor = true;
            // 
            // psdTabellone1
            // 
            this.psdTabellone1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.psdTabellone1.Location = new System.Drawing.Point(0, 62);
            this.psdTabellone1.Name = "psdTabellone1";
            this.psdTabellone1.SHOW_OPTIONS = false;
            this.psdTabellone1.SHOW_PNL_COLORS = false;
            this.psdTabellone1.Size = new System.Drawing.Size(1524, 621);
            this.psdTabellone1.TabIndex = 0;
            // 
            // FPSDTabellone
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1524, 683);
            this.Controls.Add(this.psdTabellone1);
            this.Controls.Add(this.panel1);
            this.Name = "FPSDTabellone";
            this.Text = "FPSDTabellone";
            this.TopMost = true;
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private UserControls.PSDTabellone psdTabellone1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox chkColoraPianetiDopoStelleNonSature;
        private System.Windows.Forms.CheckBox chkColoraPianetiDopoStelleSature;
        private System.Windows.Forms.CheckBox chkColoraStelle;
        private System.Windows.Forms.CheckBox chkColoraPianeti;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnRicarica;
        private System.Windows.Forms.CheckBox chkColoraStelleSatureFinoY;
    }
}