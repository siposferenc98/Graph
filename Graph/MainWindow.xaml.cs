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
using Microsoft.Win32;
using System.IO;
using System.Text.RegularExpressions;

namespace Graph
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Node> graph;
        public MainWindow()
        {
            InitializeComponent();
        }


        private bool valid()
        {
            List<int> nodeok = new();
            foreach (Node node in graph)
            {
                if (!nodeok.Contains(node.ertek))
                {
                    nodeok.Add(node.ertek);
                }
                else
                    return false;
            }
            return true;
        }

        private void beolvas(object sender, RoutedEventArgs e)
        {
            graph = new();
            OpenFileDialog of = new();
            if (of.ShowDialog() == true)
            {
                StreamReader sr = new(of.FileName);
                while (!sr.EndOfStream)
                {
                    try
                    {
                        List<int> sor = sr.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse).ToList();
                        if (sor.Count > 0)
                        {
                            Node node = new Node();
                            node.ertek = sor[0];
                            for (int i = 1; i < sor.Count; i++)
                            {
                                node.neigh.Add(sor[i]);
                            }
                            graph.Add(node);
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Hibás fájl!");
                    }
                }
                sr.Close();

                if (valid())
                {
                    list.Items.Clear();
                    foreach (Node node in graph)
                    {
                        list.Items.Add(node.ertek);
                    }
                    list.SelectedIndex = 0;
                }
                else
                    MessageBox.Show("A gráfod tartalmaz duplikált elemet!");
            }
            else
                list.Items.Clear();
        }

        private void changed(object sender, SelectionChangedEventArgs e)
        {
            int balrol = 10;
            int fentrol = 20;
            int node = list.SelectedIndex;
            rajzol.Children.Clear();
            if (node != -1)
            {
                for (int i = 0; i < graph[node].neigh.Count; i++)
                {
                   GrafRajzol.nodegen(this, "rajzol", graph[node].neigh[i].ToString() , 0 , balrol, fentrol, false);
                    balrol += 70;
                }
            }

        }

        private void grafElohoz(object sender, RoutedEventArgs e)
        {
            if (graph != null)
            {
                Graf graf = new(graph, list);
                graf.Owner = this;
                graf.Show();

            }
            else
                MessageBox.Show("Nincs betöltve file/gráf!");

        }
    }
}
