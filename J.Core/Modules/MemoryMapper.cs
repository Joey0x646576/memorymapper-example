using System;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Text.Json;

namespace J.Core.Modules
{
    public sealed class MemoryMapper<T> where T : class
    {
        /// <summary>
        /// Store a object in an memory mapped file, non-persisted
        /// </summary>
        /// <param name="mapName">The name of the memory-mapped file</param>
        /// <param name="entity">The model you want to serialize into a memory-mapped file</param>
        /// <param name="capacitySize">The maximum size, in bytes, to allocate to the memory-mapped file</param>
        public void CreateMemoryMappedFile(string mapName, T entity, long capacitySize)
        {
            try
            {
                // Creates a memory-mapped file with the specified name and size.
                var mmf = MemoryMappedFile.CreateOrOpen(mapName, capacitySize);

                // Creates a stream, this is meant for reading/writing to the mmf
                // You can define an offset e.g. 0x1C, but keep in mind you need this offset when reading
                // The length will remain at 0, this way we start at the offset and ends at the approx end of the memory-mapped file
                var mmvStream = mmf.CreateViewStream(0, 0);

                // Uses System.Text.Json, this became available in .NET Core, you might need external libraries to achieve this in .NET FW
                // If you are lazy, you could use the obsolete BinaryFormatter as well
                using var jsonWriter = new Utf8JsonWriter(mmvStream);
                JsonSerializer.Serialize(jsonWriter, entity);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
        }

        /// <summary>
        /// Store a string in an memory mapped file, non-persisted
        /// </summary>
        /// <param name="mapName">The name of the memory-mapped file</param>
        /// <param name="text">The text value written to the stream</param>
        /// <param name="capacitySize">The maximum size, in bytes, to allocate to the memory-mapped file</param>
        public void CreateMemoryMappedFile(string mapName, string text, long capacitySize)
        {
            try
            {
                // Creates a memory-mapped file with the specified name and size.
                var mmf = MemoryMappedFile.CreateOrOpen(mapName, capacitySize);

                // Creates a stream, this is meant for reading/writing to the mmf
                // You can define an offset e.g. 0x1C, but keep in mind you need this offset when reading
                // The length will remain at 0, this way we start at the offset and ends at the approx end of the memory-mapped file
                var mmvStream = mmf.CreateViewStream(0, 0);

                using var sr = new StreamWriter(mmvStream);
                sr.Write(text);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
        }

        /// <summary>
        /// Tries to read the content of the memory-mapped file
        /// </summary>
        /// <param name="mapName">The name of the memory-mapped file</param>
        /// <returns>Generic entity which resides in the ViewStream</returns>
        public T ReadMemoryMappedFileToEntity(string mapName)
        {
            try
            {
                // We use the mapName to find the existing memory-mapped file
                var mmf = MemoryMappedFile.OpenExisting(mapName);
                var mmvStream = mmf.CreateViewStream(0, 0);

                using var sr = new StreamReader(mmvStream);
                var json = sr.ReadToEnd().Trim('\0');

                return mmvStream.CanRead ? JsonSerializer.Deserialize<T>(json) : null;
            }
            catch (FileNotFoundException)
            {
                Debug.WriteLine("Memory-mapped file not found.");
                throw;
            }
        }

        /// <summary>
        /// Tries to read the content of the memory-mapped file
        /// </summary>
        /// <param name="mapName">The name of the memory-mapped file</param>
        /// <returns>Text which resides in the ViewStream</returns>
        public string ReadMemoryMappedFileToString(string mapName)
        {
            try
            {
                // We use the mapName to find the existing memory-mapped file
                var mmf = MemoryMappedFile.OpenExisting(mapName);
                var mmvStream = mmf.CreateViewStream(0, 0);

                using var sr = new StreamReader(mmvStream);
                var text = sr.ReadToEnd().Trim('\0');

                return mmvStream.CanRead ? text : null;
            }
            catch (FileNotFoundException)
            {
                Debug.WriteLine("Memory-mapped file not found.");
                throw;
            }
        }
    }
}
