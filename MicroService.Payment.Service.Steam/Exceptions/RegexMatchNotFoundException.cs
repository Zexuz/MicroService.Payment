using System;

namespace MicroService.Payment.Service.Steam.Exceptions
{
    internal class RegexMatchNotFoundException : Exception
    {
        public RegexMatchNotFoundException(string s) : base(s)
        {
        }
    }
}