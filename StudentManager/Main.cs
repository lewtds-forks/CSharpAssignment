using System;
using System.Collections.Generic;
using System.Linq;
using StudentManager.TextUi;

namespace StudentManager
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Manager m = new Manager();

            var c1 = new Class()
            {
                Name = "C1203L",
                Teacher = "NhatNK"
            };

            var c2 = new Class()
            {
                Name = "C1204L",
                Teacher = "SinhNX"
            };

            var s1 = new Student() {
                ID = "B01414",
                Name = "Trung"
            };

            var s2 = new Student()
            {
                ID = "B01415",
                Name = "Thinh"
            };

            var r1 = new Room()
            {
                Name = "Class 1"
            };

            var t1 = new TimeSlot()
            {
                StartTime = DateTime.MinValue.AddDays(4).AddHours(17).AddMinutes(30),
                EndTime = DateTime.MinValue.AddDays(4).AddHours(19).AddMinutes(30)
            };

            m.Classes.Add(c1);
            m.Classes.Add(c2);

            m.Students.Add(s1);
            m.Students.Add(s2);

            m.Rooms.Add(r1);

            m.TimeSlots.Add(t1);

            m.RegisterStudentWithClass(s1, c1);
            m.RegisterStudentWithClass(s2, c2);

            m.RemoveClassRoomTimeSlot(c1, r1, t1);

            m.UriMapping = new Dictionary<string, string>();
            m.UriMapping.Add("classes", "/tmp/sm/classes.xml");
            m.UriMapping.Add("students", "/tmp/sm/students.xml");
            m.UriMapping.Add("rooms", "/tmp/sm/rooms.xml");
            m.UriMapping.Add("timeslots", "/tmp/sm/timeslots.xml");
//            m.UriMapping.Add("class-students", "/tmp/sm/class-students.xml");
//            m.UriMapping.Add("allocation", "/tmp/sm/allocation.xml");
//
            m.database = new XMLDatabase();

            m.Save();
//            new MainScreen(m).Start();
        }
    }
}
