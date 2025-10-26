using Crs.Home.ApocalypsData.DataEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crs.Home.ApocalypsData
{
    public static class LottoMath
    {
        public static int CalcolaFigura(int numero)
        {
            if (numero < 1 || numero > 90)
                return 0;

            int somma = numero;
            while (somma > 9)
            {
                somma = somma / 10 + somma % 10;
            }
            return somma;
        }

        public static Dictionary<string, List<int>> CaricaFamiglieNumeri()
        {
            var famiglie = new Dictionary<string, List<int>>();

            // 🎯 FIGURE (1-9)
            for (int figura = 1; figura <= 9; figura++)
            {
                var numeriFigura = new List<int>();
                for (int num = 1; num <= 90; num++)
                {
                    if (CalcolaFigura(num) == figura)
                        numeriFigura.Add(num);
                }
                famiglie.Add($"figura_{figura}", numeriFigura);
            }

            // 🔢 DECINE (0-8)
            for (int decina = 0; decina <= 8; decina++)
            {
                var numeriDecina = new List<int>();
                for (int unita = 0; unita <= 9; unita++)
                {
                    int numero = decina * 10 + unita;
                    if (numero >= 1 && numero <= 90)
                        numeriDecina.Add(numero);
                }
                famiglie.Add($"decina_{decina}", numeriDecina);
            }

            // 🔄 UNITÀ (0-9)
            for (int unita = 0; unita <= 9; unita++)
            {
                var numeriUnita = new List<int>();
                for (int decina = 0; decina <= 8; decina++)
                {
                    int numero = decina * 10 + unita;
                    if (numero >= 1 && numero <= 90)
                        numeriUnita.Add(numero);
                }
                famiglie.Add($"unita_{unita}", numeriUnita);
            }

            // 🎵 NUMERI ARMONICI (figura = decina)
            var numeriArmonici = new List<int>();
            for (int num = 1; num <= 90; num++)
            {
                int figura = CalcolaFigura(num);
                int decina = num / 10;
                if (figura == decina)
                    numeriArmonici.Add(num);
            }
            famiglie.Add("armonici", numeriArmonici);

            // 🔺 NUMERI SPECCHIO (figura + unità = 10)
            var numeriSpecchio = new List<int>();
            for (int num = 1; num <= 90; num++)
            {
                int figura = CalcolaFigura(num);
                int unita = num % 10;
                if (figura + unita == 10)
                    numeriSpecchio.Add(num);
            }
            famiglie.Add("specchio", numeriSpecchio);

            return famiglie;
        }

        
    }
}
