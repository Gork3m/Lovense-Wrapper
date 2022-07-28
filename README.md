# LovenseWrapper
A c# wrapper to control your toys remotely, using access codes.
You can embed this into your own projects, like:

* Discord bot to vibrate you when someone joins a server (LOL)
* A roblox script to vibrate you when you die in a specific game
* etc..

# Usage

Initialize a new session:
```cs
LovenseSession session = new LovenseSession("access code"); //initialize with a 6 char long access code
if (!session.Connect()) {
   Console.WriteLine("ERROR: " + session.error);
   Console.WriteLine("Press any key to exit..");
   return;
}
```

Once connected, you can control the toys remotely.

```cs
session.toy.Vibrate(20); // 20 = max power

// you can control multiple toys as well!
session.toys[2].Vibrate(12);

// and stop vibrating by doing
session.toy.Vibrate(0);
```

When you're done, *(well, actually the person you're controlling)*, you can disconnect safely:

```cs
session.Disconnect();
```