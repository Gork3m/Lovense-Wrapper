using System;
using LovenseWrapper;
using LovenseWrapper.API;
using LovenseWrapper.Debugging;


namespace TestCLI {
    class Program {
        static void Main(string[] args) {
            Debug.Log("Starting..", ConsoleColor.DarkYellow);
            Debug.Log("Enter access code: ");
            string code = Console.ReadLine();
            LovenseSession session = new LovenseSession(code);
            if (session.Connect()) {
                Debug.Log("Connected", ConsoleColor.Green);
            } else {
                Debug.Log("ERROR: " + session.error);
                Console.WriteLine("Press any key to exit..");
                return;
            }

            Debug.Log("press any key to vibrate", ConsoleColor.Green);
            Console.ReadKey();
            for (int i2 = 0; i2 < 5; i2++) {
                for (int i = 0; i < 20; i++) {
                    session.toy.Vibrate(i);
                    System.Threading.Tasks.Task.Delay(100);
                }
                for (int i = 20; i > 1; i--) {
                    session.toy.Vibrate(i);
                    System.Threading.Tasks.Task.Delay(100);
                }
            }
            Debug.Log("press any key to stop vibrating", ConsoleColor.Cyan);
            Console.ReadLine();
            session.toy.Vibrate(0);
            
            

            Console.ReadLine();
        }
    }
}
