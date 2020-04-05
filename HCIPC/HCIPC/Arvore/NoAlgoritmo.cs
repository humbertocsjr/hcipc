using System;
using System.Collections.Generic;

namespace HCIPC.Arvore
{
    public class NoAlgoritmo : NoBlocoBase
    {
        public string Nome { get; set; }

        public NoAlgoritmo() : base()
        {
        }

        protected override void Executar(ref EstadoExecucao estado)
        {
            foreach (var no in Nos)
            {
                //TODO: Verificar se for do tipo VAR ou INICIO, executar, ignorando funcoes e procedimentos

                //Executa o comando na lista de comandos dentro do Algoritmo
                no.ExecutarNo(ref estado);
            }
        }
    }
}
