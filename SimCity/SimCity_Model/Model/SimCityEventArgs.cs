using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SimCity_Model.Model
{
    public class SimCityEventArgs : EventArgs
    {
        private (int, int, int) _gameTime;
        public (int, int, int) GameTime { get { return _gameTime; } }

        public SimCityEventArgs((int, int, int) gameTime)
        {
            _gameTime = gameTime;
        }
    }
}
