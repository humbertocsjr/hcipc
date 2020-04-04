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
            estado.Valor = Valor;
        }
    }
}
