using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCPC8086
{
    class SaidaNE : SaidaBase
    {
        public override string ExtensaoPadrao => "exe";

        public override byte[] VincularBinarios(List<Binario> binarios)
        {
            throw new NotImplementedException();
        }
    }
}
