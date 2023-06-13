using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageLogic
{
    public class GarageManagment
    {
        private Dictionary<int, Vehicle> m_VehicleDicttionary = new Dictionary<int, Vehicle>();

        public Vehicle VehicleCreation(string i_LicensePlate, CreateVehicle.eVehicleTypes i_VehicleTypes)
        {
            Vehicle newVehicle;

            if(m_VehicleDicttionary.TryGetValue(i_LicensePlate.GetHashCode(), out newVehicle))
            {
                throw new ArgumentException("Error!. Plate: " + i_LicensePlate + "already Exists!");
            }

            newVehicle = CreateVehicle.VehicleCreation(i_LicensePlate, i_VehicleTypes);

            return newVehicle;
        }

        public void AddVehicleToTheGarage(ref Vehicle i_NewVehicle)
        {
            Vehicle vehicleInDictionary;

            if (m_VehicleDicttionary.TryGetValue(i_NewVehicle.GetHashCode(), out vehicleInDictionary))
            {
                throw new ArgumentException("Error!. The car with Plate: " + vehicleInDictionary.LicenseNumber + " Already Exists!");
            }

            m_VehicleDicttionary.Add(i_NewVehicle.GetHashCode(), i_NewVehicle);
        }

        public Dictionary<int, Vehicle> VehicleDictionary
        {
            get
            {
                return m_VehicleDicttionary;
            }
        }

        public bool IsVehicleAlreadyExists(string i_LicensePlate)
        {
            Vehicle vehicleToSearch;

            return m_VehicleDicttionary.TryGetValue(i_LicensePlate.GetHashCode(), out vehicleToSearch);
        }

        public void VehicleStatusChange(string i_LicensePlate, Vehicle.eRepairStatus i_RepairStatus)
        {
            Vehicle vehicleStatusToChange;

            if(!m_VehicleDicttionary.TryGetValue(i_LicensePlate.GetHashCode(), out vehicleStatusToChange))
            {
                throw new ArgumentException("Error!. The vehicle with Plate: " + i_LicensePlate + "Does not exist!");
            }

            if (!Enum.IsDefined(typeof(Vehicle.eRepairStatus), i_RepairStatus))
            {
                throw new ArgumentException("Error!. Invalid Repair Status");
            }

            vehicleStatusToChange.Status = i_RepairStatus;
        }

        public List<Vehicle> GetAllVehicles()
        {
            List<Vehicle> returnVehicles = new List<Vehicle>();

            foreach(KeyValuePair<int, Vehicle> keyValuePair in m_VehicleDicttionary)
            {
                returnVehicles.Add(keyValuePair.Value);
            }

            return returnVehicles;
        }

        public List<Vehicle> GetAllVehicleInSpecificStatus(Vehicle.eRepairStatus i_RepairStatus)
        {
            List<Vehicle> returnVehiclesInSpecificStatus = new List<Vehicle>();

            if (!Enum.IsDefined(typeof(Vehicle.eRepairStatus), i_RepairStatus))
            {
                throw new ArgumentException("Error!. Status: " + i_RepairStatus + " Does not Exist!!");
            }

            foreach(KeyValuePair<int, Vehicle> keyValuePair in m_VehicleDicttionary)
            {
                if (keyValuePair.Value.Status == i_RepairStatus)
                {
                    returnVehiclesInSpecificStatus.Add(keyValuePair.Value);
                }
            }

            return returnVehiclesInSpecificStatus;
        }

        public void AddAirToMaximum(string i_LicensePlate)
        {
            Vehicle vehicleToAddAir;
            m_VehicleDicttionary.TryGetValue(i_LicensePlate.GetHashCode(), out vehicleToAddAir);

            vehicleToAddAir.FillWheelAir();
        }

        public void AddFuel(string i_LicensePlate, float i_AmountFuelToAdd, Fuel.eFuelType i_FuelType)
        {
            Vehicle vehicleToFillTo;
            Fuel vehicleFuelGasTank;

            if(!m_VehicleDicttionary.TryGetValue(i_LicensePlate.GetHashCode(), out vehicleToFillTo))
            {
                throw new ArgumentException("Erroe The Vehicle with plat: " + i_LicensePlate + "Doesn't exist!");
            }

            vehicleFuelGasTank = vehicleToFillTo.GetPowerSouce() as Fuel;

            if (vehicleToFillTo == null)
            {
                throw new ArgumentException("Error!. The Vehicle Doesnt work on Fuel!!");
            }

            if (!Enum.IsDefined(typeof(Fuel.eFuelType), i_FuelType))
            {
                throw new ArgumentException("Error!. This kind of Fuel Does Exists Please select from the list!");
            }

            if (vehicleFuelGasTank.FuelType != i_FuelType)
            {
                throw new ArgumentException(i_FuelType + "Does does not fit for the vehicle!, " + vehicleFuelGasTank.FuelType + "is the right fit!");
            }

            vehicleToFillTo.AddFunctionToEnergy(i_AmountFuelToAdd);
            vehicleToFillTo.UpdatePercentageOfEnergy();
        }

        public void ChargeTheBattery(string i_LicensePlate, float i_AmountOfEnergy)
        {
            Vehicle vehicleToCharge;
            Battery vehicleBattery;

            if (!m_VehicleDicttionary.TryGetValue(i_LicensePlate.GetHashCode(), out vehicleToCharge))
            {
                throw new ArgumentException("Error!. The vehicle with plate: " + i_LicensePlate + "Does not Exist!");
            }

            vehicleBattery = vehicleToCharge.GetPowerSouce() as Battery;
            if (vehicleBattery == null)
            {
                throw new ArgumentException("Error!. The vehicle does not work out batteries!");
            }

            vehicleToCharge.AddFunctionToEnergy(i_AmountOfEnergy);
        }
    }
}
