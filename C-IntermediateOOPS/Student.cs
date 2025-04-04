using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Intermediate
{
    internal class Student
    {
        //public DateTime _BirthDate;
        //public void SetBirthDate(DateTime birthDate)
        //{
        //    _BirthDate = birthDate;
        //}
        //public DateTime GetBirthdate()
        //{
        //    return _BirthDate;
        //}
       
        public DateTime _BirthDate { get;private set; }
        public string Name { get;private set; } 
        public Student(DateTime birthDate)
        {
            _BirthDate = birthDate;
        }
        public int Age
        {
            get
            {
                var  timeSpan = DateTime.Today - _BirthDate;
                // in a result days are stored 
                //  to calculate year we have to divided by 365
                var years = timeSpan.Days / 365;
                return years;
            }
        }
        public int Month
        {
            get
                //1 year = 365
                //1 month = 31
            {
                var timeSpan = DateTime.Today - _BirthDate;
                // in a result days are stored 
                //  to calculate year we have to divided by 365
            ;
                return timeSpan.Days;
            }
        }

    }
}
