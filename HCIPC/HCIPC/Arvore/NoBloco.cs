using System;
namespace HCIPC.Arvore
{
    public class NoBloco : NoBlocoBase
    {
        public NoBloco()
        {
        }

        protected override void Executar(ref EstadoExecucao estado)
        {
            foreach (var no in Nos)
            {
                no.ExecutarNo(ref estado);
            }
        }
    }
}
