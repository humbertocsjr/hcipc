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
    public abstract class NoComparadorBase : NoOperacaoMatematicaBase
    {
        public NoComparadorBase()
        {
        }

        public decimal LerNumero(object valor)
        {
            decimal retorno = 0m;
            if(valor is int)
            {
                retorno = (decimal)(int)valor;
            }
            else
            if (valor is decimal)
            {
                retorno = (decimal)valor;
            }
            else
            {
                throw new Erro(this, "Esperado valor numérico");
            }
            return retorno;
        }

        public bool ENumerico(object valor)
        {
            return valor is decimal || valor is int;
        }

        public bool ETexto(object valor)
        {
            return valor is string;
        }

        public bool SaoNumericos(object valor1, object valor2)
        {
            return ENumerico(valor1) & ENumerico(valor2);
        }

        public bool SaoTextos(object valor1, object valor2)
        {
            return ETexto(valor1) & ETexto(valor2);
        }

        public bool ContemTextos(object valor1, object valor2)
        {
            return ETexto(valor1) | ETexto(valor2);
        }
    }
}
