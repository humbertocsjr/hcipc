using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCPC8086
{
    abstract class ArqBase : HCIPC.Integracao.ArquiteturaDoCompilador
    {
        public Compilador Compilador { get; set; }
        public Binario Binario { get; set; }
    }
}
