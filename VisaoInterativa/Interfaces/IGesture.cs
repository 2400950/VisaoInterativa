namespace VisaoInterativa.Interfaces
{
    // Define o contrato para um gesto reconhecido.
    // Em vez de strings "Avançar" ou "Recuar", usamos este tipo.
    public interface IGesture
    {
        bool IsAvançar { get; }  // Indica se é o gesto Avançar
        bool IsRecuar { get; }   // Indica se é o gesto Recuar
        string Nome { get; }     // Nome amigável para feedback
    }
}