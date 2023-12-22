using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace SimCity_Model.Model
{
    public enum GameSpeed { SLOW, NORMAL, FAST}
    public class GameModel
    {
        #region Fields
        private Grid _gameGrid;
        private Population _population;
        private Budget _budget;
        private int _year;
        private int _month;
        private int _day;
        private (int, int, int) _gameTime;
        private List<Zone> _zones;
        private String? _selected;
        private GameSpeed _gameSpeed;
        private List<Building> _buildings;
        private ServiceList _serviceList;
        private HashSet<Zone?> _actualConnection;
        private List<HashSet<Zone>> _connectionsBetweenZones;
        private List<Forest> _forests;
        private Road? _last;
        private Cable? _lastc;
        private int _powerPlantCount;
        private List<(int, int)> _powerPlants;
        private bool _populationThresholdFlag;
        #endregion

        #region Events
        public event EventHandler<SimCityEventArgs>? GameCreated;
        public event EventHandler<SimCityEventArgs>? GameAdvanced;
        public event EventHandler<SimCityEventArgs>? GameOver;

        public event EventHandler<SimCityBuildEventArgs>? ZonesChanged;
        public event EventHandler<SimCityBuildEventArgs>? RoadsChanged;
        public event EventHandler<SimCityBuildEventArgs>? ServiceBuildingsChanged;
        public event EventHandler<SimCityBuildEventArgs>? ForestChanged;

        public event EventHandler<SimCityEventArgs>? YearPassed;
        public event EventHandler<SimCityEventArgs>? NewCitizen;
        public event EventHandler<SimCityCatastropheEventArgs>? CatastrophyHappened;
        
        #endregion

        #region Properties
        public Grid GameGrid { get { return _gameGrid; }}

        public List<(int,int)> PowerPlants { get => _powerPlants; set => _powerPlants = value; }
        public Population Population { get { return _population; }}
        public (int, int, int) GameTime { get { return _gameTime; } set { _gameTime = value; } }
        public Budget Budget { get { return _budget;} }
        public GameSpeed GameSpeed { get { return _gameSpeed;} set { _gameSpeed = value; } }
        public List<Building> Buildings { get => _buildings; set => _buildings = value; }
        public String? Selected { get => _selected; 
            set
            {
                _selected = value;
            }
        }
        public int ZonesSize { get => _zones.Count; }
        public ServiceList ServiceList { get => _serviceList; set => _serviceList = value; }
        public List<HashSet<Zone>> Connections { get => _connectionsBetweenZones; set => _connectionsBetweenZones = value; }
        public Road? Last { get => _last; set => _last = value; }

        public Cable? LastC { get => _lastc; set => _lastc = value; }
        public List<Forest> Forests { get => _forests; set => _forests = value; }
        public Forest? getForestByField(Field field)
        {
            foreach (Forest forest in Forests)
            {
                if (forest.Fields.Contains(field))
                {
                    return forest;
                }
            }
            return null;
        }
        public Zone? getZoneByField(Field f){
            foreach(Zone zone in GameGrid.ZoneList){
                if(zone.Field == f)
                    return zone;
            }
            return null;
        }
        public Zone? getResidentialByField(Field field)
        {
            return getZoneByField(field) != null && getZoneByField(field)?.ZoneType == ZoneType.RESIDENTIAL ? 
                    getZoneByField(field) : null;
        }
        public HashSet<Zone> getActualConnection(List<HashSet<Zone>> s){
            return _connectionsBetweenZones[_connectionsBetweenZones.Count - 1];
        }
        #endregion

        #region Constructor
        public GameModel(bool shouldTookDownForest)
        {
            _zones = new List<Zone>();
            _population = new Population();
            _budget = new Budget(Population);
            _day = 1;
            _month = 1;
            _year = 2000;
            _gameTime = (_year, _month, _day);
            _gameGrid = new Grid(CONSTS.GridWidth, CONSTS.GridHeight);
            _gameSpeed = GameSpeed.NORMAL;
            _serviceList = new ServiceList();
            _buildings = new List<Building>();
            _forests = new List<Forest>();
            _powerPlants = new List<(int,int)>();
            _last = null;
            _actualConnection = new HashSet<Zone?>();
            _connectionsBetweenZones = new List<HashSet<Zone>>();
            if (shouldTookDownForest)
            {
                tookDown3RandomForest();
            }
            _populationThresholdFlag = false;
        }
        #endregion

        #region Public game methods

        #region ServiceBuildings
        public void deleteServiceBuilding(Field field)
        {
            if (ServiceList.getServiceByField(field) != null)
            {
                int x = ServiceList.ServiceBuildings.Count;
                int plusWidth = ServiceList.getServiceByField(field)!.Name!.Equals("Stadium") ||
                                ServiceList.getServiceByField(field)!.Name!.Equals("School") ||
                                ServiceList.getServiceByField(field)!.Name!.Equals("University") ||
                                ServiceList.getServiceByField(field)!.Name!.Equals("PowerPlant")
                                ? 2 : 1;
                int plusHight = ServiceList.getServiceByField(field)!.Name!.Equals("Stadium") ||
                                ServiceList.getServiceByField(field)!.Name!.Equals("University") ||
                                ServiceList.getServiceByField(field)!.Name!.Equals("PowerPlant")
                                ? 2 : 1;
                Service? service = ServiceList.getServiceByField(field);
                for (int i = field.X - plusHight; i <= field.X + plusHight; i++)
                {
                    for (int j = field.Y - plusWidth; j < field.Y + plusWidth; j++)
                    {
                        if (CONSTS.containesPosition(i, j) && (GameGrid[i, j].fieldType != FieldType.ROAD && GameGrid[i, j].fieldType != FieldType.ZONE &&
                            GameGrid[i, j].fieldType != FieldType.CABLE && GameGrid[i, j].fieldType != FieldType.COMMERCIAL_BUILDING_FIRST &&
                            GameGrid[i, j].fieldType != FieldType.COMMERCIAL_BUILDING_SECOND && GameGrid[i, j].fieldType != FieldType.COMMERCIAL_BUILDING_ZERO &&
                            GameGrid[i, j].fieldType != FieldType.INDUSTRIAL_BUILDING_FIRST && GameGrid[i, j].fieldType != FieldType.INDUSTRIAL_BUILDING_SECOND &&
                            GameGrid[i, j].fieldType != FieldType.INDUSTRIAL_BUILDING_ZERO && GameGrid[i, j].fieldType != FieldType.RESIDENTIAL_BUILDING_FIRST &&
                            GameGrid[i, j].fieldType != FieldType.RESIDENTIAL_BUILDING_SECOND && GameGrid[i, j].fieldType != FieldType.RESIDENTIAL_BUILDING_ZERO) &&
                            service!.Fields!.Contains(GameGrid[i, j]))
                        {
                            GameGrid[i, j].Neighbours.Clear();
                            if (GameGrid[i, j].fieldType == FieldType.POWERPLANT_TL || GameGrid[i, j].fieldType == FieldType.POWERPLANT_TR || GameGrid[i, j].fieldType == FieldType.POWERPLANT_BL || GameGrid[i, j].fieldType == FieldType.POWERPLANT_BR)
                            {
                                removePowerPlant(i, j);
                                BreadthFirstSearchFromPowerPlant();
                            }

                            GameGrid[i, j].fieldType = FieldType.EMPTY;
                            OnServiceBuildingsChanged(i, j);
                        }
                    }
                }
                _budget.Total += ServiceList.Deconstruct(ServiceList.getServiceByField(field));
                x = ServiceList.ServiceBuildings.Count;
            }
            
        }
        public void buildServiceBuilding(String name, Field field)
        {
            buildService(newServiceBuilding(name), field);

        }
        private Service newServiceBuilding(String name)
        {
            switch (name)
            {
                case "Police":
                    return new PoliceDepartment(1, 1);
                case "School":
                    return new School(1, 2);
                case "University":
                    return new University(2, 2);
                case "Stadium":
                    return new Stadium(2, 2);
                case "PowerPlant":
                    return new PowerPlant(2, 2);
                default:
                    throw new Exception("Nincs kiválasztva mit akarsz építeni.");
            }
        }  
        private void buildService(Service service, Field field)
        {
            if (!ableToBuildServiceBuild(field, service.Height, service.Width))
            {
                return;
            }
            List<Field> fields = new List<Field>();
            int buildPrice = 0, operationCost = 0; 

            switch (service.Name)
            {
                case "Police":
                    GameGrid[field.X, field.Y].fieldType = FieldType.POLICE;
                    AddNeightbours(field.X, field.Y);
                    OnServiceBuildingsChanged(field.X, field.Y);
                    buildPrice = CONSTS.PoliceBuiltPrice;
                    operationCost = CONSTS.PoliceOperationCost;
                    
            break;
                case "School":               
            if (CONSTS.containesPosition(field.X + 1, field.Y))
            {
                GameGrid[field.X, field.Y].fieldType = FieldType.SCHOOL_L;
                GameGrid[field.X + 1, field.Y].fieldType = FieldType.SCHOOL_R;                      
                fields.Add(GameGrid[field.X + 1, field.Y]);
                AddNeightbours(field.X, field.Y);
                AddNeightbours(field.X + 1, field.Y);
                OnServiceBuildingsChanged(field.X, field.Y);
                OnServiceBuildingsChanged(field.X + 1, field.Y);
                buildPrice = CONSTS.SchoolBuiltPrice;
                operationCost = CONSTS.SchoolOperationCost;
            }
            break;
                case "University":
                    GameGrid[field.X, field.Y].fieldType = FieldType.UNIVERSITY_TL;
            if (CONSTS.containesPosition(field.X + 1, field.Y) && CONSTS.containesPosition(field.X, field.Y + 1) && CONSTS.containesPosition(field.X + 1, field.Y + 1))
            {
                GameGrid[field.X + 1, field.Y].fieldType = FieldType.UNIVERSITY_TR;
                GameGrid[field.X, field.Y + 1].fieldType = FieldType.UNIVERSITY_BR;
                GameGrid[field.X + 1, field.Y + 1].fieldType = FieldType.UNIVERSITY_BL;
                fields.Add(GameGrid[field.X + 1, field.Y]);
                fields.Add(GameGrid[field.X, field.Y + 1]);
                fields.Add(GameGrid[field.X + 1, field.Y + 1]);
                AddNeightbours(field.X, field.Y);
                AddNeightbours(field.X + 1, field.Y);
                AddNeightbours(field.X, field.Y + 1);
                AddNeightbours(field.X + 1, field.Y + 1);
            
                OnServiceBuildingsChanged(field.X, field.Y);
                OnServiceBuildingsChanged(field.X + 1, field.Y);
                OnServiceBuildingsChanged(field.X, field.Y + 1);
                OnServiceBuildingsChanged(field.X + 1, field.Y + 1);
            
                buildPrice = CONSTS.UniversityBuiltPrice;
                operationCost = CONSTS.UniversityOperationCost;
            }
            break;
                case "Stadium":
            if (CONSTS.containesPosition(field.X + 1, field.Y) && CONSTS.containesPosition(field.X, field.Y + 1) && CONSTS.containesPosition(field.X + 1, field.Y + 1))
            {
                GameGrid[field.X, field.Y].fieldType = FieldType.STADIUM_TL;
                GameGrid[field.X + 1, field.Y].fieldType = FieldType.STADIUM_TR;
                GameGrid[field.X, field.Y + 1].fieldType = FieldType.STADIUM_BL;
                GameGrid[field.X + 1, field.Y + 1].fieldType = FieldType.STADIUM_BR;
                fields.Add(GameGrid[field.X + 1, field.Y]);
                fields.Add(GameGrid[field.X, field.Y + 1]);
                fields.Add(GameGrid[field.X + 1, field.Y + 1]);
                AddNeightbours(field.X, field.Y);
                AddNeightbours(field.X + 1, field.Y);
                AddNeightbours(field.X, field.Y + 1);
                AddNeightbours(field.X + 1, field.Y + 1);
            
                OnServiceBuildingsChanged(field.X, field.Y);
                OnServiceBuildingsChanged(field.X + 1, field.Y);
                OnServiceBuildingsChanged(field.X, field.Y + 1);
                OnServiceBuildingsChanged(field.X + 1, field.Y + 1);
            
                buildPrice = CONSTS.StadiumBuiltPrice;
                operationCost = CONSTS.StadiumOperationCost;
            }
            break;
                case "PowerPlant":
            if (CONSTS.containesPosition(field.X + 1, field.Y) && CONSTS.containesPosition(field.X, field.Y + 1) && CONSTS.containesPosition(field.X + 1, field.Y + 1))
            {
                _powerPlantCount++;
                GameGrid[field.X, field.Y].fieldType = FieldType.POWERPLANT_TL;
                GameGrid[field.X + 1, field.Y].fieldType = FieldType.POWERPLANT_TR;
                GameGrid[field.X, field.Y + 1].fieldType = FieldType.POWERPLANT_BL;
                GameGrid[field.X + 1, field.Y + 1].fieldType = FieldType.POWERPLANT_BR;
                fields.Add(GameGrid[field.X + 1, field.Y]);
                fields.Add(GameGrid[field.X, field.Y + 1]);
                fields.Add(GameGrid[field.X + 1, field.Y + 1]);
                AddNeightbours(field.X, field.Y);
                AddNeightbours(field.X + 1, field.Y);
                AddNeightbours(field.X, field.Y + 1);
                AddNeightbours(field.X + 1, field.Y + 1);
                GameGrid[field.X, field.Y].HasElectricity = true;
                GameGrid[field.X + 1, field.Y].HasElectricity = true;
                GameGrid[field.X, field.Y + 1].HasElectricity = true;
                GameGrid[field.X + 1, field.Y + 1].HasElectricity = true;
            
                PowerPlants.Add((field.X, field.Y));
            
                BreadthFirstSearchFromPowerPlant();
            
                OnServiceBuildingsChanged(field.X, field.Y);
                OnServiceBuildingsChanged(field.X + 1, field.Y);
                OnServiceBuildingsChanged(field.X, field.Y + 1);
                OnServiceBuildingsChanged(field.X + 1, field.Y + 1);
            
                buildPrice = CONSTS.PowerPlantBuiltPrice;
                operationCost = CONSTS.PowerPlantOperationCost;
            }
                    break;
                default:
                    throw new Exception("Nem építhető fel");
            }
            fields.Add(field);

            service.Fields = fields;
            service.OperationCost = operationCost;
            service.BuildPrice = buildPrice;
            _budget.Total -= buildPrice;
            ServiceList.Add(service);
            BreadthFirstSearchFromPowerPlant();
        }

        private void removePowerPlant(int i, int j)
        {
            // should be powerplant
            Service? service = ServiceList.getServiceByField(GameGrid[i, j]); 
            if (service != null) { 
                int minI = 1000, minJ = 1000;
                foreach (Field field in service.Fields!)
                {
                    if (field.X < minI)
                    {
                        minI = field.X;
                    }
                    if (field.Y < minJ)
                    {
                        minJ = field.Y;
                    }
                }
                PowerPlants.Remove((minI, minJ));
            }
        }   

        //Csak akkor lehet, ha van elég üres field, és van roadConnection
        public bool ableToBuildServiceBuild(Field field, int buildingHeight, int buildingWidth)
        {
            bool hasRoadConnection = false;
            for(int i = field.X; i < field.X + buildingHeight; i++)
            {
                for (int j = field.Y; j < field.Y + buildingWidth; j++)
                {
                    if (!CONSTS.containesPosition(i, j) || GameGrid[i, j].fieldType != FieldType.EMPTY)
                    {
                        return false;
                    }
                    if (GameGrid[i, j].HasRoadConnection)
                    {
                        hasRoadConnection = true;
                    }
                }
            }
            return hasRoadConnection;
        }
        private void payOperationCostOfServices()
        {
            foreach (Service service in ServiceList.ServiceBuildings)
            {
                _budget.Total -= service.OperationCost;
            }
        }
        #endregion

        #region Road
        //Will be dropped to Dijkstra
        public void buildRoad(Field field)
        {
            if (!field.HasRoadConnection)
            {
                _actualConnection = new HashSet<Zone?>();
                _actualConnection.Add(getZoneByField(field));
            }
            else
            {
                _actualConnection.Add(getZoneByField(field));
            }
            if (field.fieldType != FieldType.EMPTY)
            {
                return;
            }
            List<Field?> fields = new List<Field?>();
            if (CONSTS.containesPosition(field.X - 1, field.Y))
            {
                fields.Add(GameGrid[field.X - 1, field.Y]);
            }
            else
                fields.Add(null);
            if (CONSTS.containesPosition(field.X + 1, field.Y))
            {
                fields.Add(GameGrid[field.X + 1, field.Y]);
            }
            else
                fields.Add(null);
            if (CONSTS.containesPosition(field.X, field.Y - 1))
            {
                fields.Add(GameGrid[field.X, field.Y - 1]);
            }
            else
                fields.Add(null);
            if (CONSTS.containesPosition(field.X, field.Y + 1))
            {
                fields.Add(GameGrid[field.X, field.Y + 1]);
            }
            else
                fields.Add(null);
            Road r = new Road(field, fields);
            if (fields.Count > 0)
            {
                foreach (Field? f in r.Neighbours)
                {
                    if (f != null)
                    {
                         f.HasRoadConnection = true;
                         if (getZoneByField(f) != null)
                         {
                             getZoneByField(f)!.RoadConnection = true;
                         }
                        
                    }
                }
            }
            field.fieldType = FieldType.ROAD;

            field.Road = r;
            if (Last == null)
            {
                Last = r;
            }
            else
            {
                r.Prev = Last;
                Last.Next = r;
                Last = r.Prev;
            }
            _budget.PayConsructionOrMaintenanceCost(30, "Building road");
            GameGrid.Routing();
            OnRoadsChanged(field.X, field.Y);
        }
        #endregion

        #region Cabel
        public void buildCable(Field field)
        {
            if (field.fieldType != FieldType.EMPTY)
            {
                return;
            }
            Cable c = new Cable(field);
            
            field.Cable = c;
            if (LastC == null)
            {
                LastC = c;
            }
            else
            {
                c.Prev = LastC;
                LastC.Next = c;
                LastC = c.Prev;
            }
            AddNeightbours(field.X, field.Y);
                
                if ((CONSTS.containesPosition(field.X - 1, field.Y) && GameGrid[field.X - 1, field.Y].HasElectricity == true) ||
                    (CONSTS.containesPosition(field.X + 1, field.Y) && GameGrid[field.X + 1, field.Y].HasElectricity == true) ||
                    (CONSTS.containesPosition(field.X, field.Y - 1) && GameGrid[field.X, field.Y - 1].HasElectricity == true) ||
                    (CONSTS.containesPosition(field.X, field.Y + 1) && GameGrid[field.X, field.Y + 1].HasElectricity == true))
                {
                    BreadthFirstSearchFromPowerPlant();
                }
            
            _budget.PayConsructionOrMaintenanceCost(2, "Building cabel");
            OnRoadsChanged(field.X,field.Y);
        }
        #endregion

        #region Building
        public void Build()
        {
            foreach (Zone zone in GameGrid.ZoneList)
            {
                FieldType fieldType = FieldType.EMPTY;
                if (zone.ZoneType == ZoneType.INDUSTRIAL && zone.Level == 0 )
                {
                    fieldType = FieldType.INDUSTRIAL_BUILDING_ZERO;
                } else if (zone.ZoneType == ZoneType.INDUSTRIAL && zone.Level == 1)
                {
                    fieldType = FieldType.INDUSTRIAL_BUILDING_FIRST;
                }else if (zone.ZoneType == ZoneType.INDUSTRIAL && zone.Level == 2)
                {
                    fieldType = FieldType.INDUSTRIAL_BUILDING_SECOND;
                }
                else if (zone.ZoneType == ZoneType.RESIDENTIAL && zone.Level == 0)
                {
                    fieldType = FieldType.RESIDENTIAL_BUILDING_ZERO;
                    OnNewCitizen();
                }
                else if (zone.ZoneType == ZoneType.RESIDENTIAL && zone.Level == 1)
                {
                    fieldType = FieldType.RESIDENTIAL_BUILDING_FIRST;       
                }
                else if (zone.ZoneType == ZoneType.RESIDENTIAL && zone.Level == 2)
                {
                    fieldType = FieldType.RESIDENTIAL_BUILDING_SECOND; 
                }
                else if (zone.ZoneType == ZoneType.COMMERCIAL && zone.Level == 0)
                {
                    fieldType = FieldType.COMMERCIAL_BUILDING_ZERO;
                }
                else if (zone.ZoneType == ZoneType.COMMERCIAL && zone.Level == 1)
                {
                    fieldType = FieldType.COMMERCIAL_BUILDING_FIRST;
                }
                else if (zone.ZoneType == ZoneType.COMMERCIAL && zone.Level == 2)
                {
                    fieldType = FieldType.COMMERCIAL_BUILDING_SECOND;
                }
                if (GameGrid.ZoneList.Count == 1)
                {
                    zone.Field.fieldType = fieldType;
                    build(zone);
                }
                if (zone.RoadConnection && GameGrid.ZoneList.Count != 1)
                {
                    zone.Field.fieldType = fieldType;
                    build(zone);
                }
            }
        }
        private void build(Zone zone)
        {
            if (zone.GetCitizenSize >= zone.Capacity)
            {
                return;
            }
            BuildingType buildingType;
            switch (zone.ZoneType)
            {
                case ZoneType.RESIDENTIAL:
                    buildingType = BuildingType.Residential;
                    break;
                case ZoneType.COMMERCIAL:
                    buildingType = BuildingType.Comercial;
                    break;
                case ZoneType.INDUSTRIAL:
                    buildingType = BuildingType.Industrial;
                    break;
                default:
                    throw new Exception("Nem megfelelő a zóna");
            }
            Random random = new Random();
            zone.Building = Building.BUILD(buildingType, random.Next(1, 100));
            Buildings.Add(zone.Building);
        }

        #endregion

        #region Education
        public void increaseEducaitonLevel()
        {
            if (!ableIncreaseEducationLevel(2) && !ableIncreaseEducationLevel(3))
            {
                return;
            }
            int r = 0;
            Random random = new Random();
            foreach (Citizen citizen in Population.Citizens!)
            {
                r = random.Next(1, 6);
                //TODO Kiszervezni CONSTSB
                if (r == 5) // 20% eséllyel kap valaki középfokút 
                {
                    citizen.EducationLevel = 2;
                }
            }
            foreach (Citizen citizen in Population.Citizens)
            {
                r = random.Next(1, 8);
                //TODO Kiszervezni CONSTSB
                if (r == 5) // 12.5% eséllyel kap valaki felsőfokút 
                {
                    citizen.EducationLevel = 3;
                }
            }
        }

        //Adjuk meg a szintet (2 közép, 3 felső) amire akarunk fejleszteni
        public bool ableIncreaseEducationLevel(int increaseTo)
        {
            if (increaseTo == 3)
            {
                int sumUni = 0;
                foreach (var item in ServiceList.ServiceBuildings)
                {
                    if (item.Name == "University")
                    {
                        sumUni++;
                    }
                }
                if (sumUni <= 0)
                {
                    return false;
                }
            }
            else if (increaseTo == 2)
            {
                int sumSchool = 0;
                foreach (var item in ServiceList.ServiceBuildings)
                {
                    if (item.Name == "University")
                    {
                        sumSchool++;
                    }
                }
                if (sumSchool <= 0)
                {
                    return false;
                }

            }
            int sumBasic = 0;
            foreach (Citizen citizen in Population.Citizens!)
            {
                if (citizen.EducationLevel == increaseTo)
                {
                    sumBasic++;
                }
            }
            int maxCount = increaseTo == 2 ? 3 : 5;
            return Population.Citizens.Count / maxCount >= sumBasic;
            // Max egyharmaduknak lehet középfokú végzettsége
        }
        #endregion

        #region Forest
        public void increaseSatisfactionByForest()
        {

            foreach (Forest forest in Forests)
            { 
                foreach(Field field in forest.Fields)
                {

                    for (int i = 1; i <= 3; ++i)
                    {


                        if (GameGrid[Math.Max(field.X - i, 0), field.Y].ZoneType == ZoneType.RESIDENTIAL)
                        {
                            if(GameGrid.GetZoneAt(GameGrid[Math.Max(field.X - i, 0), field.Y])?.DistanceFromForest > i)
                                GameGrid.GetZoneAt(GameGrid[Math.Max(field.X - i, 0), field.Y])!.DistanceFromForest = i;
                            break;
                        }
                        else if (!CONSTS.containesPosition(field.X - i, field.Y) ||
                            GameGrid[Math.Max(field.X - i, 0), field.Y].fieldType != FieldType.EMPTY && GameGrid[Math.Max(field.X - i, 0), field.Y].fieldType != FieldType.ROAD)
                        {
                            break;
                        }
                    }

                    for (int i = 1; i <= 3; ++i)
                    {

                        if (GameGrid[Math.Min(field.X + i, GameGrid.Width - 1), field.Y].ZoneType == ZoneType.RESIDENTIAL)
                        {
                            if(GameGrid.GetZoneAt(GameGrid[Math.Min(field.X + i, GameGrid.Width - 1), field.Y])?.DistanceFromForest > i)
                                GameGrid.GetZoneAt(GameGrid[Math.Min(field.X + i, GameGrid.Width - 1), field.Y])!.DistanceFromForest = i;
                            break;
                        }
                        else if (!CONSTS.containesPosition(field.X + i, field.Y) ||
                            GameGrid[Math.Min(field.X + i, GameGrid.Width - 1), field.Y].fieldType != FieldType.EMPTY && GameGrid[Math.Min(field.X + i, GameGrid.Width - 1), field.Y].fieldType != FieldType.ROAD)
                        { 
                            break; 
                        }
                    }

                    for (int i = 1; i <= 3; ++i)
                    {

                        if (GameGrid[field.X, Math.Max(field.Y - i, 0)].ZoneType == ZoneType.RESIDENTIAL)
                        {
                            if(GameGrid.GetZoneAt(GameGrid[field.X, Math.Max(field.Y - i, 0)])?.DistanceFromForest > i)
                                GameGrid.GetZoneAt(GameGrid[field.X, Math.Max(field.Y - i, 0)])!.DistanceFromForest = i; ;
                            break;

                        }
                        else if (!CONSTS.containesPosition(field.X, field.Y - i) ||
                            GameGrid[field.X, Math.Max(field.Y - i, 0)].fieldType != FieldType.EMPTY && GameGrid[field.X, Math.Max(field.Y - i, 0)].fieldType != FieldType.ROAD)
                        {
                            break;
                        }
                    }

                    for (int i = 1; i <= 3; ++i)
                    {

                        if (GameGrid[field.X, Math.Min(field.Y + i, GameGrid.Height - 1)].ZoneType == ZoneType.RESIDENTIAL)
                        {
                            if(GameGrid.GetZoneAt(GameGrid[field.X, Math.Min(field.Y + i, GameGrid.Height - 1)])?.DistanceFromForest > i)
                                GameGrid.GetZoneAt(GameGrid[field.X, Math.Min(field.Y + i, GameGrid.Height - 1)])!.DistanceFromForest = i;
                            break;
                        }
                        else if (!CONSTS.containesPosition(field.X, field.Y + i) ||
                            GameGrid[field.X, Math.Min(field.Y + i, GameGrid.Height - 1)].fieldType != FieldType.EMPTY && GameGrid[field.X, Math.Min(field.Y + i, GameGrid.Height - 1)].fieldType != FieldType.ROAD)
                        {
                            break;
                        }
                    }
                }
            }

            _population.SatisfactionWithForestChanged();

            foreach (Zone zone in GameGrid.ZoneList)
            {
                zone.DistanceFromForest = 100;
            }


            
        }

        public void IncreaseSatisfactionByStadium()
        {
            _population.SatisfactionWithStadium();
        }

        public void IncreaseSatisfactionByPoliceStation()
        {
            for (int i = 0; i < CONSTS.GridWidth; ++i)
            {
                for (int j = 0; j < CONSTS.GridHeight; ++j)
                {
                    if (GameGrid[i, j].fieldType == FieldType.POLICE)
                    {
                        int radius = 8;
                        for (int k = Math.Max(0, i - radius); k <= Math.Min(CONSTS.GridWidth, i + radius); k++)
                        {
                            for (int l = Math.Max(0, j - radius); l <= Math.Min(CONSTS.GridHeight, j + radius); l++)
                            {
                                if (GameGrid[k, l].ZoneType == ZoneType.RESIDENTIAL)
                                {
                                    Zone? zone = GameGrid.ZoneList.Find(x => x.Position.Item1 == k && x.Position.Item2 == l) ;

                                    if (zone?.DistanceFromPolice < 9 - (int)Math.Sqrt(Math.Pow(i - k, 2) + Math.Pow(j - l, 2)))
                                    {
                                        zone.DistanceFromPolice = 9 - (int)Math.Sqrt(Math.Pow(i - k, 2) + Math.Pow(j - l, 2));
                                    }

                                }
                                else if (GameGrid[k, l].ZoneType != ZoneType.NOTHING)
                                {
                                    Zone? zone = GameGrid.ZoneList.Find(x => x.Position.Item1 == k && x.Position.Item2 == l);

                                    if (zone?.DistanceFromPolice < 9 - (int)Math.Sqrt(Math.Pow(i - k, 2) + Math.Pow(j - l, 2)))
                                    {
                                        zone.DistanceFromPolice = 9 - (int)Math.Sqrt(Math.Pow(i - k, 2) + Math.Pow(j - l, 2));
                                    }

                                }
                            }
                        }
                        
                    }
                }
            }

           _population.SatisfactionWithPoliceStation();

            foreach (Zone zone in GameGrid.ZoneList)
            {
                zone.DistanceFromPolice = 0;
            }
        }
        public void BuildForest(Field field)
        {
            if (!CONSTS.containesPosition(field.X, field.Y))
            {
                return;
            }
            if (needNewForest(field))
            {
                Forest forest = new Forest(field);
                _forests.Add(forest);
                OnForestChanged(field.X, field.Y);
                _budget.Total -= forest.BuildPrice;
            }
        }
        public void DeleteForest(Field field)
        {
            if (getForestByField(field) == null)
            {
                return;
            }
            if (getForestByField(field)!.Remove(field))
            {
                Forests.Remove(getForestByField(field)!);
                field.fieldType = FieldType.EMPTY;
                OnForestChanged(field.X, field.Y);
            }
        }
        private void tookDown3RandomForest()
        {
            Random random = new Random();
            int x, y;
            for (int k = 0; k < 3; k++)
            {
                x = random.Next(0, CONSTS.GridWidth);
                y = random.Next(0, CONSTS.GridHeight);
                GameGrid[x, y].fieldType = FieldType.FOREST;
                Forest forest = new Forest(GameGrid[x, y]);
                for (int i = x - 3; i < x + 4; i++)
                {
                    for (int j = y - 3; j < y + 4; j++)
                    {
                        if(CONSTS.containesPosition(i, j))
                        {
                            forest.Add(GameGrid[i, j]);
                            OnForestChanged(i, j);
                        }
                    }
                }
                _forests.Add(forest);
                
            }
        }
        private bool needNewForest(Field field)
        {
            for (int i = field.X - 1; i < field.X + 2; i++)
            {
                for (int j = field.Y - 1; j < field.Y + 2; j++)
                {
                    if (CONSTS.containesPosition(i, j))
                    {
                        if (GameGrid[i, j].fieldType == FieldType.FOREST)
                        {
                            getForestByField(GameGrid[i, j])?.Add(field);
                            OnForestChanged(field.X, field.Y);
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        private void ForestsGrow()
        {
            foreach (Forest forest in Forests)
            {
                IncreaseForest(forest);
            }
        }
        private void IncreaseForest(Forest forest)
        {

            if (forest.Age < 10)
            {
                Random random = new Random();

                List<Field> listToAddToForest = new List<Field>();

                foreach (Field field in forest.Fields)
                {
                    if (ableToIncraseFromHere(field))
                    {
                        foreach (Field f in getNeighbours(field))
                        {
                            if (random.Next(1, 6) == 1)
                            {
                                listToAddToForest.Add(f);
                            }
                        }
                    }
                }

                foreach (Field field1 in listToAddToForest)
                {
                    forest.Add(field1);
                    OnForestChanged(field1.X, field1.Y);
                }
                forest.Age += 1;
            }
           
        }
        private bool ableToIncraseFromHere(Field field)
        {
            return 0 < getNeighbours(field).Count;
        }
        private List<Field> getNeighbours(Field field)
        {
            List<Field> neighbours = new List<Field>();
            for (int i = field.X - 1; i < field.X + 2; i++)
            {
                for (int j = field.Y - 1; j < field.Y + 2; j++)
                {
                    if (CONSTS.containesPosition(i, j) && GameGrid[i, j].fieldType == FieldType.EMPTY)
                    {
                        neighbours.Add(GameGrid[i, j]);
                    }
                }
            }
            return neighbours;
        }
        private void payCostAfterForests()
        {
            foreach (Forest forest in _forests)
            {
                _budget.Total -= forest.OperationCost;
            }
        }
        #endregion

        #region DevelopeZones

        public void DevelopeZone(Field field)
        {
            if(GameGrid.GetZoneAt(field)?.Level == 0)
             {
                 _budget.PayConsructionOrMaintenanceCost(CONSTS.DevelopeZoneFirstLevel, "Developing zone to the first level");
             }else if(GameGrid.GetZoneAt(field).Level == 1)
             {
                 _budget.PayConsructionOrMaintenanceCost(CONSTS.DevelopeZoneSecondLevel, "Developing zone to the second level");
             }
            GameGrid.DevelopeZone(field);
            OnZonesChanged(field.X, field.Y);
        }

        #endregion

        public void AdvanceTime()
        {
            if (_day != 30)
            {
                _day++;
                GameTimeChanged();
                CheckIsGameOver();
            }
            else
            if (_month != 12)
            {
                OnCatastrophyHappened(-1, -1, -1, false); // turn off previous catastrophes 

                _day = 1;
                _month++;
                Build();
                foreach(Zone zone in GameGrid.ZoneList)
                {
                    OnZonesChanged(zone.Field.X, zone.Field.Y);
                }
                GameTimeChanged();
                _budget.TaxRevenue();
                _budget.IsTotalNegative();


                _population.WorkZoneRatioChanged((GameGrid.CommercialCount - GameGrid.IndustrialCount) / 5);
                foreach (Zone zone in GameGrid.ZoneList)
                {
                    if(zone.ZoneType == ZoneType.RESIDENTIAL)
                        zone.FactoryBuiltNear();
                }
                AddCitizen();

                if(_population.HeadCount > CONSTS.PopulationThreshold)
                {
                    _populationThresholdFlag = true;
                }

                increaseSatisfactionByForest();
                IncreaseSatisfactionByStadium();
                IncreaseSatisfactionByPoliceStation();
                increaseEducaitonLevel();

                Catastrophe();
                
            }
            else
            {
                _day = 1;
                _month = 1;
                _year++;
                GameTimeChanged();
                OnYearPassed(); ///// FONTOS
                ForestsGrow();
                payCostAfterForests();
                payOperationCostOfServices();
                _population.RetireCitizens();
                _population.IncreaseCitizensAge();
                ForestsGrow();
            }
            OnGameAdvanced();
        }
        public void NewGame()
        {
            OnGameCreated();
        }
        public void SelectZone(Field field, ZoneType zoneType)
        {
            GameGrid.SelectZone(field, zoneType);     
            _budget.PayConsructionOrMaintenanceCost(100, "Developing field into a " + zoneType + " zone.");
            AddNeightbours(field.X, field.Y);
            GameGrid.Routing();
                if ((CONSTS.containesPosition(field.X-1,field.Y) && GameGrid[field.X - 1, field.Y].HasElectricity == true) ||
                    (CONSTS.containesPosition(field.X + 1, field.Y) && GameGrid[field.X + 1, field.Y].HasElectricity == true) ||
                    (CONSTS.containesPosition(field.X, field.Y - 1) && GameGrid[field.X, field.Y - 1].HasElectricity == true) ||
                    (CONSTS.containesPosition(field.X, field.Y + 1) && GameGrid[field.X, field.Y + 1].HasElectricity == true))
                {
                    BreadthFirstSearchFromPowerPlant();
                }

            OnZonesChanged(field.X,field.Y);
        }

        public void DeleteZone(Field field)
        {
            if (field.fieldType != FieldType.EMPTY)
            {
                GameGrid.DeleteZone(field);
                _budget.PayBack(3, "deleting a zone");
                GameGrid[field.X, field.Y].HasElectricity = false;
                GameGrid.Routing();
                GameGrid[field.X, field.Y].Neighbours.Clear();
                BreadthFirstSearchFromPowerPlant();
                OnZonesChanged(field.X, field.Y);
            }
        }
        public void AddCitizen()
        {
            foreach (KeyValuePair<Zone, Dictionary<Zone, int>> pair in Grid.Connections!)
            {
                Random rand = new Random();
                if (pair.Key.GetCitizenSize == 0 || rand.Next(1, 8) < pair.Key.Satisfaction)
                {
                    if (pair.Key.Capacity > pair.Key.GetCitizenSize && GameGrid[pair.Key.Position.Item1, pair.Key.Position.Item2].HasElectricity == true)
                    {
                        foreach (KeyValuePair<Zone, int> workplace in pair.Value)
                        {
                            if (workplace.Key.GetCitizenSize == 0 || rand.Next(1, 6) < workplace.Key.Satisfaction)
                            {
                                if (pair.Key.Capacity > pair.Key.GetCitizenSize && workplace.Key.Capacity > workplace.Key.GetCitizenSize && GameGrid[workplace.Key.Position.Item1, workplace.Key.Position.Item2].HasElectricity == true)
                                {
                                    int count = rand.Next(2, Math.Max(3, Convert.ToInt32(pair.Key.Capacity * 0.3) - pair.Key.GetCitizenSize));
                                    for (int i = 0; i < count; ++i)
                                    {
                                        _population.AddCitizen(pair.Key, workplace.Key);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region Private game methods
        private bool isServiceFieldType(Field field)
        {
            return field.fieldType == FieldType.POLICE || field.fieldType == FieldType.SCHOOL_L || field.fieldType == FieldType.SCHOOL_R ||
                   field.fieldType == FieldType.UNIVERSITY_TL ||
                   field.fieldType == FieldType.UNIVERSITY_TR ||
                   field.fieldType == FieldType.UNIVERSITY_BL ||
                   field.fieldType == FieldType.UNIVERSITY_BR ||
                   field.fieldType == FieldType.POWERPLANT_TL ||
                   field.fieldType == FieldType.POWERPLANT_TR ||
                   field.fieldType == FieldType.POWERPLANT_BL ||
                   field.fieldType == FieldType.POWERPLANT_BR ||
                   field.fieldType == FieldType.STADIUM_TL ||
                   field.fieldType == FieldType.STADIUM_TR ||
                   field.fieldType == FieldType.STADIUM_BL ||
                   field.fieldType == FieldType.STADIUM_BR;
        }
        private void GameTimeChanged()
        {
            GameTime = (_year, _month, _day);
        }

        private void Catastrophe()
        {
            Random rand = new Random();
            if (rand.Next(0, 5) < 1)
            {
                if (GameGrid.ZoneList.Count > 0)
                {
                    int zoneIndex = rand.Next(0, GameGrid.ZoneList.Count);
                    int deathCount = GameGrid.ZoneList[zoneIndex].Catastrophe();
                    Field field = GameGrid.ZoneList[zoneIndex].Field;
                    OnCatastrophyHappened(field.X, field.Y, deathCount, true);
                }
            }
        }
        
        private bool IsGameOver()
        {
            if(Population.Satisfaction < -5 || (_populationThresholdFlag && Population.HeadCount < 10))
            {
                return true;
            }
            return false;
        }

        private void CheckIsGameOver()
        {
            if(IsGameOver())
            {
                OnGameOver();
            }
        }
        #endregion

        #region Private event methods

        private void OnGameAdvanced()
        {
            GameAdvanced?.Invoke(this, new SimCityEventArgs(GameTime));
        }

        private void OnGameOver()
        {
            GameOver?.Invoke(this, new SimCityEventArgs(GameTime));
        }

        private void OnNewCitizen()
        {
            NewCitizen?.Invoke(this, new SimCityEventArgs(GameTime));
        }
        private void OnYearPassed()
        {
            YearPassed?.Invoke(this, new SimCityEventArgs(GameTime));
        }
        private void OnGameCreated()
        {
            GameCreated?.Invoke(this, new SimCityEventArgs(GameTime));
        }
        private void OnZonesChanged(int x, int y)
        {
            ZonesChanged?.Invoke(this, new SimCityBuildEventArgs(x, y));
        }
        private void OnRoadsChanged(int x, int y)
        {
            RoadsChanged?.Invoke(this, new SimCityBuildEventArgs(x, y));
        }
        private void OnServiceBuildingsChanged(int x, int y)
        {
            ServiceBuildingsChanged?.Invoke(this, new SimCityBuildEventArgs(x, y));
        }
        private void OnForestChanged(int x, int y)
        {
            ForestChanged?.Invoke(this, new SimCityBuildEventArgs(x, y));
        }

        private void OnCatastrophyHappened(int x, int y, int deathCount, bool isCatastrophe)
        {
            CatastrophyHappened?.Invoke(this, new SimCityCatastropheEventArgs(x, y, deathCount, isCatastrophe));
        }

        public void DeleteRoad(Field field)
        {
            field.fieldType = FieldType.EMPTY;
            if (GameGrid.Routing())
            {
                field.Road = null;
                OnRoadsChanged(field.X, field.Y);
            }
            else
            {
                field.fieldType = FieldType.ROAD;
                GameGrid.Routing();
            }
        }
        public void DeleteCable(Field field)
        {
            GameGrid[field.X, field.Y].Neighbours.Clear();
            BreadthFirstSearchFromPowerPlant();
            field.Cable = null;
            field.fieldType = FieldType.EMPTY;
        }
        #endregion

        #region Electricity search
        private void AddNeightbours(int x, int y)
        {
            if (CONSTS.containesPosition(x - 1, y))
            {
                GameGrid[x, y].Neighbours.Add(GameGrid[x - 1, y]);
            }
            if (CONSTS.containesPosition(x + 1, y))
            {
                GameGrid[x, y].Neighbours.Add(GameGrid[x + 1, y]);
            }
            if (CONSTS.containesPosition(x, y - 1))
            {
                GameGrid[x, y].Neighbours.Add(GameGrid[x, y - 1]);
            }
            if (CONSTS.containesPosition(x, y + 1))
            {
                GameGrid[x, y].Neighbours.Add(GameGrid[x, y + 1]);
            }
        }

        private void BreadthFirstSearchFromPowerPlant()
        {
            int buildings = 0;
            for (int i = 0; i < GameGrid.Width; i++)
            {
                for (int j = 0; j < GameGrid.Height; j++)
                {
                    if (GameGrid[i,j].fieldType != FieldType.POWERPLANT_TL || GameGrid[i, j].fieldType != FieldType.POWERPLANT_TR ||
                        GameGrid[i, j].fieldType != FieldType.POWERPLANT_BL || GameGrid[i, j].fieldType != FieldType.POWERPLANT_BR)
                    {
                        if(GameGrid[i, j].HasElectricity == true)
                        {
                            buildings++;
                        }
                        GameGrid[i, j].HasElectricity = false;
                        GameGrid[i, j].Electircity = Electircity.NOTHING;
                    }
                }
            }


            
            for (int i = 0; i < PowerPlants.Count; i++)
            {
                int counter = 4+4;
                Queue<Field> queue = new Queue<Field>();
                HashSet<Field> visited = new HashSet<Field>();
                queue.Enqueue(GameGrid[PowerPlants[i].Item1, PowerPlants[i].Item2]);
                visited.Add(GameGrid[PowerPlants[i].Item1, PowerPlants[i].Item2]);

                while (queue.Count > 0)
                {
                    Field current = queue.Dequeue();
                    if (current.fieldType != FieldType.EMPTY && current.fieldType != FieldType.ROAD && current.fieldType != FieldType.FOREST && counter>0 && current.HasElectricity != true)
                    {
                        current.HasElectricity = true;

                        if (!(current.fieldType == FieldType.CABLE || current.fieldType == FieldType.POWERPLANT_TL || current.fieldType == FieldType.POWERPLANT_BR ||
                            current.fieldType == FieldType.POWERPLANT_TR || current.fieldType == FieldType.POWERPLANT_BL))
                        {
                            counter--;
                        }
                    }
                    
                    foreach (Field neighbor in current.Neighbours)
                    {
                        if (!visited.Contains(neighbor))
                        {
                            visited.Add(neighbor);
                            queue.Enqueue(neighbor);

                        }
                    }
                }
            }

           /* int hasEnought = 3 + PowerPlants.Count * 4;
            int hasLow = 2 + PowerPlants.Count * 3;
            for (int i = 0; i < GameGrid.Width; i++)
            {
                for (int j = 0; j < GameGrid.Height; j++)
                {
                    if ((GameGrid[i, j].fieldType != FieldType.POWERPLANT_TL || GameGrid[i, j].fieldType != FieldType.POWERPLANT_TR ||
                        GameGrid[i, j].fieldType != FieldType.POWERPLANT_BL || GameGrid[i, j].fieldType != FieldType.POWERPLANT_BR) && GameGrid[i, j].HasElectricity == true)
                    {

                        if (hasEnought > 0)
                        {
                            GameGrid[i, j].Electircity = Electircity.ENOUGH;
                            hasEnought--;
                        }
                        else if (hasLow > 0)
                        {
                            GameGrid[i, j].Electircity = Electircity.LOW;
                            hasLow--;
                        }
                        else
                        {
                            GameGrid[i, j].HasElectricity = false;
                            GameGrid[i, j].Electircity = Electircity.NOTHING;
                        }

                    }
                }
            }*/
        }
            #endregion
        }
}