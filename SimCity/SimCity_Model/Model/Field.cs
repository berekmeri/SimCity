using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SimCity_Model.Model
{
    public enum FieldType { 
                            EMPTY, ROAD, FOREST, CABLE, ZONE,
                            COMMERCIAL_BUILDING_ZERO, COMMERCIAL_BUILDING_FIRST, COMMERCIAL_BUILDING_SECOND, RESIDENTIAL_BUILDING_ZERO,
                            RESIDENTIAL_BUILDING_FIRST, RESIDENTIAL_BUILDING_SECOND, INDUSTRIAL_BUILDING_ZERO, INDUSTRIAL_BUILDING_FIRST, INDUSTRIAL_BUILDING_SECOND,
                            POLICE, SCHOOL_L, SCHOOL_R, UNIVERSITY_TL, UNIVERSITY_TR, UNIVERSITY_BL, UNIVERSITY_BR, POWERPLANT_TL, POWERPLANT_TR, POWERPLANT_BL, POWERPLANT_BR, STADIUM_TL, STADIUM_TR, STADIUM_BL, STADIUM_BR
    }

    public enum Electircity { NOTHING, LOW, ENOUGH}
    public class Field
    {
        #region Fields
        private FieldType _fieldType;
        private int _x;
        private int _y;
        private ZoneType _zone;
        private bool _hasRoadConnection;
        private bool _hasElectricity;
        private Road? _road;
        private Cable? _cable;
        private List<Field> _neighbours;
        private Electircity _electircity;
        #endregion

        #region Properties
        public FieldType fieldType { get { return _fieldType;} set { _fieldType = value; } }
        public List<Field> Neighbours { get { return _neighbours; } set { _neighbours = value; } }
        public int X { get { return _x;} }
        public int Y { get { return _y;} }
        public ZoneType ZoneType{ get { return _zone;} set { _zone = value; } }
        public bool HasRoadConnection { get => _hasRoadConnection; set => _hasRoadConnection = value; }
        public bool HasElectricity { get => _hasElectricity; set => _hasElectricity = value; }
        public Road? Road { get => _road; set => _road = value; }
        public Cable? Cable { get => _cable; set => _cable = value; }

        public Electircity Electircity { get => _electircity; set => _electircity = value; }
        #endregion

        #region Constructor
        public Field(int x, int y)
        {
            _fieldType = FieldType.EMPTY;
            _x = x;
            _y = y;
            HasElectricity = false;
            _neighbours = new List<Field>();
            _electircity = Electircity.NOTHING;
        }
        #endregion
    }
}
