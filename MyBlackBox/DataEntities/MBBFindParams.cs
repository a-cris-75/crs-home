using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyBlackBoxCore.DataEntities
{
    public class MBBFindParams
    {
        public DateTime Dt1 { set; get; }
        public DateTime Dt2 { set; get; }
        public int Rate { set; get; }
        public string SimbolRate { set; get; }
        public string Title { set; get; }
        public int IDUser { set; get; }
        public bool TypeFindAtLeastOne { set; get; }
        public bool OnlyFavorites { set; get; }
        
    }
}
