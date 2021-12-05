using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Graph
{
    /// <summary>
    /// Interaction logic for Graf.xaml
    /// </summary>
    public partial class Graf : Window
    {
        private List<Node> lista = new();
        public Graf(List<Node> lista, ComboBox combobox)
        {
            InitializeComponent();
            this.lista = lista;
            foreach (var item in combobox.Items)
            {
                nodeCombobox.Items.Add(item);
                nodeCombobox1.Items.Add(item);
            }
            nodeCombobox.SelectedIndex = 0;
            nodeCombobox1.SelectedIndex = 0;
            init();
        }

        private void init()
        {
            List<int> volte = new();
            lista = lista.OrderByDescending(x => x.neigh.Count).ToList();
            lista[0].x = 300;
            lista[0].y = 200;
            int[] x = new int[] { -50, 50, 50, -50 };
            int[] y = new int[] { -50, -50, 50, 50 };
            int valaszto = 0;
            Random rnd = new();
            foreach (Node node in lista)
            {
                foreach (int neigh in node.neigh)
                {
                    if (!volte.Contains(neigh))
                    {
                        Node neighbour = lista.Where(x => x.ertek == neigh).First();
                        neighbour.x = node.x + x[valaszto];
                        neighbour.y = node.y + y[valaszto];
                        volte.Add(neigh);
                        if (valaszto > 2)
                            valaszto = 0;
                        else
                            valaszto++;
                        
                    }
                    volte.Add(node.ertek);
                }
            }
            osszekot(lista);
            foreach (Node node in lista)
            {
                //listbox.Items.Add($" Node: {node.ertek} , {node.x}, {node.y},count: {node.neigh.Count}");
                GrafRajzol.nodegen(this, "grafTabla", node.ertek.ToString(), 0 , node.x, node.y, true);
            }
        }

        private void osszekot(List<Node> lista)
        {
            foreach (Node node in lista)
            {
                foreach (int neigh in node.neigh)
                {
                    Line line = new();
                    line.StrokeThickness = 2;
                    line.Stroke = new SolidColorBrush(Colors.Black);
                    line.X1 = node.x + 25;
                    line.Y1 = node.y + 25;
                    line.X2 = lista.Where(x => x.ertek == neigh).First().x + 25;
                    line.Y2 = lista.Where(x => x.ertek == neigh).First().y + 25;
                    grafTabla.Children.Add(line);
                }
            }
        }

        private async void mehet(object sender, RoutedEventArgs e)
        {
            listbox.Items.Clear();
            foreach (Node node in lista)
            {
                GrafRajzol.nodegen(this, "grafTabla", node.ertek.ToString(), 0, node.x, node.y, true);
            }
            Node kezd = lista.Where(x => x.ertek == (int)nodeCombobox.SelectedValue).First();
            Node cel = lista.Where(x => x.ertek == (int)nodeCombobox1.SelectedValue).First();
            await Task.Run(()=>DFS(kezd,cel));
            listbox.Items.Add(cel.ertek);
            while (cel.ertek != kezd.ertek)
            {
                listbox.Items.Add(cel.parent);
                cel = lista.Where(x => x.ertek == cel.parent).First();
            }
            
        }

        private void DFS(Node root, Node cel)
        {
            
            Queue<Node> open = new();
            List<int> closed = new();
            
            open.Enqueue(root);
            while(open.Any())
            {
                Node vizsgal = open.Dequeue();
                vizsgal.status = 1;
                GrafRajzol.nodegen(this, "grafTabla", vizsgal.ertek.ToString(), vizsgal.status, vizsgal.x, vizsgal.y, true);
                foreach (int neigh in vizsgal.neigh)
                {
                    if (!closed.Contains(neigh) && !open.Any(x=>x.ertek == neigh))
                    {
                        Node openadd = lista.Where(x => x.ertek == neigh).First();
                        openadd.parent = vizsgal.ertek;
                        open.Enqueue(openadd);
                    }
                }
                foreach (Node szomszedok in open)
                {
                    szomszedok.status = 3;
                    GrafRajzol.nodegen(this, "grafTabla", szomszedok.ertek.ToString(), szomszedok.status, szomszedok.x, szomszedok.y, true);
                }

                if (closed.Any())
                {
                    foreach (int ertek in closed)
                    {
                        Node piros = lista.Where(x => x.ertek == ertek).First();
                        piros.status = 2;
                        GrafRajzol.nodegen(this, "grafTabla", piros.ertek.ToString(), piros.status, piros.x, piros.y, true);
                    }
                }
                closed.Add(vizsgal.ertek);

                if (vizsgal == cel)
                {
                    cel.status = 4;
                    GrafRajzol.nodegen(this, "grafTabla", cel.ertek.ToString(), cel.status, cel.x, cel.y, true);
                    break;
                }
                Thread.Sleep(500);
            }
        }
    }
}
