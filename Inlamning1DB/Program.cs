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
            Databas db = new Databas();
            if (File.Exists(filename))
            {
                var fileRows = File.ReadAllLines(filename);
                foreach (var item in fileRows)
                {
                    Console.WriteLine(item);
                    var split = item.Split(',');
                    var name = split[0].Trim();
                    var lastName = split[1].Trim();
                    db.IfNotExist(new Person(name, lastName));


                }

            }
            foreach (var item in Familj)
            {
                Console.WriteLine(item.FirstName);
            }


            /* var id = db.ReadPersonId(new Person { FirstName = "Denana", LastName = "Ajanic" });
             if (id == 0)
             {
                 db.CreatePerson(new Person { FirstName = "Denana", LastName = "Ajanic" });
                 id = db.ReadPersonId(new Person { FirstName = "Denana", LastName = "Ajanic" });
             }
             db.DeletePerson(new Person { FirstName = "Denana", LastName = "Ajanic", ID = id });
             db.CreatePerson(new Person { FirstName = "Denana", LastName = "Ajanic" });
             id = db.ReadPersonId(new Person { FirstName = "Elvedin", LastName = "Ajanic" });



            // db.DeletePerson(new Person { FirstName = "Elvedin", LastName = "Ajanic", ID = id });

             db.CreatePerson(new Person { FirstName = "Elvedin", LastName = "Ajanic" });
             var Id = db.ReadPersonId(new Person { FirstName = "Sanjin", LastName = "Ajanic" });
             db.UpdatePerson(new Person { FirstName = "Sanjin", LastName = "Ajanic", ID = Id });

             */
            var dt = db.ReadPerson(new Person { FirstName = "Sanjin", LastName = "Ajanic" });


            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    Console.WriteLine($"{row["FirstName"]}  {row["LastName"]} ");
                }
            }

            var Sanjin = db.ReadPersonId(new Person { FirstName = "Sanjin", LastName = "Ajanic" });
            var Elvedin = db.ReadPersonId(new Person { FirstName = "Elvedin", LastName = "Ajanic" });
            var Denana = db.ReadPersonId(new Person { FirstName = "Denana", LastName = "Niksic" });
            db.SetParents(new Person { ID = Sanjin, FatherId = Elvedin, MotherId = Denana });
            var Asim = db.ReadPersonId(new Person { FirstName = "Asim", LastName = "Ajanic" });
            var Djulsuma = db.ReadPersonId(new Person { FirstName = "Djulsuma", LastName = "Puzic" });
            db.SetParents(new Person { ID = Elvedin, FatherId = Asim, MotherId = Djulsuma });
            var Zenjil = db.ReadPersonId(new Person { FirstName = "Zenjil", LastName = "Niksic" });
            var Azra = db.ReadPersonId(new Person { FirstName = "Azra", LastName = "Duranovic" });
            db.SetParents(new Person { ID = Denana, FatherId = Zenjil, MotherId = Azra });

        



        }

    }
}
