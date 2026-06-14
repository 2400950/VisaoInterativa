using VisaoInterativa.Interfaces;

namespace VisaoInterativa.Models
{
    // Implementação concreta do gesto "Avançar"
    public class AvancarGesture : IGesture
    {
        public bool IsAvançar => true;
        public bool IsRecuar => false;
        public string Nome => "Avançar";
    }
}