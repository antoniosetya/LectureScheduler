using System;
using System.IO;
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
        public List<Edge> EdgeList = new List<Edge>();
        public List<string> NodeList = new List<string>();
        Microsoft.Msagl.GraphViewerGdi.GViewer viewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();
        Microsoft.Msagl.Drawing.Graph graph = new Microsoft.Msagl.Drawing.Graph("graph");
        public MainWindow()
        {
            InitializeComponent();

        }



        // On "Load File" button click...
        private void LoadFileButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog FileDialog = new Microsoft.Win32.OpenFileDialog(); // File dialog
            FileDialog.DefaultExt = ".txt"; 
            Nullable<bool> result = FileDialog.ShowDialog(); // Show file dialog
            if (result == true) // If the file dialog retrieves a file
            {
                using (StreamReader sr = new StreamReader(FileDialog.FileName))
                {
                    string line, target_node, preq_node;
                    int i;
                    Edge temp;
                    while (sr.Peek() >= 0)
                    {
                        line = sr.ReadLine();
                        target_node = "";
                        i = 0;
                        // Get current lecture
                        while ((i < line.Length) && (line[i] != ',') && (line[i] != '.'))
                        {
                            target_node += line[i];
                            i++;
                        }
                        if (line[i] != '.') i++;
                        
                        // Get prerequisite lecture
                        while ((i < line.Length) && (line[i] != '.'))
                        {
                            preq_node = "";
                            while ((i < line.Length) && (line[i] != ',') && (line[i] != '.'))
                            {
                                preq_node += line[i];
                                i++;
                            }
                            if (line[i] != '.') i++;
                            temp = new Edge(preq_node, target_node);
                            EdgeList.Add(temp);
                        }
                    }
                }
                FileContent.Text = "";
                foreach(Edge element in EdgeList)
                {
                    FileContent.Text += element.FormatOutput();
                    FileContent.Text += "\n";
                }
                FileNameLabel.Content = FileDialog.FileName;
                
            }
        }
    }
}
