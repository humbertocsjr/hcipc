using System;
namespace HCIPC.Arvore
{
    public class NoTexto : No
    {
        public string Valor { get; set; }

        public NoTexto()
        {
            Valor = null;
        }

        protected override void Executar(ref EstadoExecucao estado)
        {
            //Retorna o valor guardado neste nó do código fonte
            estado.Valor = Valor;
        }
    }
}
