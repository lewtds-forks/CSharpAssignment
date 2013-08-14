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

//        [Test]
//        public void TestClass()
//        {
//            var a = new Class()
//            {
//                ID = 10
//            };
//
//            var b = new Class()
//            {
//                ID = 5
//            };
//
//            Assert.That(a.CompareTo(b) > 0);
//            Assert.False(a.Equals(b));
//        }
    }
}
