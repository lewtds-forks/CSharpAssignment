using System;
using System.Collections.Generic;
using System.Linq;

namespace StudentManager
{
    public class Manager
    {
        private HashSet<Tuple<Class, Student>> ClassStudentRel;
        private HashSet<Class> AllClass; // Should be a set based on Class.ID

        public Manager()
        {
            ClassStudentRel = new HashSet<Tuple<Class, Student>>();
        }

        public bool AddStudentClass(Student s, Class cl)
        {
            return ClassStudentRel.Add(new Tuple<Class, Student> (cl, s));
        }

        public bool RemoveStudentClass(Student s, Class cl)
        {
            return ClassStudentRel.Remove(new Tuple<Class, Student> (cl, s));
        }

        public bool ChangeStudentClass(Student s, Class frm, Class to)
        {
            return this.RemoveStudentClass(s, frm) &&
                this.AddStudentClass(s, to);
        }

        public Class FindClassOfStudent(Student s) {
            return (from pair in ClassStudentRel
                    where pair.Item2 == s
                    select pair.Item1).FirstOrDefault();
        }

        public IEnumerable<Student> GetAllStudentFromClass(Class cl)
        {
            return (from pair in ClassStudentRel
                    where pair.Item1 == cl
                    select pair.Item2);
        }

        public bool RegisterClass(Class cl)
        {
            return this.AllClass.Add(cl);
        }

        public bool RemoveClass(Class cl)
        {
            return this.AllClass.Remove(cl);
        }
    }
}

