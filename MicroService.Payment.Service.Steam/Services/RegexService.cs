using System.Text.RegularExpressions;
using MicroService.Payment.Service.Steam.Exceptions;

namespace MicroService.Payment.Service.Steam.Services
{
    internal class RegexService
    {
        public string GetFirstGroupMatch(Regex regex, string text, bool throwIfNotFound = true)
        {
            return GetNGroupMatch(1, regex, text, throwIfNotFound);
        }

        public string GetNGroupMatch(int n, Regex regex, string text, bool throwIfNotFound = true)
        {
            var match = regex.Match(text);
            var value = match.Groups[n].Value;

            if (string.IsNullOrEmpty(value) && throwIfNotFound)
                throw new RegexMatchNotFoundException($"The match on {text} with regex {regex} was not successfull");

            return value;
        }
    }
}