using System;
namespace HCIPC.Arvore
{
    public class NoSoma : NoOperacaoMatematicaBase
    {

        public NoSoma()
        {
        }

        protected override void Executar(ref EstadoExecucao estado)
        {
            decimal valor = ProcessarNo(Item1, ref estado) + ProcessarNo(Item2, ref estado);
            estado.Valor = valor;
        }
    }
}
