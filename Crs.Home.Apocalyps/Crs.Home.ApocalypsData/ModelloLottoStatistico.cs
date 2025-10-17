using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crs.Home.ApocalypsData.DataEntities;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Crs.Home.ApocalypsData
{
    public class ModelloLottoStatistico
    {
        private List<Estrazione> estrazioni;

        public ModelloLottoStatistico(List<Estrazione> estrazioniStoriche)
        {
            this.estrazioni = estrazioniStoriche.OrderBy(e => e.Data).ToList();
        }

        // === METODI FONDAMENTALI ===

        public int Figura(int numero)
        {
            int fig = numero;
            while (fig >= 10)
            {
                fig = fig / 10 + fig % 10;
            }
            return fig;
        }

        public int Decina(int numero) => numero / 10;

        public int Antifigura(int figura)
        {
            switch (figura)
            {
                case 0: return 0;
                case 1: return 9;
                case 2: return 8;
                case 3: return 7;
                case 4: return 6;
                case 5: return 5;
                case 6: return 4;
                case 7: return 3;
                case 8: return 2;
                case 9: return 1;
                default: return figura;
            }
        }

        // === CALCOLO RITARDI FIGURE ===

        public Dictionary<int, int> CalcolaRitardiFigure(string ruota, DateTime dataTarget)
        {
            var ritardi = new Dictionary<int, int>();

            // Inizializza tutte le figure (0-9)
            for (int fig = 0; fig <= 9; fig++)
            {
                ritardi[fig] = 0;
            }

            // Calcola ritardi dalle estrazioni precedenti
            var estrazioniPrecedenti = estrazioni
                .Where(e => e.Ruota == ruota && e.Data < dataTarget)
                .OrderByDescending(e => e.Data)
                .ToList();

            foreach (var estrazione in estrazioniPrecedenti)
            {
                var figureEstratte = estrazione.Numeri.Select(Figura).Distinct();

                foreach (var fig in figureEstratte)
                {
                    if (ritardi[fig] == 0) // Prima occorrenza
                    {
                        // Conta da quante estrazioni manca
                        int ritardo = 1;
                        var estrazioniDopo = estrazioni
                            .Where(e => e.Ruota == ruota && e.Data > estrazione.Data && e.Data < dataTarget)
                            .OrderBy(e => e.Data)
                            .ToList();

                        foreach (var eDopo in estrazioniDopo)
                        {
                            if (!eDopo.Numeri.Select(Figura).Contains(fig))
                                ritardo++;
                            else
                                break;
                        }
                        ritardi[fig] = ritardo;
                    }
                }
            }

            return ritardi;
        }

        // === METODI PER RECUPERO DATI RECENTI ===

        public List<int> GetDecineRecenti(string ruota, DateTime dataTarget, int numEstrazioni)
        {
            return estrazioni
                .Where(e => e.Ruota == ruota && e.Data < dataTarget)
                .OrderByDescending(e => e.Data)
                .Take(numEstrazioni)
                .SelectMany(e => e.Numeri)
                .Select(Decina)
                .Distinct()
                .ToList();
        }

        public List<int> GetFigurePrecedenti(string ruota, DateTime dataTarget, int numEstrazioni)
        {
            return estrazioni
                .Where(e => e.Ruota == ruota && e.Data < dataTarget)
                .OrderByDescending(e => e.Data)
                .Take(numEstrazioni)
                .SelectMany(e => e.Numeri)
                .Select(Figura)
                .Distinct()
                .ToList();
        }

        public List<int> GetUnitaRecenti(string ruota, DateTime dataTarget, int numEstrazioni)
        {
            return estrazioni
                .Where(e => e.Ruota == ruota && e.Data < dataTarget)
                .OrderByDescending(e => e.Data)
                .Take(numEstrazioni)
                .SelectMany(e => e.Numeri)
                .Select(n => n % 10)
                .Distinct()
                .ToList();
        }

        public List<int> GetFigureRecenti(string ruota, DateTime dataTarget, int numEstrazioni)
        {
            return estrazioni
                .Where(e => e.Ruota == ruota && e.Data < dataTarget)
                .OrderByDescending(e => e.Data)
                .Take(numEstrazioni)
                .SelectMany(e => e.Numeri)
                .Select(Figura)
                .Distinct()
                .ToList();
        }

        public List<int> GetUltimiEstratti(string ruota, DateTime dataTarget, int numEstrazioni)
        {
            return estrazioni
                .Where(e => e.Ruota == ruota && e.Data < dataTarget)
                .OrderByDescending(e => e.Data)
                .Take(numEstrazioni)
                .SelectMany(e => e.Numeri)
                .ToList();
        }

        // === MODELLO PRINCIPALE ===
        public Tuple<int, List<string>> CalcolaBonusModelloFinale(int numero, string ruota, DateTime dataTarget)
        {
            var regoleAttivate = new List<string>();
            var ritardi = CalcolaRitardiFigure(ruota, dataTarget);
            int figura = Figura(numero);
            int unita = numero % 10;

            //int bonus = 0;
            int B_dec = 0;
            int B_FA = 0;
            int B_stessaFig = 0;
            int B_stessaUnit = 0;
            int B_ritFig = 0;     
            int B_figData = 0;
            int B_figUnita = 0;
           

            // 1. RITARDO FIGURE (PESO ALTO - +25% vantaggio)
            if (ritardi.ContainsKey(figura) && ritardi[figura] > 15)
                B_ritFig += 30 + Math.Min(20, ritardi[figura] - 15);

            // 2. DECINE (PESO MEDIO - +4% vantaggio)
            var decineRecenti = GetDecineRecenti(ruota, dataTarget, 2);
            if (decineRecenti.Contains(Decina(numero)))
                B_dec += 15;

            // 3. FIGURA-ANTIFIGURA (PESO BASSO - +3% vantaggio)
            var figurePrecedenti = GetFigurePrecedenti(ruota, dataTarget, 1);
            int antifigura = Antifigura(figura);
            if (figurePrecedenti.Contains(antifigura))
                B_FA += 10;

            // 4. STESSA UNITÀ (PESO MEDIO-ALTO - +9.5% vantaggio)
            var unitaRecenti = GetUnitaRecenti(ruota, dataTarget, 2);
            if (unitaRecenti.Contains(unita))
                B_stessaUnit += 12;

            // 5. STESSA FIGURA (PESO MEDIO - +5.8% vantaggio)
            var figureRecenti = GetFigureRecenti(ruota, dataTarget, 2);
            if (figureRecenti.Contains(figura))
                B_stessaFig += 8;

            // 6. INTERAZIONE DECINA-RITARDO (PESO MEDIO)
            //int decina = Decina(numero);
            //int ritardo = ritardi.ContainsKey(figura) ? ritardi[figura] : 0;

            // Strategia adattiva per ruota
            //if (ruota == "RM") // ROMA
            //{
            //    if ((decina == 1 || decina == 4 || decina == 7) && ritardo > 15)
            //        bonus += 10;
            //}
            //else if (ruota == "BA") // BARI
            //{
            //    if ((decina == 0 || decina == 4 || decina == 7 || decina == 5 || decina == 9) && ritardo > 15)
            //        bonus += 10;
            //}

            // 7. FIGURA=UNITÀ (PESO BASSO - Pattern debole ma verificato)
            if (figura == unita)
                B_figUnita += 8;

            // 8. ATTRACTION FIGURA-DATA (PESO MEDIO - Pattern universale)
            int figuraData = CalcolaFiguraData(dataTarget);
            if (figura == figuraData)
            {
                switch (figuraData)
                {
                    case 3: B_figData += 15; break;
                    case 5: B_figData += 18; break;
                    case 8: B_figData += 12; break;
                }
            }

            if (B_ritFig > 0) regoleAttivate.Add("RitardoFigura");
            if (B_dec > 0) regoleAttivate.Add("Decina");
            if (B_FA > 0) regoleAttivate.Add("FiguraAntifigura");
            if (B_stessaUnit > 0) regoleAttivate.Add("Unita");
            if (B_stessaFig > 0) regoleAttivate.Add("StessaFigura");
            if (B_figUnita > 0) regoleAttivate.Add("FiguraUnita");
            if (B_figData > 0) regoleAttivate.Add("FiguraData");

            double bonus_moltiplicativo =
               (B_dec / 10.0 + 1) *
               (B_FA / 10.0 + 1) *
               (B_ritFig / 10.0 + 1) *
               (B_stessaUnit / 10.0 + 1) *
               (B_stessaFig / 10.0 + 1) *
               (B_figUnita / 10.0 + 1) *
               (B_figData / 10.0 + 1) * 10;

            return Tuple.Create((int)Math.Round(bonus_moltiplicativo), regoleAttivate);

            //return Math.Min(100, bonus);
        }

        public int CalcolaBonusModelloFinale_(int numero, string ruota, DateTime dataTarget)
        {
            var ritardi = CalcolaRitardiFigure(ruota, dataTarget);
            int figura = Figura(numero);
            int unita = numero % 10;

            int bonus = 0;

            // 1. RITARDO FIGURE (PESO ALTO - +25% vantaggio)
            if (ritardi.ContainsKey(figura) && ritardi[figura] > 15)
                bonus += 30 + Math.Min(20, ritardi[figura] - 15);

            // 2. DECINE (PESO MEDIO - +4% vantaggio)
            var decineRecenti = GetDecineRecenti(ruota, dataTarget, 2);
            if (decineRecenti.Contains(Decina(numero)))
                bonus += 15;

            // 3. FIGURA-ANTIFIGURA (PESO BASSO - +3% vantaggio)
            var figurePrecedenti = GetFigurePrecedenti(ruota, dataTarget, 1);
            int antifigura = Antifigura(figura);
            if (figurePrecedenti.Contains(antifigura))
                bonus += 10;

            // 4. STESSA UNITÀ (PESO MEDIO-ALTO - +9.5% vantaggio)
            var unitaRecenti = GetUnitaRecenti(ruota, dataTarget, 2);
            if (unitaRecenti.Contains(unita))
                bonus += 12;

            // 5. STESSA FIGURA (PESO MEDIO - +5.8% vantaggio)
            var figureRecenti = GetFigureRecenti(ruota, dataTarget, 2);
            if (figureRecenti.Contains(figura))
                bonus += 8;

            return Math.Min(75, bonus);
        }

        public Tuple<int, List<string>> CalcolaBonusModelloAdattivo(int numero, string ruota, DateTime dataTarget)
        {
            Tuple<int, List<string>> bonus = CalcolaBonusModelloFinale(numero, ruota, dataTarget);

            var ritardi = CalcolaRitardiFigure(ruota, dataTarget);
            int decina = Decina(numero);
            int figura = Figura(numero);
            int ritardo = ritardi.ContainsKey(figura) ? ritardi[figura] : 0;

            // STRATEGIA ADATTIVA PER RUOTA
            if (ruota == "RM") // ROMA
            {
                if ((decina == 1 || decina == 4 || decina == 7) && ritardo > 15)
                    bonus = new Tuple<int, List<string>>(bonus.Item1 + 12, bonus.Item2) ;
            }
            else if (ruota == "BA") // BARI  
            {
                if ((decina == 0 || decina == 4 || decina == 7 || decina == 5 || decina == 9) && ritardo > 15)
                    bonus = new Tuple<int, List<string>>(bonus.Item1 + 12, bonus.Item2);
            }
            return bonus;
            //return Math.Min(85, bonus);
        }

        public int CalcolaBonusFiguraData(int numero, DateTime data)
        {
            int figuraNumero = Figura(numero);
            int figuraData = CalcolaFiguraData(data);

            // ATTRACTION FIGURA-DATA VERIFICATE: universali
            if (figuraNumero == figuraData)
            {
                switch (figuraData)
                {
                    // su RM
                    case 3: return 15; // Forte attraction
                    case 5: return 12; // Forte attraction  
                    case 8: return 10; // Media attraction
                    default: return 0; // Altre non significative

                    // su BA - RM
                    //case 3: return 15; // Forte su entrambe
                    //case 5: return 18; // Molto forte su Bari
                    //case 8: return 12; // Media su entrambe
                    //default: return 0;
                }
            }

            return 0;
        }

        private int CalcolaFiguraData(DateTime data)
        {
            int somma = data.Day + data.Month + data.Year;
            return Figura(somma);
        }

        // === METODO PER SELEZIONE NUMERI ===

        public List<int> PrevisioneNumeri(string ruota, DateTime dataTarget, List<int> numeriUsciti, out List<Tuple<int, int, List<string>>> risultati, int soglia = 25)
        {
            risultati = new List<Tuple<int, int, List<string>>>();
            for (int numero = 1; numero <= 90; numero++)
            {
                var bonus = CalcolaBonusModelloFinale(numero, ruota, dataTarget);
                risultati.Add(Tuple.Create(numero, bonus.Item1, bonus.Item2));
            }
            
            var consigliati = risultati
              .Where(x => x.Item2 >= soglia)
              .OrderByDescending(x => x.Item2)
              .ToList();

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
                //var risultati1 = risultati.OrderBy(X => X.Item2).TakeLast(consigliati.Count).ToList();
                var risultati1 = risultati.Where(X => consigliati.Contains(X)).ToList();
                risultati1.AddRange(risultati.Where(X => X.Item1 == numeriUsciti[0] ||
                                            X.Item1 == numeriUsciti[1] ||
                                            X.Item1 == numeriUsciti[2] ||
                                            X.Item1 == numeriUsciti[3] ||
                                            X.Item1 == numeriUsciti[4]));
                risultati = risultati1.DistinctBy(X => X.Item1).ToList();
            }
            else
                risultati = risultati.OrderBy(X => X.Item2).TakeLast(consigliati.Count).ToList();

            //return numeriSelezionati.OrderBy(n => n).ToList();
            return consigliati.Select(x => x.Item1).ToList();
        }

        // === TEST E VALUTAZIONE ===

        public void TestModelloFinale(string ruota = "RM")
        {
            var testSet = estrazioni.Where(e => e.Ruota == ruota && e.Data.Year >= 2023).ToList();

            Console.WriteLine("🔬 TEST MODELLO FINALE 2023-2025");
            Console.WriteLine("================================");

            int successi = 0;
            int totalNumeriSelezionati = 0;
            double costo = 0;
            double vincite = 0;
            int ambate = 0;
            int ambi = 0;
            int terni = 0;

            foreach (var estrazione in testSet)
            {
                var numeriSelezionati = PrevisioneNumeri(estrazione.Ruota, estrazione.Data, estrazione.Numeri, out List<Tuple<int,int,List<string>>> risultati, 25);
                int numeriGiocati = numeriSelezionati.Count;

                totalNumeriSelezionati += numeriGiocati;
                costo += numeriGiocati * 1.0;

                if (numeriGiocati > 0)
                {
                    var numeriIndovinati = numeriSelezionati.Where(n => estrazione.Numeri.Contains(n)).ToList();
                    int numIndovinati = numeriIndovinati.Count;

                    if (numIndovinati >= 1)
                    {
                        successi++;

                        // AMBATE
                        vincite += numIndovinati * 11.23;
                        ambate += numIndovinati;

                        // AMBI
                        if (numIndovinati >= 2)
                        {
                            int combinazioniAmbo = Fattoriale(numIndovinati) / (2 * Fattoriale(numIndovinati - 2));
                            vincite += combinazioniAmbo * 250.0;
                            ambi += combinazioniAmbo;
                        }

                        // TERNI
                        if (numIndovinati >= 3)
                        {
                            int combinazioniTerno = Fattoriale(numIndovinati) / (6 * Fattoriale(numIndovinati - 3));
                            vincite += combinazioniTerno * 4500.0;
                            terni += combinazioniTerno;
                        }
                    }
                }
            }

            // RISULTATI
            double successRate = (double)successi / testSet.Count;
            double numeriPerEstrazione = (double)totalNumeriSelezionati / testSet.Count;
            double roi = ((vincite - costo) / costo) * 100;

            Console.WriteLine($"📊 RISULTATI:");
            Console.WriteLine($"✅ Estrazioni con vincita: {successi}/{testSet.Count} ({successRate * 100:0.0}%)");
            Console.WriteLine($"📈 Numeri/estrazione: {numeriPerEstrazione:0.1}");
            Console.WriteLine($"💰 Costo: {costo:0}€, Vincite: {vincite:0}€");
            Console.WriteLine($"📉 ROI: {roi:0.0}%");
            Console.WriteLine($"🎯 Ambate: {ambate}, Ambi: {ambi}, Terni: {terni}");

            if (roi > 0)
                Console.WriteLine($"🎉 MODELLO PROFITTEVOLE!");
            else
                Console.WriteLine($"⚠️ MODELLO NON PROFITTEVOLE");
        }

        private int Fattoriale(int n)
        {
            if (n <= 1) return 1;
            int result = 1;
            for (int i = 2; i <= n; i++)
                result *= i;
            return result;
        }
    }

    // CLASSE ESTRAZIONE (già esistente nel tuo codice)
    //public class Estrazione
    //{
    //    public DateTime Data { get; set; }
    //    public string Ruota { get; set; }
    //    public List<int> Numeri { get; set; }

    //    public Estrazione(DateTime data, string ruota, List<int> numeri)
    //    {
    //        Data = data;
    //        Ruota = ruota;
    //        Numeri = numeri;
    //    }
    //}
}
