using System;
namespace HCIPC.Arvore
{
    public abstract class No
    {
        // Dados para depuração do código interpretado
        public Fonte Fonte { get; set; }
        public int FontePosicao { get; set; }
        public int FonteLinha { get; set; }
        public int FonteColuna { get; set; }

        public No()
        {
        }

        // Comando chamado pelo Interpretador para Executar o comando deste nó
        public void ExecutarNo(ref EstadoExecucao estado)
        {
            try
            {
                //TODO: Colocar aqui a parte de depuração esperando liberação da IDE

                //Guarda o nó atual no Estado da Execução
                estado.NoAtual = this;
                //Executa o nó
                Executar(ref estado);
            }
            catch(Erro ex)
            {
                //Caso aconteça um erro conhecido (Do tipo Erro), passa adiante
                throw ex;
            }
            catch (Exception ex)
            {
                //Caso aconteça um erro desconhecido, recria no formato Erro
                throw new Erro(this, "Erro desconhecido: " + ex.ToString());
            }
        }

        //Comando implementado pelos Nós para executar o comando
        protected abstract void Executar(ref EstadoExecucao estado);
    }
}
