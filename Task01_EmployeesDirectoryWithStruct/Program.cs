using System;
using System.IO;

namespace Task01_EmployeesDirectoryWithStruct
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Программа ведения справочника - \"Работники\"");
            const string DEFAULT_NAME = @"\EmployeesList.txt";
            Console.Write("Введите путь к справочнику или нажмите Enter для установки пути по умолчанию: ");
            string path = Console.ReadLine();
            if (string.IsNullOrEmpty(path))
            {
                path = Environment.CurrentDirectory + DEFAULT_NAME;
                Console.WriteLine($"Устновлен путь у справочнику по умолчанию: {path}");
            }
            else if (!Directory.Exists(path))
            {
                Console.WriteLine("Справочник не найден!");
                return;
            }
            EmployeesDirectory employees = new EmployeesDirectory(path);
            employees.LoadEmployeeList();
            bool flag = true;
            while (flag)
            {
                Console.WriteLine("Управление справочником: \n\t1 - Показать все записи справочника" +
                    "\n\t2 - Показать записи в диапазоне дат(ы)\n\t3 - Добавить новых работников" +
                    "\n\t4 - Редактировать информацию о работнике\n\t5 - Удалить работника из справочника" +
                    "\n\t6 - Сортировка по дате(по возрастанию)\n\t7 - Сортировка по дате(по убыванию)\n\tEnter - завершить работу программы");
                Console.Write("Ваш выбор: ");
                switch (Console.ReadLine())
                {
                    case "1":
                        employees.PrintEmployeesList();
                        break;
                    case "2":
                        employees.PrintEmployeesListWithRange();
                        break;
                    case "3":
                        employees.AddEmployeeInfo();
                        break;
                    case "4":
                        employees.UpdateEmployeeInfo();
                        break;
                    case "5":
                        employees.RemoveEmployeeInfo();
                        break;
                    case "6":
                        employees.SortEmployeeArray(employees.Employees, 0, employees.Index - 1);
                        employees.PrintEmployeesList();
                        break;
                    case "7":
                        employees.SortEmployeeArray(employees.Employees, 0, employees.Index - 1, false);
                        employees.PrintEmployeesList();
                        break;
                    case "":
                        flag = false;
                        break;
                    default:
                        Console.WriteLine("Вы ввели не допустимое значение!");
                        break;
                }
            }
        }
    }
}
