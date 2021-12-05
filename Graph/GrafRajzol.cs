using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Graph
{
    class GrafRajzol
    {
        public static void nodegen(Window ablak, string objectNev, string szam, int szin, int bal, int fent, bool fix)
        {
            ablak.Dispatcher.Invoke(() =>
            {
                var canvas = (Canvas)ablak.FindName(objectNev);
                Viewbox vb = new();
                vb.Width = 50;
                vb.Height = 50;
                Grid grid = new();
                grid.Width = 20;
                grid.Height = 20;
                vb.Child = grid;
                Ellipse el = new();
                el.Stroke = new SolidColorBrush(Colors.Black);
                switch (szin)
                {
                    case 0:
                        el.Fill = new SolidColorBrush(Colors.White);
                        break;
                    case 1:
                        el.Fill = new SolidColorBrush(Colors.Blue);
                        break;
                    case 2:
                        el.Fill = new SolidColorBrush(Colors.Red);
                        break;
                    case 3:
                        el.Fill = new SolidColorBrush(Colors.Aqua);
                        break;
                    case 4:
                        el.Fill = new SolidColorBrush(Colors.Green);
                        break;
                }
                TextBlock tb = new();
                tb.TextAlignment = TextAlignment.Center;
                tb.VerticalAlignment = VerticalAlignment.Center;
                tb.Text = szam;
                grid.Children.Add(el);
                grid.Children.Add(tb);
                Canvas.SetLeft(vb, bal);
                Canvas.SetTop(vb, fent);
                canvas.Children.Add(vb);
            });
        }


    }
}
