using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlackBoxCore.DataEntities
{
    public class DbActivity
    {
        public DateTime StartActivity { set; get; }
        public DateTime StopActivity { set; get; }
        public long IDActivity { set; get; }
        public int TotMinPreview { set; get; }
        public string TitleActivity { set; get; }
        public string TextActivity { set; get; }
        public List<DbActivityLog> ListLog { set; get; }
        public string TypeActivity { set; get; }
        public string Status { set; get; }
        public DateTime LastStartLogActivity { set; get; }
        public DateTime DateTimeInserted { set; get; }
        public DateTime DateTimeLastMod { set; get; }
        public int IDUser { set; get; }

        public int GetTotSecActivity()
        {
        int res = 0;
            if (this != null && this.IDActivity > 0 && this.ListLog!=null)
            {
                foreach (DbActivityLog t in this.ListLog)
                {
                    if (t.StopActivityLog > DateTime.MinValue)
                        res = res + Convert.ToInt32(t.StopActivityLog.Subtract(t.StartActivityLog).TotalSeconds);
                }
                if (this.ListLog.Count() > 0 && this.ListLog.Last().StopActivityLog == DateTime.MinValue)
                    res = res + Convert.ToInt32(DateTime.Now.Subtract(this.ListLog.Last().StartActivityLog).TotalSeconds);
            }
            return res;
        }
        
    }
    public class DbActivityLog
    {
        public long IDActivity { set; get; }
        public string TitleActivity { set; get; }
        public DateTime StartActivityLog { set; get; }
        public DateTime StopActivityLog { set; get; }   
        public string LogActivity { set; get; }
    }
    public class DbActivityLogTypeOp: DbActivityLog
    {
        //  D: deleted, U: update/insert
        public string TypeOperation { set; get; }
        public DateTime StartActivityLogKey { set; get; }
    }

    public class WPFActivity:DbActivity
    {
        public bool IsStarted { set; get; }
        public double PercProgress { set; get; }
        /*public int TOT_SEC_ACTIVITY
        {
            get
            {
                int res = 0;
                if (this != null && this.IDActivity > 0)
                {
                    foreach (DbActivityLog t in this.ListLog)
                    {
                        if (t.StopActivityLog > DateTime.MinValue)
                            res = res + Convert.ToInt32(t.StopActivityLog.Subtract(t.StartActivityLog).TotalSeconds);
                    }
                    if (this.ListLog.Count() > 0 && this.ListLog.Last().StopActivityLog == DateTime.MinValue)
                        res = res + Convert.ToInt32(DateTime.Now.Subtract(this.ListLog.Last().StartActivityLog).TotalSeconds);
                }
                return res;
            }
        }*/
    }
    public class DbActivityNote
    {
        public long IDActivity { set; get; }
        public long IDNote { set; get; }
    }
}
