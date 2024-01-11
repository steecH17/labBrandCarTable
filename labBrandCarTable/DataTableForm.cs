using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace labBrandCarTable
{
    public partial class DataTableForm : Form
    {
        private BindingSource bindingSource = new BindingSource();
        private BindingList<ICarBrand> brands = new BindingList<ICarBrand>();
        private List<List<Vehicles>> copyVehiclesLoad = new List<List<Vehicles>>();

        private List<Vehicles> reciveServerList = new List<Vehicles>();

        Socket socket = null;

        int countRecive = 0;
        public DataTableForm()
        {
            InitializeComponent();

            InitializeDataGridViewBrands();

            InitializeBindingSource();

            InitializeDataGridViewCars();
            

        }

//----------DATA_GRID_VIEW_BRAND------------------------------------------------------------------------------------------------

        private void InitializeDataGridViewBrands()
        {
            dataGridViewBrand.AutoGenerateColumns = false;
           
            dataGridViewBrand.DataSource = bindingSource;
            
            AddComboBoxColumn();
            AddTextColumn();

        }
        
        private void InitializeBindingSource()
        {
            //brands.Add(new PassengerCarBrand("BMW", "X5M", 250, 350));
            //brands.Add(new PassengerCarBrand("Mercedes", "e200s", 410, 380));
            //brands.Add(new PassengerCarBrand("Huyndai", "Solaris", 116, 220));
            //brands.Add(new TruckBrand("Scania", "Model45G", 1120, 130));
            brands.Add(null);

            bindingSource.DataSource = brands;
            
            
        }
        private void AddTextColumn()
        {
            dataGridViewBrand.Columns.Add("Brand", "Брэнд");
            dataGridViewBrand.Columns.Add("ModelCar", "Модель");
            dataGridViewBrand.Columns.Add("HorsePower", "Лошадиные силы");
            dataGridViewBrand.Columns.Add("MaxSpeed", "Макс скорость");

            foreach (DataGridViewColumn column in dataGridViewBrand.Columns)
            {
                column.DataPropertyName = column.Name;
            }
        }
        private void AddComboBoxColumn()
        {
            DataGridViewComboBoxColumn colTypeComboBox = new DataGridViewComboBoxColumn();
            colTypeComboBox.Name = "Type";
            colTypeComboBox.HeaderText = "Type";
            colTypeComboBox.DisplayIndex = 0;

            colTypeComboBox.Items.Add("Passenger");
            colTypeComboBox.Items.Add("Truck");
            colTypeComboBox.Items.Add("Bus");

            dataGridViewBrand.Columns.Add(colTypeComboBox);
        }
        private void UpdateComboBoxColumn()
        {
            
            int columnInd = dataGridViewBrand.Columns["Type"].Index;
            for (int i = 0; i < brands.Count - 1; i++)
            {
                ICarBrand brand = brands[i];

                DataGridViewRow row = dataGridViewBrand.Rows[brands.IndexOf(brand)];
                if (brand is PassengerCarBrand) row.Cells[columnInd].Value = "Passenger";
                if (brand is TruckBrand) row.Cells[columnInd].Value = "Truck";
                if (brand is BusBrand) row.Cells[columnInd].Value = "Bus";

            }
        }
        private void UpdateColorRow()
        {
            int columnInd = dataGridViewBrand.Columns["Type"].Index;
            for (int i = 0; i < brands.Count-1; i++)
            {
                ICarBrand brand = brands[i];
                DataGridViewRow row = dataGridViewBrand.Rows[brands.IndexOf(brand)];
                if (brand is PassengerCarBrand) row.DefaultCellStyle.BackColor = Color.PowderBlue;
                if (brand is TruckBrand) row.DefaultCellStyle.BackColor = Color.PaleVioletRed;
                if (brand is BusBrand) row.DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;

            }
        }
        private void CreateNewBrandRow(DataGridViewCellEventArgs e)
        {
            int comboBoxIndex = dataGridViewBrand.Columns["Type"].Index;
            if (e.ColumnIndex == comboBoxIndex)
            {
                string valueOfComboBox = dataGridViewBrand.Rows[e.RowIndex].Cells[comboBoxIndex].Value.ToString();
                brands.Remove(null);
                if (valueOfComboBox == "Passenger") brands.Add(new PassengerCarBrand());
                if (valueOfComboBox == "Truck") brands.Add(new TruckBrand());
                if(valueOfComboBox == "Bus") brands.Add(new BusBrand());
            }

            brands.Add(null);
            UpdateComboBoxColumn();
            
            EditBrandRow(e);
        }

        private void EditBrandRow(DataGridViewCellEventArgs e)
        {
            int comboBoxIndex = dataGridViewBrand.Columns["Type"].Index;

            if (e.ColumnIndex == comboBoxIndex)
            {
                string valueOfComboBox = dataGridViewBrand.Rows[e.RowIndex].Cells[comboBoxIndex].Value.ToString();
                ICarBrand editBrand = brands[e.RowIndex];
                if (valueOfComboBox == "Passenger") brands[e.RowIndex] = new PassengerCarBrand(editBrand.Brand, editBrand.ModelCar, editBrand.HorsePower, editBrand.MaxSpeed);
                if (valueOfComboBox == "Truck") brands[e.RowIndex] = new TruckBrand(editBrand.Brand, editBrand.ModelCar, editBrand.HorsePower, editBrand.MaxSpeed);
                if (valueOfComboBox == "Bus") brands[e.RowIndex] = new BusBrand(editBrand.Brand, editBrand.ModelCar, editBrand.HorsePower, editBrand.MaxSpeed);
                brands[e.RowIndex].vehicles.Clear();
                Loader.RemoveBrand(brands[e.RowIndex].Brand);

            }
            else
            {
                string value = dataGridViewBrand.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                switch (e.ColumnIndex)
                {
                    case 1:
                        {
                            brands[e.RowIndex].Brand = value;
                            break;
                        }
                    case 2:
                        {
                            brands[e.RowIndex].ModelCar = value;
                            break;
                        }
                    case 3:
                        {
                            brands[e.RowIndex].HorsePower = int.Parse(value);
                            break;
                        }
                    case 4:
                        {
                            brands[e.RowIndex].MaxSpeed = int.Parse(value);
                            break;
                        }
                }

            }
        }
        
        

        private void DataTableForm_Load(object sender, EventArgs e)
        {
            UpdateComboBoxColumn();
            UpdateColorRow();
        }

        private void dataGridViewBrand_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewCars.Visible) ClearDataGridViewCars();
            if(e.RowIndex == dataGridViewBrand.Rows.Count - 1 && e.ColumnIndex == dataGridViewBrand.Columns["Type"].Index) 
            {
                CreateNewBrandRow(e);
            }
            else if(e.RowIndex == dataGridViewBrand.Rows.Count - 1 && e.ColumnIndex != dataGridViewBrand.Columns["Type"].Index)
            {
                MessageBox.Show("Выберите значение в поле Type!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                brands.Remove(null);
                brands.Add(null);
            }
            else
            {
                
                EditBrandRow(e);
            }
            UpdateComboBoxColumn();
            UpdateColorRow();
        }

        private void dataGridViewBrand_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            dataGridViewBrand.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = 0;
            MessageBox.Show("Некорректно введенные данные!", "DataError", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

//----------BUTTON------------------------------------------------------------------------------------------------------------
        private void buttonExit_Click(object sender, EventArgs e)
        {
            ClearDataGridViewCars();
        }
        private void buttonSave_Click(object sender, EventArgs e)
        {
            DeleteEmtyVehicles();
            SerializeXml();
            MessageBox.Show("Файл успешно сохранен!");
        }
        private void buttonLoad_Click(object sender, EventArgs e)
        {
            DeserializeXml();
            UpdateComboBoxColumn();
            UpdateColorRow();
            LoadVehiclesListToBrands();
        }

//---------DATA_GRID_VIEW_CARS----------------------------------------------------------------------------------------------

        private void InitializeDataGridViewCars()
        {
            dataGridViewCars.Visible = false;
            dataGridViewCars.AutoGenerateColumns = false;
            dataGridViewCars.AllowUserToAddRows = false;
            dataGridViewCars.Columns.Add("", "");
            dataGridViewCars.Columns.Add("", "");
            dataGridViewCars.Columns.Add("", "");

            buttonExit.Visible = false;
        }

        private void AddTextColumnForTrucks()
        {
            dataGridViewCars.Columns.Add("RegistrationNumber", "Регистрационный номер");
            dataGridViewCars.Columns.Add("WheelCount", "Количество Колес");
            dataGridViewCars.Columns.Add("BodyVolume", "Обьем");

            dataGridViewCars.Tag = "Truck";
        }

        private void AddTextColumnForCars()
        {
            dataGridViewCars.Columns.Add("RegistrationNumber", "Регистрационный номер");
            dataGridViewCars.Columns.Add("NamedMultimedia", "Название мультимедиа");
            dataGridViewCars.Columns.Add("AirbagCount", "Количество подушек безопасности");

            dataGridViewCars.Tag = "Passenger";
        }
        private void AddTextColumnForBus() 
        {
            dataGridViewCars.Columns.Add("RegistrationNumber", "Регистрационный номер");
            dataGridViewCars.Columns.Add("PassengersCount", "Общее количество пассажиров");
            dataGridViewCars.Columns.Add("NumbersOfSeat", "Количество сидячих мест");

            dataGridViewCars.Tag = "Bus";
        }
        private void ClearDataGridViewCars()
        {
            dataGridViewCars.Visible=false;
            buttonExit.Visible=false;   
            dataGridViewCars.Rows.Clear();
            dataGridViewCars.Columns.Clear();
        }
        private int IndexSectedRow()
        {
            int indexSelectedRow = -1;
            foreach (DataGridViewRow row in dataGridViewBrand.SelectedRows)
            {
                indexSelectedRow = row.Index;
            }
            return indexSelectedRow;
        }
        private void ShowTableCars(DataGridViewCellMouseEventArgs e)
        {
            dataGridViewCars.Visible = true;
            buttonExit.Visible = true;

            
            int comboBoxIndex = dataGridViewBrand.Columns["Type"].Index;
            string valueOfComboBox = dataGridViewBrand.Rows[e.RowIndex].Cells[comboBoxIndex].Value.ToString();
            
            int indexSelectedRow = IndexSectedRow();
            string brandName = brands[indexSelectedRow].Brand;

            SocketSendMessage(brandName, valueOfComboBox);///!!!!!!!!!!!!!

            //LoadVehiclesListToBrands();

            if(valueOfComboBox == "Passenger")
            {
                AddTextColumnForCars();
                //SetDataCars();
            }
            if(valueOfComboBox == "Truck")
            {
                AddTextColumnForTrucks();
                //SetDataTrucks();
            }
            if(valueOfComboBox == "Bus")
            {
                AddTextColumnForBus();
                //SetDataBus();
            }
            //SynchronizationContext syncContext = SynchronizationContext.Current;
            //Task task = СonnectRecive(syncContext, brandName);
            //LoadData(brandName, valueOfComboBox);
        }
        private void CreateEmtyRowCar(string type)
        {
            int indexSelectedRow = IndexSectedRow();
            

            if(type=="Passenger")
            {
                PassengerCarBrand brand = (PassengerCarBrand)brands[indexSelectedRow];
                brand.vehicles.Add(new Car());
            }

            if(type == "Truck")
            {
                TruckBrand brand = (TruckBrand)brands[indexSelectedRow];
                brand.vehicles.Add(new Truck());
            }

            if (type == "Bus")
            {
                BusBrand brand = (BusBrand)brands[indexSelectedRow];
                brand.vehicles.Add(new Bus());
            }
            dataGridViewCars.Rows.Add();
        }
        private void LoadCars()
        {
            int indexSelectedRow = IndexSectedRow();

            PassengerCarBrand brand = (PassengerCarBrand)brands[indexSelectedRow];
            string brandName = brand.Brand;
            
            List<Vehicles> dataCars = Loader.GetData(brandName);

            brand.vehicles = dataCars;
            if (dataCars.Count > 0)
            {
                foreach (Car car in dataCars)
                {
                    dataGridViewCars.Rows.Add(car.RegistrationNumber, car.NamedMultimedia, car.AirbagCount.ToString());
                }
            }
        }
        private void LoadTrucks()
        {
            int indexSelectedRow = IndexSectedRow();
;

            TruckBrand brand = (TruckBrand)brands[indexSelectedRow];
            string brandName = brand.Brand;
            
            List<Vehicles> dataTrucks = Loader.GetData(brandName);
            brand.vehicles = dataTrucks;    
            if (dataTrucks.Count > 0)
            {
                foreach (Truck truck in dataTrucks)
                {
                    dataGridViewCars.Rows.Add(truck.RegistrationNumber, truck.WheelCount.ToString(), truck.BodyVolume.ToString());
                }
            }
        }
        private void LoadBus()
        {
            int indexSelectedRow = IndexSectedRow();

            BusBrand brand = (BusBrand)brands[indexSelectedRow];
            string brandName = brand.Brand;

            List<Vehicles> dataBus = Loader.GetData(brandName);
            brand.vehicles = dataBus;
            if (dataBus.Count > 0)
            {
                foreach (Bus bus in dataBus)
                {
                    dataGridViewCars.Rows.Add(bus.RegistrationNumber, bus.PassengersCount.ToString(), bus.NumbersOfSeat.ToString());
                }
            }
        }
        private void SetDataTrucks()
        {
            int indexSelectedRow = IndexSectedRow();

            DeleteNullVehicles(indexSelectedRow);

            TruckBrand brand = (TruckBrand)brands[indexSelectedRow];
            string brandName = brand.Brand;
            if (brand.vehicles.Count > 0)
            {
                Loader.SetData(brand.vehicles, brandName);
            }
        }
        private void SetDataBus()
        {
            int indexSelectedRow = IndexSectedRow();

            DeleteNullVehicles(indexSelectedRow);

            BusBrand brand = (BusBrand)brands[indexSelectedRow];
            string brandName = brand.Brand;
            if (brand.vehicles.Count > 0)
            {
                Loader.SetData(brand.vehicles, brandName);
            }
        }
        private void SetDataCars()
        {
            int indexSelectedRow = IndexSectedRow();

            DeleteNullVehicles(indexSelectedRow);

            PassengerCarBrand brand = (PassengerCarBrand)brands[indexSelectedRow];
            string brandName = brand.Brand;
            if (brand.vehicles.Count > 0)
            {
                Loader.SetData(brand.vehicles, brandName);

            }
        }
        private async void LoadData(string brandName, string type)
        {
            ProgressBarLoadingForm progressBarLoadingForm = new ProgressBarLoadingForm();
            progressBarLoadingForm.BrandName = brandName;
            progressBarLoadingForm.Socket = socket;
            progressBarLoadingForm.Show();

            await Loader.Load(brandName, type);

            progressBarLoadingForm.Close();

            if(type=="Passenger") LoadCars();
            if (type == "Truck") LoadTrucks();
            if (type == "Bus") LoadBus();

            CreateEmtyRowCar(type);
            dataGridViewBrand.Enabled = true;
            buttonLoad.Enabled = true;
            buttonSave.Enabled = true;
            buttonExit.Enabled = true;
        }
        private void DeleteEmtyVehicles()
        {

            foreach(ICarBrand brand in brands)
            {
                if(brand is PassengerCarBrand)
                {
                    PassengerCarBrand carBrand = (PassengerCarBrand)brand;
                    for(int i = 0; i< carBrand.vehicles.Count; i++) 
                    {
                        if (carBrand.vehicles[i].RegistrationNumber == "null") carBrand.vehicles.RemoveAt(i);
                    }
                }
                if(brand is TruckBrand)
                {
                    TruckBrand carBrand = (TruckBrand)brand;
                    for (int i = 0; i < carBrand.vehicles.Count; i++)
                    {
                        if (carBrand.vehicles[i].RegistrationNumber == "null") carBrand.vehicles.RemoveAt(i);
                    }
                }
                if (brand is BusBrand)
                {
                    BusBrand carBrand = (BusBrand)brand;
                    for (int i = 0; i < carBrand.vehicles.Count; i++)
                    {
                        if (carBrand.vehicles[i].RegistrationNumber == "null") carBrand.vehicles.RemoveAt(i);
                    }
                }
            }
        }
        private void DeleteNullVehicles(int indexSelectedRow)
        {

            if (dataGridViewCars.Tag.ToString() == "Truck")
            {
                TruckBrand brand = (TruckBrand)brands[indexSelectedRow];
                if ((brand.vehicles.Count) > 0 && brand.vehicles[brand.vehicles.Count -1].RegistrationNumber == "null") brand.vehicles.RemoveAt(brand.vehicles.Count - 1);

            }
            if (dataGridViewCars.Tag.ToString() == "Passenger")
            {
                PassengerCarBrand brand = (PassengerCarBrand)brands[indexSelectedRow];
                if ((brand.vehicles.Count) > 0 && brand.vehicles[brand.vehicles.Count - 1].RegistrationNumber == "null") brand.vehicles.RemoveAt(brand.vehicles.Count - 1);
            }
            if (dataGridViewCars.Tag.ToString() == "Bus")
            {
                BusBrand brand = (BusBrand)brands[indexSelectedRow];
                if ((brand.vehicles.Count) > 0 && brand.vehicles[brand.vehicles.Count - 1].RegistrationNumber == "null") brand.vehicles.RemoveAt(brand.vehicles.Count - 1);
            }

        }

        private void CreateNewRowTableCars(DataGridViewCellEventArgs e)
        {
            int indexSelectedRow = IndexSectedRow();

            int comboBoxIndex = dataGridViewBrand.Columns["Type"].Index;
            string valueOfComboBox = dataGridViewBrand.Rows[indexSelectedRow].Cells[comboBoxIndex].Value.ToString();

            CreateEmtyRowCar(valueOfComboBox);

            EditCarsRow(e);
        }
        private void EditCarsRow(DataGridViewCellEventArgs e)
        {
            string value = dataGridViewCars.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();

            int indexSelectedRow = IndexSectedRow();

            ICarBrand brand = null;

            switch (e.ColumnIndex)
            {
                case 0:
                    {
                        if (dataGridViewCars.Tag.ToString() == "Passenger")
                        {
                            brand = (PassengerCarBrand)brands[indexSelectedRow];
                            Car car = (Car)brand.vehicles[e.RowIndex];
                            car.RegistrationNumber = value;
                        }

                        if (dataGridViewCars.Tag.ToString() == "Truck")
                        {
                            brand = (TruckBrand)brands[indexSelectedRow];
                            Truck truck = (Truck)brand.vehicles[e.RowIndex];
                            truck.RegistrationNumber = value;
                        }

                        if (dataGridViewCars.Tag.ToString() == "Bus")
                        {
                            brand = (BusBrand)brands[indexSelectedRow];
                            Bus bus = (Bus)brand.vehicles[e.RowIndex];
                            bus.RegistrationNumber = value;
                        }
                        break;
                    }
                case 1:
                    {
                        if (dataGridViewCars.Tag.ToString() == "Passenger")
                        {
                            brand = (PassengerCarBrand)brands[indexSelectedRow];
                            Car car = (Car)brand.vehicles[e.RowIndex];
                            car.NamedMultimedia = value;
                        }

                        if (dataGridViewCars.Tag.ToString() == "Truck")
                        {
                            brand = (TruckBrand)brands[indexSelectedRow];
                            Truck truck = (Truck)brand.vehicles[e.RowIndex];
                            truck.WheelCount = int.Parse(value);
                        }

                        if (dataGridViewCars.Tag.ToString() == "Bus")
                        {
                            brand = (BusBrand)brands[indexSelectedRow];
                            Bus bus = (Bus)brand.vehicles[e.RowIndex];
                            bus.PassengersCount = int.Parse(value);
                        }
                        break;
                    }
                case 2:
                    {
                        if (dataGridViewCars.Tag.ToString() == "Passenger")
                        {
                            brand = (PassengerCarBrand)brands[indexSelectedRow];
                            Car car = (Car)brand.vehicles[e.RowIndex];
                            car.AirbagCount = int.Parse(value);
                        }

                        if (dataGridViewCars.Tag.ToString() == "Truck")
                        {
                            brand = (TruckBrand)brands[indexSelectedRow];
                            Truck truck = (Truck)brand.vehicles[e.RowIndex];
                            truck.BodyVolume = int.Parse(value);
                        }

                        if (dataGridViewCars.Tag.ToString() == "Bus")
                        {
                            brand = (BusBrand)brands[indexSelectedRow];
                            Bus bus = (Bus)brand.vehicles[e.RowIndex];
                            bus.NumbersOfSeat = int.Parse(value);
                        }

                        break;
                    }

            }
        }

        private void dataGridViewBrand_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //dataGridViewBrand.Enabled = false;
            //buttonLoad.Enabled = false;
            //buttonSave.Enabled = false;
            //buttonExit.Enabled = false;
            ClearDataGridViewCars();
            
            if (e.RowIndex >=0 && dataGridViewBrand.SelectedRows.Count == 1 && e.RowIndex!= dataGridViewBrand.RowCount-1)
            {
                //ShowTableCars(e);
                SocketConnect();

                //progressBar1.Minimum = 0;
                //progressBar1.Maximum = 100;
                //progressBar1.Visible = true;

                countRecive = 0;
                int indexSelectedRow = IndexSectedRow();
                int comboBoxIndex = dataGridViewBrand.Columns["Type"].Index;
                string valueOfComboBox = dataGridViewBrand.Rows[e.RowIndex].Cells[comboBoxIndex].Value.ToString();

                string brandName = brands[indexSelectedRow].Brand;

                SocketSendMessage(brandName, valueOfComboBox);
                //byte[] bytes = new byte[4096];
                //int bytesRec = socket.Receive(bytes);
                //int totalCountCars = int.Parse(Encoding.UTF8.GetString(bytes, 0, bytesRec));
                //MessageBox.Show(totalCountCars.ToString());

                SynchronizationContext syncContext = SynchronizationContext.Current;
                Task task = СonnectRecive(syncContext, e);
                
            }
            else if(e.RowIndex == dataGridViewBrand.RowCount -1)
            {
                dataGridViewBrand.Enabled = true;
                buttonLoad.Enabled = true;
                buttonSave.Enabled = true;
                buttonExit.Enabled = true;
            }
            else
            {
                ClearDataGridViewCars();
            }

        }
        
        private bool CorrectDataInCell(DataGridViewCellEventArgs e)
        {
            if (dataGridViewCars.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null) return true;

            string value = dataGridViewCars.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();

            if (dataGridViewCars.Tag.ToString() == "Passenger")
            {
                if (e.ColumnIndex == dataGridViewCars.Columns["AirbagCount"].Index)
                {
                    
                    if (int.TryParse(value, out int result))
                    {
                        return true;
                    }
                    else return false;
                }
                else return true;
            }
            if(dataGridViewCars.Tag.ToString() == "Truck")
            {
                if (e.ColumnIndex == dataGridViewCars.Columns["WheelCount"].Index || e.ColumnIndex == dataGridViewCars.Columns["BodyVolume"].Index)
                {
                    if (int.TryParse(value, out int result))
                    {
                        return true;
                    }
                    else return false;
                }
                
                else return true;
            }
            if (dataGridViewCars.Tag.ToString() == "Bus")
            {
                if (e.ColumnIndex == dataGridViewCars.Columns["PassengersCount"].Index || e.ColumnIndex == dataGridViewCars.Columns["NumbersOfSeat"].Index)
                {
                    if (int.TryParse(value, out int result))
                    {
                        return true;
                    }
                    else return false;
                }

                else return true;
            }
            return true;
        }

        private void SerializeXml()
        {

            List<CCarBrand> data = new List<CCarBrand>();
            foreach(ICarBrand brand in brands)
            {
                data.Add(brand as  CCarBrand);
            }

            try 
            {
                
                XmlSerializer serializer = new XmlSerializer(typeof(List<CCarBrand>), new Type[]
                {
                    typeof(PassengerCarBrand),
                    typeof(TruckBrand),
                    typeof(BusBrand),
                    typeof(Vehicles),
                    typeof(Truck),
                    typeof(Car),
                    typeof(Bus)
                });
                FileStream writer = new FileStream("DataBrands.xml", FileMode.Create);
                
                serializer.Serialize(writer, data);
                writer.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void DeserializeXml()
        {
            Loader.ResetLoader();
            brands.Clear();
            List<CCarBrand> data = new List<CCarBrand>();
            List<Vehicles> vehiclesData = new List<Vehicles>();
            int indexOfBrand=0;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<CCarBrand>), new Type[]
                {
                    typeof(PassengerCarBrand),
                    typeof(TruckBrand),
                    typeof(BusBrand),
                    typeof(Vehicles),
                    typeof(Truck),
                    typeof(Car),
                    typeof(Bus)
                });
                FileStream writer = new FileStream("DataBrands.xml", FileMode.OpenOrCreate);
                data = serializer.Deserialize(writer) as List<CCarBrand>;
                writer.Close();
                
                foreach(CCarBrand brand in data) 
                {

                    if (brand is PassengerCarBrand)
                    {
                        brands.Add(new PassengerCarBrand(brand.Brand, brand.ModelCar, brand.HorsePower, brand.MaxSpeed, brand.vehicles));
                        copyVehiclesLoad.Add(brand.vehicles);
                    }
                    if (brand is TruckBrand)
                    {
                        brands.Add(new TruckBrand(brand.Brand, brand.ModelCar, brand.HorsePower, brand.MaxSpeed, brand.vehicles));
                        copyVehiclesLoad.Add(brand.vehicles);
                    }
                    if (brand is BusBrand)
                    {
                        brands.Add(new BusBrand(brand.Brand, brand.ModelCar, brand.HorsePower, brand.MaxSpeed, brand.vehicles));
                        copyVehiclesLoad.Add(brand.vehicles);
                    }
                    indexOfBrand++;
                }
                brands.Add(null);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void LoadVehiclesListToBrands()
        {
            if(copyVehiclesLoad.Count > 0) 
            {
                for(int i = 0; i< copyVehiclesLoad.Count; i++) 
                {
                    if (brands[i].vehicles!=null) brands[i].vehicles = copyVehiclesLoad[i];
                }
                copyVehiclesLoad.Clear();
            }
        }

        private void dataGridViewCars_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (CorrectDataInCell(e))
            {
                if (e.RowIndex == dataGridViewCars.Rows.Count - 1)
                {
                    CreateNewRowTableCars(e);
                }
                else
                {
                    EditCarsRow(e);
                }
            }
            else
            {
                dataGridViewCars.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = 0;
                MessageBox.Show("Некорректно введенные данные!", "DataError", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            SocketConnect();
        }
        private void SocketConnect()
        {
            IPHostEntry ipHost = Dns.GetHostEntry("localhost");
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 11000);
           
            socket = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket.Connect(ipEndPoint);
                //MessageBox.Show($"Подключение к {socket.RemoteEndPoint} установлено");
            }
            catch (SocketException)
            {
                MessageBox.Show($"Не удалось установить подключение с {socket.RemoteEndPoint}");
            }
        }
        private void SocketSendMessage(string brandName, string type)
        {
            byte[] buf = new byte[1024];

            string xmlData = StringXMLData(brandName, type);
            byte[] msg = Encoding.ASCII.GetBytes(xmlData);

            // Отправляем данные через сокет
            int bytesSent = socket.Send(msg);
        }

        private string StringXMLData(string brandName, string type)
        {
            List<string> dataList = new List<string>() { brandName, type };
            XmlSerializer serializer = new XmlSerializer(typeof(List<string>));
            XmlDocument xmlDocument = new XmlDocument();
            using (MemoryStream ms = new MemoryStream())
            {
                serializer.Serialize(ms, dataList);

                ms.Position = 0; // переходим в начало потока
                // Загружаем данные из потока в XML документ
                xmlDocument.Load(ms);
            }

            string xmlData;
            using (StringWriter stringWriter = new StringWriter())
            {
                xmlDocument.Save(stringWriter);
                // Получаем строку с XML данными
                xmlData = stringWriter.ToString();
            }
            return xmlData;
        }
        private void ReceiveModels(string xmlData)
        {
            // Создание XML-документа и загрузка данных
            XmlSerializer serializer = new XmlSerializer(typeof(List<Vehicles>), new Type[]
               {
                    typeof(Vehicles),
                    typeof(Truck),
                    typeof(Car),
                    typeof(Bus)
               });
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xmlData);
            using (StringReader stringReader = new StringReader(xmlDocument.OuterXml))
            {
                reciveServerList = (List<Vehicles>)serializer.Deserialize(stringReader);
            }
        }

        private async Task СonnectRecive(SynchronizationContext syncContext, DataGridViewCellMouseEventArgs e)
        {
            dataGridViewBrand.Enabled = false;
            buttonLoad.Enabled = false;
            buttonSave.Enabled = false;
            buttonExit.Enabled = false;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = 100;
            progressBar1.Visible = true;
            label1.Visible = true;
            await Task.Run(() =>
            {
                byte[] bytes = new byte[4096];

                int indexSelectedRow = IndexSectedRow();
                int comboBoxIndex = dataGridViewBrand.Columns["Type"].Index;
                string valueOfComboBox = dataGridViewBrand.Rows[e.RowIndex].Cells[comboBoxIndex].Value.ToString();

                string brandName = brands[indexSelectedRow].Brand;

                //SocketSendMessage(brandName, valueOfComboBox);
                try
                {
                    
                    //int bytesRec = socket.Receive(bytes);
                    //int totalCountCars = int.Parse(Encoding.UTF8.GetString(bytes, 0, bytesRec));
                    //bytes = new byte[4096];
                    //MessageBox.Show(totalCountCars.ToString());
                    string xmlData;
                    while (true)
                    {
                        int bytesRec = socket.Receive(bytes);

                        if (bytesRec > 0)
                        {
                            // Конвертация полученных байтов в строку XML

                            xmlData = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                            ReceiveModels(xmlData);
                            Control control = new Control();
                            syncContext.Post(_ =>
                            {
                                ClearDataGridViewCars();
                                CreateCarTable();
                            }, null);

                        }
                        else break;
                    }
                    

                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                    
                    


                }
                catch (ArgumentNullException ane)
                {
                    MessageBox.Show("ArgumentNullException : " + ane.ToString(), "ArgumentNullException", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (SocketException se)
                {
                    MessageBox.Show("SocketException : {0}" + se.ToString(), "SocketException", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception en)
                {
                    MessageBox.Show("Unexpected exception : {0}" + en.ToString(), "Unexpected exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


            });
            dataGridViewCars.Visible = true;
            progressBar1.Value = 100;
            progressBar1.Visible = false;
            progressBar1.Value = 0;
            label1.Visible = false;
            dataGridViewBrand.Enabled = true;
            buttonLoad.Enabled = true;
            buttonSave.Enabled = true;
            buttonExit.Enabled = true;
            buttonExit.Visible = true;
        }

        private void CreateCarTable()
        {
            progressBar1.Value = (int)(((double)reciveServerList.Count / 21) * 100);

            //dataGridViewCars.Visible = true;
            //buttonExit.Visible = true;
            int comboBoxIndex = dataGridViewBrand.Columns["Type"].Index;
            int indexSelectedRow = IndexSectedRow();
            string valueOfComboBox = dataGridViewBrand.Rows[indexSelectedRow].Cells[comboBoxIndex].Value.ToString();

            
            string brandName = brands[indexSelectedRow].Brand;

            

            //LoadVehiclesListToBrands();

            if (valueOfComboBox == "Passenger")
            {
                AddTextColumnForCars();
                //SetDataCars();
            }
            if (valueOfComboBox == "Truck")
            {
                AddTextColumnForTrucks();
                //SetDataTrucks();
            }
            if (valueOfComboBox == "Bus")
            {
                AddTextColumnForBus();
                //SetDataBus();
            }

            if (reciveServerList.Count > 0)
            {
                foreach (Vehicles vehicles in reciveServerList)
                {
                    if(vehicles is Car)
                    {
                        Car car = (Car)vehicles;
                        dataGridViewCars.Rows.Add(car.RegistrationNumber, car.NamedMultimedia, car.AirbagCount.ToString());
                    }
                    if(vehicles is Truck)
                    {
                        Truck truck = (Truck)vehicles;
                        dataGridViewCars.Rows.Add(truck.RegistrationNumber, truck.WheelCount.ToString(), truck.BodyVolume.ToString());
                    }
                    if(vehicles is Bus)
                    {
                        Bus bus = (Bus)vehicles;
                        dataGridViewCars.Rows.Add(bus.RegistrationNumber, bus.PassengersCount.ToString(), bus.NumbersOfSeat.ToString());
                    }
                    
                }
            }
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }
    }
}
