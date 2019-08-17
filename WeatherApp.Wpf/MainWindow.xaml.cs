using ForecastService;
using System.Windows;
using Wpf.ViewModels;

namespace Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ForecastServiceClient _forecastServiceClient;

        public MainWindow()
        {
            InitializeComponent();

            _forecastServiceClient = new ForecastServiceClient();

            DataContext = new MainWindowViewModel(_forecastServiceClient);
        }
    }
}
