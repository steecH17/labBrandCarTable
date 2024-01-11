using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace labBrandCarTable
{
    public interface ICarBrand
    {
        string Brand { get; set; }
        string ModelCar { get; set; }
        int HorsePower { get; set; }
        int MaxSpeed { get; set; }
        List<Vehicles> vehicles { get; set; }
    }
}
