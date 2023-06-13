using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageLogic
{
    internal class Truck : Vehicle
    {
        private const int k_NumberOfTruckWheels = 16;
        private const float k_MaximumAmountOfFuel = 130;
        private const float k_MaximumWheelAirPressure = 25;
        private const Fuel.eFuelType k_FuelType = Fuel.eFuelType.Soler;
        private float m_CargoVolume;
        private bool m_IsTransportingFrozenMaterials;

        private enum eTruckRequiredParameters
        {
            cargoVolume = 7,
            isTransportingFrozenMaterials,
        }

        internal Truck(string i_LicensePlateNumber)
            : base(i_LicensePlateNumber)
        {
            m_PowerSource = new Fuel(k_MaximumAmountOfFuel, k_FuelType);
        }

        private string isTransportingFrozenMaterials
        {
            get
            {
                return m_IsTransportingFrozenMaterials.ToString();
            }

            set
            {
                if(value.Length == 0)
                {
                    throw new ArgumentException("Invalid input. is transporting frozen materials cant be empty");
                }

                if(value != "Y" && value != "N" && value != "y" && value != "n")
                {
                    throw new ArgumentException("Invalid input. is transporting frozen materials answer should be Y/N");
                }

                m_IsTransportingFrozenMaterials = value == "Y" || value == "y";
            }
        }

        private string cargoVolume
        {
            get
            {
                return m_CargoVolume.ToString();
            }

            set
            {
                float parsedValue;
                if (!float.TryParse(value, out parsedValue))
                {
                    throw new FormatException("Invalid input. Cargo volume should be rational number");
                }

                if (parsedValue < 0)
                {
                    throw new ArgumentException("Invalid input. Cargo volume can't be negative");
                }

                m_CargoVolume = parsedValue;
            }
        }

        protected internal override void AddFunctionToEnergy(float i_AmountToAdd)
        {
            m_PowerSource.AddPowerToSource(i_AmountToAdd);
        }

        protected internal override void UpdatePercentageOfEnergy()
        {
            m_PrecentageOfEnergyLeft = m_PowerSource.RemainingEnergy / k_MaximumAmountOfFuel * 100;
        }

        public override List<string> GetVehicleFullDetailes()
        {
            List<string> vehicleDetailes = GetVehicleBaseDetails();

            vehicleDetailes.Add("Cargo volume: " + m_CargoVolume.ToString());
            vehicleDetailes.Add("Is transporting hazardous materials: " + m_IsTransportingFrozenMaterials.ToString());
            vehicleDetailes.Add("Power source type: fuel");
            vehicleDetailes.Add("Fuel type: " + k_FuelType);

            return vehicleDetailes;
        }

        public override List<string> GetVehicleRequiredParameters()
        {
            List<string> truckParameters = GetBaseVehicleRequiredParameters();
            Fuel.GetParametersOfFuel(truckParameters);
            truckParameters.Add("cargo volume");
            truckParameters.Add("is transporting hazardous materials (Y/N)");

            return truckParameters;
        }
        
        // $G$ DSN-999 (-3) why not use the method from the base class?
        internal List<string> GetBaseVehicleRequiredParameters()
        {
            List<string> parameters = new List<string>();

            parameters.Add("vehicle model");
            parameters.Add("owner name");
            parameters.Add("owner phone number");
            parameters.Add("wheel manufactor");
            parameters.Add("wheel air presure");

            return parameters;
        }

        public override void SetVehicleRequiredParameters(string i_ParameterToSet, int i_ParameterIndex)
        {
            eVehicleParameters vehicleParameterIndex;
            eTruckRequiredParameters truckParameterIndex;

            Enum.TryParse<eTruckRequiredParameters>(i_ParameterIndex.ToString(), out truckParameterIndex);
            if (Enum.TryParse<eVehicleParameters>(i_ParameterIndex.ToString(), out vehicleParameterIndex))
            {
                SetBaseVehicleRequiredParametersWrapper(i_ParameterToSet, i_ParameterIndex, k_NumberOfTruckWheels, k_MaximumWheelAirPressure);
            }

            switch (truckParameterIndex)
            {
                case eTruckRequiredParameters.cargoVolume:
                    {
                        cargoVolume = i_ParameterToSet;
                        break;
                    }

                case eTruckRequiredParameters.isTransportingFrozenMaterials:
                    {
                        isTransportingFrozenMaterials = i_ParameterToSet;
                        break;
                    }
            }
        }
    }
}
