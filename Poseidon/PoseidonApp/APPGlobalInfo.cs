using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRS.Library;
using PoseidonData.DataEntities;

namespace PoseidonApp
{
    public static class APPGlobalInfo
    {
        public static string CONNECTION_STRING;
        //public static string CONNECTION_STRING_TEST;
        public static string PROVIDER_NAME;
        //public static string PROVIDER_NAME_TEST;
        public static string IMPORT_FILE_PATH;
        public static string IMPORT_FILE_URL;
        public static string IMPORT_FILE_TYPE;
        public static DateTime DATA_INIZIO;
        public static DateTime DATA_FINE;
        public static string USER;
        //public static string DB_SECTION;

        public static void InitIniInfo()
        {
            try
            {
                string DB_SECTION = CRSIniFile.GetPropertyValue(APPConstants.SEC_APPL, APPConstants.PROP_DBSECTION);
                APPGlobalInfo.CONNECTION_STRING = CRSIniFile.GetConnectionStringFromIni(DB_SECTION);
                APPGlobalInfo.PROVIDER_NAME = CRSIniFile.GetProviderFromIni(DB_SECTION);
                            
                APPGlobalInfo.IMPORT_FILE_PATH = CRSIniFile.GetPropertyValue(APPConstants.SEC_FILE_IMPORT, APPConstants.PROP_IMPORTFILENAME);
                APPGlobalInfo.IMPORT_FILE_URL = CRSIniFile.GetPropertyValue(APPConstants.SEC_FILE_IMPORT, APPConstants.PROP_IMPORTFILEWEBURL);
                APPGlobalInfo.IMPORT_FILE_TYPE = CRSIniFile.GetPropertyValue(APPConstants.SEC_FILE_IMPORT, APPConstants.PROP_IMPORTFILETYPE);
                APPGlobalInfo.USER= CRSIniFile.GetPropertyValue(APPConstants.SEC_APPL, APPConstants.PROP_USER);
                

                string sdt1 = CRSIniFile.GetPropertyValue(APPConstants.SEC_APP_PARAMS, APPConstants.PROP_DATAINIZIO);
                if(string.IsNullOrEmpty(sdt1))
                    sdt1 = DateTime.Now.AddDays(-90).ToShortDateString();
                string sdt2 = CRSIniFile.GetPropertyValue(APPConstants.SEC_APP_PARAMS, APPConstants.PROP_DATAFINE);
                if (string.IsNullOrEmpty(sdt2))
                    sdt2 = DateTime.Now.ToShortDateString();
                APPGlobalInfo.DATA_INIZIO = Convert.ToDateTime(sdt1);
                APPGlobalInfo.DATA_FINE = Convert.ToDateTime(sdt2);
            }
            catch { }
        }

        public static List<Tuple<string,object>> GetParamsCalc()
        {
            List<Tuple<string, object>> res = new List<Tuple<string, object>>();
            res.Add(new Tuple<string, object>("TYPE_BASE_MATRIX_INTERVAL", (TYPE_CALC_OCC0_ESTR1 == 0) ? "OCC" : "ESTR"));
            res.Add(new Tuple<string, object>("TYPE_BASE_DATA", (TYPE_CALC_R10_NUMSCELTI1 == 0) ? "REG 1" : "NUM SCELTI"));
            if (TYPE_CALC_R10_NUMSCELTI1 == 0)
            {
                res.Add(new Tuple<string, object>("R1_ACC_per_scelta_DEC", string.Join(",", PARAMETRI_R1.ACCOPPIATE_DEC)));
                res.Add(new Tuple<string, object>("R1_POSDEC", string.Join(",", PARAMETRI_R1.POS_DEC_R12)));
                res.Add(new Tuple<string, object>("R1_COLPI", string.Join(",", PARAMETRI_R1.COLPI)));
                res.Add(new Tuple<string, object>("R1_TIPOFREQ", string.Join(",", PARAMETRI_R1.TIPO_FREQUENZA)));              
            }
            else
            {
                foreach (KeyValuePair<string, List<int>> k in NUMERI_CASUALI)
                {
                    if (k.Value.Count > 0)
                    {
                        res.Add(new Tuple<string, object>("NUMERI SCELTI " + k.Key, string.Join(",", k.Value)));
                    }
                }
            } 
            return res;
        }
        public static string GetParamsDesc(List<Tuple<string, object>> paramsCalc)
        {
            string res = "";
            //List<Tuple<string, object>> paramsCalc = GetParamsCalc();
            foreach (Tuple<string,object> k in paramsCalc)
            {
                res = res + " - " + k.Item1 + ": " + k.Item2.ToString() + "\n";
            }
            return res;

        }

        public static List<Tuple<string, object>> GetParamsSegnSat()
        {
            List<Tuple<string, object>> res = new List<Tuple<string, object>>();
            res.Add(new Tuple<string, object>("MAX_DIST_ORBIT_SAT", PARAMETRI_SEGN_SAT.MAX_DIST_ORBIT_SAT));
            res.Add(new Tuple<string, object>("MIN_IDX_SAT", PARAMETRI_SEGN_SAT.MIN_IDX_SAT));
            res.Add(new Tuple<string, object>("MIN_IDX_SAT_TO_Y", PARAMETRI_SEGN_SAT.MIN_IDX_SAT_TO_Y));
            return res;
        }

        public static Dictionary<string, List<int>> NUMERI_CASUALI = new Dictionary<string, List<int>>();

        public static PSD_PARAMETRI_R1 PARAMETRI_R1 = new PSD_PARAMETRI_R1();
        public static List<PSD_ESTR_SUCCESSI> LST_SUCCESSI = new List<PSD_ESTR_SUCCESSI>();
        public static List<PSD_ESTR_LOTTO> LST_ESTRAZIONI = new List<PSD_ESTR_LOTTO>();
        public static List<PSD_CALCS> LST_CALCOLI = new List<PSD_CALCS>();
        public static int NUM_ESTR_GROUP = 35;
        public static int NUM_OCC_GROUP = 150;
        public static int TYPE_CALC_OCC0_ESTR1 = 0;
        public static int TYPE_CALC_R10_NUMSCELTI1 = 0;
        public static PSD_PARAMETRI_SEGN_SAT PARAMETRI_SEGN_SAT = new PSD_PARAMETRI_SEGN_SAT();
    }
}
