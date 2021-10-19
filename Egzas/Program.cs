using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Egzas
{
    

    class SelectSample2
    {
        // Define some classes
        public class Student
        {
            public string First { get; set; }
            public string Last { get; set; }
            public int ID { get; set; }
            public List<int> Scores;
            public ContactInfo GetContactInfo(SelectSample2 app, int id)
            {
                ContactInfo cInfo =
                    (from ci in app.contactList
                     where ci.ID == id
                     select ci)
                    .FirstOrDefault();

                return cInfo;
            }

            public override string ToString()
            {
                return First + " " + Last + ":" + ID;
            }
        }

        public class ContactInfo
        {
            public int ID { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public override string ToString() { return Email + "," + Phone; }
        }

        public class ScoreInfo
        {
            public double Average { get; set; }
            public int ID { get; set; }
        }

        // The primary data source
        public List<Student> students = new List<Student>()
        {
             new Student {First="Svetlana", Last="Omelchenko", ID=111, Scores= new List<int>() {97, 92, 81, 60}},
             new Student {First="Claire", Last="O'Donnell", ID=112, Scores= new List<int>() {75, 84, 91, 39}},
             new Student {First="Sven", Last="Mortensen", ID=113, Scores= new List<int>() {88, 94, 65, 100}},
             new Student {First="sameName", Last="Garcia", ID=114, Scores= new List<int>() {97, 89, 85, 82}},
             new Student {First="sameName2", Last="Garcia", ID=114, Scores= new List<int>() {97, 89, 85, 82}},
             new Student {First="sameName2", Last="Garcia", ID=114, Scores= new List<int>() {97, 89, 85, 82}},
        };

        public List<Student> students2 = new List<Student>()
        {
             new Student {First="Tom", Last="Omelchenko", ID=111, Scores= new List<int>() {97, 92, 81, 60}},
             new Student {First="tommy", Last="O'Donnell", ID=112, Scores= new List<int>() {75, 84, 91, 39}},
             new Student {First="tommo", Last="Mortensen", ID=113, Scores= new List<int>() {88, 94, 65, 91}},
             new Student {First="tammo", Last="Garcia", ID=114, Scores= new List<int>() {97, 89, 85, 82}},
             new Student {First="sameName", Last="Garcia", ID=114, Scores= new List<int>() {97, 89, 85, 100}},
        };

        // Separate data source for contact info.
        public List<ContactInfo> contactList = new List<ContactInfo>()
        {
            new ContactInfo {ID=111, Email="SvetlanO@Contoso.com", Phone="206-555-0108"},
            new ContactInfo {ID=112, Email="ClaireO@Contoso.com", Phone="206-555-0298"},
            new ContactInfo {ID=113, Email="SvenMort@Contoso.com", Phone="206-555-1130"},
            new ContactInfo {ID=114, Email="CesarGar@Contoso.com", Phone="206-555-0521"}
        };
    }
    class Program
    {
        static void Main(string[] args)
        {
            ///SELECT NEW OBJECT
            SelectSample2 randomClass = new SelectSample2();
            var q0 = randomClass.students.Select((student, index) => 
                                                new { index, last = student.Last + " " + student.First });
            Console.WriteLine("Selected new object");
            foreach(var obj in q0)
            {
                Console.WriteLine("{0}: {1}",obj.index, obj.last);
            }

            Console.WriteLine(new string('~', 50));
            Console.WriteLine();

            string[] fruits = { "apple", "mango", "orange", "passionfruit", "grape" };
            // Determine whether any string in the array is longer than "banana".
            string longestName =
                fruits.Aggregate("banana", (longest, next) => 
                                next.Length > longest.Length ? next : longest,
                                // Return the final result as an upper case string.
                                fruit => fruit.ToUpper());
            Console.WriteLine(
                "The fruit with the longest name is {0}.",
                longestName);

            Console.WriteLine(new string('~', 50));
            Console.WriteLine();

            var q1 = randomClass.students.Any(student => student.ID > 111);
            var q2 = randomClass.students.All(student => student.ID > 111);
            Console.WriteLine("any id >111: ");
            Console.WriteLine(q1);
            Console.WriteLine("all id >111:");
            Console.WriteLine(q2);

            Console.WriteLine(new string('~', 50));
            Console.WriteLine();




            Console.WriteLine("Select many");
            Console.WriteLine();
            IEnumerable<string> q3 =
                new[] { randomClass.students.Select(s => s.First), randomClass.students2.Select(s => s.First)}
                .SelectMany(name => name);
            foreach (string name in q3)
            {
                Console.WriteLine(name);
            }

            Console.WriteLine();
            Console.WriteLine(new string('~', 50));
            Console.WriteLine("Concat");
            Console.WriteLine();
            IEnumerable<string> q4 = randomClass.students.Select(s => s.First).Concat(randomClass.students2.Select(s => s.First));
            foreach (string name in q4)
            {
                Console.WriteLine(name);
            }

            Console.WriteLine(new string('~', 50));
            Console.WriteLine();
            Console.WriteLine("Contains, concat");
            Console.WriteLine();

            var q5 = randomClass.students.Where(s => s.Scores.Contains(100)).Concat(randomClass.students2.Where(s => s.Scores.Contains(100)));
            foreach (var item in q5)
            {
                Console.WriteLine(item.First);
            }

            Console.WriteLine(new string('~', 50));
            Console.WriteLine();

            double[] numbers1 = { 2.0, 2.0, 2.1, 2.2, 2.3, 2.3, 2.4, 2.5 };
            double[] numbers2 = { 2.2 };

            Console.WriteLine("Except");
            Console.WriteLine();

            var q6 = numbers1.Except(numbers2);
            foreach(var item in q6)
            {
                Console.WriteLine(item);
            }


            Console.WriteLine(new string('~', 50));
            Console.WriteLine();
            Console.WriteLine("GroupBy, math.Floor");
            double[] numbers = { 2.0, 1.0, 2.1, 3.2, 2.3, 4.3, 4.4, 4.5 };

            // Group Pet.Age values by the Math.Floor of the age.
            // Then project an anonymous type from each group
            // that consists of the key, the count of the group's
            // elements, and the minimum and maximum age in the group.
            var query = numbers.GroupBy(
                number => Math.Floor(number),
                number => number,
                (baseAge, ages) => new
                {
                    Key = baseAge,
                    Count = ages.Count(),
                    Min = ages.Min(),
                    Max = ages.Max()
                });

            // Iterate over each anonymous type.
            foreach (var result in query)
            {
                Console.WriteLine("\nNumberGroup group: " + result.Key);
                Console.WriteLine("count of numbers in this group: " + result.Count);
                Console.WriteLine("Minimum: " + result.Min);
                Console.WriteLine("Maximum: " + result.Max);
            }


            Console.WriteLine(new string('~', 50));
            Console.WriteLine();
            Console.WriteLine("Intersect");

            double[] numbers21 = { 2.0, 2.0, 2.1, 2.2, 2.3, 2.3, 2.4, 2.5 };
            double[] numbers22 = { 2.2 };

            var q7 = numbers1.Intersect(numbers2);
            foreach (var item in q7)
            {
                Console.WriteLine(item);
            }









            Console.ReadKey();
        }
    }
}
