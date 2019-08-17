using ForecastService;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Timers;
using System.Windows.Threading;

namespace Wpf.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        #region ctor

        private Timer _timer = new Timer(300) { Enabled = false };
        private readonly ForecastServiceClient _forecastServiceClient;

        public MainWindowViewModel(ForecastServiceClient forecastServiceClient)
        {
            _forecastServiceClient = forecastServiceClient;
            _timer.Elapsed += GeoObjectListBox_Selected;

            AvailableGeoObjects = new ObservableCollection<GeoObjectDto>(_forecastServiceClient.GetAvailableGeoObjects());

            AvailableGeoObjectsLB_SelectionChangedCmd = new DelegateCommand<GeoObjectDto>(
                x => AvailableGeoObjectsLB_SelectionChanged(x)
            );
        }

        #endregion

        #region Properties

        private GeoObjectDto _selectedGeoObject = new GeoObjectDto();

        private ForecastDto _forecastField = new ForecastDto();
        public ForecastDto Forecast
        {
            get { return _forecastField; }
            set
            {
                _forecastField = value;
                RaisePropertyChanged(nameof(Forecast));
            }
        }

        public ObservableCollection<GeoObjectDto> AvailableGeoObjects { get; set; }

        #endregion

        #region Cmd&Events

        private void GeoObjectListBox_Selected(object sender, ElapsedEventArgs e)
        {
            _timer.Stop();

            var requestModel = new GetForecastRequest { GeoObjectName = _selectedGeoObject.Name, Date = DateTime.Now.Date };
            Forecast = _forecastServiceClient.GetForecast(requestModel);
        }

        public DelegateCommand<GeoObjectDto> AvailableGeoObjectsLB_SelectionChangedCmd { get; }
        private void AvailableGeoObjectsLB_SelectionChanged(GeoObjectDto obj)
        {
            _timer.Stop();
            _timer.Start();
            _selectedGeoObject = obj;
        }

        #endregion
    }
}
