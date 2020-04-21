using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HCIPC.Arvore;
using HCIPC.Integracao;

namespace HCPC8086
{
    class Arq8086 : ArqBase
    {
        public override void AplicarMarcadorAqui(int reserva)
        {
            Binario.Marcar(reserva);
        }

        public override void ChamarRotinaExterna(string biblioteca, string nome, List<No> nosNaoProcessados)
        {
            throw new NotImplementedException();
        }

        public override void ChamarRotinaLocal(string nome, List<No> nosNaoProcessados)
        {
            throw new NotImplementedException();
        }

        public override void CompararSeValorAtualForIgualAVerdadeiro()
        {
            throw new NotImplementedException();
        }

        public override void ConverterVariavelNumericaEmTextoGuardandoNoValorAtual(string nome, int casasAntes, int casasDepois)
        {
            throw new NotImplementedException();
        }

        public override void DeclararAlgoritmo(string nome)
        {
            throw new NotImplementedException();
        }

        public override void DeclararRotina(string nome, Dictionary<string, TiposDeVariavel> parametros, bool retornaValor, TiposDeVariavel tipo)
        {
            throw new NotImplementedException();
        }

        public override void DeclararTipoDoValorAtual(TiposDeVariavel tipo)
        {
            throw new NotImplementedException();
        }

        public override void DeclararVariavel(string nome, object valorInicial)
        {
            throw new NotImplementedException();
        }

        public override void DesempilharECompararSeAlgumEVerdadeiroGuardandoNoValorAtual()
        {
            //POP BX
            Binario.Byte(0x5b);
            //CMP AX, 0
            Binario.Byte(0x83);
            Binario.Byte(0xf8);
            Binario.Byte(0x00);
            //JNZ .fim
            Binario.Byte(0x75);
            Binario.Byte(0x07);
            //CMP BX, 0
            Binario.Byte(0x83);
            Binario.Byte(0xfb);
            Binario.Byte(0x00);
            //JNZ .fim
            Binario.Byte(0x75);
            Binario.Byte(0x02);
            //jmp short .fim
            Binario.Byte(0xeb);
            Binario.Byte(0x05);
            //mov ax, 0xffff
            Binario.Byte(0xb8);
            Binario.Byte(0xff);
            Binario.Byte(0xff);
            //jmp short .fim
            Binario.Byte(0xeb);
            Binario.Byte(0x03);
            //mov ax, 0
            Binario.Byte(0xb8);
            Binario.Byte(0x00);
            Binario.Byte(0x00);
        }

        public override void DesempilharECompararSeAmbosVerdadeirosGuardandoNoValorAtual()
        {
            //POP BX
            Binario.Byte(0x5b);
            //CMP AX, 0
            Binario.Byte(0x83);
            Binario.Byte(0xf8);
            Binario.Byte(0x00);
            //JZ .fim
            Binario.Byte(0x74);
            Binario.Byte(0x0a);
            //CMP BX, 0
            Binario.Byte(0x83);
            Binario.Byte(0xfb);
            Binario.Byte(0x00);
            //mov ax, 0xffff
            Binario.Byte(0xb8);
            Binario.Byte(0xff);
            Binario.Byte(0xff);
            //jmp short .fim
            Binario.Byte(0xeb);
            Binario.Byte(0x03);
            //mov ax, 0
            Binario.Byte(0xb8);
            Binario.Byte(0x00);
            Binario.Byte(0x00);

        }

        public override void DesempilharECompararSeDiferenteGuardandoNoValorAtual()
        {
            //POP BX
            Binario.Byte(0x5b);
            //CMP AX, BX
            Binario.Byte(0x39);
            Binario.Byte(0xd8);
            //PUSHF
            Binario.Byte(0x9c);
            //JE .falso
            Binario.Byte(0x74);
            Binario.Byte(0x05);
            //MOV AX, 0xffff
            Binario.Byte(0xb8);
            Binario.Byte(0xff);
            Binario.Byte(0xff);
            //JMP SHORT .fim
            Binario.Byte(0xeb);
            Binario.Byte(0x02);
            //.falso:
            //XOR AX, AX
            Binario.Byte(0x31);
            Binario.Byte(0xc0);
            //.fim:
            //POPF
            Binario.Byte(0x9d);
        }

        public override void DesempilharECompararSeIgualGuardandoNoValorAtual()
        {
            //POP BX
            Binario.Byte(0x5b);
            //CMP AX, BX
            Binario.Byte(0x39);
            Binario.Byte(0xd8);
            //PUSHF
            Binario.Byte(0x9c);
            //JNE .falso
            Binario.Byte(0x75);
            Binario.Byte(0x05);
            //MOV AX, 0xffff
            Binario.Byte(0xb8);
            Binario.Byte(0xff);
            Binario.Byte(0xff);
            //JMP SHORT .fim
            Binario.Byte(0xeb);
            Binario.Byte(0x02);
            //.falso:
            //XOR AX, AX
            Binario.Byte(0x31);
            Binario.Byte(0xc0);
            //.fim:
            //POPF
            Binario.Byte(0x9d);
        }

        public override void DesempilharECompararSeMaiorOuIgualGuardandoNoValorAtual()
        {
            //POP BX
            Binario.Byte(0x5b);
            //CMP AX, BX
            Binario.Byte(0x39);
            Binario.Byte(0xd8);
            //PUSHF
            Binario.Byte(0x9c);
            //JL .falso
            Binario.Byte(0x7c);
            Binario.Byte(0x05);
            //MOV AX, 0xffff
            Binario.Byte(0xb8);
            Binario.Byte(0xff);
            Binario.Byte(0xff);
            //JMP SHORT .fim
            Binario.Byte(0xeb);
            Binario.Byte(0x02);
            //.falso:
            //XOR AX, AX
            Binario.Byte(0x31);
            Binario.Byte(0xc0);
            //.fim:
            //POPF
            Binario.Byte(0x9d);
        }

        public override void DesempilharECompararSeMaiorQueGuardandoNoValorAtual()
        {
            //POP BX
            Binario.Byte(0x5b);
            //CMP AX, BX
            Binario.Byte(0x39);
            Binario.Byte(0xd8);
            //PUSHF
            Binario.Byte(0x9c);
            //JLE .falso
            Binario.Byte(0x7e);
            Binario.Byte(0x05);
            //MOV AX, 0xffff
            Binario.Byte(0xb8);
            Binario.Byte(0xff);
            Binario.Byte(0xff);
            //JMP SHORT .fim
            Binario.Byte(0xeb);
            Binario.Byte(0x02);
            //.falso:
            //XOR AX, AX
            Binario.Byte(0x31);
            Binario.Byte(0xc0);
            //.fim:
            //POPF
            Binario.Byte(0x9d);
        }

        public override void DesempilharECompararSeMenorOuIgualGuardandoNoValorAtual()
        {
            //POP BX
            Binario.Byte(0x5b);
            //CMP AX, BX
            Binario.Byte(0x39);
            Binario.Byte(0xd8);
            //PUSHF
            Binario.Byte(0x9c);
            //JG .falso
            Binario.Byte(0x7f);
            Binario.Byte(0x05);
            //MOV AX, 0xffff
            Binario.Byte(0xb8);
            Binario.Byte(0xff);
            Binario.Byte(0xff);
            //JMP SHORT .fim
            Binario.Byte(0xeb);
            Binario.Byte(0x02);
            //.falso:
            //XOR AX, AX
            Binario.Byte(0x31);
            Binario.Byte(0xc0);
            //.fim:
            //POPF
            Binario.Byte(0x9d);
        }

        public override void DesempilharECompararSeMenorQueGuardandoNoValorAtual()
        {
            //POP BX
            Binario.Byte(0x5b);
            //CMP AX, BX
            Binario.Byte(0x39);
            Binario.Byte(0xd8);
            //PUSHF
            Binario.Byte(0x9c);
            //JGE .falso
            Binario.Byte(0x7d);
            Binario.Byte(0x05);
            //MOV AX, 0xffff
            Binario.Byte(0xb8);
            Binario.Byte(0xff);
            Binario.Byte(0xff);
            //JMP SHORT .fim
            Binario.Byte(0xeb);
            Binario.Byte(0x02);
            //.falso:
            //XOR AX, AX
            Binario.Byte(0x31);
            Binario.Byte(0xc0);
            //.fim:
            //POPF
            Binario.Byte(0x9d);
        }

        public override void DesempilharEDividirGuardandoModuloNoValorAtual()
        {
            //POP BX
            Binario.Byte(0x5b);
            //XOR DX, DX
            Binario.Byte(0x31);
            Binario.Byte(0xd2);
            //IDIV BX
            Binario.Byte(0xf7);
            Binario.Byte(0xfb);
            //MOV AX, DX
            Binario.Byte(0x89);
            Binario.Byte(0xd0);
        }

        public override void DesempilharEDividirGuardandoNoValorAtual()
        {
            //POP BX
            Binario.Byte(0x5b);
            //XOR DX, DX
            Binario.Byte(0x31);
            Binario.Byte(0xd2);
            //IDIV BX
            Binario.Byte(0xf7);
            Binario.Byte(0xfb);
        }

        public override void DesempilharEMultiplicarGuardandoNoValorAtual()
        {
            //POP BX
            Binario.Byte(0x5b);
            //XOR DX, DX
            Binario.Byte(0x31);
            Binario.Byte(0xd2);
            //IMUL BX
            Binario.Byte(0xf7);
            Binario.Byte(0xeb);
        }

        public override void DesempilharESomarGuardandoNoValorAtual()
        {
            //POP BX
            Binario.Byte(0x5b);
            //ADD AX, BX
            Binario.Byte(0x01);
            Binario.Byte(0xd8);
        }

        public override void DesempilharESubtrairGuardandoNoValorAtual()
        {
            //POP BX
            Binario.Byte(0x5b);
            //SUB AX, BX
            Binario.Byte(0x29);
            Binario.Byte(0xd8);
        }

        public override void EmpilharValorAtual()
        {
            //PUSH AX
            Binario.Byte(0x50);
        }

        public override void FimAlgoritmo()
        {
            SisOp.SairParaOSistemaOperacional(0);
        }

        public override void FimRotina()
        {
            //RETF
            Binario.Byte(0xCB);
        }

        public override void GravarAtualNaVariavel(string nome)
        {
            //MOV [BP+variavel], AX
            Binario.Byte(0x89);
            Binario.Byte(0x46);
            Binario.SByteReferenciaNomeDireta(nome);
        }

        public override void GravarInteiroNoValorAtual(int valor)
        {
            UInt16 val = (UInt16)valor;
            //MOV AX, [VAL]
            Binario.Byte(0xb8);
            Binario.UInt16(val);
        }

        public override void GravarLogicoNoValorAtual(bool valor)
        {
            if(valor)
            {
                //MOV AX, 0xFFFF
                Binario.Byte(0xb8);
                Binario.UInt16(0xffff);
            }
            else
            {
                //XOR AX, AX
                Binario.Byte(0x31);
                Binario.Byte(0xc0);
            }
        }

        public override void GravarRealNoValorAtual(decimal valor)
        {
            throw new NotImplementedException();
        }

        public override void GravarTextoNoValorAtual(string valor)
        {
            var texto = UTF8Encoding.UTF8.GetBytes(valor);
            //JMP fim_texto
            Binario.Byte(0xe9);
            Binario.UInt16((UInt16)texto.Length);
            //texto
            Binario.Bytes(texto);
            //fim_texto
        }

        public override void GravarVariavelNoAtual(string nome)
        {
            //MOV AX, [BP+variavel]
            Binario.Byte(0x8b);
            Binario.Byte(0x46);
            Binario.SByteReferenciaNomeDireta(nome);
        }

        public override int LerUltimoMarcadorDoTipo(TiposDeMarcador tipo)
        {
            return Binario.LerUltimoMarcador(tipo);
        }

        public override void PularParaMarcador(int reserva)
        {
            //JMP NEAR marcador
            Binario.Byte(0xe9);
            Binario.Int16ReferenciaIndireta(reserva);
        }

        public override void PularParaMarcadorSeDiferente(int reserva)
        {
            //JE continua [Se igual]
            Binario.Byte(0x74);
            Binario.Byte(0x03);
            //JMP NEAR marcador
            Binario.Byte(0xe9);
            Binario.Int16ReferenciaIndireta(reserva);
            //continua
        }

        public override void PularParaMarcadorSeIgual(int reserva)
        {
            //JNE continua [Se diferente]
            Binario.Byte(0x75);
            Binario.Byte(0x03);
            //JMP NEAR marcador
            Binario.Byte(0xe9);
            Binario.Int16ReferenciaIndireta(reserva);
            //continua
        }

        public override void PularParaMarcadorSeMaiorOuIgual(int reserva)
        {
            //JL continua [Se menor]
            Binario.Byte(0x7C);
            Binario.Byte(0x03);
            //JMP NEAR marcador
            Binario.Byte(0xe9);
            Binario.Int16ReferenciaIndireta(reserva);
            //continua
        }

        public override void PularParaMarcadorSeMaiorQue(int reserva)
        {
            //JLE continua [Se menor ou igual]
            Binario.Byte(0x7E);
            Binario.Byte(0x03);
            //JMP NEAR marcador
            Binario.Byte(0xe9);
            Binario.Int16ReferenciaIndireta(reserva);
            //continua
        }

        public override void PularParaMarcadorSeMenorOuIgual(int reserva)
        {
            //JG continua [Se maior]
            Binario.Byte(0x7F);
            Binario.Byte(0x03);
            //JMP NEAR marcador
            Binario.Byte(0xe9);
            Binario.Int16ReferenciaIndireta(reserva);
            //continua
        }

        public override void PularParaMarcadorSeMenorQue(int reserva)
        {
            //JGE continua [Se maior ou igual]
            Binario.Byte(0x7D);
            Binario.Byte(0x03);
            //JMP NEAR marcador
            Binario.Byte(0xe9);
            Binario.Int16ReferenciaIndireta(reserva);
            //continua
        }

        public override int ReservarMarcador(TiposDeMarcador tipo)
        {
            return Binario.RegistrarMarcador(tipo);
        }
    }
}
