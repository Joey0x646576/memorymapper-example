using System;

namespace J.Core.Models
{
    [Serializable]
    public class Auth
    {

        private const string memoryMappedFileName = "example";
        public static string MemoryMappedFileName => memoryMappedFileName;
        public string Bearer { get; set; }

        public Auth(string bearer)
        {
            Bearer = bearer;
        }
    }
}
