//  
//  Identity.cs
//  
//  Author:
//       chin <${AuthorEmail}>
// 
//  Copyright (c) 2013 chin
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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace StudentManager
{
    /// <summary>
    /// Base class for identities.
    /// </summary>
    /// <remarks>
    /// The single purpose for this class' existence is to provide
    /// simple and uniform ID and equality check. Inheriting classes
    /// are expected to have a single public property decorated with
    /// the [ID] attribute, which will be used in equality comparisions.
    /// If there are multiple properties with ID attribute, only the
    /// first one will be used.
    /// </remarks>
    public abstract class Identity : IEquatable<Identity>
    {
        [AttributeUsage(System.AttributeTargets.Property)]
        public class IDAttribute : System.Attribute
        {
        }

        MethodInfo IdGetMethod;

        public Identity()
        {
            var idProperty = (from prop in this.GetType().GetProperties()
                     from attr in prop.GetCustomAttributes(false)
                     where attr is IDAttribute
                     select prop).SingleOrDefault();

            // Not null, readable and not an indexer
            if (idProperty != null &&
                idProperty.CanRead &&
                idProperty.GetIndexParameters().Length == 0)
            {
                IdGetMethod = idProperty.GetGetMethod();
            }
        }

        public object GetId()
        {
            return this.IdGetMethod.Invoke(this, null);
        }

        public static Identity GetObjectById(IEnumerable<Identity> collection,
                                             object id)
        {
            return (from obj in collection
                    where obj.GetId().Equals(id)
                    select obj).SingleOrDefault();
        }

        #region IEquatable[Identity] implementation
        public bool Equals(Identity other)
        {
            if (this.IdGetMethod != null)
            {
                object otherId = other.GetId();
                object thisId = this.GetId();
                return thisId.Equals(otherId);
            } else
            {
                return base.Equals(other);
            }
        }

        public override bool Equals(object obj)
        {
            var other = obj as Identity;
            return other != null && this.Equals(other);
        }

        public override int GetHashCode()
        {
            if (this.IdGetMethod != null)
                return this.GetId().GetHashCode();
            else
                return base.GetHashCode();
        }
        #endregion
    }
}

