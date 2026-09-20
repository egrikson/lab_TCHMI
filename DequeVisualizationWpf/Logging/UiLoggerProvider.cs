using Microsoft.Extensions.Logging;

namespace DequeVisualizationWpf.Logging
{
    public class UiLoggerProvider : ILoggerProvider
    {
        private readonly Action<string> _append;

        public UiLoggerProvider(Action<string> append)
        {
            _append = append;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new UiLogger(categoryName, _append);
        }

        public void Dispose() { }
    }
}
