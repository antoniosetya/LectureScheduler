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
        List<Courses> Course = new List<Courses>();
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
                    string line;
                    Courses temp;
                    //Edge temp;
                    while (sr.Peek() >= 0)
                    {
                        line = sr.ReadLine();
                        line = line.Replace(".",string.Empty);
                        line = line.Replace(","," ");
                        string [] cur_line = line.Split(' ');
                        temp = new Courses(cur_line[0]);
                        Course.Add(temp);
                        graph.AddNode(cur_line[0]);
                        for (int i = 1; i < cur_line.Length; i++)
                        {
                            Course[Course.Count-1].addPreReqCourses(cur_line[i]);
                            graph.AddEdge(cur_line[i],cur_line[0]);
                        }
                    }
                }
                System.Console.WriteLine(Course.Count);
                foreach (Courses element in Course)
                {
                    FileContent.Text = FileContent.Text + element.FormatOutput();
                    FileContent.Text = FileContent.Text + "\n";
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

    public class Courses
    {
        private string course;
        private List<string> preReqCourses = new List<string>();
        private int numberOfPreRequisite;

        // ctor
        public Courses(string _course = "")
        {
            course = _course;
            numberOfPreRequisite = 0;
        }
        //Setter
        public void setCourse(string course)
        {
            this.course = course;
        }
        public void addPreReqCourses(string courses)
        {
            preReqCourses.Add(courses);
            numberOfPreRequisite++;
        }
        public void setNumberOfPreRequisite(int n)
        {
            this.numberOfPreRequisite = n;
        }
        //Getter
        public string getCourses()
        {
            return course;
        }
        public int getNumberOfPreRequisite()
        {
            return numberOfPreRequisite;
        }
        public bool isNoPreRequisite()
        {
            return numberOfPreRequisite == 0;
        }
        public bool isPreRequisite(string s)
        {
            bool found = false;
            int i = 0;
            while ((!found) && (i < preReqCourses.Count))
            {
                if (s == preReqCourses[i])
                {
                    found = true;
                }
                else
                {
                    i++;
                }
            }
            return found;
        }
        // Member function
        // Format output
        public string FormatOutput()
        {
            string str_out = "";
            foreach(string preq in preReqCourses)
            {
                str_out += "<" + preq + "," + course + ">";
            }
            return str_out;
        }
    }
}
