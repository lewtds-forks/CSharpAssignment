using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace StudentManager.Tests
{
    [TestFixture]
    class TestStudentManagement
    {
        private Manager m;
        private Student trung = new Student()
        {
            ID = 10,
            Name = "Trung"
        };
        private Class c1203l = new Class()
        {
            ID = 3,
            Name = "C1203L",
            Teacher = "NhatNK"
        };

        [SetUp]
        public void Init() {
            m = new Manager();
        }

        [Test]
        public void TestGetStudentByID() {
            Assert.Null(m.GetStudentById(10));

            m.Students.Add(trung);
            Assert.AreSame(trung, m.GetStudentById(10));
            Assert.Null(m.GetStudentById(80));
        }

        [Test]
        public void TestRegisterStudent() {
            Assert.False(m.RegisterStudentWithClass(trung, c1203l));

            m.Classes.Add(c1203l);
            m.Students.Add(trung);

            Assert.True(m.RegisterStudentWithClass(trung, c1203l));
            var tuple = m.ClassStudents.SingleOrDefault();
            Assert.NotNull(tuple);
            Assert.AreSame(tuple.Item1, c1203l);
            Assert.AreSame(tuple.Item2, trung);
        }

        [Test]
        public void TestChangeClassOfStudent() {
            var bogusClass = new Class() 
            { 
                ID = 40,
                Name = "Bogus"
            };
            m.Classes.Add(c1203l);
            m.Classes.Add(bogusClass);
            m.Students.Add(trung);
            m.RegisterStudentWithClass(trung, c1203l);
            m.SwitchClassOfStudent(trung, c1203l, bogusClass);

            var tuple = m.ClassStudents.SingleOrDefault();
            Assert.AreSame(tuple.Item1, bogusClass);
            Assert.AreSame(tuple.Item2, trung);
        }
    }

    [TestFixture]
    class TestClassAllocation
    {
        private Manager m;
        private Class c1203l = new Class()
        {
            ID = 3,
            Name = "C1203L"
        };
        private Class bogus = new Class()
        {
            ID = 4,
            Name = "Bogus"
        };

        private Room lab1 = new Room() {
            Name = "Lab 1"
        };

        private TimeSlot evening = new TimeSlot() {
            StartTime = DateTime.MinValue.AddDays(4).AddHours(17).AddMinutes(30),
            EndTime = DateTime.MinValue.AddDays(4).AddHours(19).AddMinutes(30)
        };

        [SetUp]
        public void Init() {
            m = new Manager();
            m.Classes.Add(c1203l);
            m.Rooms.Add(lab1);
            m.TimeSlots.Add(evening);
        }

        [Test]
        public void RegisterGoodTimeSlot() {
            Assert.True(m.RegisterClassRoomTimeSlot(c1203l, lab1, evening));
        }

        [Test]
        public void RegisterSameSlot()
        {
            Assert.False(m.RegisterClassRoomTimeSlot(bogus, lab1, evening));
        }

        [Test]
        public void RemoveClassSlot() {
            m.RegisterClassRoomTimeSlot(c1203l, lab1, evening);
            Assert.True(m.RemoveClassRoomTimeSlot(c1203l, lab1, evening));
            Assert.Null(m.Allocation.SingleOrDefault());
        }
    }
}
