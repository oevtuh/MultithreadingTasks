using System;
using System.Threading;
using System.Threading.Tasks;
using MultithreadingAndUserInterface;

class Program
{
    static void Main()
    {
        var  tokenSource = new CancellationTokenSource();
        var searcher = new DirSearcher(tokenSource);
        searcher.StartSearch("C:\\DA_projects");

        char ch = Console.ReadKey().KeyChar;
        if (ch == 'c' || ch == 'C') {
            tokenSource.Cancel();
            Console.WriteLine("\nTask cancellation requested.");
        }

        Console.ReadKey();
    }
}