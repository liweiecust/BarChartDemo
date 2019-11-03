using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp4
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            int gridwidth = 5;
            Point dot = new Point();
            dot.X = 20;
            dot.Y = 25;

            Point pointA = new Point(50, 60);
            Point pointB = new Point(120, 90);

            Point pointStart = new Point(pointA.X + gridwidth / 2, pointA.Y + 5);
            Point pointEnd = new Point(pointB.X + gridwidth / 2, pointB.Y + 5);

            ArcSegment arcSegment = new ArcSegment(pointB, new Size(88, 120), 360, false, SweepDirection.Clockwise, true);

            PathSegmentCollection pathSegmentCollection = new PathSegmentCollection() { arcSegment };
            PathFigure pathFigure = new PathFigure();
            pathFigure.StartPoint = pointA;
            pathFigure.Segments = pathSegmentCollection;


            PathFigureCollection pathFigureCollection = new PathFigureCollection();
            pathFigureCollection.Add(pathFigure);
            PathGeometry pathGeometry = new PathGeometry();
            pathGeometry.Figures = pathFigureCollection;
            Path path = new Path();
            path.Data = pathGeometry;
            path.Stroke = Brushes.Red;
            path.StrokeThickness = 1;

            canvas.Children.Add(path);


            canvas.Background = Brushes.AliceBlue;
            canvas.UpdateLayout();
        }
    }
}
