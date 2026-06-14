using System;
using System.Timers;
using VisaoInterativa.Exceptions;
using VisaoInterativa.Interfaces;

namespace VisaoInterativa.Models
{
    // Model – responsável pela lógica de domínio (câmara, deteção de gestos).
    // Não conhece a View nem o Controller. Apenas notifica através de eventos.
    public class GestureModel
    {
        // Evento para notificar o Controller quando um gesto é detetado.
        public delegate void GestureHandler(IGesture gesture);
        public event GestureHandler GestureDetected;

        // Evento para notificar o Controller sobre o estado da câmara (ativa/inativa).
        public delegate void CameraStatusHandler(bool isActive);
        public event CameraStatusHandler CameraStatusChanged;

        private Timer simulationTimer;        // Temporizador que simula gestos (protótipo)
        private bool _isCameraAvailable = true; // Controla se a câmara está disponível (simulação)

        // Liga a câmara. Se não estiver disponível, lança exceção personalizada.
        public void StartCamera()
        {
            if (!_isCameraAvailable)
                throw new CameraNotAvailableException();

            CameraStatusChanged?.Invoke(true);  // Notifica que a câmara está ativa

            // Configura temporizador para gerar gestos aleatórios a cada 3 segundos
            simulationTimer = new Timer(3000);
            simulationTimer.Elapsed += (s, e) =>
            {
                var random = new Random();
                IGesture gesture = (random.Next(2) == 0)
                    ? (IGesture)new AvancarGesture()   // cast explícito
                    : new RecuarGesture();              // convertido implicitamente para IGesture
                GestureDetected?.Invoke(gesture);       // Notifica o Controller
            };
            simulationTimer.Start();
        }

        // Para a câmara e liberta o temporizador.
        public void StopCamera()
        {
            simulationTimer?.Stop();          // Interrompe o temporizador
            simulationTimer?.Dispose();       // Liberta os recursos do temporizador
            CameraStatusChanged?.Invoke(false); // Notifica que a câmara foi desligada
        }

        // Liberta todos os recursos (chamado no encerramento da aplicação).
        public void ReleaseResources()
        {
            StopCamera();  // Reutiliza a paragem da câmara
            // Outras libertações podem ser adicionadas aqui se necessário
        }

        // Método auxiliar para simular falha da câmara (usado em testes).
        public void SimulateCameraFailure(bool isAvailable)
        {
            _isCameraAvailable = isAvailable;  // Altera o estado simulado da câmara
        }
    }
}