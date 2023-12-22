using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimCity_Model.Model
{
    public class PoliceDepartment : Service
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
        public PoliceDepartment(int x, int y)
        {
            IncreaseSatisfactionBy = CONSTS.IncreaseSatisfactionPolice;
            Radius = CONSTS.PoliceRadius;
            Name = "Police";
            Fields = new List<Field>();
            Width = x;  
            Height = y; 
        }

        #endregion

        
        }

    }

