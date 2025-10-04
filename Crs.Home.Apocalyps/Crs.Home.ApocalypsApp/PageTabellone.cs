using System;
using System.Drawing;
using System.Windows.Forms;

namespace Crs.Home.ApocalypsApp
{
    public partial class PageTabellone : UserControl
    {
        public PageTabellone()
        {
            InitializeComponent();
            InitializeGriglia();
            InitializePanelDestra();
        }

        private void InitializeGriglia()
        {
            // Aggiungi colonne con le nuove intestazioni
            InitializeColonneGriglia();

            // Aggiungi dati di esempio
            //AggiungiDatiEsempio();

            // Applica colorazione alternata alle colonne delle ruote
            ColoraColonneRuote();
        }

        private void InitializeColonneGriglia()
        {
            // Colonne base
            grigliaEstrazioni.Columns.Add("DataEstrazione", "Data Estrazione");
            DataGridViewColumn c = grigliaEstrazioni.Columns.GetLastColumn(DataGridViewElementStates.None, DataGridViewElementStates.None);
            c.Width = 100;
            grigliaEstrazioni.Columns.Add("NumeroEstrazione", "N° Estrazione");
            c = grigliaEstrazioni.Columns.GetLastColumn(DataGridViewElementStates.None, DataGridViewElementStates.None);
            c.Width = 60;

            // Definizione ruote con abbreviazioni
            var ruote = new[]
            {
                new { Nome = "Bari", Abbreviazione = "BA" },
                new { Nome = "Cagliari", Abbreviazione = "CA" },
                new { Nome = "Firenze", Abbreviazione = "FI" },
                new { Nome = "Genova", Abbreviazione = "GE" },
                new { Nome = "Milano", Abbreviazione = "MI" },
                new { Nome = "Napoli", Abbreviazione = "NA" },
                new { Nome = "Palermo", Abbreviazione = "PA" },
                new { Nome = "Roma", Abbreviazione = "RO" },
                new { Nome = "Torino", Abbreviazione = "TO" },
                new { Nome = "Venezia", Abbreviazione = "VE" },
                new { Nome = "Nazionale", Abbreviazione = "NZ" }
            };

            // Aggiungi colonne per ogni ruota (5 numeri per ruota)
            foreach (var ruota in ruote)
            {
                for (int i = 1; i <= 5; i++)
                {
                    string nomeColonna = $"{ruota.Abbreviazione}_{i}";
                    string headerText = $"{ruota.Abbreviazione}{i}";

                    grigliaEstrazioni.Columns.Add(nomeColonna, headerText);
                    c =grigliaEstrazioni.Columns.GetLastColumn(DataGridViewElementStates.None, DataGridViewElementStates.None);
                    c.Width = 40;
                }
            }
        }

        private void ColoraColonneRuote()
        {
            // Colori per le colonne (alternati)
            Color coloreChiaro = Color.LightGray;
            Color coloreBianco = Color.White;

            // Le prime 2 colonne sono dati base, le altre sono le ruote
            for (int i = 2; i < grigliaEstrazioni.Columns.Count; i++)
            {
                DataGridViewColumn colonna = grigliaEstrazioni.Columns[i];

                // Determina il colore in base al gruppo di 5 colonne (ogni ruota)
                int indiceRuota = (i - 2) / 5;
                if (indiceRuota % 2 == 0)
                {
                    colonna.DefaultCellStyle.BackColor = coloreChiaro;
                    colonna.HeaderCell.Style.BackColor = coloreChiaro;
                }
                else
                {
                    colonna.DefaultCellStyle.BackColor = coloreBianco;
                    colonna.HeaderCell.Style.BackColor = coloreBianco;
                }

                // Stile per tutte le celle delle ruote
                colonna.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                colonna.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                colonna.DefaultCellStyle.Font = new Font("Calibri", 10, FontStyle.Bold);
            }

            // Stile per le colonne base
            if (grigliaEstrazioni.Columns.Count >= 2)
            {
                grigliaEstrazioni.Columns[0].DefaultCellStyle.BackColor = Color.White;
                grigliaEstrazioni.Columns[1].DefaultCellStyle.BackColor = Color.White;
            }
        }

        private void AggiungiDatiEsempio()
        {
            // Aggiungi alcune righe di esempio
            Random rnd = new Random();
            for (int i = 0; i < 20; i++)
            {
                object[] riga = new object[57]; // 2 colonne base + 55 colonne numeri
                riga[0] = DateTime.Now.AddDays(-i).ToString("dd/MM/yyyy");
                riga[1] = 1000 + i;

                // Numeri casuali per l'esempio (da 1 a 90)
                for (int j = 2; j < 57; j++)
                {
                    riga[j] = rnd.Next(1, 91);
                }

                grigliaEstrazioni.Rows.Add(riga);
            }
        }

        private void InitializePanelDestra()
        {
            // Aggiungi pannelli colorati di esempio
            AggiungiPannelliColori();
        }

        private void AggiungiPannelliColori()
        {
            Color[] colors = { Color.LightCoral, Color.LightGreen, Color.LightSkyBlue,
                             Color.LightGoldenrodYellow, Color.LightPink, Color.LightSeaGreen };

            int yPos = 30;
            for (int i = 0; i < colors.Length; i++)
            {
                Panel pannelloColore = new Panel();
                pannelloColore.Size = new Size(150, 40);
                pannelloColore.Location = new Point(10, yPos);
                pannelloColore.BackColor = colors[i];
                pannelloColore.BorderStyle = BorderStyle.FixedSingle;
                pannelloColore.Tag = colors[i];

                Label lblPannello = new Label();
                lblPannello.Text = $"Pannello {i + 1}";
                lblPannello.Location = new Point(5, 12);
                lblPannello.AutoSize = true;
                lblPannello.Font = new Font("Arial", 8, FontStyle.Bold);

                pannelloColore.Controls.Add(lblPannello);
                panelDestra.Controls.Add(pannelloColore);

                yPos += 50;
            }
        }

        private void BtnFiltra_Click(object sender, EventArgs e)
        {
            // Logica per applicare i filtri
            MessageBox.Show("Filtri applicati!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}