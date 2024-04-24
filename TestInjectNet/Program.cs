using System;
using System.Runtime.Remoting;
using System.Threading;

using EasyHook;

using ToInject;

namespace TestInjectNet;

internal static class Program
{
    static void Main(string[] args)
    {
        string str;
        if (args.Length > 0)
            str = args[0];
        else
        {
            Console.WriteLine("Id du process : ");
            str = Console.ReadLine();
        }
        int idProcessus = int.Parse(str);
        string channelName = null;
        RemoteHooking.IpcCreateServer<InterfaceEchange>(ref channelName, WellKnownObjectMode.Singleton);
        RemoteHooking.Inject(idProcessus,
            InjectionOptions.Default | InjectionOptions.SameAppDoman,
            typeof(MaClasse).Assembly.Location,
            typeof(MaClasse).Assembly.Location,
            channelName);

        while (true)
        {
            if (Console.KeyAvailable)
                break;
            Thread.Sleep(100);
        }
    }
}
