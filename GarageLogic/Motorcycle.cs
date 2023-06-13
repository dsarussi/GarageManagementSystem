using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageLogic
{
    internal class Motorcycle : Vehicle
    {
        private const Fuel.eFuelType k_MotorcycleFuelType = Fuel.eFuelType.Octan98;
        private const float k_MaximumBatteryTime = 2.3f;
        private const float k_MaximumAmountOfFuel = 5.8f;
        private const int k_NumberOfLicenseType = 4;
        private const int k_NumberOfMotorcycleWheels = 2;
        private const float k_MaximumWheelAirPressure = 30;
        private eMotorcycleLicense m_LicenseType;
        private int m_EngineCapacity;

        private enum eMotorcycleLicense
        {
            A = 1,
            A1,
            AA,
            B,
        }

        private enum eMotorcycleRequiredParameters
        {
            licenseType = 7,
            enginCapacity,
        }

        internal Motorcycle(string i_LicensePlateNumber, CreateVehicle.eVehicleTypes i_VehicleType)
            : base(i_LicensePlateNumber)
        {
            if(i_VehicleType == CreateVehicle.eVehicleTypes.ElectricMotorcycle)
            {
                m_PowerSource = new Battery(k_MaximumBatteryTime);
            }
            else if(i_VehicleType == CreateVehicle.eVehicleTypes.FuelMotorcycle)
            {
                m_PowerSource = new Fuel(k_MaximumAmountOfFuel, k_MotorcycleFuelType);
            }
        }

        private string engineCapcity
        {
            get
            {
                return m_EngineCapacity.ToString();
            }

            set
            {
                int parsedValue;

                if(!int.TryParse(value, out parsedValue))
                {
                    throw new FormatException("Invalid input. Engine capacity must be an integer number only");
                }

                if(parsedValue <= 0)
                {
                    throw new FormatException("Invalid input. Engine capacity must be greater than zero");
                }

                m_EngineCapacity = parsedValue;
            }
        }

        private string licenseType
        {
            get
            {
                return m_LicenseType.ToString();
            }

            set
            {
                int parsedValue;
                if(!int.TryParse(value, out parsedValue))
                {
                    throw new FormatException("Invalid input. License type should be a number from the list");
                }

                if(!Enum.IsDefined(typeof(eMotorcycleLicense), parsedValue))
                {
                    throw new ValueOutOfRangeException(1, k_NumberOfLicenseType, "Invalid input. License type should be chosen from 1 - " + k_NumberOfLicenseType);
                }

                Enum.TryParse(value, out m_LicenseType);
            }
        }

        protected internal override void AddFunctionToEnergy(float i_AmountToAdd)
        {
            m_PowerSource.AddPowerToSource(i_AmountToAdd);
        }

        public override List<string> GetVehicleFullDetailes()
        {
            List<string> vehicleDetailes = GetVehicleBaseDetails();
            vehicleDetailes.Add("License type: " + m_LicenseType.ToString());
            vehicleDetailes.Add("Engine Capacity: " + m_EngineCapacity.ToString());
            if (m_PowerSource is Fuel)
            {
                vehicleDetailes.Add("Power source type: fuel");
            }
            else if (m_PowerSource is Battery)
            {
                vehicleDetailes.Add("Power source type: battery");
            }

            return vehicleDetailes;
        }

        public override List<string> GetVehicleRequiredParameters()
        {
            List<string> baseParameters = GetBaseVehicleRequiredParameters();
            string chooseLicenseTypeMessege;

            if (m_PowerSource is Fuel)
            {
                Fuel.GetParametersOfFuel(baseParameters);
            }
            else if (m_PowerSource is Battery)
            {
                Battery.GetParametersOfBattery(baseParameters);
            }

            chooseLicenseTypeMessege = string.Format(@"motorcycle license:
1. A
2. A1
3. AA
4. B");
            baseParameters.Add(chooseLicenseTypeMessege);
            baseParameters.Add("engine capacity");

            return baseParameters;
        }

        public override void SetVehicleRequiredParameters(string i_ParameterToSet, int i_ParameterIndex)
        {
            eVehicleParameters vehicleParameterIndex;
            eMotorcycleRequiredParameters motorcycleParameterIndex;

            Enum.TryParse<eMotorcycleRequiredParameters>(i_ParameterIndex.ToString(), out motorcycleParameterIndex);
            if (Enum.TryParse<eVehicleParameters>(i_ParameterIndex.ToString(), out vehicleParameterIndex))
            {
                SetBaseVehicleRequiredParametersWrapper(i_ParameterToSet, i_ParameterIndex, k_NumberOfMotorcycleWheels, k_MaximumWheelAirPressure);
            }

            switch (motorcycleParameterIndex)
            {
                case eMotorcycleRequiredParameters.licenseType:
                    {
                        licenseType = i_ParameterToSet;
                        break;
                    }

                case eMotorcycleRequiredParameters.enginCapacity:
                    {
                        engineCapcity = i_ParameterToSet;
                        break;
                    }
            }
        }

        protected internal override void UpdatePercentageOfEnergy()
        {
            float fullCapacityValue;

            if (m_PowerSource is Fuel)
            {
                fullCapacityValue = k_MaximumAmountOfFuel;
            }
            else
            {
                fullCapacityValue = k_MaximumBatteryTime;
            }

            m_PrecentageOfEnergyLeft = m_PowerSource.RemainingEnergy / fullCapacityValue * 100;
        }
    }
}
