using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCPC8086
{
    enum TiposDePonteiro
    {
        //Nao calcula posicoes
        Direto16,
        Direto8,
        //Calcula a diferenca entre a posicao que pediu e a posicao do ponteiro, ignorando os bytes usados pelo ponteiro, exemplo [0x04] 0x00 0x11 0x22 0x33 [DESTINO]
        Indireto16,
        Indireto8
    }

    class Referencia
    {
        public TiposDePonteiro Ponteiro { get; set; } = TiposDePonteiro.Direto16;
        public int PosicaoNoCodigo { get; set; } = -1;
    }

    class Marcador : Referencia
    {
        public HCIPC.Integracao.ArquiteturaDoCompilador.TiposDeMarcador Tipo { get; set; }
    }

    class ReferenciaNumerada : Referencia
    {
        public int CodigoDaReferencia { get; set; }
    }

    class ReferenciaNomeada : Referencia
    {
        public string Algoritimo { get; set; }
        public string Nome { get; set; }
    }

    class Rotina : Referencia
    {
        public string Algoritmo { get; set; }
        public string Nome { get; set; }
        public Dictionary<string, HCIPC.Integracao.ArquiteturaDoCompilador.TiposDeVariavel> Parametros { get; set; }
    }

    class Binario
    {
        public List<Referencia> Apontamentos { get; set; }
        public List<Marcador> Marcadores { get; set; }
        public List<Rotina> Rotinas { get; set; }

        public List<byte> Dados { get; private set; } = new List<byte>();

        public string AlgoritmoAtual { get; set } = "";
        public string RotinaAtual { get; set; } = "";


        public void Bytes(byte[] valor)
        {
            Dados.AddRange(valor);
        }

        public void Byte(byte valor)
        {
            Dados.Add(valor);
        }

        public void SByte(sbyte valor)
        {
            Dados.AddRange(BitConverter.GetBytes(valor));
        }

        public void UInt16(UInt16 valor)
        {
            Dados.AddRange(BitConverter.GetBytes(valor));
        }

        public void Int16(Int16 valor)
        {
            Dados.AddRange(BitConverter.GetBytes(valor));
        }

        public void SByteReferenciaNomeIndireta(string nome)
        {
            var r = new ReferenciaNomeada();
            if (nome.Contains('.'))
            {
                r.Algoritimo = nome.Split('.')[0].ToLower();
                r.Nome = RotinaAtual.ToLower() + "." + nome.Split('.')[1].ToLower();
            }
            else
            {
                r.Algoritimo = AlgoritmoAtual;
                r.Nome = RotinaAtual.ToLower() + "." + nome;
            }
            r.PosicaoNoCodigo = Dados.Count;
            r.Ponteiro = TiposDePonteiro.Indireto8;
            Apontamentos.Add(r);
            SByte(0);
        }

        public void SByteReferenciaNomeDireta(string nome)
        {
            var r = new ReferenciaNomeada();
            if (nome.Contains('.'))
            {
                r.Algoritimo = nome.Split('.')[0].ToLower();
                r.Nome = RotinaAtual.ToLower() + "." + nome.Split('.')[1].ToLower();
            }
            else
            {
                r.Algoritimo = AlgoritmoAtual;
                r.Nome = RotinaAtual.ToLower() + "." + nome;
            }
            r.PosicaoNoCodigo = Dados.Count;
            r.Ponteiro = TiposDePonteiro.Direto8;
            Apontamentos.Add(r);
            SByte(0);
        }

        public void Int16ReferenciaNomeDireta(string nome)
        {
            var r = new ReferenciaNomeada();
            if (nome.Contains('.'))
            {
                r.Algoritimo = nome.Split('.')[0].ToLower();
                r.Nome = RotinaAtual.ToLower() + "." + nome.Split('.')[1].ToLower();
            }
            else
            {
                r.Algoritimo = AlgoritmoAtual;
                r.Nome = RotinaAtual.ToLower() + "." + nome;
            }
            r.PosicaoNoCodigo = Dados.Count;
            r.Ponteiro = TiposDePonteiro.Direto16;
            Apontamentos.Add(r);
            Int16(0);
        }

        public void Int16ReferenciaDireta(int referencia)
        {
            var r = new ReferenciaNumerada();
            r.CodigoDaReferencia = referencia;
            r.PosicaoNoCodigo = Dados.Count;
            r.Ponteiro = TiposDePonteiro.Direto16;
            Apontamentos.Add(r);
            Int16(0);
        }

        public void Int16ReferenciaNomeIndireta(string nome)
        {
            var r = new ReferenciaNomeada();
            if (nome.Contains('.'))
            {
                r.Algoritimo = nome.Split('.')[0].ToLower();
                r.Nome = RotinaAtual.ToLower() + "." + nome.Split('.')[1].ToLower();
            }
            else
            {
                r.Algoritimo = AlgoritmoAtual;
                r.Nome = RotinaAtual.ToLower() + "." + nome;
            }
            r.PosicaoNoCodigo = Dados.Count;
            r.Ponteiro = TiposDePonteiro.Indireto16;
            Apontamentos.Add(r);
            Int16(0);
        }

        public void Int16ReferenciaIndireta(int referencia)
        {
            var r = new ReferenciaNumerada();
            r.CodigoDaReferencia = referencia;
            r.PosicaoNoCodigo = Dados.Count;
            r.Ponteiro = TiposDePonteiro.Indireto16;
            Apontamentos.Add(r);
            Int16(0);
        }

        public int RegistrarMarcador(HCIPC.Integracao.ArquiteturaDoCompilador.TiposDeMarcador tipo)
        {
            Marcador m = new Marcador()
            {
                Tipo = tipo,
                PosicaoNoCodigo = -1
            };
            Marcadores.Add(m);
            return Marcadores.IndexOf(m);
        }

        public void Marcar(int marcador)
        {
            Marcadores[marcador].PosicaoNoCodigo = Dados.Count;
        }

        public int LerUltimoMarcador(HCIPC.Integracao.ArquiteturaDoCompilador.TiposDeMarcador tipo)
        {
            return Marcadores.IndexOf(Marcadores.Where(m => m.Tipo == tipo).Last());
        }

    }
}
