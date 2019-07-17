using Paywall.Automatization;
using Paywall.SkypeAutomation.Core.SkypeApi;
using Paywall.SkypeAutomation.Core.SkypeProcess;
using Paywall.SkypeAutomation.Core.Task;
using Paywall.SkypeAutomation.Implementations.Events;
using Paywall.SkypeAutomation.Implementations.SkypeApi;
using Paywall.SkypeAutomation.Implementations.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Paywall.SkypeAutomation
{
  class Program
  {
    public static DateTime ApplicationStart;
    public static int Delay = 500;
    public static SkypeProcessManager SkypeProcess;
    public static AutomatizationManagerBase AutomatizationManager;
    public static Managers Managers = null;
    public static int NumberOfThread = 2;

    // SUMMARY: Main function and thread for communication tasks
    static void Main(string[] args)
    {
      ApplicationStart = DateTime.Now;
      SkypeProcess = new SkypeProcessManager();
      Managers = new Managers();

      new Thread(new ThreadStart(CommunicationThread)).Start();
      new Thread(new ThreadStart(DatabaseThread)).Start();
      Console.WriteLine(DateTime.Now.ToString());
      Console.WriteLine("");

    }

    // SUMMARY: Communication thread for managing skype
    static void CommunicationThread()
    {
      Program.Managers.Add(new EventCommunicationManager(), ManagerThreadType.CommunicationThread);
      Program.Managers.Add(new SkypeConversionManager(), ManagerThreadType.CommunicationThread);
      Program.Managers.Add(new CommunicationTaskManager(), ManagerThreadType.CommunicationThread);
      Program.Managers.ThreadReady();
      Program.Green(" - Communicaton thread started ");

      for (; ; )
      {
        DateTime executionTime = DateTime.Now;
        Program.Managers.Call(executionTime, ManagerThreadType.CommunicationThread);

        Thread.Sleep(Program.Delay);
      }
    }

    // SUMMARY: Thread for database tasks
    static void DatabaseThread()
    {
      Program.Managers.Add(new DatabaseTaskManager(), ManagerThreadType.DatabaseTask);
      Program.Managers.ThreadReady();
      Program.Green(" - Databse thread start ");

      for(;;)
      {
        DateTime executionTime = DateTime.Now;
        Program.Managers.Call(executionTime, ManagerThreadType.DatabaseTask);

        Thread.Sleep(Program.Delay);
      }
    }


    // SUMMARY: Console write info
    public static void Blue(string message)
    {
      Console.ForegroundColor = ConsoleColor.Blue;
      Console.WriteLine(message);
      Console.ResetColor();
    }
    public static void Yellow(string message)
    {
      Console.ForegroundColor = ConsoleColor.Yellow;
      Console.WriteLine(message);
      Console.ResetColor();
    }
    public static void Green(string message)
    {
      Console.ForegroundColor = ConsoleColor.Green;
      Console.WriteLine(message);
      Console.ResetColor();
    }
    public static void Error(string message)
    {
      Console.ForegroundColor = ConsoleColor.Red;
      Console.WriteLine("ERROR!!! - " + message);
      Console.ResetColor();
    }
  }
}
