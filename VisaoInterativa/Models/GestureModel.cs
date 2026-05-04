using System;
using System.Timers;

namespace VisaoInterativa.Models
{
    public class GestureModel
    {
        // Eventos para notificar o Controller
        public delegate void GestureHandler(string gesture);
        public event GestureHandler GestureDetected;

        public delegate void CameraStatusHandler(bool isActive);
        public event CameraStatusHandler CameraStatusChanged;

        private Timer simulationTimer; // apenas para simular deteção de gestos

        public void StartCamera()
        {
            // Simulação: avisa que câmara está ativa
            CameraStatusChanged?.Invoke(true);

            // Simula um gesto a cada 3 segundos (alterna entre Avançar/Recuar)
            simulationTimer = new Timer(3000);
            simulationTimer.Elapsed += (s, e) =>
            {
                var random = new Random();
                var gesture = random.Next(2) == 0 ? "Avançar" : "Recuar";
                GestureDetected?.Invoke(gesture);
            };
            simulationTimer.Start();
        }

        public void StopCamera()
        {
            simulationTimer?.Stop();
            simulationTimer?.Dispose();
            CameraStatusChanged?.Invoke(false);
        }

        public void ReleaseResources()
        {
            StopCamera();
            // Outras paragens (se necessário no futuro)
        }
    }
}