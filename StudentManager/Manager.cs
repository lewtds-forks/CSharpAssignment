using System;
using System.Collections.Generic;
using System.Linq;

namespace StudentManager
{
    public class Manager
    {
        private HashSet<Tuple<Class, Student>> _ClassStudentRel;
        private SortedSet<Class> _AllClass;

        /// <summary>
        /// Get all class-student relationships.
        /// </summary>
        /// <remarks>Read-only. Changes won't propagate back.</remarks>
        /// <value>A hashset containing tuples of Class and Student.</value>
        public HashSet<Tuple<Class, Student>> ClassStudentRel {
            get {
                return new HashSet<Tuple<Class, Student>>(_ClassStudentRel);
            }
        }

        public IEnumerable<Student> AllStudents
        {
            get {
                return (from pair in _ClassStudentRel
                        select pair.Item2);
            }
        }

        public IEnumerable<Class> AllClasses
        {
            get {
                return this._AllClass;
            }
        }

        public Manager()
        {
            _ClassStudentRel = new HashSet<Tuple<Class, Student>>();
            _AllClass = new SortedSet<Class>();
        }

        public bool AddStudentClass(Student s, Class cl)
        {
            // The student should not be in any other class and
            // the class should already be registered
            return this.FindStudentByID(s.ID) == null && 
                this._AllClass.Contains(cl) &&
                _ClassStudentRel.Add(new Tuple<Class, Student> (cl, s));
        }

        public bool RemoveStudentClass(Student s, Class cl)
        {
            return _ClassStudentRel.Remove(new Tuple<Class, Student> (cl, s));
        }

        public bool ChangeStudentClass(Student s, Class frm, Class to)
        {
            return this.RemoveStudentClass(s, frm) &&
                this.AddStudentClass(s, to);
        }

        public Class FindClassOfStudent(Student s) {
            return (from pair in _ClassStudentRel
                    where pair.Item2 == s
                    select pair.Item1).FirstOrDefault();
        }

        public IEnumerable<Student> GetAllStudentFromClass(Class cl)
        {
            return (from pair in _ClassStudentRel
                    where pair.Item1 == cl
                    select pair.Item2);
        }

        public bool RegisterClass(Class cl)
        {
            return this._AllClass.Add(cl);
        }

        public bool RemoveClass(Class cl)
        {
            return this._AllClass.Remove(cl);
        }

        public Student FindStudentByID(int id) {
            return (from pair in _ClassStudentRel
                    where pair.Item2.ID == id
                    select pair.Item2).FirstOrDefault();
        }
    }
}

