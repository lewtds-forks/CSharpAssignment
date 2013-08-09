//
//  XMLDatabase.cs
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
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//
using System;
using System.IO;
using System.Xml.Serialization;

namespace StudentManager
{
    public class XMLDatabase
    {
        public void save<T>(String path, T data)
        {
            var serializer = new XmlSerializer(typeof(T));
            TextWriter textWriter = new StreamWriter(path);
            serializer.Serialize(textWriter, data);
            textWriter.Close();
        }
    }
}

