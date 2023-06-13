using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageLogic
{
    public abstract class Vehicle
    {
        protected readonly string r_LicenseNumber;
        protected string m_ModelName;
        protected float m_PrecentageOfEnergyLeft;
        protected Wheel[] m_wheels;
        // $G$ DSN-999 (-3) repair status and owner info are not part of vehicle data.
        protected eRepairStatus m_Status;
        protected PowerSource m_PowerSource;
        protected OwnerOfVehicle m_OwnerOfVehicle;

        // $G$ DSN-999 (-5) why public nested class?
        public class Wheel
        {
            private readonly float r_MaximumAirPressure;
            private string m_ManufacturerName;
            private float m_CurrentAirPressure;

            internal Wheel(float i_MaxAirPressure)
            {
                this.r_MaximumAirPressure = i_MaxAirPressure;
            }

            internal string ManufacturerName
            {
                get
                {
                    return m_ManufacturerName;
                }

                set
                {
                    if (value.Length == 0)
                    {
                        throw new ArgumentException("Error!. Wheel manufacturer name Cannot be Empty!!");
                    }

                    m_ManufacturerName = value;
                }
            }

            internal string CurrentAirPressure
            {
                get
                {
                    return m_CurrentAirPressure.ToString();
                }

                set
                {
                    float currentAirPressure;
                    if (!float.TryParse(value, out currentAirPressure))
                    {
                        throw new ArgumentException("Error!. current air pressure should be Rational positive number!");
                    }

                    if (currentAirPressure > r_MaximumAirPressure || currentAirPressure < 0)
                    {
                        throw new ValueOutOfRangeException(0, r_MaximumAirPressure, "Error!. current air pressure should be between 0 to " + r_MaximumAirPressure);
                    }

                    m_CurrentAirPressure = currentAirPressure;
                }
            }

            internal float MaximumAirPressure
            {
                get
                {
                    return r_MaximumAirPressure;
                }
            }

            internal void AddAirToWheel(float i_AmountOfAirToAdd)
            {
                if (i_AmountOfAirToAdd + m_CurrentAirPressure > r_MaximumAirPressure || i_AmountOfAirToAdd < 0)
                {
                    throw new ValueOutOfRangeException(0, r_MaximumAirPressure + m_CurrentAirPressure, "Error!. Amount of Air to add is between 0 - " + (r_MaximumAirPressure + m_CurrentAirPressure));
                }

                m_CurrentAirPressure += i_AmountOfAirToAdd;
            }

            internal Wheel ShallowCopying()
            {
                return (Wheel)this.MemberwiseClone();
            }
        }

        public enum eRepairStatus
        {
            InRepair = 1,
            repaired,
            Paid,
        }

        public enum eVehicleParameters
        {
            CarModel = 1,
            OwnerName,
            OwnerPhone,
            ManufacureWheel,
            AirPressureWheel,
            AmountOfEnergyInPowerSource,
        }

        protected internal Vehicle(string i_LicensePlateNumber)
        {
            r_LicenseNumber = i_LicensePlateNumber;
            m_Status = eRepairStatus.InRepair;
        }

        public override int GetHashCode()
        {
            return r_LicenseNumber.GetHashCode();
        }

        public string LicenseNumber
        {
            get
            {
                return r_LicenseNumber;
            }
        }

        private string ModelName
        {
            set
            {
                if (value.Length == 0)
                {
                    throw new ArgumentException("Error!. model name cannot be Empty!", m_ModelName);
                }

                m_ModelName = value;
            }
        }

        internal eRepairStatus Status
        {
            get
            {
                return m_Status;
            }

            set
            {
                m_Status = value;
            }
        }

        public void FillWheelAir()
        {
            float amountOfAirToFill;
            foreach (Wheel wheel in m_wheels)
            {
                amountOfAirToFill = wheel.MaximumAirPressure - float.Parse(wheel.CurrentAirPressure);
                wheel.AddAirToWheel(amountOfAirToFill);
            }
        }

        public PowerSource GetPowerSouce()
        {
            return m_PowerSource;
        }

        public bool isVehicleOnBattery()
        {
            return m_PowerSource is Battery;
        }

        public bool isVehicleUsingFuel()
        {
            return m_PowerSource is Fuel;
        }

        internal List<string> GetVehicleBaseDetails()
        {
            List<string> baseDetails = new List<string>();
            string wheelManufactor;
            float airPressureOfWheel;

            baseDetails.Add("License Plate" + r_LicenseNumber);
            baseDetails.Add("Model Name" + m_ModelName);
            baseDetails.Add("Owner's Name: " + m_OwnerOfVehicle.OwnerName);
            baseDetails.Add("Status: " + m_Status.ToString());
            wheelManufactor = m_wheels[0].ManufacturerName;
            airPressureOfWheel = float.Parse(m_wheels[0].CurrentAirPressure);
            baseDetails.Add("Wheel Manufator: " + wheelManufactor);
            baseDetails.Add("wheel Air Pressure: " + airPressureOfWheel.ToString());
            baseDetails.Add("Number Of Wheels: " + m_wheels.Length.ToString());
            baseDetails.Add("Amount Of Energy(Precentage): " + m_PrecentageOfEnergyLeft.ToString());

            return baseDetails;
        }

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

        internal void SetBaseVehicleRequiredParameters(string i_ParameterToSet, int i_ParameterIndex)
        {
            eVehicleParameters parameterIndex;

            Enum.TryParse<eVehicleParameters>(i_ParameterIndex.ToString(), out parameterIndex);
            switch (parameterIndex)
            {
                case eVehicleParameters.CarModel:
                    {
                        ModelName = i_ParameterToSet;
                        break;
                    }

                case eVehicleParameters.OwnerName:
                    {
                        m_OwnerOfVehicle = new OwnerOfVehicle();
                        m_OwnerOfVehicle.OwnerName = i_ParameterToSet;
                        break;
                    }

                case eVehicleParameters.OwnerPhone:
                    {
                        m_OwnerOfVehicle.OwnerPhone = i_ParameterToSet;
                        break;
                    }

                case eVehicleParameters.AmountOfEnergyInPowerSource:
                    {
                        m_PowerSource.SetPowerSourceCurrentValue(i_ParameterToSet);
                        UpdatePercentageOfEnergy();
                        break;
                    }
            }
        }

        protected internal void SetBaseVehicleRequiredParametersWrapper
            (string i_ParameterToSet, int i_WhichParameter, int i_NumberOfVehicleWheels, float i_MaximumAirPresure)
        {
            eVehicleParameters indexVehicleParameters;

            Enum.TryParse<eVehicleParameters>(i_WhichParameter.ToString(), out indexVehicleParameters);

            switch (indexVehicleParameters)
            {
                case eVehicleParameters.OwnerName:
                case eVehicleParameters.CarModel:
                case eVehicleParameters.OwnerPhone:
                case eVehicleParameters.AmountOfEnergyInPowerSource:
                    {
                        SetBaseVehicleRequiredParameters(i_ParameterToSet, i_WhichParameter);
                        break;
                    }

                case eVehicleParameters.ManufacureWheel:
                    {
                        m_wheels = new Vehicle.Wheel[i_NumberOfVehicleWheels];
                        m_wheels[0] = new Vehicle.Wheel(i_MaximumAirPresure);
                        m_wheels[0].ManufacturerName = i_ParameterToSet;
                        break;
                    }

                case eVehicleParameters.AirPressureWheel:
                    {
                        m_wheels[0].CurrentAirPressure = i_ParameterToSet;
                        for(int i = 1; i < i_NumberOfVehicleWheels; i++)
                        {
                            m_wheels[i] = m_wheels[0].ShallowCopying();
                        }

                        break;
                    }
            }
        }

        protected internal abstract void AddFunctionToEnergy(float i_AmountOfAdd);

        public abstract List<string> GetVehicleFullDetailes();

        protected internal abstract void UpdatePercentageOfEnergy();

        public abstract List<string> GetVehicleRequiredParameters();

        public abstract void SetVehicleRequiredParameters(string i_ParameterToSet, int i_ParameterIndex);
    }
}