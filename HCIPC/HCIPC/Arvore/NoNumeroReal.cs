using System;
namespace HCIPC.Arvore
{
    public class NoNumeroReal : No
    {
        public decimal Valor { get; set; }

        public NoNumeroReal()
        {
            Valor = 0m;
        }

        protected override void Executar(ref EstadoExecucao estado)
        {
            estado.Valor = Valor;
        }
    }
}
