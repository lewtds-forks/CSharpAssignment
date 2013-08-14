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
                ID = "aoe"
            };

            var b = new Student()
            {
                ID = "aoed"
            };

            Assert.False(a.Equals(b));
        }

        [Test]
        public void TestClass()
        {
            var a = new Class()
            {
                Name = "C1203L"
            };

            var b = new Class()
            {
                Name = "C1203F"
            };

            Assert.False(a.Equals(b));
        }
    }
}
