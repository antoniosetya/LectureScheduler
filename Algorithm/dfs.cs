using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class Courses
{
    private string course;
    private List<string> preReqCourses = new List<string>();
    private Queue<string> nextCourses = new Queue<string>();
    private int start_time;
    private int end_time;

    //Setter
    public void setCourse(string course)
    {
        this.course = course;
    }
    public void addPreReqCourses(string courses)
    {
        preReqCourses.Add(courses);
    }
    public void addNextCourses(string courses)
    {
        nextCourses.Enqueue(courses);
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
        return preReqCourses.Count;
    }
    public int getNumberOfNextCourses()
    {
        return nextCourses.Count;
    }
}

namespace Transcript2
{
    class Transcript2
    {
        public static int FindIndex(string name, List<Courses> list)
        {
            int idx = -1;
            int i = 0;
            while ((idx==-1) && (i<list.Count))
            {
                if (name == list[i].getCourses())
                {
                    idx = i;
                }
                i++;
            }

            return idx;
        }

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

        public static bool isMember(Courses course, List<Courses> list)
        {
            return (FindIndex(course.getCourses(), list) != -1);
        }

        public static void Main()
        {
            int idx = -1;
            string[] lines = System.IO.File.ReadAllLines("transcript.txt");
            List<Courses> Course = new List<Courses>();
            List<Courses> basicCourse = new List<Courses>();
            foreach (string line in lines)
            {
                idx += 1;
                var str = line.Replace(".", string.Empty);
                str = str.Replace(",", " ");
                string[] course = str.Split(' ');
                Courses C = new Courses();
                Course.Add(C);
                Course[idx].setCourse(course[0]);
                for (int i = 0; i < course.Length - 1; i++)
                {
                    Course[idx].addPreReqCourses(course[i + 1]);
                }
                if (Course[idx].getNumberOfPreRequisite() == 0)
                {
                    basicCourse.Add(Course[idx]);
                }
            }

            //Make list of nodes that can be access from one node
            for (int i = 0; i < Course.Count; i++)
            {
                for (int j = 0; j < Course[i].getNumberOfPreRequisite(); j++)
                {
                    for (int k = 0; k < Course.Count; k++)
                    {
                        if ((i!=k) && (Course[i].getPreReqCourses(j) == Course[k].getCourses()))
                        {
                            Course[k].addNextCourses(Course[i].getCourses());
                        }
                    }
                }
            }

            //Make stack of temporary node/node that is going to be checked
            List<string> CourseTemp = new List<string>();
            for (int i = 0; i<basicCourse.Count; i++)
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

            int semester = 1;
            foreach (var sem in semesterCourse)
            {
                Console.Write("Semester " + semester + " :");
                Console.WriteLine(sem.getCourses());
                semester++;
            }
        }
    }
}