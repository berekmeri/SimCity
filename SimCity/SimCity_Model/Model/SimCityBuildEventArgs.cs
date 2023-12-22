using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimCity_Model.Model
{
    public class SimCityBuildEventArgs: EventArgs
    {
        private int _x;
        private int _y;

        public int ChangeX { get { return _x; } set { _x = value; } }
        public int ChangeY { get { return _y; } set { _y = value; } }
        public SimCityBuildEventArgs(int x, int y) 
        { 
            _x = x;
            _y = y;
        }
    }
}
