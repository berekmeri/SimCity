using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace SimCity_Model.Model
{
    public class Grid
    {
        #region Fields
        private int _width;
        private int _height;
        private Field[,] _fields;
        private List<Zone> _zoneList;
        private static Dictionary<Zone, Dictionary<Zone, int>>? _connections;
        private int _zoneCount = -1;
        private int _commercialCount;
        private int _industrialCount;
        #endregion

        #region Properties
        public int Width { get { return _width; } }
        public int Height { get { return _height; } }
        public Field[,] Fields { get { return _fields; } }
        public List<Zone> ZoneList { get { return _zoneList; } }
        public int CommercialCount {get { return _commercialCount; } }
        public int IndustrialCount { get { return _industrialCount; } }


        public static Dictionary<Zone, Dictionary<Zone, int>>? Connections { get { return _connections; }}

        public Field this[Int32 x, Int32 y] { get { return _fields[x, y]; } }
        #endregion

        #region Constructor
        public Grid(int width, int height)
        {
            _width = width;
            _height = height;
            _commercialCount = 0;
            _industrialCount = 0;
            _fields = InitializeFields(width, height);
            _zoneList = new List<Zone>();
            _connections = new Dictionary<Zone, Dictionary<Zone, int>>();
        }
        #endregion

        #region Public methods
        public void SelectZone(Field field, ZoneType selectedZoneType)
        {
            if (selectedZoneType == ZoneType.COMMERCIAL)
            {
                ++_commercialCount;
            }
            else if(selectedZoneType == ZoneType.INDUSTRIAL)
            {
                ++_industrialCount;
            }

            _fields[field.X, field.Y].fieldType = FieldType.ZONE;
            _fields[field.X, field.Y].ZoneType = selectedZoneType;

            Zone zone = new((field.X, field.Y), selectedZoneType, ++_zoneCount, field);
            if (isRoadInNeighbour(field))
            {
                _zoneList.Add(zone);
                _zoneList.Last().RoadConnection = true;
            }
            else
                _zoneList.Add(new((field.X, field.Y), selectedZoneType, ++_zoneCount, field));

            if (selectedZoneType == ZoneType.INDUSTRIAL)
            {
                int radius = 3;
                for (int i = Math.Max(0, field.X - radius); i <= Math.Min(_width-1, field.X + radius); i++)
                {
                    for (int j = Math.Max(0, field.Y - radius); j <= Math.Min(_height-1, field.Y + radius); j++)
                    {
                        if (_fields[i, j].ZoneType == ZoneType.RESIDENTIAL)
                        {
                            Zone? residence = ZoneList.Find(x => x.Position.Item1 == i && x.Position.Item2 == j);
                            residence!.DistanceToFactories += 4 - (int)Math.Sqrt(Math.Pow(field.X - i, 2) + Math.Pow(field.Y - j, 2));
                            residence.FactoryBuiltNear();
                            //residence.FactoryBuiltNear((int)Math.Sqrt(Math.Pow(field.X - i, 2) + Math.Pow(field.Y - j, 2)));
                        }
                    }
                }
            }
            else if (selectedZoneType == ZoneType.RESIDENTIAL)
            {
                int radius = 3;
                for (int i = Math.Max(0, field.X - radius); i <= Math.Min(_width-1, field.X + radius); i++)
                {
                    for (int j = Math.Max(0, field.Y - radius); j <= Math.Min(_height-1, field.Y + radius); j++)
                    {
                        if (_fields[i, j].ZoneType == ZoneType.INDUSTRIAL)
                        {
                            zone.DistanceToFactories += 4 - (int)Math.Sqrt(Math.Pow(field.X - i, 2) + Math.Pow(field.Y - j, 2));
                            zone.FactoryBuiltNear();
                            //residence.FactoryBuiltNear((int)Math.Sqrt(Math.Pow(field.X - i, 2) + Math.Pow(field.Y - j, 2)));
                        }
                    }
                }
            }
            
        }
        public void DeleteZone(Field field)
        {
            if (field.ZoneType == ZoneType.COMMERCIAL)
            {
                --_commercialCount;
            }
            else if (field.ZoneType == ZoneType.INDUSTRIAL)
            {
                --_industrialCount;
            }
            
            if (field.ZoneType == ZoneType.INDUSTRIAL)
            {
                int radius = 3;
                for (int i = Math.Max(0, field.X - radius); i <= Math.Min(_width-1, field.X + radius); i++)
                {
                    for (int j = Math.Max(0, field.Y - radius); j <= Math.Min(_height-1, field.Y + radius); j++)
                    {
                        if (_fields[i, j].ZoneType == ZoneType.RESIDENTIAL)
                        {
                            Zone? residence = ZoneList.Find(x => x.Position.Item1 == i && x.Position.Item2 == j);
                            residence!.DistanceToFactories -= 4 - (int)Math.Sqrt(Math.Pow(field.X - i, 2) + Math.Pow(field.Y - j, 2));
                            residence.FactoryBuiltNear();
                            //residence.FactoryBuiltNear((int)Math.Sqrt(Math.Pow(field.X - i, 2) + Math.Pow(field.Y - j, 2)));
                        }
                    }
                }
            }

            int c = 0;
            _fields[field.X, field.Y].fieldType = FieldType.EMPTY;
            _fields[field.X, field.Y].ZoneType = ZoneType.NOTHING;
            _fields[field.X, field.Y].HasRoadConnection = false;
            while (_zoneList.Count > c && _zoneList.ElementAt(c).Position != (field.X, field.Y))
            {
                ++c;
            }
            _zoneList.RemoveAt(c);
        }
        public void DevelopeZone(Field field)
        {
            #region Enum_change
            if (_fields[field.X, field.Y].fieldType == FieldType.RESIDENTIAL_BUILDING_ZERO)
            {
                _fields[field.X, field.Y].fieldType = FieldType.RESIDENTIAL_BUILDING_FIRST;
            } else if (_fields[field.X, field.Y].fieldType == FieldType.RESIDENTIAL_BUILDING_FIRST)
            {
                _fields[field.X, field.Y].fieldType = FieldType.RESIDENTIAL_BUILDING_SECOND;
            }
            else if (_fields[field.X, field.Y].fieldType == FieldType.INDUSTRIAL_BUILDING_ZERO)
            {
                _fields[field.X, field.Y].fieldType = FieldType.INDUSTRIAL_BUILDING_FIRST;
            }
            else if (_fields[field.X, field.Y].fieldType == FieldType.INDUSTRIAL_BUILDING_FIRST)
            {
                _fields[field.X, field.Y].fieldType = FieldType.INDUSTRIAL_BUILDING_SECOND;
            }
            else if (_fields[field.X, field.Y].fieldType == FieldType.COMMERCIAL_BUILDING_ZERO)
            {
                _fields[field.X, field.Y].fieldType = FieldType.COMMERCIAL_BUILDING_FIRST;
            }
            else if (_fields[field.X, field.Y].fieldType == FieldType.COMMERCIAL_BUILDING_FIRST)
            {
                _fields[field.X, field.Y].fieldType = FieldType.COMMERCIAL_BUILDING_SECOND;
            }
            #endregion
            int i = 0;
            while (_zoneList.Count > i && _zoneList.ElementAt(i).Position != (field.X, field.Y))
            {
                ++i;
            }
            _zoneList[i].DevelopeLevel();
        }

        public bool Routing()
        {

            Dictionary<Tuple<Zone, Zone>, int> exists = new Dictionary<Tuple<Zone, Zone>, int>();
            foreach (KeyValuePair<Zone, Dictionary<Zone, int>> pairs in _connections!)
            {
                foreach (KeyValuePair<Zone, int> pair in pairs.Value)
                {
                    exists.Add(new Tuple<Zone, Zone>(pairs.Key, pair.Key), 0);
                }
            }

            _connections.Clear();

            for (int i = 0; i < _zoneList.Count; ++i)
            {
                _zoneList[i].DistanceFromStadium = 100;
                //if (_zoneList[i].ZoneType == ZoneType.RESIDENTIAL)
                //{

                    Dictionary<Tuple<int, int>, int> d = new Dictionary<Tuple<int, int>, int>();
                    Dictionary<Tuple<int, int>, Tuple<int, int>> p = new Dictionary<Tuple<int, int>, Tuple<int, int>>();

                    for (int j = 0; j < _width; j++)
                    {
                        for (int k = 0; k < _height; k++)
                        {

                            if (_fields[j, k].ZoneType != ZoneType.NOTHING || _fields[j, k].fieldType == FieldType.ROAD || _fields[j, k].fieldType == FieldType.STADIUM_TR || _fields[j, k].fieldType == FieldType.STADIUM_TL || _fields[j, k].fieldType == FieldType.STADIUM_BL || _fields[j, k].fieldType == FieldType.STADIUM_BR)
                            {
                                if (_zoneList[i].Position.Item1 == j && _zoneList[i].Position.Item2 == k)
                                {
                                    d.Add(new Tuple<int, int>(j, k), 0);
                                    p.Add(new Tuple<int, int>(j, k), new Tuple<int, int>(-1, -1));
                                }
                                else
                                {
                                    d.Add(new Tuple<int, int>(j, k), 1000000000);
                                    p.Add(new Tuple<int, int>(j, k), new Tuple<int, int>(-1, -1));
                                }


                            }
                        }
                    }


                    Dictionary<Tuple<int, int>, bool> done = new Dictionary<Tuple<int, int>, bool>();

                    Dictionary<Tuple<int, int>, int> minQ = new Dictionary<Tuple<int, int>, int>();

                    foreach (KeyValuePair<Tuple<int, int>, int> pair in d)
                    {
                        minQ[pair.Key] = pair.Value;
                    }

                    while (minQ.Count != 0)
                    {

                        int min = 1000000001;
                        Tuple<int, int> u = new Tuple<int, int>(-1, -1);


                        foreach (KeyValuePair<Tuple<int, int>, int> pair in minQ)
                        {
                            if (pair.Value < min)
                            {
                                min = pair.Value;
                                u = pair.Key;
                            }
                        }

                        minQ.Remove(u);
                        done[u] = true;

                        if (CONSTS.containesPosition(u.Item1-1,u.Item2) && d.ContainsKey(new Tuple<int, int>(u.Item1 - 1, u.Item2)) && !done.ContainsKey(new Tuple<int, int>(u.Item1 - 1, u.Item2)))
                        {
                            if (d[new Tuple<int, int>(u.Item1 - 1, u.Item2)] > d[u] + 1)
                            {
                                d[new Tuple<int, int>(u.Item1 - 1, u.Item2)] = d[u] + 1;
                                minQ[new Tuple<int, int>(u.Item1 - 1, u.Item2)] = d[u] + 1;

                            }

                        }

                        if (CONSTS.containesPosition(u.Item1 + 1, u.Item2) && d.ContainsKey(new Tuple<int, int>(u.Item1 + 1, u.Item2)) && !done.ContainsKey(new Tuple<int, int>(u.Item1 + 1, u.Item2)))
                        {

                            if (d[new Tuple<int, int>(u.Item1 + 1, u.Item2)] > d[u] + 1)
                            {
                                d[new Tuple<int, int>(u.Item1 + 1, u.Item2)] = d[u] + 1;
                                minQ[new Tuple<int, int>(u.Item1 + 1, u.Item2)] = d[u] + 1;

                            }
                        }
                        if (CONSTS.containesPosition(u.Item1, u.Item2 - 1) && d.ContainsKey(new Tuple<int, int>(u.Item1, u.Item2 - 1)) && !done.ContainsKey(new Tuple<int, int>(u.Item1, u.Item2 - 1)))
                        {
                            if (d[new Tuple<int, int>(u.Item1, u.Item2 - 1)] > d[u] + 1)
                            {
                                d[new Tuple<int, int>(u.Item1, u.Item2 - 1)] = d[u] + 1;
                                minQ[new Tuple<int, int>(u.Item1, u.Item2 - 1)] = d[u] + 1;

                            }
                        }
                        if (CONSTS.containesPosition(u.Item1, u.Item2 + 1) && d.ContainsKey(new Tuple<int, int>(u.Item1, u.Item2 + 1)) && !done.ContainsKey(new Tuple<int, int>(u.Item1, u.Item2 + 1)))
                        {

                            if (d[new Tuple<int, int>(u.Item1, u.Item2 + 1)] > d[u] + 1)
                            {
                                d[new Tuple<int, int>(u.Item1, u.Item2 + 1)] = d[u] + 1;
                                minQ[new Tuple<int, int>(u.Item1, u.Item2 + 1)] = d[u] + 1;

                            }
                        }

                    }


                    Dictionary<Zone, int> tmp = new Dictionary<Zone, int>();
                    int minFromStadium = 1000000000;


                    foreach (KeyValuePair<Tuple<int, int>, int> pair in d)
                    {

                        Zone? actZone = _zoneList.Find(x => x.Position.Item1 == pair.Key.Item1 && x.Position.Item2 == pair.Key.Item2);
                        if (actZone != null && (actZone.ZoneType == ZoneType.INDUSTRIAL || actZone.ZoneType == ZoneType.COMMERCIAL))
                        {
                            if (pair.Value < 1000000000)
                            {
                                tmp[actZone] = pair.Value;
                                exists.Remove(new Tuple<Zone, Zone>(_zoneList[i], actZone));
                            }


                        }
                        
                        else if (_fields[pair.Key.Item1, pair.Key.Item2].fieldType == FieldType.STADIUM_TR || _fields[pair.Key.Item1, pair.Key.Item2].fieldType == FieldType.STADIUM_TL || _fields[pair.Key.Item1, pair.Key.Item2].fieldType == FieldType.STADIUM_BL || _fields[pair.Key.Item1, pair.Key.Item2].fieldType == FieldType.STADIUM_BR)
                        {
                            if (_zoneList[i].DistanceFromStadium > pair.Value)
                            {
                                if (minFromStadium > pair.Value)
                                {
                                    minFromStadium = pair.Value;
                                }
                                
                            }

                        }


                    }

                    _zoneList[i].DistanceFromStadium = minFromStadium;
                    //_zoneList[i].StadiumBuiltNear();

                if (_zoneList[i].ZoneType == ZoneType.RESIDENTIAL)
                    _connections[_zoneList[i]] = tmp;
                    
                //}
            }

            

            if (exists.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Zone? GetZoneAt(Field field)
        {
            int i = 0;
            while (_zoneList.Count > i && _zoneList.ElementAt(i).Position != (field.X, field.Y))
            {
                ++i;
            }
            if (i < _zoneList.Count)
            {
                return _zoneList[i];
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region Private methods

        private Field[,] InitializeFields(int width, int height)
        {
            Field[,] fields = new Field[width, height];

            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    fields[i, j] = new Field(i, j);
                    fields[i, j].fieldType = FieldType.EMPTY;
                    fields[i, j].ZoneType = ZoneType.NOTHING;
                }
            }
            return fields;
        }
        private bool isRoadInNeighbour(Field field)
        {
            int x = field.X; 
            int y = field.Y;
            return (CONSTS.containesPosition(x - 1, y) && _fields[x - 1, y].fieldType == FieldType.ROAD) ||
                   (CONSTS.containesPosition(x + 1, y) && _fields[x + 1, y].fieldType == FieldType.ROAD) ||
                   (CONSTS.containesPosition(x, y + 1) && _fields[x, y + 1].fieldType == FieldType.ROAD) ||
                   (CONSTS.containesPosition(x, y - 1) && _fields[x, y - 1].fieldType == FieldType.ROAD);   
        }
        #endregion
    }
}
