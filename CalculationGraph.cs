using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Web2bear.IvmMarkets
{
    public class CalculationGraph : IGraphContext
    {
        private readonly Dictionary<string, IFunctionService> _nodes;
        public List<string> Calls { get; }

        public CalculationGraph()
        {
            _nodes = new Dictionary<string,IFunctionService>();
            Calls = new List<string>();
        }

        public CalculationGraph WithNode(IFunctionService node)
        {
            node.Graph = this;
            _nodes.Add(node.GetType().Name,node);
            return this;
        }
        
        public Task SendTo<T>(SetInputCommand command)
        {
            var receiverKey = typeof(T).Name;
            var receiver = _nodes.GetValueOrDefault(receiverKey);
            if (receiver == null)
                throw new Exception("Unregistered receiver");

            Calls.Add(receiverKey);
            
            return receiver.OnReceive(command);
        }

        public double CalculationResult { get; set; }
    }
}