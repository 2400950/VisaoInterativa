using System;

namespace VisaoInterativa.Exceptions
{
    // Exceção específica para erros de acesso à câmara.
    public class CameraNotAvailableException : Exception
    {
        // Mensagem predefinida
        public CameraNotAvailableException()
            : base("A câmara não está disponível ou está a ser usada por outra aplicação.")
        {
        }

        // Mensagem personalizada
        public CameraNotAvailableException(string message) : base(message)
        {
        }

        // Mensagem com exceção interna
        public CameraNotAvailableException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}