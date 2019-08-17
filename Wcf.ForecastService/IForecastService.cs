using System.ServiceModel;
using System.Threading.Tasks;
using Wcf.ForecastService.Dtos;
using Wcf.ForecastService.Models;

namespace Wcf.ForecastService
{
    [ServiceContract]
    public interface IForecastService
    {
        [OperationContract]
        Task<ForecastDto> GetForecastAsync(GetForecastRequest requestModel);

        [OperationContract]
        Task<GeoObjectDto[]> GetAvailableGeoObjectsAsync();
    }
}
