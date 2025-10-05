namespace Crs.Home.ApocalypsApp.UserControls
{
    partial class HeaderTabellone
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DateTimePicker dateTimeInizio;
        private System.Windows.Forms.DateTimePicker dateTimeFine;
        private System.Windows.Forms.Button btnCaricaDati;
        private System.Windows.Forms.Button btnImporta;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;

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
            dateTimeInizio = new DateTimePicker();
            dateTimeFine = new DateTimePicker();
            btnCaricaDati = new Button();
            btnImporta = new Button();
            label1 = new Label();
            label2 = new Label();
            toolTip1 = new ToolTip(components);
            lblLastDate = new Label();
            SuspendLayout();
            // 
            // dateTimeInizio
            // 
            dateTimeInizio.Format = DateTimePickerFormat.Short;
            dateTimeInizio.Location = new Point(36, 22);
            dateTimeInizio.Name = "dateTimeInizio";
            dateTimeInizio.Size = new Size(120, 23);
            dateTimeInizio.TabIndex = 1;
            dateTimeInizio.Value = new DateTime(2025, 8, 30, 11, 2, 23, 764);
            // 
            // dateTimeFine
            // 
            dateTimeFine.Format = DateTimePickerFormat.Short;
            dateTimeFine.Location = new Point(186, 22);
            dateTimeFine.Name = "dateTimeFine";
            dateTimeFine.Size = new Size(120, 23);
            dateTimeFine.TabIndex = 3;
            dateTimeFine.Value = new DateTime(2025, 9, 30, 11, 2, 23, 766);
            // 
            // btnCaricaDati
            // 
            btnCaricaDati.BackColor = Color.LightSteelBlue;
            btnCaricaDati.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnCaricaDati.ForeColor = Color.White;
            btnCaricaDati.Location = new Point(381, 11);
            btnCaricaDati.Name = "btnCaricaDati";
            btnCaricaDati.Size = new Size(90, 44);
            btnCaricaDati.TabIndex = 4;
            btnCaricaDati.Text = "Carica Dati";
            btnCaricaDati.UseVisualStyleBackColor = false;
            btnCaricaDati.Click += BtnCaricaDati_Click;
            // 
            // btnImporta
            // 
            btnImporta.BackColor = Color.LightSteelBlue;
            btnImporta.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnImporta.ForeColor = Color.White;
            btnImporta.Location = new Point(477, 11);
            btnImporta.Name = "btnImporta";
            btnImporta.Size = new Size(112, 44);
            btnImporta.TabIndex = 6;
            btnImporta.Text = "Importa da File";
            btnImporta.UseVisualStyleBackColor = false;
            btnImporta.Click += BtnImporta_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(6, 25);
            label1.Name = "label1";
            label1.Size = new Size(25, 15);
            label1.TabIndex = 0;
            label1.Text = "Da:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            label2.Location = new Point(166, 25);
            label2.Name = "label2";
            label2.Size = new Size(18, 15);
            label2.TabIndex = 2;
            label2.Text = "A:";
            // 
            // lblLastDate
            // 
            lblLastDate.AutoSize = true;
            lblLastDate.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point);
            lblLastDate.Location = new Point(668, 21);
            lblLastDate.Name = "lblLastDate";
            lblLastDate.Size = new Size(145, 20);
            lblLastDate.TabIndex = 7;
            lblLastDate.Text = "Last Date Imported";
            // 
            // HeaderTabellone
            // 
            BackColor = Color.LightSteelBlue;
            Controls.Add(lblLastDate);
            Controls.Add(label1);
            Controls.Add(dateTimeInizio);
            Controls.Add(label2);
            Controls.Add(dateTimeFine);
            Controls.Add(btnCaricaDati);
            Controls.Add(btnImporta);
            Name = "HeaderTabellone";
            Padding = new Padding(10);
            Size = new Size(1343, 66);
            ResumeLayout(false);
            PerformLayout();
        }
        private ToolTip toolTip1;
        private Label lblLastDate;
    }
}