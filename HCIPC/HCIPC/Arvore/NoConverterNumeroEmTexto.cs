﻿// /*
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
    public class NoConverterNumeroEmTexto : No
    {
        //Variavel
        public string Nome { get; set; }
        public int CasasAntes { get; set; }
        public int CasasDepois { get; set; }

        public NoConverterNumeroEmTexto()
        {
        }

        public override void Compilar(Integracao.ArquiteturaDoCompilador comp, ref EstadoExecucao estado)
        {
            comp.ConverterVariavelNumericaEmTextoGuardandoNoValorAtual(Nome, CasasAntes, CasasDepois);
            comp.DeclararTipoDoValorAtual(Integracao.ArquiteturaDoCompilador.TiposDeVariavel.Texto);
        }

        protected override void Executar(ref EstadoExecucao estado)
        {
            var obj = estado[Nome];
            if (obj is int)
            {
                estado.Valor = ((int)obj).ToString(new string('0', CasasAntes) + (CasasDepois > 0 ? "." + new string('0', CasasDepois) : "")); ;
            }
            else if (obj is decimal)
            {
                estado.Valor = ((decimal)obj).ToString(new string('0', CasasAntes) + (CasasDepois > 0 ? "." + new string('0', CasasDepois) : "")); ;
            }
            else
            {
                throw new Erro(this, "Esperado uma variável númerica para conversão em texto");
            }
        }
    }
}
