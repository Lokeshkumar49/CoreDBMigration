using System;
using System.Reflection;
using DbUp;
using DbUpgrade;
using Microsoft.Extensions.Configuration;

namespace DatabaseUpgrade
{
    public class Process
    {
        private readonly IConfiguration _configuration;

        public Process(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public int MigrationProcess(string arg)
        {
            try
            {
                var connectionString = GetConnectionString(arg);
                Console.WriteLine(connectionString);

                var upgrader =
                    DeployChanges.To
                        .PostgresqlDatabase(connectionString)
                        .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                        .LogToConsole()
                        .Build();

                var result = upgrader.PerformUpgrade();

                if (!result.Successful)
                    return UnSuccessful(result.Error.Message);

                return Successful();
            }
            catch (Exception ex)
            {
                return UnSuccessful(ex.Message);
            }
        }

        private string GetConnectionString(string arg)
        {
            if (arg != null) return arg;

            return _configuration.GetValue("Db:Connection_String");
        }

        private int Successful()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success!");
            Console.ResetColor();
            return 0;
        }

        private int UnSuccessful(string errorMessage)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(errorMessage);
            Console.ResetColor();
            return -1;
        }
    }
}