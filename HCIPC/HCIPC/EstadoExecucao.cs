using System;
using System.Collections.Generic;
using HCIPC.Arvore;

namespace HCIPC
{
    public class EstadoExecucao
    {
        public EstadoExecucao NivelGlobal { get; set; }
        public EstadoExecucao NivelAcima { get; set; }
        public No NoAtual { get; set; }

        public object Valor { get; set; }

        private Dictionary<string, object> Variaveis { get; set; }

        public EstadoExecucao()
        {
            NivelGlobal = null;
            NivelAcima = null;
            Valor = null;
            Variaveis = new Dictionary<string, object>();
        }

        public object this[string nome]
        {
            get
            {
                if (Variaveis.ContainsKey(nome))
                {
                    return Variaveis[nome.ToLower()];
                }
                else
                {
                    if(NivelGlobal != null)
                    {
                        return NivelGlobal[nome.ToLower()];
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            set
            {
                if(Variaveis.ContainsKey(nome))
                {
                    Variaveis[nome.ToLower()] = value;
                }
                else
                {
                    Variaveis.Add(nome.ToLower(), value);
                }
            }
        }

    }
}
