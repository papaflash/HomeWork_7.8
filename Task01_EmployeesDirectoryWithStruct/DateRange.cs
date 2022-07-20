using System;
using System.Collections.Generic;
using System.Text;

namespace Task01_EmployeesDirectoryWithStruct
{
    internal struct DateRange
    {
        public DateTime Start { private set; get; }
        public DateTime End { private set; get; }
        public DateRange(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }
    }
}
