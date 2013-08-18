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
            ID = "B01414",
            Name = "Trung"
        };
        private Class c1203l = new Class()
        {
            Name = "C1203L",
            Teacher = "NhatNK"
        };

        [SetUp]
        public void Init() {
            m = new Manager();
        }

        [Test]
        public void TestGetStudentByID() {
            Assert.Null(Identity.GetObjectById(m.Students, "B01415"));

            m.Students.Add(trung);
            Assert.AreSame(trung, Identity.GetObjectById(m.Students, "B01414"));
            Assert.Null(Identity.GetObjectById(m.Students, ""));
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
}
