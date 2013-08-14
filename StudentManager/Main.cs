using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Specialized;
using System.Collections;

namespace StudentManager
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
                }
            }
        }
        
        public void Stop()
        {
            this.running = false;
        }
    }
    
    class MainClass : ChoiceScreen
    {
        
        public MainClass()
        {
            AddChoice("1", "Create new class", CreateNewClass);
            
            AddChoice("0", "Quit", Stop);
        }
        
        public void CreateNewClass()
        {
        }
        
        public static void Main(string[] args)
        {
            new MainClass().Start();
        }
    }
}
