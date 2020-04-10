using System;
using System.Threading;
using HCIPC;

namespace HCIPCWEB
{
    public class EntradaSaidaWeb : EntradaSaida
    {
        public Sessao Sessao { get; set; }

        public EntradaSaidaWeb()
        {
        }

        public override void Enter()
        {
            Sessao.EnviarEscreva("\n");
        }

        public override void Erro(Erro erro)
        {
            Sessao.EnviarErro(erro.Linha, erro.Coluna, erro.Mensagem);
        }

        public override void Escreva(string valor)
        {
            Sessao.EnviarEscreva(valor);
        }

        public override string Leia()
        {
            Sessao.Entrada = null;
            Sessao.EnviarLeia();
            while (Sessao.Entrada == null & !Sessao.Interpretador.Abortar)
            {
                Thread.Sleep(150);
            }
            return Sessao.Entrada;
        }

        public override void InformarParada(string arquivo, int linha)
        {
        }
    }
}
