using System;
using J.Core.Models;
using J.Core.Modules;

namespace Producer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            const string bearerToken = "bearer ... from producer";

            var auth = new Auth(bearerToken);
            var memoryMapper = new MemoryMapper<Auth>();

            // Memory-mapped file from object
            memoryMapper.CreateMemoryMappedFile(Auth.MemoryMappedFileName, auth, 1000);

            Console.ReadLine();
        }
    }
}
