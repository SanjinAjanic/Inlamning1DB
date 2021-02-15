using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace Inlamning1DB
{
    class Databas
    {
        public string ConnectionString { get; set; } = @"Data Source=(localdb)\MSSQLLocalDB;Integrated Security=true;database={0}";


        public string DatabaseName { get; set; } = "Familjeträd";

        public void Setup()
        {
            if (CreateDatabase() == true)
            {
                CreateTable();
                InsertMembers();
            }
        }
        

        /// <summary>
        /// Försöker skapa upp Basen Familjeträd
        /// </summary>
        /// <returns>true om det gick och skapa databasen och om 
        /// databasen redan finns så false</returns>
        public bool CreateDatabase()
        {            
            try
            {
                DatabaseName = "master";
                ExecuteQuery("CREATE DATABASE Familjeträd");
                DatabaseName = "Familjeträd";
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Skapar upp tabellen Familj.
        /// </summary>
        internal void CreateTable()
        {
            string createTable = "CREATE TABLE Familj (" +
                "Id int PRIMARY KEY IDENTITY (1,1) NOT NULL, " +
                "FirstName nvarchar(50) NOT NULL, " +
                "LastName nvarchar(50) NOT NULL, " +
                "MotherId int NULL, " +
                "FatherId int NULL)";

            ExecuteQuery(createTable);          
        }

        internal void InsertMembers()
        {
            var filename = Path.Combine(Environment.CurrentDirectory, @"..\..\..\..\personer.txt");
            if (File.Exists(filename))
            {
                var fileRows = File.ReadAllLines(filename);
                foreach (var item in fileRows)
                {
                    //Console.WriteLine(item);
                    var split = item.Split(", ");
                    var person = new Person();
                    person.FirstName = split[0].Trim();
                    person.LastName = split[1].Trim();
                    CreatePerson(person);
                }
            }
        }

        public void ExecuteQuery(string sqlString, params (string,string)[] parameters)
        {
            SqlConnection cnn = OpenConnection();

            using (var cmd = new SqlCommand(sqlString, cnn))
            {
                foreach (var item in parameters)
                {
                    cmd.Parameters.AddWithValue(item.Item1, item.Item2);
                }
                
                Console.WriteLine($"{cmd.ExecuteNonQuery()} rows affected!");
            }
            cnn.Close();
        }

        public DataTable ExecuteQueryWithTable(string sqlString, params (string, string)[] parameters)
        {
            var dt = new DataTable(); // förbered Datatable
            var cnn = OpenConnection();


            using (var cmd = new SqlCommand(sqlString, cnn)) // förbered ett kommando
            {
                foreach (var item in parameters)
                {
                    cmd.Parameters.AddWithValue(item.Item1, item.Item2);
                }
                using (var adapter = new SqlDataAdapter(cmd)) // fyll dataset med data
                {
                    adapter.Fill(dt);
                   
                }
            }
            cnn.Close(); // stäng koppling
            return dt; // returnerar datatable
        }

        /// <summary>
        /// koppla till databasen
        /// </summary>
        public SqlConnection OpenConnection()
        {
            var conString = string.Format(ConnectionString, DatabaseName);
            var cnn = new SqlConnection(conString);
            cnn.Open();
            Console.WriteLine($"Using database: {cnn.Database}");
            return cnn;
        }

        public void CreatePerson(Person person)
        {
            string sql = "insert into Familj (FirstName, LastName, MotherId, FatherId)" +
                 "values (@FirstName, @LastName, @MotherId, @FatherId)";
            var parameters = new (string, string)[]
            {
                ("@FirstName", person.FirstName),
                ("@LastName", person.LastName),
                ("@MotherId", person.MotherId.ToString()),
                ("@FatherId", person.FatherId.ToString()),             
            };
            ExecuteQuery(sql, parameters);
        }

        public void DeletePerson(Person person)
        {
            string sql = "DELETE FROM PersonT Where PersonId=@PersonId";
               
            var parameters = new (string, string)[]
            {
                ("@FirstName", person.FirstName),
                ("@LastName", person.LastName),
                ("@MotherId", person.MotherId.ToString()),
                ("@FatherId", person.FatherId.ToString()),
                ("@PersonId", person.ID.ToString()),


            };
            ExecuteQuery(sql, parameters);
        }

        public DataTable ReadPerson(Person person)
        {
            string sql = "Select * FROM PersonT Where FirstName=@FirstName OR LastName=@LastName";

            var parameters = new (string, string)[]
            {
                ("@FirstName", person.FirstName),
                ("@LastName", person.LastName),
            };
            return ExecuteQueryWithTable(sql, parameters);
        }

        public  void UpdatePerson(Person person)
        {
            string sql = "UPDATE PersonT Set FirstName=@FirstName, LastName=@LastName, MotherId=@MotherId, FatherId=@FatherId WHERE PersonId=@PersonId";

            var parameters = new (string, string)[]
            {
                ("@FirstName", person.FirstName),
                ("@LastName", person.LastName),
                ("@MotherId", person.MotherId.ToString()),
                ("@FatherId", person.FatherId.ToString()),
                ("@PersonId", person.ID.ToString()),



            };
            ExecuteQuery(sql, parameters);
        }

        public void SetParents(Person person)
        {
            string sql = "UPDATE PersonT Set MotherId=@MotherId, FatherId=@FatherId WHERE PersonId=@PersonId";

            var parameters = new (string, string)[]
            {
                
                ("@MotherId", person.MotherId.ToString()),
                ("@FatherId", person.FatherId.ToString()),
                ("@PersonId", person.ID.ToString()),



            };
            ExecuteQuery(sql, parameters);
        }

        public int ReadPersonId(Person person)
        {

            string sql = "Select * FROM Familj Where FirstName=@FirstName AND LastName=@LastName";

            var parameters = new (string, string)[]
            {
                ("@FirstName", person.FirstName),
                ("@LastName", person.LastName),
               

            };

            var dt = ExecuteQueryWithTable(sql, parameters);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                var Id = (int)row["PersonId"];
                return Id;
            }
            return 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Person> ReadPerson(string name)
        {
            string sql = "SELECT * FROM Familj WHERE FirstName LIKE @FirstName";
            DataTable dt = ExecuteQueryWithTable(sql, ("@FirstName", $"{name}%"));
            List<Person> people = new List<Person>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    people.Add(ConvertToPerson(row));
                }               
            }
            return people;
        }
        // 1. a) gör anrop till databasen of fråga efter alla personer med namnet du fått in. klar!!!
        // 1. b) kolla om datatable innehåller någon person. klar!!!
        // 1. c) omvandla varje rad i dt.Rows till ett person-objekt och lägg till i en lista. klar!!!
        // 2. returnera listan av Person. klar !!!
        // 3. skriv en sammanfattning om vad metoden gör...



        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private Person ConvertToPerson(DataRow row)
        {
            Person person = new Person();
            person.ID = (int)row["Id"]; 
            person.FirstName = row["FirstName"].ToString();
            person.LastName = row["LastName"].ToString();
            person.MotherId = (int)row["MotherId"];
            person.FatherId = (int)row["FatherId"];
            return person;
        }

        // Ska ta varje kolumn i row och stoppa in i varje prop i person.
        // 1. Skapa en ny Person och döp den till person. klar!!
        // 2. Tilldela sedan varje prop i person ett värde från row.
        //      tex. person.FirstName = row["FirstName"]; klar!!!
        // 3. returnera person. klar!!!
        // 4. Skriv en sammanfattning om vad metoden gör!



        
        public int IfNotExist(Person person)
        {
            var id = ReadPersonId(person);
            if (id == 0)
            {                
                CreatePerson(person);
                id = ReadPersonId(person);
                person.ID = id;
            }
            return id;
        }

    }
}
