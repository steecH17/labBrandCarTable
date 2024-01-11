using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace labBrandCarTable
{
    public class Truck:Vehicles
    {

        public int WheelCount { get; set; }
        public int BodyVolume { get; set; }
        public Truck()
        {
            RegistrationNumber = "null";
            WheelCount = 0;
            BodyVolume = 0;
        }
        public Truck(string registrationNumber, int wheelCount, int bodyVolume)
        {
            RegistrationNumber = registrationNumber;
            WheelCount = wheelCount;
            BodyVolume = bodyVolume;
        }
    }
}
