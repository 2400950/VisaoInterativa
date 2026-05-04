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
            _controller.OnFeedback += (feedback) =>
                Dispatcher.Invoke(() => tbFeedback.Text = feedback);
            _controller.OnStateChanged += (state) =>
                Dispatcher.Invoke(() => tbEstado.Text = state);
        }

        private void BtnIniciar_Click(object sender, RoutedEventArgs e)
        {
            _controller.StartCapture();
        }

        private void BtnParar_Click(object sender, RoutedEventArgs e)
        {
            _controller.StopCapture();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            _controller.Shutdown();
            base.OnClosing(e);
        }
    }
}
