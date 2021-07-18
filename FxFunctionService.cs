using System;
using System.Threading.Tasks;

namespace Web2bear.IvmMarkets
{
    public class FxFunctionService : IFunctionService
    {
        private readonly Func<double, double> _calculate;
        private double _x;

        public FxFunctionService(Func<double, double> calculate)
        {
            _calculate = calculate;
            _x = 0d;
        }

        public Task OnReceive(SetInputCommand command)
        {
            switch (command.ArgumentName)
            {
                case InputArgument.X:
                    _x = command.ArgumentValue;
                    break;
            
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            var result = _calculate(_x);
            return Graph.SendTo<FeFunctionService>(new SetInputCommand(InputArgument.Fx, result));
        }

        public IGraphContext Graph { get; set; }
    }
}