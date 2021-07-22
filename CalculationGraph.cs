using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Web2bear.IvmMarkets
{
    public class CalculationGraph : IGraphContext
    {
        private readonly ITestOutputHelper _testOutput;
        private readonly Dictionary<CalcNode, ICalculationNode> _nodes;
        private double _calculationResult;
        public List<CalcNode> Calls { get; }

        public CalculationGraph(ITestOutputHelper testOutput)
        {
            _testOutput = testOutput;
            _nodes = new Dictionary<CalcNode, ICalculationNode>();
            Calls = new List<CalcNode>();
        }

        public CalculationGraph With(ICalculationNode node)
        {
            node.Graph = this;
            _nodes.Add(node.NodeId, node);
            return this;
        }


        public Task SendArg(CalcNode argName, double argValue, CalcNode receiverNodeId)
        {
            if (receiverNodeId == CalcNode.Result)
            {
                CalculationResult = argValue;
                return Task.CompletedTask;
            }

            var receiver = _nodes.GetValueOrDefault(receiverNodeId);
            if (receiver == null)
                throw new Exception("Unregistered receiver");

            Calls.Add(receiverNodeId);

            return receiver.ProcessInput(argName, argValue);
        }

        public double CalculationResult
        {
            get => _calculationResult;
            set
            {
                _testOutput.WriteLine(value.ToString());
                _calculationResult = value;
            }
        }
    }
}