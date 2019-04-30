using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace ImageProcessor.Core.Extensions
{
    public static class ConfigurationExtensions
    {
        public static string LoadValueOrThrowException(this IConfigurationRoot congiguration, string keyName, string missingKeyMessage = "")
        {
            var result = congiguration[keyName];
            if (String.IsNullOrEmpty(result))
                throw new Exception($"Unable to load [{keyName}]. " + missingKeyMessage);
            return result;
        }
    }
}
