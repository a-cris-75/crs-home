using Crs.Home.ApocalypsData.DataEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crs.Home.ApocalypsData
{

    public class ModelloAdattivo2 : ModelloLotto
    {
        private Dictionary<string, RegolaPerformance> performanceRegole;
        private int estrazioniDaUltimoAggiornamento = 0;
        private const int INTERVALLO_AGGIORNAMENTO = 30;
        private Queue<Tuple<string, DateTime, string, int, bool>> codaPerformance;

        public ModelloAdattivo2(List<Estrazione> estrazioniStoriche, ParametriModello parametri = null)
            : base(estrazioniStoriche, parametri)
        {
            InizializzaPerformanceRegole();
            codaPerformance = new Queue<Tuple<string, DateTime, string, int, bool>>();
        }

        private void InizializzaPerformanceRegole()
        {
            performanceRegole = new Dictionary<string, RegolaPerformance>
        {
            { "Decina", new RegolaPerformance { NomeRegola = "Decina", UltimoReset = DateTime.Now } },
            { "FiguraAntifigura", new RegolaPerformance { NomeRegola = "FiguraAntifigura", UltimoReset = DateTime.Now } },
            { "Polarizzazione", new RegolaPerformance { NomeRegola = "Polarizzazione", UltimoReset = DateTime.Now } },
            { "Ritardo", new RegolaPerformance { NomeRegola = "Ritardo", UltimoReset = DateTime.Now } },
            { "Armonia", new RegolaPerformance { NomeRegola = "Armonia", UltimoReset = DateTime.Now } },
            { "DifferenzaRitardi", new RegolaPerformance { NomeRegola = "DifferenzaRitardi", UltimoReset = DateTime.Now } },
            { "AutoAttrazione9", new RegolaPerformance { NomeRegola = "AutoAttrazione9", UltimoReset = DateTime.Now } },
            { "InterRuota", new RegolaPerformance { NomeRegola = "InterRuota", UltimoReset = DateTime.Now } },
            { "Temporale", new RegolaPerformance { NomeRegola = "Temporale", UltimoReset = DateTime.Now } },
            { "Sequenza", new RegolaPerformance { NomeRegola = "Sequenza", UltimoReset = DateTime.Now } }
        };
        }

        public List<int> PrevisioneConTracking(string ruota, DateTime dataTarget, List<int> numeriUsciti = null)
        {
            var scores = new List<Tuple<int, int, List<string>>>();

            for (int num = 1; num <= 90; num++)
            {
                List<string> regoleAttivate = new List<string>();
                var result = CalcolaBonusConRegole(num, ruota, dataTarget);
                //var result = CalcolaBonus(num, ruota, dataTarget);
                scores.Add(Tuple.Create(num, result.Item1, result.Item2));
            }

            var consigliati = scores.Where(x => x.Item2 >= param.Soglia).Select(x => x.Item1).ToList();

            // Registra performance dopo l'estrazione (se abbiamo i risultati reali)
            if (numeriUsciti != null)
            {
                RegistraPerformance(ruota, dataTarget, scores, numeriUsciti);
            }

            return consigliati.OrderBy(x => x).ToList();
        }

        private void RegistraPerformance(string ruota, DateTime data, List<Tuple<int, int, List<string>>> scores, List<int> numeriUsciti)
        {
            foreach (var score in scores)
            {
                int numero = score.Item1;
                bool uscito = numeriUsciti.Contains(numero);
                var regoleAttivate = score.Item3;

                foreach (string regola in regoleAttivate)
                {
                    if (performanceRegole.ContainsKey(regola))
                    {
                        var perf = performanceRegole[regola];
                        if (uscito)
                            perf.AttivazioniSuccesso++;
                        else
                            perf.AttivazioniFallite++;
                    }
                }
            }

            estrazioniDaUltimoAggiornamento++;
            if (estrazioniDaUltimoAggiornamento >= INTERVALLO_AGGIORNAMENTO)
            {
                AggiornaPesi();
                estrazioniDaUltimoAggiornamento = 0;
            }
        }

        private void AggiornaPesi()
        {
            Console.WriteLine("=== AGGIORNAMENTO PESI MODELLO ===");

            double vantaggioMedio = performanceRegole.Values
                .Where(p => (p.AttivazioniSuccesso + p.AttivazioniFallite) > 10)
                .Average(p => p.VantaggioCorrente);

            foreach (var kvp in performanceRegole)
            {
                var perf = kvp.Value;
                int totaleAttivazioni = perf.AttivazioniSuccesso + perf.AttivazioniFallite;

                if (totaleAttivazioni < 5) continue; // Troppo pochi dati

                double rapporto = perf.VantaggioCorrente / vantaggioMedio;
                rapporto = Math.Max(0.5, Math.Min(2.0, rapporto)); // Limita oscillazioni

                double nuovoPeso = OttieniPesoCorrente(kvp.Key) * rapporto;
                AggiornaParametro(kvp.Key, nuovoPeso);

                Console.WriteLine($"{kvp.Key}: Vantaggio {perf.VantaggioCorrente:F2}x, Peso {nuovoPeso:F1} (x{rapporto:F2})");

                // Reset contatori
                //perf.AttivazioniSuccesso = 0;
                //perf.AttivazioniFallite = 0;
                //perf.UltimoReset = DateTime.Now;
                perf.Reset();
            }
        }

        private double OttieniPesoCorrente(string regola)
        {
            switch (regola)
            {
                case "Decina": return param.W_dec1; // Usiamo W_dec1 come riferimento
                case "FiguraAntifigura": return param.W_FA;
                case "Polarizzazione": return param.W_pol;
                case "Ritardo": return param.W_rit15;
                case "Armonia": return param.W_arm;
                case "DifferenzaRitardi": return param.W_diff;
                case "AutoAttrazione9": return 20; // Peso fisso per B_9
                case "InterRuota": return 15; // Peso fisso per B_ir
                case "Temporale": return 12; // Peso fisso per B_temp
                case "Sequenza": return 15; // Peso fisso per B_seq
                default: return 10;
            }
        }

        private void AggiornaParametro(string regola, double nuovoPeso)
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


}
