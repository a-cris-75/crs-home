
using Crs.Home.ApocalypsData.DataEntities;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Crs.Home.ApocalypsData
{
     
    public class ModelloAdattivo : ModelloLotto
    {
        private PerformanceTracker performanceTracker;
        private int estrazioniDaUltimaCalibrazione;
        private const int INTERVALLO_CALIBRAZIONE = 30;

        public ModelloAdattivo(List<Estrazione> estrazioniStoriche, ParametriModello parametri = null)
            : base(estrazioniStoriche, parametri)
        {
            this.performanceTracker = new PerformanceTracker();
            this.estrazioniDaUltimaCalibrazione = 0;
        }

        // Metodo specifico per adattività
        public List<int> PrevisioneConAdattivita(string ruota, DateTime dataTarget, List<int> numeriUsciti, out List<Tuple<int, int, List<string>>> risultati)
        {
            // num, bonus, regoleAttivate
            risultati = new List<Tuple<int, int, List<string>>>();

            // Ottieni le ultime estrazioni per escludere numeri recenti
            var ultimeEstrazioni = GetUltimeEstrazioni(ruota, dataTarget, 3);
            var numeriRecenti = ultimeEstrazioni.SelectMany(e => e.Numeri).Distinct().ToList();

            for (int num = 1; num <= 90; num++)
            {
                // ⚠️ ESCLUDI NUMERI USCITI DI RECENTE
                if (numeriRecenti.Contains(num))
                    continue;
                //List<string> regoleAttivate = new List<string>();
                Tuple<int, List<string>> bonus = CalcolaBonusConRegole(num, ruota, dataTarget);//, regoleAttivate);
                risultati.Add(Tuple.Create(num, bonus.Item1, bonus.Item2));
            }
            int sogliaDinamica = SogliaAdattiva(risultati.Select(x => Tuple.Create(x.Item1, x.Item2)).ToList());
            var consigliati = risultati.Where(x => x.Item2 >= sogliaDinamica).ToList();
            //var consigliati = risultati.Where(x => x.Item2 >= 50).ToList();

            // se i numeri scelti sono troppi non considerarli: capita se sono all'inizio dell'analisi e la soglia dinamica è bassa
            
            // Fallback se troppo pochi numeri
            if (consigliati.Count < 3)
            {
                consigliati = risultati
                    .OrderByDescending(x => x.Item2)
                    .Take(5)
                    .ToList();
            }
            else
            if (consigliati.Count > 10)
            {
                consigliati = risultati
                    .OrderByDescending(x => x.Item2)
                    .Take(5)
                    .ToList();
            }


            if (numeriUsciti != null)
            {
                AggiornaPerformance(risultati, numeriUsciti);
                //var risultati1 = risultati.OrderBy(X => X.Item2).TakeLast(consigliati.Count).ToList();
                var risultati1 = risultati.Where(X => consigliati.Contains(X)).ToList();
                risultati1.AddRange(risultati.Where(X => X.Item1 == numeriUsciti[0] ||
                                            X.Item1 == numeriUsciti[1] ||
                                            X.Item1 == numeriUsciti[2] ||
                                            X.Item1 == numeriUsciti[3] ||
                                            X.Item1 == numeriUsciti[4]));
                risultati = risultati1.DistinctBy(X=>X.Item1).ToList();
            }
            else
                risultati = risultati.OrderBy(X => X.Item2).TakeLast(consigliati.Count).ToList();

            return consigliati.Select(x => x.Item1).ToList(); 
        }

        // Calcola dinamicamente la soglia basata sulla distribuzione
        public int SogliaAdattiva(List<Tuple<int, int>> scores)
        {
            var punteggi = scores.Select(x => x.Item2).OrderByDescending(x => x).ToList();
            // Prendi il top 8% dei punteggi
            int index = (int)(punteggi.Count * 0.08);
            return Math.Max(50, punteggi[index]);
        }
        private void AggiornaPerformance(List<Tuple<int, int, List<string>>> risultati, List<int> numeriUsciti)
        {
            foreach (var risultato in risultati)
            {
                bool uscito = numeriUsciti.Contains(risultato.Item1);
                foreach (string regola in risultato.Item3)
                {
                    performanceTracker.RegistraPerformance(regola, uscito);
                }
            }

            estrazioniDaUltimaCalibrazione++;
            if (estrazioniDaUltimaCalibrazione >= INTERVALLO_CALIBRAZIONE)
            {
                CalibraPesi();
                estrazioniDaUltimaCalibrazione = 0;
            }
        }

        private void CalibraPesi()
        {
            var aggiornamenti = performanceTracker.CalcolaAggiornamentiPesi();

            foreach (var agg in aggiornamenti)
            {
                double nuovoPeso = OttieniPesoCorrente(agg.Key) * agg.Value;
                ApplicaAggiornamento(agg.Key, nuovoPeso);
            }

            Console.WriteLine("=== PESI AGGIORNATI ===");
            foreach (var agg in aggiornamenti)
            {
                Console.WriteLine($"{agg.Key}: x{agg.Value:F2}");
            }
        }

        // METODI MANCANTI - AGGIUNGI QUESTI:
        private double OttieniPesoCorrente(string regola)
        {
            switch (regola)
            {
                case "Decina": return param.W_dec1;
                case "FiguraAntifigura": return param.W_FA;
                case "Polarizzazione": return param.W_pol;
                case "Ritardo": return param.W_rit15;
                case "Armonia": return param.W_arm;
                case "DifferenzaRitardi": return param.W_diff;
                case "AutoAttrazione9": return 20;
                case "InterRuota": return 15;
                case "Temporale": return 12;
                case "Sequenza": return 15;
                case "Fisica": return 15;
                default: return 10;
            }
        }

        private void ApplicaAggiornamento(string regola, double nuovoPeso)
        {
            int pesoInt = (int)Math.Round(nuovoPeso);

            switch (regola)
            {
                case "Decina":
                    param.W_dec1 = Math.Max(5, Math.Min(40, pesoInt));
                    param.W_dec2 = param.W_dec1 / 2;
                    break;
                case "FiguraAntifigura":
                    param.W_FA = Math.Max(10, Math.Min(50, pesoInt));
                    break;
                case "Polarizzazione":
                    param.W_pol = Math.Max(2, Math.Min(15, pesoInt));
                    break;
                case "Ritardo":
                    param.W_rit15 = Math.Max(5, Math.Min(30, pesoInt));
                    param.W_rit20 = (int)(param.W_rit15 * 1.67);
                    param.W_rit25 = (int)(param.W_rit15 * 2.33);
                    break;
                // ... altri casi ...
                case "Armonia":
                    param.W_arm = Math.Max(2, Math.Min(10, pesoInt));
                    break;
                case "DifferenzaRitardi":
                    param.W_diff = Math.Max(1, Math.Min(5, pesoInt));
                    break;
                    // Per regole con pesi fissi, aggiorniamo i valori fissi
            }
        }
    }

    public class RegolaPerformance
    {
        public string NomeRegola { get; set; }
        public int AttivazioniSuccesso { get; set; }
        public int AttivazioniFallite { get; set; }
        public double VantaggioCorrente
        {
            get
            {
                double totale = AttivazioniSuccesso + AttivazioniFallite;
                if (totale == 0) return 1.0;
                double successRate = AttivazioniSuccesso / totale;
                return successRate / 0.0556; // Vantaggio vs caso casuale
            }
        }
        public DateTime UltimoReset { get; set; }
        public int TotaleAttivazioni { get { return AttivazioniSuccesso + AttivazioniFallite; } }

        public void Reset() 
        {
            AttivazioniSuccesso = 0;
            AttivazioniFallite = 0;        
            UltimoReset = DateTime.Now;
        }
    }

    // Caricatore CSV (invariato)
    public static class CaricatoreEstrazioni
    {
        public static List<Estrazione> CaricaDaCSV(string filePath, string configfields)
        {
            var estrazioni = new List<Estrazione>();
            var culture = CultureInfo.GetCultureInfo("it-IT");

            try
            {
                bool b = DbDataAccess.GetSeqFields(configfields, out int seqdt, out int seqruota, out int seqanno, out int seqnum1);
                var lines = File.ReadAllLines(filePath);
                int idx = 0;
                foreach (var line in lines.Skip(1))
                {
                    var parts = line.Split(';');
                    if (parts.Length < 3) continue;

                    if (DateTime.TryParseExact(parts[seqdt], "dd/MM/yyyy", culture, DateTimeStyles.None, out DateTime data))
                    {
                        string ruota = parts[seqruota];
                        var numeri = parts[seqnum1].Split(',').Select(int.Parse).ToList();
                        estrazioni.Add(new Estrazione(data, ruota, idx++, numeri));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore caricamento CSV: {ex.Message}");
            }

            return estrazioni;
        }
    }

    public class StatoQuantistico
    {
        public Dictionary<int, double> Ampiezze { get; set; } // numero -> ampiezza probabilità
        public int Collassa() => Ampiezze.OrderByDescending(x => x.Value).First().Key;

        // NUOVO METODO: evoluzione temporale dello stato
        public void Evolvi(DateTime nuovoTempo)
        {
            foreach (var num in Ampiezze.Keys.ToList())
            {
                double nuovaAmpiezza = new OndaNumerica().Ampiezza(num, nuovoTempo);
                Ampiezze[num] = (Ampiezze[num] + nuovaAmpiezza) / 2.0; // Media con evoluzione
            }
        }
    }

    
    public class RilevatoreCoppieSegnale
    {
        public static bool IsCoppiaSegnale(int num1, int num2)
        {
            int fig1 = ModelloLotto.Figura(num1);
            int fig2 = ModelloLotto.Figura(num2);
            int dec1 = ModelloLotto.Decina(num1);
            int dec2 = ModelloLotto.Decina(num2);
            int distanza = Math.Abs(num1 - num2);
            int figDistanza = ModelloLotto.Figura(distanza);

            // 🎵 LEGGE ARMONICA DELLE COPPIE SEGNALE
            bool condizioneFigure = (fig1 + fig2) is 4 or 5 or 7 or 9;
            bool condizioneDecine = Math.Abs(dec1 - dec2) is 10 or 20 or 30 or 40 or 60;
            bool condizioneDistanza = figDistanza is 4 or 8 or 9;

            return condizioneFigure && condizioneDecine && condizioneDistanza;
        }

        public static List<int> GetNumeriPredetti(List<int> coppiaSegnale, string ruota, DateTime dataUscita)
        {
            // 🔍 Cerca pattern storici per questa coppia
            var numeriPredetti = new List<int>();

            // Pattern universali identificati
            var patternUniversali = new Dictionary<List<int>, List<int>>
            {
                { new List<int> {23, 67}, new List<int> {11, 34, 58} },
                { new List<int> {18, 54}, new List<int> {9, 27, 63} },
                { new List<int> {32, 45}, new List<int> {70, 71, 72, 73, 18, 19, 20, 21} },
                { new List<int> {11, 77}, new List<int> {22, 44, 66} },
                { new List<int> {29, 83}, new List<int> {15, 37, 59} }
            };

            // Cerca pattern specifico
            var coppiaOrdinata = coppiaSegnale.OrderBy(x => x).ToList();
            if (patternUniversali.ContainsKey(coppiaOrdinata))
            {
                numeriPredetti.AddRange(patternUniversali[coppiaOrdinata]);
            }

            // Pattern generico basato su figure
            int fig1 = ModelloLotto.Figura(coppiaSegnale[0]);
            int fig2 = ModelloLotto.Figura(coppiaSegnale[1]);
            int sommaFigure = fig1 + fig2;

            // Aggiungi numeri basati sulla somma delle figure
            switch (sommaFigure)
            {
                case 4: numeriPredetti.AddRange(new[] { 13, 31, 49, 67, 85 }); break;
                case 5: numeriPredetti.AddRange(new[] { 14, 23, 32, 41, 50 }); break;
                case 7: numeriPredetti.AddRange(new[] { 16, 25, 34, 43, 52 }); break;
                case 9: numeriPredetti.AddRange(new[] { 18, 27, 36, 45, 54, 63, 72, 81, 90 }); break;
            }

            return numeriPredetti.Distinct().ToList();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var estrazioni = CaricatoreEstrazioni.CaricaDaCSV("estrazioni.csv", "data;ruota;numeri");

            if (estrazioni.Count == 0)
            {
                Console.WriteLine("Nessuna estrazione caricata. Verifica il file.");
                return;
            }

            //var modello = new ModelloLotto(estrazioni);
            DateTime dataPrevisione = new DateTime(2009, 6, 13);

            //foreach (string ruota in new[] { "Roma", "Bari", "Napoli" }) // Test su 3 ruote
            //{
            //    var consigli = modello.Previsione(ruota, dataPrevisione);
            //    var punteggi = modello.GetPunteggiCompleti(ruota, dataPrevisione);

            //    Console.WriteLine($"{ruota}: {consigli.Count} numeri");
            //    Console.WriteLine($"  Numeri: {string.Join(", ", consigli)}");
            //    Console.WriteLine($"  Punteggi: {string.Join(", ", punteggi.Where(x => x.Value >= 40).OrderByDescending(x => x.Value).Select(x => $"{x.Key}({x.Value})"))}");
            //    Console.WriteLine();
            //}

            var modelloAdattivo = new ModelloAdattivo(estrazioni);
            //dataPrevisione = new DateTime(2009, 6, 13);

            foreach (string ruota in new[] { "Roma", "Bari", "Napoli" }) // Test su 3 ruote
            {
                var risultatiReali = new List<int> { 7, 38, 39, 65, 90 };
                var consigli = modelloAdattivo.PrevisioneConAdattivita(ruota, dataPrevisione, risultatiReali, out List<Tuple<int,int,List<string>>> risBonusRegole);
                //var punteggi = modelloAdattivo.GetPunteggiCompleti(ruota, dataPrevisione);

                Console.WriteLine($"{ruota}: {consigli.Count} numeri");
                Console.WriteLine($"  Numeri: {string.Join(", ", consigli)}");
                Console.WriteLine($"  Punteggi: {string.Join(", ", risBonusRegole.Where(x => x.Item2 >= 40).OrderByDescending(x => x.Item2).Select(x => $"{x.Item1}({x.Item2})"))}");
                Console.WriteLine();
            }
        }
    }
}
