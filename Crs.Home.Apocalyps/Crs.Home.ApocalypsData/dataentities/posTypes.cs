using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crs.Home.ApocalypsData.DataEntities
{
    #region CLASSI USATE IN GENOMA
    public class NumeroRegola1{
    	public int NumeroA{set; get;}
        public int NumeroB{set; get;}

        public int FreqDirettaStorico{set; get;}
        public int FreqIndirettaStorico{set; get;}

        public int UscitaDirettaInEstrSucc{set; get;}  //--- su sua ruota
        public int UscitaIndirettaInEstrSucc{set; get;}  //--- sull'altra ruota dell'accoppiata

        public int NumUsciteDiretteDec{set; get;}
        public int NumUsciteIndiretteDec { set; get; }
    }

    public class RisultatiR1{
        public int NumGiocate{set; get;}
  	//--- Indica la frequenza dell'uscita del numero e del suo inverso sulla
    //--- prima e sulla seconda ruota dell'accoppiata
  	    public int Freq_dir1{set; get;}
        public int Freq_ind1{set; get;}
        public int Freq_dir2{set; get;}
        public int Freq_ind2{set; get;}

        public int Freq_dir{set; get;}
        public int Freq_ind{set; get;}

    //--- Indica le positività dirette e indirette sulla ruota
    //--- Corrispondono alla somma delle frequenze dir e ind su una ruota e sull'altra
        public int FreqTot_R1{set; get;}
        public int FreqTot_R2{set; get;}
    //--- Indica la distanza massima fra le frequenze
        public int DistMax_fd1{set; get;}
        public int DistMax_fd2{set; get;}
        public int DistMax_fI1{set; get;}
        public int DistMax_fI2{set; get;}
    //--- Indica la distanza massima fra le frequenze dir e ind
        public int DistMax_fD{set; get;}
        public int DistMax_fI{set; get;}
    //--- indica la distanza media fra le frequenze
        public float DistMed_fD{set; get;}
        public float DistMed_fI{set; get;}

        public float DistMed_fD1{set; get;}
        public float DistMed_fI1{set; get;}
        public float DistMed_fD2{set; get;}
        public float DistMed_fI2{set; get;}

    }

    public class Previsioni{
        public DateTime DataPrev   {set; get;}
        public string Ruota           {set; get;}
        public int NumDirColpo1    {set; get;}
        public int NumDirColpo2    {set; get;}
        public int NumDirColpo3    {set; get;}
        public int NumIndColpo1    {set; get;}
        public int NumIndColpo2    {set; get;}
        public int NumIndColpo3    {set; get;}
        }
    
    //--- Matrice contenente tutti i risultati riguardanti un'accoppiata e un colpo
    //public class MatriceRisultatiR1 = array[0..4,0..3] of RisultatiR1;

    //type Tlinea = (LFrequenza1,LMedia1,LCanaleInf1,LCanaleSup1,LCanaleInf2,LCanaleSup2,LFrequenza2,LMedia2,LCostante1,LCostante2);
    public enum TLinea {LFrequenza,LMedia,LCanaleInf,LCanaleSup,LCostante,LMediaAssoluta,LSinusoide,LIperbole};
    public enum Colpi { Uno, Due, Tre};

    public class ParametriPendenze {
        public float Ang_LineaApertura_Da  {set; get;}
        public float Ang_LineaSegnale_Da   {set; get;}
        public float Ang_LineaChiusura_Da  {set; get;}
        public float Ang_LineaApertura_A   {set; get;}
        public float Ang_LineaSegnale_A    {set; get;}
        public float Ang_LineaChiusura_A   {set; get;}
        public float Diff_Ang_LSegnAper_Da {set; get;}
        public float Diff_Ang_LSegnAper_A  {set; get;}
    }

    public class PendenzeLinee {
        public float PendenzaFreq1   {set; get;}
        public float PendenzaFreq2   {set; get;}
        public float PendenzaMedia1  {set; get;}
        public float PendenzaMedia2  {set; get;}
    }

    public class RangePendenzeLinee {
       public  float PendenzaFreq1Da{set; get;}
        public float PendenzaFreq1A {set; get;}
        public float PendenzaFreq2Da{set; get;}
        public float PendenzaFreq2A {set; get;}
        public float PendenzaMedia1Da{set; get;}
        public float PendenzaMedia1A {set; get;}
        public float PendenzaMedia2Da{set; get;}
        public float PendenzaMedia2A {set; get;}
    }

    public class PendenzeLineeSegnale {
        public float PendenzaLApertura  {set; get;}
        public float PendenzaLSegnale   {set; get;}
        public float PendenzaLChiusura  {set; get;}
    }

    public class ParametriR1 {
        //numEstrASettimana : word;
        public bool[] Accoppiate;
        public int PosizioneNum1 {set; get;}
        public int PosizioneNum2 {set; get;}
        public DateTime DataInizio	{set; get;}
        public DateTime DataFine    {set; get;}
        public DateTime DataPrevisione{set; get;}
        public int Limitazioni		{set; get;}	// indica il tipo di eccezione per i num > 90
        //risInNEstr		: array[0..2] of boolean;  // indica in quali estrazioni successive calcolo i risultati (1,2,3)
        public bool[] Colpi;  // indica in quali estrazioni successive calcolo i risultati (1,2,3)
        public bool[] TipoFreq;   // 0: dir1 1:dir2 2:ind2 3:ind1
        public int NumLaterali     {set; get;}

        public string Decine     {set; get;}
        public char RuoteOAcc   {set; get;}
        public char EstrDecGem  {set; get;}
        //-- rappresenta l'insieme delle ruote su cui si desidera effettuare i calcoli
        //-- successivamente i calcoli vengono eseguiti per singola ruota o accoppiata
        //-- ma questo parametro è diverso e viene passato singolarmente alle procedure
        //ruote: array[0..9] of boolean;

        //-- da pargraf 56-01-2009

        //calcolaSuTutteLeDecineR1{set; get;}
        //calcolaSuTutteLeDecineR2{set; get;}
    }
    /*
    public class ParametriNumScelti {
      bool[] Numeri;
    }*/

    //-- descrivono i parametri di calcolo generali, che identificano un insieme di calcoli
    public class ParametriR1Generali {
        public bool CalcolaSuTutteLeDecineR1{set; get;}
        public bool CalcolaSuTutteLeDecineR2{set; get;}
    }

    public class ParametriGenerali {
        public DateTime DataInizio {set; get;}
        public DateTime DataFine	{set; get;}
        public DateTime DataPrevisione{set; get;}
        //ruote: array[0..9] of boolean;
        public int NumLaterali     {set; get;}
    }

    public class ParametriGrafico {

        public DateTime DataInizio{set; get;}
        public DateTime DataFine  {set; get;}

        //calcolaSuTutteLeDecineR1{set; get;}
        //calcolaSuTutteLeDecineR2{set; get;}
        //--- parametri per la costruzione delle linee del grafico
        public string TipoMedia1{set; get;}
        public string TipoMedia2{set; get;}
        public string TipoMedia3{set; get;}
        public float AmpiezzaEstrazioniF1{set; get;}
        public float AmpiezzaEstrazioniF2{set; get;}
        public float AmpiezzaEstrazioniF3{set; get;}
        public float AmpiezzaCanale1{set; get;}
        public float AmpiezzaCanale2{set; get;}
        public float AmpiezzaCanale3{set; get;}
        public bool Canale1Variabile{set; get;}
        public bool Canale2Variabile{set; get;}
        public bool Canale3Variabile{set; get;}
        //--- Se le linee di apertura e chiusura sono linee costanti
        public float AltezzaLineaAperturaCostante{set; get;}
        public float AltezzaLineaChiusuraCostante{set; get;}

        public bool ShowMedia1{set; get;}
        public bool ShowMedia2{set; get;}
        public bool ShowMedia3{set; get;}
        public bool ShowCostante2{set; get;}
       public  bool ShowCostante3{set; get;}
    }

    public class LineaSegnale {
        //--- parametri per la costruzione delle linee del grafico
        public TLinea Linea{set; get;}
        public string TipoMedia{set; get;}
        public float AmpiezzaEstrazioniF{set; get;}
        public float AmpiezzaCanale{set; get;}
        public bool CanaleVariabile{set; get;}
        //--- Se le linee di apertura e chiusura sono linee costanti
        public float AltezzaLineaCostante{set; get;}
        public float TraslazioneADx{set; get;} // nel caso della sinusoide può essere utile, ma anche per le altre linee
                              	 // per ora implementato solo per la sinusoide (05/02/2011)

        public bool ShowMedia{set; get;}
        public bool ShowCostante{set; get;}

        public int TipoBaseNumerica {set; get;}
        public ParametriR1 ParamR1 {set; get;}
        public bool[] NumeriScelti; //90
    }

    public class ParametriGraficoSegnale {
        public bool SegnAperturaPos {set; get;}       //
        public bool SegnAperturaNeg {set; get;}       //
        public bool SegnChiusuraPos {set; get;}       //
        public bool SegnChiusuraNeg {set; get;}       //
        public bool WithLimitsSupInf{set; get;}       //
        public float PendMinLineaApertura{set; get;}
        public float PendMaxLineaChiusura{set; get;}
        //--------------------------- FILTRI -----------------------------//
        public int MaxNumPrevGiuste{set; get;}       //
        public int GoGiocate       {set; get;}
        public int StopGiocate     {set; get;}
        public float HeigthSegnaleDa {set; get;}
        public float HeigthSegnaleA  {set; get;}
        //--- Specifica se prendere in considerazione il filtro sulle pendenze
        public bool Pendenze        {set; get;}
        public ParametriPendenze ParamPend{set; get;}
        //----------------------------------------------------------------//

    }

    //-- DEFINISCONO IL TIPO DI SEGNALE DA CONSIDERARE
    public class ParametriSegnale {
        //--- sostituiscono il tipo di segnale indicando le linee che si incrociano
        //--- aprendo le giocate e chiudendole
        public TLinea LineaChiusura   {set; get;}      //
        public TLinea LineaApertura   {set; get;}       //
        public TLinea LineaSegnale   {set; get;}
        public bool SegnAperturaPos {set; get;}       //
        public bool SegnAperturaNeg {set; get;}       //
        public bool SegnChiusuraPos {set; get;}       //
        public bool SegnChiusuraNeg {set; get;}       //
        public bool WithLimitsSupInf{set; get;}       //
        public int MaxNumPrevGiuste{set; get;}       //
        //--------------------------- FILTRI -----------------------------//
        public int GoGiocate       {set; get;}
        public int StopGiocate     {set; get;}
        public float HeigthSegnaleDa {set; get;}
        public float HeigthSegnaleA  {set; get;}
        //--- Specifica se prendere in considerazione il filtro sulle pendenze
        public bool Pendenze        {set; get;}
        public ParametriPendenze ParamPend;
        //----------------------------------------------------------------//

        public string descrizione{set; get;}
        //numProtocollo{set; get;}
    }

    public class ParametriCalcoloR1 {
        public bool[] Colpi; //3
        public bool[] Ruote; //10
        public bool[] Tipofreq;//4
        public string Decine;
    }
    #endregion

    public class PSD_PARAMETRI_R1
    {
        public List<string> POS_DEC_R12;
        public List<int> COLPI;
        public List<int> TIPO_FREQUENZA;
        public List<int> POS_ESTR;
        // DATO CHE I NUMERI POSSONO ESSERE GENERATI INCROCIANDO DIVERSE RUOTE (ES BA-CA, CA-FI, FI-GE..)
        // DEVO POTER SCEGLIERE la lista corretta per poter stabilire una numerosità coerente in fase di determinazione dei successi
        public List<string> ACCOPPIATE_DEC;

        public PSD_PARAMETRI_R1()
        {
            POS_DEC_R12 = new List<string>();
            COLPI = new List<int>();
            TIPO_FREQUENZA = new List<int>();
            POS_ESTR = new List<int>();
            ACCOPPIATE_DEC = new List<string>();
        }
    }

    public class PSD_PARAMETRI_SEGN_SAT
    {
        public int MAX_DIST_ORBIT_SAT { set; get; }
        public float MIN_IDX_SAT { set; get; }
        public float MIN_IDX_SAT_TO_Y { set; get; }
    }

        public class PSD_RES_RUOTA
    {
        public string RUOTA { set; get; }
        // occorrenze in un periodo di tempo
        public int OCCORRENZE { set; get; }
        // variazione da periodo precedente
        public float VARIAZIONE { set; get; }
    }

    public class PSD_RES
    {
        public string RUOTA { set; get; }
        public int OCCORRENZE_GROUPBY { set; get; }      
        public int REGOLA { set; get; }
        // IN BASE ALLA QUERY POSSO RAGGRUPARE I DATI IN BASE AI FILTRI SOTTO DEFINITI
        // NELLA QRY DEVO DEFINIRE I CAMPI RISULTANTI CON : NOMECAMPO AS FILTRO1_STR se è una stringa 
        // in questo modo posso ottenere una lista generica su cui posso applicare i filtri successivamente
        // .. vedi GetRes
        // non necessariamente (in base al calcolo) la DATA_EVENTO potrebbe avere un valore, se per esempio
        // raggruppo i dati per periodo, lo stesso vale per NUMERO
        public DateTime DATA_EVENTO { set; get; }
        public int ID_ESTRAZIONE { set; get; }
        public int NUM_ESTRATTO { set; get; }
        public string FILED1_STR { set; get; }
        public string FIELD2_STR { set; get; }
        public string FIELD3_STR { set; get; }
        public int FIELD1_INT { set; get; }
        public int FIELD2_INT { set; get; }
        public int FIELD3_INT { set; get; }
    }

    /// <summary>
    /// Contiene il resoconto dei dcalcoli sul tabellone delle estrazioni raggruppando un intervallo di estrazioni.
    /// L'intervallo è definito da un certo insieme di estrazioni consecutive (parametro: normaletene è 150, che è un valore 
    /// indicante un numero di successi abbastanza costante o con poca variabilità)
    /// </summary>
    public class PSD_RES_TOT
    {
        public int IDX { set; get; }
        public DateTime DATA_EVENTO_DA { set; get; }
        public DateTime DATA_EVENTO_A { set; get; }
        // occorrenze nel periodo (da DATA_EVENTO_DA a DATA_EVENTO_A)
        public int OCCORRENZE { set; get; }
        // dall'inizio del calcolo
        //public int OCCORRENZE_TOT { set; get; }
        // media delle occorrenze nel periodo dall'inizio del calcolo
        //public float OCCORRENZE_MEDIA_INTERVALLO_TOT { set; get; }
        // media delle occorrenze per estrazione nel periodo
        public float OCCORRENZE_MEDIA_ESTRAZIONE { set; get; }
        // variazione dal periodo prec di occorrenze o num estrazioni (utili per raggiungere le occorenze)
        public float VARIAZIONE_OCC_TOPREC { set; get; }
        public float VARIAZIONE_NUMESTR_TOPREC { set; get; }
        // variazione dalla media di occorrenze o num estrazioni (utili per raggiungere le occorenze)
        //public float VARIAZIONE_TOMEDIA { set; get; }
        public int ID_ESTR_DA { set; get; }
        public int ID_ESTR_A { set; get; }
        public int NUM_ESTRAZIONI { set; get; }
        public int NUM_ESTRATTI { set; get; }
        public float PERC_OCCORRENZE { get; set; }

        // media delle estrazioni per avere tot occorrenze dall'inizio del calcolo
        //public float NUM_ESTRAZIONI_MEDIA_INTERVALLO_TOT { set; get; }
        //public int NUM_ESTRAZIONI_TOT { set; get; }

        public int ID_PARAMETRI_CALCOLO { set; get; }
        // baricentro nel tabellone X Y: sommo le pos delle occorrenze (x,y) e divido per il numero di occorrenze
        // per trovare le coordinate della media di tutte le occorrenze sul tabellone
        public float BARICENTRO_X { set; get; }
        public float BARICENTRO_Y { set; get; }
        // è la lunghezza del segmento che unisce due posizioni di baricentro consecutive, in valore assoluto
        // dà indicazioni suol movimento del baricentro rispetto a quello calcolato nell'intervallo temporale precedente
        public float BAR_BARPREC_LEN_ABS { set; get; }
        // è LA DISTANZA DAL BARICENTRO ASSOLUTO DELLA MATRICE
        public float BARICENTRO_X_MATRIX { set; get; }
        public float BARICENTRO_Y_MATRIX { set; get; }
        public float BAR_BARMATRIX_LEN_ABS { set; get; }
        public string DIREZIONE_SPOSTAMENTO_BARICENTRO_X { set; get; }
        public string DIREZIONE_SPOSTAMENTO_BARICENTRO_Y { set; get; }
        public string DIREZIONE_SPOSTAMENTO_BARICENTRO_X_DA_MATRIX { set; get; }
        public string DIREZIONE_SPOSTAMENTO_BARICENTRO_Y_DA_MATRIX { set; get; }
        public float DELTA_X_BAR_PREC { set; get; }
        public float DELTA_Y_BAR_PREC { set; get; }
        public float DELTA_X_BAR_MATRIX { set; get; }
        public float DELTA_Y_BAR_MATRIX { set; get; }
        public int OCC_BA { set; get; }
        public int OCC_CA { set; get; }
        public int OCC_FI { set; get; }
        public int OCC_GE { set; get; }
        public int OCC_MI { set; get; }
        public int OCC_NA { set; get; }
        public int OCC_PA { set; get; }
        public int OCC_RM { set; get; }
        public int OCC_TO { set; get; }
        public int OCC_VE { set; get; }
        public int OCC_NZ { set; get; }

        public PSD_RES_RUOTA BA { set; get; }
        public PSD_RES_RUOTA CA { set; get; }
        public PSD_RES_RUOTA FI { set; get; }
        public PSD_RES_RUOTA GE { set; get; }
        public PSD_RES_RUOTA MI { set; get; }
        public PSD_RES_RUOTA NA { set; get; }
        public PSD_RES_RUOTA PA { set; get; }
        public PSD_RES_RUOTA RM { set; get; }
        public PSD_RES_RUOTA TO { set; get; }
        public PSD_RES_RUOTA VE { set; get; }
        public PSD_RES_RUOTA NZ { set; get; }
    }

    /// <summary>
    /// Classe che rappresenta una sintesi dei risultati per calcolo fra due matrici M1 M2, dove M1 è la matrice delle stelle. 
    /// Serve per tarare i parametri, e quindi per scegliere l'indice di saturazione da tenere in considerazione, assieme alla massima distanza 
    /// di saturazione per identificare gruppi omogenei per ogni matrice. 
    /// Dovrei cercare in ogni calcolo, cioè in ogni confronto fra due matrici, un numero costante di gruppi di saturazione in base ai parametri,
    /// in modo da poter prevedere in ogni calcolo un numero di gruppi di saturazione. 
    /// </summary>
    public class PSD_RES_TOT_SAT// : PSD_RES_TOT
    {
        public DateTime DATA_EVENTO_DA_M1 { set; get; }
        public DateTime DATA_EVENTO_A_M1 { set; get; }
        public DateTime DATA_EVENTO_DA_M2 { set; get; }
        public DateTime DATA_EVENTO_A_M2 { set; get; }
        public string ID_CALC { set; get; } // identifica il calcolo: potrebbe essere l'intervallo di date delle matrici coinvolte
        public int NUM_CALC { set; get; } // numero del calcolo: progressivo che identifica la posizione nella sequenza (pocointeressante)
        public int ID_MATRIX_1 { set; get; } // id della matrice 1 coinvolta
        public int ID_MATRIX_2 { set; get; }
        public int NUM_GRPSAT { set; get; } // numero di gruppi di saturazione
        public float PERC_GRPSAT_STARS { set; get; } // percentuale di gruppi di saturazione rispetto al numero di stelle
        public int NUM_PLANETS_IN_GRPSAT_STARS { set; get; } // numero totale di stelle e pianeti coinvolti
        public int NUM_PLANETS_IN_GRPSAT_DISTINCT { set; get; } // numero di stelle e pianeti diversi coinvolti
        public int TOT_STARS { set; get; } // num totale di STELLE della matrice1, anche le non sature 
        public int TOT_PLANETS { set; get; } // num totale di pianeti della matrice2, anche quelli non coinvolti 
        public float PERC_PLANETS_IN_GRPSAT_DISTINCT_TOT_PLANETS { set; get; } // percentuiale di pianeti distinti coinvolti rispetto al totale dei pianeti della metrice2
        public int NUM_PLANETS_IN_GRPSAT_COMMON { set; get; } //numero di pianeti e stelle comuni
        public int NUM_PLANETS_IN_GRPSAT { set; get; } // numero di pianeti totali delle stelle sature 

        public int NUM_GRPSAT_LESSTHANSAT_TO_POSY { set; get; } // numero di gruppi di saturazione calcolati fino a posY
        public float PERC_GRPSAT_LESSTHANSAT_TO_POSY_STARS { set; get; }  // percentuale di gruppi di saturazione fino a posY rispetto al numero di stelle

        public int NUM_GRPSAT_GREATERTHANSAT_TO_POSY { set; get; }
        public float PERC_GRPSAT_GREATERTHANSAT_TO_POSY_STARS { set; get; }
        // percentuale stelle(segnali) con valore di saturazione fino a pos y e che hanno entro dist saturazione un pianeta
        // serve per capire quanti egnali vanno buon fine
        public float PERC_GRPSAT_GREATERTHANSAT_TO_POSY_WITH_PLANETS_DISTSAT_AFTERY { set; get; }

        public int NUM_PLANETS_FROM_POSY_TO_DISTSAT_GROUPCOLUMN { set; get; }
        public int NUM_PLANETS_FROM_POSY_TO_DIST_GROUPCOLUMN { set; get; } //numero di pianeti dopo la stella (temporalmemnte) ad una distanza Y fino a 3 e all'interno del gruppo di colonne
        public int NUM_PLANETS_FROM_POSY_TO_DIST { set; get; } // numero di pianeti dopo la stella (temporalmemnte) ad una distanza X fino a 4 e posY al max di distanza di saturazione
        public int NUM_PLANETS_FROM_POSY_TO_DISTSAT { set; get; } // numero di pianeti dopo la stella (temporalmemnte) ad una distanza al max di dist saturazione

        public int NUM_PLANETS_GREATERTHANSAT_FROM_POSY_TO_DISTSAT { set; get; } // numero di pianeti dopo la stella (temporalmemnte) ad una distanza al max di dist saturazione
        public int NUM_PLANETS_GREATERTHANSAT_FROM_POSY_TO_DISTSAT_GROUPCOLUMN { set; get; }

        public int NUM_PLANETS_GREATERTHANSAT_FROM_POSY_TO_DIST_GROUPCOLUMN { set; get; } //numero di pianeti dopo la stella (temporalmemnte) ad una distanza Y fino a 3 e all'interno del gruppo di colonne
        public int NUM_PLANETS_GREATERTHANSAT_FROM_POSY_TO_DIST { set; get; } // numero di pianeti dopo la stella (temporalmemnte) ad una distanza X fino a 4 e posY al max di distanza di saturazione

        // PERCENTUALE DI PIANETI presenti dopo la stella satura (con saturazione calcolata fino alla sua comparsa Y con soglia <) rispetto alle stelle sature (segnali)
        public float PERC_PLANETS_FROM_POSY_TO_DISTSAT_STARS { set; get; }
        public float PERC_PLANETS_FROM_POSY_TO_DISTSAT_STARS_GROUPCOLUMN { set; get; }
        // PERCENTUALE DI PIANETI presenti dopo la stella satura (con saturazione calcolata fino alla sua comparsa Y con soglia >=) rispetto alle stelle sature (segnali)
        public float PERC_PLANETS_GREATERTHANSAT_FROM_POSY_TO_DISTSAT_STARS { set; get; }
        public float PERC_PLANETS_GREATERTHANSAT_FROM_POSY_TO_DISTSAT_STARS_GROUPCOLUMN { set; get; }

        public float PERC_PLANETS_FROM_POSY_TO_DIST_STARS { set; get; }
        public float PERC_PLANETS_FROM_POSY_TO_DIST_STARS_GROUPCOLUMN { set; get; }
        public float PERC_PLANETS_GREATERTHANSAT_FROM_POSY_TO_DIST_STARS { set; get; }
        public float PERC_PLANETS_GREATERTHANSAT_FROM_POSY_TO_DIST_STARS_GROUPCOLUMN { set; get; }

        //public float PERC_PLANETS_FROM_POSY_TO_DIST { set; get; }
        //PERC_PLANETS_FROM_POSY_STARS
        //public float PERC_PLANETS_GREATERTHANSAT_FROM_POSY_TO_DIST { set; get; }
    }

    /// <summary>
    /// NON USATA PER ORA (VEDI PSD_MATH_STAR_SYSTEM) : Struttura contenente i risultati dei calcoli delle distanze fra un numero in pos x y, e una lista di numeri in posizioni diverse
    /// </summary>
    public class PSD_MATH_STAR_SYSTEM_RECURSIVE
    {
       
        public PSD_MATH_PLANET_NUM STARNUM { set; get; }

        // lista dei numeri confrontati con l'attuale => item 1: X, item2: Y, item 3: num
        public List<PSD_MATH_PLANET_NUM> PLANETS { set; get; }
        // lista delle distanze (posx + posy) dei pianeti dal numero attuale
        //public List<int> DISTANCES { set; get; }

        /// <summary>
        /// la distanza calcolata in maniera diversa: traduco la griglia con al centro il numero con 
        /// un sistema a stella con al centro il numero e una serie di orbite che ospitano i numeri da confrontare
        /// </summary>
        //public List<int> DISTANCES_ORBIT { set; get; }

        /// <summary>
        /// Considero la posizione convenzionalemte ad un orologio: la sposizione 0 è in alto (posizione delle 12 sull'orologio)
        ///  - sulla prima orbita (distanza 1) posso avere al max 8 pianeti (lato 3: 1+2)*2 + (1+2-2)*2 => max 8 ore/posizioni
        ///  - sulla seconda (distanza 2) 16: lato*2+(lato-2)*2 = (lato prec (3) + 2) * 2 + lato prec(3) * 2 
        ///  - sulla terza 24  = 7*2 + 5*2 ..
        /// </summary>
        //public List<int> POSITION_ORBIT { set; get; }

        public List<Tuple<int,int>> Get_GROUP_DISTANCES()
        {
            List<Tuple<int,int>> res = new List<Tuple<int, int>>();

            var group = this.PLANETS.GroupBy(X=>X.DISTANCE);

            foreach (var dist in group)
            {
                res.Add(new Tuple<int,int>( dist.Key, dist.Count()));
            }
            return res;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Tuple<int, int>> Get_GROUP_DISTANCES_ORBIT()
        {
            List<Tuple<int, int>> res = new List<Tuple<int, int>>();

            var group = this.PLANETS.GroupBy(X => X.DISTANCE_ORBIT);

            foreach (var dist in group)
            {
                res.Add(new Tuple<int, int>(dist.Key, dist.Count()));
            }
            return res;
        }

        
    }

    /// <summary>
    /// Numero del tabellone / matrice identificato dalla sua posizione nella matrice stessa.
    /// Per riconoscerne la POSIZIONE FACILMENTE ho i campi  GROUPROW e GROUPCOL che aiutano a capirne la posizione.
    /// </summary>
    public class PSD_MATH_MATRIX_NUM
    {
        public string ID { set; get; }
        public int NUM { set; get; }
        public int POSX { set; get; }
        public int POSY { set; get; }
        public float VAL_SAT { set; get; }
        public DateTime DATENUM { set; get; }
        // serve per i dentificare un gruppo, che nel caso specifico è IL PERIODO di estrazione definita nella riga dell'insieme di estrazioni
        public string GROUPROW { set; get; }
        // serve per i dentificare un gruppo, che nel caso specifico è la RUOTA definita nella colonna dell'estrazione
        public string GROUPCOL { set; get; }
        // identifica la posizione nel gruppo di colonne sulla matrice (per esempio in BA il quinto estratto è BA5)
        public string POSGROUPCOL { set; get; }
        // nome della matrice di appartenenza
        public string STAR_SYSTEM_MATRIX_NAME { set; get; }
    }

    public class PSD_MATH_PLANET_NUM: PSD_MATH_MATRIX_NUM
    {
        // definiscono le distanze dal numero di origine, se quello rappresentato fa parte della matrice risultante
        public int DISTANCE { set; get; }
        public int DISTANCE_ORBIT { set; get; }
        public int POSITION_ORBIT { set; get; }
        // INDICA LA STAR  a cui è collegato il pianeta satellite
        public PSD_MATH_MATRIX_NUM STAR_NUM { set; get; }
    }

    /// <summary>
    /// Struttura usata per mostrare i risultati per ogni numero della matrice di origine. Contiene un numero identificato del valore della pos x e y sull matrice.
    /// Il numero è associato ad una matrice (lista di pianeti del suo sistema) rappresentata da una lista di numeri con valori di distanze dal numero STARNUM.  
    /// </summary>
    public class PSD_MATH_STAR_SYSTEM_RES
    {
        public string ID
        {
            get
            {
                return POSGROUPCOL + "-" + POSY.ToString().PadLeft(3, '0');
            }
        }
        public int STARNUM { set; get; }
        public int POSX { set; get; }
        public int POSY { set; get; }
        public DateTime DATENUM { set; get; }
        // serve per i dentificare un gruppo, che nel caso specifico è IL PERIODO di estrazione definita nella riga dell'insieme di estrazioni
        public string GROUPROW { set; get; }
        // serve per i dentificare un gruppo, che nel caso specifico è la RUOTA definita nella colonna dell'estrazione
        public string GROUPCOL { set; get; }
        // identifica la posizione nel gruppo di colonne sulla matrice (per esempio in BA il quinto estratto è BA5)
        public string POSGROUPCOL
        {
            get
            {
                //string res = GROUPCOL;
                //if(POSX>0) res = GROUPCOL + ((POSX % 5 >0)?(POSX % 5):5).ToString();
                return GROUPCOL + ((POSX % 5 > 0) ? (POSX % 5) : 5).ToString();
            }
        }
        // nome della matrice di appartenenza
        public string STAR_SYSTEM_MATRIX_NAME { set; get; }
        // lista dei numeri confrontati con l'attuale => item 1: X, item2: Y, item 3: num
        public List<PSD_MATH_PLANET_NUM> PLANETS { set; get; }

        public List<Tuple<int, int>> GROUP_DISTANCES_ORBIT
        {
            get { return Get_GROUP_DISTANCES_ORBIT(); }
        }
        // lista con item1 = valore distanza, item2 = numero di pianeti a distanza item1
        private List<Tuple<int, int>> Get_GROUP_DISTANCES_ORBIT()
        {
            List<Tuple<int, int>> res = new List<Tuple<int, int>>();

            var group = this.PLANETS.GroupBy(X => X.DISTANCE_ORBIT);

            foreach (var dist in group)
            {
                res.Add(new Tuple<int, int>(dist.Key, dist.Count()));
            }
            return res;
        }

        public int MIN_DIST_PLANETS
        {
            get
            {
                return GROUP_DISTANCES_ORBIT.Min(X => X.Item1);
            }
        }
        public int Get_DIST_PLANETS_LEVEL(int idxlev)
        {
            if (GROUP_DISTANCES_ORBIT.Count > idxlev)
                return GROUP_DISTANCES_ORBIT.OrderBy(X => X.Item1).ToArray()[idxlev].Item1;
            else return -1;

        }
        public int Get_NUM_PLANETS_LEVEL(int idxlev)
        {
            if (GROUP_DISTANCES_ORBIT.Count > idxlev)
                return GROUP_DISTANCES_ORBIT.OrderBy(X => X.Item1).ToArray()[idxlev].Item2;
            else return -1;

        }
        public float MED_DIST_PLANETS
        {
            get
            {
                float res = 0;
                foreach (Tuple<int, int> t in GROUP_DISTANCES_ORBIT)
                {
                    res = res + t.Item1 * t.Item2;
                }
                if (res > 0)
                    res = res / GROUP_DISTANCES_ORBIT.Sum(X => X.Item2);

                return res;
            }
        }
        public int TOT_DIST_PLANETS
        {
            get
            {
                int res = 0;
                foreach (Tuple<int, int> t in GROUP_DISTANCES_ORBIT)
                {
                    res = res + t.Item1 * t.Item2;
                }
                return res;
            }
        }

        public int NUM_PLANETS_MIN_DIST
        {
            get { return Get_PLANETS_MIN_DIST().Count; }
        }

        public int NUM_PLANETS_DIST_SATURAZIONE_1
        {
            get { return Get_PLANETS_TO_DIST(MAXDIST_TO_CALC_SATURAZIONE).Count; }
        }
        public int NUM_PLANETS_DIST_SATURAZIONE_2
        {
            get { return Get_PLANETS_TO_DIST(MAXDIST_TO_CALC_SATURAZIONE_2).Count; }
        }
        public int NUM_PLANETS_DIST_SATURAZIONE_3
        {
            get { return Get_PLANETS_TO_DIST(MAXDIST_TO_CALC_SATURAZIONE_3).Count; }
        }

        public List<PSD_MATH_PLANET_NUM> Get_PLANETS_MIN_DIST()
        {
            return this.PLANETS.Where(X => X.DISTANCE_ORBIT == MIN_DIST_PLANETS).ToList();
        }

        public List<PSD_MATH_PLANET_NUM> Get_PLANETS_TO_DIST(int maxdist)
        {
            return this.PLANETS.Where(X => X.DISTANCE_ORBIT <= maxdist).ToList();
        }

        public int NUM_PLANETS_MIN_DIST_AFTER_CALC { set; get; }
        public bool EXISTS_COPPIA_UNIVOCA_AFTER_CALC { set; get; }

        // indicatore di saturazione: maggiori sono i pianeti vicini maggiore è il valore
        public float INDICE_SATURAZIONE_MIN_DIST
        {
            get {
                int mindist = MIN_DIST_PLANETS;
                float res = 0;
                if (mindist >= 0)
                    res = (float)NUM_PLANETS_MIN_DIST / (mindist + 1);
                return res;
            }
        }
        //Get_MIN_DIST_PLANETS_LEVEL
        public int MAXDIST_TO_CALC_SATURAZIONE;
        public float INDICE_SATURAZIONE_TO_DIST
        {
            get { return Get_INDICE_SATURAZIONE_TO_DIST(MAXDIST_TO_CALC_SATURAZIONE); }
        }

        public float INDICE_SATURAZIONE_TO_DIST_TO_POSY
        {
            get { return Get_INDICE_SATURAZIONE_TO_DIST_TO_POSY(MAXDIST_TO_CALC_SATURAZIONE); }
        }

        public int MAXDIST_TO_CALC_SATURAZIONE_2;
        public float INDICE_SATURAZIONE_TO_DIST_2
        {
            get { return Get_INDICE_SATURAZIONE_TO_DIST(MAXDIST_TO_CALC_SATURAZIONE_2); }
        }

        public int MAXDIST_TO_CALC_SATURAZIONE_3;
        public float INDICE_SATURAZIONE_TO_DIST_3
        {
            get { return Get_INDICE_SATURAZIONE_TO_DIST(MAXDIST_TO_CALC_SATURAZIONE_3); }
        }

        public float Get_INDICE_SATURAZIONE_TO_DIST(int maxdist)
        {
            float res = 0;
            for (int dist = 0; dist <= maxdist; dist++)
            {
                int numplanets = this.PLANETS.Where(X => X.DISTANCE_ORBIT == dist).Count();
                res = res + (float) numplanets / (dist+1);
            }
            return res;
        }

        public float Get_INDICE_SATURAZIONE_TO_DIST_TO_POSY(int maxdist)
        {
            float res = 0;
            for (int dist = 0; dist <= maxdist; dist++)
            {
                int numplanets = this.PLANETS.Where(X => X.DISTANCE_ORBIT == dist && X.POSY<= this.POSY).Count();
                res = res + (float)numplanets / (dist + 1);
            }
            return res;
        }


        #region INDICI DI SATURAZIONE: indicano un valore di concentrazione di pianeti attorno ad una stella per i livelli 1 2 3 
        public float INDICE_SATURAZIONE_TO_LEVEL1
        {
            get { return Get_INDICE_SATURAZIONE_TO_LEVEL(1); }
        }
        public float INDICE_SATURAZIONE_TO_LEVEL2
        {
            get { return Get_INDICE_SATURAZIONE_TO_LEVEL(2); }
        }
        public float INDICE_SATURAZIONE_TO_LEVEL3
        {
            get { return Get_INDICE_SATURAZIONE_TO_LEVEL(3); }
        }
        // indicatore di saturazione: maggiori sono i pianeti vicini maggiore è il valore
        public float Get_INDICE_SATURAZIONE_TO_LEVEL(int maxlevels)
        {
            float res = 0;
            for (int lev = 1; lev <= maxlevels; lev++)
            {
                int dist = Get_DIST_PLANETS_LEVEL(lev);
                int numplanets = Get_NUM_PLANETS_LEVEL(lev);

                res = res + (float) numplanets / dist;
            }
            return res;
        }
  
        #endregion
    }

    public class PSD_MATH_STAR_SYSTEM_RES_PREV: PSD_MATH_STAR_SYSTEM_RES
    {
        public int NUM_PLANETS_FROM_POSX4_POSY3
        {
            get { return Get_NUM_PLANETS_FROM_POSX_POSY(3, 4); }
        }

        /// <summary>
        /// Restituisce il numero di pianeti nei dintorni dopo la POSY, enrto tre estrazioni successive e nella ruota
        /// </summary>
        public int NUM_PLANETS_FROM_GROUPCOLUMN_POSY3
        {
            get
            {
                //int posxinruota = Convert.ToInt32(this.POSGROUPCOL.Substring(2));
                int posingroupcolumn = this.POSX % 5;
                if (posingroupcolumn == 0) posingroupcolumn = 5;
                return this.PLANETS.Where(X => X.POSY > this.POSY && X.POSY - this.POSY < 3 && (this.POSX - X.POSX < posingroupcolumn || X.POSX - this.POSX <= 5 - posingroupcolumn)).Count();
            }
        }

        /// <summary>
        /// Restituisce il numero di pianeti nei dintorni dopo la POSY, entro la distanza di saturazione in verticale (anche se la distanza dim saturazione comprende la somma di distanza X e Y) 
        /// e nella ruota
        /// </summary>
        public int NUM_PLANETS_FROM_GROUPCOLUMN_POSYDISTSAT
        {
            get
            {
                //int posxinruota = Convert.ToInt32(this.POSGROUPCOL.Substring(2));
                int posingroupcolumn = this.POSX % 5;
                if (posingroupcolumn == 0) posingroupcolumn = 5;
                return this.PLANETS.Where(X => X.POSY > this.POSY && X.POSY - this.POSY < this.MAXDIST_TO_CALC_SATURAZIONE && (this.POSX - X.POSX < posingroupcolumn || X.POSX - this.POSX <= 5 - posingroupcolumn)).Count();
            }
        }
        /// <summary>
        /// Restituisce il numero di pianeti nei dintorni dopo la POSY, entro la distanza di saturazione del pianeta rispetto alla stella
        /// e nella ruota
        /// </summary>
        public int NUM_PLANETS_FROM_POSXY_TO_MAXDISTSAT
        {
            get
            {
                //int posxinruota = Convert.ToInt32(this.POSGROUPCOL.Substring(2));
                int posingroupcolumn = this.POSX % 5;
                if (posingroupcolumn == 0) posingroupcolumn = 5;
                return this.PLANETS.Where(X => X.POSY > this.POSY && X.DISTANCE_ORBIT <= this.MAXDIST_TO_CALC_SATURAZIONE).Count();
            }
        }

        /// <summary>
        /// Restituisce il numero dei pianeti presenti oltre la coordinata Y (quindi estratti dopo alla stella, rispetto alla matrice).
        /// Ovviamente i pianeti vengono estratti successivamente ma in base alla posizione della stella riesco a determinare un certo indice di saturazione fino ad un certo y.
        /// Serve per capire se dopo un certo Y (pos Y della stella) posso prevedere la presenza di pianeti in modo proficuo.
        /// In altre parole, se ho un certo numero di pianeti confermato ad un cetro punto dalla presenza di una stella, posso dire che con una buona
        /// probabilità avrò altri pianeti successivamente?
        /// </summary>
        /// <param name="maxdistY"></param>
        /// <param name="maxdistX"></param>
        /// <returns></returns>
        public int Get_NUM_PLANETS_FROM_POSX_POSY(int maxdistY, int maxdistX)
        {
            return this.PLANETS.Where(X => X.POSY > this.POSY && X.POSY - this.POSY < maxdistY && Math.Abs(X.POSX - this.POSX) < maxdistX).Count();
        }

        public float Get_INDICE_SATURAZIONE_TO_POSY(int maxdistsat)
        {
            float res = 0;
            for (int dist = 0; dist <= maxdistsat; dist++)
            {
                int numplanets = this.PLANETS.Where(X => X.DISTANCE_ORBIT == dist && X.POSY <= this.POSY).Count();
                res = res + (float)numplanets / (dist + 1);
            }
            return res;

        }
    }

    // Tabella del db
    public class PSD_GRPSAT
    {
        // codice: campo libero
        public string CODE_GRP { set; get; }
        // identificativo univoco del gruppo
        public string ID { set; get; }
        // chgiave numerica interna
        public int ID_GRPSAT { set; get; }
        public int POSX { set; get; }
        public int POSY { set; get; }
        public float IDX_SAT { set; get; }
        public int LEVEL { set; get; }
        // DISTANZA MASSIMA su cui è calcolata la saturazione
        public int MAX_SAT_DIST { set; get; }
        public int DIST_PREV_GRP { set; get; }
        public int ID_MATRIX { set; get; }
        // NUMERO DI PIANETI entro la distanza di saturazioone
        public int NUM_PLANETS_TO_SAT_DIST { set; get; }
    }

    public class PSD_GRPSAT_ITEM: PSD_GRPSAT
    {
        public bool IS_STAR{ set; get; }
        public string ID_ITEM{ set; get; }

    }

    public class PSD_MATRIX
    {
        public string ID { set; get; }
        public int ID_MATRIX { set; get; }
        // corrisponde al numero di estrazioni
        public int NUM_ROWS { set; get; }
        public int NUM_COLS { set; get; }
        public DateTime DATE_BEGIN{ set; get; }
        public DateTime DATE_END { set; get; }
        public int OCCORRENZE { set; get; }
        // indica il livello di sovrapposizione dei gruppi di sovrapposizione di matrici differenti
        public int LEVEL { set; get; }

    }

    public class PSD_MATH_STAR_SYSTEM_MIN_DIST_RES
    {
        public PSD_MATH_MATRIX_NUM STARNUM { set; get; }
        public PSD_MATH_PLANET_NUM PLANET { set; get; }
        public bool IS_COPPIA_UNIVOCA { set; get; }
        // CI SAREBBE QUESTA INFO IN PLANET, ma per comodità la inserisco anche fuori dato che comunque un pianeta può essere assocato a tutte le stelle
        // e questo crea confuzione
        // NB: ho sostituito questa info al posto di INDEX_COPPIA_MIGLIO_DISTANZA, che non ha trovato applicazione
        public int DISTANCE_ORBIT { set; get; }
        public int NUM_CALC { set; get; }
    }

    public class PSD_MATH_STAR_SYSTEM_MIN_DIST_RES_GRID
    {
        public int STARNUM_NUM { set; get; }
        // identifica la posizione nel gruppo di colonne sulla matrice (per esempio in BA il quinto estratto è BA5)
        public string STARNUM_POSGROUPCOL { set; get; }
        public string STARNUM_ID { set; get; }
        public int STARNUM_POSX { set; get; }
        public int STARNUM_POSY { set; get; }
        public DateTime STARNUM_DATENUM { set; get; }
        // identifica la posizione nel gruppo di colonne sulla matrice (per esempio in BA il quinto estratto è BA5)
        public string PLANET_POSGROUPCOL { set; get; }
        public string PLANET_ID { set; get; }
        public int PLANET_NUM { set; get; }
        public int PLANET_POSX { set; get; }
        public int PLANET_POSY { set; get; }
        public DateTime PLANET_DATENUM { set; get; }
        public int DISTANCE { set; get; }
        public int DISTANCE_ORBIT { set; get; }
        public bool IS_COPPIA_UNIVOCA { set; get; }
        //public int NUM_COPPIE_NON_UNIVOCHE { set; get; }
        public int INDEX_COPPIA_MIGLIOR_DISTANZA { set; get; }
        public int NUM_CALC { set; get; }

    }

    public class PSD_CALCS
    {
        public int IDCalcolo { set; get; }
        public string DESC_RES { set; get; }
        public string DESC_PARAMS_CALC { set; get; }
        public string DESC_PARAMS_SEGN { set; get; }
        public DateTime DATE_BEGIN { set; get; }
        public DateTime DATE_END { set; get; }
        public List<PSD_RES_TOT_SAT> LST_RES_TOT { set; get; }
        public List<PSD_ESTR_SEGNALI> LST_SEGNALI { set; get; }
        public List<PSD_ESTR_SUCCESSI> LST_PREVISIONI { set; get; }
        // CONTIENE i parametri per esempio della regola 1
        public List<Tuple<string,object>> LST_PARAMETRI_CALCOLO { set; get; }
        public List<Tuple<string, object>> LST_PARAMETRI_SEGNALE { set; get; }
    }
}
