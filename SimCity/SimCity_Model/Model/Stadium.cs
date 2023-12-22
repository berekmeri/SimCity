using SimCity_Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimCity_Model.Model
{
    public class Stadium : Service
    {
        #region Fields
        private int _radius;
        private int _increaseSatisfactionBy;
        #endregion

        #region Properties
        public int Radius { get { return _radius; } set { _radius = value; } }
        public int IncreaseSatisfactionBy { get { return _increaseSatisfactionBy; } set { _increaseSatisfactionBy = value; } }
        #endregion

        #region Constructor
        public Stadium(int x, int y)
        {
            _radius = CONSTS.StadiumRadius;
            Fields = new List<Field>();
            Name = "Stadium";
            Width = x;
            Height = y;
        }
        #endregion
    }
}
