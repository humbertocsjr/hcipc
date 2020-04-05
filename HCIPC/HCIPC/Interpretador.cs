using System;
using System.Collections.Generic;
using System.IO;
using HCIPC.Arvore;

namespace HCIPC
{
    public class Interpretador
    {
        Dictionary<string, Fonte> _fontes = new Dictionary<string, Fonte>();
        Fonte _inicial = null;
        EstadoExecucao _estado = new EstadoExecucao();

        public EntradaSaida EntradaSaidaPadrao { get; set; }

        public Interpretador()
        {
            //Inicializa com o módulo de E/S padrão, podendo ser subistituido
            EntradaSaidaPadrao = new EntradaSaidaTerminal();
            _estado.Interpretador = this;
        }

        public void AdicionarArquivo(string endereco, bool arquivoExecutavelPrincipal)
        {
            //Le o arquivo e converte
            AdicionarCodigoFonte(endereco, File.ReadAllText(endereco), arquivoExecutavelPrincipal);
        }

        public void AdicionarCodigoFonte(string arquivo, string codigo, bool arquivoExecutavelPrincipal)
        {
            //Le o codigo fonte, gravando como uma das bibliotecas atuais
            _fontes.Add(arquivo, new Fonte() { NomeDoArquivo = arquivo, CodigoFonte = codigo });
            //Gera a arvore de Nós onde ficam os Tokens do código fonte
            _fontes[arquivo].ProcessarCodigoFonte();
            //TODO: Futuramente guardar no EstadoExecucao, as funcoes e procedimentos
            //Caso seja o código fonte a ser executado, armazena agora
            if (arquivoExecutavelPrincipal) _inicial = _fontes[arquivo];
        }

        public List<string> CodigosFontesCarregados()
        {
            //Busca e retorna os arquivos armazenados na memoria
            List<string> retorno = new List<string>();
            foreach (var fonte in _fontes)
            {
                retorno.Add(fonte.Key);
            }
            return retorno;
        }

        public bool Executar()
        {
            //Executa o código marcado para tal
            return Executar(_inicial.NoRaiz, ref _estado);
        }

        private bool Executar(No raiz,  ref EstadoExecucao estado)
        {
            try
            {
                //Executa o nó raiz (Normalmente o nó "Algoritmo")
                raiz.ExecutarNo(ref estado);
                return true;
            }
            catch (Erro ex)
            {
                //Caso de um erro envia para o E/S para exibi-lo
                estado.ES.Erro(ex);
                return false;
            }
        }
    }
}
