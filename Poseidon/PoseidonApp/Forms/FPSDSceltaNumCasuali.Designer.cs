namespace PoseidonApp.Forms
{
    partial class FPSDSceltaNumCasuali
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
            this.psdTavolaNumCasuali1 = new PoseidonApp.UserControls.PSDSceltaNumCasuali();
            this.pnlBtns = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.pnlBtns.SuspendLayout();
            this.SuspendLayout();
            // 
            // psdTavolaNumCasuali1
            // 
            this.psdTavolaNumCasuali1.Dock = System.Windows.Forms.DockStyle.Top;
            this.psdTavolaNumCasuali1.IsSetMode = true;
            this.psdTavolaNumCasuali1.Location = new System.Drawing.Point(0, 0);
            this.psdTavolaNumCasuali1.Name = "psdTavolaNumCasuali1";
            this.psdTavolaNumCasuali1.Size = new System.Drawing.Size(505, 358);
            this.psdTavolaNumCasuali1.TabIndex = 0;
            // 
            // pnlBtns
            // 
            this.pnlBtns.Controls.Add(this.btnCancel);
            this.pnlBtns.Controls.Add(this.btnOk);
            this.pnlBtns.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBtns.Location = new System.Drawing.Point(0, 366);
            this.pnlBtns.Name = "pnlBtns";
            this.pnlBtns.Size = new System.Drawing.Size(505, 41);
            this.pnlBtns.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(248, 3);
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
            this.btnOk.Location = new System.Drawing.Point(168, 3);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(74, 35);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // FPSDTavolaNumCasuali
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(505, 407);
            this.Controls.Add(this.pnlBtns);
            this.Controls.Add(this.psdTavolaNumCasuali1);
            this.Name = "FPSDTavolaNumCasuali";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tavola Numeri Casuali";
            this.TopMost = true;
            this.pnlBtns.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private UserControls.PSDSceltaNumCasuali psdTavolaNumCasuali1;
        private System.Windows.Forms.Panel pnlBtns;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
    }
}