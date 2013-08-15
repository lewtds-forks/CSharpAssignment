//
//  TestPersistence.cs
//
//  Author:
//       Trung Ngo <ndtrung4419@gmail.com>
//
//  Copyright (c) 2013 Trung Ngo
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace StudentManager
{
    [TestFixture]
    public class TestPersistence
    {
        XMLDatabase xml = new XMLDatabase();

        [Test]
        public void SaveSingleObject()
        {
            var s = new Student()
            {
                ID = "B01414",
                Name = "Trung",
                Address = "Home"
            };

            xml.save<Student>("student.xml", s);
            Assert.AreEqual(
                @"<?xml version=""1.0"" encoding=""utf-8""?>
<Student xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <ID>B01414</ID>
  <Name>Trung</Name>
  <Address>Home</Address>
</Student>",
                System.IO.File.ReadAllText("student.xml")
            );
        }

        [Test]
        public void SaveHashSet()
        {
            var s = new HashSet<Class>();
            s.Add(new Class() {
                Name = "C1203L",
                Teacher = "NhatNK"
            });
            xml.save<HashSet<Class>>("classes.xml", s);
        }
        
        [Test]
        public void LoadIdentity() {
            var s = new Student()
            {
                ID = "B01414",
                Name = "Trung",
                Address = "Home"
            };
            
            xml.save<Student>("student.xml", s);
            var a = xml.load<Student>("student.xml");
            Assert.AreEqual(s, a);
        }
        
        [Test]
        public void LoadHashSet()
        {
            var expected = new HashSet<Class>();
            expected.Add(new Class() {
                Name = "C1203L",
                Teacher = "NhatNK"
            });
            xml.save<HashSet<Class>>("classes.xml", expected);
            var got = xml.load<HashSet<Class>>("classes.xml");
            Assert.AreEqual(expected, got);
        }
    }
}
