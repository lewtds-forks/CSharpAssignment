using System;
using System.Collections.Generic;

namespace StudentManager
{
    public class Student: IComparable<Student>, IEquatable<Student>
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public override string ToString()
        {
            return String.Format("{0} {1} {2}", ID, Name, Address);
        }

        public int CompareTo(Student other)
        {
            return this.ID - other.ID;
        }

        public bool Equals(Student other)
        {
            return this.ID == other.ID;
        }
    }
}

