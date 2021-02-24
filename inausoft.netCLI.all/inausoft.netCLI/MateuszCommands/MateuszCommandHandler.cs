using System;

namespace inausoft.netCLI.MateuszCommands
{
    public class MateuszCommandHandler : IMateuszCommandHandler<MateuszCommandHandler.MateuszCommand>
    {

        private MateuszCommand command;

        public MateuszCommandHandler()
        {
            command = new MateuszCommand();
        }

        public MateuszCommand GetCommand()
        {
            return command;
        }

        public int Run()
        {
            //Do some logic with arguments
            string exampleArgument = GetCommand().ExampleArgument ?? throw new ArgumentException();
            return 0;
        }

        public class MateuszCommand : IMateuszCommand
        {

            [Option("argument")]
            public string ExampleArgument { get; set; }

            public bool? OptionalParameter { get; set; }

        }

    }
}