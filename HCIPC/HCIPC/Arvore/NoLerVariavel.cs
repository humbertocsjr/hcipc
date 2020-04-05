using System;
namespace HCIPC.Arvore
{
    public class NoLerVariavel : No
    {
        public string Nome { get; set; }

        public NoLerVariavel()
        {
        }

        protected override void Executar(ref EstadoExecucao estado)
        {
            estado.Valor = estado[Nome];
            if(estado[Nome] == null)
            {
                throw new Erro(this, "Variável '" + Nome + "' não encontrada.");
            }
        }
    }
}
