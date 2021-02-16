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
            MainMenu();

        }


        public static void AddPerson()
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
                int nr;
                do
                {

                    Console.WriteLine("Choose a number");

                    int.TryParse(Console.ReadLine(), out nr);

                } while (nr > people.Count || nr <= 0);
                var person = people[nr - 1];
                people = db.ReadPerson(people[nr-1].FirstName);


                SelectedPerson(person);

           

            }

         
        }

        public static void UpdatePerson(Person person)
        {
            Console.WriteLine("Vad vill du ändra?");
            Console.WriteLine("1. Förnamn");
            Console.WriteLine("2. Efternamn");
            Console.WriteLine("3. Mamma");
            Console.WriteLine("4. Pappa");
            Console.WriteLine("5. Ta bort person");
           
            Console.Write("> ");
            var db = new Databas();
            int.TryParse(Console.ReadLine(), out int chooise);
            switch (chooise)
            {
                case 1:
                    Console.Write("Ange förnamn: ");
                    person.FirstName = Console.ReadLine();
                    break;
                case 2:
                    Console.Write("Ange efternamn: ");
                    person.LastName = Console.ReadLine();
                    break;
                case 3:
                    Console.Write("Ange Mamma: ");
                    string name = Console.ReadLine();
                    var mother = db.ReadPerson(name);
                    person.MotherId = mother[0].ID;
                    break;
                case 4:
                    Console.Write("Ange Pappa: ");
                    name = Console.ReadLine();
                    var father = db.ReadPerson(name);
                    person.FatherId = father[0].ID;
                        break;
                case 5:
                    db.DeletePerson(person);
                    MainMenu();
                    break;
                    
                default:
                    break;
            }
            db.UpdatePerson(person);
        }

        private static void MainMenu()
        {
            while (true)
            {

                Console.Clear();
                Console.WriteLine("1. Add person");
                Console.WriteLine("2. Find person");
                Console.WriteLine("E. Exit program");
                Console.Write("> ");
                var chooise = Console.ReadLine();
                if (chooise == "1")
                {
                    AddPerson();
                }
                else if (chooise == "2")
                {
                    FindPerson();
                }
                else if (chooise.ToUpper() == "E")
                {
                    Environment.Exit(0);
                }
                else
                {
                    Console.WriteLine("Invalid input try again");
                    Console.ReadKey();
                }


            }
     
        }
        public static void SelectedPerson(Person person)
        {
            Console.WriteLine("Vad vill du göra?");
            Console.WriteLine("1. Visa föräldrar");
            Console.WriteLine("2. Uppdatera person");
            Console.Write("> ");
            var choice = Console.ReadLine();
            if (choice == "1")
            {
                ShowParents(person);
            }
            else if (choice == "2")
            {
                UpdatePerson(person);
            }
            else
            {
                MainMenu();
            }
        }
        private static void ShowParents(Person person)
        {
            var db = new Databas();
            
            List<Person> parents = new List<Person>();
            parents.Add(db.ReadPersonById(person.MotherId));
            parents.Add(db.ReadPersonById(person.FatherId));
            foreach (var parent in parents)
            {
                Console.WriteLine(parent.FirstName + " " + parent.LastName);
            }
            Console.ReadLine();
        }
    }
}
