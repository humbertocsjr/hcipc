using System;
using System.Collections.Generic;
using System.Threading;
using Fleck;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace HCIPCWEB
{
    class MainClass
    {
        public static Dictionary<IWebSocketConnection, Sessao> Sessoes = new Dictionary<IWebSocketConnection, Sessao>();


        public static void Main(string[] args)
        {
            Console.WriteLine("Humberto Costa | Interpretador de Pseudo-Código via WebSockets");

            var serv = new WebSocketServer("ws://0.0.0.0:1990");
            serv.Start(socket =>
            {
                socket.OnOpen = () => AoAbrir(socket);
                socket.OnClose = () => AoFechar(socket);
                socket.OnMessage = mensagem => AoReceber(socket, mensagem);
            });

            while (true)
            {
                foreach (KeyValuePair<IWebSocketConnection, Sessao> sessao in Sessoes.ToList())
                {
                    if(DateTime.Now.Subtract(sessao.Value.UltimoAcesso).TotalHours >= 6)
                    {
                        sessao.Value.Interpretador?.AbortarExecucao();
                        try
                        {
                            sessao.Key.Close();
                        }
                        catch (Exception)
                        {

                        }
                        Sessoes.Remove(sessao.Key);
                    }
                }
                Thread.Sleep(10000);
            }
        }

        public static void AoAbrir(IWebSocketConnection conex)
        {
            Sessoes.Add(conex, new Sessao()
            {
                Conexao = conex
            }) ;
            Console.WriteLine("[" + DateTime.Now.ToString() + "] " + conex.ConnectionInfo.Id.ToString() + " - Aberta conexão do IP" + conex.ConnectionInfo.ClientIpAddress.ToString());

        }

        public static void AoFechar(IWebSocketConnection conex)
        {
            Sessoes[conex].Interpretador?.AbortarExecucao();
            Sessoes.Remove(conex);
            Console.WriteLine("[" + DateTime.Now.ToString() + "] " + conex.ConnectionInfo.Id.ToString() + " - Fechada conexão do IP" + conex.ConnectionInfo.ClientIpAddress.ToString());
        }

        public static void AoReceber(IWebSocketConnection conex, string mensagem)
        {
            try
            {
                Sessoes[conex].ProcessarRetorno(mensagem);
                Console.WriteLine("[" + DateTime.Now.ToString() + "] " + conex.ConnectionInfo.Id.ToString() + " - Recebida mensagem conexão do IP" + conex.ConnectionInfo.ClientIpAddress.ToString());
            }
            catch (Exception ex)
            {

            }
        }
    }
}
