using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace DbUpgrade
{
    public static class ConfigurationExtensions
    {
        public static string GetValue(this IConfiguration configuration, string key)
        {
            var environmentValue = GetEnvironmentValue(key);

            return !string.IsNullOrWhiteSpace(environmentValue)
                ? environmentValue
                : configuration.GetSection(key).Value;
        }

        public static List<string> GetList(this IConfiguration configuration, string key)
        {
            var list = new List<string>();

            var count = configuration.GetSection(key).GetChildren().Count();

            for (var i = 0; i < count; i++) list.Add(configuration.GetValue(key + ":" + i));

            return list;
        }

        private static string GetEnvironmentValue(string key)
        {
            var envKey = key.Replace(@":", "_").ToUpper();
            return Environment.GetEnvironmentVariable(envKey);
        }
    }
}