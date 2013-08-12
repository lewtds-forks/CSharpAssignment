using System;
using System.Collections.Generic;

namespace StudentManager
{
    public class Student: Identity
    {
        public string Name { get; set; }

        public string Address { get; set; }

        public override string ToString()
        {
            return String.Format("{0} {1} {2}", ID, Name, Address);
        }
    }
}

