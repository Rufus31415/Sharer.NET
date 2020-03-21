using System;
using Sharer;
using System.Linq;
using System.Collections.Generic;

namespace Sharer.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            WriteHeader();

            SharerConnection connection;

        LBL_START:

            try
            {
                // ask for COM port
                string port = null;
                while (port == null)
                {
                    var ports = SharerConnection.GetSerialPortNames();
                    port = DisplayOptions("On which COM port your Arduino is connected to ?", ports);
                }

                WriteHeader();

                // ask for baurate
                int bauds = 0;
                while (bauds == 0) bauds = DisplayOptions("Choose a baudrate.", new int[] { 300, 600, 1200, 2400, 4800, 9600, 14400, 19200, 28800, 31250, 38400, 57600, 115200 });

                connection = new SharerConnection(port, bauds);
                connection.Connect();
            }
            catch (Exception ex)
            {
                ShowException(ex);
                goto LBL_START;
            }

        LBL_SHOW_OPTIONS:

            try
            {
                WriteHeader();

                // Build option list
                var options = new List<string>();
                options.Add(GET_INFO);
                options.AddRange(connection.Functions.Select((x) => $"{CALL}{x.Name}"));
                options.AddRange(connection.Variables.SelectMany((x) => new string[] { $"{READ}{x.Name}", $"{WRITE}{x.Name}" }));

                var answer = DisplayOptions("What would you like to do ?", options);
                Console.WriteLine();

                if (answer == null) goto LBL_SHOW_OPTIONS;
                else if (answer.StartsWith(GET_INFO)) Console.WriteLine(connection.GetInfos()); // print board info
                else if (answer.StartsWith(CALL))
                {
                    // call a function
                    var name = answer.Substring(CALL.Length);
                    var function = connection.Functions.FirstOrDefault((x) => string.Equals(x.Name, name));
                    if (function == null) throw new Exception($"No function called {name}");

                    Console.WriteLine($"Call function: {function.Prototype}");

                    var argValues = new List<string>();
                    foreach(var arg in function.Arguments)
                    {
                        Console.Write($"  {arg.Name}= ");
                        argValues.Add(Console.ReadLine());
                    }

                    var result = connection.Call(name, argValues.ToArray());

                    Console.Write("--> ");
                    Console.WriteLine(result);
                }
                else if (answer.StartsWith(READ))
                {
                    // read variable
                    var name = answer.Substring(READ.Length);
                    var variable = connection.Variables.FirstOrDefault((x) => string.Equals(x.Name, name));
                    if (variable == null) throw new Exception($"No variable called {name}");

                    var value = connection.ReadVariable(name);
                    Console.WriteLine($"  {variable.Type.ToString()} {name} = {value?.ToString()}");
                }
                else if (answer.StartsWith(WRITE))
                {
                    // write variable
                    var name = answer.Substring(WRITE.Length);
                    Console.Write($"Enter a new value for {name}: ");
                    var value = Console.ReadLine();

                    if (connection.WriteVariable(name, value)) Console.WriteLine("OK");
                    else Console.WriteLine("Failed to write variable");
                }
                else Console.Write("Answer not understood !?");

            }
            catch (Exception ex) { ShowException(ex); }

            Console.WriteLine("Press any key to continue testing Sharer...");
            Console.Read();
            goto LBL_SHOW_OPTIONS;

        }

        private static void ShowException(Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ex.Message);
            Console.ResetColor();
        }

        private static T DisplayOptions<T>(string question, IEnumerable<T> options)
        {
            Console.WriteLine(question);
            if (options == null || options.Count() == 0) {
                Console.WriteLine("  No options to choose");
                return default(T);
            }

            for (int i = 0; i < options.Count(); i++) Console.WriteLine($"  {i}: {options.ElementAt(i)}");

            while (true)
            {
                Console.Write("Answer (empty to retry): ");
                var answer = Console.ReadLine();

                if (string.IsNullOrEmpty(answer)) return default(T);
                else if (int.TryParse(answer, out int num) && num >= 0 && num < options.Count()) return options.ElementAt(num);
                else Console.WriteLine($"Invalid answer \"{answer}\", please enter a number between 0 and {options.Count() - 1}");
            }
        }

        private static void WriteHeader()
        {
            Console.Clear();
            Console.WriteLine("    ███████╗██╗  ██╗ █████╗ ██████╗ ███████╗██████╗ ");
            Console.WriteLine("    ██╔════╝██║  ██║██╔══██╗██╔══██╗██╔════╝██╔══██╗");
            Console.WriteLine("    ███████╗███████║███████║██████╔╝█████╗  ██████╔╝");
            Console.WriteLine("    ╚════██║██╔══██║██╔══██║██╔══██╗██╔══╝  ██╔══██╗");
            Console.WriteLine("    ███████║██║  ██║██║  ██║██║  ██║███████╗██║  ██║");
            Console.WriteLine("    ╚══════╝╚═╝  ╚═╝╚═╝  ╚═╝╚═╝  ╚═╝╚══════╝╚═╝  ╚═╝");
        }

        private const string CALL = "Call function ";
        private const string WRITE = "Write variable ";
        private const string READ = "Read variable ";
        private const string GET_INFO = "Get board information";
    }


}
