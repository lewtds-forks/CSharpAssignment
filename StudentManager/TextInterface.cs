//  
//  TextInterface.cs
//  
//  Author:
//       Trung Ngo <ndtrung4419@gmail.com>
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
using System.Linq;
using System.Collections.Specialized;
using System.Collections;



namespace StudentManager.TextUi
{
    using StudentManager.Utility;
    public abstract class ChoiceScreen
    {
        private OrderedDictionary commands;
        private bool running = false;

        protected event Action PreMenuHook;
        protected event Action PreActionHook;
        protected event Action PostActionHook;

        protected ChoiceScreen Parent;

        public ChoiceScreen(ChoiceScreen parent)
        {
            this.Parent = parent;
            commands = new OrderedDictionary();
        }
        
        public void AddChoice(String key, String label, Action action)
        {
            commands.Add(key, new Tuple<String, Action>(label, action));
        }
        
        public virtual void Start()
        {
            this.running = true;
            while (this.running)
            {
                if (PreMenuHook != null)
                    PreMenuHook();
                foreach (DictionaryEntry de in commands)
                {
                    Console.WriteLine(
                        String.Format("{0} - {1}",
                                  de.Key,
                                  (de.Value as Tuple<String, Action>).Item1));
                }

                Console.Write("\nChoice: ");
                
                String choice = System.Console.ReadLine();
                // FIXME This implementation does not allow key aliases
                //       i.e. using both "b"(ack) and "q"(uit) for the
                //       quit action on the main screen.
                if (commands.Contains(choice = choice.ToUpper()) ||
                    commands.Contains(choice = choice.ToLower()))
                {
                    if (PreActionHook != null)
                        PreActionHook();

                    (commands[choice] as Tuple<String, Action>).Item2.Invoke();

                    if (PostActionHook != null)
                        PostActionHook();
                } else
                {
                    this.ErrorWriteLine("No such choice! Press any key to continue...");
                    Console.ReadKey();
                }
            }
        }

        /// <summary>
        /// Stop this instance and return to parent screen.
        /// </summary>
        public virtual void Stop()
        {
            this.running = false;
        }

        /// <summary>
        /// Stop this instance and chain all parents' Quit method.
        /// </summary>
        public void Quit()
        {
            this.Stop();
            if (Parent != null)
                Parent.Quit();
        }
    }

    class MainScreen : ChoiceScreen
    {
        Manager manager;

        public MainScreen(Manager manager):
            base(null)
        {
            this.manager = manager;

            AddChoice("1", "Add a class", AddClass);
            AddChoice("2", "Select a class to edit", SelectClass);
            AddChoice("Q", "Quit", Quit);

            this.PreMenuHook += () => {
                Console.Clear();
                int l1 = 15, l2 = 15;

                Console.WriteLine("= Student Management System =\n\n");

                this.EmphasizeWriteLine("CLASS LIST".PadCenter(l1+l2));
                this.EmphasizeWriteLine(
                        String.Format("{0, " + l1 + "}{1," + l2 + "}",
                                      "Class name", "Teacher"));

                foreach (var cl in manager.Classes)
                {
                    Console.WriteLine(
                        String.Format("{0, " + l1 + "}{1," + l2 + "}", cl.Name, cl.Teacher));
                }
                Console.WriteLine("\n----\n");
            };

            Console.CancelKeyPress += delegate {
                Console.WriteLine("\nControl - C pressed.");
                Stop();
            };
        }

        public void AddClass()
        {
            Console.Write("Class name: ");
            String name = Console.ReadLine();

            Console.Write("Teacher: ");
            String teacher = Console.ReadLine();

            var c = new Class()
            {
                Name = name,
                Teacher = teacher
            };
            if (this.Confirm("Are you sure?"))
                manager.Classes.Add(c);
        }

        public void SelectClass()
        {
            Console.Write("Select a class ID: ");
            String name = Console.ReadLine();
            var c = (Class) Identity.GetObjectById(manager.Classes, name);
            if (c != null)
            {
                new ClassScreen(c, manager, this).Start();
            }
            else
            {
                this.ErrorWriteLine("No such class!");
                Console.ReadKey();
            }

        }

        public override void Stop() {
            Console.WriteLine("Saving...");
            manager.Save();
            Console.WriteLine("Done!");
            base.Stop();
        }

    }

    class ClassScreen : ChoiceScreen
    {
        Class c;
        Manager manager;

        public ClassScreen(Class c, Manager manager, ChoiceScreen parent):
            base(parent)
        {
            this.c = c;
            this.manager = manager;

            AddChoice("1", "Select student ID", SelectStudent);
            AddChoice("2", "Add student", AddStudent);
            AddChoice("3", "Remove class", RemoveClass);
            AddChoice("4", "Change class info", ChangeInfo);
            AddChoice("B", "Back", Stop);
            AddChoice("Q", "Quit", Quit);

            this.PreMenuHook += () => {
                Console.Clear();
                Console.WriteLine("= Student Management System =\n\n");

                this.EmphasizeWriteLine("CLASS INFO");
                Console.WriteLine(String.Format
                    ("Name   : {0}\n" +
                     "Teacher: {1}\n" +
                     "", c.Name, c.Teacher));

                this.EmphasizeWriteLine("STUDENT LIST".PadCenter(30));
                this.EmphasizeWriteLine(String.Format("{0,10}{1,10}{2,10}",
                                                "ID",
                                                "Name",
                                                "Address"));

                var studentList =
                    (from tuple in this.manager.ClassStudents
                     where tuple.Item1.Equals(this.c)
                     select tuple.Item2);
                foreach (Student student in studentList)
                {
                    var addr = String
                        .IsNullOrEmpty(student.Address) ? "N/A" : student.Address;
                    Console.WriteLine(String.Format("{0,10}{1,10}{2,10}",
                                                    student.ID,
                                                    student.Name,
                                                    addr));
                }

                Console.WriteLine("\n----\n");
            };
        }

        void AddStudent()
        {
            Console.Write("Name: ");
            String name = Console.ReadLine();
            Console.Write("Student ID: ");
            String id = Console.ReadLine();
            Console.Write("Address (optional): ");
            String addr = Console.ReadLine();

            Student s = new Student() {
                ID = id,
                Name = name,
                Address = addr
            };

            if(this.Confirm("Are you sure?"))
            {
                this.manager.Students.Add(s);
                this.manager.RegisterStudentWithClass(s, c);
            }
        }

        void SelectStudent()
        {
            Console.Write("Select a student ID: ");
            String id = Console.ReadLine();
            var s = (Student) Identity
                .GetObjectById(this.manager.Students, id);
            if (s != null)
            {
                new StudentScreen(s, c, this.manager, this).Start();
            }
            else
            {
                this.ErrorWriteLine("No such student!");
                Console.ReadKey();
            }
        }

        void RemoveClass()
        {
            if(this.Confirm("Are you sure you want to remove this class?"))
            {
                this.manager.Classes.Remove(c);
                (from tuple in this.manager.ClassStudents
                 where tuple.Item1.Equals(c)
                 select new { Class = tuple.Item1, Student = tuple.Item2 })
                .Select((t) => {
                        return this.manager.RemoveStudentFromClass(t.Student, t.Class);
                    });
                Stop();
            }
        }

        void ChangeInfo() {
            Console.Write("Class name: ");
            var name = Console.ReadLine();
            Console.Write("Teacher: ");
            var teacher = Console.ReadLine();
            if(this.Confirm("Are you sure you want to change this class' info?"))
            {
                c.Name = name;
                c.Teacher = teacher;
            }
        }
    }

    class StudentScreen : ChoiceScreen
    {
        Student student;
        Class c;
        Manager manager;

        public StudentScreen(Student student,
                                 Class klass,
                                 Manager manager,
                                 ChoiceScreen parent) :
            base(parent)
        {
            this.student = student;
            this.c = klass;
            this.manager = manager;
            AddChoice("1", "Remove student", RemoveStudent);
            AddChoice("2", "Transfer student to a new class", ChangeClass);
            AddChoice("3", "Update student's info", ChangeInfo);
            AddChoice("B", "Back", Stop);
            AddChoice("Q", "Quit", Quit);

            this.PreMenuHook += () => {
                Console.Clear();

                Console.WriteLine("= Student Management System =\n\n");

                this.EmphasizeWriteLine("STUDENT INFO".PadCenter(30));
                Console.WriteLine(String.Format("ID: {0,-10}Name: {1,-10}",
                                                student.ID,
                                                student.Name));
                var addr = String.IsNullOrWhiteSpace(student.Address) ?
                    "N/A" : student.Address;
                Console.WriteLine("Address: " + addr);
                Console.WriteLine("\n----\n");
            };
        }

        public void RemoveStudent()
        {
            // Confirmation
            if (this.Confirm("Are you sure you want to remove this student?\n" +
             "You will have to create this student again if you want to" +
             "add him/her into another class. Consider using the transfer" +
             "student command instead."))
            {
                manager.Students.Remove(student);
                manager.RemoveStudentFromClass(student, c);
                Stop();
            }
        }

        public void ChangeClass()
        {
            Console.Write("Class name to transfer to: ");
            var className = Console.ReadLine();
            var otherClass = (Class) Identity
                .GetObjectById(manager.Classes, className);
            if (otherClass != null &&
                manager.SwitchClassOfStudent(student, c, otherClass)) {
                Stop();
            }
            else {
                this.ErrorWriteLine("There was an error!");
                Console.ReadKey();
            }
        }

        public void ChangeInfo()
        {
            Console.Write("Name: ");
            var name = Console.ReadLine();
            Console.Write("ID: ");
            var id = Console.ReadLine();
            Console.Write("Address (optional): ");
            var addr = Console.ReadLine();

            if (this.Confirm("Are you sure?"))
            {
                student.Name = name;
                student.ID = id;
                student.Address = addr;
            }
        }
    }
}

