using System;
namespace HCIPC
{
    public abstract class EntradaSaida
    {
        public EntradaSaida()
        {
        }


        public void Escreva(decimal valor)
        {
            Escreva(valor.ToString());
        }

        public void Escreva(int valor)
        {
            Escreva(valor.ToString());
        }

        public void Escreva(object valor)
        {
            Escreva(valor.ToString());
        }

        public abstract void Erro(Erro erro);
        public abstract void Enter();
        public abstract void Escreva(string valor);
        public abstract string Leia();
    }
}
