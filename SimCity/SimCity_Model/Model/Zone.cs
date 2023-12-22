using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimCity_Model.Model
{
    public enum ZoneType { INDUSTRIAL,RESIDENTIAL,COMMERCIAL,NOTHING}
    public class Zone
    {
        #region Fields
        private bool _roadConnection;
        private int _level;
        private int _capacity; // Resdental - 100, Industrial - 70, Commercial - 60
        private Building? _building;
        private ZoneType _zoneType;
        private List<Citizen> _citizen;
        private int _distanceToFactories;
        private int _tax;
        private (int, int) _position;
        private int _id;
        private Field _field;
        private int _distanceFromForest;
        private int _distanceFromPolice;
        private int _distanceFromStadium;
        #endregion

        #region Properties
        public List<Citizen> Citizen { get { return _citizen; } }
        public (int, int) Position { get => _position; set => _position = value; }
        public int Level { get => _level; set => _level = value; }
        public int Capacity { get => _capacity; set => _capacity = value; } 
        public int Tax { get => _tax; set => _tax = value; }
        public int GetCitizenSize { get => _citizen.Count; }

        public int Satisfaction { get => GetSatisfaction();  }
        public int DistanceToFactories { get => _distanceToFactories; set => _distanceToFactories = value; }
        public int getCostOfDevelop() { return 0; }
        public ZoneType ZoneType { get => _zoneType; }
        public int getId() { return _id; }
        public Building? Building { get => _building; set => _building = value; }
        public Field Field { get => _field; set => _field = value; }
        public bool RoadConnection { get => _roadConnection; set => _roadConnection = value; }
        public int DistanceFromForest { get => _distanceFromForest; set => _distanceFromForest = value; }
        public int DistanceFromPolice { get => _distanceFromPolice; set => _distanceFromPolice = value; }
        public int DistanceFromStadium { get => _distanceFromStadium; set => _distanceFromStadium = value; }

        #endregion

        #region Public Method
        public int getElectricityConsumtion() { return 0; }
        public int TaxCalculate() { return 0; }
        public void DevelopeLevel() {
            if (_level < 2)
            {
                _capacity = _capacity + (_capacity / 3);
                ++_level;
            }
        }
        public int getSatisfaction() { return 0; }

        public void FactoryBuiltNear()
        {
            foreach (Citizen citizen in _citizen)
            {
                citizen.FactoryBuiltNear(_distanceToFactories);
            }
        }

        public void StadiumBuiltNear()
        {
            foreach (Citizen citizen in _citizen)
            {
                citizen.StadiumBuiltNear();
            }

        }

       

        public int Catastrophe()
        {
            List<Citizen> whoWillDie = new List<Citizen>();
            foreach (Citizen citizen in _citizen)
            {
                Random rand = new Random();

                if (rand.Next(0, 5) < 2)
                {
                    whoWillDie.Add(citizen);
                }
            }

            foreach (Citizen citizen in whoWillDie)
            {
                Population.MoveOutCitizen(citizen);
            }

            return whoWillDie.Count;
        }
        #endregion

        #region Private Methods

        int GetSatisfaction()
        {
            int sum = 0;
            foreach (Citizen citizen in _citizen)
            {
                sum += citizen.Satisfaction;
            }

            if (GetCitizenSize > 0)
            {
                return sum / GetCitizenSize;
            }
            return 0;
        }

        #endregion
        #region Constructor
        public Zone((int,int) position, ZoneType zoneType,int id,Field field)
        {
            _citizen = new List<Citizen>();

            _distanceFromForest = 100;
            _distanceFromStadium = 100;
            _distanceToFactories = 0;
            _distanceFromPolice = 0;
            _position = position;
            _zoneType = zoneType;
            _level = 0;
            _id = id;
            switch (_zoneType) { 
                case Model.ZoneType.RESIDENTIAL:
                    _capacity = 100;
                    break;
                case Model.ZoneType.COMMERCIAL:
                    _capacity = 60;
                    break;
                case Model.ZoneType.INDUSTRIAL:
                    _capacity = 70;
                    break;
            }
            _field = field;
        }
        #endregion
       
    }
}
