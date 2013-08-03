using System;
using System.Collections.Generic;

namespace StudentManager
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Student student = new Student()
            {
                ID = 5,
                Name = "Thinh",
                Address = "KT"
            };

            Student chin = new Student()
            {
                ID = 5,
                Name = "Trung",
                Address = "Home"
            };
            var m = new Manager();
            var c = new Class()
            {
                ID = 10,
                Teacher = "NhatNK"
            };
            m.RegisterClass(c);
            m.AddStudentClass(student, c);
            Console.WriteLine(m.AddStudentClass(chin, c));
            Console.WriteLine(m.FindStudentByID(5) == null);

            foreach (var s in m.AllClasses)
            {
                Console.WriteLine(s.Teacher);
            }
        }
    }
}
