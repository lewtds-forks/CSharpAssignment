using System;
using System.Collections.Generic;
using System.Linq;

namespace StudentManager
{
    public class Manager
    {
        private HashSet<Tuple<Class, Student>> classStudentRels;
        private SortedSet<Class> _AllClass;
        private HashSet<Tuple<Class, Room, TimeSlot>> classRoomSlots;

        /// <summary>
        /// Get all class-student relationships.
        /// </summary>
        /// <remarks>Read-only. Changes won't propagate back.</remarks>
        /// <value>A hashset containing tuples of Class and Student.</value>
        public HashSet<Tuple<Class, Student>> ClassStudentRel {
            get {
                return new HashSet<Tuple<Class, Student>>(classStudentRels);
            }
        }

        public IEnumerable<Student> AllStudents
        {
            get {
                return (from pair in classStudentRels
                        select pair.Item2);
            }
        }

        public IEnumerable<Class> AllClasses
        {
            get {
                // ToList() to make it immutable
                return this._AllClass.ToList();
            }
        }

        public Manager()
        {
            classStudentRels = new HashSet<Tuple<Class, Student>>();
            _AllClass = new SortedSet<Class>();
        }

        public bool AddStudentClass(Student s, Class cl)
        {
            // The student should not be in any other class and
            // the class should already be registered
            return this.FindStudentByID(s.ID) == null && 
                this._AllClass.Contains(cl) &&
                classStudentRels.Add(new Tuple<Class, Student> (cl, s));
        }

        public bool RemoveStudentClass(Student s, Class cl)
        {
            return classStudentRels.Remove(new Tuple<Class, Student> (cl, s));
        }

        public bool ChangeStudentClass(Student s, Class frm, Class to)
        {
            return this.RemoveStudentClass(s, frm) &&
                this.AddStudentClass(s, to);
        }

        public Class FindClassOfStudent(Student s) {
            return (from pair in classStudentRels
                    where pair.Item2 == s
                    select pair.Item1).FirstOrDefault();
        }

        public IEnumerable<Student> GetAllStudentFromClass(Class cl)
        {
            return (from pair in classStudentRels
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
            return (from pair in classStudentRels
                    where pair.Item2.ID == id
                    select pair.Item2).FirstOrDefault();
        }
    }
}

