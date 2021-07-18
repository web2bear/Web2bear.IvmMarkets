namespace Web2bear.IvmMarkets
{
    public class SetInputCommand
    {
        public SetInputCommand(InputArgument argumentName, double argumentValue)
        {
            ArgumentName = argumentName;
            ArgumentValue = argumentValue;
        }

        public InputArgument ArgumentName { get; }
        public double ArgumentValue { get; }
    }
}