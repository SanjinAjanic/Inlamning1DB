using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Inlamning1DB
{
    class Databas
    {
        public string ConnectionString { get; set; } = @"Data Source=(localdb)\MSSQLLocalDB;Integrated Security=true;database={0}";
        public string DatabaseName { get; set; } = "Persons";

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
            string sql = "insert into PersonT (FirstName, LastName, MotherId, FatherId)" +
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

            string sql = "Select * FROM PersonT Where FirstName=@FirstName AND LastName=@LastName";

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
