using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace DequeVisualizationWpf.Drawnings;

public class AnimationEngine
{
    private static readonly Duration Time = new Duration(TimeSpan.FromSeconds(0.4));

    public void AnimateAppearance(Border node)
    {
        Color from = GetColor("NodeHighlightBrush");
        Color to = GetColor("NodeAddedBrush");

        node.Background = new SolidColorBrush(from);
        node.RenderTransformOrigin = new Point(0.5, 0.5);
        node.RenderTransform = new ScaleTransform(0.6, 0.6);
        node.Opacity = 0;

        var storyboard = new Storyboard();

        Add(storyboard, node, "Opacity", new DoubleAnimation(0, 1, Time));
        Add(storyboard, node, "RenderTransform.ScaleX", new DoubleAnimation(0.6, 1, Time));
        Add(storyboard, node, "RenderTransform.ScaleY", new DoubleAnimation(0.6, 1, Time));
        Add(storyboard, node, "Background.Color", new ColorAnimation(from, to, Time));

        storyboard.Begin();
    }

    private static void Add(Storyboard storyboard, DependencyObject target, string property, Timeline animation)
    {
        Storyboard.SetTarget(animation, target);
        Storyboard.SetTargetProperty(animation, new PropertyPath(property));
        storyboard.Children.Add(animation);
    }

    private static Color GetColor(string resourceKey)
    {
        return ((SolidColorBrush)Application.Current.FindResource(resourceKey)).Color;
    }
}
