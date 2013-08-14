using System;
using System.Collections.Generic;

namespace StudentManager
{
    public class Room : Identity
    {
        [Identity.ID]
        public String Name { get; set; }
    }

    public struct TimeSlot
    {
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
    }

    public class Class : Identity
    {
        [Identity.ID]
        public string Name { get; set; }

        public string Teacher { get; set; }
    }
}

