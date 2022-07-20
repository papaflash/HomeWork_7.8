using System;
using Task01_EmployeesDirectoryWithStruct;

namespace TestUserArray
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string str = "Шагов Александр Валерьевич 35 187 13.09.1986 г.Горький";
            Console.Write(str);
            string newStr = Console.ReadLine();
            Console.WriteLine(newStr);
        }
    }
}