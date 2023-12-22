using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimCity_Model.Model
{
    public enum BuildingType { Industrial, Residential, Comercial }

    public abstract class Building
    {
        #region Fields
        private Field? _field;
        private List<Field>? _fields;
        private BuildingType _buildingType;
        private String? _name;
        #endregion

        #region Properties
        public Field? Field { get => _field; set => _field = value; }
        public List<Field>? Fields { get { return _fields; } set { _fields = value; } }
        public BuildingType BuildingType { get { return _buildingType; } set { _buildingType = value; } }
        public String? Name { get { return _name; } set { _name = value; } }
        #endregion

        public static Building BUILD(BuildingType buildingType, int capacity)
        {
            switch (buildingType)
            {
                case BuildingType.Industrial:
                    return new Industrial(capacity);
                case BuildingType.Residential:
                    return new Residential(capacity);
                case BuildingType.Comercial:
                    return new Comercial(capacity);
                default:
                    throw new Exception("Nincs kiválasztva épület típus");
            }
        }

        #region Methods

        #endregion
    }
}
