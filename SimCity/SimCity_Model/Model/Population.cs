using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SimCity_Model.Model
{
    public class Population
    {
        #region Fields
        private static HashSet<Citizen>? _citizens;
        #endregion

        #region Properties
        public static HashSet<Citizen>? Citizens { get => _citizens; set => _citizens = value; }

        public int Satisfaction { get => CountSatisfaction(); }

        public int HeadCount { get => _citizens!.Count; }

       // public int TotalTax { get => TaxOfAll(); }

        #endregion

        #region Constructor
        public Population()
        {
            _citizens = new HashSet<Citizen>();
        }
        #endregion


        #region public Methods
        public void AddCitizen(Zone resident, Zone workplace)
        {

            Citizen citizen = new Citizen(resident, workplace);
            _citizens?.Add(citizen);
            resident.Citizen.Add(citizen);
            workplace.Citizen.Add(citizen);
        }

        public int TaxOfAll(int tax)
        {
            TaxCollection(tax);
       

            int totalTax = 0;
            foreach (Citizen citizen in _citizens!)
            {
                if (citizen.Age < 65)
                {
                    totalTax += citizen.Tax;
                }
            }
            return totalTax;
        }
        public int CountSatisfaction()
        {
            int sum = 0;
            foreach (Citizen citizen in _citizens!)
            {
                if (citizen.Satisfaction < -5 && citizen.Age <= 60)
                {
                    Random rand = new Random();
                    if(rand.Next(0, 101) < 20)
                        MoveOutCitizen(citizen);
                }
                sum += citizen.Satisfaction;
            }

            if (HeadCount > 0)
            {
                return sum / HeadCount;
            }
            return 0;
        }

        /*public void TaxKeyChanged()
        {
            foreach (Citizen citizen in _citizens)
            {
                citizen.TaxChangeSatisfactionLevel();
            }
        }*/

        public void HeadCountChanged()
        {
            foreach (Citizen citizen in _citizens!)
            {
                citizen.HeadCountChanged();
            }
        }

        public void publicDebtChanged(int debtLevel)
        {
            foreach (Citizen citizen in _citizens!)
            {
                citizen.SatisfactionWithPublicDebtChanged(debtLevel);
            }
        }

        public void WorkZoneRatioChanged(int ratio)
        {
            foreach (Citizen citizen in _citizens!)
            {
                citizen.SatisfactionWithWorkZoneRatioChanged(ratio);
            }
        }

        public void SatisfactionWithForestChanged()
        {
            foreach (Citizen citizen in _citizens!)
            {
                citizen.ForestPlantNear();
            }
        }

        public void SatisfactionWithPoliceStation()
        {
            foreach (Citizen citizen in _citizens!)
            {
                citizen.policeStationBuiltNear();
            }
        }

        public void SatisfactionWithStadium()
        {
            foreach (Citizen citizen in _citizens!)
            {
                citizen.StadiumBuiltNear();
            }
        }

        public void IncreaseCitizensAge()
        {
            foreach (Citizen citizen in _citizens!)
            {
                citizen.GotOlder();
            }

        }

       
        public void RetireCitizens()
        {
            foreach (Citizen citizen in Citizens!)
            {
                if (citizen.Age >= 65 && citizen.EducationLevel != 0)
                {
                    citizen.EducationLevel = 0; // Nem dolgozik tovább,
                                           // tehát nincs is olyan képzettségi szint
                                           // ami után fizettséget lehetne számolni utána
                }
            }
        }

        //most static lett gyorsan, majd 't
        public static void MoveOutCitizen(Citizen citizen)
        {
            if(citizen.Workplace != null)
                citizen.Workplace.Citizen.Remove(citizen);
            citizen.Residence?.Citizen.Remove(citizen);
            _citizens?.Remove(citizen);
            
        }
        public void MoveInCitizens()
        {

        }
        public void HandleDyingAndBirth()
        {

        }

        #endregion private Methods

        #region private Methods

        private void TaxCollection(int tax)
        {
            foreach (Citizen citizen in _citizens!)
            {
                citizen.CalculateTax(tax);
            }

        }

        private void CheckSatisfaction()
        {
            foreach (Citizen citizen in _citizens!)
            {
                if (citizen.Satisfaction < -5 && citizen.Age < 65)
                {
                    citizen.Workplace?.Citizen.Remove(citizen);
                    citizen.Residence?.Citizen.Remove(citizen);
                    _citizens.Remove(citizen);
                }
            }
        }

        #endregion
    }
}
