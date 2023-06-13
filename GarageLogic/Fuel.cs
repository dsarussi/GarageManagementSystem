using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageLogic
{
    public class Fuel : PowerSource
    {
        private readonly eFuelType m_eFuelType;

        public enum eFuelType
        {
            Octan95 = 1,
            Octan96,
            Octan98,
            Soler,
        }

        internal eFuelType FuelType
        {
            get
            {
                return m_eFuelType;
            }
        }

        internal Fuel(float i_MaxEnergyOfFuel, eFuelType i_FuelType)
            : base(i_MaxEnergyOfFuel)
        {
            m_eFuelType = i_FuelType;
        }

        internal static void GetParametersOfFuel(List<string> io_ParametersToAdd)
        {
            io_ParametersToAdd.Add("Amount Of Gas (Liters)");
        }
    }
}
