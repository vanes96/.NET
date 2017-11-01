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
            
            var type = Type.GetType(method.DeclaringType.FullName);
            var Constructor = type.GetConstructor(Type.EmptyTypes);
            var ClassObject = Constructor.Invoke(new object[] { });

            var timer = new Stopwatch();
            timer.Restart();    // Reset + Start
            Console.Write("Begin {0}...", method.Name);

            if (paramsAttrib == null)
            {
                if (parameters.Length > 0)
                {
                    //throw new NotImplementedException("У метода имеются параметры, но отсутствует атрибут Params!");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\rОшибка: у метода " + '"' + method.Name + '"' + " есть параметры, но нет атрибута Params!!!");
                    Console.ResetColor();
                    return;
                }
                else
                    InvokeNoAttributes(method, myTestAttrib, ClassObject);
            }
            else
            {
                if (parameters.Length > 0)
                foreach (var param in paramsAttrib.Params)
                {
                    if (method.IsStatic)
                        for (var i = 0; i < myTestAttrib.TestCount; i++)
                            method.Invoke(null, new[] { param });
                    else
                        for (var i = 0; i < myTestAttrib.TestCount; i++)
                            method.Invoke(ClassObject, new[] { param });
                }
                else
                    InvokeNoAttributes(method, myTestAttrib, ClassObject);
            }

            timer.Stop();
            var avgTime = timer.ElapsedMilliseconds / (double)myTestAttrib.TestCount;
            if (paramsAttrib != null && parameters.Length == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\rПредупреждение: у метода " + '"' + method.Name + '"' + " есть атрибут Params, но нет параметров!");
                Console.WriteLine("\r{0}:\tavgT: {1} ms = {2} ms / {3}", method.Name, avgTime, timer.ElapsedMilliseconds, myTestAttrib.TestCount);
                Console.ResetColor();
            }
            else
                Console.WriteLine("\r{0}:\tavgT: {1} ms = {2} ms / {3}", method.Name, avgTime, timer.ElapsedMilliseconds, myTestAttrib.TestCount);
        }

        private static void InvokeNoAttributes(MethodInfo method, MyTestAttribute myTestAttrib, object ClassObject)
        {
            if (method.IsStatic)
                for (var i = 0; i < myTestAttrib.TestCount; i++)
                    method.Invoke(null, null);
            else
                for (var i = 0; i < myTestAttrib.TestCount; i++)
                    method.Invoke(ClassObject, null);
        }
    }
}