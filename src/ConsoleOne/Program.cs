using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using MyCompany.MyExamples.ProjectParser.BusinessLayer.Configuration;
using MyCompany.MyExamples.ProjectParser.BusinessLayer.DependencyWalking;
using MyCompany.MyExamples.ProjectParser.BusinessLayer.DependencyWalking.Interfaces;
using MyCompany.MyExamples.ProjectParser.BusinessLayer.Parsers;
using MyCompany.MyExamples.ProjectParser.BusinessLayer.Parsers.Interfaces;

using Serilog;

namespace MyCompany.MyExamples.ProjectParser.ConsoleOne
{
    public static class Program
    {
        public static async Task<int> Main(string[] args)
        {
            /* easy concrete logger that uses a file for demos */
            Serilog.ILogger lgr = new Serilog.LoggerConfiguration()
                .WriteTo.File("MyCompany.MyExamples.ProjectParser.ConsoleOne.log.txt", rollingInterval: Serilog.RollingInterval.Day)
                .CreateLogger();

            try
            {
                /* look at the Project-Properties/Debug(Tab) for this environment variable */
                string environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                Console.WriteLine(string.Format("ASPNETCORE_ENVIRONMENT='{0}'", environmentName));
                Console.WriteLine(string.Empty);

                IConfigurationBuilder builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json")
                        .AddJsonFile($"appsettings.{environmentName}.json", true, true)
                        .AddEnvironmentVariables();

                IConfigurationRoot configuration = builder.Build();

                IServiceProvider servProv = BuildDi(configuration, lgr);

                string dotnetFrameworkCsProjFullFileName = @"C:\MySubFolder\MyCsProj.csproj";

                await RunParserDemo(servProv, dotnetFrameworkCsProjFullFileName);
            }
            catch (Exception ex)
            {
                string flattenMsg = GenerateFullFlatMessage(ex, true);
                Console.WriteLine(flattenMsg);
            }

            Console.WriteLine("Press ENTER to exit");
            Console.ReadLine();

            return 0;
        }

        private static async Task RunParserDemo(IServiceProvider servProv, string dotnetFrameworkCsProjFullFileName)
        {
            ICsharpProjectParser demo = servProv.GetService<ICsharpProjectParser>();
            var result = demo.Parse(dotnetFrameworkCsProjFullFileName, 1);
            string report = demo.ConvertCsParseResult(result, null, 1);
            Console.WriteLine(report);

            // Writes text to a temporary file and returns path 
            string fileName = System.IO.Path.GetTempFileName();
            fileName = fileName.Replace(".tmp", ".txt");
            System.IO.File.WriteAllText(fileName, report);
            System.Diagnostics.Process.Start(new ProcessStartInfo(fileName) { UseShellExecute = true });
            await Task.CompletedTask.ConfigureAwait(false);
        }

        private static string GenerateFullFlatMessage(Exception ex)
        {
            return GenerateFullFlatMessage(ex, false);
        }

        private static IServiceProvider BuildDi(IConfiguration configuration, Serilog.ILogger lgr)
        {
            IServiceCollection servColl = new ServiceCollection()
                .AddSingleton(lgr)
                .AddLogging();

            servColl.AddSingleton<ICsharpProjectParser, CsharpFrameworkProjectParser>();
            servColl.AddSingleton<IDependencyWalker, DependencyWalker>();

            /* hard coded for now */
            ParseSettings parseSettings = new ParseSettings();
            parseSettings.IgnorePrefixes = new List<string> { "System", "Microsoft", "mscorlib" };
            servColl.AddSingleton<ParseSettings>(parseSettings);

            /* need trace to see Oracle.EF statements */
            servColl.AddLogging(loggingBuilder => loggingBuilder.AddConsole().SetMinimumLevel(LogLevel.Trace));

            servColl.AddLogging(blder =>
            {
                blder.SetMinimumLevel(LogLevel.Trace); /* need trace to see Oracle.EF statements */
                blder.AddSerilog(logger: lgr, dispose: true);
            });

            ServiceProvider servProv = servColl.BuildServiceProvider();

            return servProv;
        }

        private static string GenerateFullFlatMessage(Exception ex, bool showStackTrace)
        {
            string returnValue;

            StringBuilder sb = new StringBuilder();
            Exception nestedEx = ex;

            while (nestedEx != null)
            {
                if (!string.IsNullOrEmpty(nestedEx.Message))
                {
                    sb.Append(nestedEx.Message + System.Environment.NewLine);
                }

                if (showStackTrace && !string.IsNullOrEmpty(nestedEx.StackTrace))
                {
                    sb.Append(nestedEx.StackTrace + System.Environment.NewLine);
                }

                if (ex is AggregateException)
                {
                    AggregateException ae = ex as AggregateException;

                    foreach (Exception aeflatEx in ae.Flatten().InnerExceptions)
                    {
                        if (!string.IsNullOrEmpty(aeflatEx.Message))
                        {
                            sb.Append(aeflatEx.Message + System.Environment.NewLine);
                        }

                        if (showStackTrace && !string.IsNullOrEmpty(aeflatEx.StackTrace))
                        {
                            sb.Append(aeflatEx.StackTrace + System.Environment.NewLine);
                        }
                    }
                }

                nestedEx = nestedEx.InnerException;
            }

            returnValue = sb.ToString();

            return returnValue;
        }
    }
}