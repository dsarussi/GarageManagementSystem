using System;

namespace GarageLogic
{
    public class OwnerOfVehicle
    {
        private string m_OwnerName;
        private string m_OwnerPhone;

        internal string OwnerName
        {
            get
            {
                return m_OwnerName;
            }

            set
            {
                if (value.Length == 0)
                {
                    throw new ArgumentException("Error!.Owner's name cannot be Empty!");
                }

                m_OwnerName = value;
            }
        }

        internal string OwnerPhone
        {
            get
            {
                return m_OwnerPhone;
            }

            set
            {
                long phoneNumber;
                if (!long.TryParse(value, out phoneNumber) || (value.Length != 9 && value.Length != 10))
                {
                    throw new ArgumentException("Error!.Owner's Phone cannot be Empty!");
                }

                m_OwnerPhone = value;
            }
        }
    }
}
