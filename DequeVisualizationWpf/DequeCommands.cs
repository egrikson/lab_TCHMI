using System.Windows.Input;

namespace DequeVisualizationWpf
{
    public static class DequeCommands
    {
        public static readonly RoutedUICommand AddFirst =
            new RoutedUICommand("Добавить в начало", "AddFirst", typeof(DequeCommands));

        public static readonly RoutedUICommand AddLast =
            new RoutedUICommand("Добавить в конец", "AddLast", typeof(DequeCommands));

        public static readonly RoutedUICommand RemoveFirst =
            new RoutedUICommand("Удалить из начала", "RemoveFirst", typeof(DequeCommands));

        public static readonly RoutedUICommand RemoveLast =
            new RoutedUICommand("Удалить с конца", "RemoveLast", typeof(DequeCommands));

        public static readonly RoutedUICommand Description =
            new RoutedUICommand("Описание алгоритма", "Description", typeof(DequeCommands));
    }
}
