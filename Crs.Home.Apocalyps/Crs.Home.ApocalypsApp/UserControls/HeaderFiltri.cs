using Crs.Home.ApocalypsData.DataEntities;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Crs.Home.ApocalypsApp.UserControls
{
    public partial class HeaderFiltri : UserControl
    {
        public DataGridView GrigliaDestinazione { get; set; }

        public HeaderFiltri()
        {
            InitializeComponent();
        }

        public event EventHandler OnImportazioneCompletata;

        private void BtnCaricaDati_Click(object sender, EventArgs e)
        {
            if (GrigliaDestinazione != null)
            {
                // Simula il caricamento dati
                CaricaDatiGriglia();
                MessageBox.Show("Dati caricati nella griglia!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Griglia di destinazione non impostata!", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnImporta_Click(object sender, EventArgs e)
        {
            bool formatoSingolaRiga = radioFormatoSingolaRiga.Checked;
            ApriDialogImportazione(formatoSingolaRiga);
        }

        private void ApriDialogImportazione(bool formatoSingolaRiga)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "File di testo (*.txt)|*.txt|Tutti i file (*.*)|*.*";
                openFileDialog.Title = "Seleziona file estrazioni";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    ImportaDatiDaFile(filePath, formatoSingolaRiga);
                }
            }
        }

        private void ImportaDatiDaFile(string filePath, bool formatoSingolaRiga)
        {
            try
            {
                // Simula l'importazione dei dati
                var estrazioniImportate = formatoSingolaRiga ?
                    ImportaFormatoSingolaRiga(filePath) :
                    ImportaFormatoMultiplaRiga(filePath);

                // Qui puoi salvare nel database o processare i dati
                MessageBox.Show($"Importate {estrazioniImportate.Count} estrazioni dal file!",
                    "Importazione Completata", MessageBoxButtons.OK, MessageBoxIcon.Information);

                OnImportazioneCompletata?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Errore durante l'importazione: {ex.Message}",
                    "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private System.Collections.Generic.List<Estrazione> ImportaFormatoSingolaRiga(string filePath)
        {
            // Simula l'importazione formato singola riga per estrazione
            var estrazioni = new System.Collections.Generic.List<Estrazione>();

            // Esempio: leggere il file e parsare i dati
            // string[] lines = System.IO.File.ReadAllLines(filePath);
            // foreach (string line in lines) { ... }

            // Dati di esempio
            //Random rnd = new Random();
            //for (int i = 0; i < 10; i++)
            //{
            //    var estrazione = new Estrazione
            //    {
            //        DataEstrazione = DateTime.Now.AddDays(-i),
            //        NumeroEstrazione = 1000 + i,
            //        Ruota = "Tutte",
            //        NumeriEstratti = new System.Collections.Generic.List<int>
            //        {
            //            rnd.Next(1, 91),
            //            rnd.Next(1, 91),
            //            rnd.Next(1, 91),
            //            rnd.Next(1, 91),
            //            rnd.Next(1, 91)
            //        }
            //    };
            //    estrazioni.Add(estrazione);
            //}

            return estrazioni;
        }

        private System.Collections.Generic.List<Estrazione> ImportaFormatoMultiplaRiga(string filePath)
        {
            // Simula l'importazione formato multipla riga (una per ruota per estrazione)
            var estrazioni = new System.Collections.Generic.List<Estrazione>();
            string[] ruote = { "BA", "CA", "FI", "GE", "MI", "NA", "PA", "RO", "TO", "VE", "NZ" };

            Random rnd = new Random();
            for (int i = 0; i < 5; i++) // 5 estrazioni
            {
                foreach (string ruota in ruote)
                {
                    //var estrazione = new Estrazione
                    //{
                    //    Data = DateTime.Now.AddDays(-i),
                    //    //Numeri = 1000 + i,
                    //    Ruota = ruota,
                    //    SeqAnno = i + 1,
                    //    Numeri = new List<int>
                    //    {
                    //        rnd.Next(1, 91),
                    //        rnd.Next(1, 91),
                    //        rnd.Next(1, 91),
                    //        rnd.Next(1, 91),
                    //        rnd.Next(1, 91)
                    //    }
                        
                    //};
                    //estrazioni.Add(estrazione);
                }
            }

            return estrazioni;
        }

        private void CaricaDatiGriglia()
        {
            if (GrigliaDestinazione == null) return;

            // Pulisci la griglia esistente
            GrigliaDestinazione.Rows.Clear();

            // Simula il caricamento dati basato sul range di date
            DateTime dataInizio = dateTimeInizio.Value;
            DateTime dataFine = dateTimeFine.Value;

            Random rnd = new Random();
            for (int i = 0; i < 15; i++)
            {
                object[] riga = new object[57];
                riga[0] = dataInizio.AddDays(i).ToString("dd/MM/yyyy");
                riga[1] = 1000 + i;

                for (int j = 2; j < 57; j++)
                {
                    riga[j] = rnd.Next(1, 91);
                }

                GrigliaDestinazione.Rows.Add(riga);
            }
        }
    }

    //public class Estrazione
    //{
    //    public DateTime DataEstrazione { get; set; }
    //    public int NumeroEstrazione { get; set; }
    //    public string Ruota { get; set; }
    //    public System.Collections.Generic.List<int> NumeriEstratti { get; set; }
    //}
}