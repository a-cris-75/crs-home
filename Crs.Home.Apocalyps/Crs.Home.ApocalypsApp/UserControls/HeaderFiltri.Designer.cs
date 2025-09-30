namespace Crs.Home.ApocalypsApp.UserControls
{
    partial class HeaderFiltri
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DateTimePicker dateTimeInizio;
        private System.Windows.Forms.DateTimePicker dateTimeFine;
        private System.Windows.Forms.Button btnCaricaDati;
        private System.Windows.Forms.Button btnImporta;
        private System.Windows.Forms.RadioButton radioFormatoSingolaRiga;
        private System.Windows.Forms.RadioButton radioFormatoMultiplaRiga;
        private System.Windows.Forms.GroupBox groupBoxFormato;
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
            dateTimeInizio = new DateTimePicker();
            dateTimeFine = new DateTimePicker();
            btnCaricaDati = new Button();
            btnImporta = new Button();
            radioFormatoSingolaRiga = new RadioButton();
            radioFormatoMultiplaRiga = new RadioButton();
            groupBoxFormato = new GroupBox();
            label1 = new Label();
            label2 = new Label();
            groupBoxFormato.SuspendLayout();
            SuspendLayout();
            // 
            // dateTimeInizio
            // 
            dateTimeInizio.Format = DateTimePickerFormat.Short;
            dateTimeInizio.Location = new Point(40, 18);
            dateTimeInizio.Name = "dateTimeInizio";
            dateTimeInizio.Size = new Size(120, 23);
            dateTimeInizio.TabIndex = 1;
            dateTimeInizio.Value = new DateTime(2025, 8, 30, 11, 2, 23, 764);
            // 
            // dateTimeFine
            // 
            dateTimeFine.Format = DateTimePickerFormat.Short;
            dateTimeFine.Location = new Point(190, 18);
            dateTimeFine.Name = "dateTimeFine";
            dateTimeFine.Size = new Size(120, 23);
            dateTimeFine.TabIndex = 3;
            dateTimeFine.Value = new DateTime(2025, 9, 30, 11, 2, 23, 766);
            // 
            // btnCaricaDati
            // 
            btnCaricaDati.BackColor = Color.SteelBlue;
            btnCaricaDati.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnCaricaDati.ForeColor = Color.White;
            btnCaricaDati.Location = new Point(320, 15);
            btnCaricaDati.Name = "btnCaricaDati";
            btnCaricaDati.Size = new Size(90, 30);
            btnCaricaDati.TabIndex = 4;
            btnCaricaDati.Text = "Carica Dati";
            btnCaricaDati.UseVisualStyleBackColor = false;
            btnCaricaDati.Click += BtnCaricaDati_Click;
            // 
            // btnImporta
            // 
            btnImporta.BackColor = Color.DeepSkyBlue;
            btnImporta.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnImporta.ForeColor = Color.White;
            btnImporta.Location = new Point(795, 16);
            btnImporta.Name = "btnImporta";
            btnImporta.Size = new Size(100, 30);
            btnImporta.TabIndex = 6;
            btnImporta.Text = "Importa da File";
            btnImporta.UseVisualStyleBackColor = false;
            btnImporta.Click += BtnImporta_Click;
            // 
            // radioFormatoSingolaRiga
            // 
            radioFormatoSingolaRiga.Checked = true;
            radioFormatoSingolaRiga.Font = new Font("Segoe UI", 9F);
            radioFormatoSingolaRiga.Location = new Point(10, 15);
            radioFormatoSingolaRiga.Name = "radioFormatoSingolaRiga";
            radioFormatoSingolaRiga.Size = new Size(170, 20);
            radioFormatoSingolaRiga.TabIndex = 0;
            radioFormatoSingolaRiga.TabStop = true;
            radioFormatoSingolaRiga.Text = "Singola riga per estrazione";
            // 
            // radioFormatoMultiplaRiga
            // 
            radioFormatoMultiplaRiga.Font = new Font("Segoe UI", 9F);
            radioFormatoMultiplaRiga.Location = new Point(180, 15);
            radioFormatoMultiplaRiga.Name = "radioFormatoMultiplaRiga";
            radioFormatoMultiplaRiga.Size = new Size(180, 20);
            radioFormatoMultiplaRiga.TabIndex = 1;
            radioFormatoMultiplaRiga.Text = "Riga per ruota per estrazione";
            // 
            // groupBoxFormato
            // 
            groupBoxFormato.Controls.Add(radioFormatoSingolaRiga);
            groupBoxFormato.Controls.Add(radioFormatoMultiplaRiga);
            groupBoxFormato.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            groupBoxFormato.Location = new Point(420, 5);
            groupBoxFormato.Name = "groupBoxFormato";
            groupBoxFormato.Size = new Size(369, 44);
            groupBoxFormato.TabIndex = 5;
            groupBoxFormato.TabStop = false;
            groupBoxFormato.Text = "Formato Importazione";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label1.Location = new Point(10, 21);
            label1.Name = "label1";
            label1.Size = new Size(25, 15);
            label1.TabIndex = 0;
            label1.Text = "Da:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label2.Location = new Point(170, 21);
            label2.Name = "label2";
            label2.Size = new Size(18, 15);
            label2.TabIndex = 2;
            label2.Text = "A:";
            // 
            // HeaderFiltri
            // 
            BackColor = Color.LightSkyBlue;
            Controls.Add(label1);
            Controls.Add(dateTimeInizio);
            Controls.Add(label2);
            Controls.Add(dateTimeFine);
            Controls.Add(btnCaricaDati);
            Controls.Add(groupBoxFormato);
            Controls.Add(btnImporta);
            Name = "HeaderFiltri";
            Padding = new Padding(10);
            Size = new Size(935, 62);
            groupBoxFormato.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }
    }
}