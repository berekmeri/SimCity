using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimCity_Model.Model
{
    public class Comercial : Building
    {
        #region Fields
        private int _capacity;
        private int _currentCitizenNumber;
        #endregion

        #region Properties
        public int Capacity { get { return _capacity; } set { _capacity = value; } }
        public int CurrentCitizenNumber { get { return _currentCitizenNumber; } set { _currentCitizenNumber = value; } }

        #endregion

        #region Constructor
        public Comercial(int capacity)
        {
            _capacity = capacity;
            _currentCitizenNumber = 0;
        }
        #endregion

        #region Methods
        public void addCitizen(int num)
        {
            _currentCitizenNumber += num;
        }
        public void removeCitizen(int num)
        {
            _currentCitizenNumber -= num;
        }

        #endregion
    }
}
