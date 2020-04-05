using System;
namespace HCIPC.Arvore
{
    public class NoAtribuicao : No
    {
        public string VariavelDestino { get; set; }
        public No Conteudo { get; set; }

        public NoAtribuicao()
        {
        }

        protected override void Executar(ref EstadoExecucao estado)
        {
            //Executa o nó que determina o valor a ser atribuido
            Conteudo.ExecutarNo(ref estado);
            //Armazena o novo valor na variável
            //TODO: Emitir erro se os tipos forem incompatíveis
            estado[VariavelDestino] = estado.Valor;
        }
    }
}
