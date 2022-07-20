using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Task01_EmployeesDirectoryWithStruct
{
    internal struct EmployeesDirectory
    {
        Employee[] _employees;
        public Employee[] Employees { get { return _employees; } set { _employees = value; } }
        const int DEFAULT_NEW_ARRAY_SIZE = 10;
        const int COUNT_TITLES = 9;
        string _filePath;
        int _index;
        /// <summary>
        /// Кол-во элементов в массиве работников
        /// </summary>
        public int Index { get { return _index; } set { _index = value; } }
        string[] _titles;
        /// <summary>
        /// Путь к списку сотрудников
        /// </summary>
        public string FilePath
        {
            get { return _filePath; }
            set { _filePath = value; }
        }
        /// <summary>
        /// Индексатор массива с информацией о работниках Employee
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Employee this[int index]
        {
            get { return _employees[index]; }
            set { _employees[index] = value; }
        }
        public EmployeesDirectory(string filePath)
        {
            _employees = new Employee[DEFAULT_NEW_ARRAY_SIZE];
            _filePath = filePath;
            _titles = new string[COUNT_TITLES];
            _index = 0;
        }
        /// <summary>
        /// Чтение данных из справочника "работники"
        /// </summary>
        internal void LoadEmployeeList()
        {
            using (StreamReader reader = new StreamReader(new FileStream(FilePath, FileMode.OpenOrCreate, FileAccess.Read)))
            {
                while (!reader.EndOfStream)
                {
                    if (Index >= Employees.Length)
                        ResizeArray(Employees, Employees.Length + DEFAULT_NEW_ARRAY_SIZE);
                    this[Index] = ParseEmployeeFromFile(reader.ReadLine());
                    Index++;
                }
            }
        }
        /// <summary>
        /// Метод изменения размера массива, размер увеличивается на константу = 10
        /// </summary>
        /// <param name="array">Массив с данными</param>
        /// <param name="newSize">Новый размер массива</param>
        private void ResizeArray(Employee[] array, int newSize)
        {
            Array.Resize(ref array, array.Length + newSize);
        }
        /// <summary>
        /// Метод приведения данных из текстовой строки в структуру Employee
        /// </summary>
        /// <param name="strLine">Считанная строка из файла</param>
        /// <param name="separator">Разделитель по умолчанию - #</param>
        /// <returns>Возвращает экземпляр структуры Employee</returns>
        private Employee ParseEmployeeFromFile(string strLine, char separator = '#')
        {
            string[] values = strLine.Split(separator);
            return new Employee
            {
                ID = int.Parse(values[0]),
                DateRecord = DateTime.Parse(values[1]),
                LastName = values[2],
                FirstName = values[3],
                MiddleName = values[4],
                Age = ushort.Parse(values[5]),
                Height = ushort.Parse(values[6]),
                Birthday = DateTime.Parse(values[7]),
                City = values[8]
            };
        }
        /// <summary>
        /// Метод записи информации о работниках в справочник
        /// </summary>
        /// <param name="employees">Массив с информацией о рабочих</param>
        private void SaveEmployeeList()
        {
            if (Index == 0)
            {
                Console.WriteLine("Нет данных для записи!");
                return;
            }
            using (StreamWriter writer = new StreamWriter(new FileStream(FilePath, FileMode.Create, FileAccess.Write)))
            {
                for (int i = 0; i < Index; i++)
                {
                    //Employees[i].DateRecord = DateTime.Now;
                    writer.WriteLine(Employees[i].ToStringForWrite());
                }
            }
        }
        /// <summary>
        /// Метод добавления информации о новых работниках
        /// </summary>
        internal void AddEmployeeInfo()
        {
            while (true)
            {
                Console.WriteLine("Введите данные нового работника");
                Employee employee = new Employee();
                employee.ID = Index + 1;
                employee.DateRecord = DateTime.Now;
                Console.Write("Фамилия: ");
                employee.LastName = Console.ReadLine();
                Console.Write("Имя: ");
                employee.FirstName = Console.ReadLine();
                Console.Write("Отчество: ");
                employee.MiddleName = Console.ReadLine();
                Console.Write("Возраст работника: ");
                employee.Age = ushort.Parse(Console.ReadLine());
                Console.Write("Рост работника: ");
                employee.Height = ushort.Parse(Console.ReadLine());
                Console.Write("Дата рождения: ");
                employee.Birthday = DateTime.Parse(Console.ReadLine());
                Console.Write("Место рождения: ");
                employee.City = Console.ReadLine();
                if (Index >= Employees.Length) ResizeArray(Employees, DEFAULT_NEW_ARRAY_SIZE);
                Employees[Index++] = employee;
                Console.WriteLine("чтобы добавить след. Работника нажмите - Enter, для выхода нажмите - Escape");
                if (Console.ReadKey().Key == ConsoleKey.Escape)
                {
                    Console.WriteLine(new String('=', 120));
                    break;
                }

            }
            PrintEmployeesList();
            SaveEmployeeList();
        }
        /// <summary>
        /// Метод удаления информации по ID
        /// </summary>
        /// <param name="id">Номер сотрудника</param>
        internal void RemoveEmployeeInfo()
        {
            if (!CheckCountElements()) return;
            PrintEmployeesList();
            Console.Write("Введите номер сотрудника для удаления: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Вы ввели не допустимое значение!");
                return;
            }
            if (CheckIdRange(id))
            {
                id--;
                Employee[] newEmployees = new Employee[Employees.Length];
                for (int i = 0, j = 0; i < Index; i++, j++)
                {
                    if (id == j)
                        j++;
                    newEmployees[i] = Employees[j];
                    newEmployees[i].ID = i + 1;
                }
                Employees = newEmployees;
                Index--;
            }
            PrintEmployeesList();
            SaveEmployeeList();
        }
        /// <summary>
        /// Метод редактирования информации о работнике
        /// </summary>
        internal void UpdateEmployeeInfo()
        {
            PrintEmployeesList();
            Console.Write("Введите номер сотрудника для редактирования: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Вы ввели не допустимое значение!");
                return;
            }
            if (CheckIdRange(id))
            {
                int currIndex = id - 1;
                do
                {
                    PrintEmployeesList(id);
                    Console.WriteLine("С помощью цифр укажите какое поле необходимо изменить:");
                    Console.WriteLine("\t1 - Фамилия, 2 - Имя, 3 - Отчество, 4 - Возраст, 5 - Рост, 6 - Дата рождения, 7 - Место рождения");
                    Console.Write("Ваш выбор полей: ");
                    if (!int.TryParse(Console.ReadLine(), out int fieldNum))
                    {
                        fieldNum = -1;
                    }
                    switch (fieldNum)
                    {
                        case 1:
                            Console.WriteLine($"Текущее значение поля Фамилия - {Employees[currIndex].LastName}");
                            Console.Write("Введите новое значение: ");
                            Employees[currIndex].LastName = Console.ReadLine();
                            break;
                        case 2:
                            Console.WriteLine($"Текущее значение поля Имя - {Employees[currIndex].FirstName}");
                            Console.Write("Введите новое значение: ");
                            Employees[currIndex].FirstName = Console.ReadLine();
                            break;
                        case 3:
                            Console.WriteLine($"Текущее значение поля Отчество - {Employees[currIndex].MiddleName}");
                            Console.Write("Введите новое значение: ");
                            Employees[currIndex].MiddleName = Console.ReadLine();
                            break;
                        case 4:
                            Console.WriteLine($"Текущее значение поля Возраст - {Employees[currIndex].Age}");
                            Console.Write("Введите новое значение: ");
                            Employees[currIndex].Age = ushort.Parse(Console.ReadLine());
                            break;
                        case 5:
                            Console.WriteLine($"Текущее значение поля Рост - {Employees[currIndex].Height}");
                            Console.Write("Введите новое значение: ");
                            Employees[currIndex].Height = ushort.Parse(Console.ReadLine());
                            break;
                        case 6:
                            Console.WriteLine($"Текущее значение поля Дата рождения - {Employees[currIndex].Birthday}");
                            Console.Write("Введите новое значение: ");
                            Employees[currIndex].Birthday = DateTime.Parse(Console.ReadLine());
                            break;
                        case 7:
                            Console.WriteLine($"Текущее значение поля Место рождения - {Employees[currIndex].City}");
                            Console.Write("Введите новое значение: ");
                            Employees[currIndex].City = Console.ReadLine();
                            break;
                        default:
                            Console.WriteLine("Поле с таким номером не существует!");
                            return;
                    }
                    Console.WriteLine("Данные обновлены:");
                    PrintTitles();
                    Employees[currIndex].DateRecord = DateTime.Now;
                    Console.WriteLine(Employees[currIndex]);
                    Console.WriteLine("Для выхода из редактирования нажмите - Escape, Enter - продолжить редактирование");
                } while (Console.ReadKey().Key != ConsoleKey.Escape);
                SaveEmployeeList();
            }
        }
        /// <summary>
        /// Метод сортировки массива по дате создания записей
        /// </summary>
        /// <param name="empArr">массив с информацие о работниках</param>
        /// <param name="start">индекс начала массива</param>
        /// <param name="end">индекс конца массива конец</param>
        /// <param name="sortByAsc">необязательный аргумент, для выбора направления сортировки</param>
        internal void SortEmployeeArray(Employee[] empArr, int start, int end, bool sortByAsc = true)
        {
            if (CheckCountElements()) return;
            if (start >= end) return;
            int pivot = SortPartition(empArr, start, end, sortByAsc);
            SortEmployeeArray(empArr, start, pivot - 1, sortByAsc);
            SortEmployeeArray(empArr, pivot + 1, end, sortByAsc);
        }
        /// <summary>
        /// Метод разбиение массива на части перед сортировкой
        /// </summary>
        /// <param name="empArr">массив с информацие о работниках</param>
        /// <param name="start">индекс начала массива</param>
        /// <param name="end">индекс конца массива конец</param>
        /// <param name="sortByAsc">необязательный аргумент, для выбора направления сортировки</param>
        /// <returns>возвращает индекс нового опорного элемента</returns>
        private int SortPartition(Employee[] empArr, int start, int end, bool sortByAsc = true)
        {
            int marker = start;
            for (int i = start; i <= end; i++)
            {
                switch (sortByAsc)
                {
                    case true:
                        if (empArr[i].DateRecord <= empArr[end].DateRecord)
                        {
                            Employee temp = empArr[marker];
                            empArr[marker] = empArr[i];
                            empArr[i] = temp;
                            marker += 1;
                        }
                        break;
                    case false:
                        if (empArr[i].DateRecord >= empArr[end].DateRecord)
                        {
                            Employee temp = empArr[marker];
                            empArr[marker] = empArr[i];
                            empArr[i] = temp;
                            marker += 1;
                        }
                        break;
                }

            }
            return marker - 1;
        }
        /// <summary>
        /// Метод печати заголовков
        /// </summary>
        private void PrintTitles()
        {
            Console.WriteLine($"{" ID",-3} {"Дата записи",15} {"Фамилия",15} {"Имя",10} {"Отчество",15} {"Возраст",10} {"Рост",5} {"Дата рождения",15} {"Место рождения",15}");
        }
        /// <summary>
        /// Метод вывода на экран информацию о работниках
        /// </summary>
        internal void PrintEmployeesList(int id = -1)
        {
            PrintTitles();
            if (id == -1)
            {
                for (int i = 0; i < Index; i++)
                    Console.WriteLine(Employees[i].ToString());
            }
            else if (CheckIdRange(id))
            {
                Console.WriteLine(Employees[id - 1].ToString());
            }
            Console.WriteLine(new String('=', 120));
        }
        /// <summary>
        /// Метод вывода на экран информации в диапазоне дат(ы)
        /// </summary>
        internal void PrintEmployeesListWithRange()
        {
            DateRange range = ParseDateRange();
            PrintTitles();
            if (range.Start == DateTime.MinValue && range.End == DateTime.MinValue)
            {
                Console.WriteLine("Вы не указали или некорректно ввели диапазон дат!");
            }
            else if (range.End == range.Start)
            {
                for (int i = 0; i < Index; i++)
                {
                    if (Employees[i].DateRecord.ToShortDateString() == range.Start.ToShortDateString())
                        Console.WriteLine(Employees[i]);
                }
            }
            else
            {
                for (int i = 0; i < Index; i++)
                {
                    if (Employees[i].DateRecord >= range.Start && Employees[i].DateRecord <= range.End.AddDays(1))
                        Console.WriteLine(Employees[i]);
                }
            }
            Console.WriteLine(new String('=', 120));
        }
        /// <summary>
        /// Метод для извлечение из строки диапазон дат(ы)
        /// </summary>
        /// <returns>возвращает структуру с диапазоном дат</returns>
        private DateRange ParseDateRange()
        {
            Console.Write("Введите дату или диапазон дат, диапазон дат должен быть разделен \"-\": ");
            string[] dates = Console.ReadLine().Split('-');
            if (dates.Length == 1 && !string.IsNullOrEmpty(dates[0]))
            {
                DateTime date = DateTime.Parse(dates[0]);
                return new DateRange(date, date);
            }
            else if (dates.Length == 2)
            {
                return new DateRange(DateTime.Parse(dates[0]), DateTime.Parse(dates[1]));
            }
            else
                return new DateRange();
        }
        /// <summary>
        /// Метод проверки вхождения выбранного номера в списке работников
        /// </summary>
        /// <param name="id">Номер сотрудника</param>
        /// <returns>Возвращает истиность вхождения id в диапазон работников</returns>
        private bool CheckIdRange(int id)
        {
            if (id > 0 && id <= Index)
                return true;
            else
            {
                Console.WriteLine("Информация по данному ID не найдена!");
                return false;
            }
        }
        /// <summary>
        /// Проверка на наличие записей в массиве
        /// </summary>
        /// <returns></returns>
        private bool CheckCountElements()
        {
            if (Index == 0)
            {
                Console.WriteLine("В списке нет записей!");
                return false;
            }
            return true;
        }
    }
}
