using Crs.Home.ApocalypsData.DataEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crs.Home.ApocalypsData
{
    public class ModelloParametriOscillanti : ModelloLotto
    {
        public ModelloParametriOscillanti(List<Estrazione> estrazioniStoriche, ParametriModello parametri = null)
           : base(estrazioniStoriche, parametri)
        {
            //this.performanceTracker = new PerformanceTracker();
            //this.estrazioniDaUltimaCalibrazione = 0;
        }

        public List<int> PrevisioneConParametriOscillanti(string ruota, DateTime dataTarget,
            List<int> numeriUsciti, out List<Tuple<int, int, List<string>>> risultati,
            int maxNumDaPrevedere = 8,
            List<string> regoleDaConsiderare = null)  // ⚠️ NUOVO PARAMETRO OPZIONALE
        {
            // 🎵 CREA PARAMETRI SPECIFICI PER QUESTA DATA
            var parametriOscillanti = new ParametriOscillanti(dataTarget);

            Console.WriteLine($"🎵 Parametri per {dataTarget:dd/MM/yyyy}:");
            Console.WriteLine($"   W_dec1: {parametriOscillanti.W_dec1}, W_FA: {parametriOscillanti.W_FA}, " +
                              $"W_rit25: {parametriOscillanti.W_rit25}, Soglia: {parametriOscillanti.Soglia}");

            risultati = new List<Tuple<int, int, List<string>>>();

            // ⚠️ SE NULL, USA TUTTE LE REGOLE (COMPATIBILITÀ BACKWARD)
            if (regoleDaConsiderare == null)
            {
                regoleDaConsiderare = new List<string>
                {
                    "Decina", "FiguraAntifigura", "Polarizzazione", "Ritardo",
                    "Armonia", "DifferenzaRitardi", "AutoAttrazione9", "InterRuota",
                    "Temporale", "Sequenza", "CoppieSegnale", "Fisica"
                };
            }

            for (int num = 1; num <= 90; num++)
            {
                // Escludi numeri recenti
                var ultimeEstrazioni = GetUltimeEstrazioni(ruota, dataTarget, 3);
                var numeriRecenti = ultimeEstrazioni.SelectMany(e => e.Numeri).ToList();
                if (numeriRecenti.Contains(num)) continue;

                // ⚠️ PASSA regoleDaConsiderare AL METODO DI CALCOLO
                var bonus = CalcolaBonusConRegoleOscillanti(num, ruota, dataTarget,
                            parametriOscillanti, regoleDaConsiderare);
                risultati.Add(Tuple.Create(num, bonus.Item1, bonus.Item2));
            }

            var consigliati = risultati
                .Where(x => x.Item2 >= parametriOscillanti.Soglia)
                .OrderByDescending(x => x.Item2)
                .ToList();

            // Fallback se troppo pochi numeri
            if (consigliati.Count < 3)
            {
                consigliati = risultati
                    .OrderByDescending(x => x.Item2)
                    .Take(Math.Max(3, consigliati.Count))
                    .ToList();
            }
            else if (consigliati.Count > maxNumDaPrevedere)
            {
                consigliati = risultati
                    .OrderByDescending(x => x.Item2)
                    .Take(maxNumDaPrevedere)
                    .ToList();
            }

            if (numeriUsciti != null)
            {
                var risultati1 = risultati.Where(X => consigliati.Contains(X)).ToList();
                risultati1.AddRange(risultati.Where(X => X.Item1 == numeriUsciti[0] ||
                                            X.Item1 == numeriUsciti[1] ||
                                            X.Item1 == numeriUsciti[2] ||
                                            X.Item1 == numeriUsciti[3] ||
                                            X.Item1 == numeriUsciti[4]));
                risultati = risultati1.DistinctBy(X => X.Item1).ToList();
            }
            else
            {
                risultati = risultati.OrderByDescending(X => X.Item2).Take(consigliati.Count).ToList();
            }

            Console.WriteLine($"   Numeri consigliati: {string.Join(", ", consigliati.Select(x => x.Item1))}");
            Console.WriteLine($"   Regole attive: {string.Join(", ", regoleDaConsiderare)}"); // ⚠️ NUOVO LOG
            return consigliati.Select(x => x.Item1).ToList();
        }

        private Tuple<int, List<string>> CalcolaBonusConRegoleOscillanti(int numero, string ruota,
                DateTime dataTarget, ParametriOscillanti parametri, List<string> regoleDaConsiderare)
        {
            var regoleAttivate = new List<string>();

            // 🎯 CALCOLA SOLO LE REGOLE SELEZIONATE
            int B_dec = regoleDaConsiderare.Contains("Decina") ?
                CalcolaBonusDecinaOscillante(numero, ruota, dataTarget, parametri) : 0;

            int B_FA = regoleDaConsiderare.Contains("FiguraAntifigura") ?
                CalcolaBonusFAOscillante(numero, ruota, dataTarget, parametri) : 0;

            int B_pol = regoleDaConsiderare.Contains("Polarizzazione") ?
                CalcolaBonusPolarizzazioneOscillante(numero, ruota, dataTarget, parametri) : 0;

            int B_rit = regoleDaConsiderare.Contains("Ritardo") ?
                CalcolaBonusRitardoOscillante(numero, ruota, dataTarget, parametri) : 0;

            int B_arm = regoleDaConsiderare.Contains("Armonia") ?
                CalcolaBonusArmoniaOscillante(numero, ruota, dataTarget, parametri) : 0;

            int B_diff = regoleDaConsiderare.Contains("DifferenzaRitardi") ?
                CalcolaBonusDifferenzaRitardiOscillante(numero, ruota, dataTarget, parametri) : 0;

            int B_9 = regoleDaConsiderare.Contains("AutoAttrazione9") ?
                CalcolaBonusAutoAttrazione9Oscillante(numero, ruota, dataTarget, parametri) : 0;

            int B_ir = regoleDaConsiderare.Contains("InterRuota") ?
                CalcolaBonusInterRuotaOscillante(numero, ruota, dataTarget, parametri) : 0;

            int B_temp = regoleDaConsiderare.Contains("Temporale") ?
                CalcolaBonusTemporaleOscillante(numero, ruota, dataTarget, parametri) : 0;

            int B_seq = regoleDaConsiderare.Contains("Sequenza") ?
                CalcolaBonusSequenzaOscillante(numero, ruota, dataTarget, parametri) : 0;

            int B_coppie = regoleDaConsiderare.Contains("CoppieSegnale") ?
                CalcolaBonusCoppieSegnale(numero, ruota, dataTarget) : 0;

            int B_fisica = regoleDaConsiderare.Contains("Fisica") ?
                CalcolaBonusFisicaCompleto(numero, ruota, dataTarget) : 0;

            // 🎯 REGISTRA SOLO REGOLE ATTIVATE E SELEZIONATE
            if (B_dec > 0 && regoleDaConsiderare.Contains("Decina"))
                regoleAttivate.Add("Decina");
            if (B_FA > 0 && regoleDaConsiderare.Contains("FiguraAntifigura"))
                regoleAttivate.Add("FiguraAntifigura");
            if (B_pol > 0 && regoleDaConsiderare.Contains("Polarizzazione"))
                regoleAttivate.Add("Polarizzazione");
            if (B_rit > 0 && regoleDaConsiderare.Contains("Ritardo"))
                regoleAttivate.Add("Ritardo");
            if (B_arm > 0 && regoleDaConsiderare.Contains("Armonia"))
                regoleAttivate.Add("Armonia");
            if (B_diff > 0 && regoleDaConsiderare.Contains("DifferenzaRitardi"))
                regoleAttivate.Add("DifferenzaRitardi");
            if (B_9 > 0 && regoleDaConsiderare.Contains("AutoAttrazione9"))
                regoleAttivate.Add("AutoAttrazione9");
            if (B_ir > 0 && regoleDaConsiderare.Contains("InterRuota"))
                regoleAttivate.Add("InterRuota");
            if (B_temp > 0 && regoleDaConsiderare.Contains("Temporale"))
                regoleAttivate.Add("Temporale");
            if (B_seq > 0 && regoleDaConsiderare.Contains("Sequenza"))
                regoleAttivate.Add("Sequenza");
            if (B_coppie > 0 && regoleDaConsiderare.Contains("CoppieSegnale"))
                regoleAttivate.Add("CoppieSegnale");
            if (B_fisica > 0 && regoleDaConsiderare.Contains("Fisica"))
                regoleAttivate.Add("Fisica");

            // 🎯 CALCOLO MOLTIPLICATIVO SOLO CON REGOLE ATTIVE
            double bonus_moltiplicativo = 1.0;

            if (regoleDaConsiderare.Contains("Decina")) bonus_moltiplicativo *= (B_dec / 10.0 + 1);
            if (regoleDaConsiderare.Contains("FiguraAntifigura")) bonus_moltiplicativo *= (B_FA / 10.0 + 1);
            if (regoleDaConsiderare.Contains("Polarizzazione")) bonus_moltiplicativo *= (B_pol / 10.0 + 1);
            if (regoleDaConsiderare.Contains("Ritardo")) bonus_moltiplicativo *= (B_rit / 10.0 + 1);
            if (regoleDaConsiderare.Contains("Armonia")) bonus_moltiplicativo *= (B_arm / 10.0 + 1);
            if (regoleDaConsiderare.Contains("DifferenzaRitardi")) bonus_moltiplicativo *= (B_diff / 10.0 + 1);
            if (regoleDaConsiderare.Contains("AutoAttrazione9")) bonus_moltiplicativo *= (B_9 / 10.0 + 1);
            if (regoleDaConsiderare.Contains("InterRuota")) bonus_moltiplicativo *= (B_ir / 10.0 + 1);
            if (regoleDaConsiderare.Contains("Temporale")) bonus_moltiplicativo *= (B_temp / 10.0 + 1);
            if (regoleDaConsiderare.Contains("Sequenza")) bonus_moltiplicativo *= (B_seq / 10.0 + 1);
            if (regoleDaConsiderare.Contains("CoppieSegnale")) bonus_moltiplicativo *= (B_coppie / 10.0 + 1);
            if (regoleDaConsiderare.Contains("Fisica")) bonus_moltiplicativo *= (B_fisica / 10.0 + 1);

            bonus_moltiplicativo *= 10;

            return Tuple.Create((int)Math.Round(bonus_moltiplicativo), regoleAttivate);
        }

        private int CalcolaBonusDecinaOscillante(int numero, string ruota, DateTime dataTarget, ParametriOscillanti parametri)
        {
            int B_dec = 0;
            var estrRuota = GetUltimeEstrazioni(ruota, dataTarget, 2);
            var decineUltime2 = new HashSet<int>();

            foreach (var e in estrRuota)
            {
                foreach (int n in e.Numeri)
                {
                    decineUltime2.Add(Decina(n));
                }
            }

            int dNum = Decina(numero);
            if (decineUltime2.Contains(dNum))
            {
                if (estrRuota.Count > 0 && estrRuota[0].Numeri.Any(n => Decina(n) == dNum))
                    B_dec += parametri.W_dec1;
                if (estrRuota.Count > 1 && estrRuota[1].Numeri.Any(n => Decina(n) == dNum))
                    B_dec += parametri.W_dec2;
            }
            return B_dec;
        }

        private int CalcolaBonusFAOscillante(int numero, string ruota, DateTime dataTarget, ParametriOscillanti parametri)
        {
            int B_FA = 0;
            int fNum = Figura(numero);
            if (fNum == 9) return 0;

            var estrRuota = GetUltimeEstrazioni(ruota, dataTarget, 3);

            for (int i = 0; i < estrRuota.Count; i++)
            {
                foreach (int n in estrRuota[i].Numeri)
                {
                    int fN = Figura(n);
                    if (fN == 9) continue;
                    var coppia = Tuple.Create(fN, fNum);
                    if (coppieFA.Contains(coppia))
                    {
                        if (i == 0) B_FA += parametri.W_FA;
                        else if (i == 1) B_FA += (int)(parametri.W_FA * 0.5);
                        else if (i == 2) B_FA += (int)(parametri.W_FA * 0.33);
                        break;
                    }
                }
            }
            return B_FA;
        }

        // 🎵 AGGIUNGI METODI SIMILI PER GLI ALTRI BONUS...
        private int CalcolaBonusPolarizzazioneOscillante(int numero, string ruota, DateTime dataTarget, ParametriOscillanti parametri)
        {
            int B_pol = 0;
            var estrRuota = GetUltimeEstrazioni(ruota, dataTarget, 1);
            if (estrRuota.Count == 0) return 0;

            var ultimaCinquina = estrRuota[0].Numeri;
            var figureUltima = new HashSet<int>(ultimaCinquina.Select(Figura));
            int varieta = figureUltima.Count;
            int fNum = Figura(numero);

            if (varieta == 5) // Alta varietà → bonus figure già usate
            {
                var figureUltime3 = new HashSet<int>();
                foreach (var e in GetUltimeEstrazioni(ruota, dataTarget, 3))
                {
                    figureUltime3.UnionWith(e.Numeri.Select(Figura));
                }
                if (figureUltime3.Contains(fNum))
                    B_pol += parametri.W_pol;
            }
            else if (varieta <= 3) // Bassa varietà → bonus figure nuove
            {
                if (!figureUltima.Contains(fNum))
                    B_pol += parametri.W_pol;
            }
            return B_pol;
        }

        private int CalcolaBonusRitardoOscillante(int numero, string ruota, DateTime dataTarget, ParametriOscillanti parametri)
        {
            int B_rit = 0;
            var ritardi = CalcolaRitardiFigure(ruota, dataTarget);
            int fNum = Figura(numero);
            int ritardoF = ritardi.ContainsKey(fNum) ? ritardi[fNum] : 1000;

            if (ritardoF >= 25) B_rit += parametri.W_rit25;
            else if (ritardoF >= 20) B_rit += parametri.W_rit20;
            else if (ritardoF >= 15) B_rit += parametri.W_rit15;

            return B_rit;
        }

        private int CalcolaBonusArmoniaOscillante(int numero, string ruota, DateTime dataTarget, ParametriOscillanti parametri)
        {
            int B_arm = 0;
            var figureUltime3 = GetUltimeEstrazioni(ruota, dataTarget, 3)
                .SelectMany(e => e.Numeri.Select(Figura));
            int fNum = Figura(numero);

            foreach (int fAltra in figureUltime3)
            {
                if (fAltra + fNum == 9 || fAltra + fNum == 10)
                {
                    B_arm += parametri.W_arm;
                    break;
                }
            }
            return B_arm;
        }

        private int CalcolaBonusDifferenzaRitardiOscillante(int numero, string ruota, DateTime dataTarget, ParametriOscillanti parametri)
        {
            int B_diff = 0;
            var ritardi = CalcolaRitardiFigure(ruota, dataTarget);
            int fNum = Figura(numero);
            int afNum = Antifigura(numero);

            if (coppieFA.Contains(Tuple.Create(fNum, afNum)))
            {
                int ritardoF = ritardi.ContainsKey(fNum) ? ritardi[fNum] : 1000;
                int ritardoAF = ritardi.ContainsKey(afNum) ? ritardi[afNum] : 1000;
                int diff = Math.Abs(ritardoF - ritardoAF);
                if (diff > parametri.Soglia_diff)
                {
                    B_diff += (diff - parametri.Soglia_diff) * parametri.W_diff;
                }
            }
            return B_diff;
        }

        private int CalcolaBonusAutoAttrazione9Oscillante(int numero, string ruota, DateTime dataTarget, ParametriOscillanti parametri)
        {
            int B_9 = 0;
            int fNum = Figura(numero);
            if (fNum != 9) return 0;

            var estrRuota = GetUltimeEstrazioni(ruota, dataTarget, 3);
            for (int i = 0; i < estrRuota.Count; i++)
            {
                if (estrRuota[i].Numeri.Any(n => Figura(n) == 9))
                {
                    if (i == 0) B_9 += 20; // Fisso per coerenza
                    else if (i == 1) B_9 += 10;
                    else if (i == 2) B_9 += 5;
                    break;
                }
            }
            return B_9;
        }

        private int CalcolaBonusInterRuotaOscillante(int numero, string ruota, DateTime dataTarget, ParametriOscillanti parametri)
        {
            int B_ir = 0;
            foreach (string altraRuota in ruote)
            {
                if (altraRuota == ruota) continue;

                var estrAltraRuota = GetUltimeEstrazioni(altraRuota, dataTarget, 3);
                foreach (var e in estrAltraRuota)
                {
                    if (e.Numeri.Contains(numero))
                    {
                        if (CoppiaForti(ruota, altraRuota, true))
                            B_ir += 15; // Fisso
                        else
                            B_ir += 10; // Fisso
                        break;
                    }
                }
            }
            return B_ir;
        }

        private int CalcolaBonusTemporaleOscillante(int numero, string ruota, DateTime dataTarget, ParametriOscillanti parametri)
        {
            int B_temp = 0;
            DayOfWeek giornoTarget = dataTarget.DayOfWeek;
            int fNum = Figura(numero);

            var patternTemporali = new Dictionary<Tuple<string, DayOfWeek, int>, int>
    {
        { Tuple.Create("Roma", DayOfWeek.Tuesday, 8), 12 },
        { Tuple.Create("Napoli", DayOfWeek.Friday, 3), 12 },
        { Tuple.Create("Palermo", DayOfWeek.Saturday, 9), 12 },
        { Tuple.Create("Milano", DayOfWeek.Monday, 5), 10 },
        { Tuple.Create("Bari", DayOfWeek.Thursday, 1), 8 }
    };

            var key = Tuple.Create(ruota, giornoTarget, fNum);
            if (patternTemporali.ContainsKey(key))
            {
                B_temp += patternTemporali[key];
            }
            return B_temp;
        }

        private int CalcolaBonusSequenzaOscillante(int numero, string ruota, DateTime dataTarget, ParametriOscillanti parametri)
        {
            int B_seq = 0;
            var estrattiRecenti = GetUltimeEstrazioni(ruota, dataTarget, 3)
                .SelectMany(e => e.Numeri).ToList();

            if (CompletaSequenza(numero, estrattiRecenti))
            {
                B_seq += 15; // Fisso
            }
            return B_seq;
        }

        private int CalcolaBonusCoppieSegnale(int numero, string ruota, DateTime dataTarget)
        {
            int bonus = 0;

            // 🔍 CERCA COPPIE SEGNALE NELLE ULTIME 5 ESTRAZIONI
            var ultimeEstrazioni = GetUltimeEstrazioni(ruota, dataTarget, 5);

            foreach (var estrazione in ultimeEstrazioni)
            {
                var numeriEstrazione = estrazione.Numeri;

                // VERIFICA TUTTE LE COPPIE POSSIBILI NELL'ESTRAZIONE
                for (int i = 0; i < numeriEstrazione.Count - 1; i++)
                {
                    for (int j = i + 1; j < numeriEstrazione.Count; j++)
                    {
                        int num1 = numeriEstrazione[i];
                        int num2 = numeriEstrazione[j];

                        if (RilevatoreCoppieSegnale.IsCoppiaSegnale(num1, num2))
                        {
                            // 🎯 COPPIA SEGNALE TROVATA!
                            var coppia = new List<int> { num1, num2 };
                            var numeriPredetti = RilevatoreCoppieSegnale.GetNumeriPredetti(coppia, ruota, estrazione.Data);

                            // CALCOLA BONUS IN BASE AL TEMPO TRASCORSO
                            int giorniTrascorsi = (dataTarget - estrazione.Data).Days;
                            int moltiplicatoreTempo = Math.Max(1, 5 - giorniTrascorsi); // 5→1 punti

                            if (numeriPredetti.Contains(numero))
                            {
                                bonus += 15 * moltiplicatoreTempo; // Bonus significativo
                            }
                        }
                    }
                }
            }

            return bonus;
        }
    }

    public class ParametriOscillanti
    {
        private DateTime _dataTarget;

        public ParametriOscillanti(DateTime dataTarget)
        {
            _dataTarget = dataTarget;
        }

        // 🎵 FUNZIONI ONDA BASE
        private double OndaGiornaliera() => (_dataTarget.DayOfYear % 1) * 2 * Math.PI;
        private double OndaSettimanale() => ((_dataTarget.DayOfYear / 7.0) % 1) * 2 * Math.PI;
        private double OndaMensile() => ((_dataTarget.DayOfYear / 30.4375) % 1) * 2 * Math.PI;
        private double OndaStagionale() => ((_dataTarget.DayOfYear / 91.25) % 1) * 2 * Math.PI;

        // 🎵 PARAMETRI OSCILLANTI
        public int W_dec1 => 8 + (int)(2 * Math.Sin(OndaSettimanale() * 2)); // 6-10
        public int W_dec2 => W_dec1 / 2;
        public int W_FA => 13 + (int)(3 * Math.Cos(OndaMensile() * 1.5)); // 10-16
        public int W_rit15 => 8 + (int)(2 * Math.Sin(OndaGiornaliera() * 3));
        public int W_rit20 => 13 + (int)(3 * Math.Cos(OndaSettimanale() * 2));
        public int W_rit25 => 21 + (int)(4 * Math.Sin(OndaStagionale() * 1.2)); // 17-25

        public int W_pol => 3 + (int)(1 * Math.Sin(OndaMensile()));
        public int W_arm => 3 + (int)(1 * Math.Cos(OndaSettimanale()));
        public int W_diff => 1;

        // Piccole ottimizzazioni basate sui dati:
        //public int W_dec1 => 8 + (int)(2 * Math.Sin(OndaSettimanale() * 2.1)); // Leggero stretch
        //public int W_FA => 13 + (int)(3 * Math.Cos(OndaMensile() * 1.6)); // Frequenza ottimizzata
        //public int W_rit25 => 21 + (int)(4 * Math.Sin(OndaStagionale() * 1.1)); // Ciclo più lungo

        public int Soglia_diff => 10 + (int)(2 * Math.Sin(OndaMensile())); // 8-12

        public int Soglia => 65 + (int)(5 * Math.Sin(OndaCombinata())); // 60-70
        //public int Soglia => 120 + (int)(5 * Math.Sin(OndaCombinata())); // 60-70

        private double OndaCombinata() =>
            (OndaSettimanale() + OndaMensile() * 0.7 + OndaStagionale() * 0.3) / 2;
    }

}
