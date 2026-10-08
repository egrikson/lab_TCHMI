using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DequeVisualizationWpf.Drawnings;

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
            Background = (Brush)Application.Current.FindResource("PointerBrush"),
            BorderBrush = (Brush)Application.Current.FindResource("NodeBorderBrush"),
            BorderThickness = new Thickness(1, 0, 0, 0),
            Child = new TextBlock
            {
                Text = "•",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Foreground = (Brush)Application.Current.FindResource("TailBrush")
            }
        };
        Grid.SetColumn(pointerBox, 1);

        grid.Children.Add(valueBox);
        grid.Children.Add(pointerBox);

        return new Border
        {
            Style = (Style)Application.Current.FindResource("NodeStyle"),
            Child = grid
        };
    }
}
