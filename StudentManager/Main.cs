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
            var m = new Manager();
            var c = new Class()
            {
                ID = 10
            };
            m.AddStudentToClass(student, c);
            Console.WriteLine(m.GetStudentFromID(5));
            Console.WriteLine(m.GetClassFromID(10));
            Console.WriteLine(student);

        }
    }
}
