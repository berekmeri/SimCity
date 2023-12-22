using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimCity_Model.Model
{
    public class Forest
    {
        #region Field

        private List<Field> _fields;
        private int _buildPrice; 
        private int _moneyBack;
        private int _age;

        #endregion

        #region Properties
        public List<Field> Fields { get => _fields; set => _fields = value; }
        public int BuildPrice { get => _buildPrice; set => _buildPrice = value; }
        public int MoneyBack { get => _moneyBack; set => _moneyBack = value; }
        public int OperationCost { get => CONSTS.ForestOperationCost * _fields.Count;}
        public int Age { get => _age; set => _age = value; }
        #endregion

        #region Consturctor

        //By Default
        public Forest(List<Field> fields) { 
            _fields = fields;
            foreach (Field field in fields)
            {
                field.fieldType = FieldType.FOREST;
            }
            _age = 0;
        } 

        //Built
        public Forest(Field field)
        {
            _fields = new List<Field>
            {
                field
            };
            _fields[0].fieldType = FieldType.FOREST;
            _age = 0;
        }
        #endregion

        #region Methods
        public void Add(Field field)
        {
            _fields.Add(field);
            field.fieldType = FieldType.FOREST;
        }
        public bool Remove(Field field)
        {
            _fields.Remove(field);
            if (_fields.Count == 0)
            {
                return true;
            }
            field.fieldType = FieldType.EMPTY;
            return false;
        }
        public void Remove(int index)
        {
            _fields[index].fieldType = FieldType.EMPTY;
            _fields.RemoveAt(index);
        }
        #endregion



    }
}
