using System;
using System.Collections.Generic;

namespace StudentManager
{
    public struct TimeSlot
    {
	
    }

    public class Class
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public List<TimeSlot> TimeTable { get; set; }

        public string Room { get; set; }

        public string Teacher { get; set; }

        public Class()
        {

        }
    }
}

