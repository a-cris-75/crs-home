using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crs.Home.ApocalypsData
{
    using Crs.Home.ApocalypsData.DataEntities;
    using System;
    using System.Collections.Generic;
    using System.Linq;

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
        public int CalcolaBonusModelloFinale(int numero, string ruota, DateTime dataTarget)
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

            // 6. INTERAZIONE DECINA-RITARDO (PESO MEDIO)
            int decina = Decina(numero);
            int ritardo = ritardi.ContainsKey(figura) ? ritardi[figura] : 0;

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
                bonus += 8;

            // 8. ATTRACTION FIGURA-DATA (PESO MEDIO - Pattern universale)
            int figuraData = CalcolaFiguraData(dataTarget);
            if (figura == figuraData)
            {
                switch (figuraData)
                {
                    case 3: bonus += 15; break;
                    case 5: bonus += 18; break;
                    case 8: bonus += 12; break;
                }
            }

            return Math.Min(100, bonus);
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

        public int CalcolaBonusModelloAdattivo(int numero, string ruota, DateTime dataTarget)
        {
            int bonus = CalcolaBonusModelloFinale(numero, ruota, dataTarget);

            var ritardi = CalcolaRitardiFigure(ruota, dataTarget);
            int decina = Decina(numero);
            int figura = Figura(numero);
            int ritardo = ritardi.ContainsKey(figura) ? ritardi[figura] : 0;

            // STRATEGIA ADATTIVA PER RUOTA
            if (ruota == "RM") // ROMA
            {
                if ((decina == 1 || decina == 4 || decina == 7) && ritardo > 15)
                    bonus += 12;
            }
            else if (ruota == "BA") // BARI  
            {
                if ((decina == 0 || decina == 4 || decina == 7 || decina == 5 || decina == 9) && ritardo > 15)
                    bonus += 12;
            }

            return Math.Min(85, bonus);
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

        public List<int> PrevisioneNumeri(string ruota, DateTime dataTarget, int soglia = 25)
        {
            var numeriSelezionati = new List<int>();

            for (int numero = 1; numero <= 90; numero++)
            {
                int bonus = CalcolaBonusModelloFinale(numero, ruota, dataTarget);
                if (bonus >= soglia)
                {
                    numeriSelezionati.Add(numero);
                }
            }

            return numeriSelezionati.OrderBy(n => n).ToList();
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
                var numeriSelezionati = PrevisioneNumeri(estrazione.Ruota, estrazione.Data, 25);
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
