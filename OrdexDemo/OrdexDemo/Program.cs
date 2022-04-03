using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdexDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Type your filename and press enter
                Console.Write("Enter File Name:");

                // Create a string variable and get user input from the keyboard and store it in the variable
                string FileName = Console.ReadLine();

                //Opens a file in read mode 
                int count = 0;

                System.IO.StreamReader file = new System.IO.StreamReader(FileName);
                FileInfo fi = new FileInfo(FileName);
                if (fi.Extension != ".txt")
                {
                    Console.WriteLine("File must be txt format.");
                    Console.ReadLine();
                }

                //Gets each line till end of file is reached  
                while ((FileName = file.ReadLine()) != null)
                {
                    //Splits each line into words 
                    String[] words = FileName.Trim().Split(' ', '\t');
                    //Counts each word 
                    words = words.Except(new List<string> { string.Empty }).ToArray();
                    count = count + words.Length;
                }

                Console.WriteLine("Number of words present in given file: " + count);
                file.Close();
                Console.ReadKey();
            }
            catch (Exception)
            {
                Console.WriteLine("File not found");
                Console.ReadLine();
            }
        }
    }
}
