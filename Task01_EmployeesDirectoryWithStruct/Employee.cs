using System;
using System.Collections.Generic;
using System.Text;

namespace Task01_EmployeesDirectoryWithStruct
{
    public struct Employee
    {
        int _id;
        DateTime _dateRecord;
        string _lastName;
        string _firstName;
        string _middleName;
        ushort _age;
        ushort _height;
        DateTime _birthday;
        string _city;
        /// <summary>
        /// Id сотрудника
        /// </summary>
        public int ID { get { return _id; } set { _id = value; } }
        /// <summary>
        /// Время записи в справочник
        /// </summary>
        public DateTime DateRecord { get { return _dateRecord; } set { _dateRecord = value; } } 
        /// <summary>
        /// Фамилия
        /// </summary>
        public string LastName
        {
            get { return _lastName; }
            set { SetStringProperty(ref _lastName, value); }
        }
        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName
        {
            get { return _firstName; }
            set { SetStringProperty(ref _firstName, value); }
        }
        /// <summary>
        /// Отчество
        /// </summary>
        public string MiddleName
        {
            get { return _middleName; }
            set { SetStringProperty(ref _middleName, value); }
        }
        /// <summary>
        /// Возраст
        /// </summary>
        public ushort Age
        {
            get { return _age; }
            set 
            { 
                if(value > 0 && value != Age)
                _age = value; 
            }
        }
        /// <summary>
        /// Рост
        /// </summary>
        public ushort Height
        {
            get { return _height; }
            set
            {
                if (value > 0 && value != Height)
                    _height = value;
            }
        }
        /// <summary>
        /// Дата рождения
        /// </summary>
        public DateTime Birthday
        {
            get { return _birthday; }
            set
            {
                if(value != null && value != Birthday)
                    _birthday = value;
            }
        }
        /// <summary>
        /// Место рождения
        /// </summary>
        public string City
        {
            get { return _city; }
            set { SetStringProperty(ref _city, value); }
        }
        public Employee(int id, DateTime dateRecord, string firstName, string lastName, ushort age, ushort height, DateTime birthDay, string city, string middleName = "")
        {
            _id = id;
            _dateRecord = dateRecord;
            _lastName = lastName;
            _firstName = firstName;
            _middleName = middleName;
            _age = age;
            _height = height;
            _birthday = birthDay;
            _city = city;
        }
        /// <summary>
        /// Метод безопасного присвоения значения свойству типа string
        /// </summary>
        /// <param name="prop">Тек. Значение свойства</param>
        /// <param name="value">Новое значение</param>
        void SetStringProperty(ref string prop, string value)
        {
            if (!(string.IsNullOrEmpty(value) || prop == value))
                prop = value;
        }
        /// <summary>
        /// Переопределеный метод приведения структуры к строке
        /// </summary>
        /// <returns>возвращает форматированную строку</returns>
        public override string ToString()
        {
            return string.Join(' ',
                ID.ToString("D3"),
                $"{DateRecord,18:g}",
                $"{LastName, 10}",
                $"{FirstName, 15}",
                $"{MiddleName,13}",
                $"{Age,6}",
                $"{Height,7}",
                $"{Birthday,14:d}",
                $"{City,14}");
        }
        /// <summary>
        /// Метод приведения данных в строку для записи в файл
        /// </summary>
        /// <param name="separator">Разделитель по умолчанию</param>
        /// <returns>Вовзращает форматированную строку для записи</returns>
        internal string ToStringForWrite(char separator = '#')
        {
            return string.Join(separator,
                ID.ToString(),
                $"{DateRecord:g}",
                $"{LastName}",
                $"{FirstName}",
                $"{MiddleName}",
                $"{Age}",
                $"{Height}",
                $"{Birthday:d}",
                $"{City}");
        }
    }
}
