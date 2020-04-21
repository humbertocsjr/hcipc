using HCIPC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCPC8086
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Humberto Costa | Compilador de Pseudo-Codigo para 8086");
            Compilador comp = new Compilador();
            var arqSaida = "";
            List<string> arqs = new List<string>();
            for (int i = 0; i < args.Length; i++)
            {
                var arg = args[i];
                switch (arg.ToLower())
                {
                    case "-?":
                    case "/?":
                    case "-help":
                    case "/help":
                    case "-ajuda":
                    case "/ajuda":
                        Console.WriteLine("Argumentos aceitos....:");
                        Console.WriteLine(" -saida|-o  [NOME]    : Define o nome do arquivo de saida");
                        Console.WriteLine(" -dos                 : Sistema Operacional DOS com 8086");
                        Console.WriteLine(" -8086                : Processador 8086");
                        Console.WriteLine(" -com                 : Executavel .COM (ORG 0x100 CS=DS=ES=SS)");
                        Console.WriteLine(" -bin                 : Executavel .BIN (ORG 0x0 CS=DS=ES=SS)");
                        Console.WriteLine(" -mz                  : Executavel .EXE para DOS");
                        Console.WriteLine(" -ne                  : Executavel .EXE para Win16");
                        break;
                    case "-saida":
                    case "-o":
                        i++;
                        arqSaida = args[i];
                        break;
                    case "-dos":
                        comp.SistemaOperacional = new SisOpDOS();
                        comp.Arquitetura = new Arq8086();
                        break;
                    case "-8086":
                        comp.Arquitetura = new Arq8086();
                        break;
                    case "-com":
                        comp.SaidaExecutavel = new SaidaCOM();
                        break;
                    case "-bin":
                        comp.SaidaExecutavel = new SaidaBIN();
                        break;
                    case "-mz":
                        comp.SaidaExecutavel = new SaidaMZ();
                        break;
                    case "-ne":
                        comp.SaidaExecutavel = new SaidaNE();
                        break;
                    default:
                        if(!File.Exists(arg))
                        {
                            Console.WriteLine("Arquivo '" + arg + "' inexiste.");
                            return;
                        }
                        arqs.Add(arg);
                        break;
                }
            }
            foreach (var arq in arqs)
            {
                Console.Write(" Adicionando arquivo '" + arq + "' . . . ");
                try
                {
                    comp.AdicionarCodigoFonte(arq, File.ReadAllText(arq));
                    Console.WriteLine("[ OK ]");
                }
                catch (Erro ex)
                {
                    Console.WriteLine("[ FALHA ]");
                    try
                    {
                        Console.WriteLine(ex.No.Fonte.NomeDoArquivo + " - " + ex.Linha + " - " + ex.Coluna + " - " + ex.Mensagem);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine(ex.Mensagem);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("[ FALHA ]");
                    Console.WriteLine(ex.ToString());
                }
            }
            Console.Write(" Compilando . . .");
            var saida = comp.Compilar();
            if(string.IsNullOrWhiteSpace( arqSaida))
            {
                arqSaida = "a." + comp.SaidaExecutavel.ExtensaoPadrao;
            }
            Console.WriteLine("[ OK ]");
            Console.Write(" Gravando arquivo '" + arqSaida + "' . . . ");
            File.WriteAllBytes(arqSaida, saida);
            Console.WriteLine("[ OK ]");
        }
    }
}
