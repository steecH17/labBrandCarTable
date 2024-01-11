using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace labBrandCarTable
{
    public class BusBrand:CCarBrand
    {
        public BusBrand()
        {
            vehicles = new List<Vehicles>();
            Brand = "null";
            ModelCar = "null";
            HorsePower = 0;
            MaxSpeed = 0;
        }
        public BusBrand(string brand, string modelCar, int horsePower, int maxSpeed)
        {
            vehicles = new List<Vehicles>();
            Brand = brand;
            ModelCar = modelCar;
            HorsePower = horsePower;
            MaxSpeed = maxSpeed;
        }
        public BusBrand(string brand, string modelCar, int horsePower, int maxSpeed, List<Vehicles> data)
        {
            this.vehicles = new List<Vehicles>();
            Bus bus = null;
            foreach (Vehicles vehicle in data)
            {
                bus = (Bus)vehicle;
                this.vehicles.Add(new Bus(bus.RegistrationNumber, bus.PassengersCount, bus.NumbersOfSeat));
            }

            Brand = brand;
            ModelCar = modelCar;
            HorsePower = horsePower;
            MaxSpeed = maxSpeed;
        }
    }
}
