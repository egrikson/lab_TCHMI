using System.Windows;

namespace DequeVisualizationWpf.Drawnings;

/// <summary>Расчёт позиций узлов с переносом на новую строку.</summary>
public class LayoutEngine
{
    public const double RowHeight = 100;

    public Point GetNextPosition(double x, double y, double width, double panelWidth)
    {
        if (x + width > panelWidth - 50)
        {
            x = 20;
            y += RowHeight;
        }

        return new Point(x, y);
    }
}
