using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimCity_Model.Model
{
    public class PowerPlant: Service
    {
        #region Fields

        #endregion

        #region Properties
        #endregion

        #region Constructor
        public PowerPlant(int x, int y)
        {
            Name = "PowerPlant";
            Fields = new List<Field>();
            Width = x;
            Height = y;
        }
        #endregion

        #region Methods
        #endregion
    }
}
