// /*
// * Copyright (c) 2020
// *      Humberto Costa dos Santos Junior.  All rights reserved.
// *
// * Redistribution and use in source and binary forms, with or without
// * modification, are permitted provided that the following conditions
// * are met:
// * 1. Redistributions of source code must retain the above copyright
// *    notice, this list of conditions and the following disclaimer.
// * 2. Redistributions in binary form must reproduce the above copyright
// *    notice, this list of conditions and the following disclaimer in the
// *    documentation and/or other materials provided with the distribution.
// * 3. All advertising materials mentioning features or use of this software
// *    must display the following acknowledgement:
// *      This product includes software developed by Humberto Costa dos Santos Junior and its contributors.
// * 4. Neither the name of the Humberto Costa dos Santos Junior nor the names 
// *    of its contributors may be used to endorse or promote products derived 
// *    from this software without specific prior written permission.
// *
// * THIS SOFTWARE IS PROVIDED BY THE REGENTS AND CONTRIBUTORS ``AS IS'' AND
// * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
// * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
// * ARE DISCLAIMED.  IN NO EVENT SHALL THE REGENTS OR CONTRIBUTORS BE LIABLE
// * FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
// * DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS
// * OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
// * HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
// * LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY
// * OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF
// * SUCH DAMAGE.
// */
using System;
using System.Collections.Generic;
using HCIPC.Arvore;

namespace HCIPC
{
    public class EstadoExecucao
    {
        //Algoritmo Atual
        public string Algoritmo { get; set; }
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
        //Rotinas publicas, [Nome Armazenado em Minusculas com o nome do Algoritmo na frente com um ponto separando]
        private static Dictionary<string, NoFuncaoProcedimento> Rotinas { get; set; }

        public EstadoExecucao()
        {
            if (Rotinas == null) Rotinas = new Dictionary<string, NoFuncaoProcedimento>();
            NivelGlobal = null;
            NivelAcima = null;
            Valor = null;
            Variaveis = new Dictionary<string, object>();
        }

        //Registra uma Função/Procedimento para ser possível de ser chamado posteriormente
        public void RegistrarRotinaLocal(string nome, NoFuncaoProcedimento rotina)
        {
            Rotinas.Add((Algoritmo + "." + nome).ToLower(), rotina);
        }


        //Executa uma rotina local (do mesmo algoritmo)
        public object ExecutarRotinaLocal(string nome, params object[] parametros)
        {
            return ExecutarRotina(Algoritmo, nome, parametros);
        }


        //Identifica um tipo nativo do .NET com o seu nome
        private string IdentificadorDeTipos(Type tipo)
        {
            switch (tipo.Name.ToLower())
            {
                case "decimal":
                    return "Real";
                case "int":
                    return "Inteiro";
                case "bool":
                    return "Lógico";
                case "string":
                    return "Caractere";
                default:
                    return "<DESCONHECIDO>";
            }
        }


        //Executa uma rotina, já preparado para uso futuro no caso de existirem bibliotecas externas
        public object ExecutarRotina(string algoritmo, string nome, params object[] parametros)
        {
            //Arruma o nome que deve buscar
            string nomeRotina = (algoritmo + "." + nome).ToLower();
            //Valor retornado pela função
            object retorno = null;
            //Caso exista a rotina, execute
            if(Rotinas.ContainsKey(nomeRotina))
            {
                //Monta um Ambiente de memoria proprio para este nivel
                //separando as variaveis locais das do nivel exatamente acima
                var exe = new EstadoExecucao()
                {
                    NivelAcima = this,
                    //Caso o nivel atual seja o global registra no novo nivel de execucao, como global
                    //TODO: Nao funciona com bibliotecas externas, teria que toda vez que carregar uma biblioteca externa executa-la para que sejam inicializadas as variaveis 
                    NivelGlobal = NivelGlobal == null ? this : NivelGlobal,
                    Interpretador = Interpretador,
                    Algoritmo = algoritmo
                };
                int i = 0;
                if(parametros.Length != Rotinas[nomeRotina].Parametros.Count)
                {
                    throw new Erro(NoAtual, "Quantidade de parametros fornecidos é diferente da quantidade esperada por essa rotina");
                }
                foreach (var par in Rotinas[nomeRotina].Parametros)
                {
                    exe[par.Key] = parametros[i];
                    if(parametros[i].GetType().FullName != par.Value.FullName)
                    {
                        if(parametros[i] is int & par.Value.FullName == typeof(decimal).FullName)
                        {
                            exe[par.Key] = (decimal)(int)parametros[i];
                        }
                        else if (parametros[i] is decimal & par.Value.FullName == typeof(int).FullName)
                        {
                            exe[par.Key] = (int)(decimal)parametros[i];
                        }
                        else
                        {
                            throw new Erro(NoAtual, "Parametro '" + par.Key + "' era esperado o tipo " + IdentificadorDeTipos(par.Value) + " porém foi fornecido um valor do tipo "+ IdentificadorDeTipos(parametros[i].GetType()));
                        }
                    }
                    i++;
                }
                Rotinas[nomeRotina].ExecutarRotina(ref exe);
                if(Rotinas[nomeRotina].RetornaValor)
                {
                    retorno = exe.Valor;
                }
            }
            else
            {
                throw new Erro(NoAtual, "Rotina '" + nome + "' do algoritmo '" + algoritmo + "' não foi encontrada. Verifique o nome ou se está referenciando corretamente.");
            }
            return retorno;
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
                else if(Variaveis.ContainsKey(nome.ToLower()))
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
                if(NivelGlobal != null && NivelGlobal[nome.ToLower()] != null)
                {
                    NivelGlobal[nome.ToLower()] = value;
                }
                else if(Variaveis.ContainsKey(nome.ToLower()))
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
