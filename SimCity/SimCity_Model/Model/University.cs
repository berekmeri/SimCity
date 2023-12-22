using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimCity_Model.Model
{
    public class University: Service
    {
        #region Constructor
        public University(int x, int y)
        {
            Name = "University";
            Fields = new List<Field>();
            Width = x;
            Height = y; 
        }
        #endregion

    }
}
