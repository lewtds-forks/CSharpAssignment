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

    public struct ClassRoomSlotTuple
    {
        public object ClassId {get; set;}
        public object RoomId {get; set;}
        public object SlotId {get; set;}
    }

    class Manager
    {
        // Mutable collections, expected to be edited by
        // users.
        public HashSet<Class> Classes { get; private set; }

        public HashSet<Student> Students { get; private set; }

        public HashSet<Room> Rooms { get; private set; }

        // TODO Prevent overlapping timeslots
        public HashSet<TimeSlot> TimeSlots { get; private set; }

        // TODO Need testing
        private HashSet<Tuple<Class, Student>> classStudents;
        private HashSet<Tuple<Class, Room, TimeSlot>> allocation;

        // Immutable through IEnumerable.
        // This level of encapsulation is enough for such a small
        // project.
        public IEnumerable<Tuple<Class, Student>> ClassStudents
        { get { return this.classStudents; } }

        public IEnumerable<Tuple<Class, Room, TimeSlot>> Allocation
        { get { return this.allocation; } }

        public Dictionary<String, String> UriMapping { get; set; }
        public IPersistenceService database;

        /// <summary>
        /// In memory constructor. All data is created fresh. Mostly for testing
        /// purposes.
        /// </summary>
        public Manager()
        {
            Classes = new HashSet<Class>();
            Students = new HashSet<Student>();
            Rooms = new HashSet<Room>();
            TimeSlots = new HashSet<TimeSlot>();
            classStudents = new HashSet<Tuple<Class, Student>>();
            allocation = new HashSet<Tuple<Class, Room, TimeSlot>>();
        }

        /// <summary>
        /// Initializes with external data sources.
        /// </summary>
        /// <param name='database'>
        /// A database accessing object.
        /// </param>
        /// <param name='UriMapping'>
        /// A mapping between resources' name and their URI. The list of resources that
        /// we need is: classes, students, rooms, timeslots, classstudents, allocation.
        /// </param>
        public Manager(IPersistenceService database, Dictionary<String, String> UriMapping)
        {
            this.UriMapping = UriMapping;
            this.database = database;

            // FIXME What if any of the database resources isn't created yet?
            Classes = database.load<HashSet<Class>>
                (UriMapping["classes"]);
            Students = database.load<HashSet<Student>>
                (UriMapping["students"]);
            Rooms = database.load<HashSet<Room>>
                (UriMapping["rooms"]);
            TimeSlots = database.load<HashSet<TimeSlot>>
                (UriMapping["timeslots"]);
            var _classStudents = database.load<List<ClassStudentTuple>>
                (UriMapping["class-students"]);
            var _allocation = database.load<List<ClassRoomSlotTuple>>
                (UriMapping["allocation"]);

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

            allocation = new HashSet<Tuple<Class, Room, TimeSlot>>();
            foreach(var tuple in _allocation)
            {
                var klass = (Class) Identity
                    .GetObjectById(this.Classes, tuple.ClassId);
                var room = (Room) Identity
                    .GetObjectById(this.Rooms, tuple.RoomId);
                var slot = (TimeSlot) Identity
                    .GetObjectById(this.TimeSlots, tuple.SlotId);
                RegisterClassRoomTimeSlot(klass, room, slot);
            }
        }

        public Student GetStudentById(String id)
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
            // FIXME This won't remove the student from Students
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

        public void Save()
        {
            database.save<HashSet<Class>>
                (UriMapping["classes"], Classes);
            database.save<HashSet<Student>>
                (UriMapping["students"], Students);
            database.save<HashSet<Room>>
                (UriMapping["rooms"], Rooms);
            database.save<HashSet<TimeSlot>>
                (UriMapping["timeslots"], TimeSlots);

            // Custom serialization
            var _classStudents = new List<ClassStudentTuple>();

            foreach(var tuple in classStudents)
            {
                _classStudents.Add(new ClassStudentTuple() {
                    StudentId = tuple.Item2.GetId(),
                    ClassId = tuple.Item1.GetId()
                });
            }

            database.save<List<ClassStudentTuple>>
                (UriMapping["class-students"], _classStudents);


            var _allocation = new List<ClassRoomSlotTuple>();

            foreach(var tuple in allocation)
            {
                _allocation.Add(new ClassRoomSlotTuple() {
                    ClassId = tuple.Item1.GetId(),
                    RoomId = tuple.Item2.GetId(),
                    SlotId = tuple.Item3.GetId()
                });
            }

            database.save<List<ClassRoomSlotTuple>>
                (UriMapping["allocation"], _allocation);
        }
    }
}

