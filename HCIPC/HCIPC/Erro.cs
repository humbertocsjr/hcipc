using System;
using HCIPC.Arvore;

namespace HCIPC
{
    public class Erro : Exception
    {
        public No No { get; set; }
        public string Mensagem { get; set; }
        public int Linha { get { return No.FonteLinha; } }
        public int Coluna { get { return No.FonteColuna; } }
        public int Posição { get { return No.FontePosicao; } }

        public Erro(No no, string mensagem)
        {
            No = no;
            Mensagem = mensagem;
        }
    }
}
