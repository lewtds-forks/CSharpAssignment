using System;
using System.Collections.Generic;
using System.Linq;

namespace StudentManager
{
    class Manager
    {
        // Mutable collections, expected to be edited by
        // users.
        public HashSet<Class> Classes { get; private set; }

        public SortedSet<Student> Students { get; private set; }

        public HashSet<Room> Rooms { get; private set; }

        // TODO Prevent overlapping timeslots
        public HashSet<TimeSlot> TimeSlots { get; private set; }

        private HashSet<Tuple<Class, Student>> classStudents;
        private HashSet<Tuple<Class, Room, TimeSlot>> allocation;

        // Immutable through IEnumerable.
        // This level of encapsulation is enough for such a small
        // project.
        public IEnumerable<Tuple<Class, Student>> ClassStudents
        { get { return this.classStudents; } }

        public IEnumerable<Tuple<Class, Room, TimeSlot>> Allocation
        { get { return this.allocation; } }

        public Manager()
        {
            Classes = new HashSet<Class>();
            Students = new SortedSet<Student>();
            Rooms = new HashSet<Room>();
            TimeSlots = new HashSet<TimeSlot>();
            classStudents = new HashSet<Tuple<Class, Student>>();
            allocation = new HashSet<Tuple<Class, Room, TimeSlot>>();
        }

        public Student GetStudentById(int id)
        {
            return (from student in this.Students
                    where student.ID == id
                    select student).SingleOrDefault();
        }

//        public Class GetClassById(int id)
//        {
//            return (from cl in this.Classes
//                    where cl.ID == id
//                    select cl).SingleOrDefault();
//        }

        public Class GetClassByName(String name)
        {
            return (from cl in this.Classes
                    where cl.Name == name
                    select cl).SingleOrDefault();
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
            // TODO This won't remove the student from Students
            return this.classStudents.Remove(new Tuple<Class, Student>(c, s));
        }

        public bool SwitchClassOfStudent(Student s, Class old, Class nw)
        {
            return RemoveStudentFromClass(s, old) &&
                RegisterStudentWithClass(s, nw);
        }

        /// <summary>
        /// Registers a class with a room-timeslot pair. The class, the room and
        /// the timeslot should have been registered to the Classes, Rooms and
        /// TimeSlots properties already.
        /// </summary>
        public bool RegisterClassRoomTimeSlot(Class cl, Room r, TimeSlot t)
        {
            // More safety checks
            return Classes.Contains(cl) &&
                Rooms.Contains(r) &&
                TimeSlots.Contains(t) &&
                (from roomSlot in this.allocation
                 where roomSlot.Item2.Equals(r) && roomSlot.Item3.Equals(t)
                 select roomSlot).Count() == 0 &&
                this.allocation.Add(new Tuple<Class, Room, TimeSlot>(cl, r, t));
        }

        public bool RemoveClassRoomTimeSlot(Class cl, Room r, TimeSlot t)
        {
            return this.allocation.Remove(new Tuple<Class, Room, TimeSlot>(cl, r, t));
        }
    }
}

