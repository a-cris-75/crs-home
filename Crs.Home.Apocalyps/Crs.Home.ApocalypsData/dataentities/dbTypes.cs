using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crs.Home.ApocalypsData.DataEntities
{
    public class DbAccoppiate
    {
        public string Accoppiate { set; get; }
        public string Descrizione { set; get; }
    }

    public class DbAmbiInDecina
    {
        public DateTime Data_Evento { set; get; }
        public string Descrizione { set; get; }
    }

    public class LOTTO
    {
        public DateTime Data { set; get; }
        public int Numero { set; get; }
        public int IDEstrazione { set; get; }
        public int BA1 { set; get; }
        public int BA2 { set; get; }
        public int BA3 { set; get; }
        public int BA4 { set; get; }
        public int BA5 { set; get; }

        public int CA1 { set; get; }
        public int CA2 { set; get; }
        public int CA3 { set; get; }
        public int CA4 { set; get; }
        public int CA5 { set; get; }

        public int FI1 { set; get; }
        public int FI2 { set; get; }
        public int FI3 { set; get; }
        public int FI4 { set; get; }
        public int FI5 { set; get; }

        public int GE1 { set; get; }
        public int GE2 { set; get; }
        public int GE3 { set; get; }
        public int GE4 { set; get; }
        public int GE5 { set; get; }

        public int MI1 { set; get; }
        public int MI2 { set; get; }
        public int MI3 { set; get; }
        public int MI4 { set; get; }
        public int MI5 { set; get; }

        public int NA1 { set; get; }
        public int NA2{ set; get; } 
        public int NA3 { set; get; }
        public int NA4 { set; get; }
        public int NA5 { set; get; }

        public int PA1 { set; get; }
        public int PA2 { set; get; }
        public int PA3 { set; get; }
        public int PA4 { set; get; }
        public int PA5 { set; get; }

        public int RM1 { set; get; }
        public int RM2 { set; get; }
        public int RM3 { set; get; }
        public int RM4 { set; get; }
        public int RM5 { set; get; }

        public int TO1 { set; get; }
        public int TO2 { set; get; }
        public int TO3 { set; get; }
        public int TO4 { set; get; }
        public int TO5 { set; get; }

        public int VE1 { set; get; }
        public int VE2 { set; get; }
        public int VE3 { set; get; }
        public int VE4 { set; get; }
        public int VE5 { set; get; }

        public int NZ1 { set; get; }
        public int NZ2 { set; get; }
        public int NZ3 { set; get; }
        public int NZ4 { set; get; }
        public int NZ5 { set; get; }
    }

    public class PSD_ESTR_LOTTO_SAT
    {
        public DateTime Data { set; get; }
        public int Numero { set; get; }
        public string IDEstrazione { set; get; }
        //public string Numero { set; get; }
        public float BA1 { set; get; }
        public float BA2 { set; get; }
        public float BA3 { set; get; }
        public float BA4 { set; get; }
        public float BA5 { set; get; }
               
        public float CA1 { set; get; }
        public float CA2 { set; get; }
        public float CA3 { set; get; }
        public float CA4 { set; get; }
        public float CA5 { set; get; }
               
        public float FI1 { set; get; }
        public float FI2 { set; get; }
        public float FI3 { set; get; }
        public float FI4 { set; get; }
        public float FI5 { set; get; }
               
        public float GE1 { set; get; }
        public float GE2 { set; get; }
        public float GE3 { set; get; }
        public float GE4 { set; get; }
        public float GE5 { set; get; }
               
        public float MI1 { set; get; }
        public float MI2 { set; get; }
        public float MI3 { set; get; }
        public float MI4 { set; get; }
        public float MI5 { set; get; }
               
        public float NA1 { set; get; }
        public float NA2 { set; get; }
        public float NA3 { set; get; }
        public float NA4 { set; get; }
        public float NA5 { set; get; }
               
        public float PA1 { set; get; }
        public float PA2 { set; get; }
        public float PA3 { set; get; }
        public float PA4 { set; get; }
        public float PA5 { set; get; }
               
        public float RM1 { set; get; }
        public float RM2 { set; get; }
        public float RM3 { set; get; }
        public float RM4 { set; get; }
        public float RM5 { set; get; }
               
        public float TO1 { set; get; }
        public float TO2 { set; get; }
        public float TO3 { set; get; }
        public float TO4 { set; get; }
        public float TO5 { set; get; }
               
        public float VE1 { set; get; }
        public float VE2 { set; get; }
        public float VE3 { set; get; }
        public float VE4 { set; get; }
        public float VE5 { set; get; }
               
        public float NZ1 { set; get; }
        public float NZ2 { set; get; }
        public float NZ3 { set; get; }
        public float NZ4 { set; get; }
        public float NZ5 { set; get; }
    }

    public class ESTRAZIONI
    {
        public int IDEstrazione { set; get; }
        public DateTime Data { set; get; }
        // indica il numero di estrazione
        public int Numero { set; get; }
        public string Ruota { set; get; }
        public int N1 { set; get; }
        public int N2 { set; get; }
        public int N3 { set; get; }
        public int N4 { set; get; }
        public int N5 { set; get; }
        
    }

    public class GEMELLI
    {
        public int IDEstrazione { set; get; }
        public DateTime Data { set; get; }
        public int Numero { set; get; }
        public string Ruota { set; get; }
    }
   
    public class PSD_ESTR_SEGNALI: ESTRAZIONI
    {
        // VALORE DEL NUMERO ESTRATTO associato al segnale, ce ne potrebbe essere più d'uno
        public int NumeroSegnaleA { set; get; }
        public int NumeroSegnaleB { set; get; }
        public int NumeroSegnaleC { set; get; }
        // lista con nome parametro valore
        //public List<Tuple<string,object>> ListaParametri { set; get; }
        public int IDSegnale { set; get; }
        public int IDCalcolo { set; get; }
        public List<PSD_ESTR_SUCCESSI> ListaPrevisioni { set; get; }
    }
    // identifica i numeri previsti effettivamante estratti 
    public class PSD_ESTR_SUCCESSI: ESTRAZIONI
    {
        public int NumeroA { set; get; }
        public int NumeroB { set; get; }
        public int NumeroC { set; get; }
        public int TipoPrevisione { set; get; }
        public string TipoPrevisioneDesc { set; get; }
        // indica una delle 5 posizioni a cui associare la previsione
        public int Posizione { set; get; }
        public int Occorrenze { set; get; }
        public int IDSegnale { set; get; }       
        public bool IsSuccesso { set; get; }
    }
}
