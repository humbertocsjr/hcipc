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
            var dados = estado.ES.Leia();
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
                decimal valor = 0m;
                if(decimal.TryParse(dados, out valor))
                {
                    estado.Valor = estado[Nome] = valor;
                }
                else
                {
                    throw new Erro(this, "Esperada inserção de valor numérico real pelo usuário");
                }
            }
            else if (estado[Nome] is int)
            {
                int valor = 0;
                if (int.TryParse(dados, out valor))
                {
                    estado.Valor = estado[Nome] = valor;
                }
                else
                {
                    throw new Erro(this, "Esperada inserção de valor numérico inteiro pelo usuário");
                }
            }
            else
            {
                throw new Erro(this, "Variável '" + Nome + "' não contém um tipo compatível com o Leia");
            }
        }
    }
}
