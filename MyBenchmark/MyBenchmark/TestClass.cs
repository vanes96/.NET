using System;

namespace MyBenchmark
{
    public class TestClass
    {
        private bool p1, p2, p3, p4, p5, p6;
        public int q1, q2, q3, q4, q5, q6;

        public TestClass()
        {
            p1 = p3 = p5 = true;
            p2 = p4 = p6 = false;
        }

        //todo: Сделать ДЗ, эти методы должны тестироватся
        //[MyTest(1000)]
        public void Framework()
        {
            q1 = Convert.ToInt32(p1);
            q2 = Convert.ToInt32(p2);
            q3 = Convert.ToInt32(p3);
            q4 = Convert.ToInt32(p4);
            q5 = Convert.ToInt32(p5);
            q6 = Convert.ToInt32(p6);
        }

        //[MyTest(1000)]
        public void IfThenElse()
        {
            q1 = p1 ? 1 : 0;
            q2 = p2 ? 1 : 0;
            q3 = p3 ? 1 : 0;
            q4 = p4 ? 1 : 0;
            q5 = p5 ? 1 : 0;
            q6 = p6 ? 1 : 0;
        }
    }
}