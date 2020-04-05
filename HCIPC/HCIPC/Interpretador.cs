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
using System.IO;
using HCIPC.Arvore;

namespace HCIPC
{
    public class Interpretador
    {
        Dictionary<string, Fonte> _fontes = new Dictionary<string, Fonte>();
        Fonte _inicial = null;
        EstadoExecucao _estado = new EstadoExecucao();

        public EntradaSaida EntradaSaidaPadrao { get; set; }

        public Interpretador()
        {
            //Inicializa com o módulo de E/S padrão, podendo ser subistituido
            EntradaSaidaPadrao = new EntradaSaidaTerminal();
            _estado.Interpretador = this;
        }

        public void AdicionarArquivo(string endereco, bool arquivoExecutavelPrincipal)
        {
            //Le o arquivo e converte
            AdicionarCodigoFonte(endereco, File.ReadAllText(endereco), arquivoExecutavelPrincipal);
        }

        public void AdicionarCodigoFonte(string arquivo, string codigo, bool arquivoExecutavelPrincipal)
        {
            //Le o codigo fonte, gravando como uma das bibliotecas atuais
            _fontes.Add(arquivo, new Fonte() { NomeDoArquivo = arquivo, CodigoFonte = codigo });
            //Gera a arvore de Nós onde ficam os Tokens do código fonte
            _fontes[arquivo].ProcessarCodigoFonte();
            //TODO: Futuramente guardar no EstadoExecucao, as funcoes e procedimentos
            //Caso seja o código fonte a ser executado, armazena agora
            if (arquivoExecutavelPrincipal) _inicial = _fontes[arquivo];
        }

        public List<string> CodigosFontesCarregados()
        {
            //Busca e retorna os arquivos armazenados na memoria
            List<string> retorno = new List<string>();
            foreach (var fonte in _fontes)
            {
                retorno.Add(fonte.Key);
            }
            return retorno;
        }

        public bool Executar()
        {
            //Executa o código marcado para tal
            return Executar(_inicial.NoRaiz, ref _estado);
        }

        private bool Executar(No raiz,  ref EstadoExecucao estado)
        {
            try
            {
                //Executa o nó raiz (Normalmente o nó "Algoritmo")
                raiz.ExecutarNo(ref estado);
                return true;
            }
            catch (Erro ex)
            {
                //Caso de um erro envia para o E/S para exibi-lo
                estado.ES.Erro(ex);
                return false;
            }
        }
    }
}
