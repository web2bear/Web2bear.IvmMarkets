using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Web2bear.IvmMarkets
{
    public class BasicTests
    {
        private readonly CalculationGraph _graph;

        public BasicTests(ITestOutputHelper testOutput)
        {
            _graph = new CalculationGraph(testOutput)
                .With(new CalculationNode(CalcNode.Fab)
                    .InputFrom(CalcNode.A, CalcNode.B)
                    .UseFormula(v => v[CalcNode.A] + v[CalcNode.B])
                    .OutputTo(CalcNode.Fcd))
                .With(new CalculationNode(CalcNode.Fcd)
                    .InputFrom(CalcNode.C, CalcNode.D, CalcNode.Fab, CalcNode.Fy)
                    .UseFormula(v => v[CalcNode.C] + v[CalcNode.D] + v[CalcNode.Fab] + v[CalcNode.Fy])
                    .OutputTo(CalcNode.Fe))
                .With(new CalculationNode(CalcNode.Fy)
                    .InputFrom(CalcNode.Y)
                    .UseFormula(v => v[CalcNode.Y] + 1)
                    .OutputTo(CalcNode.Fcd))
                .With(new CalculationNode(CalcNode.Fx)
                    .InputFrom(CalcNode.X)
                    .UseFormula(v => v[CalcNode.X] + 2)
                    .OutputTo(CalcNode.Fe))
                .With(new CalculationNode(CalcNode.Fe)
                    .InputFrom(CalcNode.E, CalcNode.Fcd, CalcNode.Fx)
                    .UseFormula(v => v[CalcNode.E] + v[CalcNode.Fcd] + v[CalcNode.Fx])
                    .OutputTo(CalcNode.Result));
        }

        [Fact]
        public async Task SetXShouldBeValid()
        {
            await _graph.SendArg(CalcNode.X, 10, CalcNode.Fx);

            Assert.Equal(12, _graph.CalculationResult);
            Assert.Collection(_graph.Calls,
                i => Assert.Equal(CalcNode.Fx, i),
                i => Assert.Equal(CalcNode.Fe, i));
        }

        [Fact]
        public async Task SetAShouldBeValid()
        {
            await _graph.SendArg(CalcNode.A, 10, CalcNode.Fab);

            Assert.Equal(10, _graph.CalculationResult);
            Assert.Collection(_graph.Calls,
                i => Assert.Equal(CalcNode.Fab, i),
                i => Assert.Equal(CalcNode.Fcd, i),
                i => Assert.Equal(CalcNode.Fe, i)
            );
        }

        [Fact]
        public async Task SetAcAndXShouldBeValid()
        {
            
            await Task.WhenAll(
                _graph.SendArg(CalcNode.C, 5, CalcNode.Fcd),
                _graph.SendArg(CalcNode.X, 5, CalcNode.Fx)
            );

            Assert.Equal(12, _graph.CalculationResult);
            Assert.Collection(_graph.Calls.OrderBy(x=>(int)x),
                i => Assert.Equal(CalcNode.Fx, i),
                i => Assert.Equal(CalcNode.Fcd, i),
                i => Assert.Equal(CalcNode.Fe, i),
                i => Assert.Equal(CalcNode.Fe, i)
            );
        }
        
        
        [Fact]
        public async Task MultisetShouldBeSafe()
        {
            var rnd =new Random();

            var tasks =Enumerable.Range(1, 10000)
                .Select(v =>DelayThenSend(v,rnd.Next(100, 500)) )
                .Append(_graph.SendArg(CalcNode.C, 5, CalcNode.Fcd));
            
            await Task.WhenAll(tasks);

        }

        private async Task DelayThenSend(double v,int delay)
        {
           await Task.Delay(delay);
           await _graph.SendArg(CalcNode.X, v, CalcNode.Fx);
        }
    }
}