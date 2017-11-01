using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MyBenchmark
{
    class Program
    {
        private const int size1 = 10000, size2 = 20000;
        static void Main(string[] args)
        {
            TesterCore.RunAllTests(typeof(Program).Assembly);
            Console.ReadLine();
        }

        [MyTest(500)]
        [Params(size1, size2)]
        public static void TestStatic(int ArraySize)
        {
            var sha256 = SHA256.Create();  
            var data = new byte[ArraySize];

            new Random().NextBytes(data);

            byte[] Sha256 = sha256.ComputeHash(data);
        }

        [MyTest(500)]
        [Params(size1, size2)]
        public void TestNonStatic(int ArraySize)
        {
            var sha256 = SHA256.Create();  
            var data = new byte[ArraySize];

            new Random().NextBytes(data);

            byte[] Sha256 = sha256.ComputeHash(data);
        }

        [MyTest(500)]
        public static void TestStaticNoParams()
        {
            var sha256 = SHA256.Create();
            var data = new byte[size1];

            new Random().NextBytes(data);

            byte[] Sha256 = sha256.ComputeHash(data);
        }
        [MyTest(500)]
        public void TestNonStaticNoParams()
        {
            var sha256 = SHA256.Create();
            var data = new byte[size1];

            new Random().NextBytes(data);

            byte[] Sha256 = sha256.ComputeHash(data);
        }

        [MyTest(500)]
        [Params(size1, size2)]
        public void NoParametersWarning()
        {
            var sha256 = SHA256.Create();
            var data = new byte[size1];

            new Random().NextBytes(data);

            byte[] Sha256 = sha256.ComputeHash(data);
        }

        [MyTest(500)]
        public void NoAttributeError(int ArraySize)
        {
            var sha256 = SHA256.Create();
            var data = new byte[ArraySize];

            new Random().NextBytes(data);

            byte[] Sha256 = sha256.ComputeHash(data);
        }
    }
}
