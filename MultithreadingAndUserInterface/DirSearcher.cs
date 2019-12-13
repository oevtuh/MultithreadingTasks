using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultithreadingAndUserInterface
{
    public class DirSearcher
    {
        private CancellationToken ct;
        private CancellationTokenSource tokenSource;
        public DirSearcher(CancellationTokenSource tokenSourceParam)
        {
            tokenSource = tokenSourceParam;
            ct = tokenSource.Token;
        }
        public void StartSearch(string path)
        {
            var task = Task.Factory.StartNew(() =>
            {
                // Were we already canceled?
                ct.ThrowIfCancellationRequested();

                try
                {
                    DirSearch(path);
                }
                catch (System.Exception excpt)
                {
                    Console.WriteLine(excpt.Message);
                }
            }, tokenSource.Token);
        }
        private void DirSearch(string ditPath)
        {
            Thread.Sleep(1000);
            // Were we already canceled?
            ct.ThrowIfCancellationRequested();

            foreach (string d in Directory.GetDirectories(ditPath))
            {
                foreach (string f in Directory.GetFiles(d))
                {
                    if (ct.IsCancellationRequested)
                    {

                        // Clean up here, then...
                        ct.ThrowIfCancellationRequested();
                    }
                    Console.WriteLine(f);
                }
                DirSearch(d);
            }
        }
    }
}
