namespace Web2bear.IvmMarkets
{
    public class SetInputCommand
    {
        public SetInputCommand(CalcNode argumentName, double argumentValue)
        {
            ArgumentName = argumentName;
            ArgumentValue = argumentValue;
        }

        public CalcNode ArgumentName { get; }
        public double ArgumentValue { get; }
    }
}