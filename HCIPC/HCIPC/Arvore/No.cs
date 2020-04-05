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
namespace HCIPC.Arvore
{
    public abstract class No
    {
        // Dados para depuração do código interpretado
        public Fonte Fonte { get; set; }
        public int FontePosicao { get; set; }
        public int FonteLinha { get; set; }
        public int FonteColuna { get; set; }

        public No()
        {
        }

        // Comando chamado pelo Interpretador para Executar o comando deste nó
        public void ExecutarNo(ref EstadoExecucao estado)
        {
            try
            {
                //TODO: Colocar aqui a parte de depuração esperando liberação da IDE

                //Guarda o nó atual no Estado da Execução
                estado.NoAtual = this;
                //Executa o nó
                Executar(ref estado);
            }
            catch(Erro ex)
            {
                //Caso aconteça um erro conhecido (Do tipo Erro), passa adiante
                throw ex;
            }
            catch (Exception ex)
            {
                //Caso aconteça um erro desconhecido, recria no formato Erro
                throw new Erro(this, "Erro desconhecido: " + ex.ToString());
            }
        }

        //Comando implementado pelos Nós para executar o comando
        protected abstract void Executar(ref EstadoExecucao estado);
    }
}
