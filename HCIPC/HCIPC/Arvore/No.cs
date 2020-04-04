using System;
namespace HCIPC.Arvore
{
    public abstract class No
    {
        // Dados para depuração do código interpretado
        public Fonte Fonte { get; set; }
        public int FonteLinha { get; set; }
        public int FonteColuna { get; set; }

        public No()
        {
        }

        public void ExecutarNo(ref EstadoExecucao estado)
        {
            //TODO: Colocar aqui a parte de depuração esperando liberação da IDE
            estado.NoAtual = this;
            Executar(ref estado);
        }

        protected abstract void Executar(ref EstadoExecucao estado);
    }
}
