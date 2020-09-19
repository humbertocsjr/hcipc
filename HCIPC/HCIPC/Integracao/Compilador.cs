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
using System.Text;
using System.Threading.Tasks;

namespace HCIPC.Integracao
{

    public abstract class SistemaOperacionalDoCompilador
    {
        public abstract void EscrevaParaUsuarioConteudoDoValorAtual();
        public abstract void EscrevaParaUsuarioQuebraDeLinha();
        public abstract void LeiaDoUsuarioGravandoNaVariavelTexto(string nome);
        public abstract void LeiaDoUsuarioGravandoNaVariavelInteiro(string nome);
        public abstract void LeiaDoUsuarioGravandoNaVariavelReal(string nome);
        public abstract void LeiaDoUsuarioGravandoNaVariavelLogico(string nome);
        public abstract void SairParaOSistemaOperacional(int codigoDeErro);
        // Passado os parametros nao processados pois cada arquitetura passa os parametros em uma ordem especifica que deve ser processado pelo compilador
        public abstract void ChamarRotinaExterna(string biblioteca, string nome, List<Arvore.No> nosNaoProcessados);

    }

    public abstract class ArquiteturaDoCompilador
    {
        public enum TiposDeVariavel
        {
            Inteiro,
            Real,
            Texto,
            Logico,
            Dados,
            Nenhum
        }

        public enum TiposDeMarcador
        {
            InicioRotina,
            FimRotina,
            Enquanto,
            FimEnquanto,
            Repita,
            Ate,
            Se,
            Senao,
            FimSe,
            SaidaDaRepeticao,
            IgnorarRotina
        }

        public SistemaOperacionalDoCompilador SisOp { get; set; }

        public TiposDeVariavel ConverterTipo(Type tipo)
        {
            if(tipo == null)
            {
                return TiposDeVariavel.Nenhum;
            }
            else if(tipo.IsInstanceOfType(""))
            {
                return TiposDeVariavel.Texto;
            }
            else if (tipo.IsInstanceOfType(0))
            {
                return TiposDeVariavel.Inteiro;
            }
            else if (tipo.IsInstanceOfType(0m))
            {
                return TiposDeVariavel.Real;
            }
            else if (tipo.IsInstanceOfType(true))
            {
                return TiposDeVariavel.Logico;
            }
            else if (tipo.IsInstanceOfType(new byte[] { }))
            {
                return TiposDeVariavel.Dados;
            }
            else
            {
                throw new ArgumentOutOfRangeException("Tipo de variavel incompativel");
            }
        }

        public Dictionary<string, TiposDeVariavel> ConverterTipo(Dictionary<string, Type> tipos)
        {
            var retorno = new Dictionary<string, TiposDeVariavel>();
            foreach (var item in tipos)
            {
                retorno.Add(item.Key, ConverterTipo(item.Value));
            }
            return retorno;
        }

        public abstract void DeclararAlgoritmo(string nome);
        public abstract void DeclararRotina(string nome, Dictionary<string, TiposDeVariavel> parametros, bool retornaValor, TiposDeVariavel tipo);

        public abstract void FimAlgoritmo();
        public abstract void FimRotina();

        public abstract void DeclararTipoDoValorAtual(TiposDeVariavel tipo);

        public abstract int ReservarMarcador(TiposDeMarcador tipo);

        public abstract void AplicarMarcadorAqui(int reserva);

        public abstract int LerUltimoMarcadorDoTipo(TiposDeMarcador tipo);

        public abstract void PularParaMarcador(int reserva);
        public abstract void PularParaMarcadorSeIgual(int reserva);
        public abstract void PularParaMarcadorSeDiferente(int reserva);
        public abstract void PularParaMarcadorSeMaiorQue(int reserva);
        public abstract void PularParaMarcadorSeMaiorOuIgual(int reserva);
        public abstract void PularParaMarcadorSeMenorQue(int reserva);
        public abstract void PularParaMarcadorSeMenorOuIgual(int reserva);

        public abstract void GravarVariavelNoAtual(string nome);
        public abstract void GravarAtualNaVariavel(string nome);
        public abstract void GravarLogicoNoValorAtual(bool valor);
        public abstract void GravarInteiroNoValorAtual(int valor);
        public abstract void GravarRealNoValorAtual(decimal valor);
        public abstract void GravarTextoNoValorAtual(string valor);

        // Passado os parametros nao processados pois cada arquitetura passa os parametros em uma ordem especifica que deve ser processado pelo compilador
        public abstract void ChamarRotinaLocal(string nome, List<Arvore.No> nosNaoProcessados);
        
        public abstract void ConverterVariavelNumericaEmTextoGuardandoNoValorAtual(string nome, int casasAntes, int casasDepois);

        public abstract void DeclararVariavel(string nome, object valorInicial);

        public abstract void EmpilharValorAtual();

        public abstract void CompararSeValorAtualForIgualAVerdadeiro();

        public abstract void DesempilharECompararSeDiferenteGuardandoNoValorAtual();
        public abstract void DesempilharECompararSeIgualGuardandoNoValorAtual();
        public abstract void DesempilharECompararSeMaiorOuIgualGuardandoNoValorAtual();
        public abstract void DesempilharECompararSeMenorOuIgualGuardandoNoValorAtual();
        public abstract void DesempilharECompararSeMaiorQueGuardandoNoValorAtual();
        public abstract void DesempilharECompararSeMenorQueGuardandoNoValorAtual();
        public abstract void DesempilharECompararSeAmbosVerdadeirosGuardandoNoValorAtual();
        public abstract void DesempilharECompararSeAlgumEVerdadeiroGuardandoNoValorAtual();

        public abstract void DesempilharEDividirGuardandoNoValorAtual();
        public abstract void DesempilharEDividirGuardandoModuloNoValorAtual();
        public abstract void DesempilharEMultiplicarGuardandoNoValorAtual();
        public abstract void DesempilharESomarGuardandoNoValorAtual();
        public abstract void DesempilharESubtrairGuardandoNoValorAtual();

    }
}
