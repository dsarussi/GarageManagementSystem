using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageLogic
{
    public class ValueOutOfRangeException : Exception
    {
        private float m_MinimumRange;
        private float m_MaxRange;

        internal ValueOutOfRangeException(float i_MinimumRange, float i_MaxRange, string i_messege)
             : base(i_messege)
        {
            m_MinimumRange = i_MinimumRange;
            m_MaxRange = i_MaxRange;
        }
    }
}
