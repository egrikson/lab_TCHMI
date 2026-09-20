using System.Windows.Controls;
using System.Windows.Media;

namespace DequeVisualizationWpf.Drawnings;

/// <summary>Анимация появления узла: кратковременная подсветка блока.</summary>
public class AnimationEngine
{
    public async Task AnimateAppearance(Border node)
    {
        node.Background = Brushes.LightGreen;
        await Task.Delay(50);
        node.Background = Brushes.LightYellow;
    }
}
