using System;
using System.Collections.Generic;
using System.Linq;
using StudentManager.TextUi;

namespace StudentManager
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Manager m = new Manager();
            new MainScreen(m).Start();
        }
    }
}
