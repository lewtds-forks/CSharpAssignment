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
            mapping.Add("class-students", "class-students.xml");

            var m = new Manager();
            var db = new XMLDatabase();
            m.UriMapping = mapping;
            m.Database = db;

            // Check if all the files above exists
            if ((from pair in mapping
                 select System.IO.File.Exists(pair.Value)).All(true.Equals))
            {
                m.Load();
            }

            new MainScreen(m).Start();
        }
    }
}
