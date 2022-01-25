using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Calculator.ViewModels;

namespace Calculator.Views
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            var txt = this.Find<TextBox>("input");
            txt.KeyUp += (s, e) => ((MainWindowViewModel)this.DataContext).OnKeyPressed(s, e);
        }
    }
}