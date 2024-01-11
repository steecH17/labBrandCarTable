using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace labBrandCarTable
{
    public class PassengerCarBrand:CCarBrand
    {
        
        public PassengerCarBrand()
        {
            vehicles = new List<Vehicles>();
            Brand = "null";
            ModelCar = "null";
            HorsePower = 0;
            MaxSpeed = 0;
        }
        public PassengerCarBrand(string brand, string modelCar, int horsePower, int maxSpeed)
        {
            vehicles = new List<Vehicles>();
            Brand = brand;
            ModelCar = modelCar;
            HorsePower = horsePower;
            MaxSpeed = maxSpeed;
        }
        public PassengerCarBrand(string brand, string modelCar, int horsePower, int maxSpeed, List<Vehicles> data)
        {
            this.vehicles = new List<Vehicles>();
            Car car = null;
            foreach(Vehicles vehicle in data)
            {
                car = (Car)vehicle;
                this.vehicles.Add(new Car(car.RegistrationNumber, car.NamedMultimedia, car.AirbagCount)); 
            }
           
            Brand = brand;
            ModelCar = modelCar;
            HorsePower = horsePower;
            MaxSpeed = maxSpeed;
        }
    }
}
