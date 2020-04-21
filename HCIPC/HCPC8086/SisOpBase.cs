using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCPC8086
{
    abstract class SisOpBase : HCIPC.Integracao.SistemaOperacionalDoCompilador
    {
        public Compilador Compilador { get; set; }
        public Binario Binario { get; set; }
        public ArqBase Arquitetura { get; set; }
    }
}
