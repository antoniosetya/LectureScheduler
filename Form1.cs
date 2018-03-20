using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LectureScheduler
{
    public partial class Form1 : Form
    {
        List<Courses> Course; // Stores the list of courses
        Microsoft.Msagl.GraphViewerGdi.GViewer viewer = new Microsoft.Msagl.GraphViewerGdi.GViewer(); // Graph viewer engine
        Microsoft.Msagl.Drawing.Graph graph; // The graph that MSAGL accepts
        List<List<string>> GraphAnim; // "Animation" data, it stores which node(s) it should animate on each step
        int CurAnimGraph; // "Animation" data pointer
        string output_file = "output.html";

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Setting up the file dialog
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.InitialDirectory = Directory.GetCurrentDirectory();
            openFileDialog1.Title = "Please select an input file...";
            // Show file dialog
            DialogResult result = openFileDialog1.ShowDialog();

            if (result == DialogResult.OK) // If the file dialog retrieves a file
            {
                graph = new Microsoft.Msagl.Drawing.Graph("graph"); // Initialize new MSAGL graph
                FileContent.Text = ""; // Clear FileContent textbox
                Course = new List<Courses>(); // Clear leftover Course content
                // Read input file
                using (StreamReader sr = new StreamReader(openFileDialog1.OpenFile()))
                {
                    string line;
                    Courses temp;
                    while (sr.Peek() >= 0)
                    {
                        line = sr.ReadLine(); // Read file line by line
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
                // Re-initialize graph viewer and animation
                DrawGraphFirstTime();
                GraphAnim = new List<List<string>>();
                CurAnimGraph = 0;
                // Run topological sort based on user's choice
                if (radioButton1.Checked)
                {
                    BFS();
                }
                else if (radioButton2.Checked)
                {
                    DFS();
                }
                // Setting animation controls
                AnimGraphPrev.Enabled = false;
                AnimGraphNext.Enabled = true;
                AutoAnimGraph.Enabled = true;
                // Hiding labels
                label4.Visible = false;
                label3.Visible = false;
                // Showing output to browser
                ResultBrowser.Url = new System.Uri(Directory.GetCurrentDirectory() + "\\" + output_file);
            }
        }

        private void DrawGraphFirstTime()
        {
            // Bind graph to viewer engine
            viewer.Graph = graph;
            // Bind viewer engine to the panel
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
            // De-bind graph to viewer engine
            viewer.Graph = null;
        }

        // BFS algorithm
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
                        // Processing this node if there's no prerequisite
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
            // Format output to output.html
            using (StreamWriter output = new StreamWriter(Directory.GetCurrentDirectory() + "\\" + output_file))
            {
                SetupOutput(output);
                output.WriteLine("<table border='1' style='width : 100%'>");
                output.WriteLine("<tr>");
                int semester = 1;
                output.WriteLine("<td>Semester " + semester + "</td>\n<td>");
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
                        output.WriteLine(semesterCourse[i].Remove(0, semester.ToString().Length) + "<br>");
                    }
                    else
                    {
                        semester++;
                        output.WriteLine("</td>\n</tr>\n<tr>");
                        output.WriteLine("<td>Semester " + semester + "</td>");
                        output.WriteLine("<td>" + semesterCourse[i].Remove(0, semester.ToString().Length) + "<br>");
                        // Initialize the next element of list of graph animation
                        GraphAnim.Add(new List<string>());
                    }
                    // Adding this node to graph animation
                    GraphAnim[semester - 1].Add(semesterCourse[i].Remove(0, semester.ToString().Length));
                }
                output.WriteLine("</table>");
                SetupEndOutput(output);
            }
        }

        // Algorithms that will help the DFS algorithm
        // Search an element of list of Courses by it's course name
        public static int FindIndex(string name, List<Courses> list)
        {
            int idx = -1;
            int i = 0;
            while ((idx == -1) && (i < list.Count))
            {
                if (name == list[i].getCourses())
                {
                    idx = i;
                }
                i++;
            }

            return idx;
        }

        // Search an element of list of Courses by it's end time
        public static int FindIndex2(int value, List<Courses> list)
        {
            int idx = -1;
            int i = 0;
            while ((idx == -1) && (i < list.Count))
            {
                if (value == list[i].getEndTime())
                {
                    idx = i;
                }
                i++;
            }

            return idx;
        }

        // Membership testing in a list of Courses
        public static bool isMember(Courses course, List<Courses> list)
        {
            return (FindIndex(course.getCourses(), list) != -1);
        }

        // DFS algorithm
        private void DFS()
        {
            // Basic Course : list that holds all courses that has no prerequisite
            List<Courses> basicCourse = new List<Courses>();
            // Populate all courses that has no prerequisite
            foreach (Courses elm in Course)
            {
                if (elm.isNoPreRequisite())
                {
                    basicCourse.Add(elm);
                }
            }
            //Make list of nodes that can be access from one node
            for (int i = 0; i < Course.Count; i++)
            {
                for (int j = 0; j < Course[i].getNumberOfPreRequisite(); j++)
                {
                    for (int k = 0; k < Course.Count; k++)
                    {
                        if ((i != k) && (Course[i].getPreReqCourses(j) == Course[k].getCourses()))
                        {
                            Course[k].addNextCourses(Course[i].getCourses());
                        }
                    }
                }
            }

            //Make stack of temporary node/node that is going to be checked
            List<string> CourseTemp = new List<string>();
            for (int i = 0; i < basicCourse.Count; i++)
            {
                CourseTemp.Add(basicCourse[i].getCourses());
            }

            //Giving time stamp
            int time = 1;
            int idxT;
            string name;
            while (CourseTemp.Count > 0)
            {
                name = CourseTemp[CourseTemp.Count - 1];
                idxT = FindIndex(name, Course);
                if (Course[idxT].getStartTime() == 0)
                {
                    Course[idxT].setStartTime(time);
                    time++;
                }

                if (Course[idxT].getNumberOfNextCourses() != 0)
                {
                    string next = Course[idxT].getNextCourses();
                    int idxN = FindIndex(next, Course);
                    if (Course[idxN].getEndTime() == 0)
                    {
                        CourseTemp.Add(next);
                    }
                }
                else
                {
                    Course[idxT].setEndTime(time);
                    // Set graph animation
                    GraphAnim.Insert(0,new List<string>());
                    GraphAnim[0].Add(Course[idxT].getCourses());
                    CourseTemp.RemoveAt(CourseTemp.Count - 1);
                    time++;
                }
            }

            //Make new list with sorted courses
            List<Courses> semesterCourse = new List<Courses>();
            semesterCourse.AddRange(basicCourse);
            List<int> listEndTime = new List<int>();
            foreach (var crs in Course)
            {
                listEndTime.Add(crs.getEndTime());
            }
            for (int i = 0; i < Course.Count; i++)
            {
                int max = listEndTime.Max();
                listEndTime.Remove(max);
                int indeks = FindIndex2(max, Course);
                if (!(isMember(Course[indeks], semesterCourse)))
                {
                    semesterCourse.Add(Course[indeks]);
                }
            }

            // Formatting output
            int semester = 1;
            using (StreamWriter file_out = new StreamWriter(Directory.GetCurrentDirectory() + "\\" + output_file))
            {
                SetupOutput(file_out);
                file_out.WriteLine("<table border='1' style='width : 100%'>");
                foreach (var sem in semesterCourse)
                {
                    file_out.WriteLine("<tr><td>Semester " + semester + "</td>");
                    file_out.WriteLine("<td>" + sem.getCourses() + "</td></tr>");
                    semester++;
                }
                file_out.WriteLine("</table>");
                SetupEndOutput(file_out);
            }
            
        }

        // Setting up the first lines of the output file
        private void SetupOutput(StreamWriter outfile)
        {
            outfile.WriteLine("<!DOCTYPE html>");
            outfile.WriteLine("<head>");
            outfile.WriteLine("<meta name='viewport' content='width = device - width, initial - scale = 1.0'>");
            outfile.WriteLine("</head>");
            outfile.WriteLine("<body>");
            outfile.WriteLine("<h3 align='center'>Result</h3>");
        }

        // Puts the "closing lines" of the output file
        private void SetupEndOutput(StreamWriter outfile)
        {
            outfile.WriteLine("</body>");
            outfile.WriteLine("</html>");
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e) { }

        // "Next" button clicked
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
                AutoAnimGraph.Enabled = false;
            }
        }
        
        // "Previous" button clicked
        private void AnimGraphPrev_Click(object sender, EventArgs e)
        {
            CurAnimGraph--;
            if (!AnimGraphNext.Enabled) AnimGraphNext.Enabled = true;
            if (!AutoAnimGraph.Enabled) AutoAnimGraph.Enabled = true;
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

        private void radioButton1_CheckedChanged(object sender, EventArgs e) { }

        private void radioButton2_CheckedChanged(object sender, EventArgs e) { }

        // "Auto Animation" button click
        private bool stopAnim = false;
        private System.Threading.Tasks.TaskCompletionSource<bool> AnimThreadStopped;
        private async void AutoAnimGraph_Click(object sender, EventArgs e)
        {
            if (AutoAnimGraph.Text == "Stop auto animation")
            {
                stopAnim = true;
                AutoAnimGraph.Text = "Stopping...";
                AutoAnimGraph.Enabled = false;
                AnimThreadStopped = new System.Threading.Tasks.TaskCompletionSource<bool>();
                await AnimThreadStopped.Task;
                AutoAnimGraph.Text = "Auto Animation";
                AutoAnimGraph.Enabled = true;
            }
            else
            {
                var tm = new System.Threading.Thread(AutoAnimGraphThread);
                tm.IsBackground = true;
                tm.Start();
                AutoAnimGraph.Text = "Stop auto animation";
            }
            
        }

        // This is the part where the auto-animation actually do something
        private void AutoAnimGraphThread()
        {
            do
            {
                System.Threading.Thread.Sleep(1000);
                this.Invoke(new Action(() => AnimGraphNext.PerformClick()));
            }
            while (AnimGraphNext.Enabled && !stopAnim);
            if (stopAnim)
            {
                stopAnim = false;
                AnimThreadStopped.SetResult(true);
            }
            this.Invoke(new Action(() => AutoAnimGraph.Text = "Auto Animation"));
        }
    }

    // Class Courses
    public class Courses
    {
        private string course; // Stores the name of this course
        private List<string> preReqCourses = new List<string>(); // Stores names of this course's prerequisites
        private int numberOfPreRequisite; // The number of course prerequisite
        private Queue<string> nextCourses = new Queue<string>();
        private int start_time;
        private int end_time;

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
        public void addNextCourses(string courses)
        {
            nextCourses.Enqueue(courses);
        }
        public void setNumberOfPreRequisite(int n)
        {
            this.numberOfPreRequisite = n;
        }
        public void setStartTime(int st)
        {
            this.start_time = st;
        }
        public void setEndTime(int et)
        {
            this.end_time = et;
        }
        //Getter
        public string getCourses()
        {
            return course;
        }
        public int getStartTime()
        {
            return start_time;
        }
        public int getEndTime()
        {
            return end_time;
        }
        public string getPreReqCourses(int idx)
        {
            return preReqCourses[idx];
        }
        public string getNextCourses()
        {
            return (string)nextCourses.Dequeue();
        }
        public int getNumberOfPreRequisite()
        {
            return numberOfPreRequisite;
        }
        public int getNumberOfNextCourses()
        {
            return nextCourses.Count;
        }
        // Member function
        // Check if this course has no prerequisite
        public bool isNoPreRequisite()
        {
            return numberOfPreRequisite == 0;
        }
        // Check if a course is a prerequisite for this course
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
