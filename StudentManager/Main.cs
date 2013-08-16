using System;
using System.Collections.Generic;
using System.Linq;
using StudentManager.TextUi;

namespace StudentManager
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var mapping = new Dictionary<string, string>();
            mapping.Add("classes", "/tmp/sm/classes.xml");
            mapping.Add("students", "/tmp/sm/students.xml");
            mapping.Add("rooms", "/tmp/sm/rooms.xml");
            mapping.Add("timeslots", "/tmp/sm/timeslots.xml");
            mapping.Add("class-students", "/tmp/sm/class-students.xml");
            mapping.Add("allocation", "/tmp/sm/allocation.xml");

            var db = new XMLDatabase();
            Manager m = new Manager(db, mapping);
            
            new MainScreen(m).Start();
        }
    }
}
