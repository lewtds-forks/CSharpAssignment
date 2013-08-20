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
            mapping.Add("classes", "classes.xml");
            mapping.Add("students", "students.xml");
            mapping.Add("rooms", "rooms.xml");
            mapping.Add("timeslots", "timeslots.xml");
            mapping.Add("class-students", "class-students.xml");
            mapping.Add("allocation", "allocation.xml");

            var db = new XMLDatabase();
            Manager m = new Manager(db, mapping);
            
            new MainScreen(m).Start();
        }
    }
}
