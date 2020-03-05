using System;

namespace TES3.Util
{
    public static class ArgumentProcessing
    {
        public static void ProcessArguments(string[] args, IArgumentProcessor processor)
        {
            try
            {
                foreach (var arg in args)
                {
                    if (arg.StartsWith("--"))
                    {
                        processor.OnSwitch(arg.Substring(2));
                    }
                    else if (arg.StartsWith("-"))
                    {
                        for (var j = 1; j < arg.Length; ++j)
                        {
                            processor.OnOption(arg[j]);
                        }
                    }
                    else
                    {
                        processor.OnValue(arg);
                    }
                }

                processor.ValidateState();
            }
            catch (ConsoleApplicationException e)
            {
                Console.Error.WriteLine(e.Message);
                processor.PrintHelpMessage(new IndentWriter(Console.Out));
                throw e;
            }
        }
    }

    public interface IArgumentProcessor
    {
        void OnOption(char option);

        void OnSwitch(string name);

        void OnValue(string value);

        void ValidateState();

        void PrintHelpMessage(IndentWriter writer);
    }

    public class ConsoleApplicationException : Exception
    {
        public ConsoleApplicationException(string message, int returnCode = -1) : base(message)
        {
            ReturnCode = returnCode;
        }

        public int ReturnCode
        {
            get;
        }
    }


}
