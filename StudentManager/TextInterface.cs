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
                } else
                {
                    Console.WriteLine("No such choice!");
                }
            }
        }
        
        public void Stop()
        {
            this.running = false;
        }
    }

    class MainScreen : ChoiceScreen
    {
        public MainScreen()
        {
            AddChoice("1", "Create new class", CreateNewClass);

            AddChoice("q", "Quit", Stop);
        }
        
        public void CreateNewClass()
        {
        }
    }
}

