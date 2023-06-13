using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageLogic
{
    public abstract class PowerSource
    {
        protected readonly float r_MaxAmountOfEnergy;
        protected float m_RemainingEnergy;

        protected internal PowerSource(float i_MaxAmountOfEnerygy)
        {
            r_MaxAmountOfEnergy = i_MaxAmountOfEnerygy;
        }

        internal float RemainingEnergy
        {
            get
            {
                return m_RemainingEnergy;
            }
        }

        internal void AddPowerToSource(float i_AmountToAdd)
        {
            if (i_AmountToAdd + m_RemainingEnergy > r_MaxAmountOfEnergy || i_AmountToAdd < 0)
            {
                throw new ValueOutOfRangeException(0, r_MaxAmountOfEnergy - m_RemainingEnergy, "ERROR. Values should be between 0 - " + (r_MaxAmountOfEnergy - m_RemainingEnergy));
            }

            m_RemainingEnergy += i_AmountToAdd;
        }

        internal void SetPowerSourceCurrentValue(string i_CurrentValueToPut)
        {
            float currentValue;
            if(!float.TryParse(i_CurrentValueToPut, out currentValue) || i_CurrentValueToPut.Length < 0)
            {
                throw new FormatException("ERROR! .Power source value should be A Rational number");
            }

            if (currentValue > r_MaxAmountOfEnergy || currentValue < 0)
            {
                throw new ValueOutOfRangeException(0, r_MaxAmountOfEnergy, "ERROR. Power source value sholud be between 0 - " + r_MaxAmountOfEnergy);
            }

            m_RemainingEnergy = currentValue;
        }
    }
}
