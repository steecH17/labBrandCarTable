using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace labBrandCarTable
{
    public class CCarBrand:ICarBrand
    {
        public string Brand { get; set; }
        public string ModelCar { get; set; }
        public int HorsePower { get; set; }
        public int MaxSpeed { get; set; }
        public List<Vehicles> vehicles { get; set; }

        public CCarBrand()
        {
            vehicles = new List<Vehicles>();
            Brand = "null";
            ModelCar = "null";
            HorsePower = 0;
            MaxSpeed = 0;

        }
        public CCarBrand(string brand, string modelCar, int horsePower, int maxSpeed)
        {
            vehicles = new List<Vehicles>();
            Brand = brand;
            ModelCar = modelCar;
            HorsePower = horsePower;
            MaxSpeed = maxSpeed;
        }
    }
}
