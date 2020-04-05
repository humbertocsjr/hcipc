using System;
namespace HCIPC.Arvore
{
    public class NoNumeroInteiro : No
    {
        public int Valor { get; set; }

        public NoNumeroInteiro()
        {
        }

        protected override void Executar(ref EstadoExecucao estado)
        {
            //Retorna o valor guardado neste nó do código fonte
            estado.Valor = Valor;
        }
    }
}
