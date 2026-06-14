using VisaoInterativa.Interfaces;

namespace VisaoInterativa.Models
{
    // Implementação concreta do gesto "Recuar"
    public class RecuarGesture : IGesture
    {
        public bool IsAvançar => false;
        public bool IsRecuar => true;
        public string Nome => "Recuar";
    }
}