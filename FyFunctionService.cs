using System;
using System.Threading.Tasks;

namespace Web2bear.IvmMarkets
{
    public class FyFunctionService : IFunctionService
    {
        private readonly Func<double, double> _calculate;
        private double _y;

        public FyFunctionService(Func<double, double> calculate)
        {
            _calculate = calculate;
            _y = 0d;
        }

        public Task OnReceive(SetInputCommand command)
        {
            switch (command.ArgumentName)
            {
                case InputArgument.Y:
                    _y = command.ArgumentValue;
                    break;
            
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            var result = _calculate(_y);
            return Graph.SendTo<FcdFunctionService>(new SetInputCommand(InputArgument.Fy, result));
        }

        public IGraphContext Graph { get; set; }
    }
}