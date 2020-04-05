using System;
using System.Collections.Generic;

namespace HCIPC.Arvore
{
    public class NoEscreva : No
    {
        public List<No> Argumentos { get; set; }
        public bool EnterAoFinal { get; set; }

        public NoEscreva()
        {
            EnterAoFinal = false;
            Argumentos = new List<No>();
        }

        protected override void Executar(ref EstadoExecucao estado)
        {
            foreach (var no in Argumentos)
            {
                estado.Valor = null;
                no.ExecutarNo(ref estado);
                if(estado.Valor != null) Console.Write(estado.Valor);
            }
            if (EnterAoFinal) Console.WriteLine();
            estado.Valor = null;
        }
    }
}
