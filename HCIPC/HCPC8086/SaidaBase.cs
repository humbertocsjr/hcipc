using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCPC8086
{
    abstract class SaidaBase
    {
        public abstract string ExtensaoPadrao { get; }
        public abstract byte[] VincularBinarios(List<Binario> binarios);
    }
}
