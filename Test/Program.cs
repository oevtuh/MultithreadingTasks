using System;
using System.Threading;

namespace ConsoleApplication1
{
    class MyThread
    {
        private object lockObject;
        public Thread Thrd;
        AutoResetEvent mre;

        public MyThread(string name, AutoResetEvent evt, object lockObj)
        {
            lockObject = lockObj;
            Thrd = new Thread(this.Run);
            Thrd.Name = name;
            mre = evt;
            Thrd.Start();
        }

        void Run()
        {
            Console.WriteLine("Внутри потока " + Thrd.Name);

            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine(Thrd.Name);
                Thread.Sleep(500);
            }

            Console.WriteLine(Thrd.Name + " завершен!");

            // Уведомление о событии
            //mre.Set();
            lock (lockObject)
            {
                Monitor.PulseAll(lockObject);
            }
        }
    }

    class MyThreadLeft
    {
        private object lockObject;
        public Thread Thrd;
        AutoResetEvent mre;

        public MyThreadLeft(string name, AutoResetEvent evt, object lockObj)
        {
            lockObject = lockObj;
            Thrd = new Thread(this.Run);
            Thrd.Name = name;
            mre = evt;
            Thrd.Start();
        }

        void Run()
        {
            Console.WriteLine("Внутри потока " + Thrd.Name);

            lock (lockObject)
            {
                Monitor.Wait(lockObject);
            }
            //mre.WaitOne();

            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine(Thrd.Name);
                Thread.Sleep(500);
            }

            Console.WriteLine(Thrd.Name + " завершен!");

            // Уведомление о событии
            mre.Set();
            lock (lockObject)
            {
                Monitor.Pulse(lockObject);
            }
        }
    }

    class Program
    {
        static void Main()
        {
            object lockObject = new Object();
            AutoResetEvent evtObj = new AutoResetEvent(false);

            MyThread mt1 = new MyThread("Событийный поток 1", evtObj, lockObject);

            MyThreadLeft mtleft = new MyThreadLeft("Событийный поток LEFT", evtObj, lockObject);
            Console.WriteLine("Основной поток ожидает событие");

            //evtObj.WaitOne();
            lock (lockObject)
            {
                Monitor.Wait(lockObject);
            }


            Console.WriteLine("Основной поток получил уведомление о событии от первого потока");

            //evtObj.Set();
            lock (lockObject)
            {
                Monitor.Pulse(lockObject);
            }

            mt1 = new MyThread("Событийный поток 2", evtObj, lockObject);

            //evtObj.WaitOne();

            Console.WriteLine("Основной поток получил уведомление о событии от второго потока");
            Console.ReadLine();
        }
    }
}