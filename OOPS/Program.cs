using OOPS;
using OOPS.Learn;

public class Program
{
    private static void Main(string[] args)
    {
        // Encapsualtion
        Console.WriteLine("Step 1: Encapsulation");

        Vehicle vehicle = new Vehicle("ODIII","CAR");
        vehicle.DisplayVehicle();
        //vehicle._make
        vehicle.StartEngine();
        Console.WriteLine("Step 2: Inheritance");
        Car car = new Car("OdIII","CAR",4);


        car.DisplayVehicle();
        car.DisplayDoors();
        car.StartEngine();

    }
    //static void Learn()
    //{
    //    // Encapsulation.
    //    Account obj = new Account();
    //    Console.WriteLine($"The Balance of Account is {obj.GetBalance()}");
    //    //obj.balance = 1000000; Error 


    //    // Abstaraction
    //    Car car = new Car();
    //    car.Brakes = "b1";

    //    // car.Exchuast_system(); due to Protection Level it's Not Accessible
    //    // Inheritance

    //    Rectangle r = new Rectangle();
    //    r.SetWidth(10);
    //    r.SetHeight(10);
    //    Console.WriteLine($"The Area of Rectangle is {r.GetArea()}");


    //    // Polymorphism
    //    Base b = new Base();
    //    Console.WriteLine($"Adding two Integer number are {b.Add(1, 9)}");
    //    Console.WriteLine($"Adding two Double number are {b.Add(1.2, 8.8)}");

    //}
}