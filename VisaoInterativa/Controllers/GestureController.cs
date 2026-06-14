using System;
using VisaoInterativa.Exceptions;
using VisaoInterativa.Interfaces;
using VisaoInterativa.Models;

namespace VisaoInterativa.Controllers
{
    // Controller – orquestra a interação entre View e Model.
    // Captura exceções, decide repetições e comunica com a View através de eventos.
    public class GestureController
    {
        private GestureModel _model;
        private int _retryCount = 0;
        private const int MaxRetries = 3;

        // Eventos para comunicação com a View (desacoplamento)
        public event Action<string> OnFeedback;            // Mensagens simples
        public event Action<string> OnStateChanged;        // Estado da aplicação
        public event Action<string, string[]> OnErrorWithOptions; // Erro com opções (Sim/Não)

        public GestureController()
        {
            _model = new GestureModel();
            _model.GestureDetected += OnGestureDetected;
            _model.CameraStatusChanged += OnCameraStatusChanged;
        }

        // Inicia a captura (chamado pela View)
        public void StartCapture()
        {
            try
            {
                _model.StartCamera();
                OnStateChanged?.Invoke("A capturar...");
                OnFeedback?.Invoke("Câmara iniciada");
                _retryCount = 0;
            }
            catch (CameraNotAvailableException ex)
            {
                _retryCount++;
                if (_retryCount <= MaxRetries)
                {
                    OnErrorWithOptions?.Invoke(
                        $"Erro: {ex.Message} Tentativa {_retryCount} de {MaxRetries}. Deseja tentar novamente?",
                        new[] { "Sim", "Não" }
                    );
                }
                else
                {
                    OnErrorWithOptions?.Invoke(
                        "Número máximo de tentativas atingido. A câmara continua indisponível.",
                        new[] { "OK" }
                    );
                }
                OnStateChanged?.Invoke("Erro na câmara");
            }
            catch (Exception ex)
            {
                OnErrorWithOptions?.Invoke($"Erro inesperado: {ex.Message}", new[] { "OK" });
                OnStateChanged?.Invoke("Erro");
            }
        }

        // Para a captura (chamado pela View)
        public void StopCapture()
        {
            try
            {
                _model.StopCamera();
                OnStateChanged?.Invoke("Parado");
                OnFeedback?.Invoke("Câmara parada");
            }
            catch (Exception ex)
            {
                OnErrorWithOptions?.Invoke($"Erro ao parar: {ex.Message}", new[] { "OK" });
            }
        }

        // Liberta recursos no encerramento
        public void Shutdown()
        {
            try
            {
                _model.ReleaseResources();
                OnFeedback?.Invoke("Recursos libertados");
                OnStateChanged?.Invoke("Encerrado");
            }
            catch (Exception ex)
            {
                // Apenas regista, não bloqueia
                System.Diagnostics.Debug.WriteLine($"Erro no encerramento: {ex.Message}");
            }
        }

        // Chamado quando o utilizador escolhe "Sim" no diálogo de erro
        public void RetryStartCapture()
        {
            StartCapture();
        }

        // Reage aos gestos recebidos do Model
        private void OnGestureDetected(IGesture gesture)
        {
            try
            {
                if (gesture.IsAvançar)
                    System.Diagnostics.Debug.WriteLine("Simular Seta Direita");
                else if (gesture.IsRecuar)
                    System.Diagnostics.Debug.WriteLine("Simular Seta Esquerda");

                OnFeedback?.Invoke($"Gesto: {gesture.Nome}");
            }
            catch (Exception ex)
            {
                OnFeedback?.Invoke($"Erro ao simular tecla: {ex.Message}");
            }
        }

        // Reage às mudanças de estado da câmara
        private void OnCameraStatusChanged(bool isActive)
        {
            OnFeedback?.Invoke(isActive ? "Câmara ativada" : "Câmara desativada");
        }
    }
}