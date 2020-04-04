using System;
namespace HCIPC.Arvore
{
    public abstract class NoOperacaoMatematicaBase : No
    {
        public No Item1 { get; set; }
        public No Item2 { get; set; }

        public NoOperacaoMatematicaBase()
        {
        }

        protected decimal ProcessarNo(No no, ref EstadoExecucao estado)
        {
            decimal valor = 0m;
            no.ExecutarNo(ref estado);
            if (estado.Valor is int)
                valor = (decimal)(int)estado.Valor;
            else if (estado.Valor is float)
                valor = (decimal)(float)estado.Valor;
            else if (estado.Valor is decimal)
                valor = (decimal)estado.Valor;
            else
                throw new Erro(no, "Esperado valor numérico para soma");
            return valor;
        }
    }
}
