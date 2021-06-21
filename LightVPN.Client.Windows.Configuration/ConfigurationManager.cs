﻿using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using LightVPN.Client.Windows.Configuration.Interfaces;

namespace LightVPN.Client.Windows.Configuration
{
    public sealed class ConfigurationManager<T> : IConfigurationManager<T>
    {
        private readonly string _configurationPath;

        public ConfigurationManager(string configurationPath)
        {
            _configurationPath = configurationPath;
        }

        private void Verify()
        {
            if (!File.Exists(_configurationPath)) Write(new object());
        }

        public async Task WriteAsync(T value, CancellationToken cancellationToken = default)
        {
            Verify();

            var json = JsonSerializer.Serialize(value);
            await File.WriteAllTextAsync(_configurationPath, json, cancellationToken);
        }

        public void Write(T value)
        {
            Verify();

            var json = JsonSerializer.Serialize(value);
            File.WriteAllText(_configurationPath, json);
        }

        public async Task<T> ReadAsync(CancellationToken cancellationToken = default)
        {
            Verify();

            var fileContents = await File.ReadAllTextAsync(_configurationPath, cancellationToken);
            return JsonSerializer.Deserialize<T>(fileContents);
        }

        public T Read()
        {
            Verify();

            var fileContents = File.ReadAllText(_configurationPath);
            return JsonSerializer.Deserialize<T>(fileContents);
        }
    }
}