using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;


namespace BashSoft
{
    public static class Data
    {
        public static bool isDataInitialized = false;
        private static Dictionary<string, Dictionary<string, List<int>>> studentsByCource;
        //Dictionary<course_name, Dictionary<user_name, scoresOnTask>>

        public static void InitializeData(string fileName)
        {
            if (!isDataInitialized)
            {
                OutputWriter.WriteMessageOnNewLine("Reading data...");
                studentsByCource = new Dictionary<string, Dictionary<string, List<int>>>();
                ReadData(fileName);
            }
            else
            {
                OutputWriter.WriteMessageOnNewLine(ExceptionMessages.DataAlreadyInitialisedException);
            }
        }

        private static void ReadData(string fileName)
        {
            string path = SessionData.currentPath + "\\" + fileName;

            if (File.Exists(path)) //Directory.Exists??
            {
                string pattern = @"([A-Z][a-zA-z#+]*_[A-Z][a-z]{2}_\d{4})\s+([A-Z][a-z]{0,3}\d{2}_\d{2,4})\s+(\d+)";
                Regex rgx = new Regex(pattern);

                string[] allInputLines = File.ReadAllLines(path);

                for (int i = 0; i < allInputLines.Length; i++)
                {
                    if (!string.IsNullOrEmpty(allInputLines[i]) && rgx.IsMatch(allInputLines[i]))
                    {
                        Match currentMatch = rgx.Match(allInputLines[i]);

                        string courseName = currentMatch.Groups[1].Value;
                        string userName = currentMatch.Groups[2].Value;
                        int score;
                        bool hasParsedScore = int.TryParse(currentMatch.Groups[3].Value, out score);

                        if (hasParsedScore && score >= 0 && score <= 100)
                        {

                            if (!studentsByCource.ContainsKey(courseName))
                            {
                                studentsByCource.Add(courseName, new Dictionary<string, List<int>>());
                            }
                            if (!studentsByCource[courseName].ContainsKey(userName))
                            {
                                studentsByCource[courseName].Add(userName, new List<int>());
                            }

                            studentsByCource[courseName][userName].Add(score);
                        }
                    }
                }
            }
            
            isDataInitialized = true;
            OutputWriter.WriteMessageOnNewLine("Data read!");
        }

        private static bool IsQueryForCoursePossible(string courseName)
        {
            if (isDataInitialized)
            {
                if (studentsByCource.ContainsKey(courseName))
                {
                    return true;
                }
                else
                {
                    OutputWriter.DisplayException(ExceptionMessages.InexistingCourseInDataBase);
                }
            }
            else
            {
                OutputWriter.DisplayException(ExceptionMessages.DataNotInitializedExceptionMessage);
            }
           return false;
        }

        private static bool IsQueryForStudentPossible(string courseName, string studentUserName)
        {
            //if (isDataInitialized)
           // {
                if (IsQueryForCoursePossible(courseName) && studentsByCource[courseName].ContainsKey(studentUserName))
                {
                    return true;
                }
                else
                {
                    OutputWriter.DisplayException(ExceptionMessages.InexistingStudentInDataBase);
                }
           // }
           // else
           //     OutputWriter.DisplayException(ExceptionMessages.DataNotInitializedExceptionMessage);
            return false;
        }

        public static void GetStudentScoresFromCourse(string courseName, string userName)
        {
            if (IsQueryForStudentPossible(courseName, userName))
            {
                OutputWriter.PrintStudent(new KeyValuePair<string, List<int>>(userName, studentsByCource[courseName][userName]));
            }
        }

        public static void GetAllStudentsFromCourse(string courseName)
        {
            if (IsQueryForCoursePossible(courseName))
            {
                OutputWriter.WriteMessageOnNewLine($"{courseName}:");
                foreach (var studentMarksEntry in studentsByCource[courseName])
                {
                    OutputWriter.PrintStudent(studentMarksEntry);
                }
            }
        }
     
    }
}
