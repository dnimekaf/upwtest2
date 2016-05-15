using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiTester.Controllers;

namespace ApiTester
{
    class Program
    {
        static void Main(string[] args)
        {
                AccountsTester.RunAsync().Wait();
            
        }
    }
}
