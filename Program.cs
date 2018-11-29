using System.IO;
using System.Linq;
using DatabaseUpgrade;
using Microsoft.Extensions.Configuration;

namespace DbUpgrade
{
    internal class Program
    {
        public static int Main(string[] args)
        {
            var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();

            var process = new Process(configuration);
            return process.MigrationProcess(args.FirstOrDefault());
        }
    }
}