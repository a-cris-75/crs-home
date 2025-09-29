using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crs.Home.ApocalypsApp
{
    public static class COSTANTS
    {
        public const int TYPE_FILE_ESTRAZIONI_2_DATA_RUOTA_ESTR = 2;
        public const int TYPE_FILE_ESTRAZIONI_1_TUTTE_LE_RUOTE = 1;
        public const int TYPE_FILE_ESTRAZIONI_3 = 3;
        public static string[] RUOTE = { "BA", "CA", "FI", "GE", "MI", "NA", "PA", "RM", "TO", "VE", "NZ" };
        public static string[] RUOTA_NAZIONALE = { "RN", "NZ", "NAZIONALE" };
        public const int TIPO_FREQ_DIR1 = 1;
        public const int TIPO_FREQ_DIR2 = 2;
        public const int TIPO_FREQ_IND2 = 3;
        public const int TIPO_FREQ_IND1 = 4;
        public const int TIPO_EVIDENZA_1_OCCORRENZE = 1;
        public const int TIPO_EVIDENZA_2_DECINE_SCELTI = 2;
        public const int TIPO_EVIDENZA_3_TIPO_R1 = 3;
        public const int TIPO_EVIDENZA_4_NUM_SCELTI = 4;
        public const int TIPO_EVIDENZA_5_DECINE_ESTR = 5;
        public const int TYPE_RES_TOT_GRUOPBY_OCC = 1;
        public const int TYPE_RES_TOT_GRUOPBY_NUMESTR = 2;
        public static Dictionary<string,int> RUOTE_INT =  new Dictionary<string, int>
        {
	        {"BA", 1},
	        {"CA", 2},
	        {"FI", 3},
	        {"GE", 4},
	        {"MI", 5},
	        {"NA", 6},
	        {"PA", 7},
	        {"RM", 8},
	        {"TO", 9},
	        {"VE", 10},
            {"NZ", 11}
        };

    }
}
