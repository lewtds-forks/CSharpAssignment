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
            m.AddStudentClass(student, c);
            Console.WriteLine(student);

        }
    }
}
