using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageLogic
{
    public class Car : Vehicle
    {
        private const int r_NumberOfWheels = 4;
        private const float r_AirPressure = 29;
        private const float r_MaxBatteryEnergy = 2.6f;
        private const float r_MaxGasTankCapacity = 48;
        private const int r_AmountOfColors = 4;
        private const int r_NumberOfMaxDoors = 5;
        private const Fuel.eFuelType k_FuelType = Fuel.eFuelType.Octan95;
        private eNumberOfDoors m_NumberOfDoors;
        private eColorOfCar m_Colors;

        private enum eColorOfCar
        {
            Red = 1,
            White,
            Black,
            Blue,
        }

        private enum eCarParameters
        {
            CarColors = 7,
            AmountOfDoors,
        }

        private enum eNumberOfDoors
        {
            Two = 2,
            Three,
            Four,
            Five,
        }

        internal Car(string i_LicensePlateNumber, CreateVehicle.eVehicleTypes i_TypeOfVehicle)
            : base(i_LicensePlateNumber)
        {
            if (i_TypeOfVehicle == CreateVehicle.eVehicleTypes.ElectricCar)
            {
                m_PowerSource = new Battery(r_MaxBatteryEnergy);
            }
            else if (i_TypeOfVehicle == CreateVehicle.eVehicleTypes.FuelCar)
            {
                m_PowerSource = new Fuel(r_MaxGasTankCapacity, k_FuelType);
            }
        }

        internal string NumberOfDoors
        {
            get
            {
                return m_NumberOfDoors.ToString();
            }

            set
            {
                int parsedValueToConvert;
                if (!int.TryParse(value, out parsedValueToConvert) || !Enum.IsDefined(typeof(eNumberOfDoors), parsedValueToConvert))
                {
                    throw new ValueOutOfRangeException(2, r_NumberOfMaxDoors, "Error!. Number of Doors is between 2 - " + r_NumberOfMaxDoors);
                }

                Enum.TryParse<eNumberOfDoors>(value, out m_NumberOfDoors);
            }
        }

        internal string ColorOfCar
        {
            get
            {
                return m_Colors.ToString();
            }

            set
            {
                int parsedValueToConvert;
                if (!int.TryParse(value, out parsedValueToConvert) || !Enum.IsDefined(typeof(eColorOfCar), parsedValueToConvert))
                {
                    throw new ArgumentException("Error!. You need to pick a number from the list!");
                }

                if (!Enum.IsDefined(typeof(eColorOfCar), parsedValueToConvert))
                {
                    throw new ValueOutOfRangeException(1, r_AmountOfColors, "Error!. Color selection should be around 1 - " + r_AmountOfColors);
                }

                Enum.TryParse<eColorOfCar>(value, out m_Colors);
            }
        }

        public override List<string> GetVehicleFullDetailes()
        {
            List<string> detailsOfVehicle = GetVehicleBaseDetails();

            detailsOfVehicle.Add("Amount of Doors: " + m_NumberOfDoors.ToString());
            detailsOfVehicle.Add("Color Of Car: " + m_Colors.ToString());

            if (m_PowerSource is Battery)
            {
                detailsOfVehicle.Add("Car Power Source: Battery");
            }
            else if (m_PowerSource is Fuel)
            {
                detailsOfVehicle.Add("Car Power Source: Fuel");
            }

            return detailsOfVehicle;
        }

        public override List<string> GetVehicleRequiredParameters()
        {
            List<string> parameters = GetBaseVehicleRequiredParameters();

            string colorCar;
            if (m_PowerSource is Battery)
            {
                Battery.GetParametersOfBattery(parameters);
            }
            else if (m_PowerSource is Fuel)
            {
                Fuel.GetParametersOfFuel(parameters);
            }

            colorCar = String.Format(@"Choose one of the colors: 
1.Red
2.White
3.Black
4.Blue");
            parameters.Add(colorCar);
            parameters.Add("How many doors does you car has?");
            return parameters;
        }

        protected internal override void UpdatePercentageOfEnergy()
        {
            float capacityValue;
            if (m_PowerSource is Battery)
            {
                capacityValue = r_MaxBatteryEnergy;
            }
            else
            {
                capacityValue = r_MaxGasTankCapacity;
            }

            m_PrecentageOfEnergyLeft = m_PowerSource.RemainingEnergy / capacityValue * 100;
        }

        public override void SetVehicleRequiredParameters(string i_ParameterToSet, int i_ParameterIndex)
        {
            eCarParameters carParameters;
            eVehicleParameters vehicleParameters;

            Enum.TryParse<eCarParameters>(i_ParameterToSet.ToString(), out carParameters);
            if (Enum.TryParse<eVehicleParameters>(i_ParameterIndex.ToString(), out vehicleParameters))
            {
                 SetBaseVehicleRequiredParametersWrapper(i_ParameterToSet, i_ParameterIndex, r_NumberOfWheels, r_AirPressure);
            }

            switch (carParameters)
            {
                case eCarParameters.AmountOfDoors:
                    {
                        NumberOfDoors = i_ParameterToSet;
                        break;
                    }

                case eCarParameters.CarColors:
                    {
                        ColorOfCar = i_ParameterToSet;
                        break;
                    }
            }
        }

        protected internal override void AddFunctionToEnergy(float i_AmountOfAdd)
        {
            m_PowerSource.AddPowerToSource(i_AmountOfAdd);
        }
    }
}
