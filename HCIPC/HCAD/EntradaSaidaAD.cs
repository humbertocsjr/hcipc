using System;
using GLib;
using Gtk;
using HCIPC;

namespace HCAD
{
    public class EntradaSaidaAD : HCIPC.EntradaSaida
    {
        public MainWindow Janela { get; set; }
        public TextView Editor { get; set; }
        public TextView Saida { get; set; }
        public Entry Entrada { get; set; }
        public bool EntradaLiberada { get; set; }

        public EntradaSaidaAD()
        {
        }

        public override void Enter()
        {
            Gtk.Application.Invoke(delegate {
                Saida.Buffer.Text += Environment.NewLine;
            });
        }

        public override void Erro(Erro erro)
        {
            Enter();
            Enter();
            Escreva("ERRO............: Linha " + erro.Linha + " Coluna " + erro.Coluna + " - " + erro.Mensagem);
            var linha = erro.No.Fonte.ExtrairLinha(erro.Linha);
            if (linha != null)
            {
                Escreva("Código com erro.: [O '*' marca o local do erro]");
                Enter();
                Escreva(linha);
                Enter();
                Escreva(new string(' ', erro.Coluna - 1));
                Escreva("*");
                Enter();

            }
        }

        public override void Escreva(string valor)
        {
            Gtk.Application.Invoke(delegate {
                Saida.Buffer.Text += valor;
            });
        }

        public override string Leia()
        {

            Gtk.Application.Invoke(delegate {
                Entrada.GrabFocus();
            });
            while (!EntradaLiberada)
            {
                System.Threading.Thread.Sleep(100);
            }

            var tmp = Entrada.Text;
            Gtk.Application.Invoke(delegate {
                EntradaLiberada = false;
                Entrada.Text = "";
            });

            Escreva(tmp);
            Enter();

            return tmp;

        }
    }
}
