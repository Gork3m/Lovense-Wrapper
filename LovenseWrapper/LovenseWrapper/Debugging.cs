using System;

namespace LovenseWrapper.Debugging {
    public static class Debug {
        public static void Log(string msg, ConsoleColor color = ConsoleColor.DarkGray) {

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("[" + DateTime.Now + "] ");
            Console.ForegroundColor = color;
            Console.WriteLine(msg);
            Console.ResetColor();

        }


    }
}