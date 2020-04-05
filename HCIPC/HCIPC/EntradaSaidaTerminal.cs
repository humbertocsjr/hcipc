using System;
namespace HCIPC
{
    public class EntradaSaidaTerminal : EntradaSaida
    {
        public EntradaSaidaTerminal()
        {
        }

        public override void Erro(Erro erro)
        {
            Console.WriteLine();
            Console.WriteLine();
            var cor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("ERRO............: Linha " + erro.Linha + " Coluna " + erro.Coluna + " - " + erro.Mensagem);
            var linha = erro.No.Fonte.ExtrairLinha(erro.Linha);
            if(linha !=null)
            {
                Console.WriteLine("Código com erro.:");

                Console.WriteLine(linha);
                Console.Write(new string(' ', erro.Coluna -1));
                Console.WriteLine("*");

            }
            Console.ForegroundColor = cor;
        }

        public override void Enter()
        {
            Console.WriteLine();
        }

        public override void Escreva(string valor)
        {
            Console.Write(valor);
        }

        public override string Leia()
        {
            return Console.ReadLine();
        }
    }
}
