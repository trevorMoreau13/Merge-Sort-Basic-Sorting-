using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;


namespace Merge_Sort
{
    class Program
    {
        //Original code from: YusufsKaygusuz @ https://github.com/YusufsKaygusuz/Merge-Sort

        public static Stopwatch stopwatch = new Stopwatch();
        static void Main(string[] args)
        {
            List<Person> unsortedPeople = new List<Person>();
            string inputFilePath = "C:\\Users\\Bloodred13\\Documents\\Purdue\\IT481 Software Managment\\M4\\Data_To_Sort\\data_10.txt"; // Update with the actual file path
            string outputFilePath = "C:\\Users\\Bloodred13\\Documents\\Purdue\\IT481 Software Managment\\M4\\Data_To_Sort\\data_10_Sorted.txt"; // File path for the sorted data

            try
            {
                List<int> peopleID = new List<int>();
                string[] lines = File.ReadAllLines(inputFilePath);
                foreach (var line in lines)
                {
                    var parts = line.Split(',');
                    var person = new Person(parts[0], parts[1], parts[2], DateTime.Parse(parts[3]));

                    try
                    {
                        peopleID.Add(Convert.ToInt32(person.ID));


                    }
                    catch
                    {
                        Console.WriteLine("Error: " + person.ID + " is not a valid ID number.");
                    }
                    unsortedPeople.Add(person);


                }




                peopleID = MergeSort(peopleID.ToArray()).ToList();


                List<Person> sortedPeople = new List<Person>();
                foreach (int personID in peopleID)
                {
                    sortedPeople.Add(unsortedPeople.Find(x => Convert.ToInt32(x.ID) == personID));
                    unsortedPeople.Remove(unsortedPeople.Find(x => x.ID == personID.ToString()));
                }

                // Writing sorted data to a new file
                using (StreamWriter file = new StreamWriter(outputFilePath))
                {
                    foreach (var person in sortedPeople)
                    {
                        file.WriteLine($"{person.ID},{person.FirstName},{person.LastName},{person.DateOfBirth.ToShortDateString()}");
                    }
                }

                Console.WriteLine($"Data sorted in {stopwatch.Elapsed} ");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            Console.ReadLine();
        }

        public static int[] MergeSort(int[] data)
        {
            stopwatch.Start();
            data = DoMerge(data, 0, data.Length - 1);
            stopwatch.Stop();
            return data;
        }

        static int[] DoMerge(int[] data, int left, int right)
        {
            if (left < right)
            {
                int middle = (left + right) / 2;
                DoMerge(data, left, middle);
                DoMerge(data, middle + 1, right);
                Merge(data, left, middle, right);
                return data;
            }
            else
            {
                return data;
            }
        }

        static void Merge(int[] data, int left, int middle, int right)
        {
            int middle1 = middle + 1;
            int oldPosition = left;
            int size = right - left + 1;
            int[] temp = new int[size];
            int i = 0;

            while (left <= middle && middle1 <= right)
            {
                if (data[left] <= data[middle1])
                    temp[i++] = data[left++];
                else
                    temp[i++] = data[middle1++];
            }

            if (left > middle)
                for (int j = middle1; j <= right; j++)
                {
                    temp[i++] = data[middle1++];
                }
            else
                for (int j = left; j <= middle; j++)
                {
                    temp[i++] = data[left++];
                }

            for (int k = 0; k < size; k++)
            {
                data[oldPosition++] = temp[k];
            }

        }


    }


    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ID { get; set; }
        public DateTime DateOfBirth { get; set; }

        public Person(string id, string firstName, string lastName, DateTime dateOfBirth)
        {
            FirstName = firstName;
            LastName = lastName;
            ID = id;
            DateOfBirth = dateOfBirth;
        }
    }
}