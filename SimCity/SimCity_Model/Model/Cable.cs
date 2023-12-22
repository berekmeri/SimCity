using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimCity_Model.Model
{
    public class Cable
    {
        private Field _field;
        private Cable? _prev;
        private Cable? _next;


        public Field Field { get => _field; set => _field = value; }
        public Cable? Prev { get => _prev; set => _prev = value; }
        public Cable? Next { get => _next; set => _next = value; }

        public Cable(Field field)
        {
            _field = field;
            _field.fieldType = FieldType.CABLE;
            _prev = null;
            _next = null;
        }
    }
}

