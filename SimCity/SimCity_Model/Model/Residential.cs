using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimCity_Model.Model
{
    public class Residential : Building
    {
        #region Fields
        private int _capacity;
        private int _currentCitizenNumber;
        private int _distanceFromForest;
        #endregion

        #region Properties
        public int Capacity { get { return _capacity; } set { _capacity = value; } }
        public int CurrentCitizenNumber { get { return _currentCitizenNumber; } set { _currentCitizenNumber = value; } }
        public int DistanceFromForest { get => _distanceFromForest; set => _distanceFromForest = value; }
        #endregion

        #region Constructor
        public Residential(int capacity)
        {
            _capacity = capacity;
            _currentCitizenNumber = 0;
            _distanceFromForest = -1; // Amikor nincs látható erdő a közelében
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
