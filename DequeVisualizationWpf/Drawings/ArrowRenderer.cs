using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DequeVisualizationWpf.Drawnings;

public class ArrowRenderer
{
    private const double Offset = 5;
    private const double HeadSize = 7;
    private const double Detour = 15;

    public void DrawLink(Canvas canvas, Rect from, Rect to, bool wrapped)
    {
        if (wrapped)
            DrawWrappedLink(canvas, from, to);
        else
            DrawStraightLink(canvas, from, to);
    }

    private static void DrawStraightLink(Canvas canvas, Rect from, Rect to)
    {
        double y = from.Y + from.Height / 2;

        AddLine(canvas, new Point(from.Right, y - Offset), new Point(to.Left, y - Offset));
        AddHead(canvas, new Point(to.Left, y - Offset), new Vector(1, 0));

        AddLine(canvas, new Point(from.Right, y + Offset), new Point(to.Left, y + Offset));
        AddHead(canvas, new Point(from.Right, y + Offset), new Vector(-1, 0));
    }

    private static void DrawWrappedLink(Canvas canvas, Rect from, Rect to)
    {
        double y1 = from.Y + from.Height / 2;
        double y2 = to.Y + to.Height / 2;
        double midY = (from.Bottom + to.Y) / 2;

        var polyline = new Polyline
        {
            Stroke = (Brush)Application.Current.FindResource("LinkBrush"),
            StrokeThickness = 2,
            Points = new PointCollection
            {
                new Point(from.Right, y1),
                new Point(from.Right + Detour, y1),
                new Point(from.Right + Detour, midY),
                new Point(Math.Max(to.Left - Detour, 2), midY),
                new Point(Math.Max(to.Left - Detour, 2), y2),
                new Point(to.Left, y2)
            }
        };

        canvas.Children.Add(polyline);

        AddHead(canvas, new Point(to.Left, y2), new Vector(1, 0));
        AddHead(canvas, new Point(from.Right, y1), new Vector(-1, 0));
    }

    private static void AddLine(Canvas canvas, Point from, Point to)
    {
        canvas.Children.Add(new Line
        {
            X1 = from.X,
            Y1 = from.Y,
            X2 = to.X,
            Y2 = to.Y,
            Stroke = (Brush)Application.Current.FindResource("LinkBrush"),
            StrokeThickness = 2
        });
    }

    private static void AddHead(Canvas canvas, Point tip, Vector direction)
    {
        direction.Normalize();

        var normal = new Vector(-direction.Y, direction.X);
        var basePoint = tip - direction * HeadSize;

        canvas.Children.Add(new Polygon
        {
            Fill = (Brush)Application.Current.FindResource("LinkBrush"),
            Points = new PointCollection
            {
                tip,
                basePoint + normal * (HeadSize / 2),
                basePoint - normal * (HeadSize / 2)
            }
        });
    }
}
