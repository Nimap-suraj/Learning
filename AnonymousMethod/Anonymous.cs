//public delegate void MyDelegate(int num);

namespace AnonymousMethod
{/// <summary>
/// We learn
/// Anonymus Type and Anonymous Method.
/// </summary>
    // Without Delegate
    //public static void MyMethod(MyDelegate del,int num)
    //{
    //    num = num * 10;
    //    del(num);
    //}
    public class Anonymous
    {

        //MyDelegate obj = new MyDelegate(MyMethod);
        // PART 2
        //MyDelegate obj = delegate (int a)
        //{
        //    a = a * 10;
        //    Console.WriteLine("anonomus method ans " + a);
        //};  
        //obj(10);
        // parameter anonymous method.
        //Program.MyMethod(
        //    delegate(int num) 
        //{ 
        //    num = num * 10;
        //    Console.WriteLine("Anonymous Method : " + num); 
        //}
        //,10);
        //        Anonymous type is a class without
        //any name that can contain public read only properties.
        //it cannot contain any other properties
        //such as fields,methods,events etc...
        //we use var keyword in anonymous type
        //var student = new { Id = 1, FirstName = surajshah, lastName = shah };
        //var Student = new
        //{
        //    Id = 1,
        //    FirstName = "Suraj",
        //    LastName = "shah",
        //    Address = new
        //    {
        //        AddressId = 205,
        //        Name = "Building no 3"
        //    }
        //};
        //Console.WriteLine(Student.Id);
        //Console.WriteLine(Student.FirstName);
        //Console.WriteLine(Student.LastName);
        //Console.WriteLine(Student.Address.Name);
        //Console.ReadLine();
    }
}
