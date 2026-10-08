using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DequeVisualizationWpf.Drawnings;

public class AnimationEngine
{
    public async Task AnimateAppearance(Border node)
    {
        node.Background = (Brush)Application.Current.FindResource("NodeHighlightBrush");
        await Task.Delay(50);
        node.Background = (Brush)Application.Current.FindResource("NodeAddedBrush");
    }
}
