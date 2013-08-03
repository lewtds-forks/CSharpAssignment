using System;

namespace StudentManager
{
    public class Student
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public override string ToString()
        {
            return String.Format("{0} {1} {2} {3}", ID, ClassID, Name, Address);
        }
    }
}

