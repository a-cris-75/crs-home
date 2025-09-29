using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyBlackBoxCore.DataEntities
{
    public class DbNoteKey
    {
        public int IDNote { set; get; }
        public string Name { set; get; }
        public int Level { set; get; } // indica il livello di importanza della chiave: serve per la ricerca eventualmente
    }
}
