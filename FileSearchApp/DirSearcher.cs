using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileSearchApp
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
        public delegate void DisplayFileName(string file);

        public event DisplayFileName SenFileName;
        public void StartSearch(string path, string term)
        {
            var task = Task.Factory.StartNew(() =>
            {
                // Were we already canceled?
                ct.ThrowIfCancellationRequested();

                try
                {
                    DirSearch(path, term);
                }
                catch (System.Exception excpt)
                {
                    Console.WriteLine(excpt.Message);
                }
            }, tokenSource.Token);
        }
        private void DirSearch(string ditPath, string term)
        {
            //Thread.Sleep(1000);
            // Were we already canceled?
            ct.ThrowIfCancellationRequested();

            string[] folders = new string[0];
            try
            {
                folders = Directory.GetDirectories(ditPath);
            }
            catch (Exception ex)
            {

            }

            foreach (string d in folders)
            {
                string[] t= new string[0];
                try
                {
                    t = Directory.GetFiles(d, String.Format("*${0}*", term));
                }
                catch (Exception ex)
                {

                }

                foreach (string f in t)
                {
                    if (ct.IsCancellationRequested)
                    {
                        // Clean up here, then...
                        ct.ThrowIfCancellationRequested();
                    }

                    SenFileName?.Invoke(f);
                }
                DirSearch(d, term);
            }
        }
    }
}
