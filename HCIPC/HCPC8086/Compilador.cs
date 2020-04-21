using HCIPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCPC8086
{
    class Compilador
    {
        public ArqBase Arquitetura { get; set; } = new Arq8086();
        public SisOpBase SistemaOperacional { get; set; } = new SisOpDOS();

        public Binario Binario { get; set; } = new Binario();
        public SaidaBase SaidaExecutavel { get; set; } = new SaidaCOM();

        public void AdicionarCodigoFonte(string nome, string codigo)
        {
            EstadoExecucao estado = new EstadoExecucao();
            Fonte fonte = new Fonte();
            fonte.CodigoFonte = codigo;
            var no = fonte.ProcessarCodigoFonte();
            Arquitetura.Compilador = this;
            Arquitetura.Binario = Binario;
            Arquitetura.SisOp = SistemaOperacional;
            SistemaOperacional.Compilador = this;
            SistemaOperacional.Binario = Binario;
            SistemaOperacional.Arquitetura = Arquitetura;
            no.Compilar(Arquitetura, ref estado);
        }

        public byte[] Compilar()
        {
            return SaidaExecutavel.VincularBinarios(new List<Binario>(new Binario[] { Binario }));
        }
    }
}
