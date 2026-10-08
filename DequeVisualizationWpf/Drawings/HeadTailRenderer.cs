using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DequeVisualizationWpf.Drawnings;

public class HeadTailRenderer
{
    public void DrawHead(Canvas canvas, Rect node)
    {
        DrawLabel(canvas, "head", (Brush)Application.Current.FindResource("HeadBrush"), node.X + node.Width / 2 - 14, node.Y - 20);
    }

    public void DrawTail(Canvas canvas, Rect node)
    {
        DrawLabel(canvas, "tail", (Brush)Application.Current.FindResource("TailBrush"), node.X + node.Width / 2 - 12, node.Bottom + 3);
    }

    private static void DrawLabel(Canvas canvas, string text, Brush brush, double left, double top)
    {
        var label = new TextBlock
        {
            Text = text,
            Foreground = brush,
            FontWeight = FontWeights.Bold,
            FontSize = 12
        };

        Canvas.SetLeft(label, left);
        Canvas.SetTop(label, top);

        canvas.Children.Add(label);
    }
}
