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
using HCIPC.Arvore;

namespace HCIPC.Integracao
{
    public class DeclaraRotina
    {
        internal Interpretador Interpretador { get; set; }
        internal NoFuncaoProcedimento Rotina { get; set; }

        internal DeclaraRotina(Interpretador interpretador)
        {
            Interpretador = interpretador;
            Rotina = new NoFuncaoProcedimento()
            {
                Fonte = new Fonte()
                {
                    NomeDoArquivo = "[BIBLIOTECA NATIVA]"
                },
                FonteColuna = 0,
                FonteLinha = 0,
                FontePosicao =0,
                
            };
        }

        /// <summary>
        /// Define o tipo de Retorno de uma Função ou Procedimento
        /// </summary>
        /// <param name="tipo">Defina como NULL para declara como Procedimento ou com um tipo para definir como Função</param>
        /// <returns></returns>
        public DeclaraRotina DefineTipoDeRetorno(Type tipo)
        {
            Rotina.RetornaValor = tipo != null;
            Rotina.TipoRetornado = tipo;
            return this;
        }

        /// <summary>
        /// Define o nome desta Rotina
        /// </summary>
        /// <param name="biblioteca">Nome da Biblioteca</param>
        /// <param name="nome">Nome da Rotina</param>
        /// <returns></returns>
        public DeclaraRotina DefineNomeDaRotina (string biblioteca, string nomeDaRotina)
        {
            Rotina.Fonte.NomeDoArquivo = biblioteca;
            Rotina.Nome = nomeDaRotina;
            return this;
        }

        /// <summary>
        /// Adiciona um Parametro a rotina
        /// </summary>
        /// <param name="nome">Nome do Parametro</param>
        /// <param name="tipo">Tipo do Parametro</param>
        /// <returns></returns>
        public DeclaraRotina AdicionarParametro(string nome, Type tipo)
        {
            Rotina.Parametros.Add(nome, tipo);
            return this;
        }

        /// <summary>
        /// Define a ação a ser executada quando é chamada a rotina, caso haja um valor de retorno, este deve ser armazenado no paramtro EstadoExecucao, dentro da propriedade Valor
        /// </summary>
        /// <param name="aoExecutar"></param>
        /// <returns></returns>
        public DeclaraRotina DefineAcao(Action<EstadoExecucao, NoFuncaoProcedimento> aoExecutar)
        {
            Rotina.RotinaNativa = aoExecutar;
            return this;
        }

        /// <summary>
        /// Executar ao final das definições para Registrar esta nova rotina
        /// </summary>
        public void GravarRegistroDestaRotinaNoInterpretador()
        {
            if( !string.IsNullOrEmpty(Rotina.Nome) & Rotina.RotinaNativa != null)
            {
                Interpretador.RegistrarRotina(Rotina.Fonte.NomeDoArquivo, Rotina.Nome, Rotina);
            }
            else
            {
                throw new Erro(Rotina, "Falta um dos requisitos para o registro da rotina, verificar se foi preenchido o nome da rotina e/ou a ação da rotina.");
            }
        }

    }
}
