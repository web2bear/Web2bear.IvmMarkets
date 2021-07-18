using System.Threading.Tasks;

namespace Web2bear.IvmMarkets
{
    public interface IGraphContext
    {
        Task SendTo<T>(SetInputCommand command);
        
        double CalculationResult { get; set; }
    }
}