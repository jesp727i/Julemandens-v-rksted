// using Jesper.Er.Dejlig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Julemandens_verksted
{
    class Program
    {
        Semaphore santaSem = new Semaphore(0,1);
        Semaphore reindeerSem = new Semaphore(9, 9);
        Semaphore elfSem = new Semaphore(3, 3);
        Mutex m = new Mutex();
        Mutex elfMutex = new Mutex();
        bool home = true;
        int elfCount = 0;
        int reindeerCount = 0;
        Random rand = new Random();
        Thread T;
        //object o = new object();

        static void Main(string[] args)
        {
            Program p = new Program();
            p.Run();
        }

        void Run()
        {
            int ran;
            Thread SantaC = new Thread(santa);
            SantaC.Start();
            while (home)
            {
                
                ran = rand.Next(1, 3);
                if (ran == 1)
                {
                    T = new Thread(reindeer);
                    Console.WriteLine("spawn reindeer");
                }
                else
                {
                    T = new Thread(Elf);
                    Console.WriteLine("spawn elf");
                }
                T.Start();
                Thread.Sleep(1000);
            }
        }

        void santa()
        {
            while (true) {
                santaSem.WaitOne();
                Console.WriteLine("santa løsladt");
                m.WaitOne();
                if (reindeerCount == 9)
                {
                    reindeerCount = 0;
                    prepSleigh();
                    reindeerSem.Release(9);
                    m.ReleaseMutex();
                }
                else
                {
                    elfSem.Release(3);
                    helpElves();
                }
            }
        }

        void reindeer()
        {
            reindeerSem.WaitOne();
            m.WaitOne();
            
            reindeerCount++;
            if (reindeerCount == 9) santaSem.Release();

            Console.WriteLine("r " + reindeerCount);
            m.ReleaseMutex();

            //reindeerSem.WaitOne();
            getHitched();
        }

        void Elf()
         {
            
            elfMutex.WaitOne();
            m.WaitOne();
            elfSem.WaitOne();

            elfCount++;
            Console.WriteLine("e " + elfCount);

            if (elfCount == 3)
            {
                Console.WriteLine("sæt sante fri");
                santaSem.Release();
                elfMutex.ReleaseMutex();
            }
            else elfMutex.ReleaseMutex();

            m.ReleaseMutex();
            //elfSem.WaitOne();

           

        }

        private void getHelp()
        {
            Thread.Sleep(1000);
        }

        private void getHitched()
        {
            Thread.Sleep(1000);
        }

        void prepSleigh()
        {
            Console.WriteLine("Forbereder Slæden");
            Thread.Sleep(1000);
        }

        void helpElves()
        {

            Console.WriteLine("elver får hjælp");
            getHelp();
            elfCount--;
            if (elfCount == 0) m.ReleaseMutex();
            else helpElves();
        }
    }
}
