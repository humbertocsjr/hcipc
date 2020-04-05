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
            //Declara uma variável, armazenando um valor genérico dela (normalmente "0), para que seja comparado no futuro os tipos de variável
            estado[Nome] = ValorInicial;
        }
    }
}
