//  
//  TextInterface.cs
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
using System.Linq;
using System.Collections.Specialized;
using System.Collections;

namespace StudentManager.TextUi
{
    abstract class ChoiceScreen
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
                
                String choice = System.Console.ReadLine();
                // FIXME This implementation does not allow key aliases
                //       i.e. using both "b"(ack) and "q"(uit) for the
                //       quit action on the main screen.
                if (commands.Contains(choice))
                {
                    if (PreActionHook != null)
                        PreActionHook();

                    (commands[choice] as Tuple<String, Action>).Item2.Invoke();

                    if (PostActionHook != null)
                        PostActionHook();
                } else
                {
                    Console.WriteLine("No such choice!");
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

        public MainScreen(Manager manager): base(null)
        {
            this.manager = manager;

            AddChoice("1", "Manage classes", ManageClasses);
            AddChoice("2", "Manage timetable", ManageTimetable);
            AddChoice("q", "Quit", Quit);
        }

        public void ManageClasses()
        {
            new ClassScreen(this.manager, this).Start();
        }

        public void ManageTimetable()
        {

        }

        public override void Stop()
        {
            base.Stop();
        }
    }

    class ClassScreen : ChoiceScreen
    {
        Manager manager;

        public ClassScreen(Manager manager, ChoiceScreen parent):
            base(parent)
        {
            this.manager = manager;

            AddChoice("1", "Add a class", AddClass);
            AddChoice("2", "Select a class to edit", SelectClass);
            AddChoice("b", "Back", Stop);
            AddChoice("q", "Quit", Quit);

            this.PreMenuHook += () => {
                foreach (var cl in manager.Classes)
                {
                    Console.WriteLine(String.Format("{0} {1}", cl.Name, cl.Teacher));
                }
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

            manager.Classes.Add(c);
        }

        class EachClassScreen : ChoiceScreen
        {
            Class c;
            ClassScreen parent;

            public EachClassScreen(Class c, ClassScreen parent):
                base(parent)
            {
                this.c = c;
                this.parent = parent;

                AddChoice("1", "Select student ID", SelectStudent);
                AddChoice("2", "Remove class", RemoveClass);
                AddChoice("3", "Change class info", ChangeClassInfo);
                AddChoice("b", "Back", Stop);
                AddChoice("q", "Quit", Quit);

                this.PreMenuHook += () => {
                    var studentList =
                        (from tuple in this.parent.manager.ClassStudents
                         where tuple.Item1.Equals(this.c)
                         select tuple.Item2);

                    foreach (Student student in studentList)
                    {
                        Console.WriteLine(String.Format("{0} {1}", student.ID, student.Name));
                    }
                };
            }

            void SelectStudent()
            {
            }

            void RemoveClass()
            {
            }

            void ChangeClassInfo() {}
        }

        public void SelectClass()
        {
            Console.Write("Select a class ID: ");
            String name = Console.ReadLine();
            Class c = manager.GetClassByName(name);
            new EachClassScreen(c, this).Start();
        }
    }
}

