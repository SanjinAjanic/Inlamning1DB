using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace Inlamning1DB
{
    class Program

    {

        static void Main(string[] args)

        {

            var filename = @"C:\Users\sanji\OneDrive\Skrivbord\personer.txt";
            //List<Person> Familj = new List<Person>();
            //using (StreamReader sa = new StreamReader(filename))

            //{
            //    List<string> splitArray = new List<string>();
            //    while (sa.ReadLine() != null)
            //    {

            //        splitArray.Add(sa.ReadLine().Split(',');

            //    }
            //    foreach (var item in splitArray)
            //    {
            //        Console.WriteLine(item);
            //    }
            //}
            // varför så komplicerad kod?
            List<Person> Familj = new List<Person>();
            if (File.Exists(filename))
            {
                var fileRows  =  File.ReadAllLines(filename);
                foreach (var item in fileRows)
                {
                    Console.WriteLine(item);
                    var split  =  item.Split(',');
                    var name = split[0];
                    var lastName = split[1];
                    Familj.Add(new Person(name.ToString(), lastName.ToString()));
                    //osv
                    // db.DelitePerson(new Person { FirstName = firstName, LastName = lastName });
                }

            }
            foreach (var item in Familj)
            {
                Console.WriteLine(item.FirstName);
            }
            // Samma sak som koden ovan
            // Ja blandade ihopp saker
            // Keep it simple.
            //okej tack :)
            // ska det funka nu?


            Databas db = new Databas();
            db.DelitePerson(new Person { FirstName = "Denana", LastName = "Ajanic" });
            db.CreatePerson(new Person { FirstName = "Denana", LastName = "Ajanic" });
            db.DelitePerson(new Person { FirstName = "Elvedin", LastName = "Ajanic" });
            db.CreatePerson(new Person { FirstName = "Elvedin", LastName = "Ajanic" });
            db.UpdatePerson(new Person { FirstName = "Sanjin", LastName = "Ajanic" });
            var dt = db.ReadPerson(new Person { FirstName = "Sanjin", LastName = "Ajanic" });
            var Id = db.ReadPersonId(new Person { FirstName = "Sanjin", LastName = "Ajanic" });
            Console.WriteLine(Id);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    Console.WriteLine($"{row["FirstName"]}  {row["LastName"]} ");
                }
            }




        }

    }
}
