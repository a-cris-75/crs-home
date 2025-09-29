namespace PoseidonApp.Forms
{
    partial class FPSDSceltaR1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FPSDSceltaR1));
            this.psdSceltaR11 = new PoseidonApp.UserControls.PSDSceltaR1();
            this.pnlBtns = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.pnlBtns.SuspendLayout();
            this.SuspendLayout();
            // 
            // psdSceltaR11
            // 
            this.psdSceltaR11.ACCOPPIATE_DEC_R12 = ((System.Collections.Generic.List<string>)(resources.GetObject("psdSceltaR11.ACCOPPIATE_DEC_R12")));
            this.psdSceltaR11.COLPI = ((System.Collections.Generic.List<int>)(resources.GetObject("psdSceltaR11.COLPI")));
            this.psdSceltaR11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.psdSceltaR11.Location = new System.Drawing.Point(0, 0);
            this.psdSceltaR11.Name = "psdSceltaR11";
            this.psdSceltaR11.POS_DEC_R12 = ((System.Collections.Generic.List<string>)(resources.GetObject("psdSceltaR11.POS_DEC_R12")));
            this.psdSceltaR11.Size = new System.Drawing.Size(873, 81);
            this.psdSceltaR11.TabIndex = 1;
            this.psdSceltaR11.TIPO_FREQUENZA = ((System.Collections.Generic.List<int>)(resources.GetObject("psdSceltaR11.TIPO_FREQUENZA")));
            // 
            // pnlBtns
            // 
            this.pnlBtns.Controls.Add(this.btnCancel);
            this.pnlBtns.Controls.Add(this.btnOk);
            this.pnlBtns.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBtns.Location = new System.Drawing.Point(0, 81);
            this.pnlBtns.Name = "pnlBtns";
            this.pnlBtns.Size = new System.Drawing.Size(873, 41);
            this.pnlBtns.TabIndex = 2;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(413, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(74, 35);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.Location = new System.Drawing.Point(333, 3);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(74, 35);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // FPSDSceltaR1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(873, 122);
            this.Controls.Add(this.psdSceltaR11);
            this.Controls.Add(this.pnlBtns);
            this.Name = "FPSDSceltaR1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Scelta R1";
            this.pnlBtns.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private UserControls.PSDSceltaR1 psdSceltaR11;
        private System.Windows.Forms.Panel pnlBtns;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
    }
}