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
using System.Collections.Specialized;
using System.Collections;

namespace StudentManager.TextUi
{
    abstract class ChoiceScreen
    {
        protected OrderedDictionary commands;
        protected bool running = false;
        protected event Action PreHook;
        protected event Action PostHook;

        public String Message { get; set; }
        
        public ChoiceScreen()
        {
            commands = new OrderedDictionary();
        }
        
        public void AddChoice(String key, String label, Action action)
        {
            commands.Add(key, new Tuple<String, Action>(label, action));
        }
        
        public void Start()
        {
            this.running = true;
            while (this.running)
            {
                if (PreHook != null)
                    PreHook();
                foreach (DictionaryEntry de in commands)
                {
                    Console.WriteLine(
                        String.Format("{0} - {1}",
                                  de.Key,
                                  (de.Value as Tuple<String, Action>).Item1));
                }
                
                String choice = System.Console.ReadLine();
                if (commands.Contains(choice))
                {
                    (commands[choice] as Tuple<String, Action>).Item2.Invoke();
                    if (PostHook != null)
                        PostHook();
                } else
                {
                    Console.WriteLine("No such choice!");
                }
            }
        }
        
        public virtual void Stop()
        {
            this.running = false;
        }
    }

    class MainScreen : ChoiceScreen
    {
        Manager manager;

        public MainScreen(Manager manager)
        {
            this.manager = manager;

            AddChoice("1", "Manage classes", ManageClasses);

            AddChoice("q", "Quit", Stop);
        }

        public void ManageClasses()
        {
            new ClassScreen(this.manager).Start();
        }

        public override void Stop()
        {
            base.Stop();
        }
    }

    class ClassScreen : ChoiceScreen
    {
        Manager manager;

        public ClassScreen(Manager manager)
        {
            this.manager = manager;

            AddChoice("1", "List classes", ListClasses);
            AddChoice("2", "Manage timetable", ManageTimetable);
            AddChoice("b", "Back", Stop);
        }

        public void ListClasses()
        {
            foreach (var cl in manager.Classes)
            {
                Console.WriteLine(String.Format("{0} {1} {2}", cl.ID, cl.Name, cl.Teacher));
            }

            Class c = null;
            do
            {
                Console.Write("Please select a class ID: ");
                String id = System.Console.ReadLine();
                c = manager.GetClassById(id);
            } while (c == null);
        }

        public void ManageTimetable()
        {

        }
    }
}

