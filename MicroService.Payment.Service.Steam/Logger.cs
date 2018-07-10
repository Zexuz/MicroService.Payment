using System;

namespace MicroService.Payment.Service.Steam
{
    public class Logger<TEntity>:ILogger<TEntity>
    {
        public void Info(string message)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write($"{typeof(TEntity).FullName}:");
            Console.ResetColor();
            Console.WriteLine($"{message}");   
        }
    }
}