using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace labBrandCarTable
{
    public class Car:Vehicles
    {
        
        public string NamedMultimedia { get; set; }
        public int AirbagCount { get; set; }
        public Car()
        {
            RegistrationNumber = "null";
            NamedMultimedia = "null";
            AirbagCount = 0;
        }
        public Car(string registrationNumber, string namedMultimedia, int airbagCount)
        {
            RegistrationNumber = registrationNumber;
            NamedMultimedia = namedMultimedia;
            AirbagCount = airbagCount;
        }
    }
}
