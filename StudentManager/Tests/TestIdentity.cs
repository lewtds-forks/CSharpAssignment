using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace StudentManager.Tests
{
    [TestFixture]
    class TestIdentityComparison
    {
        [Test]
        public void TestStudent()
        {
            var a = new Student()
            {
                ID = 10
            };

            var b = new Student()
            {
                ID = 5
            };

            var c = new Student() { ID = 10 };

            Assert.That(a.CompareTo(b) > 0);
            Assert.False(a.Equals(b));
            Assert.True(a.Equals(c));
        }

        [Test]
        public void TestClass()
        {
            var a = new Class()
            {
                ID = 10
            };

            var b = new Class()
            {
                ID = 5
            };

            var c = new Class() { ID = 10 };

            Assert.That(a.CompareTo(b) > 0);
            Assert.False(a.Equals(b));
            Assert.True(a.Equals(c));
        }
    }
}
