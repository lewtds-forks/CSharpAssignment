using System;
using System.Collections.Generic;
using System.Linq;

namespace StudentManager
{
    public class Manager
    {
        private HashSet<Tuple<Class, Student>> classStudentRels;
        private SortedSet<Class> allClasses;
        private HashSet<Tuple<Class, Room, TimeSlot>> classRoomSlots;
        private HashSet<Room> availableRooms;
        private HashSet<TimeSlot> availableTimeSlots;

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
                return this.allClasses.ToList();
            }
        }

        public Manager()
        {
            classStudentRels = new HashSet<Tuple<Class, Student>>();
            allClasses = new SortedSet<Class>();
            availableRooms = new HashSet<Room>();
            availableTimeSlots = new HashSet<TimeSlot>();
            classRoomSlots = new HashSet<Tuple<Class, Room, TimeSlot>>();
        }

        public bool AddStudentClass(Student s, Class cl)
        {
            // The student should not be in any other class and
            // the class should already be registered
            return this.FindStudentByID(s.ID) == null && 
                this.allClasses.Contains(cl) &&
                classStudentRels.Add(new Tuple<Class, Student> (cl, s));
        }

        public bool RemoveStudentClass(Student student, Class clss)
        {
            return classStudentRels.Remove(new Tuple<Class, Student> (clss, student));
        }

        public bool ChangeStudentClass(Student student, Class frm, Class to)
        {
            return this.RemoveStudentClass(student, frm) &&
                this.AddStudentClass(student, to);
        }

        public Class FindClassOfStudent(Student s) {
            return (from pair in classStudentRels
                    where pair.Item2 == s
                    select pair.Item1).FirstOrDefault();
        }

        public IEnumerable<Student> GetAllStudentFromClass(Class clss)
        {
            return (from pair in classStudentRels
                    where pair.Item1 == clss
                    select pair.Item2);
        }

        public bool RegisterClass(Class clss)
        {
            return this.allClasses.Add(clss);
        }

        public bool RemoveClass(Class clss)
        {
            return this.allClasses.Remove(clss);
        }

        public Student FindStudentByID(int id) {
            return (from pair in classStudentRels
                    where pair.Item2.ID == id
                    select pair.Item2).FirstOrDefault();
        }

        public bool RegisterClassRoomSlot(Class clss, Room room, TimeSlot slot)
        {
            // TODO What about overlapping timeslots?
            return this.allClasses.Contains(clss) &&
                this.availableRooms.Contains(room) &&
                this.availableTimeSlots.Contains(slot) &&
                this.classRoomSlots.Add(new Tuple<Class, Room, TimeSlot>(clss, room, slot));
        }
    }
}

