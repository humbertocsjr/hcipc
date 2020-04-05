using System;
namespace HCIPC.Arvore
{
    public class NoLeia : No
    {
        public string Nome { get; set; }

        public NoLeia()
        {
        }

        protected override void Executar(ref EstadoExecucao estado)
        {
            var dados = Console.ReadLine();
            if (estado[Nome] == null)
            {
                throw new Erro(this, "Variável '" + Nome + "' não existe");
            }
            else if (estado[Nome] is string)
            {
                estado.Valor = estado[Nome] = dados;
            }
            else if (estado[Nome] is decimal)
            {
                estado.Valor = estado[Nome] = decimal.Parse(dados);
            }
            else if (estado[Nome] is int)
            {
                estado.Valor = estado[Nome] = int.Parse(dados);
            }
            else
            {
                throw new Erro(this, "Variável '" + Nome + "' não contém um tipo compatível com o Leia");
            }
        }
    }
}
