//  
//  Utility.cs
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


namespace StudentManager.Utility
{
    using StudentManager.TextUi;
    public static class StringExtension
    {
        public static string PadCenter(this string str, int width)
        {
            return str.PadRight(width * 2 / 3).PadLeft(width);
        }
    }

    public static class ChoiceScreenExtension
    {
        public static bool Confirm(this ChoiceScreen scrn,
                                   string message,
                                   string yes = "Y",
                                   string no = "N")
        {
            while (true)
            {
                EmphasizeWriteLine(scrn,
                    String.Format("{0} ({1}/{2})", message, yes, no),
                    bg: ConsoleColor.Red);
                var choice = Console.ReadLine();
                if (yes.ToLower().Equals(choice.ToLower()))
                {
                    return true;
                }
                else if (no.ToLower().Equals(choice.ToLower()))
                {
                    return false;
                }
                ErrorWriteLine(scrn, "No such choice!");
            }
        }

        public static void EmphasizeWrite(this ChoiceScreen scrn,
                                          string message,
                                          ConsoleColor fg=ConsoleColor.White,
                                          ConsoleColor bg=ConsoleColor.Blue)
        {
            Console.BackgroundColor = bg;
            Console.ForegroundColor = fg;
            Console.Write(message);
            Console.ResetColor();
        }

        public static void EmphasizeWriteLine(this ChoiceScreen scrn,
                                              string message,
                                              ConsoleColor fg=ConsoleColor.White,
                                              ConsoleColor bg=ConsoleColor.Blue)
        {
            EmphasizeWrite(scrn, message + '\n', fg, bg);
        }

        public static void ErrorWriteLine(this ChoiceScreen scrn, string message)
        {
            EmphasizeWriteLine(scrn, message, bg: ConsoleColor.Red);
        }
    }
}

