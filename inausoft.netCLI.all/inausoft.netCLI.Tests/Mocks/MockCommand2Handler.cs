namespace inausoft.netCLI.Tests.Mocks
{
    class MockCommand2Handler : CommandHandler<Command2>
    {
        public Command2 LastRunParameters { get; private set; }

        public override int Run(Command2 command)
        {
            LastRunParameters = command;

            return 0;
        }
    }
}
