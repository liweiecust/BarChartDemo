using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp4.ViewModel;

namespace WpfApp4
{
    /// <summary>
    /// Interaction logic for BarChartControl.xaml
    /// </summary>
    public partial class BarChartControl : UserControl, INotifyPropertyChanged
    {
        BackgroundWorker backWorker = new BackgroundWorker();
        bool flag = true;
        public BarChartControl()
        {
            InitializeComponent();


            Data = new List<DataPoint>
                {
                     new DataPoint("Jason",15), new DataPoint("Ashk", 8),new DataPoint("Ele", 7),new DataPoint("Tigger", 19),
                     new DataPoint("Arison", 10),new DataPoint("A",11),new DataPoint("Odongo",15),new DataPoint("Owle",6),
                     new DataPoint("Tlero", 13),new DataPoint("Terick", 16),new DataPoint("Siri", 9),new DataPoint("L", 11)

                };
            
            backWorker.DoWork += Changecolor;
            //backWorker.ProgressChanged += BackWorker_ProgressChanged;
            //backWorker.WorkerReportsProgress = true;
            //Thread thread = new Thread(new ThreadStart(Changecolor));
            //thread.Start();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public List<DataPoint> Data          //CLR wrapper, expose the instance property so that it can be used as a path in binding
        {
            get { return (List<DataPoint>)GetValue(CollectionProperty); }

            set
            {
                SetValue(CollectionProperty, value);
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Data"));
                }

            }

        }

        public static readonly DependencyProperty CollectionProperty = DependencyProperty.Register("Data", typeof(List<DataPoint>), typeof(MainWindow), new PropertyMetadata());

        private void BarChartControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            BottomGrid.Height = 25;
            LeftGrid.Width = 25;
            SetXDatasContent();
            DrawAxis();
        }

        private void SetXDatasContent()
        {
            MainGridFrom0To1.Children.Clear();
            BottomGrid.Children.Clear();
            LeftGrid.Children.Clear();
            if (Data.Count > 0)
            {
                int count = Data.Count;
                for (int i = 0; i < count + 1; i++)
                {
                    BottomGrid.ColumnDefinitions.Add(new ColumnDefinition());
                    MainGridFrom0To1.ColumnDefinitions.Add(new ColumnDefinition());
                }
                int index = 0;
                foreach (var data in Data)
                {
                    //底部
                    var textblock = new TextBlock();
                    textblock.Text = data.Name;
                    //Forground
                    textblock.VerticalAlignment = VerticalAlignment.Top;
                    textblock.TextAlignment = TextAlignment.Center;
                    textblock.HorizontalAlignment = HorizontalAlignment.Right;

                    double textBlockWidth = 25;
                    textblock.Width = 30;
                    textblock.Margin = new Thickness(0, 5, -textBlockWidth / 2, 0);

                    Grid.SetColumn(textblock, index);
                    BottomGrid.Children.Add(textblock);


                    //主内容
                    var stackPanel = new StackPanel();
                    stackPanel.Orientation = Orientation.Vertical;

                    var tbl = new TextBlock();
                    tbl.Height = 15;
                    tbl.Margin = new Thickness(0, 0, 0, 5);
                    tbl.Text = data.Value.ToString();
                    textblock.Background = Brushes.Transparent;
                    tbl.HorizontalAlignment = HorizontalAlignment.Center;
                    stackPanel.Children.Add(tbl);

                    var rectangle = new Rectangle();
                    rectangle.Width = 25;
                    double maxValue = Data.Max(i => i.Value);
                    rectangle.Height = (data.Value / maxValue) * (MainGridFrom0To1.ActualHeight - tbl.Height-20-25); //

                    rectangle.Fill = Brushes.Blue;
                    rectangle.HorizontalAlignment = HorizontalAlignment.Center;

                    stackPanel.Children.Add(rectangle);
                    stackPanel.Margin = new Thickness(0, 0, -textBlockWidth / 2, 0);
                    stackPanel.VerticalAlignment = VerticalAlignment.Bottom;
                    stackPanel.HorizontalAlignment = HorizontalAlignment.Right;
                    Grid.SetColumn(stackPanel, index);
                    MainGridFrom0To1.Children.Add(stackPanel);
                    index++;
                }
            }

        }
        public void QuickSort(DataPoint[] array, int left, int right)
        {
            int i, j, Key;

            Key = array[left].Value;
            i = left;                              //3, 52, 5, 2, 1, 6, 3, 7, 3, 78, 32, 12, 54, 25, 11, 23
            j = right;                             //1, 52, 5, 2, 3, 6, 3, 7, 3, 78, 32, 12, 54, 25, 11, 23
            while (i != j)                           //1, 3, 5, 2, 52, 6, 3, 7, 3, 78, 32, 12, 54, 25, 11, 23
            {                                      //1, 2, 5, 3, 52, 6, 3, 7, 3, 78, 32, 12, 54, 25, 11, 23
                while (array[j].Value >= Key && i < j)      //1, 2, 3, 5, 52, 6, 3, 7, 3, 78, 32, 12, 54, 25, 11, 23
                {
                    j--;

                }
                if (i < j)
                {
                    Swap(ref array[i], ref array[j]);
                    //Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action<DataPoint[]>(UpdatePlot), array); 不行，不更新


                    Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Background, new Action<DataPoint[],int,int>(UpdatePlot), array,i,j);
                    Thread.Sleep(1500);
                }
                else
                    break;
                while (array[i].Value < Key && i < j)
                {
                    i++;

                }
                if (i < j)
                {
                    Swap(ref array[i], ref array[j]);

                    Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Background, new Action<DataPoint[], int, int>(UpdatePlot), array,i,j);
                    Thread.Sleep(1500);
                }
                else
                    break;
            }
            if (j >= 1 && j <= right - 2)
            {
                QuickSort(array, left, i - 1);
                QuickSort(array, i + 1, right);
            }
            else if (j == 0)
            {
                QuickSort(array, i + 1, right);
            }
            else if (j == right - 1)
            {
                QuickSort(array, left, i - 1);
            }
        }

        public void Swap(ref DataPoint A, ref DataPoint B)
        {
            var temp = A;
            A = B;
            B = temp;
            Thread.Sleep(300);
           
        }
        private void Changecolor(object send, DoWorkEventArgs e)
        {
            Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.SystemIdle, new Action(Change));

        }
        public void DrawLine(Rectangle rec,Point i,Point j)
        {
          
            double gridwidth = MainGridFrom0To1.Width / MainGridFrom0To1.ColumnDefinitions.Count;
            Ellipse ellipse = new Ellipse();
            GeneralTransform generaltrans = rec.TransformToVisual(null);// 获得rectangle 相对于屏幕的visual
            Point point = generaltrans.Transform(new Point(0, 0));
            LineGeometry linegeometry = new LineGeometry();
            linegeometry.StartPoint = new Point(point.X + gridwidth / 2, point.Y + 5);
           
        }
        public void UpdatePlot(DataPoint[] array,int a,int b)
        {
            MainGridFrom0To1.Children.Clear();
            BottomGrid.Children.Clear();
            LeftGrid.Children.Clear();
            //canvas.Children.Clear();
            if (array.Length > 0)
            {
                int count = array.Length;
                //for (int i = 0; i < count + 1; i++)           //Clear 无效，网格越来越多
                //{
                //    BottomGrid.ColumnDefinitions.Add(new ColumnDefinition());
                //    MainGridFrom0To1.ColumnDefinitions.Add(new ColumnDefinition());
                //}
                int index = 0;
                Point pointA = new Point();
                Point pointB = new Point();
                foreach (var data in array)
                {
                    //底部
                    var textblock = new TextBlock();
                    textblock.Text = data.Name;
                    //Forground
                    textblock.VerticalAlignment = VerticalAlignment.Top;
                    textblock.TextAlignment = TextAlignment.Center;
                    textblock.HorizontalAlignment = HorizontalAlignment.Right;
                    textblock.Background = Brushes.Transparent;

                    double textBlockWidth = 25;
                    textblock.Width = 30;
                    textblock.Margin = new Thickness(0, 5, -textBlockWidth / 2, 0);

                    Grid.SetColumn(textblock, index);
                    BottomGrid.Children.Add(textblock);


                    //主内容
                    var stackPanel = new StackPanel();
                    stackPanel.Orientation = Orientation.Vertical;

                    var tbl = new TextBlock();
                    tbl.Height = 15;
                    tbl.Margin = new Thickness(0, 0, 0, 5);
                    tbl.Text = data.Value.ToString();

                    tbl.HorizontalAlignment = HorizontalAlignment.Center;
                    stackPanel.Children.Add(tbl);

                    var rectangle = new Rectangle();
                    rectangle.Width = 25;
                    double maxValue = Data.Max(i => i.Value);
                  
                    rectangle.Height = (data.Value / maxValue) * (MainGridFrom0To1.ActualHeight-tbl.Height-20); //
                    rectangle.HorizontalAlignment = HorizontalAlignment.Center;

                    stackPanel.Children.Add(rectangle);
                  
                    stackPanel.Margin = new Thickness(0, 0, -textBlockWidth / 2, 0);
                    stackPanel.VerticalAlignment = VerticalAlignment.Bottom;
                    stackPanel.HorizontalAlignment = HorizontalAlignment.Right;
                    Grid.SetColumn(stackPanel, index);

                    MainGridFrom0To1.Children.Add(stackPanel);
                    MainGridFrom0To1.UpdateLayout();
                    if (index == a || index == b)
                    {
                        rectangle.Fill = Brushes.Red;

                        GeneralTransform generalTransform = rectangle.TransformToAncestor(MainGridFrom0To1);

                        if (index == a)
                        {
                            pointA = generalTransform.Transform(new Point(0, 0));
                        }
                        else
                        {
                            pointB = generalTransform.Transform(new Point(0, 0));
                        }
                    }
                    else
                    {
                        rectangle.Fill = Brushes.Blue;
                    }
                    index++;
                }
                Thread.Sleep(1000);
                Drawline(pointA, pointB);

               

            }
        }
        public double distance(Point point1,Point point2)
        {
            return Math.Sqrt((point1.X - point2.X) * (point1.X - point2.X) + (point1.Y - point2.Y) * (point1.Y - point2.Y));
        }
        public void Drawline(Point pointA,Point pointB)
        {
            // LineGeometry linegeometry = new LineGeometry();
            double gridwidth = MainGridFrom0To1.ActualWidth / MainGridFrom0To1.ColumnDefinitions.Count;
            //linegeometry.StartPoint = new Point(pointA.X + gridwidth / 2, pointA.Y + 5);
            //linegeometry.EndPoint = new Point(pointB.X + gridwidth / 2, pointB.Y + 5);

            Point dot = new Point();
            dot.X = LeftGrid.ActualWidth + MainGridFrom0To1.ActualWidth / 2;
            dot.Y = HeaderGrid.ActualHeight + MainGridFrom0To1.ActualHeight;

            //Line line = new Line();
            //line.X1 = pointA.X + gridwidth / 2;
            //line.Y1 = pointA.Y + 5;
            //line.X2 = pointB.X + gridwidth / 2;
            //line.Y2 = pointB.Y + 5;
            //line.Stroke = Brushes.Red;
            //canvas.Children.Add(line);
            Point pointStart = new Point(pointA.X + gridwidth / 2, pointA.Y -15);
            Point pointEnd = new Point(pointB.X /4, pointB.Y - 15);
            //ArcSegment arcSegment = new ArcSegment(pointA,new Size();
            //EllipseGeometry ellipseGeometry = new EllipseGeometry(dot, distance(dot, pointStart), distance(dot, pointEnd));


            //GeometryGroup geometryGroup = new GeometryGroup();
            //geometryGroup.Children.Add(ellipseGeometry);
            //GeometryDrawing geometryDrawing = new GeometryDrawing();
            //geometryDrawing.Geometry = geometryGroup;


            //DrawingImage drawingImage = new DrawingImage(geometryDrawing);
            //Image image = new Image();
            //image.Source = drawingImage;
            //canvas.Children.Add(image);
            ArcSegment arcSegment = new ArcSegment(pointEnd, new Size(distance(dot, pointStart), distance(dot, pointEnd)), 150, false, SweepDirection.Clockwise, true);

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
            canvas.Children.Add(path);
            canvas.UpdateLayout();
        }
        public void Change()
        {
         
           
            QuickSort(Data.ToArray(), 0, Data.Count - 1);

        }
        private  void Button_Click(object sender, RoutedEventArgs e)
        {
            backWorker.RunWorkerAsync();
        }
        public void DrawAxis()
        {
            //Window window = Window.GetWindow(BottomGrid);
            //GeneralTransform generalTransform = BottomGrid.TransformToAncestor(window);

            //Point upperleftpoint = generalTransform.Transform(new Point(0, 0));
            //Point upperrightpoint = new Point(upperleftpoint.X + BottomGrid.ActualWidth, upperleftpoint.Y);


            LineGeometry lineGeometry = new LineGeometry();
            //lineGeometry.StartPoint = upperleftpoint;
            //lineGeometry.EndPoint = upperrightpoint;

            Path path = new Path();
            path.Stroke = Brushes.Red;
            path.StrokeThickness = 4;

            //path.Data = new PathGeometry()
            //{
            //    Figures=new PathFigureCollection()
            //    {
            //        new PathFigure()
            //        {
            //            Segments=new PathSegmentCollection()
            //            {
            //                new LineSegment() {Point=upperrightpoint}

            //            }
            //        }
            //    }

            //};

            Vector vector = VisualTreeHelper.GetOffset(BottomGrid);
            //使用Vector VisualTreeHelper.GetOffset(Visual visual)方法，其会返回visual在其父控件中的偏移量，然后你再将返回值的Vector对象转换成

            //Point对象就可以了；
          
            lineGeometry.StartPoint = new Point(vector.X-LeftGrid.ActualWidth,vector.Y- HeaderGrid.ActualHeight-path.StrokeThickness);
            lineGeometry.EndPoint = new Point(vector.X - LeftGrid.ActualWidth + BottomGrid.ActualWidth+20, vector.Y - HeaderGrid.ActualHeight - path.StrokeThickness);

            path.Data = lineGeometry;
            canvas.Children.Add(path);
            //Canvas.SetZIndex(path, 0);
            //Grid.SetZIndex(path, 0);

            //Yaxis
          
            LineGeometry lineGeometryY = new LineGeometry();
          

            Path pathY = new Path();
            pathY.Stroke = Brushes.Red;
            pathY.StrokeThickness = 4;

            Vector vectorY = VisualTreeHelper.GetOffset(MainGridFrom0To1);


            lineGeometryY.EndPoint = new Point(vectorY.X - LeftGrid.ActualWidth, vectorY.Y - HeaderGrid.ActualHeight-20);

            lineGeometryY.StartPoint = lineGeometry.StartPoint;
            //lineGeometryY.StartPoint = new Point(vectorY.X - LeftGrid.ActualWidth , vectorY.Y +MainGridFrom0To1.ActualHeight- HeaderGrid.ActualHeight);

            pathY.Data = lineGeometryY;
            canvas.Children.Add(pathY);

        }

      
        //---------------------------------------------------------------------------
        //public Brush BorderBrush
        //{
        //    get { return (Brush)GetValue(BorderBrushProperty); }
        //    set { SetValue(BorderBrushProperty, value); }
        //}

        //public static readonly DependencyProperty BorderBrushProperty = DependencyProperty.Register("BorderBrush",
        //typeof(Brush), typeof(BarChartControl),
        //new PropertyMetadata(Brushes.Black));

        //public Thickness BorderThickness
        //{
        //    get { return (Thickness)GetValue(BorderThicknessProperty); }
        //    set { SetValue(BorderThicknessProperty, value); }
        //}

        //public static readonly DependencyProperty BorderThicknessProperty = DependencyProperty.Register("BorderThickness",
        //typeof(Thickness), typeof(BarChartControl),
        //new PropertyMetadata(new Thickness(1.0, 0.0, 1.0, 1.0)));

        //public AxisYModel AxisY
        //{
        //    get { return (AxisYModel)GetValue(AxisYProperty); }
        //    set { SetValue(AxisYProperty, value); }
        //}

        //public static readonly DependencyProperty AxisYProperty = DependencyProperty.Register("AxisY",
        //typeof(AxisYModel), typeof(BarChartControl),
        //new PropertyMetadata(new AxisYModel()));

        //public AxisXModel AxisX
        //{
        //    get { return (AxisXModel)GetValue(AxisXProperty); }
        //    set { SetValue(AxisXProperty, value); }
        //}

        //public static readonly DependencyProperty AxisXProperty = DependencyProperty.Register("AxisX",
        //typeof(AxisXModel), typeof(BarChartControl),
        //new PropertyMetadata(new AxisXModel()));

        //public double HeaderHeight
        //{
        //    get { return (double)GetValue(HeaderHeightProperty); }
        //    set { SetValue(HeaderHeightProperty, value); }
        //}
        //public static readonly DependencyProperty HeaderHeightProperty = DependencyProperty.Register("HeaderHeight",
        //typeof(double), typeof(BarChartControl), new PropertyMetadata(10.0));
        //public string Header
        //{
        //    get { return (string)GetValue(HeaderProperty); }
        //    set { SetValue(HeaderProperty, value); }
        //}
        //public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header",
        //typeof(string), typeof(BarChartControl), new PropertyMetadata());


        //private void BarChartControl_OnLoaded(object sender, RoutedEventArgs e)
        //{
        //    MainBorder.BorderBrush = BorderBrush;
        //    MainBorder.BorderThickness = BorderThickness;

        //    BottomGrid.Height = AxisX.Height;
        //    LeftGrid.Width = AxisY.Width;

        //    SetYTitlesContent();

        //    SetXDatasContent();
        //}

     


        //private void SetYTitlesContent()
        //{
        //    var axisYModel = AxisY;
        //    if (axisYModel.Titles.Count > 0)
        //    {
        //        int gridRows = axisYModel.Titles.Count - 1;
        //        for (int i = 0; i < gridRows; i++)
        //        {
        //            LeftGrid.RowDefinitions.Add(new RowDefinition());
        //            MainGridForRow1.RowDefinitions.Add(new RowDefinition());
        //        }
        //        int index = 0;
        //        foreach (var title in axisYModel.Titles)
        //        {
        //            var textblock = new TextBlock();
        //            textblock.Text = title.Name;
        //            textblock.Foreground = axisYModel.ForeGround;
        //            textblock.HorizontalAlignment = HorizontalAlignment.Right;
        //            textblock.Height = title.LabelHeight;
        //            if (index < gridRows)
        //            {
        //                textblock.VerticalAlignment = VerticalAlignment.Bottom;
        //                textblock.Margin = new Thickness(0, 0, 5, -title.LabelHeight / 2);//因为设置在行底部还不够,必须往下移
        //                Grid.SetRow(textblock, gridRows - index - 1);
        //            }
        //            else
        //            {
        //                textblock.VerticalAlignment = VerticalAlignment.Top;
        //                textblock.Margin = new Thickness(0, -title.LabelHeight / 2, 5, 0);//最后一个,设置在顶部
        //                Grid.SetRow(textblock, 0);
        //            }
        //            LeftGrid.Children.Add(textblock);

        //            var border = new Border();
        //            border.Height = title.LineHeight;
        //            border.BorderBrush = title.LineBrush;
        //            double thickness = Convert.ToDouble(title.LineHeight) / 2;
        //            border.BorderThickness = new Thickness(0, thickness, 0, thickness);
        //            if (index < gridRows)
        //            {
        //                border.VerticalAlignment = VerticalAlignment.Bottom;
        //                border.Margin = new Thickness(0, 0, 0, -thickness);//因为设置在行底部还不够,必须往下移
        //                Grid.SetRow(border, gridRows - index - 1);
        //            }
        //            else
        //            {
        //                border.VerticalAlignment = VerticalAlignment.Top;
        //                border.Margin = new Thickness(0, -thickness, 0, 0);//最后一个,设置在顶部
        //                Grid.SetRow(border, 0);
        //            }
        //            Grid.SetColumn(border, 0);
        //            Grid.SetColumnSpan(border, AxisX.Datas.Count + 1);
        //            MainGridForRow1.Children.Add(border);
        //            index++;
        //        }
        //    }
        //}

        /// <summary>
        /// 设置分行
        /// </summary>
        /// <param name="leftGrid"></param>
        /// <param name="count"></param>
        private void SetGridRowDefinitions(Grid leftGrid, int count)
        {

            for (int i = 0; i < count; i++)
            {
                leftGrid.RowDefinitions.Add(new RowDefinition());
            }
        }


    }


    public class AxisXModel
    {
        private double _height = 20;
        /// <summary>
        /// 高度
        /// </summary>
        public double Height
        {
            get
            {
                return _height;
            }
            set { _height = value; }
        }

        private Brush _foreGround = Brushes.Black;
        /// <summary>
        /// 字体颜色
        /// </summary>
        public Brush ForeGround
        {
            get { return _foreGround; }
            set { _foreGround = value; }
        }

        List<AxisXDataModel> _datas = new List<AxisXDataModel>();
        /// <summary>
        /// 数据
        /// </summary>
        public List<AxisXDataModel> Datas
        {
            get { return _datas; }
            set { _datas = value; }
        }
    }
    public class AxisYModel
    {
        private double _width = 20;
        /// <summary>
        /// 宽度
        /// </summary>
        public double Width { get { return _width; } set { _width = value; } }

        private Brush _foreGround = Brushes.Black;
        /// <summary>
        /// 字体颜色
        /// </summary>
        public Brush ForeGround { get { return _foreGround; } set { _foreGround = value; } }

        List<AxisYDataModel> _titles = new List<AxisYDataModel>();
        /// <summary>
        /// 左侧标题列表
        /// </summary>
        public List<AxisYDataModel> Titles
        {
            get { return _titles; }
            set { _titles = value; }
        }
    }

    public class AxisXDataModel : DataModel
    {
        private double _labelWidth = 20;
        /// <summary>
        /// 底部标签-单个宽度
        /// </summary>
        public double LabelWidth
        {
            get { return _labelWidth; }
            set { _labelWidth = value; }
        }
        private double _barWidth = 20;
        /// <summary>
        /// Bar宽度
        /// </summary>
        public double BarWidth
        {
            get { return _barWidth; }
            set { _barWidth = value; }
        }

        private Color _fillBrush = Colors.Blue;
        /// <summary>
        /// Bar填充颜色
        /// </summary>
        public Color FillBrush
        {
            get
            {
                return _fillBrush;
            }
            set { _fillBrush = value; }
        }

        private Color _fillEndBrush = Colors.Blue;

        public Color FillEndBrush
        {
            get
            {
                return _fillEndBrush;
            }
            set { _fillEndBrush = value; }
        }
    }
    public class AxisYDataModel : DataModel
    {
        private double _labelHeight = 15;
        /// <summary>
        /// 左侧标题栏-单个标题高度
        /// </summary>
        public double LabelHeight
        {
            get { return _labelHeight; }
            set { _labelHeight = value; }
        }
        private double _lineHeight = 0.2;
        /// <summary>
        /// GridLine高度
        /// </summary>
        public double LineHeight
        {
            get { return _lineHeight; }
            set { _lineHeight = value; }
        }

        private Brush _lineBrush = Brushes.Blue;
        /// <summary>
        /// Bar填充颜色
        /// </summary>
        public Brush LineBrush
        {
            get { return _lineBrush; }
            set { _lineBrush = value; }
        }
    }
    public class DataModel
    {
        /// <summary>
        /// 显示名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public double Value { get; set; }
    }


}


