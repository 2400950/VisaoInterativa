using System;
using System.Windows;
using VisaoInterativa.Controllers;

namespace VisaoInterativa.Views
{
    public partial class MainWindow : Window
    {
        private GestureController _controller;

        public MainWindow()
        {
            InitializeComponent();
            _controller = new GestureController();

            // Subscreve eventos do Controller para atualizar a UI
            _controller.OnFeedback += (feedback) =>
                Dispatcher.Invoke(() => tbFeedback.Text = feedback);
            _controller.OnStateChanged += (state) =>
                Dispatcher.Invoke(() => tbEstado.Text = state);
            _controller.OnErrorWithOptions += (message, options) =>
                Dispatcher.Invoke(() => ShowErrorDialog(message, options));
        }

        private void BtnIniciar_Click(object sender, RoutedEventArgs e)
        {
            _controller.StartCapture();
        }

        private void BtnParar_Click(object sender, RoutedEventArgs e)
        {
            _controller.StopCapture();
        }

        // Mostra diálogo de erro com opções (Sim/Não ou OK)
        private void ShowErrorDialog(string message, string[] options)
        {
            if (options.Length > 0 && options[0] == "Sim")
            {
                var result = MessageBox.Show(message, "Erro", MessageBoxButton.YesNo, MessageBoxImage.Error);
                if (result == MessageBoxResult.Yes)
                    _controller.RetryStartCapture();
            }
            else
            {
                MessageBox.Show(message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            _controller.Shutdown();
            base.OnClosing(e);
        }
    }
}