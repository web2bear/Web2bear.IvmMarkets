using System.Threading.Tasks;

namespace Web2bear.IvmMarkets
{
    public interface IGraphContext
    {
        Task SendArg(CalcNode argName, double argValue, CalcNode receiverNodeId);
    }
}