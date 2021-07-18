using System.Threading.Tasks;

namespace Web2bear.IvmMarkets
{
    public interface IFunctionService
    {
        Task OnReceive(SetInputCommand command);
        IGraphContext Graph { get; set; }
    }
}