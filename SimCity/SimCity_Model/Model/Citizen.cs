using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimCity_Model.Model
{
    public class Citizen
    {
        #region Fields
        private int _age;
        private int _educationLevel; // 1 = alapfok (default); 2 = középfok; 3 = felsőfok;
        private int _satisfactionWithTax;
        private int _satisfactionWithCommutingDistance;
        private int _satisfactionWithPublicSafety;
        private int _satisfactionWithNearFactories;
        private int _satisfactionWithPublicDebt;
        private int _satisfactionWithWorkZoneRatio;
        private int _satisfactionWithNearForest;
        private int _satisfactionWithNearStadium;
        private Zone? _residence;
        private Zone? _workplace;
        private int _tax;
        private int _bruttoIncome;
        private int _retirementIncome;
        private double _pensionRandomNum; // (amíg nem nyugdíjas addig 0, nyugdíjután random szám ami növekszik)
        #endregion

        #region Properties
        public int Age { get; set; }
        public int isElderly() { return 0; }
        public int EducationLevel { get => _educationLevel; set => _educationLevel = value; }

        //ratio nincs benne
        public int Satisfaction { get => _satisfactionWithNearStadium + _satisfactionWithTax + _satisfactionWithPublicDebt + _satisfactionWithPublicSafety + _satisfactionWithNearFactories + _satisfactionWithCommutingDistance + _satisfactionWithWorkZoneRatio + _satisfactionWithNearForest; }
        public Zone? Workplace { get => _workplace; set => _workplace = value; }
        public Zone? Residence { get => _residence; set => _residence = value; }
        public int Tax { get => _tax; set => _tax = value; }

        public int BruttoIncome { get => _bruttoIncome * _educationLevel; set => _bruttoIncome = value * _educationLevel; } // Easy counting with Education Level

        public double PensionRandomNum { get => _pensionRandomNum; set => _pensionRandomNum = value; }
        #endregion

        #region Constructor
        public Citizen(Zone residence, Zone workplace)
        {
            Random rand = new Random();

            _age = rand.Next(18, 60);
            _educationLevel = 1;
            _satisfactionWithTax = 0;
            _satisfactionWithPublicSafety = 0;
            _satisfactionWithNearFactories = 0;
            _satisfactionWithWorkZoneRatio = 0;
            _satisfactionWithNearForest = 0;
            _satisfactionWithNearStadium = 0;

            _residence = residence;
            _workplace = workplace;

            int commutinDistance = Grid.Connections![_residence][_workplace];
            _satisfactionWithCommutingDistance = (commutinDistance > 20) ? -(commutinDistance / 10) : ((20 - commutinDistance) / 5);

            _bruttoIncome = rand.Next(200, 801);
            _retirementIncome = 0;

        }
        #endregion

        #region Public Methods
        public void GotOlder()
        {
            ++_age;
            if (_age == 65)
            {
                _workplace?.Citizen.Remove(this);
                _workplace = null;
                _pensionRandomNum = 0.02;
            }
            else if (_age > 65)
            {
                Random rand = new Random();
                if (rand.NextDouble() < _pensionRandomNum)
                {
                    _age = 18;
                    _satisfactionWithTax = 0;
                    _satisfactionWithPublicSafety = 0;
                    _satisfactionWithCommutingDistance = 0;
                    _satisfactionWithNearFactories = 0;
                    _satisfactionWithWorkZoneRatio = 0;
                    _satisfactionWithNearForest = 0;
                    _satisfactionWithNearStadium = 0;

                    _bruttoIncome = rand.Next(200, 801);
                    _retirementIncome = 0;
                    _pensionRandomNum = 0;


                    _residence?.Citizen.Remove(this);
                    _residence = null;

                    bool hasnewResidence = false;
                    while (!hasnewResidence)
                    {
                        _residence = Grid.Connections?.Keys.ElementAt(rand.Next(Grid.Connections.Count));
                        if (_residence?.GetCitizenSize <= _residence?.Capacity && Grid.Connections?[_residence].Count > 0)
                        {
                            _workplace = Grid.Connections[_residence].Keys.ElementAt(rand.Next(Grid.Connections[_residence].Count));
                            if (_workplace.GetCitizenSize <= _workplace.Capacity)
                            {
                                hasnewResidence = true;
                                int commutinDistance = Grid.Connections[_residence][_workplace];
                                _satisfactionWithCommutingDistance = (commutinDistance > 20) ? - (commutinDistance / 10) : ((20 - commutinDistance) / 5);

                            }
                        }

                        
                    }

                }
                else
                {
                    _pensionRandomNum += 0.012;
                }
            }
        }

        public void CalculateTax(int taxKey)
        {
            if (_age < 65)
            {
                _tax = (int)Math.Floor(Convert.ToDouble(_bruttoIncome) * (Convert.ToDouble(taxKey) * 0.01)) 
                        * EducationLevel; // Easy tax counting after educationLevel
                if (_age >= 45)
                {
                    _retirementIncome += _tax / 2;
                }

                TaxChangeSatisfactionLevel(taxKey);
            }
            else
            {
                _satisfactionWithTax = 0;
            }
            
        }
        public void FactoryBuiltNear(int distances)
        {
            _satisfactionWithNearFactories = -distances;

        }

        public void StadiumBuiltNear()
        {
            _satisfactionWithNearStadium = 0;
            _satisfactionWithNearStadium += (_residence?.DistanceFromStadium > 20) ? 0 : ((20 - _residence?.DistanceFromStadium ?? 0) / 5);
            if (_workplace != null)
               _satisfactionWithNearStadium += (_workplace.DistanceFromStadium > 20) ? 0 : ((20 - _workplace.DistanceFromStadium) / 5);
            

        }

        public void ForestPlantNear()
        {
            if (_residence?.DistanceFromForest != 100)
            {
                _satisfactionWithNearForest = Math.Max(4 -_residence?.DistanceFromForest ?? 0, 0);

            }
        }

        public void FactoryDestroyedNear(int distance)
        {
            _satisfactionWithNearFactories -= distance;
        }

        public void WorkPlaceChanged(Zone newWorkplace)
        {

            _workplace = newWorkplace;
            //_satisfactionWithCommutingDistance += (CommutingDistance > 20) ? -(CommutingDistance / 10) : ((20 - CommutingDistance) / 5);
            //a zona osztályban kéne egy hownearispolice integer mező, ami 0a értéket vesz fel ha van rajta rendőrség(rendőrség ugysem lehet lakozonában)
            //és akkor ha messze van
            //akár több rendorseg is lehet
            //_satisfactionWithPublicSafety += Math.Max(2, 2 - _workplace.HowNearIsPolice);

        }

        public void residenceChanged(Zone newResidence)
        {
            _residence = newResidence;
           // _satisfactionWithCommutingDistance += (CommutingDistance > 20) ? -(CommutingDistance / 10) : ((20 - CommutingDistance) / 5);

            //_satisfactionWithPublicSafety += Math.Max(2, 2 - _workplace.HowNearIsPolice);
        }

        public void policeStationBuiltNear()
        {
            _satisfactionWithPublicSafety = 0;
            _satisfactionWithPublicSafety += Math.Max(0,_residence?.DistanceFromPolice ?? 0) * (Population.Citizens!.Count/50+1);
            if(_workplace != null)
                _satisfactionWithPublicSafety += Math.Max(0, _workplace.DistanceFromPolice)/2;
        }

        public void policeStationDestroyedNear(int distance)
        {
            _satisfactionWithPublicSafety -= Math.Max(2, 2 - distance);

        }


        public void HeadCountChanged()
        {
            //_satisfactionWithPublicSafety *= (20 - (20 - _population.HeadCount)) / 10;

       
        }

        public void SatisfactionWithWorkZoneRatioChanged(int ratio)
        {
            _satisfactionWithWorkZoneRatio = Math.Min(-ratio, 0);
        }


        public void TaxChangeSatisfactionLevel(int taxKey)
        {
            if (taxKey < 10)
            {
                _satisfactionWithTax = 2;
            }
            else if (taxKey < 20)
            {
                _satisfactionWithTax = 1;
            }
            else if (taxKey < 30)
            {
                _satisfactionWithTax = 0;
            }
            else if (taxKey < 50)
            {
                _satisfactionWithTax = -2;
            }
            else if (taxKey < 70)
            {
                _satisfactionWithTax = -4;
            }
            else
            {
                _satisfactionWithTax = -6;
            }

        }

        public void SatisfactionWithPublicDebtChanged(int debtLevel)
        {
            if (debtLevel == 0)
            {
                _satisfactionWithPublicDebt = 0;
            }
            else
            {
                _satisfactionWithPublicDebt -= debtLevel / 1000;
            }
        }
        #endregion
    }
}
