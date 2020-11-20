namespace HmvScraping.Console
{
    using System;
    using System.Linq;
    using Microsoft.Extensions.DependencyInjection;
    using CommandLine;
    using CommandLine.Text;
    using HmvScraping;

    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddHmvScraping();
            services.AddSingleton<IApplication, Application>();
            services.AddSingleton<IUserNotification, ConsoleUserNotification>();

            var provider = services.BuildServiceProvider();
            var app = provider.GetRequiredService<IApplication>();

            var result = Parser.Default.ParseArguments<Options>(args);
            if (result.Tag == ParserResultType.NotParsed)
            {
                return;
            }

            try
            {
                var options = (result as Parsed<Options>)?.Value;
                app.Run(options);
            }
            catch (Exception e)
            {
                ShowError(e);
                if (e is ArgumentException)
                {
                    ShowHelp(result);
                }
            }
        }

        private static void ShowError(Exception e)
        {
            Console.WriteLine("ERROR(S):");
            Console.WriteLine($"  {e.Message}");
            Console.WriteLine();
            Console.WriteLine($"{e.GetType().FullName}: {e.Message}");
            Console.Write("---> StackTrace: ");
            Console.WriteLine(e.StackTrace);
            var innerException = e.InnerException;
            while (innerException != null)
            {
                Console.Write("-----> Cause: ");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                innerException = innerException.InnerException;
            }
            Console.WriteLine();
        }

        private static void ShowHelp(ParserResult<Options> result)
        {
            var helpText = HelpText.AutoBuild(result, h => h, e =>
            {
                return e;
            });
            Console.WriteLine(helpText);
        }
    }
}
