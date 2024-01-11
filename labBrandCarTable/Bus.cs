using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace labBrandCarTable
{
    public class Bus:Vehicles
    {
        public int PassengersCount {  get; set; }
        public int NumbersOfSeat {  get; set; }
        public Bus() 
        {
            RegistrationNumber = "null";
            PassengersCount = 0;
            NumbersOfSeat = 0;
        }
        public Bus(string registrationNumber, int passengersCount, int numbersSeat)
        {
            RegistrationNumber = registrationNumber;
            PassengersCount = passengersCount;
            NumbersOfSeat = numbersSeat;
        }
    }
}
