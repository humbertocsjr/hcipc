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


            // Carrega o Código fonte
            var interpretador = new Interpretador();

            //TODO: Implementar leitura dos argumentos do Main para carregar quaisquer arquivos
            interpretador.AdicionarArquivo("teste.hcp", true);

            //Executa o código fonte até o fim ou interrompe se encontrar um erro
            if(interpretador.Executar())
            {
                Console.WriteLine();
                Console.WriteLine("==== Encerrado com Sucesso ====");
                Console.WriteLine("Pressione qualquer tecla para encerrar");
                Console.ReadKey();

            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("==== Encerrado com Erro ====");
                Console.WriteLine("Pressione qualquer tecla para encerrar");
                Console.ReadKey();

            }


        }
    }
}
