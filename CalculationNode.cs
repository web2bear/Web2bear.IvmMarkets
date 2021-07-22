using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace Web2bear.IvmMarkets
{
    public interface ICalculationNode
    {
        CalcNode NodeId { get; }
        IGraphContext Graph { get; set; }
        Task ProcessInput(CalcNode node, double value);
    }

    public class CalculationNode : ICalculationNode
    {
        private Func<ConcurrentDictionary<CalcNode, double>, double> _formula;

        private ConcurrentDictionary<CalcNode, double> _inputArgs;
        private CalcNode _outputNodeId;

        public CalculationNode(CalcNode nodeId)
        {
            NodeId = nodeId;
            _formula = args => 1d;
            _inputArgs = new ConcurrentDictionary<CalcNode, double>();
            _outputNodeId = CalcNode.Result;
        }

        public CalcNode NodeId { get; }

        public IGraphContext Graph { get; set; }

        public CalculationNode InputFrom(params CalcNode[] nodeIds)
        {
            _inputArgs = new ConcurrentDictionary<CalcNode, double>(
                nodeIds.ToDictionary(n => n, n => 0d));
            return this;
        }


        public CalculationNode UseFormula(Func<ConcurrentDictionary<CalcNode, double>, double> formula)
        {
            _formula = formula;
            return this;
        }

        public CalculationNode OutputTo(CalcNode nodeId)
        {
            _outputNodeId = nodeId;
            return this;
        }

        public Task ProcessInput(CalcNode node, double value)
        {
            if (!_inputArgs.ContainsKey(node))
                throw new ArgumentOutOfRangeException(node.ToString());

            _inputArgs[node] = value;

            var result = _formula(_inputArgs);

            return Graph.SendArg(NodeId, result, _outputNodeId);
        }
    }
}