using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DequeVisualizationWpf.Drawnings;

/// <summary>Отрисовка меток HEAD и TAIL рядом с крайними узлами дека.</summary>
public class HeadTailRenderer
{
    public void DrawHead(Canvas canvas, Rect node)
    {
        // метка head — над узлом
        DrawLabel(canvas, "head", Brushes.Red, node.X + node.Width / 2 - 14, node.Y - 20);
    }

    public void DrawTail(Canvas canvas, Rect node)
    {
        // метка tail — под узлом, чтобы не перекрывать head, когда в деке один элемент
        DrawLabel(canvas, "tail", Brushes.Gray, node.X + node.Width / 2 - 12, node.Bottom + 3);
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
