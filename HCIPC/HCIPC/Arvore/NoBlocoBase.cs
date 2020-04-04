using System;
using System.Collections.Generic;

namespace HCIPC.Arvore
{
    public abstract class NoBlocoBase : No
    {
        public List<No> Nos { get; set; }

        public NoBlocoBase()
        {
            Nos = new List<No>();
        }
    }
}
