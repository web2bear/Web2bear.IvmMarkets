using System;
using System.Threading.Tasks;

namespace Web2bear.IvmMarkets
{
    public class FeFunctionService : IFunctionService
    {
        private readonly Func<double, double, double, double> _calculate;
        private double _e;
        private double _fcd;
        private double _fx;

        public FeFunctionService(Func<double, double, double, double> calculate)
        {
            _calculate = calculate;
            _e = 0d;
            _fcd = 0d;
            _fx = 0d;
        }

        public Task OnReceive(SetInputCommand command)
        {
            switch (command.ArgumentName)
            {
                case InputArgument.E:
                    _e = command.ArgumentValue;
                    break;
                case InputArgument.Fcd:
                    _fcd = command.ArgumentValue;
                    break;
                case InputArgument.Fx:
                    _fx = command.ArgumentValue;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            Graph.CalculationResult = _calculate(_e, _fcd, _fx);
            return Task.CompletedTask;
        }

        public IGraphContext Graph { get; set; }
    }
}