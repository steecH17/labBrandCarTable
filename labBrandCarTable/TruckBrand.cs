using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace labBrandCarTable
{
    public class TruckBrand:CCarBrand
    {

        public TruckBrand()
        {
            vehicles = new List<Vehicles>();
            Brand = "null";
            ModelCar = "null";
            HorsePower = 0;
            MaxSpeed = 0;

        }
        public TruckBrand(string brand, string modelCar, int horsePower, int maxSpeed)
        {
            vehicles = new List<Vehicles>();
            Brand = brand;
            ModelCar = modelCar;
            HorsePower = horsePower;
            MaxSpeed = maxSpeed;
        }
        public TruckBrand(string brand, string modelCar, int horsePower, int maxSpeed, List<Vehicles> data)
        {
            vehicles = new List<Vehicles>();
            foreach (Vehicles vehicle in data) 
            {
                vehicles.Add(vehicle);
            }
            Brand = brand;
            ModelCar = modelCar;
            HorsePower = horsePower;
            MaxSpeed = maxSpeed;
        }
    }
}
