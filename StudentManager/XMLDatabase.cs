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
    public interface IPersistenceService
    {
        void save<T>(String uri, T data);
        T load<T>(String uri);
    }

    public class XMLDatabase : IPersistenceService
    {
        public void save<T>(String uri, T data)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var textWriter = new StreamWriter(uri)) {
                serializer.Serialize(textWriter, data);
            }
        }
        
        public T load<T>(String uri)
        {
            T data;
            var serializer = new XmlSerializer(typeof(T));
            using (var textReader = new StreamReader(uri)) {
                 data = (T) serializer.Deserialize(textReader);
            }
            return data;
        }
    }
}

