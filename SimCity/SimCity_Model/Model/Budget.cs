using SimCity_Model.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace SimCity_Model.Model
{
    public class Budget
    {

        #region Fields

        Population _population;

        private int _total;
        private int _taxKey;
        private int _howLongItsNegative;
        private ObservableCollection<String> _expenses;
        private ObservableCollection<String> _incomes;

        #endregion

        #region Properties

        public int Total { get => _total; set => _total = value; }
        public int TaxKey { get => _taxKey; set => _taxKey = value; }
        public ObservableCollection<String> Expenses { get => _expenses; private set => _expenses = value; }
        public ObservableCollection<String> Incomes { get => _incomes; private set => _incomes = value; }


        #endregion

        #region Constructor
        public Budget(Population population)
        {
            _population = population;
            _total = 200000;
            _taxKey = 25;
            _howLongItsNegative = 0;
            _expenses = new ObservableCollection<String>();
            _incomes = new ObservableCollection<String>();
        }
        #endregion

        #region public Methods

        public void TaxRevenue() {
            _total += _population.TaxOfAll(_taxKey);
            _incomes.Add("Taxes: +" + _population.TaxOfAll(_taxKey));
        }

        public void PayConsructionOrMaintenanceCost(int cost, String expenseType)
        {
            _total -= cost;
            _expenses.Add(expenseType + ": -" + cost);
        }
        public void PayBack(int cost, String paybackReason)
        {
            _total += cost;
            _incomes.Add("Payback from " + paybackReason + ": +" + cost);
        }

        public void IsTotalNegative()
        {
            if (_total < 0)
            {
                ++_howLongItsNegative;
                _population.publicDebtChanged((int)(_total * Math.Pow(1.2, _howLongItsNegative - 1)));
            }
            else
            {
                _howLongItsNegative = 0;
                _population.publicDebtChanged(0);

            }
        }

        #endregion
    }
}
