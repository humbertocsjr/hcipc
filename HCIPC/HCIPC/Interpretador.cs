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
            EntradaSaidaPadrao = new EntradaSaidaTerminal();
            _estado.Interpretador = this;
        }

        public void AdicionarArquivo(string endereco, bool arquivoExecutavelPrincipal)
        {
            AdicionarCodigoFonte(endereco, File.ReadAllText(endereco), arquivoExecutavelPrincipal);
        }

        public void AdicionarCodigoFonte(string arquivo, string codigo, bool arquivoExecutavelPrincipal)
        {
            _fontes.Add(arquivo, new Fonte() { NomeDoArquivo = arquivo, CodigoFonte = codigo });
            _fontes[arquivo].ProcessarCodigoFonte();
            if (arquivoExecutavelPrincipal) _inicial = _fontes[arquivo];
        }

        public List<string> CodigosFontesCarregados()
        {
            List<string> retorno = new List<string>();
            foreach (var fonte in _fontes)
            {
                retorno.Add(fonte.Key);
            }
            return retorno;
        }

        public bool Executar()
        {
            return Executar(_inicial.NoRaiz, ref _estado);
        }

        private bool Executar(No raiz,  ref EstadoExecucao estado)
        {
            try
            {
                raiz.ExecutarNo(ref estado);
                return true;
            }
            catch (Erro ex)
            {
                estado.ES.Erro(ex);
                return false;
            }
        }
    }
}
