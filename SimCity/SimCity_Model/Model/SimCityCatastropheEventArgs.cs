using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimCity_Model.Model
{
    public class SimCityCatastropheEventArgs
    {
        private int _x;
        private int _y;
        private int _numberOfDeaths;
        private bool _isCatastrophe;

        public int X { get { return _x;} set { _x = value; } }
        public int Y { get { return _y;} set { _y = value; } }
        public int NumberOfDeaths { get { return _numberOfDeaths; }  set { _numberOfDeaths = value; } }
        public bool IsCatastrophe { get { return _isCatastrophe; } set { _isCatastrophe = value; } }

        public SimCityCatastropheEventArgs(int x, int y, int numberOfDeaths, bool catastrophe) {_x = x; _y = y; _numberOfDeaths = numberOfDeaths; _isCatastrophe = catastrophe; }
    }
}
