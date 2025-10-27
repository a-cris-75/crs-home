using Crs.Home.ApocalypsData.DataEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crs.Home.ApocalypsData
{
    // STRUTTURE DATI
    public struct Point3D
    {
        public int Figura { get; set; }
        public int Decina { get; set; }
        public int Unita { get; set; }
    }

    public class Simmetrie
    {
        public Dictionary<string, double> PatternForti { get; set; } = new();
        public Dictionary<string, double> SimmetriePiani { get; set; } = new();
        public Dictionary<string, double> AssiPrincipali { get; set; } = new();
    }

    // MODELLO GEOMETRICO COMPLETO
    //public class ModelloGeometricoCompleto
    //{
    //    private Cubo3DLotto _cubo;

    //    public ModelloGeometricoCompleto(string ruota, List<Estrazione> estrazioni)
    //    {
    //        foreach (var estrazione in estrazioni)
    //        {
    //            if (_cubo == null)
    //                _cubo = new Cubo3DLotto();
    //            _cubo.AggiornaConEstrazione(estrazione.Numeri);
    //        }   
    //    }

    //    public ModelloGeometricoCompleto(Cubo3DLotto cubo)
    //    {
    //        _cubo = cubo;
    //    }

    //    public List<int> PrevediNumeri(int quanti, out List<Tuple<int, int, List<string>>> numPesiRegole)
    //    {
    //        var punteggi = new Dictionary<int, double>();

    //        for (int numero = 1; numero <= 90; numero++)
    //        {
    //            var coord = ConvertiNumeroInCoordinata(numero);
    //            double punteggio = 0;

    //            // 1. RAFFORZAMENTO ASSE 3-7
    //            if ((coord.Figura == 3 || coord.Figura == 7) &&
    //                (coord.Unita == 7 || coord.Unita == 3))
    //                punteggio += 2.0;

    //            // 2. PIANO FIGURA+UNITÀ=10
    //            if (coord.Figura + coord.Unita == 10)
    //                punteggio += 1.5;

    //            // 3. DIAGONALI PRINCIPALI
    //            if (coord.Figura == coord.Decina)
    //                punteggio += 1.2;

    //            // 4. COMPENSAZIONE ZONE DEBOLI
    //            if ((coord.Figura == 1 && coord.Decina <= 1) ||
    //                (coord.Figura == 9 && coord.Decina >= 7))
    //                punteggio += 1.0;

    //            // 5. PROSSIMITÀ A CELLE ILLUMINATE
    //            punteggio += CalcolaProssimitaLuminosa(coord);

    //            punteggi[numero] = punteggio;
    //        }

    //        //numPesiRegole = new List<Tuple<int, int, List<string>>>();

    //        numPesiRegole = punteggi.Select(pe => Tuple.Create(pe.Key, (int)(pe.Value * 100), new List<string> { "Energia Modello Geometrico * 100" }))
    //           .ToList();

    //        return punteggi.OrderByDescending(p => p.Value)
    //                      .Select(p => p.Key)
    //                      .Take(quanti)
    //                      .ToList();
    //    }

    //    private double CalcolaProssimitaLuminosa(Point3D coord)
    //    {
    //        int vicine = 0;
    //        for (int df = -1; df <= 1; df++)
    //            for (int dd = -1; dd <= 1; dd++)
    //                for (int du = -1; du <= 1; du++)
    //                {
    //                    int f = coord.Figura + df;
    //                    int d = coord.Decina + dd;
    //                    int u = coord.Unita + du;

    //                    if (f >= 1 && f <= 9 && d >= 0 && d <= 8 && u >= 0 && u <= 9)
    //                    {
    //                        if (_cubo.cuboIlluminato[f, d, u])
    //                            vicine++;
    //                    }
    //                }
    //        return vicine * 0.1;
    //    }

    //    private Point3D ConvertiNumeroInCoordinata(int numero)
    //    {
    //        return new Point3D
    //        {
    //            Figura = LottoMath.CalcolaFigura(numero),
    //            Decina = (numero - 1) / 10,
    //            Unita = numero % 10
    //        };
    //    }
    //}

    public class ModelloGeometricoCompleto
    {
        private Cubo3DLotto _cubo;

        public ModelloGeometricoCompleto(string ruota, List<Estrazione> estrazioni)
        {
            foreach (var estrazione in estrazioni)
            {
                if (_cubo == null)
                    _cubo = new Cubo3DLotto();
                _cubo.AggiornaConEstrazione(estrazione.Numeri);
            }
        }

        public ModelloGeometricoCompleto(Cubo3DLotto cubo)
        {
            _cubo = cubo;
        }

        public List<int> PrevediNumeri(int quanti, out List<Tuple<int, int, List<string>>> numPesiRegole)
        {
            var punteggi = new Dictionary<int, double>();
        
            for (int numero = 1; numero <= 90; numero++)
            {
                var coord = ConvertiNumeroInCoordinata(numero);
                double punteggio = 0;

                // 1. RAFFORZAMENTO ASSE 3-7
                punteggio += CalcolaRafforzamentoAsse37(coord);

                // 2. PIANO FIGURA+UNITÀ=10
                if (coord.Figura + coord.Unita == 10)
                    punteggio += 1.5;

                // 3. ALLINEAMENTO DIAGONALI
                punteggio += CalcolaAllineamentoDiagonali(coord);

                // 4. COMPENSAZIONE ZONE DEBOLI
                punteggio += CalcolaCompensazioneZoneDeboli(coord);

                // 5. PROSSIMITÀ A CELLE ILLUMINATE
                punteggio += CalcolaProssimitaLuminosa(coord);

                punteggi[numero] = punteggio;
            }

            numPesiRegole = punteggi.Select(pe => Tuple.Create(pe.Key, (int)(pe.Value * 100), new List<string> { "Energia Modello Geometrico * 100" }))
                    .ToList();

            return punteggi.OrderByDescending(p => p.Value)
                          .Select(p => p.Key)
                          .Take(quanti)
                          .ToList();
        }

        private double CalcolaRafforzamentoAsse37(Point3D coord)
        {
            double punteggio = 0;
        
            // Bonus per numeri che rafforzano l'asse 3-7
            if ((coord.Figura == 3 || coord.Figura == 7) && 
                (coord.Unita == 7 || coord.Unita == 3))
                punteggio += 2.0;

            // Bonus minore per numeri con figura 3 o 7
            if (coord.Figura == 3 || coord.Figura == 7)
                punteggio += 0.5;

            return punteggio;
        }

        private double CalcolaAllineamentoDiagonali(Point3D coord)
        {
            double punteggio = 0;

            // 1. DIAGONALE PRINCIPALE (figura = decina) - es: 11, 22, 33...
            if (coord.Figura == coord.Decina)
                punteggio += 1.0;

            // 2. DIAGONALE SECONDARIA (figura + decina = 8) - es: 08, 17, 26...
            if (coord.Figura + coord.Decina == 8)
                punteggio += 0.8;

            // 3. DIAGONALE TERZIARIA (figura = unità) - es: 1, 22, 83...
            if (coord.Figura == coord.Unita)
                punteggio += 0.6;

            // 4. DIAGONALE ARMONICA (figura + decina + unità = 10) - es: 118, 226, 334...
            if (coord.Figura + coord.Decina + coord.Unita == 10)
                punteggio += 1.2;

            return punteggio;
        }

        private double CalcolaCompensazioneZoneDeboli(Point3D coord)
        {
            double punteggio = 0;

            // Zone identificate come deboli nell'analisi
            var zoneDeboli = new[] 
            {
                (fig: 1, dec: 0, unit: 9), // Zona 1-0-9
                (fig: 9, dec: 8, unit: 0), // Zona 9-8-0  
                (fig: 2, dec: 1, unit: 8), // Zona 2-1-8
                (fig: 8, dec: 7, unit: 1)  // Zona 8-7-1
            };

            foreach (var zona in zoneDeboli)
            {
                double distanza = Math.Abs(coord.Figura - zona.fig) + 
                                 Math.Abs(coord.Decina - zona.dec) + 
                                 Math.Abs(coord.Unita - zona.unit);
            
                if (distanza <= 2)
                    punteggio += (3 - distanza) * 0.8;
            }

            return punteggio;
        }

        private double CalcolaProssimitaLuminosa(Point3D coord)
        {
            int vicine = 0;
            for (int df = -1; df <= 1; df++)
            for (int dd = -1; dd <= 1; dd++)
            for (int du = -1; du <= 1; du++)
            {
                int f = coord.Figura + df;
                int d = coord.Decina + dd;
                int u = coord.Unita + du;
            
                if (f >= 1 && f <= 9 && d >= 0 && d <= 8 && u >= 0 && u <= 9)
                {
                    if (_cubo.cuboIlluminato[f, d, u])
                        vicine++;
                }
            }
            return vicine * 0.1;
        }

        private Point3D ConvertiNumeroInCoordinata(int numero)
        {
            return new Point3D 
            {
                Figura = LottoMath.CalcolaFigura(numero),
                Decina = (numero - 1) / 10,
                Unita = numero % 10
            };
        }
    }

    public class Cubo3DLotto
    {
        public bool[,,] cuboIlluminato; // [figura, decina, unità]
        private List<Point3D> storicoIlluminazioni;

        public Cubo3DLotto()
        {
            cuboIlluminato = new bool[10, 9, 10]; // Figure 0-9, Decine 0-8, Unità 0-9
            storicoIlluminazioni = new List<Point3D>();
        }

        public void AggiornaConEstrazione(List<int> numeri)
        {
            foreach (var numero in numeri)
            {
                var coordinata = ConvertiNumeroInCoordinata(numero);
                cuboIlluminato[coordinata.Figura, coordinata.Decina, coordinata.Unita] = true;
                storicoIlluminazioni.Add(coordinata);
            }
        }

        public Point3D ConvertiNumeroInCoordinata(int numero)
    {
        if (numero < 1 || numero > 90)
            throw new ArgumentException("Numero deve essere tra 1 e 90");

            int figura = LottoMath.CalcolaFigura(numero);
            int decina = (numero - 1) / 10; // 0-8
            int unita = numero % 10; // 0-9 (0 per 10,20,30...)

            return new Point3D { Figura = figura, Decina = decina, Unita = unita };
        }

        public int ConvertiCoordinataInNumero(Point3D coord)
        {
            // Nota: multiple numeri possono avere stessa figura, 
            // ma qui restituiamo il principale
            return coord.Decina * 10 + coord.Unita;
        }

        public List<int> TrovaCelleBuiePromettenti(int quanti = 8)
        {
            var celleBuie = TrovaCelleBuie();
            var punteggi = new Dictionary<Point3D, double>();

            foreach (var cella in celleBuie)
            {
                double punteggio = 0;

                // 🎯 1. VICINANZA A CELLE ILLUMINATE (accumulo energetico)
                punteggio += CalcolaVicinanzaLuminosa(cella);

                // 🎯 2. APPARTENENZA A PIANI ARMONICI
                punteggio += CalcolaPunteggioArmonia(cella);

                // 🎯 3. ALLINEAMENTO CON DIAGONALI PRINCIPALI
                punteggio += CalcolaAllineamentoDiagonali(cella);

                // 🎯 4. CLUSTER TEMPORALI VICINI
                //punteggio += CalcolaInfluenzaTemporale(cella);

                punteggi[cella] = punteggio;
            }

            return punteggi.OrderByDescending(p => p.Value)
                          .Select(p => ConvertiCoordinataInNumero(p.Key))
                          .Take(quanti)
                          .ToList();
        }

        public List<Point3D> TrovaCelleBuie()
        {
            var celleBuie = new List<Point3D>();

            for (int figura = 1; figura <= 9; figura++) // Figure 1-9
            {
                for (int decina = 0; decina <= 8; decina++)
                {
                    for (int unita = 0; unita <= 9; unita++)
                    {
                        if (!cuboIlluminato[figura, decina, unita])
                        {
                            celleBuie.Add(new Point3D
                            {
                                Figura = figura,
                                Decina = decina,
                                Unita = unita
                            });
                        }
                    }
                }
            }

            return celleBuie;
        }

        private double CalcolaAllineamentoDiagonali(Point3D coord)
        {
            double punteggio = 0;

            // 1. DIAGONALE PRINCIPALE (figura = decina)
            if (coord.Figura == coord.Decina)
                punteggio += 1.0;

            // 2. DIAGONALE SECONDARIA (figura + decina = 8)
            if (coord.Figura + coord.Decina == 8)
                punteggio += 0.8;

            // 3. DIAGONALE TERZIARIA (figura = unità)
            if (coord.Figura == coord.Unita)
                punteggio += 0.6;

            // 4. DIAGONALE QUARTARIA (decina = unità)
            if (coord.Decina == coord.Unita)
                punteggio += 0.5;

            // 5. DIAGONALE ARMONICA (figura + decina + unità = 10)
            if (coord.Figura + coord.Decina + coord.Unita == 10)
                punteggio += 1.2;

            // 6. DIAGONALE COMPLEMENTARE (figura + unità = decina)
            if (coord.Figura + coord.Unita == coord.Decina)
                punteggio += 0.7;

            return punteggio;
        }

        private double CalcolaVicinanzaLuminosa(Point3D cella)
        {
            int celleIlluminateVicine = 0;

            // Conta celle illuminate nel vicinato 3D
            for (int df = -1; df <= 1; df++)
                for (int dd = -1; dd <= 1; dd++)
                    for (int du = -1; du <= 1; du++)
                    {
                        int f = cella.Figura + df;
                        int d = cella.Decina + dd;
                        int u = cella.Unita + du;

                        if (f >= 0 && f < 10 && d >= 0 && d < 9 && u >= 0 && u < 10)
                        {
                            if (cuboIlluminato[f, d, u] && !(df == 0 && dd == 0 && du == 0))
                                celleIlluminateVicine++;
                        }
                    }

            return celleIlluminateVicine * 0.1; // Più vicine a zone luminose = più promettenti
        }

        private double CalcolaPunteggioArmonia(Point3D cella)
        {
            double punteggio = 0;

            // Bonus per celle su piani armonici
            if (cella.Figura == cella.Decina) // figura = decina (11, 22, 33...)
                punteggio += 0.5;

            if (cella.Figura + cella.Unita == 10) // figura + unità = 10 (19, 28, 37...)
                punteggio += 0.4;

            if (cella.Decina + cella.Unita == 9) // decina + unità = 9 (18, 27, 36...)
                punteggio += 0.3;

            return punteggio;
        }
    }
}
