using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageLogic
{
    public class CreateVehicle
    {
        public enum eVehicleTypes
        {
            ElectricMotorcycle = 1,
            FuelMotorcycle,
            ElectricCar,
            FuelCar,
            Truck,
        }

        internal static Vehicle VehicleCreation(string i_LicensePlateNumber, eVehicleTypes i_VehicleType)
        {
            Vehicle vehicle = null;
            if (!Enum.IsDefined(typeof(eVehicleTypes), i_VehicleType))
            {
                throw new ArgumentException("Error!. " + i_VehicleType + " does not Exist!");
            }

            switch (i_VehicleType)
            {
                case eVehicleTypes.ElectricMotorcycle:
                case eVehicleTypes.FuelMotorcycle:
                    {
                        vehicle = new Motorcycle(i_LicensePlateNumber, i_VehicleType);
                        break;
                    }

                case eVehicleTypes.ElectricCar:
                case eVehicleTypes.FuelCar:
                    {
                        vehicle = new Car(i_LicensePlateNumber, i_VehicleType);
                        break;
                    }

                case eVehicleTypes.Truck:
                    {
                        vehicle = new Truck(i_LicensePlateNumber);
                        break;
                    }
            }

            return vehicle;
        }
    }
}
