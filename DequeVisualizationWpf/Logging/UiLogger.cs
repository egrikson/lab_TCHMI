using Microsoft.Extensions.Logging;

namespace DequeVisualizationWpf.Logging
{
    /// <summary>
    /// Логгер, выводящий записи в область «Журнал событий» главного окна.
    /// Формат записи совпадает с <see cref="FileLogger"/>.
    /// </summary>
    public class UiLogger : ILogger
    {
        private readonly string _category;
        private readonly Action<string> _append;

        public UiLogger(string categoryName, Action<string> append)
        {
            _category = FileLogger.ShortCategory(categoryName);
            _append = append;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state,
            Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel))
                return;

            _append(FileLogger.Format(logLevel, _category, state, exception, formatter));
        }
    }
}
