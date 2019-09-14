using System;
using System.Collections.Generic;
using System.Text;

namespace Mannheim.Cli
{
    public class CommandInput : ICommandInput
    {
        public string Prompt(string promptString)
        {
            Console.Write($"{promptString}> ");
            return Console.ReadLine();
        }

        public string PromptSecret(string promptString)
        {
            Console.Write($"{promptString}> ");
            string input = "";
            var stopInput = false;
            while (!stopInput)
            {
                var key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.Enter:
                        stopInput = true;
                        Console.WriteLine();
                        break;
                    case ConsoleKey.Backspace:
                    case ConsoleKey.Delete:
                        if (input.Length > 0) input = input[0..^1];
                        Console.Write("<");
                        break;
                    default:
                        input += key.KeyChar;
                        Console.Write("*");
                        break;
                }
            }

            return input;
        }
    }

    public interface ICommandInput
    {
        string Prompt(string promptString);
        string PromptSecret(string promptString);
    }
}