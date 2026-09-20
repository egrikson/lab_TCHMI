using System.IO;
using Microsoft.Extensions.Logging;

namespace DequeVisualizationWpf
{
    public class DequeAlgorithm : IAlgorithm
    {
        private DequeAlgorithmSettings? _settings;
        private readonly ILogger _logger;

        public DequeAlgorithm(ILogger logger)
        {
            _logger = logger;
        }

        public void Run(AlgorithmSettingsBase settings)
        {
            _settings = (DequeAlgorithmSettings)settings;
            _logger.LogInformation("Алгоритм запущен");
        }

        public void SaveToFile(string path) => File.WriteAllLines(path, _settings!.Deque.Select(x => x.ToString()));

        public void LoadFromFile(string path)
        {
            var lines = File.ReadAllLines(path);

            _settings!.Deque.Clear();

            foreach (var line in lines)
            {
                if (int.TryParse(line, out int value))
                    _settings.Deque.AddLast(value);
            }
        }
    }
}
