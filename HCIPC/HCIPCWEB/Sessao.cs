using System;
using System.Dynamic;
using System.Threading.Tasks;
using HCIPC;
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace HCIPCWEB
{
    public class Sessao
    {
        public DateTime Inicio { get; set; }
        public DateTime UltimoAcesso { get; set; }
        public Fleck.IWebSocketConnection Conexao { get; set; }
        public Interpretador Interpretador { get; set; }
        public string Entrada { get; set; }

        public Sessao()
        {
            Inicio = DateTime.Now;
            UltimoAcesso = DateTime.Now;
        }

        public void Carregar(string codigo)
        {
            UltimoAcesso = DateTime.Now;
            Interpretador = new Interpretador();
            Interpretador.EntradaSaidaPadrao = new EntradaSaidaWeb()
            {
                Sessao = this
            };

            Interpretador.AdicionarCodigoFonte("web", codigo, true);

        }

        public void ProcessarRetorno(string mensagem)
        {

            dynamic obj = JsonConvert.DeserializeObject(mensagem);
            string tipo = ((string)obj.Tipo).ToString().ToLower();
            switch (tipo)
            {
                case "codigo":
                    {
                        Carregar(((string)obj.Codigo.ToString()));
                    }
                    break;
                case "executar":
                    {
                        new Task(() =>
                        {
                            Interpretador.Executar();
                        }).Start();
                    }
                    break;
                case "entrada":
                    {
                        HtmlDocument html = new HtmlDocument();
                        html.LoadHtml((string)obj.Mensagem.ToString());
                        Entrada = html.DocumentNode.InnerText;
                    }
                    break;
                default:
                    Conexao.Close();
                    break;
            }
        }

        public void EnviarErro(int linha, int coluna, string mensagem)
        {
            HtmlDocument html = new HtmlDocument();
            html.LoadHtml(mensagem);
            dynamic obj = new ExpandoObject();
            obj.Tipo = "erro";
            obj.Linha = linha;
            obj.Coluna = coluna;
            obj.Mensagem = html.DocumentNode.InnerHtml;
            Enviar(obj);
        }

        public void EnviarEscreva(string mensagem)
        {
            HtmlDocument html = new HtmlDocument();
            html.LoadHtml(mensagem);
            dynamic obj = new ExpandoObject();
            obj.Tipo = "escreva";
            obj.Mensagem = html.DocumentNode.InnerHtml;
            Enviar(obj);
        }

        public void EnviarLeia()
        {
            dynamic obj = new ExpandoObject();
            obj.Tipo = "leia";
            Enviar(obj);
        }

        public void Enviar(ExpandoObject msg)
        {
            Conexao.Send(JsonConvert.SerializeObject(msg));
        }
    }
}
