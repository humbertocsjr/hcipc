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
using System.Linq;
using HCIPC.Arvore;

namespace HCIPC
{
    public class Fonte
    {
        private string _fonte = "";

        public List<int> PontosDeParada { get; set; }

        public string NomeDoArquivo { get; set; }
        public string CodigoFonte
        {
            get
            {
                return _fonte;
            }
            set
            {
                //Quando um código fonte é gravado, armazena as posições dos caracteres em referencia as linhas e colunas deles
                Posicoes.Clear();
                PosicoesLinhas.Clear();
                _fonte = value;
                int linha = 1;
                int coluna = 0;
                PosicoesLinhas.Add(1, 0);
                for (int i = 0; i < _fonte.Length; i++)
                {
                    if(_fonte[i] == '\n')
                    {
                        linha++;
                        coluna = 0;
                        PosicoesLinhas.Add(linha, i);
                    }
                    else if (_fonte[i] == '\r')
                    {
                    }
                    else
                    {
                        coluna++;
                    }
                    Posicoes.Add(i, new int[] { linha, coluna });
                }
                PosicoesLinhas.Add(linha + 1, _fonte.Length);
            }
        }
        //Armazena o indice de Posicões de linhas e colunas
        private Dictionary<int, int[]> Posicoes { get; set; }
        private Dictionary<int, int> PosicoesLinhas { get; set; }
        //Armazena posição atual e anterior, para andar pelo código fonte durante geração da arvore
        private int PosicaoLeitura { get; set; }
        private int PosicaoLeituraNoAnterior { get; set; }
        private int PosicaoLeituraNoAtual { get; set; }
        private int PosicaoLeituraTrechoAtual { get; set; }
        private int Linha { get; set; }
        private int Coluna { get; set; }
        private char Atual { get; set; }
        //Nó raiz deste codigo fonte, gerado pelo ProcessarCodigoFonte
        public No NoRaiz { get; set; }

        public Fonte()
        {
            PontosDeParada = new List<int>();
            PosicoesLinhas = new Dictionary<int, int>();
            Posicoes = new Dictionary<int, int[]>();
            PosicaoLeitura = 0;
            PosicaoLeituraNoAnterior = 0;
            PosicaoLeituraNoAtual = 0;
            PosicaoLeituraTrechoAtual = 0;
            NomeDoArquivo = "";
            CodigoFonte = "";
            Atual = ' '; // Necessario ser espaço para que o LerTrecho funcione corretamente
        }

        public string ExtrairLinha(int linha)
        {
            //Extrai uma linha do codigo fonte
            if (PosicoesLinhas.ContainsKey(linha))
            {
                return _fonte.Substring(PosicoesLinhas[linha], PosicoesLinhas[linha + 1] - PosicoesLinhas[linha]).Replace("\n","").Replace("\r","");
            }
            return null;
        }

        private char LerCaractere()
        {
            //Le e extrai próximo caractere, andando pelo codigo fonte
            char retorno = (char)0;
            if (CodigoFonte.Length > PosicaoLeitura)
            {
                retorno = CodigoFonte[PosicaoLeitura];
                PosicaoLeitura++;
            }
            Atual = retorno;
            return retorno;
        }

        private char PreverCaractere()
        {
            //Busca próximo caractere sem andar pelo código fonte
            char retorno = (char)0;
            if (CodigoFonte.Length > PosicaoLeitura)
            {
                retorno = CodigoFonte[PosicaoLeitura];
            }
            return retorno;
        }

        private void DesfazerUltimoLerTrecho()
        {
            //Volta um trecho de codigo fonte
            PosicaoLeitura = PosicaoLeituraNoAtual;
            Atual = CodigoFonte[PosicaoLeitura - 1];
        }

        private string LerTrecho()
        {
            //Extrai o proximo trecho (palavra, caractere, token)
            string trecho = "";
            // Pula Espacos em Branco
            while (new char[]{ ' ', '\t' }.Contains(Atual)) { LerCaractere(); }
            PosicaoLeituraTrechoAtual = PosicaoLeitura - 1;
            // Verifica o tipo de trecho
            switch (Atual)
            {
                case char a when ((a >= 'A' & a <= 'Z') | (a >= 'a' & a <= 'z')):
                    //Identificador
                    {
                        while
                            (
                                (Atual >= 'A' & Atual <= 'Z') |
                                (Atual >= 'a' & Atual <= 'z') |
                                (Atual >= '0' & Atual <= '9') |
                                Atual == '_' |
                                Atual == '.'
                            )
                        {
                            trecho += Atual;
                            LerCaractere();
                        }
                    }
                    break;
                case char a when ((a >= '0' & a <= '9') ):
                    //Numero
                    {
                        while
                            (
                                (Atual >= '0' & Atual <= '9') |
                                Atual == '.'
                            )
                        {
                            trecho += Atual;
                            LerCaractere();
                        }
                    }
                    break;
                case '"':
                    //Texto entre aspas
                    trecho += Atual;
                    LerCaractere();
                    while(Atual != '"')
                    {
                        if(Atual == '\\')
                        {
                            LerCaractere();
                        }
                        trecho += Atual;
                        LerCaractere();
                    }
                    LerCaractere();
                    break;
                case ':':
                    switch (PreverCaractere())
                    {
                        case '=':
                            //Caso seja ':=' armazena como um unico token
                            trecho += Atual;
                            LerCaractere();
                            trecho += Atual;
                            LerCaractere();
                            break;
                        default:
                            //Senão armazena o '=' sozinho
                            trecho += Atual;
                            LerCaractere();
                            break;
                    }
                    break;
                case '<':
                    switch (PreverCaractere())
                    {
                        case '-': //'<-'
                        case '=': //'<='
                            trecho += Atual;
                            LerCaractere();
                            trecho += Atual;
                            LerCaractere();
                            break;
                        default: // '<'
                            trecho += Atual;
                            LerCaractere();
                            break;
                    }
                    break;
                case '>':
                    switch (PreverCaractere())
                    {
                        case '='://'>='
                            trecho += Atual;
                            LerCaractere();
                            trecho += Atual;
                            LerCaractere();
                            break;
                        default://'>'
                            trecho += Atual;
                            LerCaractere();
                            break;
                    }
                    break;
                default:
                    //Armazena os caracteres que não entram nas regras anteriores, individualmente
                    trecho += Atual;
                    LerCaractere();
                    break;
            }
            return trecho;
        }

        private No LerNoTipo(Type tipo)
        {
            //Caso seja um nó do tipo esperado retorna, senão emite erro
            No no = ProcessarNo();
            if (!tipo.IsInstanceOfType(no))
            {
                throw new Erro(no, "Esperado nó do tipo '" + tipo + "' porém encontrado '" + no.GetType() + "'");
            }
            return no;
        }

        private No ProcessarExpressao5(ref List<No> nos)
        {
            //Organiza o terceiro nivel da árvore de nós de uma expressão
            No no = null;
            if (nos.First() is NoNumeroInteiro)
            {
                no = nos.First();
                nos.Remove(nos.First());
                if(nos.Any() && nos.First() is NoDoisPontos)
                {
                    nos.Remove(nos.First());
                    if (nos.First() is NoNumeroInteiro)
                    {
                        var casasAntes = nos.First();
                        nos.Remove(nos.First());
                        if (nos.First() is NoDoisPontos)
                        {
                            nos.Remove(nos.First());
                            if (nos.First() is NoNumeroInteiro)
                            {
                                no = ((NoNumeroInteiro)no).ParaTexto(((NoNumeroInteiro)casasAntes).Valor, ((NoNumeroInteiro)nos.First()).Valor);
                                nos.Remove(nos.First());
                            }
                            else
                            {
                                throw new Erro(nos.First(), "Esperado um número inteiro para definir a quantidade de casas a serem exibidas antes da virgula");
                            }

                        }
                        else
                        {
                            no = ((NoNumeroInteiro)no).ParaTexto(((NoNumeroInteiro)casasAntes).Valor, 0);
                        }
                    }
                    else
                    {
                        throw new Erro(nos.First(), "Esperado um número inteiro para definir a quantidade de casas a serem exibidas antes da virgula");
                    }
                }
            }
            else if (nos.First() is NoNumeroReal)
            {
                no = nos.First();
                nos.Remove(nos.First());
                if (nos.Any() && nos.First() is NoDoisPontos)
                {
                    nos.Remove(nos.First());
                    if (nos.First() is NoNumeroInteiro)
                    {
                        var casasAntes = nos.First();
                        nos.Remove(nos.First());
                        if (nos.First() is NoDoisPontos)
                        {
                            nos.Remove(nos.First());
                            if (nos.First() is NoNumeroInteiro)
                            {
                                no = ((NoNumeroReal)no).ParaTexto(((NoNumeroInteiro)casasAntes).Valor, ((NoNumeroInteiro)nos.First()).Valor);
                                nos.Remove(nos.First());
                            }
                            else
                            {
                                throw new Erro(nos.First(), "Esperado um número inteiro para definir a quantidade de casas a serem exibidas antes da virgula");
                            }

                        }
                        else
                        {
                            no = ((NoNumeroReal)no).ParaTexto(((NoNumeroInteiro)casasAntes).Valor, 0);
                        }
                    }
                    else
                    {
                        throw new Erro(nos.First(), "Esperado um número inteiro para definir a quantidade de casas a serem exibidas antes da virgula");
                    }
                }
            }
            else if (nos.First() is NoLerVariavel)
            {
                no = nos.First();
                nos.Remove(nos.First());
            }
            else if (nos.First() is NoTexto)
            {
                no = nos.First();
                nos.Remove(nos.First());
            }
            else if (nos.First() is NoAbreParenteses)
            {
                nos.Remove(nos.First());
                no = ProcessarExpressao(ref nos);
                if(nos.First() is NoFechaParenteses) nos.Remove(nos.First());
            }
            else if (nos.First() is NoBloco | nos.First() is NoDeclararVariavel)
            {
                no = nos.First();
                nos.Remove(nos.First());
            }
            else if (nos.First() is NoChamaFuncaoProcedimento)
            {
                no = nos.First();
                nos.Remove(nos.First());
            }
            else if (nos.First() is NoConverterNumeroEmTexto)
            {
                no = nos.First();
                nos.Remove(nos.First());
            }
            return no;

        }

        private No ProcessarExpressao4(ref List<No> nos)
        {
            //Organiza o segundo nivel da árvore de nós de uma expressão
            No no = null;
            no = ProcessarExpressao5(ref nos);
            if(no is null)
            {
                return no;
            }
            if (nos.Any())
            {
                if (nos.First() is NoSoma)
                {
                    var noMatematica = nos.First();
                    nos.Remove(nos.First());
                    ((NoOperacaoMatematicaBase)noMatematica).Item1 = no;
                    ((NoOperacaoMatematicaBase)noMatematica).Item2 = ProcessarExpressao4(ref nos);
                    no = noMatematica;
                }
                else if (nos.First() is NoSubtracao)
                {
                    var noMatematica = nos.First();
                    nos.Remove(nos.First());
                    ((NoOperacaoMatematicaBase)noMatematica).Item1 = no;
                    ((NoOperacaoMatematicaBase)noMatematica).Item2 = ProcessarExpressao4(ref nos);
                    no = noMatematica;
                }
            }
            return no;

        }

        private No ProcessarExpressao3(ref List<No> nos)
        {
            //Organiza o primeiro nivel da árvore de nós de uma expressão
            No no = null;
            no = ProcessarExpressao4(ref nos);
            if (no is null)
            {
                return no;
            }
            if (nos.Any())
            {
                if (nos.First() is NoMultiplicacao)
                {
                    var noMatematica = nos.First();
                    nos.Remove(nos.First());
                    ((NoOperacaoMatematicaBase)noMatematica).Item1 = no;
                    ((NoOperacaoMatematicaBase)noMatematica).Item2 = ProcessarExpressao3(ref nos);
                    no = noMatematica;
                }
                else if (nos.First() is NoDivisao)
                {
                    var noMatematica = nos.First();
                    nos.Remove(nos.First());
                    ((NoOperacaoMatematicaBase)noMatematica).Item1 = no;
                    ((NoOperacaoMatematicaBase)noMatematica).Item2 = ProcessarExpressao3(ref nos);
                    no = noMatematica;
                }
                else if (nos.First() is NoModulo)
                {
                    var noMatematica = nos.First();
                    nos.Remove(nos.First());
                    ((NoOperacaoMatematicaBase)noMatematica).Item1 = no;
                    ((NoOperacaoMatematicaBase)noMatematica).Item2 = ProcessarExpressao3(ref nos);
                    no = noMatematica;
                }
            }
            return no;
        }

        private No ProcessarExpressao2(ref List<No> nos)
        {
            //Organiza o primeiro nivel da árvore de nós de uma expressão
            No no = null;
            no = ProcessarExpressao3(ref nos);
            if (no is null)
            {
                return no;
            }
            if (nos.Any())
            {
                if
                    (
                        nos.First() is NoIgual |
                        nos.First() is NoDiferente |
                        nos.First() is NoMaiorQue |
                        nos.First() is NoMaiorIgualA |
                        nos.First() is NoMenorQue |
                        nos.First() is NoMenorIgualA
                    )
                {
                    var noMatematica = nos.First();
                    nos.Remove(nos.First());
                    ((NoOperacaoMatematicaBase)noMatematica).Item1 = no;
                    ((NoOperacaoMatematicaBase)noMatematica).Item2 = ProcessarExpressao2(ref nos);
                    no = noMatematica;
                }
            }
            return no;
        }

        private No ProcessarExpressao(ref List<No> nos)
        {
            //Organiza o primeiro nivel da árvore de nós de uma expressão
            No no = null;
            no = ProcessarExpressao2(ref nos);
            if (no is null)
            {
                return no;
            }
            if (nos.Any())
            {
                if
                    (
                        nos.First() is NoE |
                        nos.First() is NoOu
                    )
                {
                    var noMatematica = nos.First();
                    nos.Remove(nos.First());
                    ((NoOperacaoMatematicaBase)noMatematica).Item1 = no;
                    ((NoOperacaoMatematicaBase)noMatematica).Item2 = ProcessarExpressao(ref nos);
                    no = noMatematica;
                }
            }
            return no;
        }

        public No ProcessarCodigoFonte()
        {
            //Processa o nó raiz e o armazena
            No no = ProcessarNo();
            while (no is NoFimDeLinha) no = ProcessarNo();
            NoRaiz = no;
            return no;
        }

        public No ProcessarNo()
        {
            //Processa o próximo nó, quando for um nó especial de bloco, processa os nós abaixo
            //Guarda a posição atual no codigo fonte
            PosicaoLeituraNoAnterior = PosicaoLeituraNoAtual;
            PosicaoLeituraNoAtual = PosicaoLeitura;
            //Nó que será retornado
            No no = null;
            //Usado para conversões em numeros
            decimal numero = 0m;
            //Le o proximo trecho de codigo fonte (Palavra, sinal, virgula, numero, etc)
            string trecho = LerTrecho();
            //Guarda posicao no codigo fonte deste trecho lido
            int posicao = PosicaoLeituraTrechoAtual;
            //Dependendo do conteudo do trecho, trata-o, e converta em um Nó (Comando a ser executado)
            switch (trecho.ToLower())
            {
                case "":
                    // Caso chegue ao fim do codigo fonte encerra
                    break;
                case "algoritmo":
                    // Comando Algoritmo
                    {
                        no = new NoAlgoritmo();
                        ((NoAlgoritmo)no).Nome = ((NoTexto)LerNoTipo(typeof(NoTexto))).Valor;
                        No sub;
                        //Processa e armazena os nós dentro do algoritmo ate chegar no 'fimalgoritmo'
                        while(!((sub = ProcessarNo()) is NoFimAlgoritmo))
                        {
                            ((NoAlgoritmo)no).Nos.Add(sub);
                        }
                    }
                    break;
                case "funcao":
                case "função":
                case "procedimento":
                    {
                        no = new NoFuncaoProcedimento()
                        {
                            RetornaValor = trecho.ToLower() != "procedimento"
                        };
                        var pars = new List<NoDeclararVariavel>();
                        No sub;
                        sub = ProcessarNo();
                        //Quando declarado no formato [procedimento NOME (PAR1, PAR2)] se le como um no do tipo NoChamaFuncaoProcedimento
                        if(sub is NoChamaFuncaoProcedimento)
                        {
                            ((NoFuncaoProcedimento)no).Nome = ((NoChamaFuncaoProcedimento)sub).Nome;
                            foreach (var par in ((NoChamaFuncaoProcedimento)sub).Parametros)
                            {
                                if(par is NoBloco)
                                {
                                    foreach (var par1 in ((NoBloco)par).Nos)
                                    {
                                        if(par1 is NoDeclararVariavel)
                                        {
                                            pars.Add((NoDeclararVariavel)par1);
                                        }
                                    }
                                }
                                else if(par is NoDeclararVariavel)
                                {
                                    pars.Add((NoDeclararVariavel)par);
                                }
                                else if(par is NoVirgula)
                                {
                                    //Ignora
                                }
                                else
                                {
                                    throw new Erro(par, "Esperado uma declaração de variável como parametro da Função/Procedimento");
                                }
                            }
                        }
                        else if(sub is NoLerVariavel)
                        {
                            ((NoFuncaoProcedimento)no).Nome = ((NoLerVariavel)sub).Nome;
                        }
                        else
                        {
                            throw new Erro(sub, "Esperado o Nome da Função/Procedimento");
                        }
                        sub = ProcessarNo();
                        if(sub is NoDoisPontos)
                        {
                            if (sub is NoTipoInteiro)
                            {
                                ((NoFuncaoProcedimento)no).TipoRetornado = typeof(int);
                            }
                            else if (sub is NoTipoReal)
                            {
                                ((NoFuncaoProcedimento)no).TipoRetornado = typeof(decimal);
                            }
                            else if (sub is NoTipoCaracter)
                            {
                                ((NoFuncaoProcedimento)no).TipoRetornado = typeof(string);
                            }
                            else if (sub is NoTipoLogico)
                            {
                                ((NoFuncaoProcedimento)no).TipoRetornado = typeof(bool);
                            }
                            else
                            {
                                throw new Erro(sub, "Esperado um tipo de variável");
                            }
                        }
                        else
                        {
                            DesfazerUltimoLerTrecho();
                        }
                        foreach (var par in pars)
                        {
                            ((NoFuncaoProcedimento)no).Parametros.Add(par.Nome, par.ValorInicial.GetType());
                        }
                        while (!((sub = ProcessarNo()) is NoFimFuncaoProcedimento))
                        {
                            ((NoFuncaoProcedimento)no).Nos.Add(sub);
                        }
                    }
                    break;
                case "fimfuncao":
                case "fimfunção":
                case "fimprocedimento":
                    no = new NoFimFuncaoProcedimento();
                    break;
                case "fimalgoritmo":
                    no = new NoFimAlgoritmo();
                    break;
                case "inteiro":
                    no = new NoTipoInteiro();
                    break;
                case "real":
                    no = new NoTipoReal();
                    break;
                case "caracter":
                case "caractere":
                case "caracteres":
                    no = new NoTipoCaracter();
                    break;
                case "inicio":
                case "início":
                    no = new NoInicio();
                    break;
                case "var":
                    no = new NoVar();
                    break;
                case "+":
                    no = new NoSoma();
                    break;
                case "(":
                    no = new NoAbreParenteses();
                    break;
                case ")":
                    no = new NoFechaParenteses();
                    break;
                case ",":
                    no = new NoVirgula();
                    break;
                case "-":
                    no = new NoSubtracao();
                    break;
                case "/":
                    no = new NoDivisao();
                    break;
                case "*":
                    no = new NoMultiplicacao();
                    break;
                case "verdadeiro":
                case "sim":
                    no = new NoVerdadeiro();
                    break;
                case "falso":
                case "nao":
                case "não":
                    no = new NoFalso();
                    break;
                case "entao":
                case "então":
                    no = new NoEntao();
                    break;
                case "senao":
                case "senão":
                    no = new NoSeNao();
                    break;
                case "faca":
                case "faça":
                    no = new NoFaca();
                    break;
                case "fimse":
                    no = new NoFimSe();
                    break;
                case "fimenquanto":
                    no = new NoFimEnquanto();
                    break;
                case "ate":
                case "até":
                    no = new NoAte();
                    break;
                case "e":
                    no = new NoE();
                    break;
                case "ou":
                    no = new NoOu();
                    break;
                case "mod":
                case "%":
                    no = new NoModulo();
                    break;
                case "=":
                    no = new NoIgual();
                    break;
                case "<":
                    no = new NoMenorQue();
                    break;
                case "<=":
                    no = new NoMenorIgualA();
                    break;
                case ">":
                    no = new NoMaiorQue();
                    break;
                case ">=":
                    no = new NoMaiorIgualA();
                    break;
                case "!=":
                case "<>":
                    no = new NoDiferente();
                    break;
                case ":":
                    no = new NoDoisPontos();
                    break;
                case ":=":
                case "<-":
                    no = new NoAtribuicao();
                    break;
                case "\n":
                    no = new NoFimDeLinha();
                    break;
                case "\r":
                    no = new NoFimDeLinha();
                    break;
                case "repita":
                    {
                        int contaParenteses = 1;
                        List<No> nos = new List<No>();
                        List<No> nosTemp = new List<No>();
                        List<No> nosFinal = new List<No>();
                        //Etapa 1 - Gera comando Repita
                        no = new NoRepita();
                        No sub;
                        //Processa e armazena os nós dentro do enquanto
                        while (!((sub = ProcessarNo()) is NoAte))
                        {
                            ((NoRepita)no).Repeticao.Add(sub);
                        }
                        var proximo = LerNoTipo(typeof(NoAbreParenteses));
                        //Etapa 2 - Ler os argumentos sem processar
                        while (contaParenteses > 0)
                        {
                            proximo = ProcessarNo();
                            if (proximo is NoAbreParenteses)
                            {
                                contaParenteses++;
                            }
                            else if (proximo is NoFechaParenteses)
                            {
                                contaParenteses--;
                            }
                            else
                            {
                                nos.Add(proximo);
                            }
                        }
                        //Etapa 3 - Processar argumentos
                        nosFinal.Add(ProcessarExpressao(ref nos));
                        ((NoRepita)no).Condicao = nosFinal.First();
                    }
                    break;
                case string s when decimal.TryParse(s, out numero):
                    //Caso seja um número, diferenciando quando decimal
                    if (trecho.Contains('.'))
                    {
                        no = new NoNumeroReal() { Valor = numero };
                    }
                    else
                    {
                        no = new NoNumeroInteiro() { Valor = (int)numero };
                    }
                    break;
                default:
                    {
                        if (trecho.StartsWith("\""))
                        {
                            //Texto entre aspas
                            no = new NoTexto()
                            {
                                Valor = trecho.Substring(1)
                            };

                        }
                        else
                        {
                            No proximo = ProcessarNo();
                            if (proximo is NoAtribuicao)
                            {
                                //Quando é uma atribuição. Ex: VARIAVEL <- 1
                                NoAtribuicao atrib = new NoAtribuicao();
                                List<No> nos = new List<No>();
                                //Le os nós até o fim da linha
                                while (!((proximo = ProcessarNo()) is NoFimDeLinha))
                                {
                                    nos.Add(proximo);
                                }
                                atrib.VariavelDestino = trecho.ToLower();
                                //Com os nós separados, monta a arvore de Expressão, o que na prática muda de notação padrão matemática para notação polonesa reversa que é facilmente executavel posteriormente
                                atrib.Conteudo = ProcessarExpressao(ref nos);

                                no = atrib;
                            }
                            else if(proximo is NoDoisPontos | proximo is NoVirgula)
                            {
                                //Quando seja uma declaração de variavel. Ex: variavel1 : inteiro
                                List<No> nos = new List<No>();
                                nos.Add(new NoDeclararVariavel()
                                {
                                    Nome = trecho,
                                    Fonte = this,
                                    FontePosicao = PosicaoLeituraTrechoAtual,
                                    FonteLinha = Posicoes[PosicaoLeituraTrechoAtual][0],
                                    FonteColuna = Posicoes[PosicaoLeituraTrechoAtual][1]
                                });
                                //Caso seja uma declaração de multiplas variáveis separadas por vírgula, processa elas. Ex: var1, var2, var3: inteiro
                                if (proximo is NoVirgula)
                                {
                                    while ((trecho = LerTrecho()) != ":")
                                    {
                                        if(trecho != ",")
                                        {
                                            No noTemporario = new NoDeclararVariavel()
                                            {
                                                Nome = trecho,
                                                Fonte = this,
                                                FontePosicao = PosicaoLeituraTrechoAtual,
                                                FonteLinha = Posicoes[PosicaoLeituraTrechoAtual][0],
                                                FonteColuna = Posicoes[PosicaoLeituraTrechoAtual][1]
                                            };
                                            nos.Add(noTemporario);
                                        }
                                    }
                                }
                                //Depois de gerado os nós de declaração, verifica qual o tipo após o ':' e declara neles
                                proximo = ProcessarNo();
                                foreach (NoDeclararVariavel noTemp in nos)
                                {
                                    //Define um valor inicial generico, para que futuramente possa comparar tipos
                                    if (proximo is NoTipoInteiro)
                                    {
                                        noTemp.ValorInicial = 0;
                                    }
                                    else if (proximo is NoTipoReal)
                                    {
                                        noTemp.ValorInicial = 0m;
                                    }
                                    else if (proximo is NoTipoCaracter)
                                    {
                                        noTemp.ValorInicial = "";
                                    }
                                    else if (proximo is NoTipoLogico)
                                    {
                                        noTemp.ValorInicial = false;
                                    }
                                    else if (proximo is NoNumeroInteiro)
                                    {
                                        var casasAntes = proximo;
                                        nos.Remove(nos.First());
                                        proximo = ProcessarNo();
                                        if (proximo is NoDoisPontos)
                                        {
                                            proximo = ProcessarNo();
                                            if (proximo is NoNumeroInteiro)
                                            {
                                                no = new NoConverterNumeroEmTexto()
                                                {
                                                    Nome = noTemp.Nome,
                                                    CasasAntes = ((NoNumeroInteiro)casasAntes).Valor,
                                                    CasasDepois = ((NoNumeroInteiro)proximo).Valor
                                                };
                                                break;
                                            }
                                            else
                                            {
                                                throw new Erro(nos.First(), "Esperado um número inteiro para definir a quantidade de casas a serem exibidas antes da virgula");
                                            }
                                        }
                                        else
                                        {
                                            no = new NoConverterNumeroEmTexto()
                                            {
                                                Nome = noTemp.Nome,
                                                CasasAntes = ((NoNumeroInteiro)casasAntes).Valor,
                                                CasasDepois = 0
                                            };
                                            DesfazerUltimoLerTrecho();
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        throw new Erro(proximo, "Esperado um tipo de variável");
                                    }
                                }
                                if(!(no is NoConverterNumeroEmTexto))
                                {
                                    //Agrupa todas as declarações em um unico bloco
                                    no = new NoBloco()
                                    {
                                        Nos = nos
                                    };
                                }
                            }
                            else if(proximo is NoAbreParenteses)
                            {
                                //Se for a chamada de um procedimento. Ex: ESCREVAL("TESTE")
                                int contaParenteses = 1;
                                List<No> nos = new List<No>();
                                List<No> nosTemp = new List<No>();
                                List<No> nosFinal = new List<No>();
                                //Etapa 1 - Ler os argumentos sem processar
                                while (contaParenteses > 0)
                                {
                                    proximo = ProcessarNo();
                                    if (proximo is NoAbreParenteses)
                                    {
                                        contaParenteses++;
                                    }
                                    else if (proximo is NoFechaParenteses)
                                    {
                                        contaParenteses--;
                                    }
                                    else
                                    {
                                        nos.Add(proximo);
                                    }
                                }
                                //Etapa 2 - Processar argumentos
                                foreach (var noTemp in nos)
                                {
                                    if(noTemp is NoVirgula)
                                    {
                                        nosFinal.Add(ProcessarExpressao(ref nosTemp));
                                        nosTemp.Clear();
                                    }
                                    else
                                    {
                                        nosTemp.Add(noTemp);
                                    }
                                }
                                if(nosTemp.Any())nosFinal.Add(ProcessarExpressao(ref nosTemp));
                                //Etapa 3 - Montar chamada do procedimento
                                switch (trecho.ToLower())
                                {
                                    case "escreva":
                                        no = new NoEscreva()
                                        {
                                            Argumentos = nosFinal
                                        };
                                        break;
                                    case "escreval":
                                        no = new NoEscreva()
                                        {
                                            Argumentos = nosFinal,
                                            EnterAoFinal = true
                                        };
                                        break;
                                    case "leia":
                                        no = new NoLeia()
                                        {
                                            Nome = ((NoLerVariavel)nosFinal.First()).Nome
                                        };
                                        break;
                                    case "se":
                                        {
                                            no = new NoSe()
                                            {
                                                Condicao = nosFinal.First()
                                            };
                                            proximo = LerNoTipo(typeof(NoEntao));
                                            No sub;
                                            //Processa e armazena os nós dentro do se
                                            while (!((sub = ProcessarNo()) is NoFimSe) && !(sub is NoSeNao))
                                            {
                                                ((NoSe)no).SeSim.Add(sub);
                                            }
                                            if (sub is NoSeNao)
                                            {
                                                while (!((sub = ProcessarNo()) is NoFimSe))
                                                {
                                                    ((NoSe)no).SeNao.Add(sub);
                                                }
                                            }
                                        }
                                        break;
                                    case "enquanto":
                                        {
                                            no = new NoEnquanto()
                                            {
                                                Condicao = nosFinal.First()
                                            };
                                            proximo = LerNoTipo(typeof(NoFaca));
                                            No sub;
                                            //Processa e armazena os nós dentro do enquanto
                                            while (!((sub = ProcessarNo()) is NoFimEnquanto))
                                            {
                                                ((NoEnquanto)no).Repeticao.Add(sub);
                                            }
                                        }
                                        break;
                                    default:
                                        {
                                            no = new NoChamaFuncaoProcedimento()
                                            {
                                                Nome = trecho,
                                                Parametros = nosFinal
                                            };
                                        }
                                        break;
                                }
                            }
                            else
                            {
                                //Se for apenas uma leitura de variável, e não uma das anteriores
                                no = new NoLerVariavel()
                                {
                                    Nome = trecho
                                };
                                DesfazerUltimoLerTrecho();
                            }
                        }
                    }
                    break;
            }

            //Armazena no nó, qual a posição atual no código fonte, em caso de erro no futuro, o depurador conseguir ler
            if (no != null)
            {
                no.Fonte = this;
                no.FontePosicao = posicao;
                no.FonteLinha = Posicoes[posicao][0];
                no.FonteColuna = Posicoes[posicao][1];
            }

            return no;
        }
    }
}
