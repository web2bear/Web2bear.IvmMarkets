using System.Threading.Tasks;
using Xunit;

namespace Web2bear.IvmMarkets
{
    public class BasicTests
    {
        private readonly CalculationGraph _graph;

        public BasicTests()
        {
            _graph = new CalculationGraph()
                .WithNode(new FabFunctionService((a, b) => a + b))
                .WithNode(new FcdFunctionService((c, d, fab, fy) => c + d + fab + fy))
                .WithNode(new FyFunctionService((y) => y + 1))
                .WithNode(new FxFunctionService((x) => x + 2))
                .WithNode(new FeFunctionService((e, fcd, fx) => e + fcd + fx));
        }

        [Fact]
        public async Task SetXShouldBeValid()
        {
            await _graph.SendTo<FxFunctionService>(new SetInputCommand(InputArgument.X, 10));

            Assert.Equal(12, _graph.CalculationResult);
            Assert.Collection(_graph.Calls,
                i => Assert.Equal("FxFunctionService", i),
                i => Assert.Equal("FeFunctionService", i));
        }
        
        [Fact]
        public async Task SetAShouldBeValid()
        {
            await _graph.SendTo<FabFunctionService>(new SetInputCommand(InputArgument.A, 10));

            Assert.Equal(10, _graph.CalculationResult);
            Assert.Collection(_graph.Calls,
                i => Assert.Equal("FabFunctionService", i),
                i => Assert.Equal("FcdFunctionService", i),
                i => Assert.Equal("FeFunctionService", i)
                );
        }
        
        [Fact]
        public async Task SetAcAndXShouldBeValid()
        {
            await _graph.SendTo<FcdFunctionService>(new SetInputCommand(InputArgument.C, 5));
            await _graph.SendTo<FxFunctionService>(new SetInputCommand(InputArgument.X, 5));

            Assert.Equal(12, _graph.CalculationResult);
            Assert.Collection(_graph.Calls,
                i => Assert.Equal("FcdFunctionService", i),
                i => Assert.Equal("FeFunctionService", i),
                i => Assert.Equal("FxFunctionService", i),
                i => Assert.Equal("FeFunctionService", i)
            );
        }
    }
}