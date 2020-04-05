﻿using System;
using System.Collections.Generic;
using System.Linq;
using HCIPC.Arvore;

namespace HCIPC
{
    public class Fonte
    {
        private string _fonte = "";

        public string NomeDoArquivo { get; set; }
        public string CodigoFonte
        {
            get
            {
                return _fonte;
            }
            set
            {
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
        private Dictionary<int, int[]> Posicoes { get; set; }
        private Dictionary<int, int> PosicoesLinhas { get; set; }
        private int PosicaoLeitura { get; set; }
        private int PosicaoLeituraNoAnterior { get; set; }
        private int PosicaoLeituraNoAtual { get; set; }
        private int PosicaoLeituraTrechoAtual { get; set; }
        private int Linha { get; set; }
        private int Coluna { get; set; }
        private char Atual { get; set; }
        public No NoRaiz { get; set; }

        public Fonte()
        {
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
            if(PosicoesLinhas.ContainsKey(linha))
            {
                return _fonte.Substring(PosicoesLinhas[linha], PosicoesLinhas[linha + 1] - PosicoesLinhas[linha]).Replace("\n","").Replace("\r","");
            }
            return null;
        }

        private char LerCaractere()
        {
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
            char retorno = (char)0;
            if (CodigoFonte.Length > PosicaoLeitura)
            {
                retorno = CodigoFonte[PosicaoLeitura];
            }
            return retorno;
        }

        private void DesfazerUltimoLerTrecho()
        {
            PosicaoLeitura = PosicaoLeituraNoAtual;
            Atual = CodigoFonte[PosicaoLeitura - 1];
        }

        private string LerTrecho()
        {
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
                                (Atual >= '0' & Atual <= '9')
                            )
                        {
                            trecho += Atual;
                            LerCaractere();
                        }
                    }
                    break;
                case '"':
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
                            trecho += Atual;
                            LerCaractere();
                            trecho += Atual;
                            LerCaractere();
                            break;
                        default:
                            trecho += Atual;
                            LerCaractere();
                            break;
                    }
                    break;
                case '<':
                    switch (PreverCaractere())
                    {
                        case '-':
                        case '=':
                            trecho += Atual;
                            LerCaractere();
                            trecho += Atual;
                            LerCaractere();
                            break;
                        default:
                            trecho += Atual;
                            LerCaractere();
                            break;
                    }
                    break;
                case '>':
                    switch (PreverCaractere())
                    {
                        case '=':
                            trecho += Atual;
                            LerCaractere();
                            trecho += Atual;
                            LerCaractere();
                            break;
                        default:
                            trecho += Atual;
                            LerCaractere();
                            break;
                    }
                    break;
                default:
                    trecho += Atual;
                    LerCaractere();
                    break;
            }
            return trecho;
        }

        private No LerNoTipo(Type tipo)
        {
            No no = ProcessarNo();
            if (!tipo.IsInstanceOfType(no))
            {
                throw new Erro(no, "Esperado nó do tipo '" + tipo + "' porém encontrado '" + no.GetType() + "'");
            }
            return no;
        }

        private No ProcessarExpressao3(ref List<No> nos)
        {
            No no = null;
            if (nos.First() is NoNumeroInteiro)
            {
                no = nos.First();
                nos.Remove(nos.First());
            }
            else if (nos.First() is NoNumeroReal)
            {
                no = nos.First();
                nos.Remove(nos.First());
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
            return no;

        }

        private No ProcessarExpressao2(ref List<No> nos)
        {
            No no = null;
            no = ProcessarExpressao3(ref nos);
            if (nos.Any())
            {
                if (nos.First() is NoSoma)
                {
                    var noMatematica = nos.First();
                    nos.Remove(nos.First());
                    ((NoOperacaoMatematicaBase)noMatematica).Item1 = no;
                    ((NoOperacaoMatematicaBase)noMatematica).Item2 = ProcessarExpressao(ref nos);
                    no = noMatematica;
                }
                else if (nos.First() is NoSubtracao)
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

        private No ProcessarExpressao(ref List<No> nos)
        {
            No no = null;
            no = ProcessarExpressao2(ref nos);
            if (nos.Any())
            {
                if (nos.First() is NoMultiplicacao)
                {
                    var noMatematica = nos.First();
                    nos.Remove(nos.First());
                    ((NoOperacaoMatematicaBase)noMatematica).Item1 = no;
                    ((NoOperacaoMatematicaBase)noMatematica).Item2 = ProcessarExpressao(ref nos);
                    no = noMatematica;
                }
                else if (nos.First() is NoDivisao)
                {
                    var noMatematica = nos.First();
                    nos.Remove(nos.First());
                    ((NoOperacaoMatematicaBase)noMatematica).Item1 = no;
                    ((NoOperacaoMatematicaBase)noMatematica).Item2 = ProcessarExpressao(ref nos);
                    no = noMatematica;
                }
                else if (nos.First() is NoModulo)
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
            No no = ProcessarNo();
            NoRaiz = no;
            return no;
        }

        public No ProcessarNo()
        {
            PosicaoLeituraNoAnterior = PosicaoLeituraNoAtual;
            PosicaoLeituraNoAtual = PosicaoLeitura;
            No no = null;
            decimal numero = 0m;
            string trecho = LerTrecho();
            int posicao = PosicaoLeituraTrechoAtual;
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
                        while(!((sub = ProcessarNo()) is NoFimAlgoritmo))
                        {
                            ((NoAlgoritmo)no).Nos.Add(sub);
                        }
                    }
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
                case "mod":
                case "%":
                    no = new NoModulo();
                    break;
                case "=":
                    no = new NoIgual();
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
                case string s when decimal.TryParse(s, out numero):
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
                                NoAtribuicao atrib = new NoAtribuicao();
                                List<No> nos = new List<No>();

                                while (!((proximo = ProcessarNo()) is NoFimDeLinha))
                                {
                                    nos.Add(proximo);
                                }
                                atrib.VariavelDestino = trecho.ToLower();
                                atrib.Conteudo = ProcessarExpressao(ref nos);

                                no = atrib;
                            }
                            else if(proximo is NoDoisPontos | proximo is NoVirgula)
                            {
                                List<No> nos = new List<No>();
                                nos.Add(new NoDeclararVariavel() { Nome = trecho });
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
                                proximo = ProcessarNo();
                                foreach (NoDeclararVariavel noTemp in nos)
                                {
                                    if (proximo is NoTipoInteiro)
                                    {
                                        noTemp.ValorInicial = 0;
                                    }
                                    else if (proximo is NoTipoReal)
                                    {
                                        noTemp.ValorInicial = 0f;
                                    }
                                    else if (proximo is NoTipoCaracter)
                                    {
                                        noTemp.ValorInicial = "";
                                    }
                                }
                                no = new NoBloco()
                                {
                                    Nos = nos
                                };
                            }
                            else if(proximo is NoAbreParenteses)
                            {
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
                                //Etapa 3 - Montar chamada
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
                                    default:
                                        //TODO: Fazer chamar procedimento
                                        break;
                                }
                            }
                            else
                            {
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
