namespace MicroService.Payment.Service.Steam.Exceptions
{
    internal class NoneExpectedResultException : System.Exception
    {
        public NoneExpectedResultException(string e) : base(e)
        {
        }
    }
}