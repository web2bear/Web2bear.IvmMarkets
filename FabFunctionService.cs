using System;
using System.Threading.Tasks;

namespace Web2bear.IvmMarkets
{
    public class FabFunctionService : IFunctionService
    {
        
        private readonly Func<double, double, double> _calculate;
        private double _a;
        private double _b;

        public FabFunctionService(Func<double, double, double> calculate)
        {
            _calculate = calculate;
            _a = 0d;
            _b = 0d;
        }

        public  Task OnReceive(SetInputCommand command)
        {
            switch (command.ArgumentName)
            {
                case InputArgument.A:
                    _a = command.ArgumentValue;
                    break;
                case InputArgument.B:
                    _b = command.ArgumentValue;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            var result = _calculate(_a, _b);
            return Graph.SendTo<FcdFunctionService>(new SetInputCommand(InputArgument.Fab, result));
        }

        public IGraphContext Graph { get; set; }
    }
}