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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace animation1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private bool rendering = false;

        private void StopRendering()
        {
            CompositionTarget.Rendering -= RenderFrame;
            rendering = false;
        }


        private List<EllipseInfo> ellipses = new List<EllipseInfo>();

        private double accelerationY = 0.1;
        private int minStartingSpeed = 1;
        private int maxStartingSpeed = 250;
        private double speedRatio = 0.1;
        private int minEllipses = 200;
        private int maxEllipses = 200;
        private int ellipseRadius = 10;

        private void RenderFrame(object sender, EventArgs e)
        {
            if (ellipses.Count == 0)
            {
                int halfCanvasWidth = (int)canvas.ActualWidth / 2;

                Random rand = new Random();
                int ellipseCount = rand.Next(minEllipses, maxEllipses + 1);
                for (int i = 0; i < ellipseCount; i++)
                {
                    Ellipse ellipse = new Ellipse();
                    ellipse.Fill = Brushes.White;
                    ellipse.Width = ellipseRadius;
                    ellipse.Height = ellipseRadius;
                    Canvas.SetLeft(ellipse, halfCanvasWidth + rand.Next(-halfCanvasWidth, halfCanvasWidth));
                    Canvas.SetTop(ellipse, 0);
                    canvas.Children.Add(ellipse);

                    EllipseInfo info = new EllipseInfo(ellipse, speedRatio * rand.Next(minStartingSpeed, maxStartingSpeed));
                    ellipses.Add(info);
                }
            }
            else
            {
                for (int i = ellipses.Count - 1; i >= 0; i--)
                {
                    EllipseInfo info = ellipses[i];
                    double top = Canvas.GetTop(info.Ellipse);
                    Canvas.SetTop(info.Ellipse, top + 1 * info.VelocityY);

                    if (top >= (canvas.ActualHeight - ellipseRadius * 2 - 10))
                    {
                        ellipses.Remove(info);
                    }
                    else
                    {
                        info.VelocityY += accelerationY;
                    }
                }
            }
        }

        private void cmdStart_Clicked(object sender, MouseButtonEventArgs e)
        {
            text.Foreground = Brushes.DarkBlue;
            text1.Foreground = Brushes.White;
            if (!rendering)
            {
                ellipses.Clear();
                canvas.Children.Clear();

                CompositionTarget.Rendering += RenderFrame;
                rendering = true;

            }
        }

        private void cmdStop_Clicked(object sender, MouseButtonEventArgs e)
        {
            StopRendering();
        }
    }

    public class EllipseInfo
    {
        public Ellipse Ellipse
        {
            get; set;
        }

        public double VelocityY
        {
            get; set;
        }

        public EllipseInfo(Ellipse ellipse, double velocityY)
        {
            VelocityY = velocityY;
            Ellipse = ellipse;
        }
    }
}
