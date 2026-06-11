using System;
using System.Collections.Generic;
using System.Text;

namespace Heroes_VS_Monsters
{
    public static class ConsoleColored
    {
        public static void WriteColored(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }
    }
}
