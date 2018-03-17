using System;
using System.Collections;
using System.Collections.Generic;

public class Courses {
	private string course;
	private List<string> preReqCourses = new List<string> ();
	private int numberOfPreRequisite;
	
	//Setter
	public void setCourse(string course){
		this.course = course;
	}
	public void addPreReqCourses(string courses){
		preReqCourses.Add(courses);
	}
	public void setNumberOfPreRequisite(int n){
		this.numberOfPreRequisite = n;
	}
	//Getter
	public string getCourses(){
		return course;
	}
	public int getNumberOfPreRequisite(){
		return numberOfPreRequisite;
	}
	public bool isNoPreRequisite(){
		return numberOfPreRequisite == 0;
	}
	public bool isPreRequisite(string s){
		bool found = false;
		int i = 0;
		while ((!found) && (i < preReqCourses.Count)){
			if (s == preReqCourses[i]){
				found = true;
			}
			else{
				i++;
			}
		}
		return found;
	}
}

namespace Transcript {
	class Transcript {
		public static void Main(){
			int idx = -1;
			string[] lines = System.IO.File.ReadAllLines("transcript.txt");
			List<Courses> Course = new List<Courses>();
			foreach (string line in lines){
				idx += 1;
				var str = line.Replace(".", string.Empty);
				str = str.Replace(",", string.Empty);
				string[] course = str.Split(' ');
				Courses C = new Courses();
				Course.Add(C);
				Course[idx].setCourse(course[0]);
				Course[idx].setNumberOfPreRequisite(course.Length-1);
				for (int i = 0; i < course.Length-1; i++){
					Course[idx].addPreReqCourses(course[i+1]);
				}
			}
			
			List<string> semesterCourse = new List<string>();
			
			int sems = 0;
			List<string> linkCourse = new List<string>();
			while (Course.Count != 0){
				sems++;
				linkCourse.Clear();
				int nCourse = Course.Count;
				for (int i = 0; i < nCourse; i++){
					if (Course[i-linkCourse.Count].isNoPreRequisite()){
						semesterCourse.Add(sems+Course[i-linkCourse.Count].getCourses());
						linkCourse.Add(Course[i-linkCourse.Count].getCourses());
						Course.RemoveAt(i-linkCourse.Count+1);
					}
				}
				for (int i = 0; i < Course.Count; i++){
					for (int j = 0; j < linkCourse.Count; j++){
						if (Course[i].isPreRequisite(linkCourse[j])){
							Course[i].setNumberOfPreRequisite(Course[i].getNumberOfPreRequisite()-1);
						}
					}
				}
			}
			int semester = 1;
			Console.WriteLine("Semester " + semester + ":");
			for (int i = 0; i < semesterCourse.Count; i++){
				bool semesterN = true;
				string nString = semester.ToString();
				for (int j = 0; j < nString.Length; j++){
					if (nString[j] != semesterCourse[i][j]){
						semesterN = false;
					}
				}
				if (semesterN){
					Console.WriteLine(semesterCourse[i].Remove(0,semester.ToString().Length));
				}
				else{
					semester++;
					Console.WriteLine("Semester " + semester + ":");
					Console.WriteLine(semesterCourse[i].Remove(0,semester.ToString().Length));
				}
			}
		}
	}
}