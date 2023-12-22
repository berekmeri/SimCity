using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimCity_Model.Model
{

    public class Road
    {

        private Field _field;
        private List<Field?> _neighbours; // bal fent jobb lent
        private Road? _prev;
        private Road? _next;


        public Field Field { get => _field; set => _field = value; }
        public List<Field?> Neighbours { get => _neighbours; set => _neighbours = value; }
        public Road? Prev { get => _prev; set => _prev = value; }
        public Road? Next { get => _next; set => _next = value; }

        public Road(Field field, List<Field?> neighbours)
        {
            _field = field;
            _neighbours = neighbours;
            _field.fieldType = FieldType.ROAD;
            _prev = null;
            _next = null;
        }
    }
}
