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
            //Recebe a entrada do usuário
            var dados = estado.ES.Leia();
            //Verifica se a variável destino existe
            if (estado[Nome] == null)
            {
                //Se não existir da erro
                throw new Erro(this, "Variável '" + Nome + "' não existe");
            }
            else if (estado[Nome] is string)
            {
                //Se for texto grava diretamente
                estado.Valor = estado[Nome] = dados;
            }
            else if (estado[Nome] is decimal)
            {
                //Se for ponto flutuante (Real), converte e armazena
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
                //Se for inteiro, converte e armazena
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
                //Nenhuma das anteriores, da erro porque não sabe converter
                throw new Erro(this, "Variável '" + Nome + "' não contém um tipo compatível com o Leia");
            }
        }
    }
}
