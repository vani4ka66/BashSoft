using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BashSoft
{
   public static class OutputWriter
    {
        public static void WriteMessage(string message)
        {
            Console.Write(message);
        }
        public static void WriteMessageOnNewLine(string message)
        {
            Console.WriteLine(message);
        }
        public static void WriteEmptyLine()
        {
            Console.WriteLine();
        }
        public static void DisplayException(string message)
        {
            // we need to get the current foreground color(font color), save it, 
            //change the foreground color to red,
            //write the given message 
            //and finally change the foreground color to the one before. 
            ConsoleColor currentColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = currentColor;
        }
        public static void PrintStudent(KeyValuePair<string, List<int>> student)
        {
            OutputWriter.WriteMessageOnNewLine(string.Format($"{student.Key} - {string.Join(", ", student.Value)}"));
        }
    }
}
