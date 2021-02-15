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
            var db = new Databas();
            db.Setup();
            FindPerson();





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

            //var sanjin = new Person() { FirstName = "Sanjin", LastName = "Ajanic" };

            ////var dt = db.ReadPerson(new Person { FirstName = "Sanjin", LastName = "Ajanic" });
            //var dt = db.ReadPerson(sanjin);


            //if (dt.Rows.Count > 0)
            //{
            //    foreach (DataRow row in dt.Rows)
            //    {
            //        Console.WriteLine($"{row["FirstName"]}  {row["LastName"]} ");
            //    }
            //}

            ////var Sanjin = db.ReadPersonId(new Person { FirstName = "Sanjin", LastName = "Ajanic" });
            //var Sanjin = db.ReadPersonId(sanjin);
            //var Elvedin = db.ReadPersonId(new Person { FirstName = "Elvedin", LastName = "Ajanic" });
            //sanjin.FatherId = Elvedin;
            //sanjin.LastName = "Coolman";
            //var Denana = db.ReadPersonId(new Person { FirstName = "Denana", LastName = "Niksic" });
            //db.SetParents(new Person { ID = Sanjin, FatherId = Elvedin, MotherId = Denana });
            //var Asim = db.ReadPersonId(new Person { FirstName = "Asim", LastName = "Ajanic" });
            //var Djulsuma = db.ReadPersonId(new Person { FirstName = "Djulsuma", LastName = "Puzic" });
            //db.SetParents(new Person { ID = Elvedin, FatherId = Asim, MotherId = Djulsuma });
            //var Zenjil = db.ReadPersonId(new Person { FirstName = "Zenjil", LastName = "Niksic" });
            //var Azra = db.ReadPersonId(new Person { FirstName = "Azra", LastName = "Duranovic" });
            //db.SetParents(new Person { ID = Denana, FatherId = Zenjil, MotherId = Azra });





        }


        public void AddPerson()
        {
            Person person = new Person();
            Console.Write("Enter a first name: ");
            person.FirstName = Console.ReadLine();
            Console.Write("Enter a last name: ");
            person.LastName = Console.ReadLine();
            Databas db = new Databas();
            db.IfNotExist(person);
        }

        public static void FindPerson()
        {
            Console.Write("Enter person to find: ");
            var name = Console.ReadLine();
            Databas db = new Databas();
            List<Person> people = db.ReadPerson(name);
            if (people.Count > 0)             
            {
                int myInt = 1;
              
                foreach (var item in people)
                {
                    Console.WriteLine(myInt + ". " + item.FirstName + " " + item.LastName);
                    myInt++;                    
                }
                Console.WriteLine("Choose a number");
                Console.ReadLine();

                // be användaren om en siffra - klar!!!
                // läs in användarens svar i en variabel.
                // kolla så att siffran inte är utanför listans gräns
                // välja ut personen ur listan som stämmer överens med användarens svar.
                // stoppa personen i en egen Person variabel.



                // 1. Be användaren om ett namn. Klar!!!
                // 2. skicka in namnet i ReadPerson och kolla om det finns några personer med det namnet. klar !!!
                // 3. a) skapa en int som har värdet 1 ovanför foreach-loopen på rad 96. klar!!!
                // 3. b) skriv ut din int först i cw klar!!!
                // 3. c) Om listan som kommer tillbaka inte är tom visa personerna i listan. klar!!!
                // 4. låt användaren välja den personen som har hittats.
                // 5. om listan är tom skriv ut att det inte fanns någon match.



            }
        }
    }
}
