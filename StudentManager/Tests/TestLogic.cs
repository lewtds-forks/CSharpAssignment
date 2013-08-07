using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace StudentManager.Tests
{
    [TestFixture]
    class TestLogic
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
        public void TestRemoveStudent() {
            m.Classes.Add(c1203l);
            m.Students.Add(trung);
        }
    }
}
