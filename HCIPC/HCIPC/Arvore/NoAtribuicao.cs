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
            Conteudo.ExecutarNo(ref estado);
            estado[VariavelDestino] = estado.Valor;
        }
    }
}
