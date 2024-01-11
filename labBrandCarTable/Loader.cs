using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace labBrandCarTable
{
    public class Loader
    {
        private static Dictionary<string, List<Vehicles>> modelCarData = new Dictionary<string, List<Vehicles>>();
        private static Dictionary<string, int> countCarInData = new Dictionary<string, int>();
        private static Random random = new Random();
        private static List<string> RegistrationNumber = new List<string>() { "A101AA43", "A200BC52", "B111BB33", "C445CF67",
        "D345DD23", "G505HH32", "S210FS32", "A234BD34", "K434KL23", "D345FF44", "H435GG45", "H893GG43", "F431HJ99", "F539FG09",
            "T343UI44", "O548OO32", "H323LK56", "D234GF32", "F322FG44", "D234DD34"};
        private static List<string> NamedMultimedia = new List<string>() { "DFGDFGDG", "IFDGUIF4", "GFGSRES43DF", "DGFGDIUYF",
            "WEROUY54", "ETGNJIJ21"};
        private static List<int> AirbagCount = new List<int>() { 3, 5, 4, 8, 10 };
        private static List<int> WheelCount = new List<int>() { 4, 6, 8, 12, 14 };
        private static List<int> BodyVoume = new List<int>() { 3, 4, 5 ,6 , 45, 90, 12, 11, 54};
        private static List<int> PassengersCount = new List<int>() { 12, 20, 15, 35, 8, 10 , 40};
        private static List<int> NumbersOfSeat = new List<int>() { 12, 15, 6, 8, 20 };
        



        public static async Task Load(string brandName, string type)
        {
            string regNum, multimedia;
            int airbagCount, wheelCount, volume, passengersCount, numbersOfSeat;
            if (!modelCarData.ContainsKey(brandName))
            {
                await Task.Run(() =>
                {
                    modelCarData[brandName] = new List<Vehicles>();
                    int numbersOfCars = random.Next(10, 21);
                    countCarInData.Add(brandName, numbersOfCars);
                    for (int i = 0; i < numbersOfCars; i++)
                    {
                        if (type == "Passenger")
                        {
                            regNum = RegistrationNumber[random.Next(0, RegistrationNumber.Count)];
                            multimedia = NamedMultimedia[random.Next(0, NamedMultimedia.Count)];
                            airbagCount = AirbagCount[random.Next(0, AirbagCount.Count)];
                            modelCarData[brandName].Add(new Car(regNum, multimedia, airbagCount));
                        }
                        if (type == "Truck")
                        {
                            regNum = RegistrationNumber[random.Next(0, RegistrationNumber.Count)];
                            wheelCount = WheelCount[random.Next(0, WheelCount.Count)];
                            volume = BodyVoume[random.Next(0, BodyVoume.Count)];
                            modelCarData[brandName].Add(new Truck(regNum, wheelCount, volume));
                        }
                        if (type == "Bus")
                        {
                            regNum = RegistrationNumber[random.Next(0, RegistrationNumber.Count)];
                            passengersCount = PassengersCount[random.Next(0, PassengersCount.Count)];
                            while(true)
                            {
                                numbersOfSeat = NumbersOfSeat[random.Next(0, NumbersOfSeat.Count)];
                                if (numbersOfSeat <= passengersCount) break;
                            }
                            modelCarData[brandName].Add(new Bus(regNum, passengersCount, numbersOfSeat));
                        }
                        Thread.Sleep(random.Next(0, 501));
                    }
                });
                
            }
            
        }
        public static List<Vehicles> GetData(string brandName)
        {
            List<Vehicles> data = new List<Vehicles>();
            for(int i = 0; i< modelCarData[brandName].Count; i++) 
            {
                data.Add(modelCarData[brandName][i]);
            }
            return data;

        }
        public static void SetData(List<Vehicles> data, string brandName) 
        {
            if (modelCarData.ContainsKey(brandName))
            {
                modelCarData[brandName].Clear();
                for (int i = 0; i < data.Count; i++)
                {
                    modelCarData[brandName].Add(data[i]);

                }
                countCarInData[brandName] = modelCarData[brandName].Count;
            }
            else
            {
                modelCarData[brandName] = new List<Vehicles>();
                for (int i = 0; i < data.Count; i++)
                {
                    modelCarData[brandName].Add(data[i]);

                }
                countCarInData.Add(brandName, modelCarData[brandName].Count);
            }
        }
        public static int GetProgress(string brandName) 
        {
            if (modelCarData.ContainsKey(brandName))
            {
                int loadedCars = modelCarData[brandName].Count;
                int totalCars = countCarInData[brandName];
                return (int) (((double)loadedCars / totalCars)*100);
                
            }
            else return 0;
        }
        public static void RemoveBrand(string brandName) 
        {
            modelCarData.Remove(brandName);
            countCarInData.Remove(brandName);
        }
        public static void ResetLoader()
        {
            modelCarData.Clear();
            countCarInData.Clear();
        }
    }
    
}
