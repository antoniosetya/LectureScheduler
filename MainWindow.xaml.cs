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

namespace LectureScheduler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    // Edge of a graph
    public class Edge
    {
        // Nodes that are connected by this edge
        public string N1;
        public string N2;
        // ctor
        public Edge(string _n1 = "", string _n2 = "")
        {
            N1 = _n1;
            N2 = _n2;
        }
        // Formats output
        public string FormatOutput()
        {
            return ("<" + N1 + "," + N2 + ">");
        }
    }

    public partial class MainWindow : Window
    {
        public List<Edge> Graph = new List<Edge>();
        public MainWindow()
        {
            InitializeComponent();
            // Just testing out Edge class and Graph list
            Edge temp = new Edge("C1","C2");
            Graph.Add(temp);
            temp = new Edge("C2", "C3");
            Graph.Add(temp);
            string str_out = "";
            foreach(Edge element in Graph)
            {
                str_out += element.FormatOutput();
            }
            FileNameLabel.Content = str_out;
        }

        // On "Load File" button click...
        private void LoadFileButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog FileDialog = new Microsoft.Win32.OpenFileDialog(); // File dialog
            FileDialog.DefaultExt = ".txt"; 
            Nullable<bool> result = FileDialog.ShowDialog(); // Show file dialog
            if (result == true) // If the file dialog retrieves a file
            {
                FileNameLabel.Content = FileDialog.FileName;
            }
        }
    }
}
