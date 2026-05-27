using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace inventory.Services // 🟢 Make sure this namespace matches your folder structure!
{
    public class AdvancedBufferService : IDisposable, IAsyncDisposable
    {
        private Utf8JsonWriter? _jsonWriter;
        private FileStream? _fileStream;
        

        public AdvancedBufferService() 
            : this("logs/inventory_default_buffer.json") // Chains to the original one with a default string
        {
        }
        public AdvancedBufferService(string path)
        {
            _fileStream = new FileStream(path, FileMode.OpenOrCreate);
            _jsonWriter = new Utf8JsonWriter(_fileStream);
        }

        public async ValueTask DisposeAsync()
        {
            if (_jsonWriter != null)
            {
                await _jsonWriter.FlushAsync();
                _jsonWriter.Dispose();
            }
            if (_fileStream != null)
            {
                await _fileStream.DisposeAsync();
            }
            GC.SuppressFinalize(this);
        }

        public void Dispose()
        {
            _jsonWriter?.Flush();
            _jsonWriter?.Dispose();
            _fileStream?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}