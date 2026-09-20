using System.IO;
using Microsoft.Extensions.Logging;

namespace DequeVisualizationWpf.Logging
{
    public class FileLogger : ILogger
    {
        private static readonly object _sync = new();

        private readonly string _category;
        private readonly string _filePath;

        public FileLogger(string categoryName, string filePath)
        {
            _category = ShortCategory(categoryName);
            _filePath = filePath;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state,
            Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel))
                return;

            lock (_sync)
            {
                File.AppendAllText(_filePath, Format(logLevel, _category, state, exception, formatter) + Environment.NewLine);
            }
        }

        /// <summary>
        /// Формат записи по п. 3.4 пояснительной записки:
        /// дата и время, уровень, категория, текст сообщения.
        /// </summary>
        public static string Format<TState>(LogLevel logLevel, string category, TState state,
            Exception? exception, Func<TState, Exception?, string> formatter)
        {
            string message = formatter(state, exception);

            if (exception != null)
                message += " | " + exception.Message;

            return $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{LevelName(logLevel)}] [{category}] {message}";
        }

        public static string LevelName(LogLevel level) => level switch
        {
            LogLevel.Trace => "DEBUG",
            LogLevel.Debug => "DEBUG",
            LogLevel.Information => "INFO",
            LogLevel.Warning => "WARNING",
            LogLevel.Error => "ERROR",
            LogLevel.Critical => "ERROR",
            _ => "INFO"
        };

        public static string ShortCategory(string categoryName)
        {
            int dot = categoryName.LastIndexOf('.');
            return dot >= 0 ? categoryName.Substring(dot + 1) : categoryName;
        }
    }
}
