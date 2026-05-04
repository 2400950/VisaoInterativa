using System;
using VisaoInterativa.Models;

namespace VisaoInterativa.Controllers
{
    public class GestureController
    {
        private GestureModel _model;

        public event Action<string> OnFeedback;
        public event Action<string> OnStateChanged;

        public GestureController()
        {
            _model = new GestureModel();
            _model.GestureDetected += OnGestureDetected;
            _model.CameraStatusChanged += OnCameraStatusChanged;
        }

        public void StartCapture()
        {
            _model.StartCamera();
            OnStateChanged?.Invoke("A capturar...");
            OnFeedback?.Invoke("Câmara iniciada");
        }

        public void StopCapture()
        {
            _model.StopCamera();
            OnStateChanged?.Invoke("Parado");
            OnFeedback?.Invoke("Câmara parada");
        }

        public void Shutdown()
        {
            _model.ReleaseResources();
            OnFeedback?.Invoke("Recursos libertados");
            OnStateChanged?.Invoke("Encerrado");
        }

        private void OnGestureDetected(string gesture)
        {
            // Simular tecla (no futuro: enviar para SO)
            if (gesture == "Avançar")
                System.Diagnostics.Debug.WriteLine("Simular Seta Direita");
            else if (gesture == "Recuar")
                System.Diagnostics.Debug.WriteLine("Simular Seta Esquerda");

            OnFeedback?.Invoke($"Gesto: {gesture}");
        }

        private void OnCameraStatusChanged(bool isActive)
        {
            OnFeedback?.Invoke(isActive ? "Câmara ativada" : "Câmara desativada");
        }
    }
}
