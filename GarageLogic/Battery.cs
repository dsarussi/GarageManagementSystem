using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageLogic
{
    public class Battery : PowerSource
    {
        internal Battery(float i_AmountOfEnergy)
            : base(i_AmountOfEnergy)
        {
        }

        internal static void GetParametersOfBattery(List<string> io_ParametersToAdd)
        {
            io_ParametersToAdd.Add("Battery Life Span (in Hours)");
        }
    }
}
