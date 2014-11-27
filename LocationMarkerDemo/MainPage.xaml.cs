using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using XAMLGameFunctions;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace LocationMarkerDemo
{
    /// <summary>
    /// XAMLGameFunctions.LocationMarker Demo.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        List<PointInt> coords = new List<PointInt>();
        Path lastPath = null;
        public MainPage()
        {
            this.InitializeComponent();


            for (int y = 0; y < 600; y += 50)
            {
                for (int x = 0; x < 800; x += 50)
                {
                    Path p = new Path();
                    RectangleGeometry rg = new RectangleGeometry();
                    rg.Rect = new Rect(x, y, 50, 50);
                    p.Data = rg;
                    if ((x + y) % 100 == 0)
                        p.Fill = new SolidColorBrush(Colors.White);
                    else
                        p.Fill = new SolidColorBrush(Colors.Black);

                    mainPanel.Children.Add(p);
                }
            }
        }

        private void mainPanel_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            PointerPoint click = e.GetCurrentPoint(mainPanel);
            int x = (int)(click.Position.X / 50);
            int y = (int)(click.Position.Y / 50);
            int index = (16 * y) + x;
            if (((SolidColorBrush)((Path)mainPanel.Children[index]).Fill).Color == Colors.Green)
            {
                coords.Remove(new PointInt(x, y));
                ((Path)mainPanel.Children[index]).Fill = new SolidColorBrush((x + y) % 2 == 0 ? Colors.White : Colors.Black);
            }
            else
            {
                coords.Add(new PointInt(x, y));
                ((Path)mainPanel.Children[index]).Fill = new SolidColorBrush(Colors.Green);
            }
        }

        private void generateButton_Click(object sender, RoutedEventArgs e)
        {
            if (lastPath != null)
                mainPanel.Children.Remove(lastPath);


            Path path;

            if ((bool)overflowEdgesCheckBox.IsChecked)
                path = LocationMarker.GenerateCoverage(coords.ToArray(), 50, double.Parse(overflowBox.Text), double.Parse(wobbleBox.Text));
            else
                path = LocationMarker.GenerateCoverage(coords.ToArray(), 50, double.Parse(overflowBox.Text), 16, 12, double.Parse(wobbleBox.Text));

            path.Stroke = new SolidColorBrush(Colors.Indigo);
            path.StrokeThickness = 2;
            path.Fill = new SolidColorBrush(Colors.Gold);
            path.Opacity = 0.8;
            lastPath = path;
            mainPanel.Children.Add(path);
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            lastPath = null;

            for (int y = 0; y < 600; y += 50)
            {
                for (int x = 0; x < 800; x += 50)
                {
                    Path p = new Path();
                    RectangleGeometry rg = new RectangleGeometry();
                    rg.Rect = new Rect(x, y, 50, 50);
                    p.Data = rg;
                    if ((x + y) % 100 == 0)
                        p.Fill = new SolidColorBrush(Colors.White);
                    else
                        p.Fill = new SolidColorBrush(Colors.Black);

                    mainPanel.Children.Add(p);
                }
            }
            coords = new List<PointInt>();
        }
    }
}
