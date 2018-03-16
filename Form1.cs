using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LectureScheduler
{
    public partial class Form1 : Form
    {
        public List<Edge> EdgeList = new List<Edge>(); // Edge list
        public List<string> NodeList = new List<string>(); // Node list
        Microsoft.Msagl.GraphViewerGdi.GViewer viewer = new Microsoft.Msagl.GraphViewerGdi.GViewer(); // Graph viewer engine
        Microsoft.Msagl.Drawing.Graph graph; // The graph that MSAGL accepts
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.DefaultExt = ".txt";
            DialogResult result = openFileDialog1.ShowDialog(); // Show file dialog
            if (result == DialogResult.OK) // If the file dialog retrieves a file
            {
                graph = new Microsoft.Msagl.Drawing.Graph("graph"); // Initialize new MSAGL graph
                FileContent.Text = ""; // Clear FileContent textbox
                using (StreamReader sr = new StreamReader(openFileDialog1.OpenFile()))
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
                        // Add node to MSAGL graph (first line) and to the node list (second line)
                        graph.AddNode(target_node);
                        NodeList.Add(string.Copy(target_node));
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
                            // Add this edge to MSAGL graph (first line) and to the edge list (remaining)
                            graph.AddEdge(preq_node, target_node);
                            temp = new Edge(preq_node, target_node);
                            EdgeList.Add(temp);
                        }
                    }
                }
                foreach (Edge element in EdgeList)
                {
                    FileContent.Text += element.FormatOutput();
                    FileContent.Text += "\n";
                }
                // Bind graph to viewer engine
                viewer.Graph = graph;
                // Bind viewer engine to second window
                GraphWindow.SuspendLayout();
                viewer.Dock = System.Windows.Forms.DockStyle.Fill;
                GraphWindow.Controls.Add(viewer);
                GraphWindow.ResumeLayout();
                // Shows second window
                GraphWindow.ShowDialog();
            }
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }
    }

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
}
