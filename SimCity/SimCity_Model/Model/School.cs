using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimCity_Model.Model
{
    public class School : Service
    {
        #region Constructor
        public School(int x, int y) 
        {
            Name = "School";
            Fields = new List<Field>();
            Width = x;
            Height = y;
        }
        #endregion
    }
}
