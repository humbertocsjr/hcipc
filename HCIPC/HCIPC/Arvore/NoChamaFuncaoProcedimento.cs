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

namespace HCIPC.Arvore
{
    public class NoChamaFuncaoProcedimento : No
    {
        public string Nome { get; set; }
        public List<No> Parametros { get; set; }

        public NoChamaFuncaoProcedimento()
        {
        }

        public override void Compilar(Integracao.ArquiteturaDoCompilador comp, ref EstadoExecucao estado)
        {
            if(Nome.Contains("."))
            {
                comp.SisOp.ChamarRotinaExterna(Nome.Split('.')[0], Nome.Split('.')[1], Parametros);
            }
            else
            {
                comp.ChamarRotinaLocal(Nome, Parametros);
            }
        }

        protected override void Executar(ref EstadoExecucao estado)
        {
            List<object> pars = new List<object>();
            foreach (var par in Parametros)
            {
                estado.Valor = null;
                par.ExecutarNo(ref estado);
                pars.Add(estado.Valor);
            }
            if(Nome.Contains("."))
            {
                estado.Valor = estado.ExecutarRotina(Nome.Split('.')[0], Nome.Split('.')[1], pars.ToArray()) ;
            }
            else
            {
                estado.Valor = estado.ExecutarRotinaLocal(Nome, pars.ToArray());
            }
        }
    }
}
