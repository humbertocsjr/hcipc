using System;
namespace HCIPC.Arvore
{
    public class NoDivisao : NoOperacaoMatematicaBase
    {
        public NoDivisao()
        {
        }

        protected override void Executar(ref EstadoExecucao estado)
        {
            try
            {
                //Processa os itens e calcula o resultado deles
                decimal valor = ProcessarNo(Item1, ref estado) / ProcessarNo(Item2, ref estado);
                estado.Valor = valor;
            }
            catch (DivideByZeroException)
            {
                throw new Erro(this, "Divisão por zero");
            }
            catch (OverflowException)
            {
                throw new Erro(this, "Resultado do cálculo ultrapassa o limite de valor do destino");
            }
        }
    }
}
