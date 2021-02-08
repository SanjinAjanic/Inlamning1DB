using System;
using System.Data;

namespace Inlamning1DB
{
    class Program

    {

        static void Main(string[] args)

        {
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
