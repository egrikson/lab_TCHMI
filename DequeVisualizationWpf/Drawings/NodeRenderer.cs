using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DequeVisualizationWpf.Drawnings;

/// <summary>
/// Создаёт визуальный узел дека: блок из двух секций —
/// значение элемента и область указателей.
/// </summary>
public class NodeRenderer
{
    public const double NodeWidth = 80;
    public const double NodeHeight = 40;

    private const double ValueWidth = 50;
    private const double PointerWidth = 30;

    public Border CreateNode(int value)
    {
        var grid = new Grid();
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(ValueWidth) });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(PointerWidth) });

        var valueBox = new TextBlock
        {
            Text = value.ToString(),
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            FontWeight = FontWeights.Bold
        };
        Grid.SetColumn(valueBox, 0);

        var pointerBox = new Border
        {
            BorderBrush = Brushes.Black,
            BorderThickness = new Thickness(1, 0, 0, 0),
            Child = new TextBlock
            {
                Text = "•",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Foreground = Brushes.DimGray
            }
        };
        Grid.SetColumn(pointerBox, 1);

        grid.Children.Add(valueBox);
        grid.Children.Add(pointerBox);

        return new Border
        {
            Width = NodeWidth,
            Height = NodeHeight,
            Background = Brushes.White,
            BorderBrush = Brushes.Black,
            BorderThickness = new Thickness(1),
            Child = grid
        };
    }
}
