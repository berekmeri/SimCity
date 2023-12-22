using SimCity_Model.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace SimCity.ViewModel
{
    public class SimCityViewModel : ViewModelBase
    {
        #region Fields
        private GameModel _model;
        private bool _isGamePaused;
        private Visibility _zoneDataVisibility;

        private String? _listedZoneCapacity = String.Empty;
        private String? _listedZoneCitizenCount = String.Empty;
        private String? _listedZoneSatisfaction = String.Empty;
        private String? _listedZoneCapacityLevel = String.Empty;
        private String? _listedZoneHasElectricity = String.Empty;
        private int _listedZoneX;
        private int _listedZoneY;
        private int _listedZoneIndex;

        private int _startX;
        private int _startY;
        private int _visibleGridWidth;
        private int _visibleGridHeight;

        private int progressStatus;

        private bool _displayCatastrophe;
        private String? _catastropheMessage = String.Empty;
        private Visibility _catastropheMessageVisibility;
        private (int, int) _previousCatastrophe;
        #endregion

        #region Properties
        public int GridWidth { get { return _model.GameGrid.Width; } }
        public int GridHeight { get { return _model.GameGrid.Height; } }
        public string GameTime { get { return _model.GameTime.Item1 + "/" + _model.GameTime.Item2 + "/" + _model.GameTime.Item3; } }
        public string IsGamePausedText { get { return _isGamePaused ? "Start" : "Pause"; } }
        public ObservableCollection<SimCityField> Fields { get; set; }

        public int Funds { get { return _model.Budget.Total; } }
        public int Population { get { return _model.Population.HeadCount; } }
        public int Tax { get { return _model.Budget.TaxKey; } }

        public int Satisfaction { get { return _model.Population.Satisfaction; } }

        public DelegateCommand NewGameCommand { get; private set; }
        public DelegateCommand ExitCommand { get; private set; }
        public DelegateCommand IncreaseTaxCommand { get; private set; }
        public DelegateCommand DecreaseTaxCommand { get; private set; }
        public DelegateCommand StartPauseCommand { get; private set; }
        public DelegateCommand MoveMapLeftCommand { get; private set; }
        public DelegateCommand MoveMapRightCommand { get; private set; }
        public DelegateCommand MoveMapUpCommand { get; private set; }
        public DelegateCommand MoveMapDownCommand { get; private set; }
        public DelegateCommand IncreaseSelectedZoneLevelCommand { get; private set; }
        public DelegateCommand ZoomInCommand { get; private set; }
        public DelegateCommand ZoomOutCommand { get; private set; }

        public int VisibleGridWidth
        {
            get { return _visibleGridWidth; }
            set
            {
                _visibleGridWidth = value;
                OnPropertyChanged(nameof(VisibleGridWidth));
            }
        }
        public int VisibleGridHeight 
        {
            get { return _visibleGridHeight; }
            set
            {
                _visibleGridHeight = value;
                OnPropertyChanged(nameof(VisibleGridHeight));
            }
        }

        public bool IsResidentialZoneSelected
        {
            get { return _model.Selected == "Residential"; }
            set
            {
                _model.Selected = "Residential";
                PropertyChange();
            }
        }
        public bool IsCommercialZoneSelected
        {
            get { return _model.Selected == "Commercial"; }
            set
            {
                _model.Selected = "Commercial";
                PropertyChange();
            }
        }
        public bool IsIndustrialZoneSelected
        {
            get { return _model.Selected == "Industrial"; }
            set
            {
                _model.Selected = "Industrial";
                PropertyChange();
            }
        }
        public bool IsDeleteZoneSelected
        {
            get { return _model.Selected == "ZoneDelete"; }
            set
            {
                _model.Selected = "ZoneDelete";
                PropertyChange();
            }
        }

        public bool IsRoadBuildSelected
        {
            get => _model.Selected == "Road";
            set
            {
                _model.Selected = "Road";
                PropertyChange();
            }
        }

        public bool IsRoadDeleteSelected
        {
            get => _model.Selected == "RoadDelete";
            set
            {
                _model.Selected = "RoadDelete";
                PropertyChange();
            }
        }
        public bool IsCableBuildSelected
        {
            get => _model.Selected == "Cable";
            set
            {
                _model.Selected = "Cable";
                PropertyChange();
            }
        }
        public bool IsCableDeleteSelected
        {
            get => _model.Selected == "CableDelete";
            set
            {
                _model.Selected = "CableDelete";
                PropertyChange();
            }
        }

        public bool IsPoliceSelected
        {
            get => _model.Selected == "Police";
            set
            {
                if (_model.Selected ==  "Police")
                    return;

                _model.Selected = "Police";
                PropertyChange();
            }
        }
        public bool IsStadiumSelected
        {
            get => _model.Selected == "Stadium";
            set
            {
                if (_model.Selected == "Stadium")
                    return;

                _model.Selected = "Stadium";
                PropertyChange();
            }
        }
        public bool IsSchoolSelected {
            get => _model.Selected == "School";
            set
            {
                if (_model.Selected == "School")
                    return;

                _model.Selected = "School";
                PropertyChange();
            }
        }
        public bool IsUniversitySelected {
            get => _model.Selected == "University";
            set
            {
                if (_model.Selected == "University")
                    return;

                _model.Selected = "University";
                PropertyChange();
            }
        }
        public bool IsPowerPlantSelected {
            get => _model.Selected == "PowerPlant";
            set
            {
                if (_model.Selected == "PowerPlant")
                    return;

                _model.Selected = "PowerPlant";
                PropertyChange();
            }
        }
        public bool IsServiceDeleteSelected
        {
            get => _model.Selected == "ServiceDelete";
            set
            {
                _model.Selected = "ServiceDelete";
                PropertyChange();
            }
        }
        public bool IsForestBuildSelected
        {
            get => _model.Selected == "ForestBuild";
            set
            {
                _model.Selected = "ForestBuild";
                PropertyChange();
            }
        }
        public bool IsForestDeleteSelected
        {
            get => _model.Selected == "ForestDelete";
            set
            {
                _model.Selected = "ForestDelete";
                PropertyChange();
            }
        }
        public bool IsGetZoneDataSelected
        {
            get => _model.Selected == "GetZoneData";
            set
            {
                _model.Selected = "GetZoneData";
                PropertyChange();
            }
        }
        public bool IsGameSpeedSlow
        {
            get { return _model.GameSpeed == GameSpeed.SLOW; }
            set
            {
                if (_model.GameSpeed == GameSpeed.SLOW)
                    return;

                _model.GameSpeed = GameSpeed.SLOW;
                OnPropertyChanged(nameof(IsGameSpeedSlow));
                OnPropertyChanged(nameof(IsGameSpeedNormal));
                OnPropertyChanged(nameof(IsGameSpeedFast));
                OnGameSpeedChanged();
            }
        }
        public bool IsGameSpeedNormal
        {
            get { return _model.GameSpeed == GameSpeed.NORMAL; }
            set
            {
                if (_model.GameSpeed == GameSpeed.NORMAL)
                    return;

                _model.GameSpeed = GameSpeed.NORMAL;
                OnPropertyChanged(nameof(IsGameSpeedSlow));
                OnPropertyChanged(nameof(IsGameSpeedNormal));
                OnPropertyChanged(nameof(IsGameSpeedFast));
                OnGameSpeedChanged();
            }
        }
        public bool IsGameSpeedFast
        {
            get { return _model.GameSpeed == GameSpeed.FAST; }
            set
            {
                if (_model.GameSpeed == GameSpeed.FAST)
                    return;

                _model.GameSpeed = GameSpeed.FAST;
                OnPropertyChanged(nameof(IsGameSpeedSlow));
                OnPropertyChanged(nameof(IsGameSpeedNormal));
                OnPropertyChanged(nameof(IsGameSpeedFast));
                OnGameSpeedChanged();
            }
        }

        public Visibility ZoneDataVisibility
        {
            get { return _zoneDataVisibility; }
            set
            {
                _zoneDataVisibility = value;
                OnPropertyChanged(nameof(ZoneDataVisibility));
            }
        }
        public String? ListedZoneCapacity
        {
            get { return _listedZoneCapacity; }
            set
            {
                _listedZoneCapacity = value;
                OnPropertyChanged(nameof(ListedZoneCapacity));
            }
        }
        public String? ListedZoneCitizenCount
        {
            get { return _listedZoneCitizenCount; }
            set
            {
                _listedZoneCitizenCount = value;
                OnPropertyChanged(nameof(ListedZoneCitizenCount));
            }
        }

        public String? ListedZoneSatisfaction
        {
            get { return _listedZoneSatisfaction; }
            set
            {
                _listedZoneSatisfaction = value;
                OnPropertyChanged(nameof(ListedZoneSatisfaction));
            }
        }
        public String? ListedZoneCapacityLevel
        {
            get { return _listedZoneCapacityLevel; }
            set
            {
                _listedZoneCapacityLevel = value;
                OnPropertyChanged(nameof(ListedZoneCapacityLevel));
            }
        }

        public String? ListedZoneHasElectricity
        {
            get { return _listedZoneHasElectricity; }
            set
            {
                _listedZoneHasElectricity = value;
                OnPropertyChanged(nameof(ListedZoneHasElectricity));
            }
        }
        public int ListedZoneX
        {
            get { return _listedZoneX; }
            set
            {
                _listedZoneX = value;
                OnPropertyChanged(nameof(ListedZoneX));
            }
        }
        public int ListedZoneY
        {
            get { return _listedZoneY; }
            set
            {
                _listedZoneY = value;
                OnPropertyChanged(nameof(ListedZoneY));
            }
        }
        public int ListedZoneIndex
        {
            get { return _listedZoneIndex; }
            set
            {
                _listedZoneIndex = value;
                OnPropertyChanged(nameof(ListedZoneIndex));
            }
        }

        public ObservableCollection<String> Expenses
        {
            get { return _model.Budget.Expenses; }
            set
            {
                OnPropertyChanged(nameof(Expenses));
            }
        }
        public ObservableCollection<String> Incomes
        {
            get { return _model.Budget.Incomes; }
            set
            {
                OnPropertyChanged(nameof(Incomes));
            }
        }

        public bool DisplayCatasthrophe { get { return _displayCatastrophe; } set { _displayCatastrophe = value;} }
        public String? CatastropheMessage
        {
            get { return _catastropheMessage; }
            set
            {
                _catastropheMessage = value;
                OnPropertyChanged(nameof(CatastropheMessage));
            }
        }

        public Visibility CatastropheMessageVisibility
        {
            get { return _catastropheMessageVisibility; }
            set
            {
                _catastropheMessageVisibility = value;
                OnPropertyChanged(nameof(CatastropheMessageVisibility));
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// Új játék eseménye.
        /// </summary>
        public event EventHandler? NewGame;

        /// <summary>
        /// Játékból való kilépés eseménye.
        /// </summary>
        public event EventHandler? ExitGame;

        /// <summary>
        /// Játéksebesség változásának eseménye
        /// </summary>
        public event EventHandler? GameSpeedChanged;
        public event EventHandler? GameStartedPaused;
        #endregion

        #region Constructors
        public SimCityViewModel(GameModel model)
        {
            _model = model;
            _isGamePaused = false;
            _zoneDataVisibility = Visibility.Hidden;
            _model.GameCreated += new EventHandler<SimCityEventArgs>(Model_GameCreated);
            _model.GameAdvanced += new EventHandler<SimCityEventArgs>(Model_GameAdvanced);

            _model.ZonesChanged += new EventHandler<SimCityBuildEventArgs>(Model_ZonesChanged);
            _model.RoadsChanged += new EventHandler<SimCityBuildEventArgs>(Model_RoadsChanged);
            _model.ServiceBuildingsChanged += new EventHandler<SimCityBuildEventArgs>(Model_ServiceBuildingsChanged);
            _model.ForestChanged += new EventHandler<SimCityBuildEventArgs>(Model_ForestsChanged);

            _model.NewCitizen += new EventHandler<SimCityEventArgs>(Model_NewCitizen);
            _model.CatastrophyHappened += new EventHandler<SimCityCatastropheEventArgs>(Model_CatasrophyHappened);

            _startX = 0;
            _startY = 0;

            _visibleGridWidth = CONSTS.VisibleGridWidthNormal;
            _visibleGridHeight = CONSTS.VisibleGridHeightNormal;

            progressStatus = 0;

            _displayCatastrophe = false;
            _previousCatastrophe = (-1, -1);

            NewGameCommand = new DelegateCommand(param => OnNewGame());
            ExitCommand = new DelegateCommand(param => OnExitGame());
            IncreaseTaxCommand = new DelegateCommand(param => IncreaseTax());
            DecreaseTaxCommand = new DelegateCommand(param => DecreaseTax());
            StartPauseCommand = new DelegateCommand(param => StartPauseGame());

            MoveMapLeftCommand = new DelegateCommand(param => OnMoveMapLeft());
            MoveMapRightCommand = new DelegateCommand(param => OnMoveMapRight());
            MoveMapUpCommand = new DelegateCommand(param => OnMoveMapUp());
            MoveMapDownCommand = new DelegateCommand(param => OnMoveMapDown());

            ZoomInCommand = new DelegateCommand(param => OnZoomIn());
            ZoomOutCommand = new DelegateCommand(param => OnZoomOut());

            IncreaseSelectedZoneLevelCommand = new DelegateCommand(param => IncreaseSelectedZoneLevel());

            Fields = new ObservableCollection<SimCityField>();
            GenerateFields();
        }
        #endregion

        #region Private methods
        private void RefreshGrid()
        {
            if (progressStatus == 0)
            {
                foreach (SimCityField field in Fields)
                {
                    if (!CONSTS.containesPosition(field.X, field.Y))
                    {
                        return;
                    }
                    field.FieldType = _model.GameGrid[field.X, field.Y].fieldType;
                    field.ZoneType = _model.GameGrid[field.X, field.Y].ZoneType;
                    //field.Text = field.Index + ": " + field.X + ", " + field.Y;
                    int x = _previousCatastrophe.Item1;
                    int y = _previousCatastrophe.Item2;

                    if (DisplayCatasthrophe && y - _startY >= 0 && x - _startX >= 0 && y - _startY < VisibleGridHeight && x - _startX < VisibleGridWidth)
                    {
                        int fieldIndex = (y - _startY) * VisibleGridWidth + (x - _startX);
                        Fields[fieldIndex].IsCatastrophe = true;
                    }
                }
            }
            OnPropertyChanged();
        }
        private void RefreshField(int index)
        {
            SimCityField field = Fields[index];
            field.FieldType = _model.GameGrid[field.X, field.Y].fieldType;
            field.ZoneType = _model.GameGrid[field.X, field.Y].ZoneType;
            OnPropertyChanged();
        }

        private void IncreaseTax()
        {
            ++_model.Budget.TaxKey;
            OnPropertyChanged(nameof(Tax));
        }
        private void DecreaseTax()
        {
            --_model.Budget.TaxKey;
            OnPropertyChanged(nameof(Tax));
        }
        private void StartPauseGame()
        {
            if (_isGamePaused)
            {
                _isGamePaused = false;
                OnPropertyChanged(nameof(IsGamePausedText));
            }
            else
            {
                _isGamePaused = true;
                OnPropertyChanged(nameof(IsGamePausedText));
            }
            OnGameStartedPaused();
        }

        private void FieldClick(int currentIndex)
        {
            if (_isGamePaused)
            {
                return;
            }

            int x = Fields[currentIndex].X; //Az adott mező X koordinátája kiszámolva az indexből
            int y = Fields[currentIndex].Y; //Az adott mező Y koordinátája kiszámolva az indexből

            if (CONSTS.containesPosition(x, y))
            {
                switch (_model.Selected)
                {
                    case "Residential":
                        zoneSwitch(_model.Selected, currentIndex, x, y);
                        break;
                    case "Commercial":
                        zoneSwitch(_model.Selected, currentIndex, x, y);
                        break;
                    case "Industrial":
                        zoneSwitch(_model.Selected, currentIndex, x, y);
                        break;
                    case "ZoneDelete":
                        if (Fields[currentIndex].ZoneIndex < _model.ZonesSize &&
                        Fields[currentIndex].FieldType == FieldType.ZONE)
                        {
                            _model.DeleteZone(_model.GameGrid[x, y]);
                        }
                        break;
                    case "Road":
                        if (Fields[currentIndex].FieldType == FieldType.EMPTY)
                        {
                            Fields[currentIndex].FieldType = FieldType.ROAD;
                            Fields[currentIndex].ZoneType = ZoneType.NOTHING;
                            _model.buildRoad(_model.GameGrid[x, y]);
                        }
                        break;
                    case "RoadDelete":
                        if (Fields[currentIndex].FieldType == FieldType.ROAD)
                        {
                            //Fields[currentIndex].FieldType = FieldType.EMPTY;
                            //Fields[currentIndex].ZoneType = ZoneType.NOTHING;
                            _model.DeleteRoad(_model.GameGrid[x, y]);
                        }
                        break;
                    case "Cable":
                        if (Fields[currentIndex].FieldType == FieldType.EMPTY)
                        {
                            Fields[currentIndex].FieldType = FieldType.CABLE;
                            Fields[currentIndex].ZoneType = ZoneType.NOTHING;
                            if (CONSTS.containesPosition(x, y))
                            {
                                _model.buildCable(_model.GameGrid[x, y]);
                            }
                        }
                        break;
                    case "CableDelete":
                        if (Fields[currentIndex].FieldType == FieldType.CABLE)
                        {
                            Fields[currentIndex].FieldType = FieldType.EMPTY;
                            Fields[currentIndex].ZoneType = ZoneType.NOTHING;
                            _model.DeleteCable(_model.GameGrid[x, y]);
                        }
                        break;
                    case "Police":                        
                        serviceSwitch(_model.Selected, x, y);
                        break;
                    case "Stadium":
                        serviceSwitch(_model.Selected, x, y);
                        break;
                    case "School":
                        serviceSwitch(_model.Selected, x, y);
                        break;
                    case "University":
                        serviceSwitch(_model.Selected, x, y);
                        break;
                    case "PowerPlant":
                        serviceSwitch(_model.Selected, x, y);
                        break;
                    case "ServiceDelete":
                        Fields[currentIndex].ZoneType = ZoneType.NOTHING;
                        _model.deleteServiceBuilding(_model.GameGrid[x, y]);
                        Fields[currentIndex].ZoneIndex = -1;
                        break;
                    case "ForestBuild":
                        _model.BuildForest(_model.GameGrid[x, y]);
                        break;
                    case "ForestDelete":
                        _model.DeleteForest(_model.GameGrid[x, y]);
                        break;
                    case "GetZoneData":
                        ListZoneDetails(currentIndex, x, y);
                        break;
                    default:
                        break;
                }
            }
        }
        private void zoneSwitch(String zoneType, int fieldIndex, int x, int y)
        {
            if (Fields[fieldIndex].FieldType == FieldType.EMPTY)
            {
                Fields[fieldIndex].FieldType = FieldType.ZONE;
                ZoneType type;
                switch (zoneType)
                {
                    case "Residential":
                        type = ZoneType.RESIDENTIAL;
                        break;
                    case "Commercial":
                        type = ZoneType.COMMERCIAL;
                        break;
                    case "Industrial":
                        type = ZoneType.INDUSTRIAL;
                        break;
                    default:
                        throw new Exception();
                }
                Fields[fieldIndex].ZoneType = type;
                if (CONSTS.containesPosition(x, y)) { 
                _model.SelectZone(_model.GameGrid[x, y], type);
                }
            }
        }
        private void serviceSwitch(String serviceType, int x, int y)
        {
            String name;
            switch (serviceType)
            {
                case "Police":
                    name = "Police";
                    break;
                case "Stadium":
                    name = "Stadium";
                    break;
                case "School":
                    name = "School";
                    break;
                case "University":
                    name = "University";
                    break;
                case "PowerPlant":
                    name = "PowerPlant";
                    break;
                default:
                    throw new Exception();
            }
            _model.buildServiceBuilding(name, _model.GameGrid[x, y]);

        }
        private void GenerateFields()
        {
            while (progressStatus == 1) ;
            progressStatus = 2;
            Fields.Clear();
            for (int j = _startY; j <  _startY + VisibleGridHeight; j++)
            {
                for (int i = _startX; i <  _startX + VisibleGridWidth; i++)
                {

                    if (progressStatus == 1)
                    {
                        progressStatus = 3;
                        return;
                    }
                    Fields.Add(new SimCityField
                    {
                        Text = String.Empty,
                        X = i,
                        Y = j,
                        Index = (j - _startY) * VisibleGridWidth + (i - _startX),
                        FieldType = FieldType.EMPTY,
                        ZoneType = ZoneType.NOTHING,
                        ZoneIndex = -1,
                        IsCatastrophe = false,
                        FieldClickedCommand = new DelegateCommand(commandParameter => FieldClick(Convert.ToInt32(commandParameter)))
                    });
                }
            }

            progressStatus = 0;

            RefreshGrid();
        }

        private void ListZoneDetails(int currentIndex, int x, int y)
        {
            if (Fields[currentIndex].ZoneType == ZoneType.NOTHING)
            {
                ZoneDataVisibility = Visibility.Hidden;
                return;
            }

            ZoneDataVisibility = Visibility.Visible;
            Zone? CurrentZone = _model.GameGrid.GetZoneAt(_model.GameGrid[x, y]);
            ListedZoneCapacity = "Capacity: " + CurrentZone?.Capacity;
            ListedZoneCitizenCount = "Population: " + CurrentZone?.GetCitizenSize;
            ListedZoneSatisfaction = "Satisfaction: " + CurrentZone?.Satisfaction;
            ListedZoneCapacityLevel = "Level: " + CurrentZone?.Level;
            ListedZoneHasElectricity = "Electricity: " + CurrentZone?.Field.HasElectricity;
            ListedZoneIndex = currentIndex;
            ListedZoneX = x;
            ListedZoneY = y;
        }
        #endregion

        #region Game event handlers
        private void Model_GameCreated(object? sender, SimCityEventArgs e)
        {
            RefreshGrid();
        }

        private void Model_ZonesChanged(object? sender, SimCityBuildEventArgs e)
        {
            //RefreshGrid();
            int x = e.ChangeX;
            int y = e.ChangeY;
            if (y - _startY >= 0 && x -_startX >= 0 && y - _startY < VisibleGridHeight && x - _startX < VisibleGridWidth)
            {
                int fieldIndex = (y - _startY) * VisibleGridWidth + (x - _startX);
                RefreshField(fieldIndex);
            }
        }
        private void Model_RoadsChanged(object? sender, SimCityBuildEventArgs e)
        {
            //RefreshGrid();
            int x = e.ChangeX;
            int y = e.ChangeY;
            if (y - _startY >= 0 && x - _startX >= 0 && y - _startY < VisibleGridHeight && x - _startX < VisibleGridWidth)
            {
                int fieldIndex = (y - _startY) * VisibleGridWidth + (x - _startX);
                RefreshField(fieldIndex);
            }
        }
        private void Model_ServiceBuildingsChanged(object? sender, SimCityBuildEventArgs e)
        {
            //RefreshGrid();
            int x = e.ChangeX;
            int y = e.ChangeY;
            if (y - _startY >= 0 && x - _startX >= 0 && y - _startY < VisibleGridHeight && x - _startX < VisibleGridWidth)
            {
                int fieldIndex = (y - _startY) * VisibleGridWidth + (x - _startX);
                RefreshField(fieldIndex);
            }
        }
        private void Model_ForestsChanged(object? sender, SimCityBuildEventArgs e)
        {
            int x = e.ChangeX;
            int y = e.ChangeY;
            if (y - _startY >= 0 && x - _startX >= 0 && y - _startY < VisibleGridHeight && x - _startX < VisibleGridWidth)
            {
                int fieldIndex = (y - _startY) * VisibleGridWidth + (x - _startX);
                RefreshField(fieldIndex);
            }
        }
        private void Model_NewCitizen(object? sender, SimCityEventArgs e)
        {

        }

        private void Model_CatasrophyHappened(object? sender, SimCityCatastropheEventArgs e)
        {
            DisplayCatasthrophe = e.IsCatastrophe;
            if(DisplayCatasthrophe)
            {
                CatastropheMessage = "Catastrophe happened that killed " + e.NumberOfDeaths + " citizens!";
                CatastropheMessageVisibility = Visibility.Visible;
                int x = e.X;
                int y = e.Y;
                if (y - _startY >= 0 && x - _startX >= 0 && y - _startY < VisibleGridHeight && x - _startX < VisibleGridWidth)
                {
                    int fieldIndex = (y - _startY) * VisibleGridWidth + (x - _startX);
                    Fields[fieldIndex].IsCatastrophe = true;
                }
                _previousCatastrophe = (x, y);
            }
            else
            {
                int x = _previousCatastrophe.Item1;
                int y = _previousCatastrophe.Item2;
                if ((x != -1 || y != -1) && y - _startY >= 0 && x - _startX >= 0 && y - _startY < VisibleGridHeight && x - _startX < VisibleGridWidth)
                {
                    int fieldIndex = (y - _startY) * VisibleGridWidth + (x - _startX);
                    Fields[fieldIndex].IsCatastrophe = false;
                }
                _previousCatastrophe = (-1, -1);
                CatastropheMessageVisibility = Visibility.Hidden;
            }
        }
        private void Model_GameAdvanced(object? sender, SimCityEventArgs e)
        {
            OnPropertyChanged(nameof(GameTime));
            //test
            OnPropertyChanged(nameof(Population));
            OnPropertyChanged(nameof(Funds));
            OnPropertyChanged(nameof(Tax));
            OnPropertyChanged(nameof(Satisfaction));

        }
        #endregion

        #region Event methods
        private void OnNewGame()
        {
            NewGame?.Invoke(this, EventArgs.Empty);
        }
      
        private void OnExitGame()
        {
            ExitGame?.Invoke(this, EventArgs.Empty);
        }
        private void OnGameSpeedChanged()
        {
            GameSpeedChanged?.Invoke(this, EventArgs.Empty);
        }
        private void OnGameStartedPaused()
        {
            GameStartedPaused?.Invoke(this, EventArgs.Empty);
        }
        private void OnMoveMapLeft()
        {
            if (_startX > 0)
            {
                _startX--;
                if (progressStatus == 2)
                {
                    progressStatus = 1;
                }
                GenerateFields();
            }
        }
        private void OnMoveMapRight()
        {
            if(_startX < GridWidth - VisibleGridWidth)
            {
                _startX++;
                if (progressStatus == 2)
                {
                    progressStatus = 1;
                }
                GenerateFields();
            }
        }
        private void OnMoveMapUp()
        {
            if (_startY > 0)
            {
                _startY--;
                if (progressStatus == 2)
                {
                    progressStatus = 1;
                }
                GenerateFields();
            }
        }
        private void OnMoveMapDown()
        {
            if (_startY < GridHeight - VisibleGridHeight)
            {
                _startY++;
                if (progressStatus == 2)
                {
                    progressStatus = 1;
                }
                GenerateFields();
            }
        }

        private void IncreaseSelectedZoneLevel()
        {
            _model.DevelopeZone(_model.GameGrid[ListedZoneX, ListedZoneY]);
            ListZoneDetails(ListedZoneIndex, ListedZoneX, ListedZoneY);
        }

        private void SpeedPropertyChanged()
        {
            OnPropertyChanged(nameof(IsGameSpeedSlow));
            OnPropertyChanged(nameof(IsGameSpeedNormal));
            OnPropertyChanged(nameof(IsGameSpeedFast));
        }
        private void PropertyChange()
        {
            OnPropertyChanged(nameof(IsResidentialZoneSelected));
            OnPropertyChanged(nameof(IsCommercialZoneSelected));
            OnPropertyChanged(nameof(IsIndustrialZoneSelected));
            OnPropertyChanged(nameof(IsDeleteZoneSelected));
            OnPropertyChanged(nameof(IsRoadBuildSelected));
            OnPropertyChanged(nameof(IsRoadDeleteSelected));
            OnPropertyChanged(nameof(IsCableBuildSelected));
            OnPropertyChanged(nameof(IsCableDeleteSelected));
            OnPropertyChanged(nameof(IsPoliceSelected));
            OnPropertyChanged(nameof(IsStadiumSelected));
            OnPropertyChanged(nameof(IsSchoolSelected));
            OnPropertyChanged(nameof(IsUniversitySelected));
            OnPropertyChanged(nameof(IsPowerPlantSelected));
            OnPropertyChanged(nameof(IsServiceDeleteSelected));
            OnPropertyChanged(nameof(IsForestBuildSelected));
            OnPropertyChanged(nameof(IsForestDeleteSelected));
            OnPropertyChanged(nameof(IsGetZoneDataSelected));
            ZoneDataVisibility = Visibility.Hidden;
        }
        private void OnZoomIn()
        {
            if (VisibleGridWidth == CONSTS.VisibleGridWidthSmall && VisibleGridHeight == CONSTS.VisibleGridHeightSmall)
            {
                return;
            }
            else if (VisibleGridWidth == CONSTS.VisibleGridWidthNormal && VisibleGridHeight == CONSTS.VisibleGridHeightNormal)
            {
                VisibleGridWidth = CONSTS.VisibleGridWidthSmall;
                VisibleGridHeight = CONSTS.VisibleGridHeightSmall;
            }
            else if(VisibleGridWidth == CONSTS.VisibleGridWidthMedium && VisibleGridHeight == CONSTS.VisibleGridHeightMedium){
                VisibleGridWidth = CONSTS.VisibleGridWidthNormal;
                VisibleGridHeight = CONSTS.VisibleGridHeightNormal;
            }
            else if(VisibleGridWidth == CONSTS.GridWidth && VisibleGridHeight == CONSTS.GridHeight)
            {
                VisibleGridWidth = CONSTS.VisibleGridWidthMedium;
                VisibleGridHeight = CONSTS.VisibleGridHeightMedium;
            }
            GenerateFields();
        }
        private void OnZoomOut()
        {
            if (VisibleGridWidth == CONSTS.VisibleGridWidthSmall && VisibleGridHeight == CONSTS.VisibleGridHeightSmall)
            {
                VisibleGridWidth = CONSTS.VisibleGridWidthNormal;
                VisibleGridHeight = CONSTS.VisibleGridHeightNormal;
            }
            else if (VisibleGridWidth == CONSTS.VisibleGridWidthNormal && VisibleGridHeight == CONSTS.VisibleGridHeightNormal)
            {
                VisibleGridWidth = CONSTS.VisibleGridWidthMedium;
                VisibleGridHeight = CONSTS.VisibleGridHeightMedium;
            }
            else if (VisibleGridWidth == CONSTS.VisibleGridWidthMedium && VisibleGridHeight == CONSTS.VisibleGridHeightMedium)
            {
                VisibleGridWidth = CONSTS.GridWidth;
                VisibleGridHeight = CONSTS.GridHeight;
            }
            else if (VisibleGridWidth == CONSTS.GridWidth && VisibleGridHeight == CONSTS.GridHeight)
            {
                return;
            }
            _startX = 0;
            _startY = 0;
            GenerateFields();
        }
        #endregion
    }
}
