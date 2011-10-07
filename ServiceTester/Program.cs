using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO.Pipes;
using CarDvrPipes;

namespace ServiceTester
{
    class Program
    {
        static void Main(string[] args)
        {
            PipesServer test = new PipesServer();
            test.Start();
        }
    }
}
