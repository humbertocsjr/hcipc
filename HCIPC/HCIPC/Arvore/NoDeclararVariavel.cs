using System;
namespace HCIPC.Arvore
{
    public class NoDeclararVariavel : No
    {
        public object ValorInicial { get; set; }
        public string Nome { get; set; }

        public NoDeclararVariavel()
        {
        }

        protected override void Executar(ref EstadoExecucao estado)
        {
            estado[Nome] = ValorInicial;
        }
    }
}
