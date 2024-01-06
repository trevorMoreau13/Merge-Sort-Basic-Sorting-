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
        const int INSERTION_SORT_THRESHOLD = 15;
        public static Stopwatch stopwatch = new Stopwatch();
        static void Main(string[] args)
        {
            List<Person> unsortedPeople = new List<Person>();
            string inputFilePath = "C:\\Users\\Bloodred13\\Documents\\Purdue\\IT481 Software Managment\\M4\\Data_To_Sort\\data_1000.txt"; // Update with the actual file path
            string outputFilePath = "C:\\Users\\Bloodred13\\Documents\\Purdue\\IT481 Software Managment\\M4\\Data_To_Sort\\data_1000_Sorted.txt"; // File path for the sorted data

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
                if (right - left <= INSERTION_SORT_THRESHOLD) // Using insertion sort for small sub-arrays
                {
                    InsertionSort(data, left, right);
                }
                else
                {
                    int middle = (left + right) / 2;
                    DoMerge(data, left, middle);
                    DoMerge(data, middle + 1, right);
                    Merge(data, left, middle, right);
                }
            }
            return data;
        }
        static void InsertionSort(int[] data, int left, int right)
        {
            for (int i = left + 1; i <= right; i++)
            {
                int key = data[i];
                int j = i - 1;

                while (j >= left && data[j] > key)
                {
                    data[j + 1] = data[j];
                    j = j - 1;
                }
                data[j + 1] = key;
            }
        }
        static void Merge(int[] data, int left, int middle, int right)
        {
            if (data[middle] <= data[middle + 1])
            {
                // The two sub-arrays are already in order, no need to merge
                return;
            }

            int[] temp = new int[right - left + 1];
            int i = left, j = middle + 1, k = 0;

            // Merge the two sub-arrays into temp[]
            while (i <= middle && j <= right)
            {
                if (data[i] <= data[j])
                {
                    temp[k++] = data[i++];
                }
                else
                {
                    temp[k++] = data[j++];
                }
            }

            // Copy the remaining elements of the left sub-array (if any)
            while (i <= middle)
            {
                temp[k++] = data[i++];
            }

            // Copy the remaining elements of the right sub-array (if any)
            while (j <= right)
            {
                temp[k++] = data[j++];
            }

            // Copy the merged sub-array back into the original array
            for (i = left, k = 0; i <= right; i++, k++)
            {
                data[i] = temp[k];
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