using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimCity_Model.Model
{
    public abstract class Service : Building
    {
        #region Fields

        private int _buildPrice; // const final
        private int _moneyBack;
        private int _tax;
        private int _operationCost;
        private int _width;   // Stadion: 2; University: 2; School,    Police, PowerPlant: 1
        private int _height;  // Stadion: 2; University: 2; School: 2; Police, PowerPlant: 1

        #endregion

        #region Properties
        public int BuildPrice { get { return _buildPrice; } set { _buildPrice = value; } }
        public int MoneyBack { get { return _moneyBack; } set { _moneyBack = value; } }
        public int Tax { get { return _tax; } set { _tax = value; } }
        public int OperationCost { get => _operationCost; set => _operationCost = value; }
        public int Width { get { return _width; } set { _width = value; } }
        public int Height { get { return _height; } set { _height = value; } }

        #endregion

        #region Methods
        public int Deconstruct()
        {
            return _buildPrice;
        }
        
        #endregion
    }
}
