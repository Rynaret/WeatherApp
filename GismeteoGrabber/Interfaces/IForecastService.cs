using Domain.Entities;
using System.Threading.Tasks;

namespace GismeteoGrabber.Interfaces
{
    public interface IForecastService
    {
        Task SaveParsedDataAsync(Forecast[] forecasts);
    }
}
