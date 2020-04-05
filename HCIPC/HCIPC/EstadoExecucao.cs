using System;
using System.Collections.Generic;
using HCIPC.Arvore;

namespace HCIPC
{
    public class EstadoExecucao
    {
        //Acesso do Interpretador a partir do ambiente de execução
        public Interpretador Interpretador { get; set; }
        //Entrada e Saida de dados Padrão
        public EntradaSaida ES { get { return Interpretador.EntradaSaidaPadrao; } }
        //Local onde fica as variaveis globais
        public EstadoExecucao NivelGlobal { get; set; }
        //Nivel acima na hierarquia do EstadoExecucao, onde fica outros niveis da pilha de chamadas
        public EstadoExecucao NivelAcima { get; set; }
        //Nó sendo executado
        public No NoAtual { get; set; }

        //Valor de retorno genérico e geral, usado pelos nós para passar informação entre si
        public object Valor { get; set; }

        //Local onde fica as variáveis locais
        private Dictionary<string, object> Variaveis { get; set; }

        public EstadoExecucao()
        {
            NivelGlobal = null;
            NivelAcima = null;
            Valor = null;
            Variaveis = new Dictionary<string, object>();
        }

        public object this[string nome]
        {
            get
            {
                //Busca valor da variável no nivel global, e se não achar, busca no nivel local

                if (NivelGlobal != null && NivelGlobal[nome.ToLower()] != null)
                {
                    return NivelGlobal[nome.ToLower()];
                }
                else if(Variaveis.ContainsKey(nome))
                {
                    return Variaveis[nome.ToLower()];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                //Caso exista como variavel global, grava o valor, senão grava numa existente local, senão cria uma variável
                if(NivelGlobal!=null && NivelGlobal[nome] != null)
                {
                    NivelGlobal[nome] = value;
                }
                else if(Variaveis.ContainsKey(nome))
                {
                    Variaveis[nome.ToLower()] = value;
                }
                else
                {
                    Variaveis.Add(nome.ToLower(), value);
                }
            }
        }

    }
}
