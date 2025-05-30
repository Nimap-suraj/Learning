using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dependency_Injection.Models
{
    public class Person
    {
        public Home _home;
        public IEducational _school;
        //private IEmployeeService _Iemppere;
        public IEducational School
        {
            set
            {
                _school = value;  // yaha huaa mien
            }
        }
        public Person(Home home) 
        {
            _home = home;
            //_school = new School();  uppar check kar woh property injection ho raa hain use
        }
        public void TakeRefuge()
        {
            _home.ProvideShelter(this);
        }
        public void Study()
        {
            if (_school != null)
            {
                _school.Teach(this);
            }
        }
        //method
        public void GetTreatment(Hospital hospital)
        {
            hospital.Cure(this);
        }
    }
}
