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
        List<List<string>> GraphAnim; // "Animation" data, it stores which node(s) it should animate on each step
        int CurAnimGraph;

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
                label3.Visible = false;
                // Re-initialize graph viewer and animation
                DrawGraphFirstTime();
                GraphAnim = new List<List<string>>();
                CurAnimGraph = 0;
                // Run topological sort based on user's choice
                if (radioButton1.Checked)
                {
                    BFS();
                }
                /* else //radioButton2.Checked
                {
                    // DFS();
                } */
                AnimGraphPrev.Enabled = false;
                AnimGraphNext.Enabled = true;
            }
        }

        private void DrawGraphFirstTime()
        {
            // Bind graph to viewer engine
            viewer.Graph = graph;
            // Bind viewer engine to second window
            GraphPanel.SuspendLayout();
            viewer.Dock = System.Windows.Forms.DockStyle.Fill;
            GraphPanel.Controls.Add(viewer);
            GraphPanel.ResumeLayout();
        }

        private void RefreshGraph()
        {
            // Bind graph to viewer engine
            viewer.Graph = graph;
        }

        private void DetachGraph()
        {
            // De-Bind graph to viewer engine
            viewer.Graph = null;
        }

        private void BFS()
        {
            List<string> semesterCourse = new List<string>();

            int sems = 0;
            List<string> linkCourse = new List<string>();
            while (Course.Count != 0)
            {
                sems++;
                linkCourse.Clear();
                int nCourse = Course.Count;
                for (int i = 0; i < nCourse; i++)
                {
                    if (Course[i - linkCourse.Count].isNoPreRequisite())
                    {
                        // Processing this node
                        semesterCourse.Add(sems + Course[i - linkCourse.Count].getCourses());
                        linkCourse.Add(Course[i - linkCourse.Count].getCourses());
                        Course.RemoveAt(i - linkCourse.Count + 1);
                    }
                }
                for (int i = 0; i < Course.Count; i++)
                {
                    for (int j = 0; j < linkCourse.Count; j++)
                    {
                        if (Course[i].isPreRequisite(linkCourse[j]))
                        {
                            Course[i].setNumberOfPreRequisite(Course[i].getNumberOfPreRequisite() - 1);
                        }
                    }
                }
            }
            // Format output
            int semester = 1;
            FileContent.Text += "Semester " + semester + ":\n";
            GraphAnim.Add(new List<string>()); // Initialize the first element of graph animation
            for (int i = 0; i < semesterCourse.Count; i++)
            {
                bool semesterN = true;
                string nString = semester.ToString();
                for (int j = 0; j < nString.Length; j++)
                {
                    if (nString[j] != semesterCourse[i][j])
                    {
                        semesterN = false;
                    }
                }
                if (semesterN)
                {
                    FileContent.Text += semesterCourse[i].Remove(0, semester.ToString().Length) + "\n";
                }
                else
                {
                    semester++;
                    FileContent.Text += "Semester " + semester + ":\n";
                    FileContent.Text += semesterCourse[i].Remove(0, semester.ToString().Length) + "\n";
                    // Initialize the next element of list of graph animation
                    GraphAnim.Add(new List<string>());  
                }
                // Adding this node to graph animation
                GraphAnim[semester-1].Add(semesterCourse[i].Remove(0, semester.ToString().Length));
            }
        }



        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void AnimGraphNext_Click(object sender, EventArgs e)
        {
            CurAnimGraph++;
            if (!AnimGraphPrev.Enabled) AnimGraphPrev.Enabled = true;
            DetachGraph();
            foreach(string affected_node in GraphAnim[CurAnimGraph-1])
            {
                graph.FindNode(affected_node).Attr.FillColor = Microsoft.Msagl.Drawing.Color.Green;
            }
            RefreshGraph();
            if (CurAnimGraph == GraphAnim.Count)
            {
                AnimGraphNext.Enabled = false;
            }
        }

        private void AnimGraphPrev_Click(object sender, EventArgs e)
        {
            CurAnimGraph--;
            if (!AnimGraphNext.Enabled) AnimGraphNext.Enabled = true;
            DetachGraph();
            foreach (string affected_node in GraphAnim[CurAnimGraph])
            {
                graph.FindNode(affected_node).Attr.FillColor = Microsoft.Msagl.Drawing.Color.White;
            }
            RefreshGraph();
            if (CurAnimGraph == 0)
            {
                AnimGraphPrev.Enabled = false;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
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
