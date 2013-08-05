using System;
using System.Collections.Generic;

namespace StudentManager
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var trung = new Student() {
                Name = "Trung",
                ID = 5
            };

            var c1203l = new Class() {
                ID = 1,
                Name = "C1203L",
                Teacher = "NhatNK"
            };

            var lab1 = new Room() {
                Name = "Lab1"
            };

            var late = new TimeSlot() {
                StartTime = DateTime.MinValue.AddDays(4).AddHours(17).AddMinutes(30),
                EndTime = DateTime.MinValue.AddDays(4).AddHours(19).AddMinutes(30)
            };

            var m = new Manager();
            m.Classes.Add(c1203l);
            m.TimeSlots.Add(late);
            m.Rooms.Add(lab1);
            m.RegisterStudentWithClass(trung, c1203l);
            m.RegisterClassRoomTimeSlot(c1203l, lab1, late);

            foreach (var a in m.Allocation)
            {
                Console.WriteLine("{0} {1} {2}", a.Item1.Name, a.Item2.Name, a.Item3.StartTime.DayOfWeek);
            }
        }
    }
}
