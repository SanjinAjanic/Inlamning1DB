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

                    
                }

            }
            foreach (var item in Familj)
            {
                Console.WriteLine(item.FirstName);
            }

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
