﻿using System;
namespace HCIPC.Arvore
{
    public class NoSubtracao : NoOperacaoMatematicaBase
    {

        public NoSubtracao()
        {
        }

        protected override void Executar(ref EstadoExecucao estado)
        {
            try
            { 
                decimal valor = ProcessarNo(Item1, ref estado) - ProcessarNo(Item2, ref estado);
                estado.Valor = valor;
            }
            catch (OverflowException)
            {
                throw new Erro(this, "Resultado do cálculo ultrapassa o limite de valor do destino");
            }
        }
    }
}
