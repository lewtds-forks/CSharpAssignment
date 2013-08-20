using System;
using System.Collections.Generic;
using System.Linq;

namespace StudentManager
{
    // The purpose of these 2 structs is only to store their respective objects'
    // ID
    public struct ClassStudentTuple
    {
        public object StudentId {get; set;}
        public object ClassId {get; set;}
    }

    class Manager
    {
        // Mutable collections, expected to be edited by
        // users.
        public HashSet<Class> Classes { get; private set; }

        public HashSet<Student> Students { get; private set; }

        // TODO Need testing
        private HashSet<Tuple<Class, Student>> classStudents;

        // Immutable through IEnumerable.
        // This level of encapsulation is enough for such a small
        // project.
        public IEnumerable<Tuple<Class, Student>> ClassStudents
        { get { return this.classStudents; } }

        public Dictionary<String, String> UriMapping { get; set; }
        public IPersistenceService Database { get; set; }

        /// <summary>
        /// In memory constructor. All data is created fresh. Mostly for testing
        /// purposes.
        /// </summary>
        public Manager()
        {
            Classes = new HashSet<Class>();
            Students = new HashSet<Student>();
            classStudents = new HashSet<Tuple<Class, Student>>();
        }

        /// <summary>
        /// Initializes with external data sources.
        /// </summary>
        /// <param name='Database'>
        /// A Database accessing object.
        /// </param>
        /// <param name='UriMapping'>
        /// A mapping between resources' name and their URI. The list of resources that
        /// we need is: classes, students, rooms, timeslots, classstudents, allocation.
        /// </param>
        public void Load()
        {
            // FIXME What if any of the Database resources isn't created yet?
            Classes = Database.load<HashSet<Class>>
                (UriMapping["classes"]);
            Students = Database.load<HashSet<Student>>
                (UriMapping["students"]);
            var _classStudents = Database.load<List<ClassStudentTuple>>
                (UriMapping["class-students"]);

            // These "relationships" need manual deserialization
            classStudents = new HashSet<Tuple<Class, Student>>();
            foreach(var tuple in _classStudents)
            {
                var s = (Student) Identity
                    .GetObjectById(this.Students, tuple.StudentId);
                var c = (Class) Identity
                    .GetObjectById(this.Classes, tuple.ClassId);
                RegisterStudentWithClass(s, c);
            }
        }

        /// <summary>
        /// Registers a student with an existing class. The class should have
        /// been added to the Classes property already.
        /// </summary>
        public bool RegisterStudentWithClass(Student s, Class c)
        {
            return this.Students.Contains(s) &&
                this.Classes.Contains(c) &&
                this.classStudents.Add(new Tuple<Class, Student>(c, s));
        }

        public bool RemoveStudentFromClass(Student s, Class c)
        {
            // FIXME This won't remove the student from Students
            return this.classStudents.Remove(new Tuple<Class, Student>(c, s));
        }

        public bool SwitchClassOfStudent(Student s, Class old, Class nw)
        {
            return RemoveStudentFromClass(s, old) &&
                RegisterStudentWithClass(s, nw);
        }

        public void Save()
        {
            Database.save<HashSet<Class>>
                (UriMapping["classes"], Classes);
            Database.save<HashSet<Student>>
                (UriMapping["students"], Students);

            // Custom serialization
            var _classStudents = new List<ClassStudentTuple>();

            foreach(var tuple in classStudents)
            {
                _classStudents.Add(new ClassStudentTuple() {
                    StudentId = tuple.Item2.GetId(),
                    ClassId = tuple.Item1.GetId()
                });
            }

            Database.save<List<ClassStudentTuple>>
                (UriMapping["class-students"], _classStudents);
        }
    }
}

