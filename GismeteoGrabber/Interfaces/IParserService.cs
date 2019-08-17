using Domain.Entities;
using System.Threading.Tasks;

namespace GismeteoGrabber.Interfaces
{
    internal interface IParserService
    {
        Task<Forecast[]> ParseAsync();
    }
}
