using System;
using J.Core.Models;
using J.Core.Modules;

namespace Consumer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var memoryMappedAuth = new MemoryMapper<Auth>();

            // Reads the memory-mapped file, returns object
            var resultAuth = memoryMappedAuth.ReadMemoryMappedFileToEntity(Auth.MemoryMappedFileName);

            // Reads the memory-mapped file, returns string
            var resultString = memoryMappedAuth.ReadMemoryMappedFileToString(Auth.MemoryMappedFileName);


            Console.WriteLine(resultAuth.Bearer);
            Console.WriteLine(resultString);
            Console.ReadLine();
        }
    }
}
