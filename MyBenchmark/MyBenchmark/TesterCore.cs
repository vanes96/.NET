using System;
using System.Data;
using System.Diagnostics;
using System.Reflection;

namespace MyBenchmark
{
    public class TesterCore
    {
        public static void RunAllTests(Assembly asm)
        {
            foreach (var type in asm.GetTypes())
            {
                foreach (var method in type.GetMethods())
                {
                    var myTestAttrib = method.GetCustomAttribute<MyTestAttribute>();
 
                    if (myTestAttrib != null)
                         RunTest(method);
                }
            }

            Console.WriteLine("All tests have done.");
        }



        private static void RunTest(MethodInfo method)
        {
            var myTestAttrib = method.GetCustomAttribute<MyTestAttribute>();
            var paramsAttrib = method.GetCustomAttribute<ParamsAttribute>();
            var parameters = method.GetParameters();

            var timer = new Stopwatch();              
            //Console.Write("Begin {0}...", method.Name);

            if (paramsAttrib == null)
            {
                if (parameters.Length > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Ошибка: у метода " + '"' + method.Name + '"' + " есть параметры, но нет атрибута Params!!!");
                    Console.ResetColor();
                    return;
                }
                else
                {
                    timer.Restart();
                    InvokeNoAttributes(method, myTestAttrib);
                    timer.Stop();
                    WriteToConsole(method, myTestAttrib, timer, null);
                }
            }
            else
            {
                if (parameters.Length > 0)
                foreach (var param in paramsAttrib.Params)
                {
                    timer.Restart(); 
                    if (method.IsStatic)
                        for (var i = 0; i < myTestAttrib.TestCount; i++)
                            method.Invoke(null, new[] { param });
                    else
                    {
                        var obj = GetObject(method);
                        for (var i = 0; i < myTestAttrib.TestCount; i++)
                            method.Invoke(obj, new[] { param });
                    }
                    timer.Stop();
                    WriteToConsole(method, myTestAttrib, timer, param);
                }
                else
                {
                    timer.Restart();
                    InvokeNoAttributes(method, myTestAttrib);
                    timer.Stop();
                    var avgTime = timer.ElapsedMilliseconds / (double)myTestAttrib.TestCount;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Предупреждение: у метода " + '"' + method.Name + '"' + " есть атрибут Params, но нет параметров!");
                    Console.WriteLine("{0}:\tavgTime = {1} ms = {2} ms / {3}", method.Name, avgTime, timer.ElapsedMilliseconds, myTestAttrib.TestCount);
                    Console.ResetColor();
                }
            }
        }
        private static void WriteToConsole(MethodInfo method, MyTestAttribute myTestAttrib, Stopwatch timer, object param)
        {
            var avgTime = timer.ElapsedMilliseconds / (double)myTestAttrib.TestCount;
            Console.WriteLine("{0}({1}):\tavgTime = {2} ms = {3} ms / {4}", method.Name, (param != null)?param.ToString():"", avgTime, timer.ElapsedMilliseconds, myTestAttrib.TestCount);
        }

        private static object GetObject(MethodInfo method)
        {
            var type = Type.GetType(method.DeclaringType.FullName);
            var Constructor = type.GetConstructor(Type.EmptyTypes);
            var ClassObject = Constructor.Invoke(new object[] { });
            return ClassObject;
        }

        private static void InvokeNoAttributes(MethodInfo method, MyTestAttribute myTestAttrib)
        {
            if (method.IsStatic)
                for (var i = 0; i < myTestAttrib.TestCount; i++)
                    method.Invoke(null, null);
            else
                for (var i = 0; i < myTestAttrib.TestCount; i++)
                    method.Invoke(GetObject(method), null);
        }
    }
}