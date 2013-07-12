using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiveElementsIntTest.OpSpan
{
    public enum EquationType
    {
        NonCarry, Carry, Illegal
    }

    public enum EquationMethod
    {
        Add, Subtract, Null
    }

    public class OpSpanEquationMaker
    {
        private List<int> m1DigitChart;
        private Random mRandom;

        public OpSpanEquationMaker()
        {
            mRandom = new Random();
            init1DigitChart();
        }

        private void init1DigitChart()
        {
            m1DigitChart = new List<int>();
            for (int i = 1; i < 10; i++)
            {
                m1DigitChart.Add(i);
                m1DigitChart.Add(i * 10);
            }
        }

        private String Method2String(EquationMethod method)
        {
            if (method == EquationMethod.Add)
                return "+";
            else if (method == EquationMethod.Subtract)
                return "-";
            else
                return "null";
        }

        public void GenEquation(ref String _equation, ref int _answer, 
            EquationType type)
        {
            int first = 0;
            int second = 0;
            int third = 0;

            EquationMethod method1 = EquationMethod.Null;
            EquationMethod method2 = EquationMethod.Null;

            //decide 3 units or 4 units
            if (getRandomBool())//3 units
            {
                get3Units(ref first, ref second, ref third, ref method1, ref method2, type);
            }
            else//4 units
            {
                get4Units(ref first, ref second, ref third, ref method1, ref method2, type);
            }

            _equation = first.ToString() + 
                Method2String(method1) + second.ToString() + 
                Method2String(method2) + third;

            _answer = doCalc(doCalc(first, second, method1), third, method2);
        }

        private int get1DigitNum()
        {
            int retval = 0;
            int index = mRandom.Next(0, m1DigitChart.Count);
            retval = m1DigitChart[index];
            return retval;
        }

        private int get2DigitsNum()
        {
            int retval = 0;
            while (retval % 10 == 0)
            {
                retval = mRandom.Next(1, 98);
            }

            return retval;
        }

        private void get3Units(ref int first, ref int second, ref int third, 
            ref EquationMethod method1, ref EquationMethod method2,
            EquationType type)
        {
            int carryCount = 0;

            while (true)
            {
                carryCount = 0;
                first = get1DigitNum();
                second = get1DigitNum();
                third = get1DigitNum();
                method1 = getRandomMethod();
                method2 = getRandomMethod();

                if (ifCarry(first, second, method1) == EquationType.Carry)
                {
                    carryCount++;
                }

                int firstAndSecond = doCalc(first, second, method1);

                if (ifCarry(firstAndSecond, third, method2) == EquationType.Carry)
                {
                    carryCount++;
                }

                if (type == EquationType.Carry)
                {
                    if (carryCount == 1 && doCalc(firstAndSecond, third, method2) >= 0)
                        break;
                }
                else if (type == EquationType.NonCarry && doCalc(firstAndSecond, third, method2) >= 0)
                {
                    if (carryCount == 0)
                        break;
                }
            }
        }

        private int doCalc(int first, int second, EquationMethod method)
        {
            int retval = 0;

            if (method == EquationMethod.Add)
                retval = first + second;
            else
                retval = first - second;

            return retval;
        }

        private void get4Units(ref int first, ref int second, ref int third,
            ref EquationMethod method1, ref EquationMethod method2,
            EquationType type)
        {
            int carryCount = 0;
            while (true)
            {
                method1 = getRandomMethod();
                method2 = getRandomMethod();
                carryCount = 0;
                switch (mRandom.Next(0, 3))
                {
                    case 0:
                        first = get2DigitsNum();
                        second = get1DigitNum();
                        third = get1DigitNum();
                        break;
                    case 1:
                        first = get1DigitNum();
                        second = get2DigitsNum();
                        third = get1DigitNum();
                        break;
                    case 2:
                        first = get1DigitNum();
                        second = get1DigitNum();
                        third = get2DigitsNum();
                        break;
                }

                if (ifCarry(first, second, method1) == EquationType.Carry)
                {
                    carryCount++;
                }

                int firstAndSecond = doCalc(first, second, method1);

                if (ifCarry(firstAndSecond, third, method2) == EquationType.Carry)
                {
                    carryCount++;
                }

                if (type == EquationType.Carry && doCalc(firstAndSecond, third, method2) >= 0)
                {
                    if (carryCount == 1)
                        break;
                }

                if (type == EquationType.NonCarry && doCalc(firstAndSecond, third, method2) >= 0)
                {
                    if (carryCount == 0)
                        break;
                }
            }
        }

        private EquationType ifCarry(int first, int second, EquationMethod method)
        {
            int first10 = first / 10;
            int firstSingle = first % 10;
            int second10 = second / 10;
            int secondSingle = second % 10;

            EquationType retval = EquationType.NonCarry;

            if (method == EquationMethod.Add)
            {
                if (first10 + second10 >= 10 ||
                    firstSingle + secondSingle >= 10)
                    retval = EquationType.Carry;
            }
            else if (method == EquationMethod.Subtract)
            {
                if (first10 - second10 < 0 ||
                                   firstSingle - secondSingle < 0)
                    retval = EquationType.Carry;
            }

            return retval;
        }

        private bool getRandomBool()
        {
            int rdmInt = mRandom.Next(0, 2);

            if (rdmInt == 0)
                return false;
            else
                return true;
        }

        private EquationMethod getRandomMethod()
        {
            EquationMethod retval;
            if (getRandomBool())
                retval = EquationMethod.Add;
            else
                retval = EquationMethod.Subtract;

            return retval;

        }
    }
}
