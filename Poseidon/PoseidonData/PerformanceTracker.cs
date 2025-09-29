using LottoArmoniaModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoseidonData
{
    public class PerformanceTracker
    {
        private Dictionary<string, RegolaPerformance> performanceRegole;
        //private int estrazioniProcessate;
        //private const int INTERVALLO_CALIBRAZIONE = 30; // Ogni 30 estrazioni
        
        public PerformanceTracker()
        {
            performanceRegole = new Dictionary<string, RegolaPerformance>
            {
                {"Decina", new RegolaPerformance()},
                {"FiguraAntifigura", new RegolaPerformance()},
                {"Polarizzazione", new RegolaPerformance()},
                {"Ritardo", new RegolaPerformance()},
                {"Armonia", new RegolaPerformance()},
                {"DifferenzaRitardi", new RegolaPerformance()},
                {"AutoAttrazione9", new RegolaPerformance()},
                {"InterRuota", new RegolaPerformance()},
                {"Temporale", new RegolaPerformance()},
                {"Sequenza", new RegolaPerformance()},
                {"Fisica", new RegolaPerformance()}
            };
        }

        public void RegistraPerformance(string regola, bool successo)
        {
            if (performanceRegole.ContainsKey(regola))
            {
                var perf = performanceRegole[regola];
                if (successo) perf.AttivazioniSuccesso++;
                else perf.AttivazioniFallite++;


            }
        }

        public Dictionary<string, double> CalcolaAggiornamentiPesi()
        {
            var aggiornamenti = new Dictionary<string, double>();
            double vantaggioMedio = performanceRegole.Values
                .Where(p => p.TotaleAttivazioni > 10)
                .Average(p => p.VantaggioCorrente);

            foreach (var kvp in performanceRegole)
            {
                if (kvp.Value.TotaleAttivazioni < 5) continue;

                double rapporto = kvp.Value.VantaggioCorrente / vantaggioMedio;
                rapporto = Math.Max(0.7, Math.Min(1.3, rapporto)); // Limita oscillazioni

                aggiornamenti[kvp.Key] = rapporto;
            }

            // Reset contatori dopo calibrazione
            foreach (var perf in performanceRegole.Values)
            {
                perf.Reset();
            }

            return aggiornamenti;
        }
    }
}
