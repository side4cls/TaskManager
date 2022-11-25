using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net;


class Program
{
    static void Main()
    {
        List<string> processesList = ProcessesList();

        foreach (var process in processesList)
        {
            Console.WriteLine(process);
        }
        string command = Console.ReadLine();
        if ((command == "/Refresh") || (command == "/rf") || (command == "/RF"))
        {
            Refresh();
        }
        else if ((command == "/Delete") || (command == "/Del") || (command == "/del"))
        {

        }
        else if ((command == "/List") || (command == "/LS") || (command == "/ls"))
        {
            HelperList();
        }
        Console.ReadLine();
    }

    /// <summary>
    /// Список процессов
    /// </summary>
    /// <returns></returns>
    static List<string> ProcessesList()
    {
        List<string> list = new List<string>();

        Process[] processes = Process.GetProcesses();

        foreach (var process in processes)
        {
            list.Add(process.Id + "\t" + process.ProcessName);
        }

        return list;
    }

    static void DeleteTask(Process process)
    {
        string userText = Console.ReadLine();
        process.Kill();

    }
    static void Refresh()
    {
        List<string> processesList = ProcessesList();
        foreach (var process in processesList)
        {
            Console.WriteLine(process);
        }
    }

    static void HelperList()
    {
        Console.WriteLine("/Refresh /rf /RF) - Обновить таблицу процессов");
        Console.WriteLine("/Delete /Del /del- Обновить таблицу процессов");
        Console.WriteLine("/List /LS /ls - Обновить таблицу процессов");
    }
}