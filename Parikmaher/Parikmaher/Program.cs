using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Parikmaher
{
    class Program
    {
        static Queue<string> Turn;
        static Thread parikmaher;
        static Thread events;
        static Random rand = new Random();
        static bool IsSleep = false;
        static int N;
        static int percent = 20;
        static bool mutex = true;
        static int min = 4000;
        static int max = 8000;
        static int delay = 5000;

        static void Events()
        {
            bool chek = true;
            int n = 0;
            Thread vis;
            int l;
            while (chek)
            {
                l = rand.Next(100);
                if (l < percent)
                {
                    vis = new Thread(Visitor) { Name = n.ToString() };
                    n++;
                    vis.Start();
                }
                Thread.Sleep(1000);
            }
        }

        static void Visitor()
        {
            Thread.Sleep(delay);
            while (!mutex) ;
            mutex = false;
            Console.WriteLine("Visitor {0} came.", Thread.CurrentThread.Name);
            if (Turn.Count == N)
                Console.WriteLine("Visitor {0} is gone.", Thread.CurrentThread.Name);
            else
            {
                Turn.Enqueue(Thread.CurrentThread.Name);
                Console.WriteLine("Visitor {0} sat in line. Queue length {1}.", Thread.CurrentThread.Name, Turn.Count);
                if (Turn.Count == 1 && IsSleep)
                    IsSleep = false;
            }
            mutex = true;
        }

        static void Parikmaher()
        {
            bool chek = true;
            string v;
            while (chek)
            {
                while (!mutex) ;
                mutex = false;
                if (Turn.Count == 0)
                    IsSleep = true;
                mutex = true;
                if (IsSleep)
                {
                    Console.WriteLine("Hairdresser is sleep.");
                    while (IsSleep) ;
                }
                while (!mutex) ;
                mutex = false;
                v = Turn.Dequeue();
                Console.WriteLine("Hairdresser cuts visitor {0}. Queue length {1}.", v, Turn.Count);
                mutex = true;
                Thread.Sleep(rand.Next(min, max));
                Console.WriteLine("Hairdresser is finished.");
            }
        }

        static void Main()
        {
            N = Int32.Parse(Console.ReadLine());
            Turn = new Queue<string>(N);
            parikmaher = new Thread(Parikmaher);
            events = new Thread(Events);
            parikmaher.Start();
            events.Start();
        }
    }
}
