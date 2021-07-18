using System;
using System.Threading.Tasks;

namespace Web2bear.IvmMarkets
{
    public class FcdFunctionService : IFunctionService
    {
        private readonly Func<double, double, double, double, double> _calculate;
        private double _c;
        private double _d;
        private double _fab;
        private double _fy;

        public FcdFunctionService(Func<double, double, double, double, double> calculate)
        {
            _calculate = calculate;
            _c = 0d;
            _d = 0d;
            _fab = 0d;
            _fy = 0d;
        }

        public Task OnReceive(SetInputCommand command)
        {
            switch (command.ArgumentName)
            {
                case InputArgument.C:
                    _c = command.ArgumentValue;
                    break;
                case InputArgument.D:
                    _d = command.ArgumentValue;
                    break;
                case InputArgument.Fab:
                    _fab = command.ArgumentValue;
                    break;
                case InputArgument.Fy:
                    _fy = command.ArgumentValue;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var result = _calculate(_c, _d, _fab, _fy);
            return Graph.SendTo<FeFunctionService>(new SetInputCommand(InputArgument.Fcd, result));
        }

        public IGraphContext Graph { get; set; }
    }
}