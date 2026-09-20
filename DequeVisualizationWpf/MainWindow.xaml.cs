using DequeVisualizationWpf.Drawnings;
using DequeVisualizationWpf.Logging;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace DequeVisualizationWpf;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private const double StartX = 20;
    private const double StartY = 30;
    private const double StepX = 120;

    private readonly DequeAlgorithmSettings _settings;
    private readonly DequeAlgorithm _algorithm;
    private readonly ILogger<MainWindow> _logger;

    private readonly NodeRenderer nodeRenderer = new();
    private readonly ArrowRenderer arrowRenderer = new();
    private readonly HeadTailRenderer headTailRenderer = new();
    private readonly LayoutEngine layoutEngine = new();
    private readonly AnimationEngine animationEngine = new();

    private readonly ObservableCollection<string> _log = new();
    private double _lastWidth;

    public MainWindow()
    {
        InitializeComponent();

        listLog.ItemsSource = _log;

        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddProvider(new FileLoggerProvider("deque_log.txt"));
            builder.AddProvider(new UiLoggerProvider(AppendLog));
        });

        _logger = loggerFactory.CreateLogger<MainWindow>();

        _settings = new DequeAlgorithmSettings();
        _algorithm = new DequeAlgorithm(loggerFactory.CreateLogger<DequeAlgorithm>());
        _algorithm.Run(_settings);
    }

    private void btnAddFront_Click(object sender, RoutedEventArgs e)
    {
        if (int.TryParse(txtInput.Text, out int value))
        {
            _settings.Deque.AddFirst(value);
            _logger.LogInformation($"Добавлен элемент {value} в начало дека");
            DrawDeque();
        }
        else
        {
            _logger.LogWarning("Попытка добавить некорректное значение в начало");
        }
    }

    private void btnAddBack_Click(object sender, RoutedEventArgs e)
    {
        if (int.TryParse(txtInput.Text, out int value))
        {
            _settings.Deque.AddLast(value);
            _logger.LogInformation($"Добавлен элемент {value} в конец дека");
            DrawDeque();
        }
        else
        {
            _logger.LogWarning("Попытка добавить некорректное значение в конец");
        }
    }

    private void btnRemoveFront_Click(object sender, RoutedEventArgs e)
    {
        if (_settings.Deque.Count > 0)
        {
            int removed = _settings.Deque.First!.Value;
            _settings.Deque.RemoveFirst();
            _logger.LogInformation($"Удалён элемент {removed} из начала дека");
        }
        else
        {
            _logger.LogWarning("Попытка удалить элемент из пустого дека (начало)");
        }

        DrawDeque();
    }

    private void btnRemoveBack_Click(object sender, RoutedEventArgs e)
    {
        if (_settings.Deque.Count > 0)
        {
            int removed = _settings.Deque.Last!.Value;
            _settings.Deque.RemoveLast();
            _logger.LogInformation($"Удалён элемент {removed} из конца дека");
        }
        else
        {
            _logger.LogWarning("Попытка удалить элемент из пустого дека (конец)");
        }

        DrawDeque();
    }

    /// <summary>
    /// Перерисовка дека: узлы позиционируются на Canvas прикреплёнными
    /// свойствами Canvas.Left и Canvas.Top, связи рисуются между соседями.
    /// </summary>
    private void DrawDeque()
    {
        canvasDeque.Children.Clear();

        double wrapWidth = scrollDeque.ViewportWidth > 0
            ? scrollDeque.ViewportWidth
            : scrollDeque.ActualWidth;

        if (wrapWidth <= 0)
            wrapWidth = ActualWidth > 0 ? ActualWidth : Width;

        _lastWidth = scrollDeque.ActualWidth;

        double x = StartX;
        double y = StartY;
        double maxX = 0;
        double maxY = 0;

        Rect previous = Rect.Empty;
        Rect firstNode = Rect.Empty;
        Rect lastNode = Rect.Empty;

        foreach (var item in _settings.Deque)
        {
            var position = layoutEngine.GetNextPosition(x, y, NodeRenderer.NodeWidth, wrapWidth);
            bool wrapped = position.Y != y;

            x = position.X;
            y = position.Y;

            var node = nodeRenderer.CreateNode(item);
            Canvas.SetLeft(node, x);
            Canvas.SetTop(node, y);
            canvasDeque.Children.Add(node);

            _ = animationEngine.AnimateAppearance(node);

            var rect = new Rect(x, y, NodeRenderer.NodeWidth, NodeRenderer.NodeHeight);

            if (!previous.IsEmpty)
                arrowRenderer.DrawLink(canvasDeque, previous, rect, wrapped);

            if (firstNode.IsEmpty)
                firstNode = rect;

            lastNode = rect;
            previous = rect;

            maxX = Math.Max(maxX, rect.Right);
            maxY = Math.Max(maxY, rect.Bottom);

            x += StepX;
        }

        if (!firstNode.IsEmpty)
            headTailRenderer.DrawHead(canvasDeque, firstNode);

        if (!lastNode.IsEmpty)
            headTailRenderer.DrawTail(canvasDeque, lastNode);

        // Canvas не подстраивает размер под содержимое — задаём его явно,
        // иначе ScrollViewer не узнает, что прокручивать
        canvasDeque.Width = Math.Max(maxX + StartX, wrapWidth);
        canvasDeque.Height = Math.Max(maxY + StartY, canvasDeque.MinHeight);

        UpdateStatus();
    }

    private void UpdateStatus()
    {
        txtCount.Text = $"Элементов: {_settings.Deque.Count}";

        txtPointers.Text = _settings.Deque.Count == 0
            ? "HEAD: —   TAIL: —"
            : $"HEAD: {_settings.Deque.First!.Value}   TAIL: {_settings.Deque.Last!.Value}";
    }

    /// <summary>При изменении ширины окна связи перестраиваются заново.</summary>
    private void scrollDeque_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        if (Math.Abs(e.NewSize.Width - _lastWidth) < 1)
            return;

        DrawDeque();
    }

    private void AppendLog(string record)
    {
        if (!Dispatcher.CheckAccess())
        {
            Dispatcher.Invoke(() => AppendLog(record));
            return;
        }

        _log.Add(record);

        if (_log.Count > 500)
            _log.RemoveAt(0);

        scrollLog.ScrollToEnd();
    }

    private void menuSave_Click(object sender, RoutedEventArgs e)
    {
        var dlg = new Microsoft.Win32.SaveFileDialog
        {
            Filter = "Text files (*.txt)|*.txt"
        };

        if (dlg.ShowDialog() == true)
        {
            _algorithm.SaveToFile(dlg.FileName);
            _logger.LogInformation($"Дек сохранён в файл {dlg.FileName}");
        }
    }

    private void menuLoad_Click(object sender, RoutedEventArgs e)
    {
        var dlg = new Microsoft.Win32.OpenFileDialog
        {
            Filter = "Text files (*.txt)|*.txt"
        };

        if (dlg.ShowDialog() == true)
        {
            _algorithm.LoadFromFile(dlg.FileName);
            _logger.LogInformation($"Дек загружен из файла {dlg.FileName}");
            DrawDeque();
        }
    }

    private void menuDescription_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show(_settings.Description, "Описание алгоритма");
    }
}
