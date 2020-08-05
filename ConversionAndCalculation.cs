using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace calculator
{
    class ConversionAndCalculation
    {
        private static List<string> order;
        public static int Calculation(string[] postfixArr)
        {
            Stack<string> stack = new Stack<string>();
            foreach (string curStr in postfixArr)
            {
                if (!IsOperator(curStr))//is operand(number)
                {
                    stack.Push(curStr);
                }
                else
                {
                    if (stack.Count() < 2)
                    {
                        throw new Exception("Error");
                    }
                    int op1, op2;
                    op2 = Int32.Parse(stack.Pop());
                    op1 = Int32.Parse(stack.Pop());
                    switch (curStr[0])
                    {
                        case '+':
                            stack.Push((op1 + op2).ToString());
                            break;
                        case '-':
                            stack.Push((op1 - op2).ToString());
                            break;
                        case '*':
                            stack.Push((op1 * op2).ToString());
                            break;
                        case '/':
                            stack.Push((op1 / op2).ToString());
                            break;
                    }
                }
            }
            return Int32.Parse(stack.Pop());
        }
        public static bool TestExpression(string infixStr)
        {
            string[] infixArr = StrToArray(infixStr);
            int opratorCnt = 0, operandCnt = 0;
            foreach (string curStr in infixArr)
            {
                if (IsOperator(curStr))
                {
                    opratorCnt++;
                }
                else
                {
                    operandCnt++;
                }
            }
            if ((operandCnt - opratorCnt == 1))
            {
                return true;
            }
            return false;
        }

        public static string[] DoTransPos(string infixStr)
        {
            return DoTrans(StrToArray(infixStr));
        }

        public static string DoTransPre(string infixStr)
        {
            string[] infixStrArr = StrToArray(infixStr);
            int index = infixStrArr.Count() - 1;
            string[] revertArr = new string[infixStrArr.Count()];
            for (int i = 0; i < infixStrArr.Count(); i++)
            {
                revertArr[i] = infixStrArr[index];
                index--;
            }

            string[] ar = DoTrans(revertArr);
            StringBuilder stb = new StringBuilder();
            for (int i = ar.Count() - 1; i >= 0; i--)
            {
                stb.Append(ar[i]);
            }
            return stb.ToString();
        }

        private static string[] DoTrans(string[] input)
        {

            order = new List<string>();
            Stack<char> theStack = new Stack<char>();
            for (int j = 0; j < input.Count(); j++)
            {
                if (input[j].Count() != 1)
                {
                    order.Add(input[j]);
                }
                else
                {
                    char ch = input[j][0];
                    switch (ch)
                    {
                        case '+':
                        case '-':
                            GotOper(ch, 1, theStack);
                            break;
                        case '*':
                        case '/':
                            GotOper(ch, 2, theStack);
                            break;
                        case '(':
                            theStack.Push(ch);
                            break;
                        case ')':
                            GotParen(theStack);
                            break;
                        default:
                            order.Add(ch.ToString());
                            break;
                    }
                }
            }
            while (!(theStack.Count() == 0))
            {
                order.Add(theStack.Pop().ToString());
            }
            return order.ToArray();
        }


        private static string[] StrToArray(string inputStr)
        {
            List<string> result = new List<string>();
            string s = "";
            for (int i = 0; i < inputStr.Length; i++)
            {
                if (IsOperator(inputStr[i]))
                {
                    if (!s.Equals(""))
                    {
                        result.Add(s);
                    }

                    result.Add(inputStr[i].ToString());
                    s = "";
                }
                else
                {
                    s += inputStr[i];
                }
            }
            if (!s.Equals(""))
            {
                result.Add(s);
            }

            return result.ToArray();
        }

        private static bool IsOperator(string inputStr)
        {
            if (inputStr.Count() != 1)
                return false;
            return IsOperator(inputStr[0]);
        }
        private static bool IsOperator(char c)
        {
            return c == '+' || c == '-' || c == '*' || c == '/' || c == '^'
                    || c == '(' || c == ')';
        }


        private static void GotOper(char opThis, int prec1, Stack<char> theStack)
        {
            while (!(theStack.Count() == 0))
            {
                char opTop = theStack.Pop();
                if (opTop == '(')
                {
                    theStack.Push(opTop);
                    break;
                }
                else
                {
                    int prec2;
                    if (opTop == '+' || opTop == '-')
                    {
                        prec2 = 1;
                    }
                    else
                    {
                        prec2 = 2;
                    }
                    if (prec2 < prec1)
                    {
                        theStack.Push(opTop);
                        break;
                    }
                    else
                    {
                        order.Add(opTop.ToString());
                    }
                }
            }
            theStack.Push(opThis);
        }
        private static void GotParen(Stack<char> theStack)
        {
            while (!(theStack.Count() == 0))
            {
                char chx = theStack.Pop();
                if (chx == '(')
                {
                    break;
                }
                else
                {
                    order.Add(chx.ToString());
                }
            }
        }
    }
}
