using System;
using System.Collections.Generic;
using System.Linq;

namespace StudentManager
{
    public class Manager
    {
        private HashSet<Tuple<Class, Student>> ClassStudentRel;

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

        public Student GetStudentFromID(int ID)
        {
            return (from tuple in ClassStudentRel
                    where tuple.Item2.ID == ID
                    select tuple.Item2).FirstOrDefault();
        }

        public Class GetClassFromID(int ID)
        {
            return (from tuple in ClassStudentRel 
                    where tuple.Item1.ID == ID
                    select tuple.Item1).FirstOrDefault();
        }
    }
}

