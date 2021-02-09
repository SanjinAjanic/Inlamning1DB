using System;
using System.Collections.Generic;
using System.Text;

namespace Inlamning1DB
{

    class Person
    {

        public Person()
        {

        }
        public Person(string name, string lastname)
        {
            FirstName = name;
            LastName = lastname;
        }

        public int  ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int MotherId { get; set; }
        public int FatherId { get; set; }
    }
}
