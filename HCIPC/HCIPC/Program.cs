using System;
using System.IO;
using HCIPC.Arvore;

namespace HCIPC
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.Title = "Humberto Costa | Interpretador de Pseudo-Código [Github: humbertocsjr]";

            // Cria o "EstadoExecucao" que armazena o ambiente de execução (Variaveis, Estado atual do interpretador, etc)
            var exe = new EstadoExecucao();

            // Carrega o Código fonte
            var fonte = new Fonte();

            //TODO: Implementar leitura dos argumentos do Main para carregar quaisquer arquivos
            fonte.CodigoFonte = File.ReadAllText("teste.hcp");

            // Processa o código fonte, gerando a árvore de nós(Tokens do Código fonte)
            var no = fonte.ProcessarCodigoFonte();

            // Inicia o interpretador pelo Nó raiz da Arvore
            no.ExecutarNo(ref exe);

            Console.WriteLine();
            Console.WriteLine("==== Encerrado ====");
            Console.WriteLine("Pressione qualquer tecla para encerrar");
            Console.ReadKey();

        }
    }
}
