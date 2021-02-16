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
            ExecuteQuery("SET IDENTITY_INSERT Familj ON ");
            var filename = Path.Combine(Environment.CurrentDirectory, @"..\..\..\..\personer.txt");
            if (File.Exists(filename))
            {
                var fileRows = File.ReadAllLines(filename);
                foreach (var item in fileRows)
                {
                    //Console.WriteLine(item);
                    var split = item.Split(", ");
                    var person = new Person();
                    person.ID = int.Parse(split[0]);
                    person.FirstName = split[1].Trim();
                    person.LastName = split[2].Trim();
                    person.MotherId = int.Parse(split[3]);
                    person.FatherId = int.Parse(split[4]);
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
            string sql = "SET IDENTITY_INSERT Familj ON; insert into Familj (Id, FirstName, LastName, MotherId, FatherId)" +
                 "values (@Id, @FirstName, @LastName, @MotherId, @FatherId)";
            var parameters = new (string, string)[]
            {
                ("@Id", person.ID.ToString()),
                ("@FirstName", person.FirstName),
                ("@LastName", person.LastName),
                ("@MotherId", person.MotherId.ToString()),
                ("@FatherId", person.FatherId.ToString()),             
            };
            ExecuteQuery(sql, parameters);
        }

        public void DeletePerson(Person person)
        {
            string sql = "DELETE FROM Familj Where Id=@Id";
               
            var parameters = new (string, string)[]
            {
                ("@FirstName", person.FirstName),
                ("@LastName", person.LastName),
                ("@MotherId", person.MotherId.ToString()),
                ("@FatherId", person.FatherId.ToString()),
                ("@Id", person.ID.ToString()),


            };
            ExecuteQuery(sql, parameters);
        }


        internal Person ReadPersonById(int id)
        {
            string sql = "SELECT * FROM Familj WHERE Id= @Id";
            DataTable dt = ExecuteQueryWithTable(sql, ("@Id", id.ToString()));
            if (dt.Rows.Count > 0)
            {
                return ConvertToPerson(dt.Rows[0]);
            }
            return new Person();
        }

     

        public  void UpdatePerson(Person person)
        {
            string sql = "UPDATE Familj Set FirstName=@FirstName, LastName=@LastName, MotherId=@MotherId, FatherId=@FatherId WHERE Id=@Id";

            var parameters = new (string, string)[]
            {
                ("@FirstName", person.FirstName),
                ("@LastName", person.LastName),
                ("@MotherId", person.MotherId.ToString()),
                ("@FatherId", person.FatherId.ToString()),
                ("@Id", person.ID.ToString()),



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
                var Id = (int)row["Id"];
                return Id;
            }
            return 0;
        }
    
        public List<Person> ReadPerson(string name)
        {
            string sql = "SELECT * FROM Familj WHERE FirstName LIKE @FirstName";
            DataTable dt = ExecuteQueryWithTable(sql, ("@FirstName", $"%{name}%"));
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
