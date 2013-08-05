using System;
using System.Collections.Generic;

namespace StudentManager
{
    public struct Room
    {
        public String Name { get; set; }
    }

    public struct TimeSlot
    {
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
    }

    public class Class: IComparable<Class>, IEquatable<Class>
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Teacher { get; set; }

        public int CompareTo(Class other)
        {
            return this.ID - other.ID;
        }

        public bool Equals(Class other)
        {
            return this.ID == other.ID;
        }
    }
}

