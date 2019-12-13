using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace ProducerConsumer
{
    class IOTaskDispatcher : BaseDispatcher
    {
        List<Thread> _threads = new List<Thread>();
        BlockingCollection<object> _collection = new BlockingCollection<object>();
        /// <summary>
        /// Called before recieving messages, can execute for any time
        /// </summary>
        public override void Initialize()
        {
            var cores = new System.Management.ManagementObjectSearcher("Select * from Win32_Processor").Get().Count;

            for (int i = 0; i < cores; i++)
            {
                _threads.Add(new Thread(ProcessMessage));
                _threads[i].Start();
            }
        }
        
        private void ProcessMessage()
        {
            var messages = _collection.GetConsumingEnumerable();

            foreach (var message in messages)
            {
                Env.Process(message);
            }
        }

        /// <summary>
        /// This code is executed everytime message is received
        /// This method can't execute too long, so actual processing
        /// should take place somewhere else
        /// </summary>
        /// <param name="msg">Call Env.Process for this object</param>
        public override void OnReceived(object msg)
        {
            //_collection.Add(msg);
            new Thread(() => { Env.Process(msg); }).Start();
        }
    }

    class CPUTaskDispatcher : BaseDispatcher
    {
        List<Thread> _threads = new List<Thread>();
        BlockingCollection<object> _collection = new BlockingCollection<object>();

        /// <summary>
        /// Called before recieving messages, can execute for any time
        /// </summary>
        public override void Initialize()
        {
            var cores = new System.Management.ManagementObjectSearcher("Select * from Win32_Processor").Get().Count;

            for (int i = 0; i < cores; i++)
            {
                _threads.Add(new Thread(ProcessMessage));
                _threads[i].Start();
            }
        }

        private void ProcessMessage()
        {
            var messages = _collection.GetConsumingEnumerable();

            foreach (var message in messages)
            {
                Env.Process(message);
            }
        }

        /// <summary>
        /// This code is executed everytime message is received
        /// This method can't execute too long, so actual processing
        /// should take place somewhere else
        /// </summary>
        /// <param name="msg">Call Env.Process for this object</param>
        public override void OnReceived(object msg)
        {
            //_collection.Add(msg);
            new Thread(() => { Env.Process(msg); }).Start();
        }
    }

    class ThreadPoolIOTaskDispatcher : BaseDispatcher
    {
        /// <summary>
        /// Called before recieving messages, can execute for any time
        /// </summary>
        public override void Initialize()
        {
           
        }

        private void ProcessMessage()
        {
            
        }

        /// <summary>
        /// This code is executed everytime message is received
        /// This method can't execute too long, so actual processing
        /// should take place somewhere else
        /// </summary>
        /// <param name="msg">Call Env.Process for this object</param>
        public override void OnReceived(object msg)
        {
            ThreadPool.QueueUserWorkItem(Env.Process, msg);
        }
    }

    class ThreadPoolCPUTaskDispatcher : BaseDispatcher
    {
       
        /// <summary>
        /// Called before recieving messages, can execute for any time
        /// </summary>
        public override void Initialize()
        {
           
        }

        /// <summary>
        /// This code is executed everytime message is received
        /// This method can't execute too long, so actual processing
        /// should take place somewhere else
        /// </summary>
        /// <param name="msg">Call Env.Process for this object</param>
        public override void OnReceived(object msg)
        {
            ThreadPool.QueueUserWorkItem(Env.Process, msg);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            ConcurrentEnv env = new ConcurrentEnv();
            //env.Start<IOTaskDispatcher, IOProcessor>(5);                  //5005
            //env.Start<ThreadPoolIOTaskDispatcher, IOProcessor>(5);        //1004
            //Console.ReadLine();

            //env.Start<IOTaskDispatcher, IOProcessor>(10);                 //10009
            //env.Start<ThreadPoolIOTaskDispatcher, IOProcessor>(10);       //2004
            //Console.ReadLine();

            //env.Start<IOTaskDispatcher, IOProcessor>(100);                //100091 
            //env.Start<ThreadPoolIOTaskDispatcher, IOProcessor>(100);      //10038
            //Console.ReadLine();

            //env.Start<CPUTaskDispatcher, CPUProcessor>(5);                //7668
            //env.Start<ThreadPoolCPUTaskDispatcher, CPUProcessor>(5);      //2148
            //Console.ReadLine();

            //env.Start<CPUTaskDispatcher, CPUProcessor>(10);               //13768
            //env.Start<ThreadPoolCPUTaskDispatcher, CPUProcessor>(10);     //3870
            //Console.ReadLine();

            


            env.Start<CPUTaskDispatcher, CPUProcessor>(10000);              //136304
            //env.Start<ThreadPoolCPUTaskDispatcher, CPUProcessor>(100);    //33909
            Console.ReadLine();
        }
    }
}
